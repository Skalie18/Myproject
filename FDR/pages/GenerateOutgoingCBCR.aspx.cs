using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(), "ReportingPeriod", "ID");
            Common.GetAllCountries(ddlCountryList);
            ddlCountryList.SelectedValue = string.IsNullOrEmpty(Request.QueryString["xCountry"]) ? "-99999" : Request.QueryString["xCountry"].ToString();
            ddlReportingPeriod.SelectedValue = string.IsNullOrEmpty(Request.QueryString["Period"]) ? "-99999" : Request.QueryString["Period"].ToString();
            ViewState["prevPage"] = Request.UrlReferrer;
            LoadCBC();
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string reportingPeriod = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DBReadManager.GetNewOutGoingCBCR(countryCode, reportingPeriod);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);

    }

    private void PopulateYears()
    {
        int FirstYear = 2012;
        int CurrentYear = DateTime.Now.Year;
        for (int i = CurrentYear; i >= FirstYear; i--)
        {
            ddlYears.Items.Add(i.ToString());
        }
        ddlYears.Items.Insert(0, "SELECT");
    }



    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FDRPage fp = new FDRPage();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ActionId = "";
            string ReturnStatus = "";
            Button btnGenerateSingle = (e.Row.FindControl("btnGenerateSingle") as Button);
            Button btnVerify = (e.Row.FindControl("btnVerifyPackage") as Button);
            CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
            Label lblActionId = e.Row.FindControl("lblActionId") as Label;
            Label lblReturnStatus = e.Row.FindControl("lblReturningStatus") as Label;
            if (lblActionId != null) { ActionId = lblActionId.Text; }
            if (lblReturnStatus != null) { ReturnStatus = lblReturnStatus.Text; }
            if (fp.IsUserInRole("Reviewer") || fp.IsUserInRole("Approver"))
            {
                var strStatus = Server.HtmlDecode(e.Row.Cells[3].Text);
                var reportingPeriod = Server.HtmlDecode(e.Row.Cells[1].Text);
                btnVerify.Enabled = true;
                btnGenerateSingle.Enabled = true;
                if (!string.IsNullOrWhiteSpace(strStatus) && strStatus.Trim() != 
                    "Rejected" && strStatus.Trim() != "Generated" )
                {
                    btnGenerateSingle.Enabled = false;
                }

                if ((ActionId == "3" && ReturnStatus.ToLower() == "accepted")
                    || ReturnStatus.ToLower() == "rejected")
                {
                    btnGenerateSingle.Enabled = true;
                }

            }
            else { btnGenerate.Enabled = false; btnGenerateSingle.Enabled = false; }
        }
    }

    private bool PeriodIsSelected()
    {
        if (string.IsNullOrEmpty(ddlReportingPeriod.SelectedValue))
        {
            MessageBox.Show("Please select the reporting period");
            return false;
        }
        int rows = gvCBC.Rows.Count;
        if (ddlYears.SelectedIndex == 0)
        {
            for (int i = 0; i < rows; i++)
            {
                int PeriodYear = int.Parse(ddlReportingPeriod.SelectedValue.Substring(0, 4));
                CheckBox chkSelect = gvCBC.Rows[i].Cells[4].FindControl("chkSelect") as CheckBox;
                var countryCode = gvCBC.Rows[i].Cells[0].Text.Split('-')[1].Trim();
                var year = int.Parse(gvCBC.Rows[i].Cells[1].Text);
                if (chkSelect != null)
                {
                    if (chkSelect.Checked)
                    {
                        if (PeriodYear != year)
                        {
                            MessageBox.Show("Reporting Period Year should be the same as Tax Year");
                            return false;
                        }
                    }
                }
            }
        }
        else
        {
            if (int.Parse(ddlReportingPeriod.SelectedValue.Substring(0, 4)) != int.Parse(ddlYears.SelectedValue))
            {
                MessageBox.Show("Reporting Period Year should be the same as Tax Year");
                return false;
            }
        }

        return true;
    }

    private bool AtleastOneChecked()
    {
        int rows = gvCBC.Rows.Count;
        for (int i = 0; i < rows; i++)
        {
            CheckBox chkSelect = gvCBC.Rows[i].Cells[4].FindControl("chkSelect") as CheckBox;
            if (chkSelect != null)
            {
                if (chkSelect.Checked)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        Label lblStatusId = gvRow.FindControl("lblStatusId") as Label;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = string.IsNullOrWhiteSpace(WebUtility.HtmlDecode(gvRow.Cells[1].Text)) ? ddlReportingPeriod.SelectedValue : gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&idx=0&Period=" + period + "&StatusId=" + lblStatusId.Text);
    }

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkHeader = (CheckBox)sender;

        for (int i = 0; i < gvCBC.Rows.Count; i++)
        {
            CheckBox chkSlectCountry = gvCBC.Rows[i].FindControl("chkSelect") as CheckBox;

            if (chkSlectCountry != null)
            {
                if (chkSlectCountry.Enabled)
                    chkSlectCountry.Checked = chkHeader.Checked;
            }
        }
    }



    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        int i = 0;
        CheckBox chkHeader = gvCBC.HeaderRow.FindControl("ChkHeader") as CheckBox;

        for (i = 0; i < gvCBC.Rows.Count; i++)
        {
            CheckBox chkSelect = gvCBC.Rows[i].FindControl("chkSelect") as CheckBox;

            if (chkSelect != null && !chkSelect.Checked)
            {
                if (chkHeader != null)
                {
                    chkHeader.Checked = false;
                }
                break;
            }
        }

        if (gvCBC.Rows.Count == i && chkHeader != null)
        {
            chkHeader.Checked = true;
        }
    }

    protected void btnGenerateSingle_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        try
        {
            Button btnGenerate = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
            Guid newUID = new Guid();
            var xmldoc = new XmlDocument();
            XmlNode messageSpec = xmldoc.CreateNode(XmlNodeType.Element, "MessageSpec", xmldoc.NamespaceURI);
            var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
            var country = gvRow.Cells[0].Text.Split('-')[0].Trim();
            var period = ddlReportingPeriod.SelectedIndex==0? gvRow.Cells[1].Text: ddlReportingPeriod.SelectedValue;
            var year = int.Parse(gvRow.Cells[2].Text);
            if (ValidateRequiredFields(period))
            {
                decimal id = 0;
                var outgoinCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, period);
                if (outgoinCBC != null)
                {
                    id = outgoinCBC.Id;
                    var newPackage = Common.GenerateNewPackage(countryCode, period, ref messageSpec, id);
                    var newMessageSpec = messageSpec;
                    var nmPackage = newPackage;
                    var newPackagedCBC = Utils.GetOutgoingCBCR(newPackage, countryCode, year,
                            DateTime.Parse(period), Sars.Systems.Security.ADUser.CurrentSID,
                            nmPackage, id);
                    var saved = DatabaseWriter.SaveOutgoingCBC(newPackagedCBC, ref newUID);
                    if (saved > 0)
                        DBWriteManager.Insert_OutgoingPackageAuditTrail(newUID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} generated", country));
                    else
                        DBWriteManager.Insert_OutgoingPackageAuditTrail(outgoinCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} generated", country));
                    LoadCBC();
                    MessageBox.Show(string.Format("Package for {0} was successfully generated", country));
                    return;
                }

                var package = Common.GenerateNewPackage(countryCode, period, ref messageSpec, id);
                if (!string.IsNullOrEmpty(package.ToString()))
                {
                    var outgoingCBC = Utils.GetOutgoingCBCR(package.ToString(), countryCode, year,
                            DateTime.Parse(period), Sars.Systems.Security.ADUser.CurrentSID,
                            null, id);
                    id = DatabaseWriter.SaveOutgoingCBC(outgoingCBC, ref newUID);
                    if (id > 0)
                    {
                        var newPackage = Common.GenerateNewPackage(countryCode, period, ref messageSpec, id);
                        var newMessageSpec = messageSpec;
                        var nmPackage = newPackage;
                        var newPackagedCBC = Utils.GetOutgoingCBCR(newPackage, countryCode, year,
                            DateTime.Parse(period), Sars.Systems.Security.ADUser.CurrentSID,
                            nmPackage, id);
                        var saved = DatabaseWriter.SaveOutgoingCBC(newPackagedCBC, ref newUID);
                    }

                    DBWriteManager.Insert_OutgoingPackageAuditTrail(newUID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} generated", country));

                    LoadCBC();
                    MessageBox.Show(string.Format("Outgoing Package for {0} was successfully generated", country));
                }
                else
                {
                    MessageBox.Show("No package was generated");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

   

    private bool ValidateRequiredFields(string period)
    {
        if (string.IsNullOrEmpty(period) && ddlReportingPeriod.SelectedIndex==0)
        {
            MessageBox.Show("Please select the reporting period");
            return false;
        }

       

        return true;
    }
    protected void ddlCountryList_OptionChanged(Sars.Systems.Controls.CountryList.CountryListFieldEventArgs e)
    {
        LoadCBC();
    }

    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var coutryCode = ddlCountryList.SelectedValue;
        ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(coutryCode), "ReportingPeriod", "ID");
        LoadCBC();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (ViewState["prevPage"] != null)
            Response.Redirect(ViewState["prevPage"].ToString());
    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void ddlReportingPeriod_TextChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }
}