using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using FDR.DataLayer;
using Sars.ESBSchema.Header.V7_1;
using Sars.Systems.Data;
using Sars.Systems.SSRS;
using System.Xml.Linq;
using Sars.Systems.Mail;

/// <summary>
/// Summary description for FdrCommon
/// </summary>
public static class FdrCommon
{
    public static byte[] GetFileRejectionLetter(string submissionId)
    {
        var parameters = new Dictionary<string, string>
        {
            {"SubmissionId", submissionId}
        };
        var results = ReportingServiceRenderer.Render
            (
                SQLReportingServicesSettings.Settings.ReportExecutionServiceUrl,
                ReportingServiceRenderer.ExportFormat.PDF,
                parameters,
                "/FDR Templates/Rejection of Master Files and Local Files",
                SQLReportingServicesSettings.Settings.Credentials
            );
        return results;
    }

    public static string GetEfilingRejectionLetter(string taxRefNo, int year, decimal submissionId)
    {
        var templatesPath = HttpContext.Current.Request.PhysicalApplicationPath;
        if (templatesPath == null)
            return null;

        var rejectionTemplate = Path.Combine(templatesPath, "letters", Configurations.RejectionLetterTemplate);
        if (!File.Exists(rejectionTemplate))
        {
            return null;
        }
        var content = File.ReadAllText(rejectionTemplate);

        var files = DBReadManager.GetDeclinedFilesForSsubmission(submissionId);
        if (!files.HasRows)
            return null;

        var builder = new StringBuilder();
        //builder.Append("<ul>");
        foreach (DataRow dataRow in files.Tables[0].Rows)
        {
            builder.AppendFormat("<br />-<span style=\"xfa - spacerun:yes;\"> {0} / {1}</span>", dataRow["Category"],
                dataRow["Classification"]);
        }
        //builder.Append("</ul>");
        return string.Format(content, year, builder);
    }

    public static string GetEfilingAcceptanceLetter(int year)
    {

        var templatesPath = HttpContext.Current.Request.PhysicalApplicationPath;
        if (templatesPath == null)
            return null;

        var acceptanceTemplate = Path.Combine(templatesPath, "letters", Configurations.AcceptanceLetterTemplate);
        if (!File.Exists(acceptanceTemplate))
        {
            return null;
        }
        var content = File.ReadAllText(acceptanceTemplate);

        return string.Format(content, year);
    }

