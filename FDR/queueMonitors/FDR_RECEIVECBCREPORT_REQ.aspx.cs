//queueMonitors_FDR_RECEIVECBCREPORT_REQ

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using FDR.DataLayer;
using FDRService.CbcStatusMessage;
using FDRService.CbcXML;
using FDRService.CbcXML.CountryByCountryReportManagement;
using Sars.ESBSchema.ApplicationInformation.v31;
using Sars.ESBSchema.Header.V7_1;
using Sars.Systems.Compression;
using Sars.Systems.Data;
using Sars.Systems.Messaging;
using Sars.Systems.Utilities;
using LanguageCode_Type = FDRService.CbcStatusMessage.LanguageCode_Type;
using MessageType_EnumType = FDRService.CbcStatusMessage.MessageType_EnumType;
using Sars.Systems.Serialization;
public partial class queueMonitors_FDR_RECEIVECBCREPORT_REQ : System.Web.UI.Page
{
    readonly List<FileError_Type> _listOfFileErrorType = new List<FileError_Type>();
    readonly List<RecordError_Type> _listOfRecordErrorType = new List<RecordError_Type>();
    readonly List<string> _duplicateDocRefId = new List<string>();
    readonly List<string> _duplicateCorrDocRefId = new List<string>();
    readonly List<string> _allCorrDocRefId = new List<string>();
    readonly List<string> _mixed80010 = new List<string>();
    readonly List<string> _unKnownCorDocRefId = new List<string>();

    private ESBMessagingServiceClient _client = new ESBMessagingServiceClient("basic");

