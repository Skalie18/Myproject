using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;

public partial class pages_IncomingPackage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCBC();
            Common.GetAllForeignCountries(ddlCountryList);
            ddlReportingPeriod.Bind(DBReadManager.GetAllIncomingReportingPeriods(), "ReportingPeriod", "ID");
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string period = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DatabaseReader.ForeignIncomingPackage(countryCode, period);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);

    }

 
    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }

   
    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var coutryCode = ddlCountryList.SelectedValue;
        ddlReportingPeriod.Bind(DBReadManager.GetAllIncomingReportingPeriods(coutryCode), "ReportingPeriod", "ID");
        LoadCBC();
    }
    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        Label lblStatus = (Label)gvRow.FindControl("lblStatus");
        var countryCode = gvRow.Cells[1].Text.Split('-')[1].Trim();
        var param = lblStatus.Text.Split('|');
        var statusId = param[0];
        var id = param[1];
 
        Response.Redirect("VerifyIncomingCBC.aspx?idx=" + id + "&xCountry=" + countryCode + "&statusIdx=" + statusId + "&incLocal=0");
    }


    protected void ddlReportingPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }
}