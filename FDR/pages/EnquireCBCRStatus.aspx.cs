using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.GetAllCountries(ddlCountryList);
            PopulateYears();
            PopulateCBCTypes();
            LoadCBC();
        }
    }

    private void LoadCBC()
    {
        var countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        var cbcr = DatabaseReader.GetOutgoingFileStatuses(countryCode);
        Session["records"] = cbcr;
        gvCBC.Bind(cbcr);
    }


    protected void lnkViewErrors_OnClick(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn != null)
        {
            var row = btn.NamingContainer as GridViewRow;
            if (row != null)
            {
                gvCBC.SelectedIndex = row.RowIndex;
                if (gvCBC.SelectedDataKey != null)
                {

                    var packageUID = btn.CommandArgument;
                    
                    Response.Redirect("OutgoingPackageStatus.aspx?uid=" + packageUID);
                }
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var country = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        var files = DatabaseReader.GetOutgoingFileStatuses(country);
        gvCBC.Bind(files);
    }


    /// <summary>
    /// Ingatius to Explain what is happening  from here onwards
    /// </summary>
    /// <param name="dateValue"></param>
    /// <returns></returns>
    private DateTime GetDate(string dateValue)
    {
        DateTime parsedDate;
        string pattern = @"yyyy-MM-dd";
        DateTime.TryParseExact(dateValue, pattern, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate);
        DateTime date = DateTime.Parse(dateValue, CultureInfo.CurrentCulture);

        string isoDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return parsedDate;
    }

    private void PopulateYears()
    {
        int FirstYear = 2012;
        int CurrentYear = DateTime.Now.Year;
        for (int i = CurrentYear; i >= FirstYear; i--)
        {
            //ddlYears.Items.Add(i.ToString());
        }
      //  ddlYears.Items.Insert(0, "SELECT");
    }

    private void PopulateCBCTypes()
    {

     ///   ddlCBCType.Items.Insert(0, new ListItem("SELECT", "0"));
     //   ddlCBCType.Items.Insert(1, new ListItem("Incoming Foreign CBC Report", "1"));
      ///  ddlCBCType.Items.Insert(2, new ListItem("Outgoing CBC Report", "2"));

      //  ddlCBCType.Items.RemoveAt(0);
        //ddlCBCType.SelectedIndex = 1;
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
    }

    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        HiddenField hdnID = gvRow.FindControl("hdnId") as HiddenField;
        DropDownList ddlStatus = gvRow.FindControl("ddlStatus") as DropDownList;
        var countryCode = gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&Period=" + gvRow.Cells[2].Text);
    }

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }

    protected void ddlCountryList_OptionChanged(Sars.Systems.Controls.CountryList.CountryListFieldEventArgs e)
    {
        LoadCBC();
    }

    protected void gvCBC_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["records"], e.NewPageIndex);
    }

    protected void ddlCBCType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }



}