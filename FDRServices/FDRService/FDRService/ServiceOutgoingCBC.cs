using CommonDatalayer;
using FDR.DataLayer;
using IBM.WMQ;
using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Schema;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl;
using FDRService.CbcStatusMessage;
using FDRService.CbcXML;

namespace FDRService
{
    public partial class ServiceOutgoingCBC : ServiceBase
    {
        private String hostName = ConfigurationManager.AppSettings["hostname"];
        private int port = int.Parse(ConfigurationManager.AppSettings["port"]);
        private String channelName = ConfigurationManager.AppSettings["channelName"];
        private String queueManagerName = ConfigurationManager.AppSettings["queueManagerName"];
        private String queueNameReq = ConfigurationManager.AppSettings["queueNameReq"];
        private String queueNameReceiveRes = ConfigurationManager.AppSettings["queueNameReceiveRes"];
        private String queueNameRes = ConfigurationManager.AppSettings["queueNameRes"];
        private String queueNameStatusMessage = ConfigurationManager.AppSettings["queueNameStatusMessage"];
        private static String path = ConfigurationManager.AppSettings["path"];

        private MQQueueManager queueManager;
        
        private Hashtable properties;
        private MQMessage message;

        private CancellationTokenSource cts = new CancellationTokenSource();
        private Task outgoingFileTask = null;
        private Task incomingFileTask = null;

        private CbCStatusMessage_OECD statusMessage = new CbCStatusMessage_OECD();
        private List<FileError_Type> validationErrors = new List<FileError_Type>();

        public ServiceOutgoingCBC()
        {
            InitializeComponent();
            this.AutoLog = false;

            // create an event source, specifying the name of a log that
            // does not currently exist to create a new, custom log
            if (!EventLog.SourceExists("FDRService"))
            {
                EventLog.CreateEventSource("FDRService", "FDRServiceLog");
            }

            // configure the event log instance to use this source name
            eventLog1.Source = "FDRService";
            eventLog1.Log = "FDRServiceLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Service Started.");
            outgoingFileTask = new Task(OutgoingFilePoll, cts.Token, TaskCreationOptions.LongRunning);

            eventLog1.WriteEntry("Outgoing file task polled.");
            outgoingFileTask.Start();

            incomingFileTask = new Task(IncomingFilePoll, cts.Token, TaskCreationOptions.LongRunning);

            eventLog1.WriteEntry("Incoming file task polled.");
            incomingFileTask.Start();
        }

        protected override void OnStop()
        {
            cts.Cancel();
            outgoingFileTask.Wait();
        }

        private void IncomingFilePoll()
        {
            CancellationToken cancellation = cts.Token;
            TimeSpan interval = TimeSpan.Zero;
            while (!cancellation.WaitHandle.WaitOne(interval))
            {
                try
                {
                    GetResponse();

                    // Occasionally check the cancellation state.
                    if (cancellation.IsCancellationRequested)
                    {
                        break;
                    }

                    interval = TimeSpan.FromSeconds(15);
                }
                catch (Exception caught)
                {
                    eventLog1.WriteEntry(string.Format("Exception caught: {0}", caught.Message), EventLogEntryType.Error);
                    eventLog1.WriteEntry(caught.StackTrace, EventLogEntryType.Error);
                    interval = TimeSpan.FromSeconds(30);
                }
        }
        }

