using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using Sars.Systems.Security;
using System.Xml;

public partial class pages_RegenerateCBCR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["prevPage"] = Request.UrlReferrer;
            Common.GetAllCountries(ddlCountryList);
            LoadCBC();
        }
    }


    private void LoadCBC()
    {
        string countryCode = ddlCountryList.SelectedIndex > 0 ? ddlCountryList.SelectedValue : null;
        string period = string.IsNullOrEmpty(txtReportingPeriod.Text) ? null : txtReportingPeriod.Text;
        var cbcr = DBReadManager.GetVerifiedOutGoingCBCs(countryCode, period);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }





    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        Guid newUID = new Guid();
        var xmldoc = new XmlDocument();
        XmlNode messageSpec = xmldoc.CreateNode(XmlNodeType.Element, "MessageSpec", null);
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var country = gvRow.Cells[0].Text.Split('-')[0].Trim();
        var year = int.Parse(gvRow.Cells[1].Text);
        if (ValidateRequiredFields(year))
        {
            decimal id = 0;
            var package = Common.GenerateNewPackage(countryCode, dpReportingPeriod.Text, ref messageSpec, id);
            if (!string.IsNullOrEmpty(package.ToString()))
            {
                var outgoingCBC = new OutGoingCBCDeclarations()
                {
                    Id = id,
                    Country = countryCode,
                    CBCData = package.ToString(),
                    NSCBCData = null,
                    StatusId = 2,
                    Year = year,
                    ReportingPeriod = DateTime.Parse(dpReportingPeriod.Text),
                    CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
                };

                id = DatabaseWriter.SaveOutgoingCBC(outgoingCBC, ref newUID);
                if (id > 0)
                {
                    var newPackage = Common.GenerateNewPackage(countryCode, dpReportingPeriod.Text, ref messageSpec, id);
                    var newMessageSpec = messageSpec;
                    var nmPackage = Common.GenerateNMPackage(countryCode, dpReportingPeriod.Text, newMessageSpec);
                    var newPackagedCBC = new OutGoingCBCDeclarations()
                    {
                        Id = id,
                        Country = countryCode,
                        CBCData = newPackage.ToString(),
                        NSCBCData = nmPackage,
                        StatusId = 2,
                        Year = year,
                        CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
                    };
                    var saved = DatabaseWriter.SaveOutgoingCBC(newPackagedCBC, ref newUID);
                }

                DBWriteManager.Insert_OutgoingPackageAuditTrail(newUID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Package for {0} generated", country));

                LoadCBC();
                MessageBox.Show(string.Format("Package for {0} was successfully generated", country));
            }
        }
    }

    private bool ValidateRequiredFields(int Year)
    {
        if (string.IsNullOrEmpty(dpReportingPeriod.Text))
        {
            MessageBox.Show("Please select the reporting period");
            return false;
        }

        if (int.Parse(dpReportingPeriod.Text.Substring(0, 4)) != Year)
        {
            MessageBox.Show("Reporting Period Year should be the same as Year");
            return false;
        }
        return true;
    }


    protected void btnVerifyPackage_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var period = gvRow.Cells[1].Text;
        Response.Redirect("VerifyOutgoingCBC.aspx?xCountry=" + countryCode + "&Period=" + period);
    }

    protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }


    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void txtReportingPeriod_TextChanged(object sender, EventArgs e)
    {
        LoadCBC();
    }
}