using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using FDR.DataLayer;
using IBM.XMS;
using Sars.ContentManagement;
using Sars.Systems.Data;

namespace FRD_MNE_Services 
{
    public class MasterLocalFileProvider
    {
        private XMSFactoryFactory       _xffReadFactory;
        private IConnectionFactory      _connectionFactory;
        private IConnection             _mqConnection;
        private ISession                _newSesstion;
        private IDestination            _mqDestination;
        private IMessageConsumer        _mqMessageConsumer;

        public void ListenMasterLocalFilesRequests()
        {
            _xffReadFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            _connectionFactory = _xffReadFactory.CreateConnectionFactory();
            _connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, LMApplicationConfigurationdetails.RequestConnection);
            _connectionFactory.SetIntProperty(XMSC.WMQ_PORT, LMApplicationConfigurationdetails.RequestPort);
            _connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, LMApplicationConfigurationdetails.RequestChannel);
            _connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
            _connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, LMApplicationConfigurationdetails.RequestManger);
            _connectionFactory.SetIntProperty(XMSC.WMQ_BROKER_VERSION, XMSC.WMQ_BROKER_V1);
            _connectionFactory.SetStringProperty(XMSC.WMQ_PROVIDER_VERSION, MQConfigurationSettings.WMQ_PROVIDER_VERSION);
            _mqConnection = _connectionFactory.CreateConnection();
            _newSesstion = _mqConnection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            _mqDestination =
                _newSesstion.CreateQueue(string.Format("queue://{0}/{1}",
                    LMApplicationConfigurationdetails.RequestManger, LMApplicationConfigurationdetails.RequestQueueName));
            _mqMessageConsumer = _newSesstion.CreateConsumer(_mqDestination);
            MessageListener ml = OnFilesReceived;
            _mqMessageConsumer.MessageListener = ml;
            _mqConnection.Start();
           //Thread.Sleep(int.MaxValue);
        }

        public void TerminateConnections()
        {
            try
            {

                if (_mqMessageConsumer != null)
                {
                    _mqMessageConsumer.Close();
                    _mqMessageConsumer.Dispose();
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
            catch (Exception exception)
            {
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
           
        }
        private static string CreateXml(string header, string message)
        {

            var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformation
            {
                applicationInformationResult =
                    new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformationApplicationInformationResult[1]
            };

            applicationInformationStructure.applicationInformationResult[0] =

                new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformationApplicationInformationResult
                {
                    MessageType = Sars.ESBSchema.ApplicationInformation.v000002.MessageTypeEnum.Information,
                    applicationInformationCode = "0000",
                    applicationInformationDescription = "Processed",
                    applicationInformationState = "OK"
                };

            var applicationInformationStructureXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(applicationInformationStructure, "FDR", "http://www.sars.gov.za/services/esb/v000002/applicationInformation");

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
        private static string CreateFaultXml(string header, string message)
        {

            var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformation
            {
                applicationInformationResult =
                    new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformationApplicationInformationResult[1]
            };

            applicationInformationStructure.applicationInformationResult[0] =

                new Sars.ESBSchema.ApplicationInformation.v000002.applicationInformationApplicationInformationResult
                {
                    MessageType = Sars.ESBSchema.ApplicationInformation.v000002.MessageTypeEnum.Error,
                    applicationInformationCode = "9999",
                    applicationInformationDescription = message,
                    applicationInformationState = "ERROR"
                };

            var applicationInformationStructureXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(applicationInformationStructure, "FDR", "http://www.sars.gov.za/services/esb/v000002/applicationInformation");

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
        private static void OnFilesReceived(IMessage msg)
        {
            try
            {
                var textMsg = (ITextMessage) msg;
                FdrCommon.SaveMessage(textMsg.Text, @"\\ptadviis06\TaxDirectives\FDR\FILES\IN");
                var message = textMsg.Text;

               

                var doc = new XmlDocument();
                doc.LoadXml(message);
                var ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
                ns.AddNamespace("ContMgt", "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8");

                var header = doc.SelectSingleNode("//soap12:Header", ns);
                var request = doc.SelectSingleNode("//ContMgt:ContentManagementRequest", ns);
                if (header == null || request == null)
                {
                    FdrCommon.LogEvent("header == null || request == null", EventLogEntryType.Warning);
                    return;
                }

                var dataValidation = IsMasterLocalFileRequestValid(message);
                if (!dataValidation.IsValid)
                {
                    var xml = FdrCommon.FormatXml(CreateFaultXml(header.OuterXml, dataValidation.ErrorXml));
                    RespondToDocumentNotify(xml, msg.JMSMessageID);
                    return;
                }
               

                var supportingDocs =
                Sars.Systems.Serialization.XmlObjectSerializer.ConvertXmlToObject<SupportingDocumentManagementRequestStructure>(request.OuterXml);
                var xmlToSend = string.Empty;
                var deserialized = supportingDocs != null && supportingDocs.Contents != null &&
                                   supportingDocs.Contents.Any();
                if (deserialized)
                {
                    var responseStructure = new SupportingDocumentManagementResponseStructure
                    {
                        Contents = supportingDocs.Contents
                    };
                    xmlToSend = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(responseStructure, "ContMgt", "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8");
                }
                if (!string.IsNullOrEmpty(xmlToSend))
                {
                    var messageToSend = CreateXml(header.OuterXml, xmlToSend);
                    messageToSend = FdrCommon.RemoveEmptyElements(messageToSend);
                    RespondToDocumentNotify(messageToSend, msg.JMSMessageID);
                }
                if (deserialized)
                {
                    if (!supportingDocs.Contents[0].ContentLocation.ToUpper().EndsWith(AppConfig.SecondaryFileLocation))
                    {
                       var submissionId = ContentManagementRequest(message);
                        if (submissionId > 0)
                        {
                            SendNotification(submissionId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                FdrCommon.LogEvent(exception, EventLogEntryType.Error);
            }
        }

        private static void SendNotification(decimal submissionId)
        {
            var submission = DBReadManager.GeFileSubmissionById(submissionId);
            if (submission == null) return;
            var files = DBReadManager.GetFilesPerSubmission(submissionId.ToString(CultureInfo.InvariantCulture));
            var roles = DbReader.GetRolesToNotify();
            if (roles.HasRows)
            {
                var x = (from s in roles.Tables[0].Rows.OfType<DataRow>()
                         select s["EmailAddress"].ToString()).ToArray();

                var fileList = new StringBuilder();
                //fileList.Append("<ul>");
                foreach (DataRow file in files.Tables[0].Rows)
                {
                    fileList.AppendFormat("<li>{0} - {1}</li>", file["Category"], file["Classification"]);
                }

                //fileList.Append("</ul>");

                var messageBody = string.Format(
                    File.ReadAllText("FileArrivalNotification.htm")
                    , submission.TaxRefNo
                    , submission.Year
                    , fileList
                    , SARSDataSettings.Settings.ApplicationName);
                FdrCommon.SendEmail(x, messageBody, "CBC Declaration");
            }
        }

        private static decimal ContentManagementRequest(string xmlMessage)
        {
            decimal submissionId = 0;
            var doc = new XmlDocument();
            doc.LoadXml(xmlMessage);
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("ContMgt",
                "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8");
            ns.AddNamespace("esb", "http://www.sars.gov.za/esb/common/v000006/header");
            var contentManagementRequest = doc.SelectSingleNode("//ContMgt:ContentManagementRequest", ns);
            var messageIdentification = doc.SelectSingleNode("//esb:MessageIdentification", ns);
            if (contentManagementRequest != null && messageIdentification != null)
            {
                var supportingDocs =
                    Sars.Systems.Serialization.XmlObjectSerializer
                        .ConvertXmlToObject<SupportingDocumentManagementRequestStructure>(
                            contentManagementRequest.OuterXml);
                var messageIdent =
                    Sars.Systems.Serialization.XmlObjectSerializer
                        .ConvertXmlToObject<Sars.ESBSchema.ApplicationInformation.MessageIdentification>(
                            messageIdentification.OuterXml);
                try
                {
                    DbWriter.UpdateMneViaFileNotification
                        (
                            messageIdent.externalReferenceID
                            , Convert.ToInt32(supportingDocs.Contents[0].YearOfAssessment)
                        );
                }
                catch (Exception)
                {
                    ;
                }

                submissionId = DbWriter.InsertFileSubmission(messageIdent.externalReferenceID, Convert.ToInt32(supportingDocs.Contents[0].YearOfAssessment));

                if (submissionId > 0)
                {
                    foreach (var documentStructure in supportingDocs.Contents)
                    {
                        string category = null;
                        string classification = null;
                        string uniqueId = null;
                        if (documentStructure.EntityIdentifiers != null &&
                            documentStructure.EntityIdentifiers.Length >= 2)
                        {
                            category = documentStructure.EntityIdentifiers[0].Type;
                            classification = documentStructure.EntityIdentifiers[1].Type;
                        }

                        var categoryId = 3;
                        if (!string.IsNullOrEmpty(category))
                        {
                            categoryId = category.ToUpper().StartsWith("FDR_MASTER")
                                ? 1
                                : (category.ToUpper().StartsWith("FDR_LOCAL") ? 2 : 3);
                        }
                        if (categoryId == 3)
                        {
                            foreach (var identifier in documentStructure.DocumentIdentifiers)
                            {
                                if (identifier.Type.Contains("FDR_MASTER"))
                                {
                                    categoryId = 1;
                                    break;
                                }
                                if (identifier.Type.Contains("FDR_LOCAL"))
                                {
                                    categoryId = 2;
                                    break;
                                }
                            }
                        }
                        uniqueId = documentStructure.UniqueIdentifiers != null &&
                                   documentStructure.UniqueIdentifiers.Length > 0
                            ? documentStructure.UniqueIdentifiers[0]
                            : null;
                        DbWriter.SaveFile(
                            submissionId,
                            messageIdent.externalReferenceID,
                            categoryId,
                            documentStructure.ObjectName,
                            documentStructure.DocumentID,

                            documentStructure.ContentLocation,
                            documentStructure.Format,
                            documentStructure.YearOfAssessment,
                            documentStructure.Path,
                            documentStructure.SourceChannel,
                            documentStructure.DateOfReceipt.Equals(DateTime.MinValue)
                                ? string.Empty
                                : documentStructure.DateOfReceipt.ToString(CultureInfo.InvariantCulture),
                            classification,
                            uniqueId
                            );
                    }

                    DbWriter.LogIncomingFiles(submissionId, xmlMessage);
                }
            }
            return submissionId;
        }

        private static void RespondToDocumentNotify(string qMessage, string messageId)
        {
            try
            {
                var queueInfo = new QueueInfo
                {
                    Manager = LMApplicationConfigurationdetails.ResponseManger,
                    Channel = LMApplicationConfigurationdetails.ResponseChannel,
                    UseManagerName = false,
                    Port = LMApplicationConfigurationdetails.ResponsePort,
                    HostName = LMApplicationConfigurationdetails.ResponseHostName,
                    QueueName = LMApplicationConfigurationdetails.ResponseQueueName,
                    CorrelationId = messageId,
                    Message = qMessage
                };
                FdrMessaging.SendMessageToQueue(queueInfo);
                try
                {
                    FdrCommon.SaveMessage(qMessage, @"\\ptadviis06\TaxDirectives\FDR\FILES\OUT");
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

        private static DataValidations IsMasterLocalFileRequestValid(string message)
        {
            var contentManagementRequest = FdrCommon.SelectNode(message, "ContMgt", "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8", "ContentManagementRequest");

            var validations = new DataValidations(contentManagementRequest)
            {
                SchemaList = new List<InternalSchemaData>()
            };

            var contMgt = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "ML", "SARSContentManagement.xsd"));
            var sarsgmdFbasetypes = File.ReadAllText(string.Format(AppConfig.SchemBaseFolder, "ML", "SARSGMD_BaseTypes.xsd"));

            validations.SchemaList.AddRange(
                new List<InternalSchemaData>
                {
                    new InternalSchemaData {TargetNamespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8", SchemaContent = contMgt},
                    new InternalSchemaData{TargetNamespace ="http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.8",SchemaContent = sarsgmdFbasetypes}
                }
                );

            validations.ValidateSchema();
            return validations;
        }

    }
}
