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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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
        string period = ddlReportingPeriod.SelectedIndex > 0 ? ddlReportingPeriod.SelectedValue : null;
        var cbcr = DBReadManager.GetOutGoingCBCR(countryCode, period, 4);
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

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCommentsById(decimal packageId)
    {
        var result = DatabaseReader.GetCommentsForCountryByYear(packageId, 2);
        string jsonString = "";
        if (result.HasRows)
        {
            var lst = result.Tables[0].AsEnumerable()
                .Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                ).ToDictionary(z => z.Key, z => z.Value)
                ).ToList();
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
             jsonString = serializer.Serialize(lst);
        };
        return jsonString;
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
}