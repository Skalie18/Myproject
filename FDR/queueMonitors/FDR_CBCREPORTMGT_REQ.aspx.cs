using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using FDR.DataLayer;
using Sars.ESBSchema.ApplicationInformation.v31;
using Sars.Systems.Data;
using Sars.Systems.Messaging;
using Sars.Systems.Utilities;
using FDRService.CbcStatusMessage;

public partial class queueMonitors_FDR_CBCREPORTMGT_REQ : System.Web.UI.Page
{
    /*
     * 
     * THIS PAGE ALLOWS US TO DOWNLOAD STATUS REPORT AFTER WE SENT A PACKAGE
     * 
     */

    private readonly ESBMessagingServiceClient _client = new ESBMessagingServiceClient("basic");
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var message = File.ReadAllText(@"D:\SARS\INCOMING\USA.xml");

            var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v31.
                ApplicationInformationStructure
            {
                ApplicationInformationResult =
                    new Sars.ESBSchema.ApplicationInformation.v31.
                        ApplicationInformationStructureApplicationInformationResult[1]
            };


            var incomingMessage = new IncomingMessage(HttpContext.Current);
            //var message = incomingMessage.Message;
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            message = message.Replace("ns0:", "esb:");




            var dataset = new RecordSet();
            dataset.ReadXml(new StringReader(message));

