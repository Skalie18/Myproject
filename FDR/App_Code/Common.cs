using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sars.Systems.Mail;
using Sars.Systems.Security;
using System.Data;
using System.Web.Configuration;
using System.Text;
using Sars.Systems.Data;
using Sars.Systems.Utilities;
using FDR.DataLayer;
using System.Xml;
using System.Xml.Linq;
using Sars.Models.CBC;
//using Sars.CBC;
using System.Xml.Schema;
/// <summary>
/// Summary description for Common
/// </summary>
public static class Common
{


    private static List<string> GetEmailToList(string[] toList, string[] senderList)
    {
        var Tolist = new List<string>(toList);
        var newToLis = new List<string>();
        var Senderlist = new List<string>(senderList);
        var ccAsSender = senderList[0];
        foreach (var user in toList)
        {
            if (!string.IsNullOrEmpty(user))
            {
                newToLis.Add(user);
            }
        }
        if ((newToLis.Contains(ccAsSender)) && (newToLis.Count > 1))
        {
            newToLis.Remove(ccAsSender);
        }

        //newToLis.Add(ccAsSender);
        return newToLis;
    }
    private static void SendNotification(string subject, string body, string[] to, string[] senderBase)
    {
        var sender = System.Web.Configuration.WebConfigurationManager.AppSettings["from-email"];
        using (var client = new Sars.Systems.Mail.SmtpServiceClient("basicHttpEndPoint"))
        {
            var objeMessage = new SmtpMessage
            {
                From = sender,
                Body = body,
                IsBodyHtml = true,
                Subject = subject,
                CC = senderBase.ToArray(),
                To = GetEmailToList(to, senderBase).ToArray()
            };
            client.Send2(objeMessage);
        }
    }




    public static void SendEmailToRole(string role, string country, string Subject, FDRPage.Statuses status, string[] sender, string voidAction=null)
    {
        var roles = DatabaseReader.GetRolesToNotify(role);
        if (roles.HasRows)
        {
            //get user email addresses 
            var users = (from r in roles.Tables[0].Rows.OfType<DataRow>()
                         select r["EmailAddress"].ToString()).ToArray();
            var messageBody = Utils.BuildEmail(status, country, voidAction);
            Common.SendNotification(Subject, messageBody, users, sender);

            //get user sids
            var sids = (from r in roles.Tables[0].Rows.OfType<DataRow>()
                        select r["SID"].ToString()).ToArray();



            //log email send to DB
            var emailLog = new EmailLogs()
            {
                SendBy = Sars.Systems.Security.ADUser.CurrentUser.SID,
                SendTo = GetSIDs(sids),
                EmailMessage = messageBody,
                EmailSubject = Subject
            };

            DatabaseWriter.LogEmailToDB(emailLog);
        }
    }

    public static void SendEmailToUsers(RecordSet results, string reportingPeriod, string Subject, FDRPage.Statuses status, string[] sender)
    {
        var message = Utils.BuildEmail(status, reportingPeriod,null);
        //get user email addresses 
        var users = (from r in results.Tables[0].Rows.OfType<DataRow>()
                     select r["Sid"].ToString()).ToArray();
        List<string> emails = new List<string>();
        foreach (var item in users)
        {
            var user = ADUser.SearchAdUsersBySid(item);
            if (user != null)
                emails.Add(user[0].Mail);
        }
        var messageBody = Utils.BuildEmail(status, reportingPeriod,null);
        Common.SendNotification(Subject, messageBody, emails.ToArray(), sender);

    }

