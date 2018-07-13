using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Security;

public partial class pages_comments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        if (Request.QueryString["period"]!= null && Request.QueryString["TaxRefNo"] != null)
        {
            var TaxRefNo = Request.QueryString["TaxRefNo"].ToString().Trim();
            var reportingPeriod = Request.QueryString["period"].ToString();
            LoadCBC(reportingPeriod, TaxRefNo);
        }
    }

    private void LoadCBC(string reportingPeriod, string TaxRefNo)
    {
        var cbcr = DBReadManager.GetPackagesToBeCorrected(reportingPeriod, TaxRefNo);
        gvCBC.Bind(cbcr);
        Session["cbcr"] = cbcr;
        //btnCorrect.Visible = cbcr.HasRows;
       
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }

    protected void lnkViewForm_Click(object sender, EventArgs e)
    {

    }

    protected void btnCorrect_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ccode"] != null && Request.QueryString["period"] != null)
        {
            FDRPage fPage = new FDRPage();
            Guid newUid = new Guid();
            var country = Request.QueryString["ccode"].ToString();
            var reportingPeriod = Request.QueryString["period"].ToString();
            var outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(country, reportingPeriod);
            decimal id = string.IsNullOrEmpty(outCBC.Id.ToString()) ? 0 : outCBC.Id;
            var package = Common.GenerateCorrectionPackage(country, reportingPeriod);
            var email = ADUser.CurrentUser.Mail;
            string[] senderEmail = { email };
            if (!string.IsNullOrEmpty(package.ToString()))
            {
                var outgoingCBC = new OutGoingCBCDeclarations()
                {
                    Id = id,
                    Country = country,
                    CBCData = package.ToString(),
                    StatusId = 7,
                     Year = int.Parse(reportingPeriod.Substring(0,4)),
                    CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
                };
                decimal saved = DatabaseWriter.SaveOutgoingCBC(outgoingCBC, ref newUid);
                DBWriteManager.ApproveOutgoingPackage(id, country, reportingPeriod, 7, ADUser.CurrentSID);
                DBWriteManager.Insert_OutgoingPackageAuditTrail(outCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} corrected", gvCBC.Rows[0].Cells[0].Text.Split('-')[0].Trim()));

                var Subject = "Outgoing CBC Report has been corrected ";
                Common.SendEmailToRole("Reviewer", outCBC.CountryName, Subject, FDRPage.Statuses.Corrected, senderEmail);
                LoadCBC(country, reportingPeriod);
                MessageBox.Show("Package has been corrected successfully");
            }
            else
            {
                MessageBox.Show("No package was corrected");
            }
        }
    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void btnCorrectSingle_Click(object sender, EventArgs e)
    {
        Button btnGenerate = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btnGenerate.Parent.Parent;
        var period = gvRow.Cells[1].Text;
        var countryCode = gvRow.Cells[0].Text.Split('-')[1].Trim();
        var taxrefNo = Request.QueryString["TaxRefNo"].ToString().Trim();
        var newCorrectedPackage = Common.GetCorrectionsXml(countryCode, period, taxrefNo);
        var country = gvRow.Cells[0].Text.Split('-')[0].Trim();
        var email = string.IsNullOrEmpty(ADUser.CurrentUser.Mail) ? "fdr@sars.gov.za" : ADUser.CurrentUser.Mail;
        string[] senderEmail = { email };
        if (newCorrectedPackage)
        {
            MessageBox.Show("The package was corrected successfully");
            string Subject = string.Format("Outgoing Package for {0} has been verified ", country);
            Common.SendEmailToRole("Reviewer", country, Subject, FDRPage.Statuses.Corrected, senderEmail);

        }
        else
        {
            MessageBox.Show("Failed to execute correction");
        }
    }

    
}