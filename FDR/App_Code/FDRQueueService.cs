using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Services;
using System.Xml;
using FDR.DataLayer;
using Sars.ESBSchema.Letters.SysGen;
using Sars.Systems.Correspondence;
using Sars.Systems.Frd.Registration;
using Sars.Systems.Serialization;
using LanguageType = Sars.Systems.Correspondence.LanguageType;
using TypeOfTaxType = Sars.Systems.Correspondence.TypeOfTaxType;

[WebService(Namespace = "http://www.sars.gov.za/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class FDRQueueService : WebService{
    public FDRQueueService(){
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void SendSms(string cellNo, string taxrefNo, string message, int year)
    {
        if (!Configurations.SendToSms)
            return;
        if (Convert.ToInt64(cellNo) == 0)
        {
            //MessageBox.Show(string.Format("SMS will not be sent to this number {0}", cellNo));
            return;

        }
        var correspondance = new CorrespondenceManagementRequestStructure
        {
            RequestOperation = RequestOperation.ISSUE_CORRESPONDENCE,
            OutChannel = CorrespondenceManagementRequestStructureOutChannel.SMS,
            SMS = new CorrespondenceManagementRequestStructureSMS
            {
                CellularNos = new[] {cellNo},
                Message = message
            }
        };

        var messageId = Guid.NewGuid();
        var corXml = XmlObjectSerializer.GetXmlWithNoDeclaration(correspondance);
        var soapMessage =FdrCommon.CreateSoapSms(corXml, taxrefNo, messageId.ToString());

        soapMessage = FdrCommon.FormatXml(soapMessage);
        var queueInfo = new QueueInfo
        {
            Manager = MQConfigurationSettings.CorrespondenceOutQManagerName,
            Channel = MQConfigurationSettings.CorrespondenceOutQChannelName,
            UseManagerName = false,
            Port = MQConfigurationSettings.CorrespondenceOutPortNumber,
            HostName = MQConfigurationSettings.CorrespondenceOutHostName,
            QueueName = MQConfigurationSettings.CorrespondenceOutQName,
            CorrelationId = messageId.ToString(),
            Message = soapMessage
        };
        FdrMessaging.SendMessageToQueue(queueInfo);
        DBWriteManager.SaveCorrespondanceTrail(messageId.ToString(), taxrefNo, year, soapMessage);
    }
    public void SendEmail(string messageBody, string subject, string emailAddress, byte[] attachement, string fileName, string taxrefNo, int year)
    {
        if(!Configurations.SendToEmails)
            return;
        var correspondance = new CorrespondenceManagementRequestStructure
        {
            RequestOperation = RequestOperation.ISSUE_CORRESPONDENCE,
            OutChannel = CorrespondenceManagementRequestStructureOutChannel.EMAIL,
            Email = new CorrespondenceManagementRequestStructureEmail
            {
                ToAddresses = new[] {emailAddress},
                Body = messageBody,
                Subject = subject,
                Attachments = attachement == null?null: new CorrespondenceManagementRequestStructureEmailAttachment[1]
                {
                    new CorrespondenceManagementRequestStructureEmailAttachment
                    {
                        Content = attachement,//FdrCommon.GetRejectionLetter("1"), 
                        Type = DocumentType.PDF,
                        Filename = string.Format("{0}.pdf", fileName) 
                    }
                }
            }
        };
        var messageId = Guid.NewGuid();
        var message = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(correspondance);
        var soapMessage = FdrCommon.CreateSoapEmail(message, emailAddress, messageId.ToString());

        soapMessage = FdrCommon.FormatXml(soapMessage);
        var queueInfo = new QueueInfo
        {
            Manager = MQConfigurationSettings.CorrespondenceOutQManagerName,
            Channel = MQConfigurationSettings.CorrespondenceOutQChannelName,
            UseManagerName = false,
            Port = MQConfigurationSettings.CorrespondenceOutPortNumber,
            HostName = MQConfigurationSettings.CorrespondenceOutHostName,
            QueueName = MQConfigurationSettings.CorrespondenceOutQName,
            CorrelationId = messageId.ToString(),
            Message = soapMessage
        };

        FdrMessaging.SendMessageToQueue(queueInfo);
        DBWriteManager.SaveCorrespondanceTrail(messageId.ToString(), taxrefNo, year, soapMessage);
    }
    public void SendLetter(decimal submissionId, string taxrefNo, int year, string letterContent, string letterHead, bool isAcceptance, string userSid)
    {
        if (!Configurations.SendToEfiling){
            return;
        }
        var currentUser = Sars.Systems.Security.ADUser.CurrentUser;
        if (currentUser == null){
            return;
        }
        var correspondance = new CorrespondenceManagementRequestStructure{
            OutChannel = CorrespondenceManagementRequestStructureOutChannel.EFL,
            RequestOperation = RequestOperation.ISSUE_CORRESPONDENCE,
            TaxRef = new TaxRefStructure{
                TaxRefNo = taxrefNo,
                TypeOfTax = TypeOfTaxType.INCOME_TAX
            },
            TaxYear = year.ToString()
        };
        RegistrationQueryDetails registration = null;
        try{
            registration = new RegistrationQueryDetails();
            registration.LookUpRegistrationDetails(taxrefNo);
            if (!registration.DetailsFound){
                MessageBox.Show(registration.Message);
                return;
            }
        }
        catch (Exception){
        }
        if (registration == null){
            return;
        }

        if (!registration.DetailsFound){
            MessageBox.Show(string.Format("Letter could not be sent because FDR could not get registration details for the number {0} \n U3TM reason : {1}", taxrefNo, registration.Message));
            return;
        }
        if (registration.PreferredAddress == null){
            MessageBox.Show("Letter could not be sent because FDR could not get registration address.");
            return;
        }
        var sarsSysgenltr = new SARS_SYSGENLTR{
            ADDRESSEE_DETAILS = new SARS_SYSGENLTRADDRESSEE_DETAILS{
                //ADDRESS_FIELD_F001 = "What is Here",
                //ADDRESS_FIELD_F002 = "Pretoria",
                //ADDRESS_FIELD_F003 = "299 Bronkhorst street",
                //ADDRESS_FIELD_F004 = "QA TESTING",
                //ADDRESS_FIELD_F005 = "Brooklyn",
                //ADDRESS_FIELD_F006 = "2000",
                //ADDRESS_FIELD_F007 = ""

                ADDRESS_FIELD_F001 = registration.RegistrationName,
                ADDRESS_FIELD_F002 = "",
                ADDRESS_FIELD_F003 = string.Format("{0} {1}", registration.PreferredAddress.StreetNo, registration.PreferredAddress.StreetName),
                ADDRESS_FIELD_F004 = registration.PreferredAddress.Suburb,
                ADDRESS_FIELD_F005 = registration.PreferredAddress.City,
                ADDRESS_FIELD_F006 = registration.PreferredAddress.PostalCode,
                ADDRESS_FIELD_F007 = ""
            },
            FORM_INFORMATION = new FORM_INFORMATION{
                FORM_ID = "CBC_LET_GEN",
                GUID = new GlobalUniqueIDStructure {UniqueIdentifier = Guid.NewGuid().ToString()},
                TIMESTAMP = DateTime.Now,
                TAX_REF_NO = taxrefNo,
                TAX_YEAR = year.ToString(),
                LANGUAGE = "english",
                FORM_TYPE =  isAcceptance ? "CBCACC" : "CBCREJ",
                CASE_NUMBER = string.Empty
            },
            CONTACT_DETAILS = new SARS_SYSGENLTRCONTACT_DETAILS{
                HeaderLabel = "Contact Detail",
                ADDRESS_CD01 = "SARS Head Office",
                TEL_NO_CD02 = currentUser.Telephone,
                WEB_ADDRESS_CD03 = "SARS online: www.sars.gov.za",
                TOLL_FREE_NO_CD04 = "",
                FAX_NO_CD05 = "",
                EMAIL_ADDRESS_CD06 = currentUser.Name
            },
            LETTER_CONTENT = new SARS_SYSGENLTRLETTER_CONTENT{
                CONTENT = letterContent.Replace(Environment.NewLine, "")
            },
            DETAILS = new SARS_SYSGENLTRDETAILS{
                TAXREF_NO = new SARS_SYSGENLTRDETAILSTAXREF_NO{
                    TAXREF_FIELD_F004 = taxrefNo,
                    TAXREF_LABEL_L003 = "Tax Reference No.:"
                },
                DATE =  new SARS_SYSGENLTRDETAILSDATE{
                    DATE_LABEL_L008 = "Date:",
                    DATE_FIELD_F009 = DateTime.Now.ToString ("yyyy-MM-dd")
                },
                ENQUIRY_NOTE = new SARS_SYSGENLTRDETAILSENQUIRY_NOTE{
                    NOTE_LABEL_L005 = "Always quote this reference number when contacting SARS"
                }
            },
            LETTER_HEADER = new SARS_SYSGENLTRLETTER_HEADER
            {
                LETTER_HEADER_LABEL = letterHead
            },
            LETTER_NAME_HEADER = new SARS_SYSGENLTRLETTER_NAME_HEADER{
                LETTER_NAME = string.Empty //letterHead.ToUpper()
            }
            //,
            //Paragraphs = new string[2]
            //{
            //    "Paragraph 1",
            //    "Paragraph 2"
            //},
        };

        var letterDetails = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(sarsSysgenltr, "SARS", "http://www.sars.gov.za/forms/").Replace(Environment.NewLine, "");
        var letters = new List<CorrespondenceManagementRequestStructureLetter>{
            new CorrespondenceManagementRequestStructureLetter{
                Content = string.Format("<![CDATA[{0}]]>", letterDetails) ,
                TemplateDetails = new TemplateDetailsStructure{
                    Language = LanguageType.ENGLISH,
                    Name = "SARS_SysGenOutLet_RO_E_v2012.01.05"
                },
                Type = isAcceptance ? "CBCACC" : "CBCREJ"
            }
        };
        correspondance.Letters = letters.ToArray();
        var messageId = Guid.NewGuid();
        var corXml = XmlObjectSerializer.GetXmlWithNoDeclaration(correspondance);
        var soapMessage = FdrCommon.CreateSoapLetter(corXml, taxrefNo, messageId.ToString());
        soapMessage = FdrCommon.FormatXml(soapMessage).Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;lt;", "<").Replace("&amp;gt;", ">");
        var queueInfo = new QueueInfo{
            Manager = MQConfigurationSettings.CorrespondenceOutQManagerName,
            Channel = MQConfigurationSettings.CorrespondenceOutQChannelName,
            UseManagerName = false,
            Port = MQConfigurationSettings.CorrespondenceOutPortNumber,
            HostName = MQConfigurationSettings.CorrespondenceOutHostName,
            QueueName = MQConfigurationSettings.CorrespondenceOutQName,
            CorrelationId = messageId.ToString(),
            Message = soapMessage
        };
        FdrMessaging.SendMessageToQueue(queueInfo);
        DBWriteManager.SaveLetter(submissionId, soapMessage, taxrefNo, year, userSid);
        DBWriteManager.SaveCorrespondanceTrail(messageId.ToString(), taxrefNo, year);
    }
    [WebMethod]
    public string GetTaxpayerRegistrationData(string taxRefNo)
    {
        var reg = new RegistrationManagementRequestStructure
        {
            RequestOperation = RegistrationManagementRequestStructureRequestOperation.RETRIEVE_ENTITY_DETAILS,
            PartyIdentifiers = new[]
                {
                    new RegistrationManagementRequestStructurePartyIdentifier
                    {
                        IdentifierType = "REFERENCE_NO",
                        Value = taxRefNo
                    }
                }
        };
        var messageId = Guid.NewGuid();
        var corXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(reg);
        var soapMessage = FdrCommon. CreateSoapRegistrationQuery(corXml, taxRefNo, messageId.ToString());

        soapMessage = FdrCommon.FormatXml(soapMessage);

        var queueInfo = new QueueInfo
        {
            Manager = MQConfigurationSettings.RegistrationOutQManagerName,
            Channel = MQConfigurationSettings.RegistrationOutQChannelName,
            UseManagerName = false,
            Port = MQConfigurationSettings.RegistrationOutPortNumber,
            HostName = MQConfigurationSettings.RegistrationOutHostName,
            QueueName = MQConfigurationSettings.RegistrationOutQName,
            CorrelationId = messageId.ToString(),
            Message = soapMessage
        };
        FdrMessaging.SendMessageToQueue(queueInfo);

        return messageId.ToString();
    }
    [WebMethod]
    public XmlElement EnquireRegistration(string taxRefNo)
    {
        var reg = new RegistrationManagementRequestStructure
        {
            RequestOperation = RegistrationManagementRequestStructureRequestOperation.RETRIEVE_ENTITY_DETAILS,
            PartyIdentifiers = new[]
                {
                    new RegistrationManagementRequestStructurePartyIdentifier
                    {
                        IdentifierType = "REFERENCE_NO",
                        Value = taxRefNo
                    }
                }
        };
        var doc = new XmlDocument();
        var messageId = Guid.NewGuid();
        var corXml = XmlObjectSerializer.GetXmlWithNoDeclaration(reg);
        var soapMessage = FdrCommon.CreateSoapRegistrationQuery(corXml, taxRefNo, messageId.ToString());
        soapMessage = FdrCommon.FormatXml(soapMessage);
        var queueInfo = new QueueInfo{
            Manager = MQConfigurationSettings.RegistrationOutQManagerName,
            Channel = MQConfigurationSettings.RegistrationOutQChannelName,
            UseManagerName = false,
            Port = MQConfigurationSettings.RegistrationOutPortNumber,
            HostName = MQConfigurationSettings.RegistrationOutHostName,
            QueueName = MQConfigurationSettings.RegistrationOutQName,
            CorrelationId = messageId.ToString(),
            Message = soapMessage
        };
        var message = string.Empty;
        FdrMessaging.SendMessageToQueue(queueInfo);
        Thread.Sleep(1);
        var data = DBReadManager.GetResponse(messageId.ToString());
        var i = 1;
        while ( !data.HasRows && i <= Configurations.QueueResponseTime ){
            Thread.Sleep(1);
            i++;
            data = DBReadManager.GetResponse(messageId.ToString());
        }
        if ( !data.HasRows && i >= Configurations.QueueResponseTime ){
            doc.LoadXml(string.Format("<Errors><Error message=\"{0}\" /></Errors>", Configurations.QueueTimeoutMessage));
            return doc.DocumentElement;
        }
        if (string.IsNullOrEmpty(message)){
            if (Convert.ToInt32(data[0]["ReturnCode"]) != 0){
                message = data[0]["ReturnMessage"].ToString();
                doc.LoadXml(string.Format("<Errors><Error message=\"{0}\" /></Errors>", message));
                return doc.DocumentElement;
            }
        }
        var xml = data[0]["Message"].ToString();
        doc.LoadXml(xml);
        return doc.DocumentElement;
    }
}