    private static string GetSIDs(string[] userID)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var sid in userID)
        {
            sb.Append(sid);
            sb.Append(";");
        }
        return sb.Remove((sb.Length - 1), 1).ToString();
    }

    public static OutGoingCBCDeclarations GetDeclarations(string country, string reportingPeriod)
    {
        return DatabaseReader.OutGoingCBCDeclarationsDetails(country, reportingPeriod);
    }

    public static OutGoingCBCDeclarations GetIncomingDeclaration(int statusId, string country, string ReportingPeriod)
    {
        return DatabaseReader.IncomingCBCDeclaration(statusId, country, ReportingPeriod);
    }

    public static void GetAllCountries(System.Web.UI.WebControls.DropDownList ddlCountries)
    {
        ddlCountries.Controls.Clear();
        var countries = DatabaseReader.GetAllCountries();

        ddlCountries.DataTextField = "Country";
        ddlCountries.DataValueField = "CountryCode";
        ddlCountries.DataSource = countries;
        ddlCountries.DataBind();
        ddlCountries.Items.Insert(0, "SELECT");
    }

    public static void GetAllSentPackageCountries(System.Web.UI.WebControls.DropDownList ddlCountries)
    {
        ddlCountries.Controls.Clear();
        var countries = DatabaseReader.GetAllSentPackageCountries();

        ddlCountries.DataTextField = "Country";
        ddlCountries.DataValueField = "CountryCode";
        ddlCountries.DataSource = countries;
        ddlCountries.DataBind();
        ddlCountries.Items.Insert(0, "SELECT");
    }
    public static void GetAllForeignCountries(System.Web.UI.WebControls.DropDownList ddlCountries)
    {
        ddlCountries.Controls.Clear();
        var countries = DatabaseReader.GetAllForeignCountries();

        ddlCountries.DataTextField = "Country";
        ddlCountries.DataValueField = "CountryCode";
        ddlCountries.DataSource = countries;
        ddlCountries.DataBind();
        ddlCountries.Items.Insert(0, "SELECT");
    }


    public static string GenerateNMPackage(string country, string reportingPeriod, XmlNode messageSpec)
    {
        var results = DBReadManager.GetOutgoinCBCReport4Package(country, reportingPeriod);
        string xmlDoc = "";
        string taxrefNo = "";
        if (results.HasRows)
        {
            List<string> taxRefNo = new List<string>();

            foreach (DataRow row in results.Tables[0].Rows)
            {
                var cbcBody = XmlContent(row[0].ToString());
                if (taxrefNo != row[1].ToString())
                {
                    var tRefNo = row[1].ToString();
                    taxRefNo.Add(tRefNo);
                }
                taxrefNo = row[1].ToString();
            }
            var strSql = GetSqlQuery(taxRefNo, reportingPeriod);
            xmlDoc = PreparePackage(strSql, messageSpec);
        }
        return xmlDoc;
    }

    public static XmlDocument CreateNewDocument()
    {
        var xDoc = new XmlDocument();

        return xDoc;
    }
    public static string GenerateNewPackage(string country, string reportingPeriod, ref XmlNode msgSpec, decimal id = 0)
    {
        var results = DBReadManager.GetOutgoinCBCReport4Package(country, reportingPeriod);
        string taxrefNo = "";
        XmlDocument finalDocument = new XmlDocument();
        XNamespace urn = "urn:oecd:ties:cbc:v1";
 
        if (results.HasRows)
        {
            XmlNode oecd = finalDocument.CreateNode( XmlNodeType.Element, "CBC_OECD", finalDocument.NamespaceURI);
            var messageSpec = CreateMessageSpec(finalDocument, country, reportingPeriod, id);
            msgSpec = messageSpec;
            oecd.AppendChild(messageSpec);
            foreach (DataRow row in results.Tables[0].Rows)
            {
                var strCbcBody = XmlContent(row[0].ToString());
                if (taxrefNo != row[2].ToString())
                {
                    var strtaxrefNo = row[2].ToString();
                    XmlNode cbcBody = finalDocument.CreateNode(XmlNodeType.Element, "CbcBody", finalDocument.NamespaceURI);
                    cbcBody.InnerXml = strCbcBody;
                    oecd.AppendChild(cbcBody);
                    UpdateReport(strtaxrefNo, reportingPeriod);
                }
                taxrefNo = row[2].ToString();
            }

            finalDocument.AppendChild(oecd);
        }
        return finalDocument.OuterXml.ToString();
    }

    public static bool GetCorrectionsXml(string countryCode, string reportingPeriod, string taxrefNo)
    {
        try
        {
            var package = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
            if (package != null)
                PreparePackageForCorrections(package);

        }
        catch (Exception ex)
        {
            //throw new System.Exception(ex.Message);
            return false;
        }

        return true;
    }

    private static void PreparePackageForCorrections(Sars.Models.CBC.OutGoingCBCDeclarations cBCReports)
    {
        var packageXml = CreateNewDocument();
        var reportingPeriod = cBCReports.ReportingPeriod.ToString("yyyy-MM-dd");
        var results = DBReadManager.GetOutgoinCBCReport4Package(cBCReports.Country, reportingPeriod);
        var finalDoc = CreateNewDocument();
        var newUid = new Guid();
        packageXml.LoadXml(cBCReports.NMCBC);
        if (results.HasRows)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(packageXml.NameTable);
            nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
            var newOecd = finalDoc.CreateNode(XmlNodeType.Element, "CBC_OECD", finalDoc.NamespaceURI);
            var msgSpec = finalDoc.CreateNode(XmlNodeType.Element, "MessageSpec", finalDoc.NamespaceURI);
            var tnpMessageSpec = CorrectMessageSpec(finalDoc, cBCReports.Country, reportingPeriod, cBCReports.Id);
            msgSpec.InnerXml = tnpMessageSpec.ToString();
            newOecd.AppendChild(msgSpec);
            foreach (DataRow row in results.Tables[0].Rows)
            {
                var taxRefNo = row["TaxRefNo"].ToString();
                reportingPeriod = row["ReportingPeriod"].ToString();
                var package = packageXml.SelectSingleNode("CBC_OECD");
                if (package == null)
                {
                    package = packageXml.SelectSingleNode("urn:CBC_OECD", nsmgr);
                }
                if (package != null)
                {
                    newUid = cBCReports.UID;
                    var messageSpec = package.SelectSingleNode("MessageSpec");
                    if (messageSpec == null)
                    {
                        messageSpec = package.SelectSingleNode("urn:MessageSpec", nsmgr);
                    }
                    XmlNodeList cbcBodies = package.SelectNodes("CbcBody");
                    if (cbcBodies.Count == 0)
                    {
                        cbcBodies = package.SelectNodes("urn:CbcBody", nsmgr);
                    }
                    // var cbcBodyNode = finalDoc.CreateNode(XmlNodeType.Element, "CbcBody", finalDoc.NamespaceURI);
                    if (cBCReports.StatusId > 5 && cBCReports.ReturningStatus=="Accepted")
                    {
                        foreach (XmlNode cbcBody in cbcBodies)
                        {
                            var entityTaxRefNo = cbcBody.SelectSingleNode("urn:ReportingEntity/urn:Entity/urn:TIN", nsmgr);
                            if (entityTaxRefNo.InnerText == taxRefNo)// && messageSpec.ChildNodes[9].InnerText == reportingPeriod)
                            {

                                var cbcNode = finalDoc.CreateNode(XmlNodeType.Element, "CbcBody", finalDoc.NamespaceURI);
                                var newCbcDoc = CreateNewDocument();
                                var properXml = Sars.Systems.Serialization.XmlObjectSerializer.GetXmlWithNoDeclaration(row["cbc"].ToString());
                                newCbcDoc.LoadXml(row["cbc"].ToString());
                                XmlNode newcbcBody = newCbcDoc.DocumentElement;
                                var myReport = CorrectXMLReport(newcbcBody, cbcBody);
                                cbcNode.InnerXml = myReport.ToString();
                                newOecd.AppendChild(cbcNode);
                                UpdateReport(taxRefNo, cBCReports.ReportingPeriod.ToString("yyyy-MM-dd"));
                            }
                        }

                        finalDoc.InnerXml = newOecd.OuterXml;
                        var newPackagedCBC = new OutGoingCBCDeclarations()
                        {
                            Id = cBCReports.Id,
                            Country = cBCReports.Country,
                            CBCData = finalDoc.OuterXml.ToString(),
                            NSCBCData = finalDoc.OuterXml.ToString(),
                            StatusId = 7,
                            Year = cBCReports.Year,
                            ActionId = 2,
                            ReportingPeriod = cBCReports.ReportingPeriod,
                            CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
                        };
                        var saved = DatabaseWriter.SaveOutgoingCBC(newPackagedCBC, ref newUid);
                        if (saved > 0)
                            DBWriteManager.Insert_OutgoingPackageAuditTrail(newUid, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0}  corrected", cBCReports.Country));
                        else
                            DBWriteManager.Insert_OutgoingPackageAuditTrail(cBCReports.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} corrected", cBCReports.Country));
                    }
                    else
                    {
                        var xmldoc = new XmlDocument();
                        XmlNode NmessageSpec = xmldoc.CreateNode(XmlNodeType.Element, "MessageSpec", xmldoc.NamespaceURI);
                        var newPackage = GenerateNewPackage(cBCReports.Country, cBCReports.ReportingPeriod.ToString("yyyy-MM-dd"), ref NmessageSpec, cBCReports.Id);
                        var newPackagedCBC = new OutGoingCBCDeclarations()
                        {
                            Id = cBCReports.Id,
                            Country = cBCReports.Country,
                            CBCData = newPackage,
                            NSCBCData = newPackage,
                            StatusId = 2,
                            Year = cBCReports.Year,
                            ActionId = 1,
                            ReportingPeriod = cBCReports.ReportingPeriod,
                            CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
                        };
                        var saved = DatabaseWriter.SaveOutgoingCBC(newPackagedCBC, ref newUid);
                        if (saved > 0)
                            DBWriteManager.Insert_OutgoingPackageAuditTrail(newUid, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} generated", cBCReports.Country));
                        else
                            DBWriteManager.Insert_OutgoingPackageAuditTrail(cBCReports.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} generated", cBCReports.Country));
                    }
                }
            }
        }

    }


    private static void CorrectPackage(string countryCode, string reportingPeriod)
    {
        var results = DBReadManager.GetCorrectedReports(countryCode, reportingPeriod);
        if (results.HasRows)
        {

        }
    }

    private static string CorrectMessageSpec(XmlDocument finalDoc, string countryCode, string reportingPeriod, decimal id = 0, string corrMessageRefId = null)
    {
        var messageSpec = CreateMessageSpec(finalDoc, countryCode, reportingPeriod, id, corrMessageRefId);
        return messageSpec.InnerXml;
    }

    public static XmlNode GetOriginalMessageRefId(XmlNode messageSpec)
    {
        XmlNode messageRefId = messageSpec.SelectSingleNode("MessageRefId");
        return messageRefId;
    }

    public static XmlNode GetNMOriginalMessageRefId(XmlNode messageSpec, XmlDocument xmlDoc)
    {
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
        XmlNode messageRefId = messageSpec.SelectSingleNode("urn:MessageRefId", nsmgr);
        return messageRefId;
    }
    public static XmlNode CreateMessageSpec(XmlDocument finalDocument, string destinationCountry, string reportingPeriod, decimal id = 0, string corrMessageRefId = null)
    {
        XmlNode msgSpec = finalDocument.CreateNode(XmlNodeType.Element, "MessageSpec", finalDocument.NamespaceURI);
        finalDocument.AppendChild(CreateMessageSpecAttributes(msgSpec, finalDocument, destinationCountry, reportingPeriod, id, corrMessageRefId));
        return msgSpec;
    }

    private static XmlNode CreateMessageSpecAttributes(XmlNode messageSpec, XmlDocument xdoc, string destinationCountry, string strReportingPeriod, decimal id = 0, string corrmessageRefId = null)
    {
        var uniqueNo = Utils.GenerateUniqueNo();
        string uniqNo = id == 0 ? uniqueNo : id.ToString() + uniqueNo;
        XmlElement sendingEntityIn = xdoc.CreateElement("SendingEntityIN");
        sendingEntityIn.InnerText = "South African Revenue Services (SARS)";
        XmlNode sendingEntityInNode = (XmlNode)sendingEntityIn;
        messageSpec.AppendChild(sendingEntityInNode);

        XmlElement transmittingCountry = xdoc.CreateElement("TransmittingCountry");
        transmittingCountry.InnerText = CountryCode_Type.ZA.ToString();
        XmlNode transmittingCountryNode = (XmlNode)transmittingCountry;
        messageSpec.AppendChild(transmittingCountryNode);

        XmlElement receivingCountry = xdoc.CreateElement("ReceivingCountry");
        receivingCountry.InnerText = destinationCountry;
        XmlNode receivingCountryNode = (XmlNode)receivingCountry;
        messageSpec.AppendChild(receivingCountryNode);


        XmlElement mesageType = xdoc.CreateElement("MessageType");
        mesageType.InnerText = CTSCommunicationTypeCdType.CBC.ToString();
        XmlNode mesageTypeNode = (XmlNode)mesageType;
        messageSpec.AppendChild(mesageTypeNode);

        XmlElement language = xdoc.CreateElement("Language");
        language.InnerText = "EN";
        XmlNode languageNode = (XmlNode)language;
        messageSpec.AppendChild(languageNode);

        XmlElement warning = xdoc.CreateElement("Warning");
        warning.InnerText = "NONE";
        XmlNode warningNode = (XmlNode)warning;
        messageSpec.AppendChild(warningNode);

        XmlElement contact = xdoc.CreateElement("Contact");
        contact.InnerText = ADUser.CurrentUser.Mail;
        XmlNode contactNode = (XmlNode)contact;
        messageSpec.AppendChild(contactNode);


        XmlElement messageRefId = xdoc.CreateElement("MessageRefId");
        messageRefId.InnerText = destinationCountry.TrimEnd() + strReportingPeriod.Substring(0, 4) + "-" + uniqNo;
        XmlNode messageRefIdNode = (XmlNode)messageRefId;
        messageSpec.AppendChild(messageRefIdNode);

        /*  if (corrmessageRefId != null)
         {
             XmlElement corrMessageRefId = xdoc.CreateElement("CorrMessageRefId");
             corrMessageRefId.InnerText = corrmessageRefId;
             XmlNode corrMessageRefIdNode = (XmlNode)corrMessageRefId;
             messageSpec.AppendChild(corrMessageRefIdNode);
         }*/

        XmlElement reportingPeriod = xdoc.CreateElement("ReportingPeriod");
        reportingPeriod.InnerText = strReportingPeriod;
        XmlNode reportingPeriodNode = (XmlNode)reportingPeriod;
        messageSpec.AppendChild(reportingPeriodNode);



        XmlElement timeStamp = xdoc.CreateElement("Timestamp");
        timeStamp.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        XmlNode timeStampNode = (XmlNode)timeStamp;
        messageSpec.AppendChild(timeStampNode);
        return messageSpec;
    }


    private static string PreparePackage(string strSql, XmlNode messageSpec)
    {
        var xmlDoc = new XmlDocument();
        XmlElement oecd = xmlDoc.CreateElement("CBC_OECD");
        oecd.SetAttribute("xmlns", "urn:oecd:ties:cbc:v1");
        XmlNode newoecd = (XmlNode)oecd;
        XmlNode mspSpec = xmlDoc.CreateNode(XmlNodeType.Element, "MessageSpec", null);
        mspSpec.InnerXml = messageSpec.InnerXml;
        newoecd.AppendChild(mspSpec);
        var results = DBReadManager.ExecuteDynamicQuery(strSql);
        if (results.HasRows)
        {
            foreach (DataRow row in results.Tables[0].Rows)
            {
                var cbcReport = row[0];
                var cbcBody = xmlDoc.CreateNode(XmlNodeType.Element, "CbcBody", null);
                var tmpcbcBody = BuildPackage(cbcReport.ToString());
                if (!string.IsNullOrEmpty(tmpcbcBody))
                {
                    cbcBody.InnerXml = tmpcbcBody;
                    newoecd.AppendChild(cbcBody);
                }
            }
            xmlDoc.AppendChild(newoecd);

        }
        return xmlDoc.OuterXml.ToString();
    }




    private static string BuildPackage(string xml)
    {
        StringBuilder sbCBCBody = new StringBuilder();
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

        nsmgr.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
        nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        nsmgr.AddNamespace("esb", "http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1");
        nsmgr.AddNamespace("tnsf", @"http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/SubmitCountryByCountryDeclaration/xml/schemas/version/1.3");
        nsmgr.AddNamespace("axb", @"http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/CountryByCountryDeclaration/xml/schemas/version/1.3");
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");

        //XmlNodeList submitCBC = xmlDoc.SelectSingleNode("soap12:Envelope/soap12:Body/tnsf:SubmitCountryByCountryDeclarationRequest/axb:CountryByCountryDeclaration/urn:CBC_OECD/urn:CbcBody", nsmgr).ChildNodes;

        XmlNode cbcBody = xmlDoc.SelectSingleNode("soap12:Envelope/soap12:Body/tnsf:SubmitCountryByCountryDeclarationRequest/axb:CountryByCountryDeclaration/urn:CBC_OECD/urn:CbcBody", nsmgr);
        if (cbcBody != null)
        {
            sbCBCBody.Append(cbcBody.InnerXml);
        }
        return sbCBCBody.ToString();

    }

    public static string GetForeignIncomingCBCR(string xml, string msgSpec)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var newxmlDoc = new XmlDocument();
            var mspecDoc = new XmlDocument();
            mspecDoc.LoadXml(msgSpec);
            var countryByCountryDeclaration = newxmlDoc.CreateNode(XmlNodeType.Element, "CountryByCountryDeclaration", null);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

            var oecd = newxmlDoc.CreateNode(XmlNodeType.Element, "CBC_OECD", null);
            var messageSpec = newxmlDoc.CreateNode(XmlNodeType.Element, "MessageSpec", null);
            var msgSpecification = mspecDoc.SelectSingleNode("MessageSpec_Type");
            messageSpec.InnerXml = msgSpecification.InnerXml;
            oecd.AppendChild(messageSpec);
            nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
            XmlNode CbcBody_Type = xmlDoc.SelectSingleNode("/CbcBody_Type", nsmgr);
            if (CbcBody_Type != null)
            {
                var cbcBody = newxmlDoc.CreateNode(XmlNodeType.Element, "CbcBody", null);
                cbcBody.InnerXml = CbcBody_Type.InnerXml;
                oecd.AppendChild(cbcBody);
                countryByCountryDeclaration.AppendChild(oecd);
                XmlNode taxJurisdictions = newxmlDoc.CreateNode(XmlNodeType.Element, "TaxJurisdictions", null);
                XmlNode taxJurisdiction = newxmlDoc.CreateNode(XmlNodeType.Element, "TaxJurisdiction", null);
                taxJurisdictions.AppendChild(taxJurisdiction);
                countryByCountryDeclaration.AppendChild(taxJurisdictions);
                XmlNode contactDetails = newxmlDoc.CreateNode(XmlNodeType.Element, "ContactDetails", null);
                countryByCountryDeclaration.AppendChild(populateNode(contactDetails, newxmlDoc));

            }
            return Utils.RemoveForeignCBCNameSpace(countryByCountryDeclaration.OuterXml.ToString());
            // return Utils.RemoveAllNameSpaces(countryByCountryDeclaration.OuterXml.ToString());
        }
        catch (Exception x)
        {
            throw new Exception();
            //return null;
        }


    }

    public static string GetIncomingCBCR(string xml)
    {
        string sbCBC = "";
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

        nsmgr.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
        nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        nsmgr.AddNamespace("esb", "http://www.egovernment.gov.za/GMD/MessageIdentification/xml/schemas/version/7.1");
        nsmgr.AddNamespace("tnsf", @"http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/SubmitCountryByCountryDeclaration/xml/schemas/version/1.3");
        nsmgr.AddNamespace("axb", @"http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/CountryByCountryDeclaration/xml/schemas/version/1.3");
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");

        XmlNode countryByCountryDeclaration = xmlDoc.SelectSingleNode("soap12:Envelope/soap12:Body/tnsf:SubmitCountryByCountryDeclarationRequest", nsmgr);
        //XmlNode countryByCountryDeclaration = xmlDoc.SelectSingleNode("soap12:Envelope/soap12:Body/tnsf:SubmitCountryByCountryDeclarationRequest/axb:CountryByCountryDeclaration", nsmgr);
        if (countryByCountryDeclaration != null)
        {
            sbCBC = Utils.RemoveOutgoingCBCNameSpace(countryByCountryDeclaration.OuterXml); //Utils.RemoveAllNameSpaces(countryByCountryDeclaration.OuterXml);
        }
        return sbCBC;

    }

    private static XmlNode populateNode(XmlNode contactDetails, XmlDocument xdoc)
    {

        XmlElement surname = xdoc.CreateElement("Surname");
        surname.InnerText = "FDR Surname";
        XmlNode surnameNode = (XmlNode)surname;
        contactDetails.AppendChild(surnameNode);

        XmlElement fisrtName = xdoc.CreateElement("FirstNames");
        fisrtName.InnerText = "FDR Name";
        XmlNode fisrtNameNode = (XmlNode)fisrtName;
        contactDetails.AppendChild(fisrtNameNode);

        XmlElement busTelNo = xdoc.CreateElement("BusTelNo1");
        busTelNo.InnerText = "0111111111";
        XmlNode busTelNoNode = (XmlNode)busTelNo;
        contactDetails.AppendChild(busTelNoNode);


        XmlElement cellNo = xdoc.CreateElement("CellNo");
        cellNo.InnerText = "0111111111";
        XmlNode cellNoNode = (XmlNode)cellNo;
        contactDetails.AppendChild(cellNoNode);

        XmlElement emailAddress = xdoc.CreateElement("EmailAddress");
        emailAddress.InnerText = "FDR@SARS.GOV.ZA";
        XmlNode emailAddressNode = (XmlNode)emailAddress;
        contactDetails.AppendChild(emailAddressNode);

        return contactDetails;
    }

    private static string GetSqlQuery(List<string> taxRefNo, string reportingPeriod)
    {
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbValues = new StringBuilder();
        sbSql.Append("Select  [CBC] From [FDR].[dbo].[CBCDeclarations] decl");
        sbSql.Append(" INNER JOIN CBC01Data data");
        sbSql.Append(" On data.TaxRefNo = decl.TaxRefNo");
        sbSql.Append(" And data.Year = decl.TaxYear");
        sbSql.Append(string.Format(" Where  decl.[TaxYear] = {0} And ReportingPeriod={1} And decl.[TaxRefNo] In (", reportingPeriod.Substring(0, 4), "'" + reportingPeriod + "'"));
        for (int i = 0; i < taxRefNo.Count; i++)
        {
            sbValues.Append("'");
            sbValues.Append(taxRefNo[i]);
            sbValues.Append("',");
        }
        sbValues.Remove(sbValues.Length - 1, 1);
        sbSql.Append(sbValues);
        sbSql.Append(")");
        sbSql.Append(" And [IsApproved]=1");
        return sbSql.ToString();
    }
    public static string MessageRefId(string country, ref string reportingPeriod)
    {
        var results = DBReadManager.GetOutgoinCBCReport4Package(country, reportingPeriod);
        DataRow row = results.Tables[0].Rows[0];
        var oecd = XmlContent(row[0].ToString()); //, ref reportingPeriod);
        if (results.HasRows)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(oecd);
            var nodes = xmlDoc.GetElementsByTagName("CBC_OECD");
            foreach (XmlNode node in nodes)
            {
                var messageSpec = node.SelectSingleNode("MessageSpec");
                if (messageSpec != null)
                {
                    var MessageRefIdNode = messageSpec.SelectSingleNode("MessageRefId");
                    if (MessageRefIdNode != null)
                    {
                        var MessageRefId = MessageRefIdNode.InnerText;
                        return MessageRefId;
                    }
                }
            }
        }
        return string.Empty;
    }

    public static StringBuilder GenerateCorrectionPackage(string country, string reportingPeriod)
    {
        StringBuilder sbCbc = new StringBuilder();
        var newResults = DBReadManager.GetOutgoinCBCReport4Package(country, reportingPeriod);
        var oldResults = DBReadManager.GetOldOutgoingCBCData(country, reportingPeriod);
        //string corrMessageRefId = "";
        string taxRefNo = "";
        //string reportingPeriod = "";
        if (newResults.HasRows)
        {
            sbCbc.Append("<CBC_OECD urn:oecd:ties:cbc:v1>");
            sbCbc.Append("<CbcBody>");
            /*  var oldCbcBody = GetXml(oldResults, ref taxRefNo);
              var newCbcBody = GetXml(newResults, ref taxRefNo, true);
              var cbcBody = XmlCorrectionContent1(newCbcBody, oldCbcBody, ref corrMessageRefId);
              sbCbc.Append(Utils.GetMessageSpec(country, reportingPeriod, true, corrMessageRefId));
              sbCbc.Append(cbcBody);*/
            UpdateReport(taxRefNo, reportingPeriod);
            sbCbc.Append("</CbcBody>");
            sbCbc.Append("</CBC_OECD>");
        }
        return sbCbc;
    }

    private static string GetXml(DataSet dsResults)
    {
        foreach (DataRow row in dsResults.Tables[0].Rows)
        {
            var cbcBody = row[0].ToString();
            return cbcBody;
        }
        return string.Empty;
    }

    public static string XmlContent(string xml) //, ref string reportingPeriod)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        string cbcbody = "";
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
        nsmgr.AddNamespace("urn1", "urn:oecd:ties:cbc:v4");
        //var nodes = xmlDoc.GetElementsByTagName("CBC_OECD");
        foreach (XmlNode node in xmlDoc.ChildNodes)
        {
            var CbcBody = node;//.SelectSingleNode("urn:CbcBody");
            //var messageSpec = node.SelectSingleNode("MessageSpec");
            if (CbcBody != null)
            {
                cbcbody = CbcBody.InnerXml;
            }

        }

        return cbcbody;

    }



    private static string CorrectXMLReport(XmlNode newXml, XmlNode oldXml)
    {
        StringBuilder sbResults = new StringBuilder();
        if (newXml != null && oldXml != null)
        {
            var newXmlDoc = CreateNewDocument();
            newXmlDoc.LoadXml(newXml.OuterXml);

            var oldXmlDoc = CreateNewDocument();
            oldXmlDoc.LoadXml(oldXml.OuterXml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(newXmlDoc.NameTable);
            nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
            nsmgr.AddNamespace("urn1", "urn:oecd:ties:cbc:v4");

            XmlNamespaceManager nsmgr1 = new XmlNamespaceManager(oldXmlDoc.NameTable);
            nsmgr1.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
            nsmgr1.AddNamespace("urn1", "urn:oecd:ties:cbc:v4");

            var newReportingEntity = newXmlDoc.SelectSingleNode("CbcBody/ReportingEntity");
            if (newReportingEntity == null)
            {
                newReportingEntity = newXmlDoc.SelectSingleNode("urn:CbcBody/urn:ReportingEntity", nsmgr);
            }
            var newReports = newXmlDoc.SelectNodes("CbcBody/CbcReports");
            if (newReports.Count == 0)
            {
                newReports = newXmlDoc.SelectNodes("urn:CbcBody/urn:CbcReports", nsmgr);
            }
            var newAdditionalInfo = newXmlDoc.SelectNodes("CbcBody/AdditionalInfo");
            if (newAdditionalInfo.Count == 0)
            {
                newAdditionalInfo = newXmlDoc.SelectNodes("urn:CbcBody/urn:AdditionalInfo", nsmgr);
            }

            var oldReportingEntity = oldXmlDoc.SelectSingleNode("CbcBody/ReportingEntity");
            if (oldReportingEntity == null)
            {
                oldReportingEntity = oldXmlDoc.SelectSingleNode("urn:CbcBody/urn:ReportingEntity", nsmgr1);
                if (oldReportingEntity == null)
                {
                    oldReportingEntity = oldXmlDoc.SelectSingleNode("CbcBody/urn:ReportingEntity", nsmgr1);

                }

            }
            var oldReports = oldXmlDoc.SelectNodes("CbcBody/CbcReports");
            if (oldReports.Count == 0)
            {
                oldReports = oldXmlDoc.SelectNodes("urn:CbcBody/urn:CbcReports", nsmgr1);
            }
            var oldAdditionalInfo = oldXmlDoc.SelectNodes("CbcBody/AdditionalInfo");
            if (oldAdditionalInfo.Count == 0)
            {
                oldAdditionalInfo = oldXmlDoc.SelectNodes("urn:CbcBody/urn:AdditionalInfo", nsmgr1);
            }

            var entity = CorrectedEntity(newReportingEntity, oldReportingEntity, newXmlDoc, nsmgr, nsmgr1);
            if (!string.IsNullOrEmpty(entity.ToString()))
            {
                sbResults.Append(entity.ToString());
            }

            if (newReports.Count == oldReports.Count)
            {
                for (int i = 0; i < newReports.Count; i++)
                {
                    var reports = CorrectContainer(newReports[i], oldReports[i], newXmlDoc, nsmgr, nsmgr1);
                    if (!string.IsNullOrEmpty(reports.ToString()))
                    {
                        sbResults.Append(reports);
                    }
                }
            }
            else if (newReports.Count > oldReports.Count)
            {
                for (int i = 0; i < newReports.Count; i++)
                {
                    StringBuilder reports;
                    if (i < newReports.Count)
                    {
                        reports = CorrectContainer(newReports[i], oldReports[i], newXmlDoc, nsmgr, nsmgr1);
                    }
                    else
                    {
                        reports = CorrectContainer(newReports[i], null, newXmlDoc, nsmgr, nsmgr1);
                    }

                    if (!string.IsNullOrEmpty(reports.ToString()))
                    {
                        sbResults.Append(reports);
                    }
                }
            }

            if (newAdditionalInfo.Count == oldAdditionalInfo.Count)
            {
                for (int i = 0; i < newAdditionalInfo.Count; i++)
                {
                    var additioanlInfo = CorrectContainer(newAdditionalInfo[i], oldAdditionalInfo[i], newXmlDoc, nsmgr, nsmgr1);
                    if (!string.IsNullOrEmpty(additioanlInfo.ToString()))
                    {
                        sbResults.Append(additioanlInfo);
                    }
                }
            }
            else if (newAdditionalInfo.Count > oldAdditionalInfo.Count)
            {
                for (int i = 0; i < newReports.Count; i++)
                {
                    StringBuilder additioanlInfo;
                    if (i < newReports.Count)
                    {
                        additioanlInfo = CorrectContainer(newAdditionalInfo[i], oldAdditionalInfo[i], newXmlDoc, nsmgr, nsmgr1);
                    }
                    else
                    {
                        additioanlInfo = CorrectContainer(newAdditionalInfo[i], null, newXmlDoc, nsmgr, nsmgr1);
                    }
                    if (!string.IsNullOrEmpty(additioanlInfo.ToString()))
                    {
                        sbResults.Append(additioanlInfo);
                    }
                }
            }
        }
        return sbResults.ToString();

    }
    private static StringBuilder CorrectContainer(XmlNode newNode, XmlNode oldNode, XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlNamespaceManager nsmgr1)
    {
        StringBuilder sbResults = new StringBuilder();
        var docSpec = newNode.SelectSingleNode("DocSpec");


        if (docSpec == null)
        {
            docSpec = newNode.SelectSingleNode("urn:DocSpec", nsmgr);
            if (docSpec == null)
            {
                docSpec = newNode.SelectSingleNode("urn1:DocSpec", nsmgr);
            }
        }


        if (docSpec != null)
        {
            var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
            if (docTypeind == null)
            {
                docTypeind = docSpec.SelectSingleNode("urn:DocTypeIndic", nsmgr);
                if (docTypeind == null)
                {
                    docTypeind = docSpec.ChildNodes[0];
                }
            }
            var docRefId = docSpec.SelectSingleNode("DocRefId");
            if (docRefId == null)
            {
                docRefId = docSpec.ChildNodes[1];
                if (docRefId == null)
                {
                    docRefId = docSpec.SelectSingleNode("urn1:DocRefId", nsmgr);
                }
            }


            XmlNode oldDocrefId = null;
            XmlNode oldDocTypeIndic = null;
            if (oldNode != null)
            {
                var oldDocSpec = oldNode.SelectSingleNode("DocSpec");
                if (oldDocSpec == null)
                {
                    oldDocSpec = oldNode.SelectSingleNode("urn:DocSpec", nsmgr);
                    if (oldDocSpec == null)
                    {
                        oldDocSpec = oldNode.SelectSingleNode("urn1:DocSpec", nsmgr);
                    }
                }

                if (oldDocSpec != null)
                {
                    oldDocrefId = oldDocSpec.SelectSingleNode("DocRefId");
                    oldDocrefId = oldNode.SelectSingleNode("urn:DocRefId", nsmgr);
                    if (oldDocrefId == null)
                    {
                        oldDocrefId = oldNode.SelectSingleNode("urn1:DocRefId", nsmgr);
                    }
                    else
                    {
                        oldDocrefId = oldNode.ChildNodes[1];
                    }
                    oldDocTypeIndic = oldNode.FirstChild;
                }

            }
            bool isNodesEquals = Utils.IsXmlNodesEqual(newNode, oldNode); ;
            if (docTypeind != null && docRefId != null)
            {
                isNodesEquals = Utils.IsXmlNodesEqual(newNode, oldNode);
                if (!isNodesEquals)
                {
                    docTypeind.InnerText = (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString() &&
                    oldDocTypeIndic.InnerText != FDREnums.LifeSubmittingType.OECD3.ToString()) ?
                    FDREnums.LifeSubmittingType.OECD3.ToString() : FDREnums.LifeSubmittingType.OECD2.ToString();
                    var CorrDocRefID = oldDocrefId == null ? docRefId.InnerText : oldDocrefId.InnerText;
                    XmlElement corrDocRefId = xmlDoc.CreateElement("CorrDocRefID");
                    corrDocRefId.SetAttribute("urn", "urn: oecd:ties: cbc:v4");
                    corrDocRefId.InnerText = CorrDocRefID;
                    XmlNode newCorrDocRefId = (XmlNode)corrDocRefId;
                    docSpec.AppendChild(newCorrDocRefId);

                }
            }

        }
        sbResults.Append(newNode.OuterXml);
        return sbResults;
    }


    private static StringBuilder CorrectedEntity(XmlNode newEntity, XmlNode oldEntity, XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlNamespaceManager nsmgr1)
    {
        var docSpec = newEntity.SelectSingleNode("DocSpec");
        var oldDocSpec = oldEntity.SelectSingleNode("DocSpec");

        if (docSpec == null)
        {
            docSpec = newEntity.SelectSingleNode("urn:DocSpec", nsmgr);
            if (docSpec == null)
            {
                docSpec = newEntity.SelectSingleNode("urn1:DocSpec", nsmgr);
            }
        }

        if (oldDocSpec == null)
        {
            oldDocSpec = newEntity.SelectSingleNode("urn:DocSpec", nsmgr);
            if (oldDocSpec == null)
            {
                oldDocSpec = newEntity.SelectSingleNode("urn1:DocSpec", nsmgr);
            }
        }
        StringBuilder sbResults = new StringBuilder();
        if (docSpec != null)
        {
            bool isNodesEquals = Utils.IsXmlNodesEqual(newEntity, oldEntity);
            var docTypeind = docSpec.FirstChild;
            //SelectSingleNode("DocTypeIndic");
            if (docTypeind == null)
            {
                docTypeind = docSpec.SelectSingleNode("urn:DocTypeIndic", nsmgr);
                if (docTypeind == null)
                {
                    docTypeind = docSpec.SelectSingleNode("urn1:DocTypeIndic", nsmgr);
                }
            }
            var docRefId = docSpec.LastChild;
            //SelectSingleNode("DocRefId");
            if (docRefId == null)
            {
                docRefId = docSpec.SelectSingleNode("urn:DocRefId", nsmgr);
                if (docRefId == null)
                {
                    docRefId = docSpec.SelectSingleNode("urn1:DocRefId", nsmgr);
                }
            }
            XmlNode oldDocrefId = null;
            XmlNode oldDocTypeIndic = null;
            if (oldDocSpec != null)
            {
                oldDocrefId = oldDocSpec.ChildNodes[1];
                if (oldDocrefId == null)
                {
                    oldDocrefId = oldDocSpec.ChildNodes[1];
                }
                oldDocTypeIndic = oldDocSpec.FirstChild;
            }

            if (docTypeind != null && docRefId != null)
            {
                var newDocRefId = docRefId.InnerText;
                docTypeind.InnerText = (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString() &&
                    oldDocTypeIndic.InnerText != FDREnums.LifeSubmittingType.OECD3.ToString()) ?
                    FDREnums.LifeSubmittingType.OECD3.ToString() : FDREnums.LifeSubmittingType.OECD2.ToString();
                XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                childElement.SetAttribute("xmlns", "urn:oecd:ties:cbc:v4");
                childElement.InnerText = oldDocrefId.InnerText;
                XmlNode newChild = (XmlNode)childElement;

                docSpec.AppendChild(newChild);
            }


        }
        sbResults.Append(newEntity.OuterXml);
        return sbResults;
    }

    private static string XmlCorrectionContent(string newXml, string oldXml)
    {

        var newXmlDoc = new XmlDocument();
        newXmlDoc.LoadXml(newXml);
        string cbcbody = "";
        var nodes = newXmlDoc.GetElementsByTagName("CBC_OECD");
        StringBuilder sbResults = new StringBuilder();
        foreach (XmlNode node in nodes)
        {
            var CbcBody = node.SelectSingleNode("CbcBody");
            var messageSpec = node.SelectSingleNode("MessageSpec");
            var corrMessageRefId = GetMessageRefId(messageSpec);
            if (CbcBody != null)
            {
                cbcbody = CbcBody.OuterXml;
                var reportingEntity = CbcBody.SelectSingleNode("ReportingEntity");
                var reports = CbcBody.SelectNodes("CbcReports");
                var additionalInfo = CbcBody.SelectNodes("AdditionalInfo");
                if (reportingEntity != null)
                {
                    //reporting entity
                    if (reportingEntity != null)
                    {
                        var reportingEntityXml = CheckEntity(reportingEntity, newXmlDoc, corrMessageRefId);
                        if (!string.IsNullOrEmpty(reportingEntityXml.ToString()))
                            sbResults.Append(reportingEntityXml.ToString());
                    }
                }

                //if there is 1 or more cbcReports
                if (reports != null)
                {
                    var reportsXml = CheckContainerForChange(reports, newXmlDoc, corrMessageRefId);
                    if (!string.IsNullOrEmpty(reportsXml.ToString()))
                        sbResults.Append(reportsXml.ToString());
                }

                //if there is 1 or more additionalInfo
                if (additionalInfo != null)
                {
                    var additionalInfoXml = CheckContainerForChange(additionalInfo, newXmlDoc, corrMessageRefId);
                    if (!string.IsNullOrEmpty(additionalInfoXml.ToString()))
                        sbResults.Append(additionalInfoXml.ToString());
                }
            }

        }

        return sbResults.ToString();

    }

    public static string GetMessageRefId(XmlNode messageSpec)
    {
        if (messageSpec != null)
        {
            var docSpec = messageSpec.SelectSingleNode("MessageRefId");
            return docSpec.InnerText;
        }
        return "";
    }

    private static StringBuilder CheckEntity(XmlNode entity, XmlDocument xmlDoc, string corrMessageRefId)
    {
        var docSpec = entity.SelectSingleNode("DocSpec");
        StringBuilder sbResults = new StringBuilder();
        if (docSpec != null)
        {
            var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
            var docRefId = docSpec.SelectSingleNode("DocRefId");
            if (docTypeind != null && docRefId != null)
            {
                bool life = Convert.ToBoolean(WebConfigurationManager.AppSettings["LifeEnvironment"].ToString());
                if (life)
                {
                    //new record
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD1.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length == 1)
                        {

                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            // corrMessageRefId
                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);


                            sbResults.Append(entity.OuterXml);
                        }
                    }

                    // change 
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD2.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            // corrMessageRefId
                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);

                            sbResults.Append(entity.OuterXml);
                        }
                    }

                    // deletion
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            // corrMessageRefId
                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);
                            sbResults.Append(entity.OuterXml);
                        }
                    }
                }
                else
                {
                    if (docTypeind.InnerText == FDREnums.TestSubmittingType.OECD12.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            // corrMessageRefId
                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);

                            sbResults.Append(entity.OuterXml);
                        }
                    }
                }
            }

        }
        return sbResults;
    }
    private static StringBuilder CheckContainerForChange(XmlNodeList nodes, XmlDocument xmlDoc, string corrMessageRefId)
    {


        StringBuilder sbResults = new StringBuilder();
        foreach (XmlNode node in nodes)
        {
            var docSpec = node.SelectSingleNode("DocSpec");
            if (docSpec != null)
            {
                var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
                var docRefId = docSpec.SelectSingleNode("DocRefId");

                if (docTypeind != null && docRefId != null)
                {
                    //new record
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD1.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length == 1)
                        {
                            /* XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                             childElement.InnerText = dRefId[1];
                             XmlNode newChild = (XmlNode)childElement;
                             docSpec.AppendChild(newChild);*/
                            docRefId.InnerText = dRefId[0];

                            // corrMessageRefId
                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);


                            sbResults.Append(node.OuterXml);
                        }
                    }

                    // change 
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD2.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);
                            sbResults.Append(node.OuterXml);
                        }
                    }

                    // deletion
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);
                            docRefId.InnerText = dRefId[0];

                            XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                            messageRefId.InnerText = corrMessageRefId;
                            XmlNode newChildNode = (XmlNode)messageRefId;
                            docSpec.AppendChild(newChildNode);
                            sbResults.Append(node.OuterXml);
                        }
                    }


                }
            }
        }

        return sbResults;
    }


    private static void UpdateReport(string taxRef, string reportingPeriod)
    {
        int success = DatabaseWriter.UpdateCBCRS_To_Process(taxRef, reportingPeriod);
    }

}


public class GridDecorator
{
    public static void MergeRows(System.Web.UI.WebControls.GridView gridView)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            System.Web.UI.WebControls.GridViewRow row = gridView.Rows[rowIndex];
            System.Web.UI.WebControls.GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                {
                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                           previousRow.Cells[i].RowSpan + 1;
                    previousRow.Cells[i].Visible = false;
                }
            }
        }
    }
}

public static class FDREnums
{
    public enum LifeSubmittingType
    {
        OECD0,
        OECD1,
        OECD2,
        OECD3,
    }

    public enum TestSubmittingType
    {
        OECD10,
        OECD11,
        OECD12,
        OECD13,
    }
}