    public static byte[] GetFileAcceptenceLetter(string submissionId)
    {
        var parameters = new Dictionary<string, string>
        {
            {"SubmissionId", submissionId}
        };
        var results = ReportingServiceRenderer.Render
            (
                SQLReportingServicesSettings.Settings.ReportExecutionServiceUrl,
                ReportingServiceRenderer.ExportFormat.PDF,
                parameters,
                "/FDR Templates/Acceptance of Master Files and Local Files",
                SQLReportingServicesSettings.Settings.Credentials
            );
        return results;
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

    public static string WrapHtmlElement(string text)
    {
        if (text.Contains("html") || text.Contains("body"))
        {
            return text;
        }
        var sbHtmlBody = new StringBuilder();
        sbHtmlBody.Append("<!DOCTYPE html><html><head></head><body><div>");

        sbHtmlBody.Append(text);

        sbHtmlBody.Append("</div></body><html>");
        return sbHtmlBody.ToString();
    }

    public static string CreateSoapEmail(string message, string email, string messageId)
    {
        var sb = new StringBuilder();
        sb.Append(
            "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">");
        sb.Append("  <soap12:Header>");

        var header = new MessageIdentificationStructure
        {
            channelID = "FDR",
            applicationID = "CorrespondenceManagement.Electronic.Email",
            messageSeqNo = DBReadManager.GetNextMessageId(messageId),
            messageTimeStamp = DateTime.Now,
            externalReferenceID = email,
            originatingChannelID = "FDR",
            universalUniqueID = messageId,
            versionNo = Convert.ToSingle("1.9"),
            domain = "PIT",
            activityName = "CorrespondenceManagement.Electronic.Email",
            priority = 9
        };

        var headerXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(header);
        sb.Append(headerXml);

        sb.Append("</soap12:Header>");
        sb.Append("<soap12:Body>");
        sb.AppendFormat("{0}", message);
        sb.Append("</soap12:Body>");
        sb.Append("</soap12:Envelope>");
        return sb.ToString();
    }

    public static string CreateSoapSms(string message, string taxRefNo, string messageId)
    {
        var sb = new StringBuilder();
        sb.Append(
            "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">");
        sb.Append("  <soap12:Header>");

        var header = new MessageIdentificationStructure
        {
            channelID = "FDR",
            applicationID = "CorrespondenceManagement.Electronic.SMS",
            messageSeqNo = DBReadManager.GetNextMessageId(messageId),
            messageTimeStamp = DateTime.Now,
            externalReferenceID = taxRefNo,
            originatingChannelID = "FDR",
            universalUniqueID = messageId,
            versionNo = Convert.ToSingle("1.9"),
            domain = "PIT",
            activityName = "CorrespondenceManagement.Electronic.SMS",
            priority = 9
        };

        var headerXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(header);
        sb.Append(headerXml);

        sb.Append("</soap12:Header>");
        sb.Append("<soap12:Body>");
        sb.AppendFormat("{0}", message);
        sb.Append("</soap12:Body>");
        sb.Append("</soap12:Envelope>");
        return sb.ToString();
    }

    public static string CreateSoapLetter(string message, string taxRefNo, string messageId)
    {
        var sb = new StringBuilder();
        sb.Append(
            "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">");
        sb.Append("<soap12:Header>");

        var header = new MessageIdentificationStructure
        {
            channelID = "FDR",
            applicationID = "CorrespondenceManagement.Electronic.EFL",
            messageSeqNo = DBReadManager.GetNextMessageId(messageId),
            messageTimeStamp = DateTime.Now,
            externalReferenceID = taxRefNo,
            originatingChannelID = "FDR",
            universalUniqueID = messageId,
            versionNo = Convert.ToSingle("1.9"),
            domain = "PIT",
            activityName = "CorrespondenceManagement.Electronic.EFL",
            priority = 9
        };

        var headerXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(header);
        sb.Append(headerXml);

        sb.Append("</soap12:Header>");
        sb.Append("<soap12:Body>");
        sb.AppendFormat("{0}", message);
        sb.Append("</soap12:Body>");
        sb.Append("</soap12:Envelope>");
        return sb.ToString();
    }

    public static string CreateSoapRegistrationQuery(string message, string taxRefNo, string messageId)
    {
        var sb = new StringBuilder();
        sb.Append(
            "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:esb=\"http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1\">");
        sb.Append("<soap12:Header>");

        var header = new MessageIdentificationStructure
        {
            channelID = "FDR",
            applicationID = "EnquirePartyDetails",
            messageSeqNo = DBReadManager.GetNextMessageId(messageId),
            messageTimeStamp = DateTime.Now,
            externalReferenceID = taxRefNo,
            originatingChannelID = "FDR",
            universalUniqueID = messageId,
            versionNo = Convert.ToSingle("3.3"),
            domain = "PartyMgt",
            activityName = "Enquire Party Details",
            priority = 9
        };

        var headerXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(header);
        sb.Append(headerXml);

        sb.Append("</soap12:Header>");
        sb.Append("<soap12:Body>");
        sb.AppendFormat("{0}", message);
        sb.Append("</soap12:Body>");
        sb.Append("</soap12:Envelope>");
        return sb.ToString();
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
        var header = doc.SelectSingleNode(string.Format("//{0}:{1}", prifix, nodeName), ns);
        return header;
    }

    public static XmlNode SelectNode(string message, string namespace_, string nodeName)
    {
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(message);
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("", namespace_);
            var header = doc.SelectSingleNode(string.Format("//{0}", nodeName), ns);
            return header;
        }
        catch (Exception exception)
        {
            return null;
        }
    }

    public static XmlNode RemoveAllNamespaces(XmlNode documentElement)
    {
        var xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
        var outerXml = documentElement.OuterXml;
        var matchCol = Regex.Matches(outerXml, xmlnsPattern);
        foreach (var match in matchCol)
            outerXml = outerXml.Replace(match.ToString(), "");

        var result = new XmlDocument();
        result.LoadXml(outerXml);

        return result;
    }

    public static XmlNode RemoveAllNamespaces(string outerXml)
    {
        var xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
        var matchCol = Regex.Matches(outerXml, xmlnsPattern);
        foreach (var match in matchCol)
            outerXml = outerXml.Replace(match.ToString(), "");

        var result = new XmlDocument();
        result.LoadXml(outerXml);

        return result;
    }

    public static void RemoveNamespacePrefix(XElement element)
    {
        //Remove from element
        if (element.Name.Namespace != null)
            element.Name = element.Name.LocalName;

        //Remove from attributes
        var attributes = element.Attributes().ToArray();
        element.RemoveAttributes();
        foreach (var attr in attributes)
        {
            var newAttr = attr;

            if (attr.Name.Namespace != null)
                newAttr = new XAttribute(attr.Name.LocalName, attr.Value);

            element.Add(newAttr);
        }
        ;

        //Remove from children
        foreach (var child in element.Descendants())
            RemoveNamespacePrefix(child);
    }

    public static void SendEmail(string[] emailAddresses, string body, string subject)
    {

        using (var client = new SmtpServiceClient("basicHttpEndPoint"))
        {
            var oMessage = new SmtpMessage
            {
                From = Configurations.FromAddress,
                Body = body,
                IsBodyHtml = true,
                Subject = subject,
                To = emailAddresses.ToArray()
            };


            client.Send2(oMessage);
            client.Close();
        }
    }

}