            if (dataset.HasRows)
            {
                if (dataset.Tables.Contains("MessageIdentification") &&
                    dataset.Tables.Contains("CountryByCountryReportManagementRequest"))
                {
                    //var messageIdentification = dataset.Tables["MessageIdentification"];
                    var countryByCountryReportManagementRequest =
                        dataset.Tables["CountryByCountryReportManagementRequest"];

                    //var fileName = countryByCountryReportManagementRequest.Rows[0]["Filename"].ToString();
                    var destination = countryByCountryReportManagementRequest.Rows[0]["Destination"].ToString();
                    //var requestOperation = countryByCountryReportManagementRequest.Rows[0]["RequestOperation"].ToString();
                    var fileContent = countryByCountryReportManagementRequest.Rows[0]["FileContent"].ToString();

                    var fileBuffer = fileContent.Base64StringToByteArray();
                    if (Request.PhysicalApplicationPath != null)
                    {
                        var zipFolder = Path.Combine(Request.PhysicalApplicationPath,
                            SARSDataSettings.Settings.TempFolder,
                            DateTime.Now.ToFileTime().ToString());
                        if (!Directory.Exists(zipFolder))
                        {
                            Directory.CreateDirectory(zipFolder);
                        }
                        var zipFile = Path.Combine(zipFolder, string.Format("{0}.ZIP", DateTime.Now.ToFileTime()));
                        File.WriteAllBytes(zipFile, fileBuffer);

                        Sars.Systems.Compression.ZipExtraction.ExtractZipFile(zipFile, zipFolder);
                        var files = new DirectoryInfo(zipFolder).GetFiles("*.*").ToList();

                        var payLoadFiles = files.Where(f => f.Name.Contains("Payload")).ToList();
                        var metaDataFiles = files.Where(f => f.Name.Contains("Metadata")).ToList();
                        if (!payLoadFiles.Any())
                        {
                            return;
                        }
                        if (!metaDataFiles.Any())
                        {
                            return;
                        }
                        var payloadData = File.ReadAllText(payLoadFiles[0].FullName);

                        if (string.IsNullOrEmpty(payloadData))
                        {
                            return;
                        }
                        var doc = new XmlDocument();
                        var originalMessageDocument = new XmlDocument();
                        doc.LoadXml(payloadData);
                        originalMessageDocument.LoadXml(message);

                        var cbCXmlNodeList = doc.GetElementsByTagName("CbCStatusMessage_OECD", "*");
                        var headerNodeList = originalMessageDocument.GetElementsByTagName("Header", "*");
                        var countryByCountryReportManagementRequestNodeList =
                            originalMessageDocument.GetElementsByTagName("CountryByCountryReportManagementRequest", "*");
                        if (cbCXmlNodeList.Count == 0 || headerNodeList.Count == 0 ||
                            countryByCountryReportManagementRequestNodeList.Count == 0)
                        {
                            //Common.LogEvent("XmlNodeList CbCXml = xmlDoc.GetElementsByTagName(\"CBC_OECD\", \" * \"); yielded zero elements");
                            return;
                        }
                        var statusXml = cbCXmlNodeList[0].OuterXml;
                        var headerXml = headerNodeList[0].OuterXml;
                        var countryByCountryReportManagementRequestXml = countryByCountryReportManagementRequestNodeList[0].OuterXml;


                        var schenaVal = _client.ValidateSchema(Configurations.CbCStatusValidationServiceID, statusXml);
                        if (!schenaVal.IsValid)
                        {
                            applicationInformationStructure.ApplicationInformationResult[0] =
                                new 
                                    ApplicationInformationStructureApplicationInformationResult
                                {
                                    Code = "9999",
                                    Description = schenaVal.ErrorXml,
                                    MessageType = MessageTypeEnum.ERROR
                                };
                            var messageToSend = CreateXml(headerXml, applicationInformationStructure, countryByCountryReportManagementRequestXml);
                            SendMessage(messageToSend, incomingMessage, _client);
                            return;
                        }

                        if (!destination.Equals("ZA", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var code = "9999";
                            var returnMessage = "MESSAGE DESTINATION NOT ZA";
                            RespondToB2Bi(
                                applicationInformationStructure
                                , code
                                , returnMessage
                                , true
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage);
                            return;
                        }
                        if (string.IsNullOrEmpty(fileContent))
                        {
                            var code = "9999";
                            var returnMessage = "FILE CONTENT EMPTY";
                            RespondToB2Bi(
                                applicationInformationStructure
                                , code
                                , returnMessage
                                , true
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage);
                            return;
                        }

                        var statusMessage =
                            Sars.Systems.Serialization.XmlObjectSerializer.ConvertXmlToObject<CbCStatusMessage_OECD>(
                                statusXml);
                        if (statusMessage == null)
                        {
                            var code = "9999";
                            var returnMessage = "XML COULD NOT BE DESERIALISED";
                            RespondToB2Bi(
                                applicationInformationStructure
                                , code
                                , returnMessage
                                , true
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage);
                            return;
                        }

                        try
                        {
                            SaveMessageToDisc(statusMessage, message);
                        }
                        catch (Exception ex)
                        {
                           LogError(ex);
                        }
                        var originalMessageRefId = statusMessage.CbCStatusMessage.OriginalMessage.OriginalMessageRefID;
                        if (!string.IsNullOrEmpty(originalMessageRefId))
                        {
                            var indexOfDesh = originalMessageRefId.IndexOf("-", StringComparison.Ordinal);
                            if (indexOfDesh == -1)
                            {
                                var code = "9999";
                                var returnMessage = "MessageRefId not correct";
                                RespondToB2Bi(
                                    applicationInformationStructure
                                    , code
                                    , returnMessage
                                    , true
                                    , headerXml
                                    , countryByCountryReportManagementRequestXml
                                    , incomingMessage);
                                return;
                            }
                            var origionalUid = originalMessageRefId.Substring(indexOfDesh + 3, 36);

                            Guid uidGuid;
                            if (!Guid.TryParse(origionalUid, out uidGuid))
                            {
                                var code = "9999";
                                var returnMessage = "originalMessageRefId is not correct";
                                RespondToB2Bi(
                                    applicationInformationStructure
                                    , code
                                    , returnMessage
                                    , true
                                    , headerXml
                                    , countryByCountryReportManagementRequestXml
                                    , incomingMessage);
                                return;
                            }
                            //uidGuid = Guid.Parse("EF663F3E-E9DF-4FE3-8EAD-8BECCECF42AC");

                            DBWriteManager.Insert_OutgoingPackageAuditTrail
                                (
                                    uidGuid,
                                    null,
                                    string.Format("Status Message received from {0} - {1}",
                                        statusMessage.MessageSpec.TransmittingCountry,
                                        statusMessage.MessageSpec.MessageRefId)
                                );

                            DBWriteManager.Insert_OutgoingPackageAuditTrail
                                (
                                    uidGuid,
                                    null,
                                    "Incoming status update validation successful."
                                );
                            DBWriteManager.UpdatePackageWithStatusFromeOtherCountries
                                (
                                    uidGuid
                                    , statusXml
                                    , statusMessage.CbCStatusMessage.ValidationResult.Status.ToString()
                                );

                            var validationErrors = statusMessage.CbCStatusMessage.ValidationErrors;
                            if (validationErrors.FileError != null && validationErrors.FileError.Any())
                            {
                                var numErrors = 0;
                                foreach (var fileErrorType in validationErrors.FileError)
                                {
                                    var code = fileErrorType.Code;
                                    var errorDescription = fileErrorType.Details.Value;
                                    numErrors += DBWriteManager.Insert_OutgoingPackage_File_ReturnErrors(
                                        uidGuid,
                                        code,
                                        errorDescription
                                        );
                                }
                                DBWriteManager.Insert_OutgoingPackageAuditTrail
                                    (
                                        uidGuid,
                                        null,
                                        string.Format("{0} File Validation Error(s) Found", numErrors)
                                    );                             
                            }
                            DBWriteManager.Insert_OutgoingPackageAuditTrail
                                 (
                                     uidGuid,
                                     null,
                                     string.Format("File Status is : {0}", statusMessage.CbCStatusMessage.ValidationResult.Status)
                                 );
                            if (validationErrors.RecordError != null && validationErrors.RecordError.Any())
                            {
                                var numErrors = 0;
                                foreach (var recordError in validationErrors.RecordError)
                                {
                                    var code = recordError.Code;
                                    var errorDescription = recordError.Details.Value;
                                    var docRef = string.Join(" | ", recordError.DocRefIDInError);
                                    numErrors += DBWriteManager.Insert_OutgoingPackage_Record_ReturnErrors(
                                        uidGuid,
                                        code,
                                        errorDescription,
                                        docRef
                                        );
                                }
                                DBWriteManager.Insert_OutgoingPackageAuditTrail
                                    (
                                        uidGuid,
                                        null,
                                        string.Format("{0} Record Validation Error(s) Found", numErrors)
                                    );
                            }
                            applicationInformationStructure.ApplicationInformationResult[0] = new ApplicationInformationStructureApplicationInformationResult
                            {
                                Code = "0000",
                                Description = "Successful",
                                MessageType = MessageTypeEnum.INFORMATION
                            };
                            var successMessageToSend = CreateXml(headerXml, applicationInformationStructure,
                                countryByCountryReportManagementRequestXml);
                            SendMessage(successMessageToSend, incomingMessage, _client);
                        }
                        Directory.Delete(zipFolder, true);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            LogError(exception);
        }
        finally
        {
            _client.Close();
        }
    }
    private static void LogError(Exception exception)
    {
        var fileName = string.Format(@"D:\SARS\Errors\IN_STATUS\{0}.error", DateTime.Now.ToString("yyyyMMdd"));
        var dir = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.AppendAllText(fileName, Environment.NewLine + exception.ToString());
    }
    private void RespondToB2Bi(ApplicationInformationStructure applicationInformationStructure, string code,
        string returnMessage, bool isError, string headerXml, string countryByCountryReportManagementRequestXml,
        IncomingMessage incomingMessage)
    {
        applicationInformationStructure.ApplicationInformationResult[0] = new ApplicationInformationStructureApplicationInformationResult
        {
            Code = code,
            Description = returnMessage,
            MessageType = isError ? MessageTypeEnum.ERROR : MessageTypeEnum.INFORMATION
        };
        var messageToSend = CreateXml(headerXml, applicationInformationStructure, countryByCountryReportManagementRequestXml);
        SendMessage(messageToSend, incomingMessage, _client);
    }

    private static void SendMessage(string messageToSend, IncomingMessage incomingMessage, ESBMessagingServiceClient client)
    {
        var contract = new WriteMessageData
        {
            ConfigurationId = new Guid(Configurations.CBCREPORTMGT_CHANNELID),
            Message = messageToSend,
            CorrelationId = incomingMessage.MessageId
        };
        var sent = client.SendAndForget(contract);
    }

    private static string CreateXml(string header, Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructure appInfo, string body)
    {
        //var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructure
        //{
        //    ApplicationInformationResult = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult[1]
        //};
        //applicationInformationStructure.ApplicationInformationResult[0] =
        //    new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult
        //    {
        //        Code = "0000",
        //        Description = "Processed",
        //        MessageType = Sars.ESBSchema.ApplicationInformation.v31.MessageTypeEnum.INFORMATION
        //    };

        var applicationInformationStructureXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(appInfo, "fdri", "http://www.egovernment.gov.za/GMD/ApplicationInformation/xml/schemas/version/3.1");

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
            body
            );
        return FdrCommon.FormatXml(xmlBuilder.ToString());
    }

    private static void SaveMessageToDisc(CbCStatusMessage_OECD metaDataObject, string message)
    {
        var incomingCBCPath = Path.Combine(@"D:\SARS\INCOMING_STATUS", DateTime.Now.ToString("yyyy-MM-dd"),
            Configurations.CurrentEnvironment);
        if (!Directory.Exists(incomingCBCPath))
        {
            Directory.CreateDirectory(incomingCBCPath);
        }
        File.WriteAllText(string.Format(@"{0}\{1}_{2}.STATUSIN", incomingCBCPath, metaDataObject.MessageSpec.TransmittingCountry,
                DateTime.Now.ToFileTime()), message);
    }
}