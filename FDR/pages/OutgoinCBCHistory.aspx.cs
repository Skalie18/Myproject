using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
using System.Linq;
using Sars.Systems.Security;
using Sars.Systems.Extensions;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    FDRPage fp = new FDRPage();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCBC();
            PopulateYears();
        }
    }

    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        int year = ddlYears.SelectedIndex > 0 ? int.Parse(ddlYears.SelectedValue) : 0;
       // var cbcr = DatabaseReader.OutgoingCBCRDeclarationsHistory(countryCode, year);
      //  gvCBC.Bind(cbcr);
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

    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }


    protected void ddlCountryList_OptionChanged(Sars.Systems.Controls.CountryList.CountryListFieldEventArgs e)
    {
        LoadCBC();
    }
}