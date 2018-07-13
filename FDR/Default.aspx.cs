using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using FDR.DataLayer;
using Newtonsoft.Json;
using Sars.ESBSchema.Header.V7_1;
using Sars.Systems.Correspondence;
using Sars.Systems.Data;
using Sars.Systems.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //User.AddToRole("CBC Technician");
        //TestReadFiles();
        //GET();
        //Sars.Systems.Security.RolesConfiguration.Install();
        //this.OpenForm(FormType.ViewMenu);
        //User.AddToRole("CBC Technician");

        //SendEmail("Good day, Sibusiso is testing email correspondence", "Testing FDR", "sgigaba@sars.gov.za");
        //SendSms("0782100942", "1010101010", "2018", "GOOD DAY MASTER LOCAL FILE VALIDATION STATUS : ACCEPTED");
        //SendSms("0718844222", "1010101010", "2018", "GOOD DAY MASTER LOCAL FILE VALIDATION STATUS : ACCEPTED");
        //SendLetter("0401892153", "2017");


        //
        //TODO: Enquire Registration Details
        //


        //var registration = new RegistrationQueryDetails();
        //registration.LookUpRegistrationDetails("9068321141");
        //if ( !registration.DetailsFound )
        //{
        //    MessageBox.Show(registration.Message);
        //    return;
        //}
        //FDRQueueService queue = new FDRQueueService();
        //var id = queue.GetTaxpayerRegistrationData("9544122642");
        //var data = DBReadManager.GetResponse(id);
        //var i = 1;
        //while ( !data.HasRows && i <= Configurations.QueueResponseTime )
        //{
        //    Thread.Sleep(1);
        //    i++;
        //    data = DBReadManager.GetResponse(id);
        //}
        //if ( !data.HasRows && i >= Configurations.QueueResponseTime )
        //{
        //    MessageBox.Show(Configurations.QueueTimeoutMessage);
        //    return;
        //}

        //if ( Convert.ToInt32(data[0]["ReturnCode"]) != 0 )
        //{
        //    MessageBox.Show(data[0]["ReturnMessage"].ToString());
        //    return;
        //}

        //var xml = data[0]["Message"].ToString();
        //using ( var responseDataSet = new RecordSet() )
        //{
        //    responseDataSet.ReadXml(new StringReader(xml));
        //}
    }

    public static void SendLetter(string taxrefNo, string year)
    {
        var correspondance = new CorrespondenceManagementRequestStructure
        {
            RequestOperation = RequestOperation.ISSUE_CORRESPONDENCE,
            TaxRef = new TaxRefStructure
            {
                TaxRefNo = taxrefNo,
                TypeOfTax = TypeOfTaxType.INCOME_TAX
            },
            OutChannel = CorrespondenceManagementRequestStructureOutChannel.EFL,
            TaxYear = year
        };

        //optional

        var letter = new CorrespondenceManagementRequestStructureLetter
        {
        };

        var ret = File.ReadAllBytes(@"D:\Rejection of Master Files and Local Files.pdf");
        letter.Content = Convert.ToBase64String(ret);
        
        var letters = new List<CorrespondenceManagementRequestStructureLetter>
        {
            letter
        };
        correspondance.Letters = letters.ToArray();

        var messageId = Guid.NewGuid();
        var corXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(correspondance);
        var soapMessage = FdrCommon. CreateSoapLetter(corXml, taxrefNo, messageId.ToString());

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
    }
    public static void SendSms(string cellNo, string taxrefNo, string year, string message)
    {
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
        var corXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(correspondance);
        var soapMessage =FdrCommon. CreateSoapSms(corXml, taxrefNo, messageId.ToString());

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
    }
    public static void SendEmail(string messageBody, string subject, string emailAddress, byte[] attachement)
    {
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
                        Filename = @"Rejection of Master Files and Local Files.pdf"
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
    }
    public static void TestReadFiles()
    {
        var contentRetrieve = new DCTMCONTENTMNG_RETRIEVE_WS2();
        var supportingDocsRequest = new SupportingDocumentManagementRequestStructure
        {
            RequestOperation = RequestOperationType.ENQUIRE_DOCUMENT,
            Contents = new SupportingDocumentStructure[1]

        };
        supportingDocsRequest.Contents[0] = new SupportingDocumentStructure()
        {
            DocumentID = "090000a08018b485",
            Format = "pdf",
            Username = "dctm-qa3-efile",
            Password = "dctm-qa3-efile"
        };
        var response = contentRetrieve.RetrieveContent(supportingDocsRequest);
    }
}