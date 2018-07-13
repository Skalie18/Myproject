using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Sars.ESBSchema.ApplicationInformation.v31;
using Sars.Systems.Data;
using Sars.Systems.Mail;
using Sars.Systems.Serialization;

namespace FRD_MNE_Services
{
    public static class FdrCommon
    {
        private const string EventSource = "FDR MNE Provider Service";
        private const string Log = "FDR MNE Provider";
        public static void LogEvent(Exception exception, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, Log);
            }
            if (exception != null)
            {
                var error = new StringBuilder();
                error.AppendFormat("MESSAGE : {0}\n", exception.Message);

                var oEventLog = new EventLog(Log) { Source = EventSource };
                oEventLog.WriteEntry(error.ToString(), type);
            }
        }
        public static void LogEvent(string message)
        {
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, Log);
            }

            var error = new StringBuilder();
            error.AppendFormat("MESSAGE : {0}\n", message);

            var oEventLog = new EventLog(Log) { Source = EventSource };
            oEventLog.WriteEntry(error.ToString(), EventLogEntryType.Information);

        }
        public static void LogEvent(string message, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource, Log);
            }

            var error = new StringBuilder();
            error.AppendFormat("MESSAGE : {0}\n", message);

            var oEventLog = new EventLog(Log) { Source = EventSource };
            oEventLog.WriteEntry(error.ToString(), type);
        }
        public static void SaveMessage(string message, string path)
        {
            path = Path.Combine(path, ServiceConfigurationSettings.CurrentEnvironment);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var tempFileName = Path.Combine(path, String.Format("{0}.xml", DateTime.Now.ToFileTime()));
            using (var stream = File.CreateText(tempFileName))
            {
                stream.Write(message);
                stream.Flush();
                stream.Close();
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
        public static string RemoveXmlPreprocessingInstruction(string xml)
        {
            using (var mStream = new MemoryStream())
            {
                var document = new XmlDocument();
                document.LoadXml(xml);
                var writer = XmlWriter.Create(mStream, new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Auto,
                    OmitXmlDeclaration = true,
                    Indent = true
                });

                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();
                mStream.Position = 0;
                var sReader = new StreamReader(mStream);
                var newXml = sReader.ReadToEnd();
                mStream.Close();
                sReader.Close();
                return newXml;

            }
        }
        public static List<KeyValuePair<string, XNamespace>> GetNamespaces(string xml)
        {
            var z = XDocument.Parse(xml);
            if (z.Root != null)
            {
                var result = z.Root.Attributes().
                    Where(a => a.IsNamespaceDeclaration).
                    GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                        a => XNamespace.Get(a.Value)).
                    ToDictionary(g => g.Key,
                        g => g.First());

                return result == null ? null : result.ToList();
            }
            return null;
        }
        public static List<KeyValuePair<string, string>> GetXmlNamespaces(string xml)
        {
            //var xDocument = XDocument.Parse(xml);
            //var xNavigator = xDocument.CreateNavigator();
            //xNavigator.MoveToFollowing(XPathNodeType.Element);
            //var namespaces = xNavigator.GetNamespacesInScope(XmlNamespaceScope.All);
            //return namespaces == null ? null : namespaces.ToList();

            var xDocument = XDocument.Parse(xml);
            var xNavigator = xDocument.CreateNavigator();
            var x = xDocument.Elements();
            xNavigator.MoveToFollowing(XPathNodeType.Element);
            var namespaces = xNavigator.GetNamespacesInScope(XmlNamespaceScope.All);
            return namespaces == null ? null : namespaces.ToList();
        }
        public static string RemoveEmptyElements(string xml)
        {
            var doc = XDocument.Parse(xml);

            doc.Descendants()
                .Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value))
                .Remove();
            return doc.ToString();
        }
        public static void SendEmail(string[] emailAddresses, string body, string subject)
        {
            try
            {
                using (var client = new SmtpServiceClient("basicHttpEndPoint"))
                {
                    var oMessage = new SmtpMessage
                    {
                        From = AppConfig.FromAddress,
                        Body = body,
                        IsBodyHtml = true,
                        Subject = subject,
                        To = emailAddresses.ToArray()
                    };

                    try
                    {
                        client.Send2(oMessage);
                    }
                    catch (Exception exception)
                    {
                        LogEvent(exception, EventLogEntryType.Error);
                    }
                }
            }

            catch (Exception exception)
            {
                LogEvent(exception, EventLogEntryType.Error);
            }
        }
        public static XmlNode SelectNode(string message, string prifix, string namespace_, string nodeName)
        {
            //var doc = new XmlDocument();
            //doc.LoadXml(message);
            //var ns = new XmlNamespaceManager(doc.NameTable);
            //ns.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
            //var header = doc.SelectSingleNode("//soap12:Header", ns);
            //return header;

            var doc = new XmlDocument();
            doc.LoadXml(message);
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace(prifix, namespace_);
            var header = doc.SelectSingleNode(String.Format("//{0}:{1}", prifix, nodeName), ns);
            return header;
        }

        public static void SendEmail(string messageBody, string toAddress, string subject)
        {
            using (var client = new SmtpServiceClient("basicHttpEndPoint"))
            {
                var oMessage = new SmtpMessage
                {
                    From = String.Format("{0}@SARS.GOV.ZA", SARSDataSettings.Settings.ApplicationName).ToUpper(),
                    Body = messageBody,
                    IsBodyHtml = true,
                    Subject =subject,
                    To = new[] { toAddress }
                };
                client.Send2(oMessage);
            }
        }


        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length*2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }



        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars/2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string CreateFaultXml(string header, string errors)
        {
            var applicationInformationStructure = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructure
            {
                ApplicationInformationResult = new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult[1]
            };
            applicationInformationStructure.ApplicationInformationResult[0] =
                new Sars.ESBSchema.ApplicationInformation.v31.ApplicationInformationStructureApplicationInformationResult
                {
                    Code = "9999",
                    Description = errors,
                    MessageType = Sars.ESBSchema.ApplicationInformation.v31.MessageTypeEnum.ERROR
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
    }
}
