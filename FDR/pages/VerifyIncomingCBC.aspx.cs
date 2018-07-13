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
        var updatePanelControlIdThatCausedPostBack = String.Empty;

        var scriptManager = System.Web.UI.ScriptManager.GetCurrent(Page);
        string countryCode = "";
        string reportingPeriod = "";
        int? statusId = 0;
        decimal id = 0;
        if (Request.QueryString["idx"] != null && Request.QueryString["statusIdx"] != null
            && Request.QueryString["incLocal"] != null)
        {
            lblHeader.Text = "VIEW";
            id = decimal.Parse(Request.QueryString["idx"].ToString());
            statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
            var mspec = DBReadManager.GetMessageSpecById(id);
            if (mspec != null)
            {
                if (mspec.StatusID > 0)
                    statusId = mspec.StatusID;
            }

        }

        if (Request.QueryString["xCountry"] != null && Request.QueryString["Period"] != null)
        {
            countryCode = Request.QueryString["xCountry"].ToString();
            reportingPeriod = Request.QueryString["Period"].ToString();
        }
        if (!IsPostBack)
        {
            ViewState["prevPage"] = Request.UrlReferrer;
            lblHeader.Text = "VIEW";
            btnApprove.Visible = false;
            btnReject.Visible = false;
            btnDownload.Visible = false;
            btnDownloadXcel.Visible = false;
            if (fp.IsUserInRole("Reviewer"))
            {
                btnApprove.Text = "Review";
                lblHeader.Text = (statusId == 1) ? "REVIEW" : "VIEW";
                btnDownload.Visible = true;
                btnDownloadXcel.Visible = true;
            }
            if (fp.IsUserInRole("Approver"))
            {
                btnApprove.Text = "Approve";
                lblHeader.Text = (statusId == 3) ? "APPROVE" : "VIEW";
                btnDownload.Visible = true;
                btnDownloadXcel.Visible = true;
            }


            LoadCBC(id);
            LoadCBCHsitory(countryCode, reportingPeriod);

        }
        /*else
        {
            if (scriptManager != null)
            {
                var smUniqueId = scriptManager.UniqueID;
                var smFieldValue = Request.Form[smUniqueId];

                if (!String.IsNullOrEmpty(smFieldValue) && smFieldValue.Contains("|"))
                {
                    updatePanelControlIdThatCausedPostBack = smFieldValue.Split('|')[1];
                    if (updatePanelControlIdThatCausedPostBack == "ctl00$MainContent$btnDownloadXcel"
                        || updatePanelControlIdThatCausedPostBack == "ctl00$MainContent$btnDownload")
                    {
                        if (fp.IsUserInRole("Approver") || fp.IsUserInRole("Reviewer"))
                        {
                            btnApprove.Text = "Approve";
                            lblHeader.Text = (statusId == 1) ? "APPROVE" : "VIEW";
                            if (statusId == 1)
                            {
                                btnApprove.Visible = true;
                                btnReject.Visible = true;
                            }
                        }
                    }
                }
            }

        }*/

    }

    protected void lnkViewForm_Click(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn != null)
        {
            if (Request.QueryString["incLocal"] == null)
            {
                return;
            }
            var row = btn.NamingContainer as GridViewRow;
            if (row != null)
            {
                gvCBC.SelectedIndex = row.RowIndex;
                if (gvCBC.SelectedDataKey != null)
                {
                    Label lblCBCBodyId = row.FindControl("lblCBCBodyId") as Label;
                    if (lblCBCBodyId != null)
                    {
                        var year = gvCBC.SelectedDataKey[1].ToString();
                        var taxRefNo = gvCBC.SelectedDataKey[0].ToString();
                        Response.Redirect(
                            string.Format(
                                "~/pages/cbcForm.aspx?incLocal={0}&refno={1}&year={2}&bck={3}&mspecId={4}"
                                , 0
                                , taxRefNo
                                , year
                                , Request.Url.PathAndQuery.ToBase64String()
                                , lblCBCBodyId.Text));
                    }
                }
            }
        }
    }

    private void LoadCBC(decimal id)
    {
        var cbcr = DBReadManager.GetIncomingPackagePerCBC(id);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }


    private void LoadCBCHsitory(string countryCode, string reportingPeriod)
    {
        var cbcr = DBReadManager.GetOutgoingPackageHistory(countryCode, reportingPeriod);
        gvCBCHistory.Bind(cbcr);
    }


    protected void btnApprove_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        decimal id = decimal.Parse(Request.QueryString["idx"].ToString());
        int statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
        string incominLocal = Request.QueryString["incLocal"].ToString();
        var country = gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim() + "_" + gvCBC.Rows[0].Cells[5].Text;

        if (fp.IsUserInRole("Reviewer") || fp.IsUserInRole("Approver"))
        {
            try
            {
                int status = fp.IsUserInRole("Reviewer") ? 3 : 5;
                bool blnIsReviewer = fp.IsUserInRole("Reviewer") ? true : false;
                var email = string.IsNullOrEmpty(ADUser.CurrentUser.Mail) ? "fdr@sars.gov.za" : ADUser.CurrentUser.Mail;
                string[] senderEmail = { email };
                string Subject = "";
                DBWriteManager.ApproveForeignPackage(status, id, ADUser.CurrentSID);
                var mspec = DBReadManager.GetMessageSpecById(id);
                string message = string.Format("Incoming Package for {0} has been approved successfully", country);
                if (blnIsReviewer)
                {
                    DBWriteManager.Insert_OutgoingPackageAuditTrail(mspec.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Incoming Package for {0} from {1} verified", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim()));
                    Subject = string.Format("Incoming Package for {0} from {1} has been verified ", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(),
                         gvCBC.Rows[0].Cells[3].Text.Split('-')[0].Trim());
                    Common.SendEmailToRole("Approver", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(), Subject, FDRPage.Statuses.Verified, senderEmail);
                }
                else
                {
                    DBWriteManager.Insert_OutgoingPackageAuditTrail(mspec.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Incoming Package for {0} from {1} approved",
                        gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(), gvCBC.Rows[0].Cells[3].Text.Split('-')[0].Trim()));
                    Subject = string.Format("Incoming Package for {0}  from {1} has been approved", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(),
                         gvCBC.Rows[0].Cells[3].Text.Split('-')[0].Trim());
                    Common.SendEmailToRole("Reviewer", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(), Subject, FDRPage.Statuses.Approved, senderEmail);
                }
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
                MessageBox.Show(message);
                //LoadCBCHsitory(countryCode, reportingPeriod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

    private bool SameApprover(OutGoingCBCDeclarations outCBC)
    {
        if (fp.IsUserInRole("Approver"))
        {
            if (outCBC.UpdatedBy == ADUser.CurrentSID)
            {
                return true;
            }
            return false;
        }
        return false;
    }


    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (fp.IsUserInRole("Reviewer") || fp.IsUserInRole("Approver"))
        {
            Session["incoming"] = true;
            var email = ADUser.CurrentUser.Mail;
            string[] senderEmail = { email };
            foreach (GridViewRow gvRow in gvCBC.Rows)
            {

                int year = int.Parse(gvRow.Cells[5].Text);
                //var outCBC = Common.GetDeclarations(countryCode, year);
                ModalPopupExtender1.Show();
            }
        }
    }

    protected void btnDownloadXcel_Click(object sender, EventArgs e)
    {
        //System.Threading.Thread.Sleep(1000);

        if (Request.QueryString["idx"] == null)
            return;
        var mspecId = Request.QueryString["idx"].ToString();
        var xmlResults = DBReadManager.GetMessageSpecById(decimal.Parse(mspecId));
        if (xmlResults.UID != null)
        {
            string fullURL = "window.open('../queueMonitors/MANUAL/CbCReport.aspx?UID=" + xmlResults.UID + "&src=IN ', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            //string fullURL = "window.open('CbCReport.aspx?UID=" + xmlResults.UID + "&src=IN ', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
            //Response.Redirect(string.Format("~/queueMonitors/MANUAL/CbCReport.aspx?UID={0}&src=IN", xmlResults.UID));
        }


        if (fp.IsUserInRole("Reviewer"))
        {

            int? statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
            var id = decimal.Parse(Request.QueryString["idx"].ToString());
            var mspec = DBReadManager.GetMessageSpecById(id);
            if (mspec != null)
            {
                if (mspec.StatusID > 0)
                    statusId = mspec.StatusID;
            }
            if (statusId == 1)
                btnApprove.Visible = true;
        }

        if (fp.IsUserInRole("Approver"))
        {
            int? statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
            var id = decimal.Parse(Request.QueryString["idx"].ToString());
            var mspec = DBReadManager.GetMessageSpecById(id);
            if (mspec != null)
            {
                if (mspec.StatusID > 0)
                    statusId = mspec.StatusID;
            }
            if (statusId == 3)
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }
        }

        // Response.Redirect(string.Format("~/queueMonitors/MANUAL/CbCReport.aspx?UID={0}&src=IN", results.UID));

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

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        if (Request.QueryString["idx"] == null)
            return;
        var mspecId = Request.QueryString["idx"].ToString();
      
        var country = gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim() + "_" + gvCBC.Rows[0].Cells[5].Text;
        if (fp.IsUserInRole("Reviewer"))
        {

            int? statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
            var id = decimal.Parse(Request.QueryString["idx"].ToString());
            var mspec = DBReadManager.GetMessageSpecById(id);
            if (mspec != null)
            {
                if (mspec.StatusID > 0)
                    statusId = mspec.StatusID;
            }
            if (statusId == 1)
                btnApprove.Visible = true;
        }

        if (fp.IsUserInRole("Approver"))
        {
            int? statusId = int.Parse(Request.QueryString["statusIdx"].ToString());
            var id = decimal.Parse(Request.QueryString["idx"].ToString());
            var mspec = DBReadManager.GetMessageSpecById(id);
            if (mspec != null)
            {
                if (mspec.StatusID > 0)
                    statusId = mspec.StatusID;
            }
            if (statusId == 3)
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }
        }
        string fullURL = "window.open('downloadXml.aspx?&mspecId=" + mspecId + "&country=" + country + "', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
}