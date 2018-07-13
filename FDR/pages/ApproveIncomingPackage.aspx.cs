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
            Common.GetAllForeignCountries(ddlCountryList);
            ddlReportingPeriod.Bind(DBReadManager.GetAllIncomingReportingPeriods(), "ReportingPeriod", "ID");
            LoadCBC();
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string period = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DatabaseReader.ForeignIncomingPackage(countryCode, period,3);
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

 
    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        Label lblStatus = (Label)gvRow.FindControl("lblStatus");
        var countryCode = gvRow.Cells[1].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[2].Text;
        var param = lblStatus.Text.Split('|');
        var statusId = param[0];
        var id = param[1];
        Response.Redirect("VerifyIncomingCBC.aspx?idx=" + id + "&xCountry=" + countryCode + "&statusIdx=" + statusId + "&incLocal=0");
    }

    protected void ddlReportingPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

   
    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }
}