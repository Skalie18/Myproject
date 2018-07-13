using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Services;
using System.Web.Script.Services;
using System.Linq;
using System.Collections.Generic;
using Sars.Systems.Security;

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
        string reportingPeriod = ddlReportingPeriod.SelectedIndex>0? ddlReportingPeriod.SelectedValue :null;
        var cbcr = DBReadManager.GetCorrectedCBCReport();
        gvCBC.Bind(cbcr);
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
        // gvCBCHistory.Bind(cbcr);
    }
   

    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
   protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var year = gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&Period=" + year);
    }

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void btnCorrection_Click(object sender, EventArgs e)
    {
        Button btnCorrection = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnCorrection.Parent.Parent;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[1].Text;

        var url = "Corrections.aspx?ccode=" + countryCode + " &period=" + period;

        string popWindow = "window.open('" + url + "', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", popWindow, true);
    }


    private StringBuilder VoidPackage(string country, string reportingPeriod)
    {
        StringBuilder sbCbc = new StringBuilder();
        var results = DBReadManager.GetPackageForDeletion(country, reportingPeriod);
        if (results.HasRows)
        {
            foreach (DataRow row in results.Tables[0].Rows)
            {
                var cbcBody = XmlDeletionContent(row[0].ToString());
                if (!string.IsNullOrEmpty(cbcBody))
                    sbCbc.Append(cbcBody);
            }
        }

        return sbCbc;
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    private static string XmlDeletionContent(string xml)
    {

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var nodes = xmlDoc.GetElementsByTagName("CBC_OECD");
        StringBuilder sbResults = new StringBuilder();
        foreach (XmlNode node in nodes)
        {
            var CbcBodies = node.SelectNodes("CbcBody");
            var messageSpec = node.SelectSingleNode("MessageSpec");
            var corrMessageRefId = GetMessageRefId(messageSpec);
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
                        if (reportingEntity != null)
                        {
                            var reportingEntityXml = CheckEntity(reportingEntity, corrMessageRefId, xmlDoc);
                            if (!string.IsNullOrEmpty(reportingEntityXml.ToString()))
                                sbResults.Append(reportingEntityXml.ToString());
                        }
                    }

                    //if there is 1 or more cbcReports
                    if (reports != null)
                    {
                        var reportsXml = CheckContainerForDeletion(reports, corrMessageRefId, xmlDoc);
                        if (!string.IsNullOrEmpty(reportsXml.ToString()))
                            sbResults.Append(reportsXml.ToString());
                    }

                    //if there is 1 or more additionalInfo
                    if (additionalInfo != null)
                    {
                        var additionalInfoXml = CheckContainerForDeletion(additionalInfo, corrMessageRefId, xmlDoc);
                        if (!string.IsNullOrEmpty(additionalInfoXml.ToString()))
                            sbResults.Append(additionalInfoXml.ToString());
                    }
                }
            }

        }

        return sbResults.ToString();

    }

    private static string GetMessageRefId(XmlNode messageSpec)
    {
        if (messageSpec != null)
        {
            var docSpec = messageSpec.SelectSingleNode("MessageRefId");
            return docSpec.InnerText;
        }
        return "";
    }

    private static StringBuilder CheckEntity(XmlNode entity, string corrMessageRefId, XmlDocument xmlDoc)
    {
        var docSpec = entity.SelectSingleNode("DocSpec");
        StringBuilder sbResults = new StringBuilder();
        if (docSpec != null)
        {
            var docTypeind = docSpec.SelectSingleNode("DocTypeIndic");
            var docRefId = docSpec.SelectSingleNode("DocRefId");
            if (docTypeind != null && docRefId != null)
            {
                // deletion
                if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString())
                {
                    var newDocRefId = docRefId.InnerText;
                    string[] dRefId = newDocRefId.Split(',');
                    if (dRefId.Length > 1)
                    {
                        var CorrDocRefID = dRefId[0];
                        string[] docRefID = docRefId.InnerText.Split('-');
                        docRefId.InnerText = docRefID[0] + "-" + Utils.GenerateUniqueNo();
                        docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                        //corrDocRefId
                        XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                        childElement.InnerText = dRefId[1];
                        XmlNode newChild = (XmlNode)childElement;
                        docSpec.AppendChild(newChild);

                        //corrMessageRefId
                        XmlElement messageRefId = xmlDoc.CreateElement("CorrMessageRefID");
                        messageRefId.InnerText = corrMessageRefId;
                        XmlNode newChildNode = (XmlNode)messageRefId;
                        docSpec.AppendChild(newChildNode);
                        sbResults.Append(entity.OuterXml);
                    }
                }

            }

        }
        return sbResults;
    }
    private static StringBuilder CheckContainerForDeletion(XmlNodeList nodes, string corrMessageRefId, XmlDocument xmlDoc)
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
                    // deletion
                    if (docTypeind.InnerText == FDREnums.LifeSubmittingType.OECD3.ToString())
                    {
                        var newDocRefId = docRefId.InnerText;
                        string[] dRefId = newDocRefId.Split(',');
                        if (dRefId.Length > 1)
                        {
                            var CorrDocRefID = dRefId[0];
                            string[] docRefID = docRefId.InnerText.Split('-');
                            docRefId.InnerText = docRefID[0] + "-" + Utils.GenerateUniqueNo();
                            docTypeind.InnerText = FDREnums.LifeSubmittingType.OECD3.ToString();
                            //corrDocRefId
                            XmlElement childElement = xmlDoc.CreateElement("CorrDocRefID");
                            childElement.InnerText = dRefId[1];
                            XmlNode newChild = (XmlNode)childElement;
                            docSpec.AppendChild(newChild);

                            //corrMessageRefId
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

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    protected void ddlCountryList_OptionChanged(Sars.Systems.Controls.CountryList.CountryListFieldEventArgs e)
    {
        LoadCBC();
    }

    protected void lnkViewHistory_Click(object sender, EventArgs e)
    {
        LinkButton lbViewHistory = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lbViewHistory.Parent.Parent;
        var taxRefNo = gvRow.Cells[2].Text;
        var year = gvRow.Cells[5].Text;
        Response.Redirect(
            string.Format(
               "~/pages/cbcForm.aspx?incLocal={0}&refno={1}&year={2}&bck={3}&mspecId={4}"
                , 1
                , taxRefNo
                , year
                , Request.Url.PathAndQuery.ToBase64String()
                , 0));
    }

    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var coutryCode = ddlCountryList.SelectedValue;
        ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(coutryCode), "ReportingPeriod", "ID");
        LoadCBC();
    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void ddlReportingPeriod_TextChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Button btnView = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnView.Parent.Parent;

        Response.Redirect(string.Format("corrections.aspx?period={0}&TaxRefNo={1}", gvRow.Cells[3].Text,
            gvRow.Cells[2].Text));
    }
}