    readonly List<string> _newDataWithCorrDocRefId = new List<string>();
    readonly List<string> _correctedDataWithNoCorrDocRefId = new List<string>();
    readonly List<string> _incorrectFormttedDocRefId = new List<string>();
    readonly List<string> _forbiddenCorrMessageId = new List<string>();
    //readonly List<string> _duplicateCorrMessageId = new List<string>();
    readonly List<string> _declineReasons = new List<string>();
    readonly List<string> _missingInfo = new List<string>();
    readonly List<string> _mixedMessageTypeIndicators = new List<string>();
    readonly List<OECDDocTypeIndic_EnumType> _messageTypeIndicators = new List<OECDDocTypeIndic_EnumType>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            _client = new ESBMessagingServiceClient("basic");
            ReceiveReport(null);
        }
        catch (Exception exception)
        {
            LogError(exception);
        }
        finally
        {
            if (_client != null)
            {
                _client.Close();
            }
        }
    }

    private static void LogError(Exception exception)
    {
        var fileName = string.Format(@"D:\SARS\Errors\{0}.error", DateTime.Now.ToString("yyyyMMdd"));
        var dir = Path.GetDirectoryName(fileName);

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.AppendAllText(fileName, Environment.NewLine + exception.ToString());
    }

    private void ReceiveReport(string FileName)
    {
        //var message = File.ReadAllText(FileName);
        var applicationInformationStructure = new ApplicationInformationStructure
        {
            ApplicationInformationResult = new ApplicationInformationStructureApplicationInformationResult[1]
        };
        var incomingMessage = new IncomingMessage(HttpContext.Current);
        var message = incomingMessage.Message;
        if (!string.IsNullOrEmpty(message))
        {
            //var messageObject = new MessageObject
            //{
            //    MessageId = incomingMessage.MessageId,
            //    Content = incomingMessage.Message.ToBase64String(),
            //    ManagerName = incomingMessage.ManagerName,
            //    Host = incomingMessage.Host,
            //    MessageCorrelationId = incomingMessage.MessageCorrelationId,
            //    Processed = false,
            //    QueuName = incomingMessage.QueuName
            //};

            //var messageFileName = Configurations.MessagePath;
            //if (!Directory.Exists(Configurations.MessagePath))
            //{
            //    Directory.CreateDirectory(Configurations.MessagePath);
            //}
            //var fileName =
            //    Path.Combine(string.Format(@"{0}\{1}.CBCIN", Configurations.MessagePath, DateTime.Now.ToFileTime()));

            //File.WriteAllText(fileName, XmlObjectSerializer.GetXmlWithNoDeclaration(messageObject));

            //var message = incomingMessage.Message;
            //if (string.IsNullOrEmpty(message))
            //{
            //    return;
            //}

            var dataset = new RecordSet();
            dataset.ReadXml(new StringReader(message));

            if (dataset.HasRows)
            {
                var cbCStatusMessageOecd = new CbCStatusMessage_OECD
                {
                    CbCStatusMessage = new CbCMessageStatus_Type(),
                    MessageSpec = new FDRService.CbcStatusMessage.MessageSpec_Type()
                };

                if (dataset.Tables.Contains("MessageIdentification") &&
                    dataset.Tables.Contains("CountryByCountryReportManagementRequest"))
                {
                    //var messageIdentification = dataset.Tables["MessageIdentification"];
                    var countryByCountryReportManagementRequest =
                        dataset.Tables["CountryByCountryReportManagementRequest"];

                    //var fileName = countryByCountryReportManagementRequest.Rows[0]["Filename"].ToString();
                    //var destination = countryByCountryReportManagementRequest.Rows[0]["Destination"].ToString();
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

                        ZipExtraction.ExtractZipFile(zipFile, zipFolder);
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
                        var metaData = File.ReadAllText(metaDataFiles[0].FullName);

                        if (string.IsNullOrEmpty(payloadData) || string.IsNullOrEmpty(metaData))
                        {
                            return;
                        }

                        Directory.Delete(zipFolder, true);

                        var doc = new XmlDocument();
                        var originalMessageDocument = new XmlDocument();
                        var metaDataDoc = new XmlDocument();

                        doc.LoadXml(payloadData);
                        originalMessageDocument.LoadXml(message);
                        metaDataDoc.LoadXml(metaData);

                        var cbCXmlNodeList = doc.GetElementsByTagName("CBC_OECD", "*");
                        var headerNodeList = originalMessageDocument.GetElementsByTagName("Header", "*");
                        var countryByCountryReportManagementRequestNodeList = originalMessageDocument.GetElementsByTagName("CountryByCountryReportManagementRequest", "*");
                        var metaDataNodeList = metaDataDoc.GetElementsByTagName("CTSSenderFileMetadata", "*");

                        if (cbCXmlNodeList.Count == 0 || headerNodeList.Count == 0 ||
                            countryByCountryReportManagementRequestNodeList.Count == 0 || metaDataNodeList.Count == 0)
                        {
                            //Common.LogEvent("XmlNodeList CbCXml = xmlDoc.GetElementsByTagName(\"CBC_OECD\", \" * \"); yielded zero elements");
                            return;
                        }

                        var cbcXml = cbCXmlNodeList[0].OuterXml;
                        var headerXml = headerNodeList[0].OuterXml;
                        var countryByCountryReportManagementRequestXml = countryByCountryReportManagementRequestNodeList[0].OuterXml;
                        var metaDataXml = metaDataNodeList[0].OuterXml;

                        var incomingCbcOecdMessageSpec = GetCBC_OECD_MessageSpec(cbcXml);
                        if (incomingCbcOecdMessageSpec == null)
                        {
                            return;
                        }
                        var uid = Guid.NewGuid();
                        var messageRefId = string.Format("Status{0}{1}{2}{3}"
                            , FDRService.CbcStatusMessage.CountryCode_Type.ZA
                            , incomingCbcOecdMessageSpec.ReportingPeriod.Substring(0, 4)
                            , incomingCbcOecdMessageSpec.TransmittingCountry
                            , uid.ToString().ToUpper());

                        string tranCountry = null;
                        var countriesUsingHub = Configurations.CountriesUsingHub;
                        if (countriesUsingHub == null)
                        {
                            tranCountry = incomingCbcOecdMessageSpec.TransmittingCountry;
                        }
                        else
                        {
                            if (
                                countriesUsingHub.ToList()
                                    .Exists(x => x.Equals(incomingCbcOecdMessageSpec.TransmittingCountry)))
                            {
                                tranCountry = string.Format("{0}.00", incomingCbcOecdMessageSpec.TransmittingCountry);
                            }
                        }
                        var ctsSenderId = string.Format("ZA{0}{1}{2}"
                            , incomingCbcOecdMessageSpec.ReportingPeriod.Substring(0, 4)
                            , tranCountry
                            , uid);

                        var countryByCountryReportManagementRequestSchemaVal =
                            _client.ValidateSchema(Configurations.CountryByCountryReportManagementValidationServiceID,
                                countryByCountryReportManagementRequestXml);

                        if (!countryByCountryReportManagementRequestSchemaVal.IsValid)
                        {
                            var mesage = countryByCountryReportManagementRequestSchemaVal.ErrorXml;
                            RespondWithError(
                                applicationInformationStructure
                                , mesage
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage
                                , _client);
                            return;
                        }
                        var metaDataXmlSchemaVal = _client.ValidateSchema(Configurations.CTSSenderFileMetadata, metaDataXml);
                        if (!metaDataXmlSchemaVal.IsValid)
                        {
                            var mesage = metaDataXmlSchemaVal.ErrorXml;
                            RespondWithError(
                                applicationInformationStructure
                                , mesage
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage
                                , _client);
                            return;
                        }
                        var metaDataObject =
                            XmlObjectSerializer
                                .ConvertXmlToObject<FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType>(metaDataXml);

                        if (metaDataObject == null)
                        {
                            var mesage =
                                "COULD NOT DESERIELIZE CTSMETADATA XML BACK TO CTSSenderFileMetadataType OBJECT";
                            RespondWithError(
                                applicationInformationStructure
                                , mesage
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage
                                , _client
                                );
                            DBWriteManager.Insert_X_Masages_Failed_Validation(
                                incomingCbcOecdMessageSpec.TransmittingCountry
                                , FdrCommon.FormatXml(cbcXml)
                                , incomingCbcOecdMessageSpec.MessageRefId
                                , ctsSenderId
                                , incomingCbcOecdMessageSpec.ReportingPeriod
                                , "COULD NOT DESERIELIZE CTSMETADATA XML BACK TO CTSSenderFileMetadataType OBJECT"
                                );
                            return;
                        }
                        try
                        {
                            SaveMessageToDisc(metaDataObject, message);
                        }
                        catch (Exception exception)
                        {
                            LogError(exception);
                        }
                        var newCtsSenderFileMetadataType = CreateNewMetaData(metaDataObject, ctsSenderId);
                        cbcXml = FdrCommon.FormatXml(cbcXml);
                        var cbcSchemaVal = _client.ValidateSchema(Configurations.CbCValidationServiceID, cbcXml);


                        if (!cbcSchemaVal.IsValid)
                        {
                            var mesage = "The referenced file failed validation against the CbC XML Schema.";
                            RespondWithSuccess(
                                applicationInformationStructure
                                , "Successfully Received and Processed"
                                , headerXml
                                , countryByCountryReportManagementRequestXml
                                , incomingMessage
                                , _client);
                            AddFileErrorInValidations("50007", mesage);
                            var package = CreateStatusMessage(
                                cbCStatusMessageOecd
                                , incomingCbcOecdMessageSpec
                                , messageRefId
                                , metaDataObject
                                , newCtsSenderFileMetadataType
                                , true
                                , uid.ToString());
                            SendStatusMessage(package, incomingMessage, _client);

                            DBWriteManager.Insert_X_Masages_Failed_Validation(
                                incomingCbcOecdMessageSpec.TransmittingCountry
                                , FdrCommon.FormatXml(cbcXml)
                                , incomingCbcOecdMessageSpec.MessageRefId
                                , metaDataObject.SenderFileId
                                , incomingCbcOecdMessageSpec.ReportingPeriod
                                , mesage
                                );
                            return;
                        }

                        var cbcMessage = XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXml);

                        if (cbcMessage == null)
                        {
                            var mesage = "COULD NOT DESERIALIZE CBC XML BACK TO CBC_OECD OBJECT";
                            RespondWithError(applicationInformationStructure, mesage, headerXml, countryByCountryReportManagementRequestXml, incomingMessage, _client);
                            DBWriteManager.Insert_X_Masages_Failed_Validation(
                                incomingCbcOecdMessageSpec.TransmittingCountry
                                , FdrCommon.FormatXml(cbcXml)
                                , incomingCbcOecdMessageSpec.MessageRefId
                                , metaDataObject.SenderFileId
                                , incomingCbcOecdMessageSpec.ReportingPeriod
                                , mesage
                                );
                            return;
                        }
                        if (cbcMessage.MessageSpec == null)
                        {
                            var errorMessage = "CBC report is missing ReportingEntity";
                            _missingInfo.Add(errorMessage);
                            AddFileErrorInValidations("99999", errorMessage);

                            RespondWithSuccess(applicationInformationStructure, "Successfully Received and Processed",
                                headerXml,
                                countryByCountryReportManagementRequestXml, incomingMessage, _client);

                            var package = CreateStatusMessage(
                                cbCStatusMessageOecd
                                , incomingCbcOecdMessageSpec
                                , messageRefId
                                , metaDataObject
                                , newCtsSenderFileMetadataType
                                , true
                                , uid.ToString());
                            SendStatusMessage(package, incomingMessage, _client);
                            DBWriteManager.Insert_X_Masages_Failed_Validation(
                               incomingCbcOecdMessageSpec.TransmittingCountry
                               , FdrCommon.FormatXml(cbcXml)
                               , incomingCbcOecdMessageSpec.MessageRefId
                               , metaDataObject.SenderFileId
                               , incomingCbcOecdMessageSpec.ReportingPeriod
                               , errorMessage
                               );
                            return;
                        }

                        var rolesToSendTo =
                            System.Configuration.ConfigurationManager.AppSettings["Role-To-Notify-For-Incoming-CBC"]
                                .Split("|".ToCharArray());
                        if (rolesToSendTo != null)
                        {
                            if (rolesToSendTo.Length > 0)
                            {
                                foreach (var r in rolesToSendTo)
                                {
                                    var roles = DBReadManager.GetRolesToNotify(r);

                                    Notifications.SendNotification(
                                        cbcMessage.MessageSpec.ReportingPeriod.ToString("yyyy-MM-dd")
                                        , cbcMessage.MessageSpec.TransmittingCountry.ToString()
                                        , roles
                                        );
                                }
                            }
                        }
                        if (MessageNotMeantForZa(
                            metaDataObject
                            , incomingCbcOecdMessageSpec
                            , applicationInformationStructure
                            , headerXml
                            , countryByCountryReportManagementRequestXml
                            , incomingMessage
                            , cbCStatusMessageOecd
                            , messageRefId
                            , newCtsSenderFileMetadataType
                            , uid
                            , cbcMessage
                            , cbcXml))
                        {
                            var sav = DBWriteManager.Insert_X_Masages_Failed_Validation(
                             incomingCbcOecdMessageSpec.TransmittingCountry
                             , FdrCommon.FormatXml(cbcXml)
                             , incomingCbcOecdMessageSpec.MessageRefId
                             , metaDataObject.SenderFileId
                             , incomingCbcOecdMessageSpec.ReportingPeriod
                             , "CBC report is not meant for South Africa"
                             );
                            return;
                        }
                        var transmittingCountry = cbcMessage.MessageSpec.TransmittingCountry.ToString();
                        var origionalMessageRefId = cbcMessage.MessageSpec.MessageRefId;
                        if (string.IsNullOrEmpty(origionalMessageRefId))
                        {
                            var errorMessage = "The field Message.MessageRefID is missing, an empty value and a blank space are unacceptable.";
                            AddRecordErrorInValidations("70000", errorMessage, new List<string>());
                            _declineReasons.Add(errorMessage);
                        }

                        if (!IsCorrectFormatMessageRefId(cbcMessage.MessageSpec.MessageRefId, transmittingCountry, cbcMessage.MessageSpec.ReportingPeriod.Year))
                        {
                            AddFileErrorInValidations("50008", "The structure of the MessageRefID is not in the correct format.");
                        }
                        if (cbcMessage.MessageSpec.CorrMessageRefId != default(string[]))
                        {
                            var errorMessage = "The CorrMessageRefID is forbidden within the Message Header";
                            AddFileErrorInValidations("80007", errorMessage);
                            _declineReasons.Add(errorMessage);
                        }

                        var messageRefIdExists = DBReadManager.MessageRefIdAlreadyExists
                            (
                                origionalMessageRefId
                                , transmittingCountry
                            );

                        if (messageRefIdExists > 0)
                        {
                            var errorMessage = string.Format("The referenced file [{0}] has a duplicate MessageRefID value that was received on a previous file.", origionalMessageRefId);
                            AddFileErrorInValidations("50009", errorMessage);
                            _declineReasons.Add(errorMessage);
                        }

                        //var packageRejected = false;
                        foreach (var cbcBodyType in cbcMessage.CbcBody)
                        {
                            var constEntityHasErrors = false;

                            if (cbcBodyType.ReportingEntity != null)
                            {
                                var reportingEntityDocSpecDocTypeIndic =
                                    cbcBodyType.ReportingEntity.DocSpec.DocTypeIndic;

                                switch (reportingEntityDocSpecDocTypeIndic)
                                {
                                    case OECDDocTypeIndic_EnumType.OECD10:
                                    case OECDDocTypeIndic_EnumType.OECD11:
                                    case OECDDocTypeIndic_EnumType.OECD12:
                                    case OECDDocTypeIndic_EnumType.OECD13:
                                        {
                                            if (Configurations.CBCEnvironment == CBCEnvironment.PRODUCTION)
                                            {
                                                var errorMessage = "File Contains Test Data for Production Environment";
                                                constEntityHasErrors = true;
                                                AddFileErrorInValidations("50010", errorMessage);
                                                _declineReasons.Add(errorMessage);
                                            }
                                            break;
                                        }
                                    default:
                                        {
                                            if (Configurations.CBCEnvironment != CBCEnvironment.PRODUCTION)
                                            {
                                                var errorMessage = "File Contains Production Data for Test Environment";
                                                constEntityHasErrors = true;
                                                AddFileErrorInValidations("50011", errorMessage);
                                                _declineReasons.Add(errorMessage);
                                            }
                                            break;
                                        }
                                }
                            }
                            if (!constEntityHasErrors)
                            {
                                if (cbcBodyType.CbcReports != null)
                                {
                                    foreach (var cbcReport in cbcBodyType.CbcReports)
                                    {
                                        var cbcReportsDocSpecDocTypeIndic = cbcReport.DocSpec.DocTypeIndic;

                                        switch (cbcReportsDocSpecDocTypeIndic)
                                        {
                                            case OECDDocTypeIndic_EnumType.OECD10:
                                            case OECDDocTypeIndic_EnumType.OECD11:
                                            case OECDDocTypeIndic_EnumType.OECD12:
                                            case OECDDocTypeIndic_EnumType.OECD13:
                                                {
                                                    if (Configurations.CBCEnvironment == CBCEnvironment.PRODUCTION)
                                                    {
                                                        var errorMessage =
                                                            "File Contains Test Data for Production Environment";
                                                        AddFileErrorInValidations("50010", errorMessage);
                                                        _declineReasons.Add(errorMessage);
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    if (Configurations.CBCEnvironment != CBCEnvironment.PRODUCTION)
                                                    {
                                                        var errorMessage =
                                                            "File Contains Production Data for Test Environment";
                                                        AddFileErrorInValidations("50011", errorMessage);
                                                        _declineReasons.Add(errorMessage);
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                }
                            }
                        }
                        //var corrections = 0;
                        //var deletions = 0;
                        var corrDocRefIds = new List<string>();
                        var docTypeIndics = new List<OECDDocTypeIndic_EnumType>();
                        foreach (var cbcBodyType in cbcMessage.CbcBody)
                        {
                            var messageTypeIndicator = cbcMessage.MessageSpec.MessageTypeIndic;

                            if (cbcBodyType.ReportingEntity != null)
                            {
                                var docSpec = cbcBodyType.ReportingEntity.DocSpec;
                                if (docSpec != null)
                                {
                                    corrDocRefIds.Add(docSpec.CorrDocRefId);
                                    ValidateFor80010(docSpec, messageTypeIndicator);
                                    docTypeIndics.Add(docSpec.DocTypeIndic);
                                }
                                ValidateReportingEntity(cbcBodyType, transmittingCountry, cbcMessage);
                            }
                            if (cbcBodyType.CbcReports != null)
                            {
                                foreach (var cbcReport in cbcBodyType.CbcReports)
                                {
                                    var reportDocSpec = cbcReport.DocSpec;
                                    if (reportDocSpec != null)
                                    {
                                        corrDocRefIds.Add(reportDocSpec.CorrDocRefId);
                                        ValidateFor80010(reportDocSpec, messageTypeIndicator);
                                        docTypeIndics.Add(reportDocSpec.DocTypeIndic);
                                    }
                                    ValdateCbcReport(cbcReport, transmittingCountry, cbcMessage);
                                }
                            }
                            if (cbcBodyType.AdditionalInfo != null)
                            {
                                foreach (var info in cbcBodyType.AdditionalInfo)
                                {
                                    var addInfoDocSpec = info.DocSpec;

                                    if (addInfoDocSpec != null)
                                    {
                                        corrDocRefIds.Add(addInfoDocSpec.CorrDocRefId);
                                        ValidateFor80010(addInfoDocSpec, messageTypeIndicator);
                                        docTypeIndics.Add(addInfoDocSpec.DocTypeIndic);
                                    }
                                    ValidateAdditionalInfo(info, transmittingCountry, cbcMessage);
                                }
                            }
                        }

                        /////////////////////////////////////////


                        var newData = 0;
                        var correctedData = 0;
                        var deletedData = 0;
                        var mixedDocTypeIndicators = docTypeIndics.Distinct().ToList();
                        foreach (var docTypeInd in mixedDocTypeIndicators)
                        {
                            switch (docTypeInd)
                            {
                                case OECDDocTypeIndic_EnumType.OECD1:
                                case OECDDocTypeIndic_EnumType.OECD11:
                                    {
                                        newData++;
                                        break;
                                    }
                                case OECDDocTypeIndic_EnumType.OECD2:
                                case OECDDocTypeIndic_EnumType.OECD12:
                                    {
                                        correctedData++;
                                        break;
                                    }
                                case OECDDocTypeIndic_EnumType.OECD3:
                                case OECDDocTypeIndic_EnumType.OECD13:
                                    {
                                        deletedData++;
                                        break;
                                    }
                            }
                        }


                        ///////////////////////////////////////////

                        if (_duplicateDocRefId.Any())
                        {
                            var errorMessage = "The DocRefID is already used for another record.";
                            AddRecordErrorInValidations(
                                "80000"
                                , errorMessage
                                , _duplicateDocRefId);
                            //_declineReasons.Add(errorMessage);
                        }
                        if (_incorrectFormttedDocRefId.Any())
                        {
                            var errorMessage = "The structure of the DocRefID is not in the correct format.";
                            AddRecordErrorInValidations(
                                "80001"
                                , errorMessage
                                , _incorrectFormttedDocRefId);
                        }

                        if (_correctedDataWithNoCorrDocRefId.Any())
                        {
                            var errorMessage = "The corrected element does not specify any CorrDocRefId.";
                            AddRecordErrorInValidations(
                                "80005"
                                , errorMessage
                                , _correctedDataWithNoCorrDocRefId);
                            //_declineReasons.Add(errorMessage);
                        }

                        if (_newDataWithCorrDocRefId.Any())
                        {
                            AddRecordErrorInValidations(
                                "80004"
                                , "The initial element specifies a CorrDocRefId."
                                , _newDataWithCorrDocRefId);
                        }

                        if (_forbiddenCorrMessageId.Any())
                        {
                            AddRecordErrorInValidations(
                                "80006"
                                , "The CorrMessageRefID is forbidden within the DocSpec_Type."
                                , _forbiddenCorrMessageId);
                        }

                        if (_duplicateCorrDocRefId.Any())
                        {
                            var errorMessage =
                                "The same DocRefID cannot be corrected or deleted twice in the same message.";
                            AddRecordErrorInValidations(
                                "80011"
                                , errorMessage
                                , _duplicateCorrDocRefId);
                            //_declineReasons.Add(errorMessage);
                        }

                        if (newData > 0 && (correctedData > 0 || deletedData > 0))
                        {
                            var errorMessage =
                                "A message can contain either new records or [corrections/deletions], but should not contain a mixture of both.";
                            AddRecordErrorInValidations(
                                "80011"
                                , errorMessage
                                , new List<string>());
                        }

                        if (_mixed80010.Any())
                        {
                            var errorMessage =
                                "A message can contain either new records or [corrections/deletions], but should not contain a mixture of both.";
                            AddRecordErrorInValidations(
                                "80011"
                                , errorMessage
                                , _mixed80010);
                        }

                        var packageToSend = CreateStatusMessage(
                            cbCStatusMessageOecd
                            , incomingCbcOecdMessageSpec
                            , messageRefId
                            , metaDataObject
                            , newCtsSenderFileMetadataType
                            , _declineReasons.Any()
                            , uid.ToString());

                        SendStatusMessage(
                            packageToSend
                            , incomingMessage
                            , _client);
                        RespondWithSuccess(
                            applicationInformationStructure
                            , "Successfully Received and Processed"
                            , headerXml
                            , countryByCountryReportManagementRequestXml
                            , incomingMessage
                            , _client
                            );

                        //START SAVING

                        try
                        {
                            var ctsId = DBWriteManager.Insert_CTSSenderFileMetadata(
                                 metaDataObject.CTSSenderCountryCd.ToString()
                                 , metaDataObject.CTSReceiverCountryCd.ToString()
                                 , metaDataObject.CTSCommunicationTypeCd.ToString()
                                 , metaDataObject.SenderFileId
                                 , metaDataObject.FileFormatCd.ToString()
                                 , metaDataObject.FileCreateTs.ToString("yyyy-MM-ddTHH:mm:ss")
                                 , metaDataObject.TaxYear
                                 );

                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        var messageSpecId = Save_X_Details(cbcMessage, cbcXml);
                        var correction = false;
                        string originalMessageRefId = null;
                        if (correctedData > 0 || deletedData > 0)
                        {
                            correction = true;
                            if (!corrDocRefIds.Any())
                            {
                                correction = false;
                            }
                            else
                            {
                                originalMessageRefId = DBReadManager.GetOriginalMessageRefId(corrDocRefIds[0], metaDataObject.CTSSenderCountryCd.ToString());
                            }
                            if (originalMessageRefId == null)
                            {
                                correction = false;
                            }
                        }
                        DBWriteManager.Update_X_MessageSpec_FinalStatus(
                            messageSpecId,
                            _declineReasons.Any() ? "Rejected" : "Accepted"
                            , correction
                            , originalMessageRefId
                            );
                        foreach (var errorType in _listOfRecordErrorType)
                        {
                            foreach (var docRefId in errorType.DocRefIDInError)
                            {
                                if (!string.IsNullOrEmpty(docRefId))
                                {
                                    DBWriteManager.Insert_X_CBC_RecordValidations
                                        (
                                            messageSpecId
                                            , errorType.Code
                                            , docRefId
                                            , errorType.Details.Value
                                        );
                                }
                            }
                        }
                        foreach (var errorType in _listOfFileErrorType)
                        {
                            if (errorType.Details != null)
                            {
                                var code = errorType.Code;
                                var error = errorType.Details.Value;
                                DBWriteManager.Insert_X_CBC_FileValidations
                                    (
                                        messageSpecId
                                        , code
                                        , error
                                    );
                            }
                        }

                    }
                }
            }
        }
    }

    private void ValidateFor80010(DocSpec_Type docSpec, CbcMessageTypeIndic_EnumType messageTypeIndicator)
    {
        switch (docSpec.DocTypeIndic)
        {
            case OECDDocTypeIndic_EnumType.OECD1:
            case OECDDocTypeIndic_EnumType.OECD11:
                {
                    if (messageTypeIndicator == CbcMessageTypeIndic_EnumType.CBC402)
                    {
                        if (!_mixed80010.Contains(docSpec.DocRefId))
                        {
                            _mixed80010.Add(docSpec.DocRefId);
                        }
                    }
                    break;
                }

            case OECDDocTypeIndic_EnumType.OECD2:
            case OECDDocTypeIndic_EnumType.OECD12:
            case OECDDocTypeIndic_EnumType.OECD3:
            case OECDDocTypeIndic_EnumType.OECD13:
                {
                    if (messageTypeIndicator == CbcMessageTypeIndic_EnumType.CBC401)
                    {
                        if (!_mixed80010.Contains(docSpec.DocRefId))
                        {
                            _mixed80010.Add(docSpec.DocRefId);
                        }
                    }
                    break;
                }
        }
    }

    private bool MessageNotMeantForZa(
        FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType metaDataObject
        , CBC_OECD_MessageSpec incomingCbcOecdMessageSpec
        , ApplicationInformationStructure applicationInformationStructure
        , string headerXml
        , string countryByCountryReportManagementRequestXml
        , IncomingMessage incomingMessage
        , CbCStatusMessage_OECD cbCStatusMessageOecd
        , string messageRefId
        , FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType newCtsSenderFileMetadataType
        , Guid uid
        , CBC_OECD cbcMessage
        , string cbcXml)
    {
        var messageForZa = metaDataObject.CTSReceiverCountryCd ==
                           FDRService.CbcXML.CTSSenderFileMetadata.CountryCode_Type.ZA &&
                           incomingCbcOecdMessageSpec.ReceivingCountry.Equals("ZA",
                               StringComparison.CurrentCultureIgnoreCase);

        if (!messageForZa)
        {
            var error =
                string.Format("The received message is not meant to be received by the {0} jurisdiction.",
                    incomingCbcOecdMessageSpec.ReceivingCountry);
            AddFileErrorInValidations("50012", error);
            _declineReasons.Add(error);

            var mesage = "MESSAGE DESTINATION NOT ZA";
            RespondWithError(applicationInformationStructure
                , mesage
                , headerXml
                , countryByCountryReportManagementRequestXml
                , incomingMessage
                , _client);

            var package = CreateStatusMessage(cbCStatusMessageOecd
                , incomingCbcOecdMessageSpec
                , messageRefId
                , metaDataObject
                , newCtsSenderFileMetadataType
                , true
                , uid.ToString());
            SendStatusMessage(package, incomingMessage, _client);
            //Save_X_Details(cbcMessage, cbcXml);

            var messageSpecId = Save_X_Details(cbcMessage, cbcXml);
            DBWriteManager.Update_X_MessageSpec_FinalStatus(
                messageSpecId
                , _declineReasons.Any() ? "Accepted" : "Rejected"
                , false
                , incomingCbcOecdMessageSpec.MessageRefId);

            return true;
        }
        return false;
    }

    private static void SaveMessageToDisc(FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType metaDataObject, string message)
    {
        var incomingCBCPath = Path.Combine(@"D:\SARS\INCOMING", DateTime.Now.ToString("yyyy-MM-dd"),
            Configurations.CurrentEnvironment);

        if (!Directory.Exists(incomingCBCPath))
        {
            Directory.CreateDirectory(incomingCBCPath);
        }
        File.WriteAllText(string.Format(@"{0}\{1}_{2}.CBCIN", incomingCBCPath, metaDataObject.CTSSenderCountryCd,
                DateTime.Now.ToFileTime()), message);
    }

    private static decimal Save_X_Details(CBC_OECD cbcMessage, string cbcXml)
    {
        var messageSpecId = 0M;
        var messageSpec = XmlObjectSerializer.GetXmlWithNoDeclaration(cbcMessage.MessageSpec);
        var TransmittingCountry = cbcMessage.MessageSpec.TransmittingCountry.ToString();
        var ReceivingCountry = cbcMessage.MessageSpec.ReceivingCountry[0].ToString();
        var MessageType = cbcMessage.MessageSpec.MessageType.ToString();
        var Language = cbcMessage.MessageSpec.Language.ToString();
        var Warning = cbcMessage.MessageSpec.Warning;
        var Contact = cbcMessage.MessageSpec.Contact;
        var MessageRefId = cbcMessage.MessageSpec.MessageRefId;
        var ReportingPeriod = cbcMessage.MessageSpec.ReportingPeriod;
        var MessageTypeIndic = cbcMessage.MessageSpec.MessageTypeIndic.ToString();
        var MessageTimestamp = cbcMessage.MessageSpec.Timestamp;
        messageSpecId = DBWriteManager.Save_X_MessageSpec(
            messageSpecId
            , messageSpec
            , TransmittingCountry
            , ReceivingCountry
            , MessageType
            , Language
            , Warning
            , Contact
            , MessageRefId
            , ReportingPeriod
            , MessageTypeIndic
            , MessageTimestamp
            , cbcXml
            );

        if (messageSpecId == 0)
        {
            return 0;
        }

        foreach (var cbcBodyType in cbcMessage.CbcBody)
        {
            var cbcBody = XmlObjectSerializer.GetXmlWithNoDeclaration(cbcBodyType);
            var cbcBodyId = 0M;
            cbcBodyId = DBWriteManager.Save_X_CBC_Bodies(cbcBodyId, messageSpecId, cbcBody);
            if (cbcBodyId == 0)
            {
                continue;
            }

            if (cbcBodyType.ReportingEntity != null)
            {
                var reportingEntityId = DBWriteManager.Save_X_ReportingEntity(
                    0M
                    , cbcBodyId
                    , messageSpecId
                    , cbcBodyType.ReportingEntity.Entity.TIN.Value
                    ,
                    cbcBodyType.ReportingEntity.Entity.TIN.issuedBySpecified
                        ? cbcBodyType.ReportingEntity.Entity.TIN.issuedBy.ToString()
                        : null
                    , cbcBodyType.ReportingEntity.ReportingRole.ToString()
                    , cbcBodyType.ReportingEntity.DocSpec.DocTypeIndic.ToString()
                    , cbcBodyType.ReportingEntity.DocSpec.DocRefId
                    , cbcBodyType.ReportingEntity.DocSpec.CorrMessageRefId
                    , cbcBodyType.ReportingEntity.DocSpec.CorrDocRefId
                    );

                if (reportingEntityId != 0)
                {
                    if (cbcBodyType.ReportingEntity.Entity.ResCountryCode != null)
                    {
                        foreach (var resCode in cbcBodyType.ReportingEntity.Entity.ResCountryCode)
                        {
                            DBWriteManager.Save_X_ReportingEntity_Entity_ResCountryCode(0M, resCode.ToString(),
                                reportingEntityId);
                        }
                    }
                    if (cbcBodyType.ReportingEntity.Entity.IN != null)
                    {
                        foreach (var identityNo in cbcBodyType.ReportingEntity.Entity.IN)
                        {
                            DBWriteManager.Save_X_ReportingEntity_Entity_Identification_Numbers(
                                0M
                                , identityNo.Value
                                , identityNo.issuedBySpecified ? identityNo.issuedBy.ToString() : null
                                , identityNo.INType
                                , reportingEntityId);
                        }
                    }
                    if (cbcBodyType.ReportingEntity.Entity.Name != null)
                    {
                        foreach (var name in cbcBodyType.ReportingEntity.Entity.Name)
                        {
                            DBWriteManager.Save_X_ReportingEntity_Entity_Names(
                                0M
                                , name.Value
                                , reportingEntityId);
                        }
                    }
                }
            }
            if (cbcBodyType.CbcReports != null)
            {
                foreach (var cbcReport in cbcBodyType.CbcReports)
                {
                    var data = XmlObjectSerializer.GetXmlWithNoDeclaration(cbcReport);
                    var cbcReportId = DBWriteManager.Save_X_CbcReports(
                        0M,
                        cbcReport.DocSpec.DocTypeIndic.ToString()
                        , cbcReport.DocSpec.DocRefId
                        , cbcReport.DocSpec.CorrMessageRefId
                        , cbcReport.DocSpec.CorrDocRefId
                        , cbcReport.ResCountryCode.ToString()
                        , messageSpecId
                        , data
                        , cbcBodyId
                        );

                    if (cbcReportId != 0)
                    {
                        if (cbcReport.Summary != null)
                        {
                            DBWriteManager.Save_X_Summaries(0M,
                                cbcReportId
                                , cbcReport.Summary.ProfitOrLoss.Value
                                , cbcReport.Summary.TaxPaid.Value
                                , cbcReport.Summary.TaxAccrued.Value
                                , cbcReport.Summary.Capital.Value
                                , cbcReport.Summary.Earnings.Value
                                , cbcReport.Summary.NbEmployees
                                , cbcReport.Summary.Assets.Value
                                , cbcReport.Summary.Revenues.Unrelated.Value
                                , cbcReport.Summary.Revenues.Related.Value
                                , cbcReport.Summary.Revenues.Total.Value);
                        }

                        if (cbcReport.ConstEntities != null)
                        {
                            foreach (var constEntity in cbcReport.ConstEntities)
                            {
                                var constEntityId = DBWriteManager.Save_X_ConstEntities(
                                    0M
                                    , cbcReportId
                                    , constEntity.ConstEntity.TIN.Value
                                    , constEntity.IncorpCountryCode.ToString()
                                    , constEntity.OtherEntityInfo
                                    );

                                if (constEntityId != 0)
                                {
                                    if (constEntity.ConstEntity.ResCountryCode != null)
                                    {
                                        foreach (var code in constEntity.ConstEntity.ResCountryCode)
                                        {
                                            DBWriteManager.Save_X_ConstEntities_ConstEntity_ResCountryCode(0M,
                                                code.ToString(),
                                                constEntityId);
                                        }
                                    }

                                    if (constEntity.ConstEntity.IN != null)
                                    {
                                        foreach (var _in in constEntity.ConstEntity.IN)
                                        {
                                            DBWriteManager.Save_X_ConstEntities_ConstEntity_Identification_Numbers(0M,
                                                _in.Value,
                                                _in.issuedBySpecified ? _in.issuedBy.ToString() : null, _in.INType
                                                , constEntityId);
                                        }
                                    }
                                    if (constEntity.ConstEntity.Name != null)
                                    {
                                        foreach (var name in constEntity.ConstEntity.Name)
                                        {
                                            DBWriteManager.Save_X_ConstEntities_ConstEntity_Names(0M, name.Value,
                                                constEntityId);
                                        }
                                    }

                                    if (constEntity.BizActivities != null)
                                    {
                                        foreach (var activity in constEntity.BizActivities)
                                        {
                                            DBWriteManager.Save_X_ConstEntities_BizActivities(0M, activity.ToString(),
                                                constEntityId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (cbcBodyType.AdditionalInfo != null)
            {
                foreach (var addInfo in cbcBodyType.AdditionalInfo)
                {
                    var additionalInfoId = 0M;
                    additionalInfoId = DBWriteManager.Save_X_AdditionalInfo(
                        additionalInfoId
                        , addInfo.DocSpec.DocTypeIndic.ToString()
                        , addInfo.DocSpec.DocRefId
                        , addInfo.DocSpec.CorrMessageRefId
                        , addInfo.DocSpec.CorrDocRefId
                        , addInfo.OtherInfo
                        , messageSpecId
                        );
                    if (additionalInfoId > 0)
                    {
                        if (addInfo.ResCountryCode != null)
                        {
                            foreach (var countryCodeType in addInfo.ResCountryCode)
                            {
                                DBWriteManager.X_AdditionalInfo_ResCountryCode(
                                    0M
                                    , countryCodeType.ToString()
                                    , additionalInfoId
                                    );
                            }
                        }
                        if (addInfo.SummaryRef != null)
                        {
                            foreach (var cbcSummary in addInfo.SummaryRef)
                            {
                                DBWriteManager.Save_X_AdditionalInfo_SummaryRef(
                                    0M
                                    , cbcSummary.ToString()
                                    , additionalInfoId
                                    );
                            }
                        }
                    }
                }
            }
        }
        return messageSpecId;
    }

    private void ValidateAdditionalInfo(CorrectableAdditionalInfo_Type info, string transmittingCountry, CBC_OECD cbcMessage)
    {
        var additionalInfoDocrefid = info.DocSpec.DocRefId;
        var additionalInfoCorrDocrefid = info.DocSpec.CorrDocRefId;
        var additionalInfoDocTypeIndic = info.DocSpec.DocTypeIndic;
        var messageRefId = cbcMessage.MessageSpec.MessageRefId;

        var additionalInfoDocrefidExists = DBReadManager.DocRefIdAlreadyExists(additionalInfoDocrefid, transmittingCountry, messageRefId) > 0;

        if (!additionalInfoDocrefidExists)
        {
            var isFormaValid = IsCorrectFormatDocRefId(additionalInfoDocrefid,
                cbcMessage.MessageSpec.TransmittingCountry.ToString(),
                cbcMessage.MessageSpec.ReportingPeriod.Year);

            if (!isFormaValid)
            {
                if (!_incorrectFormttedDocRefId.Contains(additionalInfoDocrefid) && !string.IsNullOrEmpty(additionalInfoDocrefid))
                {
                    _incorrectFormttedDocRefId.Add(additionalInfoDocrefid);
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(additionalInfoDocrefid))
            {
                if (!_duplicateDocRefId.Contains(additionalInfoDocrefid) && (info.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD0) && (info.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD10))
                {
                    _duplicateDocRefId.Add(additionalInfoDocrefid);
                }
            }
        }

        /** ########################### ERROR 80004################################*/

        CorrDocRefIdForNewData(additionalInfoDocTypeIndic, additionalInfoCorrDocrefid, additionalInfoDocrefid);

        /** ########################### ERROR 80005 ################################*/
        MissingCorrDocRefId(additionalInfoDocTypeIndic, additionalInfoCorrDocrefid, additionalInfoDocrefid);

        /** ########################### ERROR 80006 ################################*/

        if (!string.IsNullOrEmpty(info.DocSpec.CorrMessageRefId))
        {
            _forbiddenCorrMessageId.Add(additionalInfoDocrefid);
        }

        /** ################################## ERROR 80011 ########################################*/

        var additionalInfoCorrDocRefId = info.DocSpec.CorrDocRefId;

        if (_allCorrDocRefId.Contains(additionalInfoCorrDocRefId))
        {
            if (!string.IsNullOrEmpty(additionalInfoCorrDocRefId))
            {
                if (!_duplicateCorrDocRefId.Contains(additionalInfoCorrDocRefId))
                {
                    _duplicateCorrDocRefId.Add(additionalInfoCorrDocRefId);
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(additionalInfoCorrDocRefId))
            {
                _allCorrDocRefId.Add(additionalInfoCorrDocRefId);
            }
        }
        /**#################################### ERROR 80002#########################**/

        if (!string.IsNullOrEmpty(additionalInfoCorrDocrefid))
        {
            var corDocRefIdExits = DBReadManager.CheckIfDocRefIdExistsForGivenCorDocRefId(
                additionalInfoCorrDocrefid
                , transmittingCountry
                , messageRefId
                );

            if (corDocRefIdExits == 0)
            {
                _unKnownCorDocRefId.Add(additionalInfoCorrDocrefid);
            }
        }
    }

    private void ValdateCbcReport(CorrectableCbcReport_Type cbcReport, string transmittingCountry, CBC_OECD cbcMessage)
    {
        var reprtDocrefid = cbcReport.DocSpec.DocRefId;
        var messageRefId = cbcMessage.MessageSpec.MessageRefId;
        var cbcReportCorrDocrefid = cbcReport.DocSpec.CorrDocRefId;
        var cbcReportDocTypeIndic = cbcReport.DocSpec.DocTypeIndic;

        var cbcBodyDocRefIdExists = DBReadManager.DocRefIdAlreadyExists(reprtDocrefid, transmittingCountry, messageRefId) > 0;

        if (!cbcBodyDocRefIdExists)
        {
            var isFormarValid = IsCorrectFormatDocRefId(
                reprtDocrefid,
                transmittingCountry,
                cbcMessage.MessageSpec.ReportingPeriod.Year
                );

            if (!isFormarValid)
            {
                if (!_incorrectFormttedDocRefId.Contains(reprtDocrefid) && !string.IsNullOrEmpty(reprtDocrefid))
                {
                    _incorrectFormttedDocRefId.Add(reprtDocrefid);
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(reprtDocrefid))
            {
                if (!_duplicateDocRefId.Contains(reprtDocrefid) &&
                    (cbcReport.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD0) &&
                    (cbcReport.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD10))
                {
                    _duplicateDocRefId.Add(reprtDocrefid);
                }
            }
        }

        /** ########################### ERROR 80004################################*/

        CorrDocRefIdForNewData(cbcReportDocTypeIndic, cbcReportCorrDocrefid, reprtDocrefid);

        _messageTypeIndicators.Add(cbcReportDocTypeIndic);

        /** ########################### ERROR 80005 ################################*/
        MissingCorrDocRefId(cbcReportDocTypeIndic, cbcReportCorrDocrefid, reprtDocrefid);

        /** ########################### ERROR 80006 ################################*/

        if (!string.IsNullOrEmpty(cbcReport.DocSpec.CorrMessageRefId))
        {
            _forbiddenCorrMessageId.Add(reprtDocrefid);
        }

        /** ################################## ERROR 80011 ########################################*/

        if (_allCorrDocRefId.Contains(cbcReportCorrDocrefid))
        {
            if (!string.IsNullOrEmpty(cbcReportCorrDocrefid))
            {
                if (!_duplicateCorrDocRefId.Contains(cbcReportCorrDocrefid))
                {
                    _duplicateCorrDocRefId.Add(cbcReportCorrDocrefid);
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(cbcReportCorrDocrefid))
            {
                _allCorrDocRefId.Add(cbcReportCorrDocrefid);
            }
        }

        /**#################################### ERROR 80002#########################**/

        if (!string.IsNullOrEmpty(cbcReportCorrDocrefid))
        {
            var corDocRefIdExits = DBReadManager.CheckIfDocRefIdExistsForGivenCorDocRefId(
                cbcReportCorrDocrefid
                , transmittingCountry
                , messageRefId
                );

            if (corDocRefIdExits == 0)
            {
                _unKnownCorDocRefId.Add(cbcReportCorrDocrefid);
            }
        }
    }

    private void ValidateReportingEntity(CbcBody_Type cbcBodyType, string transmittingCountry, CBC_OECD cbcMessage)
    {
        cbcMessage.MessageSpec.MessageTypeIndic = CbcMessageTypeIndic_EnumType.CBC401;

        var reportingEntityDocrefid = cbcBodyType.ReportingEntity.DocSpec.DocRefId;
        var reportingEntityCorrDocrefid = cbcBodyType.ReportingEntity.DocSpec.CorrDocRefId;
        var reportingEntityDocTypeIndic = cbcBodyType.ReportingEntity.DocSpec.DocTypeIndic;
        var messageRefId = cbcMessage.MessageSpec.MessageRefId;

        var mesasgeSpecDocRefIdExists = DBReadManager.DocRefIdAlreadyExists(reportingEntityDocrefid, transmittingCountry, messageRefId) > 0;

        if (!mesasgeSpecDocRefIdExists)
        {
            var isFormatValid = IsCorrectFormatDocRefId
                (
                    reportingEntityDocrefid
                    , transmittingCountry,
                    cbcMessage.MessageSpec.ReportingPeriod.Year
                );

            if (!isFormatValid)
            {
                if (!_incorrectFormttedDocRefId.Contains(reportingEntityDocrefid) &&
                    !string.IsNullOrEmpty(reportingEntityDocrefid))
                {
                    _incorrectFormttedDocRefId.Add(reportingEntityDocrefid);
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(reportingEntityDocrefid))
            {
                if (!_duplicateDocRefId.Contains(reportingEntityDocrefid) &&
                    (cbcBodyType.ReportingEntity.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD0) &&
                    (cbcBodyType.ReportingEntity.DocSpec.DocTypeIndic != OECDDocTypeIndic_EnumType.OECD10))
                {
                    _duplicateDocRefId.Add(reportingEntityDocrefid);
                }
            }
        }

        /** ########################### ERROR 80004 ################################ */

        CorrDocRefIdForNewData(reportingEntityDocTypeIndic, reportingEntityCorrDocrefid, reportingEntityDocrefid);

        /** ########################### ERROR 80005 ################################*/
        MissingCorrDocRefId(reportingEntityDocTypeIndic, reportingEntityCorrDocrefid, reportingEntityDocrefid);

        /** ########################### ERROR 80006 ################################*/

        if (!string.IsNullOrEmpty(cbcBodyType.ReportingEntity.DocSpec.CorrMessageRefId))
        {
            _forbiddenCorrMessageId.Add(reportingEntityDocrefid);
        }

        /** ################################## ERROR 80011 ########################################*/

        if (!string.IsNullOrEmpty(reportingEntityCorrDocrefid))
        {
            if (_allCorrDocRefId.Contains(reportingEntityCorrDocrefid))
            {
                if (!_duplicateCorrDocRefId.Contains(reportingEntityCorrDocrefid))
                {
                    _duplicateCorrDocRefId.Add(reportingEntityCorrDocrefid);
                }
            }
            else
            {
                _allCorrDocRefId.Add(reportingEntityCorrDocrefid);
            }
        }

        /**#################################### ERROR 80002#########################**/

        if (!string.IsNullOrEmpty(reportingEntityCorrDocrefid))
        {
            var corDocRefIdExits = DBReadManager.CheckIfDocRefIdExistsForGivenCorDocRefId(
                reportingEntityCorrDocrefid
                , transmittingCountry
                , messageRefId
                );

            if (corDocRefIdExits == 0)
            {
                _unKnownCorDocRefId.Add(reportingEntityCorrDocrefid);
            }
        }

        //if (_duplicateCorrMessageId.Contains(reportingEntityCorrDocrefid))
        //{
        //    _duplicateCorrMessageId.Add(reportingEntityDocrefid);
        //}
    }

    private void MissingCorrDocRefId(OECDDocTypeIndic_EnumType reportingEntityDocTypeIndic, string reportingEntityCorrDocrefid, string reportingEntityDocrefid)
    {
        if (
            reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD2 ||
            reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD3 ||
            reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD12 ||
            reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD12
            )
        {
            if (string.IsNullOrEmpty(reportingEntityCorrDocrefid))
            {
                if (!_correctedDataWithNoCorrDocRefId.Contains(reportingEntityDocrefid))
                {
                    _correctedDataWithNoCorrDocRefId.Add(reportingEntityDocrefid);
                }
            }
        }
    }

    private void CorrDocRefIdForNewData(OECDDocTypeIndic_EnumType reportingEntityDocTypeIndic, string reportingEntityCorrDocrefid, string reportingEntityDocrefid)
    {
        if (!string.IsNullOrEmpty(reportingEntityCorrDocrefid))
        {
            if (reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD1 ||
                reportingEntityDocTypeIndic == OECDDocTypeIndic_EnumType.OECD11)
            {
                if (!_newDataWithCorrDocRefId.Contains(reportingEntityDocrefid))
                {
                    _newDataWithCorrDocRefId.Add(reportingEntityDocrefid);
                }
            }
        }
    }

    private void AddRecordErrorInValidations(string code, string errorMessage, List<string> listOfDocRefIds)
    {
        _listOfRecordErrorType.Add(
            new RecordError_Type
            {
                Code = code,
                Details = new ErrorDetail_Type
                {
                    Language = LanguageCode_Type.EN,
                    Value = errorMessage,
                    LanguageSpecified = true
                },
                DocRefIDInError = listOfDocRefIds.ToArray()
            });
    }

    private void AddFileErrorInValidations(string code, string errorMessage)
    {
        _listOfFileErrorType.Add(
            new FileError_Type
            {
                Code = code,
                Details = new ErrorDetail_Type()
                {
                    Language = LanguageCode_Type.EN,
                    LanguageSpecified = true,
                    Value = errorMessage
                }
            });
    }

    private static bool IsCorrectFormatDocRefId(string docRefId, string sc, int fiscalYear)
    {
        if (docRefId.Length <= 7)
        {
            return false;
        }
        if (docRefId.IndexOf("-", StringComparison.InvariantCultureIgnoreCase) == -1)
        {
            return false;
        }

        int reportingYear;
        var part1 = docRefId.Substring(0, 6);
        var countryCode = part1.Substring(0, 2);
        var year = part1.Substring(2, 4);

        if (!int.TryParse(year, out reportingYear))
        {
            return
            false;
        }
        if (countryCode != sc)
        {
            return false;
        }
        if (reportingYear != fiscalYear)
        {
            return false;
        }
        return true;
    }

    private static bool IsCorrectFormatMessageRefId(string docRefId, string sc, int fiscalYear)
    {
        if (docRefId.Length <= 7)
        {
            return false;
        }

        var indexfdash = docRefId.IndexOf("-", StringComparison.InvariantCultureIgnoreCase);

        if (indexfdash == -1)
        {
            return false;
        }

        int reportingYear;
        var part1 = docRefId.Substring(0, 6);
        var countryCode = part1.Substring(0, 2);
        var year = part1.Substring(2, 4);

        if (!int.TryParse(year, out reportingYear))
        {
            return
            false;
        }
        if (countryCode != sc)
        {
            return false;
        }
        if (reportingYear != fiscalYear)
        {
            return false;
        }
        return true;
    }

    private string CreateStatusMessage(CbCStatusMessage_OECD cbCStatusMessageOecd, CBC_OECD_MessageSpec incomingCbcOecdMessageSpec, string messageRefId,
        FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType metaDataObject, FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType newCtsSenderFileMetadataType, bool fileRejected, string uid)
    {
        cbCStatusMessageOecd.MessageSpec.TransmittingCountry = FDRService.CbcStatusMessage.CountryCode_Type.ZA;
        cbCStatusMessageOecd.MessageSpec.ReceivingCountry =
            (FDRService.CbcStatusMessage.CountryCode_Type)Enum.Parse(typeof(FDRService.CbcStatusMessage.CountryCode_Type), incomingCbcOecdMessageSpec.TransmittingCountry);

        cbCStatusMessageOecd.MessageSpec.MessageType = MessageType_EnumType.CbCMessageStatus;
        cbCStatusMessageOecd.MessageSpec.MessageTypeIndic = CbCMessageTypeIndic_EnumType.CbCMessageStatus;
        cbCStatusMessageOecd.MessageSpec.Warning = "None";
        cbCStatusMessageOecd.MessageSpec.Contact = "";
        cbCStatusMessageOecd.MessageSpec.MessageRefId = messageRefId;
        cbCStatusMessageOecd.MessageSpec.Timestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        cbCStatusMessageOecd.MessageSpec.MessageTypeIndicSpecified = true;

        cbCStatusMessageOecd.CbCStatusMessage.OriginalMessage = new OriginalMessage_Type
        {
            OriginalMessageRefID = incomingCbcOecdMessageSpec.MessageRefId
        };

        cbCStatusMessageOecd.CbCStatusMessage.ValidationErrors = new ValidationErrors_Type();
        if (_listOfFileErrorType.Any())
        {
            cbCStatusMessageOecd.CbCStatusMessage.ValidationErrors.FileError = _listOfFileErrorType.ToArray();
        }
        if (_listOfRecordErrorType.Any())
        {
            cbCStatusMessageOecd.CbCStatusMessage.ValidationErrors.RecordError = _listOfRecordErrorType.ToArray();
        }
        cbCStatusMessageOecd.CbCStatusMessage.ValidationResult = new ValidationResult_Type
        {
            Status = fileRejected ? FileAcceptanceStatus_EnumType.Rejected : FileAcceptanceStatus_EnumType.Accepted,
            ValidatedBy = new[]
            {"ZA FDR DAC4 Validation Module, v1.0.1(Based on AEOI DAC4 FS v1.08, TS v1.08, XSD v1.0.1)"}
        };

        var newCtsSenderFileMetadataTypeXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(newCtsSenderFileMetadataType);
        var cbCStatusMessageOecdXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(cbCStatusMessageOecd);

        var zipFilename = incomingCbcOecdMessageSpec.TransmittingCountry + "_FDR_" + DateTime.Now.ToString("yyyyMMddTHHmmssZ") + ".zip";
        // var zipFolder = Path.Combine(Request.PhysicalApplicationPath, SARSDataSettings.Settings.TempFolder,DateTime.Now.ToFileTime().ToString());
        var outgoingStatusDirectoryPath = Path.Combine(Request.PhysicalApplicationPath, SARSDataSettings.Settings.TempFolder, "Outgoing-status", DateTime.Now.ToFileTime().ToString());
        var outgoingStatusDirectoryZipPath = Path.Combine(Request.PhysicalApplicationPath, SARSDataSettings.Settings.TempFolder, "Outgoing-status", "ZIP", DateTime.Now.ToFileTime().ToString());

        if (!Directory.Exists(outgoingStatusDirectoryPath))
        {
            Directory.CreateDirectory(outgoingStatusDirectoryPath);
        }

        if (!Directory.Exists(outgoingStatusDirectoryZipPath))
        {
            Directory.CreateDirectory(outgoingStatusDirectoryZipPath);
        }

        var metadataFile = Path.Combine(outgoingStatusDirectoryPath, "ZA_CBCStatus_Metadata.xml");
        var metadataStatussFile = Path.Combine(outgoingStatusDirectoryPath, "ZA_CBCStatus_Payload.xml");
        var fullZipFileName = Path.Combine(outgoingStatusDirectoryZipPath, zipFilename);
        File.WriteAllText(metadataFile, newCtsSenderFileMetadataTypeXml);
        File.WriteAllText(metadataStatussFile, cbCStatusMessageOecdXml);
        FileCompression.ZipDirectoryContents(outgoingStatusDirectoryPath, fullZipFileName, FileCompression.CompressionMethod.CompressSlowAndProper);
        var zippedText = File.ReadAllBytes(fullZipFileName);
        //Directory.Delete(outgoingStatusDirectoryPath);
        //Directory.Delete(outgoingStatusDirectoryZipPath);
        var content = Convert.ToBase64String(zippedText);
        var fullMessage = CreateStatusMessageXml(incomingCbcOecdMessageSpec, "ZA_CBCStatus_Payload.zip", content, uid);
        return fullMessage;
    }

    private static FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType CreateNewMetaData(FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType metaDataObject, string ctsSenderId)
    {
        var newCtsSenderFileMetadataType = new FDRService.CbcXML.CTSSenderFileMetadata.CTSSenderFileMetadataType
        {
            BinaryEncodingSchemeCdSpecified = true,
            BinaryEncodingSchemeCd = FDRService.CbcXML.CTSSenderFileMetadata.BinaryEncodingSchemeCdType.NONE,

            FileCreateTsSpecified = true,
            FileCreateTs = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),// Convert.ToDateTime( DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")),

            FileFormatCdSpecified = true,
            FileFormatCd = FDRService.CbcXML.CTSSenderFileMetadata.FileFormatCdType.XML,

            FileRevisionIndSpecified = true,
            FileRevisionInd = false,

            CTSSenderCountryCd = FDRService.CbcXML.CTSSenderFileMetadata.CountryCode_Type.ZA,
            CTSReceiverCountryCd = metaDataObject.CTSSenderCountryCd,
            CTSCommunicationTypeCd = FDRService.CbcXML.CTSSenderFileMetadata.CTSCommunicationTypeCdType.CBCStatus,
            OriginalCTSTransmissionId = metaDataObject.OriginalCTSTransmissionId,
            SenderFileId = ctsSenderId,
            SenderContactEmailAddressTxt = "",
            TaxYear = metaDataObject.TaxYear
        };
        return newCtsSenderFileMetadataType;
    }

    private static void RespondWithError(ApplicationInformationStructure applicationInformationStructure, string mesage,
        string headerXml, string countryByCountryReportManagementRequestXml, IncomingMessage incomingMessage,
        IESBMessagingService client)
    {
        applicationInformationStructure.ApplicationInformationResult[0] =
            new ApplicationInformationStructureApplicationInformationResult
            {
                Code = "9999",
                Description = mesage,
                MessageType = MessageTypeEnum.ERROR
            };

        var messageToSend = CreateAcknowlegementXml(headerXml, applicationInformationStructure, countryByCountryReportManagementRequestXml);
        SendMessage(messageToSend, incomingMessage, client);
    }

    private static void RespondWithSuccess(ApplicationInformationStructure applicationInformationStructure, string mesage, string headerXml, string countryByCountryReportManagementRequestXml, IncomingMessage incomingMessage, ESBMessagingServiceClient client)
    {
        applicationInformationStructure.ApplicationInformationResult[0] =
            new ApplicationInformationStructureApplicationInformationResult
            {
                Code = "0000",
                Description = mesage,
                MessageType = MessageTypeEnum.INFORMATION
            };

        var messageToSend = CreateAcknowlegementXml(headerXml, applicationInformationStructure, countryByCountryReportManagementRequestXml);
        SendMessage(messageToSend, incomingMessage, client);
    }

    private static void SendMessage(string messageToSend, IncomingMessage incomingMessage, IESBMessagingService client)
    {
        var contract = new WriteMessageData
        {
            ConfigurationId = new Guid(Configurations.RECEIVECBCREPORT_CHANNELID),
            Message = messageToSend,
            CorrelationId = incomingMessage.MessageId
        };
        //var sent = client.SendAndForget(contract);
    }

    private void SendStatusMessage(string messageToSend, IncomingMessage incomingMessage, IESBMessagingService client)
    {
        var contract = new WriteMessageData
        {
            ConfigurationId = new Guid(Configurations.UPDATECBCREPORTSTATUS_CHANNELID),
            Message = messageToSend,
            CorrelationId = incomingMessage.MessageId
        };
        //var sent = client.SendAndForget(contract);
    }

    private static string CreateAcknowlegementXml(string header, ApplicationInformationStructure appInfo, string body)
    {
        var applicationInformationStructureXml = XmlObjectSerializer.GetXmlWithNoDeclaration(appInfo, "fdri", "http://www.egovernment.gov.za/GMD/ApplicationInformation/xml/schemas/version/3.1");

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

    private static string CreateStatusMessageXml(CBC_OECD_MessageSpec messageSpec, string fileName, string fileContent, string uid)
    {
        CountryByCountryReportManagementRequestStructure structure =
            new CountryByCountryReportManagementRequestStructure
            {
                Destination = messageSpec.TransmittingCountry,
                RequestOperation = CountryByCountryReportManagementRequestStructureRequestOperation.UPDATE_REPORT_STATUS,
                Filename = fileName,
                RequestOperationSpecified = true,
                FileContent = fileContent
            };

        var xml = XmlObjectSerializer.GetXmlWithNoDeclaration(structure, "cbcMgt", "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/xml/schemas/version/1.2");

        var xmlBuilder = new StringBuilder();
        xmlBuilder.Append(
            "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> " +
            "<soap12:Envelope xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" " +
            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
            "xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">"

            );

        var headerInfo = new MessageIdentificationStructure
        {
            domain = "CBC Financial Data Reporting",
            originatingChannelID = "FDR",
            externalReferenceID = string.Format("{0}_CBCStatus_{1}Z.zip", messageSpec.ReceivingCountry, DateTime.Now.ToString("yyyyMMddTHHmmss")),
            priority = 9,
            channelID = "FDR",
            applicationID = "IssueCBCReportStatus",
            versionNo = (float)1.2,
            activityName = "Issue CBC Report Status",
            messageSeqNo = DBReadManager.GetNextMessageId(uid),
            messageTimeStamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
            universalUniqueID = uid
        };

        var headerXml = XmlObjectSerializer.GetXmlWithNoDeclaration(headerInfo, "esb", "http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1");

        xmlBuilder.AppendFormat(
            "<soapenv:Header xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\">{0}</soapenv:Header>" +
            " <soap12:Body>" +
            "{1}" +
            "</soap12:Body>" +
            "</soap12:Envelope>",
            headerXml,
            xml
            );
        return FdrCommon.FormatXml(xmlBuilder.ToString());
    }

    private static CBC_OECD_MessageSpec GetCBC_OECD_MessageSpec(string xml)
    {
        var ds = new RecordSet();

        ds.ReadXml(new StringReader(xml));

        if (ds.HasRows && ds.Tables.Contains("MessageSpec"))
        {
            var messageSpec = ds.Tables["MessageSpec"];

            if (messageSpec != null)
            {
                CBC_OECD_MessageSpec cbcOecdMessage = new CBC_OECD_MessageSpec
                {
                    MessageRefId = messageSpec.Rows[0]["MessageRefId"].ToString(),
                    ReceivingCountry =
                        messageSpec.Columns.Contains("ReceivingCountry")
                            ? messageSpec.Rows[0]["ReceivingCountry"].ToString()
                            : "",
                    ReportingPeriod = messageSpec.Rows[0]["ReportingPeriod"].ToString(),
                    SendingEntityIN =
                        messageSpec.Columns.Contains("SendingEntityIN")
                            ? messageSpec.Rows[0]["SendingEntityIN"].ToString()
                            : null,
                    TransmittingCountry = messageSpec.Rows[0]["TransmittingCountry"].ToString()
                };

                if (string.IsNullOrEmpty(cbcOecdMessage.ReceivingCountry))
                {
                    var receivingCountry = ds.Tables["ReceivingCountry"];

                    if (receivingCountry != null)
                    {
                        var rows = ds.SelectRows("ReceivingCountry", "ReceivingCountry_Text", "ZA");

                        if (rows.Length > 0)
                        {
                            cbcOecdMessage.ReceivingCountry = "ZA";
                        }
                    }
                }
                return cbcOecdMessage;
            }
        }
        return null;
    }
}
public class CBC_OECD_MessageSpec
{
    public string SendingEntityIN { get; set; }
    public string TransmittingCountry { get; set; }
    public string ReceivingCountry { get; set; }
    public string MessageRefId { get; set; }
    public string ReportingPeriod { get; set; }
}