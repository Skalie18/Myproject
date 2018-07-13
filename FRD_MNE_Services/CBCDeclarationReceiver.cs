using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using IBM.XMS;
using Sars.Systems.Data;

namespace FRD_MNE_Services
{
    public class CbcDeclarationReceiver
    {
        private XMSFactoryFactory           _xffReadFactory;
        private IConnectionFactory          _connectionFactory;
        private IConnection                 _mqConnection;
        private ISession                    _mqSession;
        private IDestination                _mqDestination;
        private IMessageConsumer            _mqMessageConsumer;

        public void ListenForCbcRequests(){
            try{
                _xffReadFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
                _connectionFactory = _xffReadFactory.CreateConnectionFactory();
                _connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, CBCApplicationConfigurationdetails.RequestConnection);
                _connectionFactory.SetIntProperty(XMSC.WMQ_PORT, CBCApplicationConfigurationdetails.RequestPort);
                _connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, CBCApplicationConfigurationdetails.RequestChannel);
                _connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
                _connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, CBCApplicationConfigurationdetails.RequestManger);
                _connectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, XMSC.WMQ_BROKER_V1);
                _connectionFactory.SetStringProperty(XMSC.WMQ_PROVIDER_VERSION,MQConfigurationSettings.WMQ_PROVIDER_VERSION);
                _mqConnection = _connectionFactory.CreateConnection();
                _mqSession = _mqConnection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
                _mqDestination = _mqSession.CreateQueue(string.Format("queue://{0}/{1}", CBCApplicationConfigurationdetails.RequestManger, CBCApplicationConfigurationdetails.RequestQueueName));
                _mqMessageConsumer = _mqSession.CreateConsumer(_mqDestination);
                MessageListener ml = OnMessage;
                _mqMessageConsumer.MessageListener = ml;
                _mqConnection.Start();
                //Thread.Sleep(100000);
            }
            catch (Exception exception){
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }

        private static void OnMessage(IMessage msg)
        {
            //var corId = FdrCommon.ByteArrayToString(msg.JMSCorrelationIDAsBytes);
            var message = string.Empty;
            try{
                var textMsg = (ITextMessage) msg;
                message = textMsg.Text;
                try{
                    message = FdrCommon.FormatXml(message);
                }
                catch (Exception){
                    ;
                }
                var dataValidation = IsCbcValid(message);
                var header = FdrCommon.SelectNode(message, "soap12", "http://www.w3.org/2003/05/soap-envelope", "Header");
                if (header != null){
                    var messageToSend = dataValidation.IsValid ? CreateXml(header.OuterXml) : FdrCommon.CreateFaultXml(header.OuterXml, dataValidation.ErrorXml);
                    var queueInfo = new QueueInfo{
                        Manager = CBCApplicationConfigurationdetails.ResponseManger,
                        Channel = CBCApplicationConfigurationdetails.ResponseChannel,
                        UseManagerName = false,
                        Port = CBCApplicationConfigurationdetails.ResponsePort,
                        HostName = CBCApplicationConfigurationdetails.ResponseHostName,
                        QueueName = CBCApplicationConfigurationdetails.ResponseQueueName,
                        CorrelationId = msg.JMSMessageID,
                        Message = messageToSend
                    };
                    FdrMessaging.SendMessageToQueue(queueInfo);
                    if (!dataValidation.IsValid){
                        return;
                    }
                }
            }
            catch (Exception exception){
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
            try{
                if (!string.IsNullOrEmpty(message)){
                    SaveIncomingCbcDeclaration(message);
                }
            }
            catch (Exception exception){
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }
        private static void SaveIncomingCbcDeclaration(string message)
        {
            var properXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(message);
            using (var data = new RecordSet()){
                data.ReadXml(new StringReader(properXml));
                var messageIdentification = data.Tables["MessageIdentification"];
                var messageSpec = data.Tables["MessageSpec"];
                var contactDetails = data.Tables["ContactDetails"];

                if (!data.Tables.Contains("MessageIdentification") || !data.Tables.Contains("MessageSpec") ||
                    !data.Tables.Contains("ContactDetails")){
                    FdrCommon.LogEvent("MessageIdentification OR  FormInfo OR ContactDetails is not in this XML");
                    return ;
                }
                if (messageIdentification.Rows.Count == 0){
                    FdrCommon.LogEvent("MessageIdentification has no data");
                    return ;
                }

                if (messageSpec.Rows.Count == 0){
                    FdrCommon.LogEvent("FormInfo has no data");
                    return;
                }

                if (contactDetails.Rows.Count == 0){
                    FdrCommon.LogEvent("ContactDetails has no data");
                    return ;
                }
                var reportingYear = messageSpec.Rows[0]["ReportingPeriod"].ToString().Trim();

                var taxRefNo = messageIdentification.Rows[0]["externalReferenceID"].ToString().Trim();
                var taxYear = Convert.ToInt32(reportingYear.Length > 4 ? reportingYear.Substring(0, 4) : reportingYear);
                var surname = contactDetails.Rows[0]["Surname"].ToString().Trim();
                var firstNames = contactDetails.Rows[0]["FirstNames"].ToString().Trim();
                var busTelNo1 = contactDetails.Rows[0]["BusTelNo1"].ToString().Trim();
                var cellNo = contactDetails.Rows[0]["CellNo"].ToString().Trim();
                var emailAddress = contactDetails.Rows[0]["EmailAddress"].ToString().Trim();
                //string postalAddress = null;
                DbReader.SaveCBCRequest(properXml,
                    taxRefNo,
                    taxYear,
                    surname,
                    firstNames,
                    busTelNo1,
                    cellNo,
                    emailAddress,
                    null);

                var roles = DbReader.GetRolesToNotify();
                if (roles.HasRows)
                {
                    var x = (from s in roles.Tables[0].Rows.OfType<DataRow>()
                        select s["EmailAddress"].ToString()).ToArray();

                    var messageBody = string.Format(
                        File.ReadAllText("CBCArrivalNotification.htm")
                        , taxRefNo
                        , reportingYear
                        , SARSDataSettings.Settings.ApplicationName);
                    FdrCommon.SendEmail(x, messageBody, "CBC Declaration");
                }
            }
        }
        private static string CreateXml(string header) 
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
                "</soap12:Body>" +
                "</soap12:Envelope>",
                header,
                applicationInformationStructureXml
                );
            return FdrCommon.FormatXml(xmlBuilder.ToString());
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
            if (_mqSession != null)
            {
                _mqSession.Close();
                _mqSession.Dispose();
            }
            _connectionFactory = null;
            _xffReadFactory = null;
        }
        private static DataValidations IsCbcValid(string message)
        {
            var submitCountryByCountryDeclarationRequest = FdrCommon.SelectNode(message, "tnsf",
                "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/SubmitCountryByCountryDeclaration/xml/schemas/version/1.2",
                "SubmitCountryByCountryDeclarationRequest");

            var validations = new DataValidations(submitCountryByCountryDeclarationRequest)
            {
                SchemaList = new List<InternalSchemaData>()
            };

            var cbcSchema = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "CbcXML.xsd"));
            var isocbctypes = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "isocbctypes.xsd"));
            var declaration =
                File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "SARSCountryByCountryDeclaration.xsd"));
            var oecdtypes = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "oecdtypes.xsd"));
            var sarsgmdBase = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "SARSGMD_BaseTypes.xsd"));
            var sarsgmdFormtypes = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "CBC", "SARSGMD_FormTypes.xsd"));

            validations.SchemaList.AddRange(
                new List<InternalSchemaData>
                {
                    new InternalSchemaData {TargetNamespace = "urn:oecd:ties:cbc:v1", SchemaContent = cbcSchema},
                    new InternalSchemaData {TargetNamespace = "urn:oecd:ties:isocbctypes:v1", SchemaContent = isocbctypes},
                    new InternalSchemaData
                    {
                        TargetNamespace =
                            "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/CountryByCountryDeclaration/xml/schemas/version/1.2",
                        SchemaContent = declaration
                    },
                    new InternalSchemaData {TargetNamespace = "urn:oecd:ties:stf:v4", SchemaContent = oecdtypes},
                    new InternalSchemaData
                    {
                        TargetNamespace =
                            "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/55.2",
                        SchemaContent = sarsgmdBase
                    },
                    new InternalSchemaData
                    {
                        TargetNamespace =
                            "http://www.sars.gov.za/enterpriseMessagingModel/GMD/FormTypes/xml/schemas/version/1.6",
                        SchemaContent = sarsgmdFormtypes
                    }
                }
                );

            validations.ValidateSchema();
            return validations;
        }
    }
}
