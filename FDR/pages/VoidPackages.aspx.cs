using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Linq;
using System.Collections.Generic;
using Sars.Systems.Security;
using Sars.Systems.Data;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["prevPage"] = Request.UrlReferrer;
            Common.GetAllSentPackageCountries(ddlCountryList);
            ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(), "ReportingPeriod", "ID");
            LoadCBC();
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string reportingPeriod = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DBReadManager.GetPackagesToVoid(countryCode, reportingPeriod);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object LoadCBCHistory(string countryCode, string reportingPeriod)
    {
        var cbcr = DBReadManager.GetOutgoingPackageHistory(countryCode.Trim(), reportingPeriod);
        string jsonString = "";
        if (cbcr.HasRows)
        {
            var lst = cbcr.Tables[0].AsEnumerable()
                .Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                ).ToDictionary(z => z.Key, z => z.Value)
                ).ToList();
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsonString = serializer.Serialize(lst);
        };
        return jsonString;
    }


    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkSelectAll = e.Row.FindControl("chkHeader") as CheckBox;
        }
        else
        {
            Button btnVoid = (e.Row.FindControl("btnVoid") as Button);
            if (e.Row != null)
            {

            }
        }
    }


    protected void chkEnableCountry_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkEnableCountry = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkEnableCountry.Parent.Parent;
        Button btnSave = row.FindControl("btnSave") as Button;
        Button btnGenerate = row.FindControl("btnGenerate") as Button;
        if (chkEnableCountry.Checked)
        {
            btnSave.Enabled = true;
            btnGenerate.Enabled = true;
        }
        else
        {
            btnSave.Enabled = false;
            btnGenerate.Enabled = false;
        }
    }




    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&Period=" + period);
    }

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {
        Guid newUid = new Guid();
        Button btnVoid = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnVoid.Parent.Parent;
        var email = ADUser.CurrentUser.Mail;
        string[] senderEmail = { email };
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var reportingPeriod = gvRow.Cells[1].Text; var outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
        var id = string.IsNullOrEmpty(outCBC.Id.ToString()) ? 0 : outCBC.Id;
        var reportP = DateTime.Parse(reportingPeriod).ToString("yyyy-MM-ddTHH:mm:ss");
        Guid uid = new Guid();
        if (outCBC != null)
        {
            var prepareVoidPackage = PrepareNMVoidPackage(id, countryCode, outCBC.NMCBC, reportingPeriod);
            var nmPackage = prepareVoidPackage;
            uid = outCBC.UID;
            var voidedCBC = new OutGoingCBCDeclarations()
            {
                Id = id,
                Country = countryCode,
                CBCData = prepareVoidPackage,
                StatusId = 8,
                Year = int.Parse(reportingPeriod.Substring(0, 4)),
                UID = uid,
                ActionId = 3,
                NSCBCData = nmPackage,
                ReportingPeriod = DateTime.Parse(reportP),
                CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
            };
            decimal saved = DatabaseWriter.SaveOutgoingCBC(voidedCBC, ref uid);
            DBWriteManager.ApproveOutgoingPackage(id, countryCode, reportingPeriod, 8, ADUser.CurrentSID);
            DBWriteManager.Insert_OutgoingPackageAuditTrail(outCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} Pending Void Review", gvRow.Cells[0].Text.Split('-')[0].Trim()));
            var Subject = "Outgoing Package pending void review ";
            Common.SendEmailToRole("Reviewer", outCBC.CountryName, Subject, FDRPage.Statuses.Voided, senderEmail);
            MessageBox.Show("Package pending void review");
            LoadCBC();
        }
    }



    private string PrepareVoidPackage(decimal id, string countryCode, string xml, string reportingPeriod)
    {
        var xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(xml);
        var finalDocument = Common.CreateNewDocument();
        var newOecdNode = finalDocument.CreateNode(XmlNodeType.Element, "CBC_OECD", null);
        var originalMesageRefId = finalDocument.CreateNode(XmlNodeType.Element, "CorrMessageRefId", null);
        XmlNode oecdNode = xmlDoc.SelectSingleNode("CBC_OECD");
        if (oecdNode != null)
        {
            XmlNodeList cbcBody = oecdNode.SelectNodes("CbcBody");
            var newMessageSpec = Common.CreateMessageSpec(finalDocument, countryCode, reportingPeriod, id, originalMesageRefId.InnerText);
            var messageSpec = oecdNode.SelectSingleNode("MessageSpec");
            var oldMessageRefId = Common.GetOriginalMessageRefId(messageSpec);
            //originalMesageRefId.InnerText = oldMessageRefId.InnerText;
            //newMessageSpec.AppendChild(docTypeInd);
            //newMessageSpec.AppendChild(originalMesageRefId);
            newOecdNode.AppendChild(newMessageSpec);

            foreach (XmlNode node in cbcBody)
            {
                if (node != null)
                {
                    var newBody = finalDocument.CreateNode(XmlNodeType.Element, "CbcBody", null);
                    var newCbcBody = XmlDeletionContent(id, originalMesageRefId.InnerText, node.OuterXml.ToString(), reportingPeriod);
                    newBody.InnerXml = newCbcBody.ToString();
                    newOecdNode.AppendChild(newBody);
                }
            }
            finalDocument.AppendChild(newOecdNode);
        }

        return finalDocument.OuterXml;
    }

    private string PrepareNMVoidPackage(decimal id, string countryCode, string xml, string reportingPeriod)
    {
        var xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(xml);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");

        var finalDocument = Common.CreateNewDocument();
        var newOecdNode = finalDocument.CreateNode(XmlNodeType.Element, "CBC_OECD", finalDocument.NamespaceURI);
        var originalMesageRefId = finalDocument.CreateNode(XmlNodeType.Element, "CorrMessageRefId", finalDocument.NamespaceURI);

        //XmlNode oecdNode = xmlDoc.SelectSingleNode("urn:CBC_OECD", nsmgr);
        XmlNode oecdNode = xmlDoc.SelectSingleNode("CBC_OECD");
        if (oecdNode == null)
        {
            oecdNode = xmlDoc.SelectSingleNode("urn:CBC_OECD", nsmgr);
        }
        if (oecdNode != null)
        {
            //XmlNodeList cbcBody = oecdNode.SelectNodes("urn:CbcBody", nsmgr);
            XmlNodeList cbcBody = oecdNode.SelectNodes("CbcBody");
            if (cbcBody.Count == 0)
            {
                cbcBody = oecdNode.SelectNodes("urn:CbcBody", nsmgr);
            }
            var newMessageSpec = Common.CreateMessageSpec(finalDocument, countryCode, reportingPeriod, id);
            var messageSpec = oecdNode.SelectSingleNode("MessageSpec");
            if (messageSpec == null)
            {
                messageSpec = oecdNode.SelectSingleNode("urn:MessageSpec", nsmgr);
            }
            newOecdNode.AppendChild(newMessageSpec);

            foreach (XmlNode node in cbcBody)
            {
                if (node != null)
                {
                    var newBody = finalDocument.CreateNode(XmlNodeType.Element, "CbcBody", finalDocument.NamespaceURI);
                    var newCbcBody = XmlNMDeletionContent(id, originalMesageRefId.InnerText, node.OuterXml.ToString(), reportingPeriod);
                    newBody.InnerXml = newCbcBody.ToString();
                    newOecdNode.AppendChild(newBody);
                }
            }
            finalDocument.AppendChild(newOecdNode);
        }

        return finalDocument.OuterXml;
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    #region xmlWithNameSpace
    private static string XmlNMDeletionContent(decimal id, string corrMessageRefId, string xml, string reportingPeriod)
    {

        var xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(xml);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("urn", "urn:oecd:ties:cbc:v1");
        nsmgr.AddNamespace("stf", "urn:oecd:ties:stf:v4");
        var cbcDocument = Common.CreateNewDocument();
        var cbcBody = cbcDocument.CreateNode(XmlNodeType.Element, "CbcBody", cbcDocument.NamespaceURI);
        //var CbcBodies = xmlDoc.SelectNodes("urn:CbcBody", nsmgr);
        var CbcBodies = xmlDoc.SelectNodes("CbcBody");
        if (CbcBodies.Count == 0)
            CbcBodies = xmlDoc.SelectNodes("urn:CbcBody", nsmgr);
        if (CbcBodies != null)
        {
            foreach (XmlNode CbcBody in CbcBodies)
            {
                var reportingEntity = CbcBody.SelectSingleNode("urn:ReportingEntity", nsmgr);
                var reports = CbcBody.SelectNodes("urn:CbcReports", nsmgr);
                var additionalInfo = CbcBody.SelectNodes("urn:AdditionalInfo", nsmgr);
                if (reportingEntity != null)
                {
                    //reporting entity
                    var newreportingEntity = cbcDocument.CreateNode(XmlNodeType.Element, "ReportingEntity", "urn:oecd:ties:cbc:v1");
                    var reportingEntityXml = CheckNMEntity(id, reportingEntity, corrMessageRefId, nsmgr);
                    if (!string.IsNullOrEmpty(reportingEntityXml.ToString()))
                    {
                        newreportingEntity.InnerXml = reportingEntityXml.InnerXml;
                        cbcBody.AppendChild(newreportingEntity);
                    }
                }

                //if there is 1 or more cbcReports
                if (reports != null)
                {
                    var newReport = cbcDocument.CreateNode(XmlNodeType.Element, "CbcReports", "urn:oecd:ties:cbc:v1");
                    var reportsXml = CheckNMContainerForDeletion(id, reports, corrMessageRefId, xmlDoc, nsmgr);
                    if (!string.IsNullOrEmpty(reportsXml.ToString()))
                    {
                        GetReportsAndAdditionalInfo(BuildXml(reportsXml.ToString()), ref cbcBody);

                    }
                }

                //if there is 1 or more additionalInfo
                if (additionalInfo != null)
                {
                    var newAdditionalInfo = cbcDocument.CreateNode(XmlNodeType.Element, "AdditionalInfo", "urn:oecd:ties:cbc:v1");
                    var additionalInfoXml = CheckNMContainerForDeletion(id, additionalInfo, corrMessageRefId, xmlDoc, nsmgr);
                    if (!string.IsNullOrEmpty(additionalInfoXml.ToString()))
                    {
                        GetReportsAndAdditionalInfo(BuildXml(additionalInfoXml.ToString()), ref cbcBody);
                    }
                }
            }
        }

        return cbcBody.InnerXml;

    }

    private static string BuildXml(string xml)
    {
        StringBuilder sbXml = new StringBuilder();
        sbXml.Append("<root>");
        sbXml.Append(xml.ToString());
        sbXml.Append("</root>");

        return sbXml.ToString();
    }

    private static void GetReportsAndAdditionalInfo(string xml, ref XmlNode cbcNode)
    {
        var doc = Common.CreateNewDocument();
        doc.LoadXml(xml);
        //gets CBC Reports and Additional Info Array
        var root = doc.FirstChild;
        if (root.HasChildNodes)
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                var importNode = cbcNode.OwnerDocument.ImportNode(root.ChildNodes[i], true);
                cbcNode.AppendChild(importNode);
            }

    }
    private static XmlNode CheckNMEntity(decimal id, XmlNode entity, string corrMessageRefId, XmlNamespaceManager nsmgr)
    {

        var entityDoc = Common.CreateNewDocument();
        var uniqueNo = Utils.GenerateUniqueNo();
        string uniqNo = id == 0 ? uniqueNo : id.ToString() + uniqueNo;
        var newEntity = entityDoc.CreateNode(XmlNodeType.Element, "ReportingEntity", "urn");
        newEntity.InnerXml = entity.InnerXml;
        var docSpec = newEntity.SelectSingleNode("urn:DocSpec", nsmgr);
        if (docSpec != null)
        {
            var docTypeind = docSpec.SelectSingleNode("urn:DocTypeIndic", nsmgr);
            var docRefId = docSpec.SelectSingleNode("urn:DocRefId", nsmgr);
            if (docRefId == null && docTypeind == null)
            {
                docRefId = docSpec.SelectSingleNode("stf:DocRefId", nsmgr);
                docTypeind = docSpec.SelectSingleNode("stf:DocTypeIndic", nsmgr);
            }
            if (docTypeind != null && docRefId != null)
            {
                // deletion
                var oldDocRefId = docRefId.InnerText;
                string[] fPart = oldDocRefId.Split('-');
                docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                docRefId.InnerText = fPart[0] + '-' + uniqNo;
                //corrDocRefId
                var corrDocRefId = entityDoc.CreateNode(XmlNodeType.Element, "CorrDocRefID", "urn:oecd:ties:stf:v4");
                corrDocRefId.InnerText = oldDocRefId;
                docSpec.AppendChild(corrDocRefId);

                //corrMessageRefId
                /* var messageRefId = entityDoc.CreateNode(XmlNodeType.Element, "CorrMessageRefID", "urn");
                 messageRefId.InnerText = corrMessageRefId;
                 docSpec.AppendChild(messageRefId);*/

            }

        }

        return newEntity;
    }

    private static StringBuilder CheckNMContainerForDeletion(decimal id, XmlNodeList nodes, string corrMessageRefId, XmlDocument xmlDoc, XmlNamespaceManager nsmgr)
    {
        StringBuilder sbNodes = new StringBuilder();
        foreach (XmlNode node in nodes)
        {

            var uniqueNo = Utils.GenerateUniqueNo();
            string uniqNo = id == 0 ? uniqueNo : id.ToString() + uniqueNo;
            var docSpec = node.SelectSingleNode("urn:DocSpec", nsmgr);
            if (docSpec != null)
            {
                var docTypeind = docSpec.SelectSingleNode("urn:DocTypeIndic", nsmgr);
                var docRefId = docSpec.SelectSingleNode("urn:DocRefId", nsmgr);
                if (docRefId == null && docTypeind == null)
                {
                    docRefId = docSpec.SelectSingleNode("stf:DocRefId", nsmgr);
                    docTypeind = docSpec.SelectSingleNode("stf:DocTypeIndic", nsmgr);
                }
                if (docTypeind != null && docRefId != null)
                {
                    var oldDocRefId = docRefId.InnerText;
                    string[] fPart = oldDocRefId.Split('-');
                    docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                    docRefId.InnerText = fPart[0] + '-' + uniqNo;
                    //corrDocRefId
                    var corrDocRefId = xmlDoc.CreateNode(XmlNodeType.Element, "CorrDocRefID", "urn:oecd:ties:stf:v4");
                    corrDocRefId.InnerText = oldDocRefId;
                    docSpec.AppendChild(corrDocRefId);

                    //corrMessageRefId
                    /* var messageRefId = xmlDoc.CreateNode(XmlNodeType.Element, "CorrMessageRefID", "urn:oecd:ties:stf:v4");
                     messageRefId.InnerText = corrMessageRefId;
                     docSpec.AppendChild(messageRefId);*/
                }
                sbNodes.Append(node.OuterXml);
            }
        }


        return sbNodes;
    }


    #endregion

    private static string XmlDeletionContent(decimal id, string corrMessageRefId, string xml, string reportingPeriod)
    {

        var xmlDoc = Common.CreateNewDocument();
        xmlDoc.LoadXml(xml);
        var cbcDocument = Common.CreateNewDocument();
        var cbcBody = cbcDocument.CreateNode(XmlNodeType.Element, "CbcBody", null);
        var CbcBodies = xmlDoc.SelectNodes("CbcBody");
        if (CbcBodies != null)
        {
            foreach (XmlNode CbcBody in CbcBodies)
            {
                var reportingEntity = CbcBody.SelectSingleNode("ReportingEntity");
                var reports = CbcBody.SelectNodes("CbcReports");
                var additionalInfo = CbcBody.SelectNodes("AdditionalInfo");
                if (reportingEntity != null)
                {
                    //reporting entity
                    var newreportingEntity = cbcDocument.CreateNode(XmlNodeType.Element, "ReportingEntity", null);
                    var reportingEntityXml = CheckEntity(id, reportingEntity, corrMessageRefId);
                    if (!string.IsNullOrEmpty(reportingEntityXml.ToString()))
                    {
                        newreportingEntity.InnerXml = reportingEntityXml.InnerXml;
                        cbcBody.AppendChild(newreportingEntity);
                    }
                }

                //if there is 1 or more cbcReports
                if (reports != null)
                {
                    var newReport = cbcDocument.CreateNode(XmlNodeType.Element, "CbcReports", null);
                    var reportsXml = CheckContainerForDeletion(id, reports, corrMessageRefId, xmlDoc);
                    if (!string.IsNullOrEmpty(reportsXml.ToString()))
                    {
                        GetReportsAndAdditionalInfo(BuildXml(reportsXml.ToString()), ref cbcBody);
                    }
                }

                //if there is 1 or more additionalInfo
                if (additionalInfo != null)
                {
                    var newAdditionalInfo = cbcDocument.CreateNode(XmlNodeType.Element, "AdditionalInfo", null);
                    var additionalInfoXml = CheckContainerForDeletion(id, additionalInfo, corrMessageRefId, xmlDoc);
                    if (!string.IsNullOrEmpty(additionalInfoXml.ToString()))
                    {
                        GetReportsAndAdditionalInfo(BuildXml(additionalInfoXml.ToString()), ref cbcBody);
                    }
                }
            }
        }

        return cbcBody.InnerXml;

    }

    private static XmlNode CheckEntity(decimal id, XmlNode entity, string corrMessageRefId)
    {

        var entityDoc = Common.CreateNewDocument();
        var uniqueNo = Utils.GenerateUniqueNo();
        StringBuilder sbNode = new StringBuilder();
        string uniqNo = id == 0 ? uniqueNo : id.ToString() + uniqueNo;
        var newEntity = entityDoc.CreateNode(XmlNodeType.Element, "ReportingEntity", null);
        //newEntity.InnerXml = entity.InnerXml;
        var docSpec = newEntity.SelectSingleNode("DocSpec");
        if (docSpec != null)
        {
            var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
            var docRefId = docSpec.SelectSingleNode("DocRefId");
            if (docTypeind != null && docRefId != null)
            {
                // deletion
                var oldDocRefId = docRefId.InnerText;
                string[] fPart = oldDocRefId.Split('-');
                docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                docRefId.InnerText = fPart[0] + '-' + uniqNo;
                //corrDocRefId
                var corrDocRefId = entityDoc.CreateNode(XmlNodeType.Element, "CorrDocRefID", null);
                corrDocRefId.InnerText = oldDocRefId;
                docSpec.AppendChild(corrDocRefId);

                //corrMessageRefId
                var messageRefId = entityDoc.CreateNode(XmlNodeType.Element, "CorrMessageRefId", null);
                messageRefId.InnerText = corrMessageRefId;
                docSpec.AppendChild(messageRefId);


            }
        }
        sbNode.Append(entity.InnerXml);
        newEntity.InnerXml = sbNode.ToString();
        return newEntity;
    }
    private static StringBuilder CheckContainerForDeletion(decimal id, XmlNodeList nodes, string corrMessageRefId, XmlDocument xmlDoc)
    {
        StringBuilder sbNodes = new StringBuilder();
        foreach (XmlNode node in nodes)
        {

            var uniqueNo = Utils.GenerateUniqueNo();
            string uniqNo = id == 0 ? uniqueNo : id.ToString() + uniqueNo;

            var docSpec = node.SelectSingleNode("DocSpec");
            if (docSpec != null)
            {
                var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
                var docRefId = docSpec.SelectSingleNode("DocRefId");
                if (docTypeind != null && docRefId != null)
                {
                    var oldDocRefId = docRefId.InnerText;
                    string[] fPart = oldDocRefId.Split('-');
                    docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                    docRefId.InnerText = fPart[0] + '-' + uniqNo;

                    var corrDocRefId = xmlDoc.CreateNode(XmlNodeType.Element, "CorrDocRefID", null);
                    corrDocRefId.InnerText = oldDocRefId;
                    docSpec.AppendChild(corrDocRefId);
                    //CorrMessageRefId
                    var messageRefId = xmlDoc.CreateNode(XmlNodeType.Element, "CorrMessageRefId", null);
                    messageRefId.InnerText = corrMessageRefId;
                    docSpec.AppendChild(messageRefId);


                }

                sbNodes.Append(node.OuterXml);
            }
        }

        return sbNodes;
    }

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    protected void ddlCountryList_OptionChanged(Sars.Systems.Controls.CountryList.CountryListFieldEventArgs e)
    {
        LoadCBC();
    }

    protected void lnkViewHistory_Click(object sender, EventArgs e)
    {
        LinkButton lbViewHistory = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lbViewHistory.Parent.Parent;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var reportingPeriod = gvRow.Cells[1].Text;
        LoadCBCHistory(countryCode, reportingPeriod);
    }



    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void txtReportingPeriod_TextChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var coutryCode = ddlCountryList.SelectedValue;
        ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(coutryCode), "ReportingPeriod", "ID");
        LoadCBC();
    }

    protected void ddlReportingPeriod_TextChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }
}