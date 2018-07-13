using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using Sars.Systems.Security;
public partial class pages_ApproveOutgoinCBC : System.Web.UI.Page
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
        var cbcr = DBReadManager.GetVerifiedOutGoingCBCs(countryCode, reportingPeriod);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }


    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }


    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        Label lblStatusId = gvRow.FindControl("lblStatusId") as Label;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[1].Text;
        Response.Redirect( string.Format("VerifyOutgoingCBC.aspx?idx={0}&xCountry={1}&Period={2}&StatusId={3}", "0", countryCode, period, lblStatusId.Text));
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