        private void OutgoingFilePoll()
        {
            CancellationToken cancellation = cts.Token;
            TimeSpan interval = TimeSpan.Zero;
            while (!cancellation.WaitHandle.WaitOne(interval))
            {
                try
                {
                    //Get List of Approved outgoing CBCR's
                    var cbcr = DBReadManager.GetOutGoingCBCR(null, 0, 5);

                    //Loop through each approved CBCR and generate XML report
                    foreach (DataRow row in cbcr.Tables[0].Rows)
                    {
                        var outCBC = DatabaseReader.OutGoingCBCDeclarationsDetails(row["CountryCode"].ToString(), int.Parse(row["Period"].ToString()));

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(outCBC.CBCData);

                        XmlNodeList CBCxml = xmlDoc.GetElementsByTagName("CBC_OECD");
                        List<XmlNode> nodees = new List<XmlNode>(CBCxml.Cast<XmlNode>());

                        string cbcXML = nodees[0].OuterXml;

                        var user = Sars.Systems.Security.ADUser.SearchAdUsersBySid(outCBC.UpdatedBy);
                        string email = "";

                        if (user != null)
                            email = user[0].Mail;

                        //Begin CTS-SenderFileMetadata-1.0
                        CTSSenderFileMetadataType sender = new CTSSenderFileMetadataType();

                        sender.BinaryEncodingSchemeCdSpecified = true;
                        sender.FileCreateTsSpecified = true;
                        sender.FileFormatCdSpecified = true;
                        sender.FileRevisionIndSpecified = true;

                        sender.BinaryEncodingSchemeCd = BinaryEncodingSchemeCdType.NONE;
                        sender.CTSCommunicationTypeCd = CTSCommunicationTypeCdType.CBCStatus;
                        sender.CTSReceiverCountryCd = (CountryCode_Type)System.Enum.Parse(typeof(CountryCode_Type), row["CountryCode"].ToString());
                        sender.CTSSenderCountryCd = (CountryCode_Type)System.Enum.Parse(typeof(CountryCode_Type), "ZA");
                        sender.SenderFileId = "";
                        sender.FileFormatCd = FileFormatCdType.XML;
                        sender.FileCreateTs = DateTime.Now;
                        sender.TaxYear = row["Period"].ToString();
                        sender.FileRevisionInd = false;
                        sender.OriginalCTSTransmissionId = "";
                        sender.SenderContactEmailAddressTxt = email;

                        XmlSerializer xsSer = new XmlSerializer(typeof(CTSSenderFileMetadataType));

                        var settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.OmitXmlDeclaration = true;
                        
                        string senderMetaDataXML;

                        using (var sww = new StringWriter())
                        {
                            using (XmlWriter writer = XmlWriter.Create(sww, settings))
                            {
                                xsSer.Serialize(writer, sender);

                                senderMetaDataXML = sww.ToString();
                            }
                        }
                        //End CTS-SenderFileMetadata-1.0

                        //Begin SARSStatusManagementV1.15
                        //StatusManagementRequest

                        //StatusManagementRequestStructure req = new StatusManagementRequestStructure();

                        //req.StatusDetails = new StatusStructure() { Description = "", Status = "" };
                        //req.DataType = "";
                        //req.TaxType = TypeOfTaxType.INCOME_TAX;
                        //req.TaxDateRange = new TaxDateRangeStructure() { FromTaxDate = DateTime.Now, ToTaxDate = DateTime.Now };
                        //req.TaxPeriodRange = new TaxPeriodRangeStructure() { FromTaxPeriod = "", ToTaxPeriod = "" };
                        //req.TaxYearRange = new TaxYearRangeStructure() { FromTaxYear = "", ToTaxYear = "" };
                        //req.SubReference = new SubReferenceStructure() { Type = SubReferenceStructureType.FORM_ID, Value = "" };
                        //req.Reference = new ReferenceStructure() { Identifier = ReferenceStructureIdentifier.TAX_REF_NO, Value = new[] { "", "", "" } };
                        //req.SourceChannel = "";
                        //req.RequestOperation = RequestOperationType.UPDATE_STATUS;

                        //XmlSerializer xsSerReq = new XmlSerializer(typeof(StatusManagementRequestStructure));
                        //string requestXML;

                        //using (var sww = new StringWriter())
                        //{
                        //    using (XmlWriter writer = XmlWriter.Create(sww, settings))
                        //    {
                        //        xsSerReq.Serialize(writer, req);

                        //        requestXML = sww.ToString();
                        //    }
                        //}

                        ////End SARSStatusManagementV1.15

                        //var str = string.Format("{0}{1}", cbcXML, senderMetaDataXML);
                        string zipFilename = row["CountryCode"].ToString() + "_FDR_" + DateTime.Now.ToString("yyyyMMddTHHmmssZ") + ".zip";
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddEntry("ZA_CBC_Metadata.xml", senderMetaDataXML);
                            zip.AddEntry("ZA_CBC_Payload.xml", cbcXML);
                            zip.Save(path + "\\Outgoing\\" + zipFilename);//location and name for creating zip file
                        }

                        //XmlTypeMapping myTypeMapping = new SoapReflectionImporter().ImportTypeMapping(typeof(CountryByCountryReportManagementRequest));

                        var subReq = new CountryByCountryReportManagementRequestStructure();

                        subReq.RequestOperation = CountryByCountryReportManagementRequestStructureRequestOperation.SUBMIT_REPORT;
                        subReq.RequestOperationSpecified = true;
                        subReq.Destination = row["CountryCode"].ToString();
                        subReq.Filename = zipFilename;
                        subReq.FileContent = EncodeToBase64(path + "\\Outgoing\\" + zipFilename);

                        //*******************************************
                        var xml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(subReq, "cbcMgt", "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/xml/schemas/version/1.2");

                        var xmlBuilder = new StringBuilder();
                        xmlBuilder.Append(
                            "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> " +
                            "<soap12:Envelope xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" " +
                            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                            "xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">"

                            );

                        var uid = Guid.NewGuid().ToString();
                        var header = new MessageIdentificationStructure();
                        header.domain = "CBC Financial Data Reporting";
                        header.originatingChannelID = "FDR";
                        header.externalReferenceID = zipFilename;
                        header.priority = 6;
                        header.channelID = "FDR";
                        header.applicationID = "SubmitCBCReport";
                        header.versionNo = (float)1.2;
                        header.activityName = "Submit CBC Report";
                        header.messageSeqNo = DBReadManager.GetNextMessageId(uid);
                        header.messageTimeStamp = DateTime.Now;
                        header.universalUniqueID = uid;
                        
                        var headerXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(header, "esb", "http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1");

                        xmlBuilder.AppendFormat(
                            "<soap12:Header xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" +
                            "{0}" +
                            "</soap12:Header>" +
                            " <soap12:Body>" +
                            "{1}" +
                            "</soap12:Body>" +
                            "</soap12:Envelope>",
                            headerXml,
                            xml
                            );


                        string myXML = FormatXml(xmlBuilder.ToString());

                        XmlDocument doc = new XmlDocument();

                        doc.LoadXml(myXML);

                        doc.Save(string.Format("{0}\\Outgoing\\OutgoingCBCR_{1}.xml", path, row["CountryCode"].ToString()));

                        try
                        {
                            PutFile(queueNameReq, File.ReadAllBytes(string.Format("{0}\\Outgoing\\OutgoingCBCR_{1}.xml", path, row["CountryCode"].ToString())));
                            DatabaseWriter.ApproveOutgoingCBC(row["CountryCode"].ToString(), int.Parse(row["Period"].ToString()), 6, "0");
                            eventLog1.WriteEntry("File " + uid + " sent to que.");
                        }
                        catch (MQException mqe)
                        {
                            interval = TimeSpan.FromSeconds(5);
                            eventLog1.WriteEntry(string.Format("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message), EventLogEntryType.Error);
                            eventLog1.WriteEntry(mqe.StackTrace, EventLogEntryType.Error);
                        }
                    }

                    // Occasionally check the cancellation state.
                    if (cancellation.IsCancellationRequested)
                    {
                        break;
                    }

                    interval = TimeSpan.FromSeconds(15);
                }
                catch (Exception caught)
                {
                    eventLog1.WriteEntry(string.Format("Exception caught (OutgoingFilePoll): {0}", caught.Message), EventLogEntryType.Error);
                    eventLog1.WriteEntry(caught.StackTrace, EventLogEntryType.Error);
                    interval = TimeSpan.FromSeconds(30);
                }
            }
        }

        public static string FormatXml(string xmlBuilder)
        {
            using (var mStream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    var document = new XmlDocument();
                    document.LoadXml(xmlBuilder);
                    writer.Formatting = Formatting.Indented;
                    document.WriteContentTo(writer);
                    writer.Flush();
                    mStream.Flush();
                    mStream.Position = 0;
                    var sReader = new StreamReader(mStream);
                    var xml = sReader.ReadToEnd();
                    mStream.Close();
                    writer.Close();
                    sReader.Close();
                    return xml;
                }
            }
        }

               
        public static string EncodeToBase64(string zipPath)
        {
            using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
            {
                byte[] filebytes = new byte[fs.Length];
                fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                return Convert.ToBase64String(filebytes);
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string[] UnzipToString(string stringBase64Zipped, string filename)
        {
            string[] results = new string[2];
            int cnt = 0;

            var bytesZipFile = Convert.FromBase64String(stringBase64Zipped);

            var msZipFile = new MemoryStream(bytesZipFile);
            msZipFile.Position = 0;
            using (var zipFile1 = ZipFile.Read(msZipFile))
            {
                //zipFile1.Save(string.Format("{0}\\Incoming\\{1}", path, filename));  // This works
                //var zipEntry1 = zipFile1.Entries.First();
                foreach (var entry in zipFile1.Entries)
                {
                    //zipEntry1.Extract("D:\\SARS\\Incoming\\", ExtractExistingFileAction.OverwriteSilently);

                    using (var ms = new MemoryStream())
                    {
                        ms.Position = 0;
                        entry.Extract(ms);

                        var streamReader1 = new StreamReader(ms);
                        var result = String.Empty;
                        ms.Position = 0;
                        result = streamReader1.ReadToEnd();

                        XDocument xdoc = XDocument.Parse(result);
                        xdoc.Declaration = null;
                        

                        results[cnt] = xdoc.ToString();
                    }

                    cnt++;

                }
            }

            return results;
        }


        public void PutFile(string quename, byte[] file)
        {
            MQQueue queue;

            // mq properties
            properties = new Hashtable();
            properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
            properties.Add(MQC.HOST_NAME_PROPERTY, hostName);
            properties.Add(MQC.PORT_PROPERTY, port);
            properties.Add(MQC.CHANNEL_PROPERTY, channelName);

            // create connection
            queueManager = new MQQueueManager(queueManagerName, properties);

            // accessing queue
            queue = queueManager.AccessQueue(quename, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

            // creating a message object
            message = new MQMessage();
            message.Write(file);

            queue.Put(message);

            // closing queue
            queue.Close();

            // disconnecting queue manager
            queueManager.Disconnect();
        }

        

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                validationErrors.Add(new FileError_Type() { Code = "50007", Details = new ErrorDetail_Type() { Value = args.Message } });
                //eventLog1.WriteEntry("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
                //throw new Exception("\tWarning: Matching schema not found.No validation occurred." + args.Message);
            }
            else
            {
                validationErrors.Add(new FileError_Type() { Code = "50007", Details = new ErrorDetail_Type() { Value = args.Message } });
                //throw new Exception("\tValidation error: " + args.Message);
                //eventLog1.WriteEntry("\tValidation error: " + args.Message);
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        XmlReaderSettings GetSettings(Stream xsd)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            XmlReader xmlXSD = XmlReader.Create(xsd);
            settings.Schemas.Add(null, xmlXSD);

            return settings;
        }

        void GetResponse()
        {
            MQQueue queue;

            try
            {
                // mq properties
                properties = new Hashtable();
                properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
                properties.Add(MQC.HOST_NAME_PROPERTY, hostName);
                properties.Add(MQC.PORT_PROPERTY, port);
                properties.Add(MQC.CHANNEL_PROPERTY, channelName);

                // create connection
                queueManager = new MQQueueManager(queueManagerName, properties);

                // accessing queue
                queue = queueManager.AccessQueue(queueNameRes, MQC.MQOO_BROWSE + MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

                // creating a message object
                message = new MQMessage();
                MQGetMessageOptions mqGetMsgOpts = new MQGetMessageOptions();
                mqGetMsgOpts.Options = MQC.MQGMO_BROWSE_FIRST;

                try
                {
                    queue.Get(message, mqGetMsgOpts);
                    MQGetMessageOptions mqGetNextMsgOpts = new MQGetMessageOptions();
                    mqGetNextMsgOpts.Options = MQC.MQGMO_BROWSE_NEXT;

                    while (true)
                    {
                        try
                        {
                            string messageText = message.ReadString(message.MessageLength);

                            byte[] byteConent = new byte[message.MessageLength];
                            message.ReadFully(ref byteConent, 0, message.MessageLength);

                            string str = Encoding.Default.GetString(byteConent);

                            var doc = new XmlDocument();
                            doc.LoadXml(str);

                            var ns = new XmlNamespaceManager(doc.NameTable);
                            ns.AddNamespace("esb", "http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1");
                            ns.AddNamespace("cbcMgt", "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/xml/schemas/version/1.2");

                            var header = doc.SelectSingleNode("//esb:MessageIdentification", ns);
                            var request = doc.SelectSingleNode("//cbcMgt:CountryByCountryReportManagementRequest", ns);
                            
                            if (header != null && request != null)
                            {
                                /*//Validate header xml
                                XmlReaderSettings settingsHeader = GetSettings(Assembly.GetExecutingAssembly().GetManifestResourceStream("FDRService.schemas.ESBHeaderV7.1.xsd"));
                                var readerHeader = XmlReader.Create(header.OuterXml, settingsHeader);
                                while (readerHeader.Read());
                                readerHeader.Close();

                                //Validate Body xml
                                XmlReaderSettings settingsRequest = GetSettings(Assembly.GetExecutingAssembly().GetManifestResourceStream("FDRService.schemas.SARSCountryByCountryReportManagementV1.2.xsd"));
                                var readerRequest = XmlReader.Create(request.OuterXml, settingsRequest);
                                while (readerRequest.Read());
                                readerRequest.Close();*/

                                //Parse XML to objects
                                var headerReq = Sars.Systems.Serialization.XmlObjectSerializer.ConvertXmlToObject<MessageIdentificationStructure>(header.OuterXml);
                                var subReq = Sars.Systems.Serialization.XmlObjectSerializer.ConvertXmlToObject<CountryByCountryReportManagementRequestStructure>(request.OuterXml);
                                
                                var fragments = UnzipToString(subReq.FileContent, subReq.Filename);  //File.ReadAllText(string.Format("D:\\Sars\\Incoming\\{0}.xml", subReq.Filename.Replace(".zip", "")));
                                string fullxml = "";

                                foreach (string s in fragments)
                                {
                                    fullxml += s;
                                }

                                var myRootedXml = "<root>" + fullxml + "</root>";
                                
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(myRootedXml);

                                //Get CBC OECD data
                                XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("CBC_OECD", "*"); // xmlDoc.GetElementsByTagName("cbc:CBC_OECD");
                                List<XmlNode> nodees = new List<XmlNode>(xmlNodeList.Cast<XmlNode>());

                                string cbcXML = nodees[0].OuterXml;

                                //Validate CBC OECD xml
                                XmlReaderSettings settingsOECD = GetSettings(Assembly.GetExecutingAssembly().GetManifestResourceStream("FDRService.schemas.CbcXML_v1.0.1.xsd"));
                                var readerOECD = XmlReader.Create(cbcXML, settingsOECD);
                                while (readerOECD.Read());
                                readerOECD.Close();

                                //Add validation errors 
                                statusMessage.CbCStatusMessage.ValidationErrors.FileError = validationErrors.ToArray();

                                bool valErrors = validationErrors.Count() > 0;
                                var valErrorList = validationErrors;
                                validationErrors = new List<FileError_Type>();

                                var appResult = new ApplicationInformationStructureApplicationInformationResult
                                {
                                    Code = valErrors ? "50007" : "0000",
                                    Description = valErrors ? string.Join(",", valErrorList) : "Processed",
                                    MessageType = valErrors ? MessageTypeEnum.ERROR : MessageTypeEnum.INFORMATION
                                };

                                //Get Response XML
                                string responseXML = CreateResponseXml(header.OuterXml, appResult);

                                //Put response XML to Queue
                                PutFile(queueNameReceiveRes, Encoding.ASCII.GetBytes(responseXML));

                                if (valErrors)
                                    throw new Exception("Validation errors occured: " + string.Join(",", valErrorList));

                                //Cast cbcXML to object
                                var cbcOECD = Sars.Systems.Serialization.XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXML);

                                if (cbcOECD == null)
                                    throw new Exception("Couldn't cast cbcOECD data to object");

                                //Create Message Spec
                                statusMessage.MessageSpec = new CbcStatusMessage.MessageSpec_Type()
                                {
                                    SendingCompanyIN = cbcOECD.MessageSpec.SendingEntityIN,
                                    TransmittingCountry = (CbcStatusMessage.CountryCode_Type)Enum.Parse(typeof(CbcStatusMessage.CountryCode_Type), cbcOECD.MessageSpec.TransmittingCountry.ToString()),
                                    ReceivingCountry = (CbcStatusMessage.CountryCode_Type)Enum.Parse(typeof(CbcStatusMessage.CountryCode_Type), cbcOECD.MessageSpec.ReceivingCountry.ToString()),
                                    MessageType = CbcStatusMessage.MessageType_EnumType.CbCMessageStatus,
                                    Warning = "",
                                    MessageRefId = cbcOECD.MessageSpec.MessageRefId,
                                    MessageTypeIndic = CbCMessageTypeIndic_EnumType.CbCMessageStatus,
                                    MessageTypeIndicSpecified = true,
                                    ReportingPeriod = cbcOECD.MessageSpec.ReportingPeriod,
                                    Timestamp = DateTime.Now
                                };

                                //Add original message information
                                statusMessage.CbCStatusMessage.OriginalMessage.OriginalMessageRefID = cbcOECD.MessageSpec.MessageRefId;
                                statusMessage.CbCStatusMessage.OriginalMessage.FileMetaData.CTSSendingTimeStamp = DateTime.Now;
                                statusMessage.CbCStatusMessage.OriginalMessage.FileMetaData.CTSSendingTimeStampSpecified = true;
                                statusMessage.CbCStatusMessage.OriginalMessage.FileMetaData.UncompressedFileSizeKBQty = "0";
                                statusMessage.CbCStatusMessage.OriginalMessage.FileMetaData.CTSTransmissionID = "";

                                var recordErrors = new List<RecordError_Type>();

                                foreach (var item in cbcOECD.CbcBody)
                                {
                                    foreach (var bodyItem in item.CbcReports)
                                    {
                                        var docRefId = bodyItem.DocSpec.DocRefId;

                                        //Check if docRefId exists if exists add to record error
                                        //recordErrors.Add(new RecordError_Type() { Code = "80000", Details = new ErrorDetail_Type() { Value = "DocRefID already used" }, DocRefIDInError = new[] { docRefId } });

                                        //Check format of docrefid if not correct add to record error
                                        //recordErrors.Add(new RecordError_Type() { Code = "80001", Details = new ErrorDetail_Type() { Value = "DocRefID format" }, DocRefIDInError = new[] { docRefId } });

                                        var corrDocRefID = bodyItem.DocSpec.CorrDocRefId;

                                        //Check if docRefid exist if NOT exist add to record error
                                        //recordErrors.Add(new RecordError_Type() { Code = "80002", Details = new ErrorDetail_Type() { Value = "CorrDocRefId unknown" }, DocRefIDInError = new[] { docRefId } });

                                    }
                                }

                                //Add record errors 
                                statusMessage.CbCStatusMessage.ValidationErrors.RecordError = recordErrors.ToArray();
                                
                                //Get File Metadata
                                xmlNodeList = xmlDoc.GetElementsByTagName("CTSSenderFileMetadata");
                                nodees = new List<XmlNode>(xmlNodeList.Cast<XmlNode>());

                                //Get File metadata xml
                                string sender = nodees[0].OuterXml;

                                //Deserialize File Metadata to object
                                XmlSerializer CTSSenderFileMetadata = new XmlSerializer(typeof(CTSSenderFileMetadataType));
                                CTSSenderFileMetadataType senderReq;

                                using (TextReader sr = new StringReader(sender))
                                {
                                    senderReq = (CTSSenderFileMetadataType)CTSSenderFileMetadata.Deserialize(sr);
                                }

                                //Save CBC OECD Data to DB
                                var cbcr = DBWriteManager.SaveIncomingCBCDeclaration(0, senderReq.CTSSenderCountryCd.ToString(), int.Parse(senderReq.TaxYear), cbcXML);

                                statusMessage.CbCStatusMessage.ValidationResult.Status = FileAcceptanceStatus_EnumType.Accepted;

                                XmlSerializer xsSubmit = new XmlSerializer(typeof(CbCStatusMessage_OECD));
                                
                                var xml = "";

                                using (var sww = new StringWriter())
                                {
                                    using (XmlWriter writer = XmlWriter.Create(sww))
                                    {
                                        xsSubmit.Serialize(writer, statusMessage);
                                        xml = sww.ToString();
                                    }
                                }

                                PutFile(queueNameStatusMessage, Encoding.Default.GetBytes(xml));

                                eventLog1.WriteEntry("got incoming file: " + headerReq.universalUniqueID);

                                //Remove message from the Queue
                                mqGetMsgOpts.Options = MQC.MQGMO_MSG_UNDER_CURSOR;
                                queue.Get(message, mqGetMsgOpts);
                            }
                            else
                            {
                                if (header == null)
                                    eventLog1.WriteEntry("Error (Incoming File): No header message found", EventLogEntryType.Error);

                                if (request == null)
                                    eventLog1.WriteEntry("Error (Incoming File): No request message found", EventLogEntryType.Error);

                                //Application Results failed schema validation
                                var appResult = new ApplicationInformationStructureApplicationInformationResult
                                {
                                    Code = "50007",
                                    Description = "Failed Schema Validation",
                                    MessageType = MessageTypeEnum.ERROR
                                };

                                //Get Response XML
                                string responseXML = CreateResponseXml(header.OuterXml, appResult);

                                //Put response XML to Queue
                                PutFile(queueNameReceiveRes, Encoding.ASCII.GetBytes(responseXML));
                            }

                            //Get next Message
                            message = new MQMessage();
                            queue.Get(message, mqGetNextMsgOpts);
                        }
                        catch (MQException mqe)
                        {
                            if (mqe.ReasonCode == 2033)
                            {
                                //eventLog1.WriteEntry("No message available");
                                break;
                            }
                            else
                            {
                                eventLog1.WriteEntry(string.Format("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message), EventLogEntryType.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            eventLog1.WriteEntry(string.Format("Exception caught (Incoming file): {0}", ex.Message), EventLogEntryType.Error);
                            message = new MQMessage();
                            queue.Get(message, mqGetNextMsgOpts);
                        }
                    }
                }
                catch (MQException mqe)
                {
                    if (mqe.ReasonCode == 2033)
                    {
                        //No message available do nothing
                        //eventLog1.WriteEntry("No message available");
                    }
                    else
                    {
                        eventLog1.WriteEntry(string.Format("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message), EventLogEntryType.Error);
                    }
                }

                // closing queue
                queue.Close();

                // disconnecting queue manager
                queueManager.Disconnect();
            }
            catch (MQException mqe)
            {
                eventLog1.WriteEntry(string.Format("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message), EventLogEntryType.Error);
                eventLog1.WriteEntry(mqe.StackTrace, EventLogEntryType.Error);
            }
        }

        private static string CreateResponseXml(string header, ApplicationInformationStructureApplicationInformationResult appResult)
        {
            var applicationInformationStructure = new ApplicationInformationStructure
            {
                ApplicationInformationResult = new ApplicationInformationStructureApplicationInformationResult[1]
            };
            applicationInformationStructure.ApplicationInformationResult[0] = appResult;
                

            var applicationInformationStructureXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(applicationInformationStructure, "fdri", "http://www.egovernment.gov.za/GMD/ApplicationInformation/xml/schemas/version/3.1");

            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append(
                "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?> " +
                "<soap12:Envelope xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">"

                );
            xmlBuilder.AppendFormat(
                 "<soap12:Header xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" +
                "{0}" +
                "</soap12:Header>" +
                " <soap12:Body>" +
                "{1}" +
                "</soap12:Body>" +
                "</soap12:Envelope>",
                header,
                applicationInformationStructureXml
                );
            return FormatXml(xmlBuilder.ToString());
        }
    }
}
