using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["prevPage"] = Request.UrlReferrer;
            Common.GetAllCountries(ddlCountryList);
            ddlReportingPeriod.Bind(DBReadManager.GetAllReportingPeriods(), "ReportingPeriod", "ID");

            LoadCBC();
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string reportingPeriod = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DBReadManager.GetOutGoingCBCR(countryCode,reportingPeriod,8);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }

   
    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Button btnSave = (e.Row.FindControl("btnSave") as Button);
            Button btnGenerate = (e.Row.FindControl("btnGenerate") as Button);
            Button btnVerify = (e.Row.FindControl("btnVerifyPackage") as Button);
            HiddenField hdnID = e.Row.FindControl("hdnId") as HiddenField;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
     

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
        // int idx = row.RowIndex;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
      
    }


    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        HiddenField hdnID = gvRow.FindControl("hdnId") as HiddenField;
        DropDownList ddlStatus = gvRow.FindControl("ddlStatus") as DropDownList;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&Period=" + period);
    }

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
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
}