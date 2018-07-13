using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using IBM.XMS;
using Sars.ThirdParty.DataActivityManagement;

namespace FRD_MNE_Services
{
    public class MneRequestProvider
    {
        private XMSFactoryFactory       _xffReadFactory;
        private IConnectionFactory      _connectionFactory;
        private IConnection             _mqConnection;
        private ISession                _newSesstion;
        private IDestination            _mqDestination;
        private IMessageConsumer        _mqMessageConsumer;
        public void ListenForMneRequests()
        {
            _xffReadFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            _connectionFactory = _xffReadFactory.CreateConnectionFactory();
            _connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, MNEApplicationConfigurationdetails.RequestConnection);
            _connectionFactory.SetIntProperty(XMSC.WMQ_PORT, MNEApplicationConfigurationdetails.RequestPort);
            _connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, MNEApplicationConfigurationdetails.RequestChannel);
            _connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
            _connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, MNEApplicationConfigurationdetails.RequestManger);
            _connectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, XMSC.WMQ_BROKER_V1);
            _connectionFactory.SetStringProperty(XMSC.WMQ_PROVIDER_VERSION, MQConfigurationSettings.WMQ_PROVIDER_VERSION);

            _mqConnection = _connectionFactory.CreateConnection();
            _newSesstion = _mqConnection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            _mqDestination =_newSesstion.CreateQueue(string.Format("queue://{0}/{1}",MNEApplicationConfigurationdetails.RequestManger, MNEApplicationConfigurationdetails.RequestQueueName));

            _mqMessageConsumer = _newSesstion.CreateConsumer(_mqDestination);
            MessageListener ml = OnMessageReceived;
            _mqMessageConsumer.MessageListener = ml;
            _mqConnection.Start();

             //Thread.Sleep(1000000);
          }
        public void TerminateConnections()
        {
            if (_mqMessageConsumer != null)
            {
                try
                {
                    _mqMessageConsumer.Close();
                    _mqMessageConsumer.Dispose();
                }
                catch (Exception exception)
                {
                    FdrCommon.LogEvent(exception, EventLogEntryType.Error);
                }
            }
            if (_mqDestination != null)
            {
                _mqDestination.Dispose();
                _mqDestination = null;
            }
            if (_mqConnection != null)
            {
                _mqConnection.Close();
                _mqConnection.Dispose();
            }
            if (_newSesstion != null)
            {
                _newSesstion.Close();
                _newSesstion.Dispose();
            }
        }
        private static string CreateXml(string header, string message)
        {
            var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructure
            {
                ApplicationInformationResult = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult[1]
            };
            applicationInformationStructure.ApplicationInformationResult[0] =
                new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult
                {
                    Code = "0000",
                    Description = "Processed",
                    MessageType = Sars.ESBSchema.ApplicationInformation.v31.MessageTypeEnum.INFORMATION
                };

            var applicationInformationStructureXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(applicationInformationStructure, "fdri", "http://www.egovernment.gov.za/GMD/ApplicationInformation/xml/schemas/version/3.1");

            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append(
                "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> " +
                "<soap12:Envelope xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">"

                );
            xmlBuilder.AppendFormat(
                "{0}" +
                " <soap12:Body>" +
                "{1}" +
                "{2}" +
                "</soap12:Body>" +
                "</soap12:Envelope>",
                header,
                applicationInformationStructureXml,
                message
                );
            return FdrCommon.FormatXml(xmlBuilder.ToString());
        }
        private static void OnMessageReceived(IMessage msg)
        {
            var textMsg = (ITextMessage)msg;
        
            var taxRefNo = "";
            var period = ""; 
            var year = "";
            try{
                var message = textMsg.Text;
                var doc = new XmlDocument();
                doc.LoadXml(message);
                var ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
                ns.AddNamespace("pnsb", "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/ThirdPartyDataActivityManagement/xml/schemas/version/1.1");
                var header = doc.SelectSingleNode("//soap12:Header", ns);
                var thirdPartyDataActivityManagementRequest = doc.SelectSingleNode("//pnsb:ThirdPartyDataActivityManagementRequest", ns);
                if (thirdPartyDataActivityManagementRequest != null && header!= null)
                {
                    var dataValidation = IsMneRequestValid(thirdPartyDataActivityManagementRequest.OuterXml);
                    if (!dataValidation.IsValid)
                    {
                        var faultXml = FdrCommon.CreateFaultXml(header.OuterXml, dataValidation.ErrorXml);
                        RespondToMneEnquire(faultXml, textMsg.JMSMessageID);
                        return;
                    }
                    foreach (XmlNode childNode in thirdPartyDataActivityManagementRequest.ChildNodes)
                    {
                        if (childNode.Name.Equals("pnsb:TaxRefNo"))
                        {
                            taxRefNo = childNode.InnerText;
                            continue;
                        }
                        if (childNode.Name.Equals("pnsb:Period"))
                        {
                            period = childNode.InnerText;
                            continue;
                        }
                        if (childNode.Name.Equals("pnsb:Year"))
                        {
                            year = childNode.InnerText;
                        }
                    }
                }
                else
                {
                    return;
                }
                var enquireResult = DbReader.Enquire(taxRefNo, year);
                var correctCbCDeclarationInd = DbReader.CanSubmitCbcDeclaration(taxRefNo, Convert.ToInt32(year)); //row["CorrectCbCDeclarationInd"].ToString();
                var correctMasterAndLocalFileInd = DbReader.CanSubmitFileDeclaration(taxRefNo, Convert.ToInt32(year)); //row["CorrectMasterAndLocalFileInd"].ToString();

                if (enquireResult.HasRows)
                {
                    var row = enquireResult[0];
                    var submitCbCDeclarationInd = row["CbCReportRequiredInd"].ToString();
                    var submitMasterAndLocalFileInd = row["MasterLocalFileRequiredInd"].ToString();

                    var response = new ThirdPartyDataActivityManagementResponse
                    {
                        CBC = new ThirdPartyDataActivityManagementResponseCBC
                        {
                            CorrectCbCDeclarationInd = correctCbCDeclarationInd ? YesNoIndType.Y : YesNoIndType.N,
                            CorrectMasterAndLocalFileInd =
                                correctMasterAndLocalFileInd ? YesNoIndType.Y : YesNoIndType.N,
                            SubmitCbCDeclarationInd =
                                submitCbCDeclarationInd.Equals("Y") ? YesNoIndType.Y : YesNoIndType.N,
                            SubmitMasterAndLocalFileInd =
                                submitMasterAndLocalFileInd.Equals("Y") ? YesNoIndType.Y : YesNoIndType.N
                        }
                    };
                    var xml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(response);

                    var messageToSend = CreateXml(header.OuterXml, xml);
                    RespondToMneEnquire(messageToSend, textMsg.JMSMessageID);

                }
                else
                {
                    var response = new ThirdPartyDataActivityManagementResponse
                    {
                        CBC = new ThirdPartyDataActivityManagementResponseCBC
                        {

                            CorrectCbCDeclarationInd = correctCbCDeclarationInd ? YesNoIndType.Y : YesNoIndType.N,
                            CorrectMasterAndLocalFileInd =
                                correctMasterAndLocalFileInd ? YesNoIndType.Y : YesNoIndType.N,
                            SubmitCbCDeclarationInd = YesNoIndType.Y,
                            SubmitMasterAndLocalFileInd = YesNoIndType.Y
                        }
                    };
                    var xml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(response);
                    var messageToSend = CreateXml(header.OuterXml, xml);
                    RespondToMneEnquire(messageToSend, textMsg.JMSMessageID);

                }
                try
                {
                    var properXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(textMsg.Text);
                    DbWriter.SaveMneRequest(msg.JMSMessageID, properXml, taxRefNo, year);
                }
                catch (Exception exception)
                {
                    FdrCommon.LogEvent(exception, EventLogEntryType.Error);
                }
            }
            catch (Exception exception)
            {
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }
        private static void NotifyFdrUsers(string taxRefNo, string year)
        {

            var body =
                string.Format(
                    "Good day, </br> this is to inform you about new file submission that just arrived for {0} for year {1}",
                    taxRefNo, year);
            const string subject = "Files arriving";
            var users = DbReader.GetUsersInARole(AppConfig.NotifyRole);
            var emailAddresses = users.Select(u => u.EmailAddress).ToArray();
            FdrCommon.SendEmail(emailAddresses, body, subject);
        }
        private static void RespondToMneEnquire(string qMessage, string correlationId)
        {
            var queueInfo = new QueueInfo
            {
                Manager = MNEApplicationConfigurationdetails.ResponseManger,
                Channel = MNEApplicationConfigurationdetails.ResponseChannel,
                UseManagerName = false,
                Port = MNEApplicationConfigurationdetails.ResponsePort,
                HostName = MNEApplicationConfigurationdetails.ResponseHostName,
                QueueName = MNEApplicationConfigurationdetails.ResponseQueueName,
                CorrelationId = correlationId,
                Message = qMessage
            };
            FdrMessaging.SendMessageToQueue(queueInfo);

            try
            {
                var message = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(qMessage);
                DbWriter.SaveMneResponse(correlationId, message);
            }
            catch (Exception)
            {
                ;
            }
        }
        private static DataValidations IsMneRequestValid(string message)
        {
            var thirdPartyDataActivityManagementRequest = FdrCommon.SelectNode(message, "pnsb", "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/ThirdPartyDataActivityManagement/xml/schemas/version/1.1",
                "ThirdPartyDataActivityManagementRequest");

            var validations = new DataValidations(thirdPartyDataActivityManagementRequest)
            {
                SchemaList = new List<InternalSchemaData>()
            };

            var sarsgmdBasetypes = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "MNE", "SARSGMD_BaseTypes.xsd"));
            var sarsThirdPartyDataActivity = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "MNE", "SARSThirdPartyDataActivityManagement.xsd"));
            var sarsthirdpartysubmissionheader = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "MNE", "SARSThirdPartySubmissionHeader.xsd"));

            validations.SchemaList.AddRange(
                new List<InternalSchemaData>
                {
                    new InternalSchemaData {TargetNamespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/55.2", SchemaContent = sarsgmdBasetypes},
                    new InternalSchemaData {TargetNamespace = "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/ThirdPartyDataActivityManagement/xml/schemas/version/1.1", SchemaContent = sarsThirdPartyDataActivity},
                    new InternalSchemaData {TargetNamespace ="http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartySubmissionHeader/xml/schemas/version/1.17", SchemaContent = sarsthirdpartysubmissionheader}
                }
            );
            validations.ValidateSchema();
            return validations;
        }
    }
}
