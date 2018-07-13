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
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    FDRPage fp = new FDRPage();
    protected void Page_Load(object sender, EventArgs e)
    {
        var updatePanelControlIdThatCausedPostBack = String.Empty;
        var scriptManager = System.Web.UI.ScriptManager.GetCurrent(Page);
        Sars.Models.CBC.OutGoingCBCDeclarations outCBC = null;
        var countryCode = "";
        var reportingPeriod = "";
        int statusId = string.IsNullOrEmpty(Request.QueryString["StatusId"]) ? 0 : int.Parse(Request.QueryString["StatusId"].ToString());
        if (Request.QueryString["xCountry"] != null & Request.QueryString["Period"] != null && (fp.IsUserInRole("Reviewer") || fp.IsUserInRole("Approver")))
        {
            countryCode = Request.QueryString["xCountry"].ToString();
            reportingPeriod = Request.QueryString["Period"].ToString();
            outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
        }

        if (!IsPostBack)
        {
            ViewState["prevPage"] = Request.UrlReferrer;
            lblHeader.Text = "VIEW";
            countryCode = Request.QueryString["xCountry"].ToString();
            reportingPeriod = Request.QueryString["Period"].ToString();
            btnReject.Visible = false;
            btnApprove.Visible = false;
            //btnSendToReviewer.Visible = false; 
            if (outCBC != null)
            {
                if (fp.IsUserInRole("Reviewer"))
                {
                    btnApprove.Text = "Verify";
                    switch (outCBC.StatusId)
                    {
                        case 2:
                            lblHeader.Text = "VERIFY";
                            break;
                        case 7:
                            lblHeader.Text = "VERIFY CORRECTED";
                            break;
                        case 8:
                            lblHeader.Text = "VERIFY VOIDED";
                            break;
                        default:
                            lblHeader.Text = "VIEW";
                            break;
                    }
                }
                else if (fp.IsUserInRole("Approver"))
                {
                    btnApprove.Text = "Approve";
                    lblHeader.Text = (outCBC.StatusId == 3) ? "APPROVE" : "VIEW";
                }
            }
            else
            {
                btnDownloadXcel.Visible = false;
                btnDownload.Visible = false;
            }

            LoadCBC(countryCode, reportingPeriod,statusId);
            LoadCBCHsitory(countryCode, reportingPeriod);

        }
        else
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
                        outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
                        if (fp.IsUserInRole("Approver"))
                        {
                            if (outCBC.StatusId == 3)
                            {
                                btnApprove.Visible = true;
                                btnReject.Visible = true;
                                if (SameApprover(outCBC))
                                {
                                    btnReject.Visible = false;
                                    btnApprove.Visible = false;
                                }
                            }
                        }
                        if (fp.IsUserInRole("Reviewer"))
                        {
                            if (outCBC.StatusId == 2 || outCBC.StatusId == 7 || outCBC.StatusId == 8)
                            {
                                btnApprove.Visible = true;
                            }
                        }
                    }
                }
            }

        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCorrectedCBCR(string countryCode, string reportingPeriod)
    {
        var cbcr = DBReadManager.GetCorrectedCBCByCountryAndReportingPeriod(countryCode,reportingPeriod);
        string jsonString = "";
        if (cbcr.HasRows)
        {
            var lst = cbcr.Tables[0].AsEnumerable()
                .Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                ).ToDictionary(z => z.Key, z => z.Value)
                ).ToList();
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsonString = serializer.Serialize(lst);
        };
        return jsonString;
    }

    protected void lnkViewForm_Click(object sender, EventArgs e)
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

                    var year = gvCBC.SelectedDataKey[1].ToString();
                    var taxRefNo = gvCBC.SelectedDataKey[0].ToString();
                    Response.Redirect(
                        string.Format(
                           "~/pages/cbcForm.aspx?incLocal={0}&refno={1}&year={2}&bck={3}&mspecId={4}"
                            , 1
                            , taxRefNo
                            , year
                            , Request.Url.PathAndQuery.ToBase64String()
                            , 0));
                }
            }
        }
    }


    private void LoadCBC(string CountryCode, string reportingPeriod, int statusId=0)
    {
        var cbcr = DBReadManager.GetOutgoingCBCReportPackaged(CountryCode, reportingPeriod, statusId);
        Session["subdata"] = cbcr;
        gvCBC.Bind(cbcr);
    }


    private void LoadCBCHsitory(string countryCode, string reportingPeriod)
    {
        var cbcr = DBReadManager.GetOutgoingPackageHistory(countryCode, reportingPeriod);
        Session["subhistorydata"] = cbcr;
        gvCBCHistory.Bind(cbcr);
    }


    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkEnableCountry = (e.Row.FindControl("chkEnableCountry") as CheckBox);
            DropDownList ddlStatus = (e.Row.FindControl("ddlStatus") as DropDownList);
            Button btnSave = (e.Row.FindControl("btnSave") as Button);
            Button btnGenerate = (e.Row.FindControl("btnGenerate") as Button);
            Button btnVerify = (e.Row.FindControl("btnVerify") as Button);
            if (chkEnableCountry != null)
            {
                bool ItemChecked = Convert.ToBoolean((e.Row.FindControl("lblEnableCountry") as Label).Text);
                chkEnableCountry.Checked = ItemChecked;
            }

            if (ddlStatus != null)
            {
                ddlStatus.Bind(DBReadManager.ValidationStatuses(), "StatusDescription", "id");
                string statusId = (e.Row.FindControl("lblStatus") as Label).Text;
                int status = int.Parse(statusId);

                if (status > 0)
                {
                    ddlStatus.Items.FindByValue(statusId).Selected = true;
                    ddlStatus.Enabled = false;
                    btnSave.Enabled = false;
                    chkEnableCountry.Enabled = false;
                    btnVerify.Enabled = false;
                }
                else
                {
                    ddlStatus.Visible = false;
                }
                if (status >= 2)
                {
                    btnGenerate.Enabled = false;
                    btnSave.Enabled = false;
                    btnVerify.Enabled = true;
                }
            }

        }
    }


    protected void btnApprove_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        var countryCode = Request.QueryString["xCountry"].ToString();
        var period = Request.QueryString["Period"].ToString();
        if (string.IsNullOrEmpty(countryCode) && string.IsNullOrEmpty(period))
        {
            return;
        }
        if (fp.IsUserInRole("Reviewer") || fp.IsUserInRole("Approver"))
        {
            try
            {
                string reportingPeriod = period;
                var outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
                int statusId = fp.IsUserInRole("Reviewer") ? 3 : 5;

                string Subject = "";
                var email = string.IsNullOrEmpty(ADUser.CurrentUser.Mail) ? "fdr@sars.gov.za" : ADUser.CurrentUser.Mail;
               // DBWriteManager.ApproveOutgoingPackage(outCBC.Id, countryCode, reportingPeriod, statusId, ADUser.CurrentSID);
                string[] senderEmail = { email };
                if (fp.IsUserInRole("Reviewer"))
                {
                    try
                    {
                        DBWriteManager.Insert_OutgoingPackageAuditTrail(outCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} verified", outCBC.CountryName));
                        Subject = string.Format("Outgoing Package for {0} has been verified ", outCBC.CountryName);
                        Common.SendEmailToRole("Approver", outCBC.CountryName, Subject, FDRPage.Statuses.Verified, senderEmail);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        DBWriteManager.Insert_OutgoingPackageAuditTrail(outCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} approved", outCBC.CountryName));
                        Subject = string.Format("Outgoing Package for {0} has been approved", outCBC.CountryName);
                        Common.SendEmailToRole("Reviewer", outCBC.CountryName, Subject, FDRPage.Statuses.Approved, senderEmail);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                LoadCBCHsitory(countryCode, reportingPeriod);
                MessageBox.Show(Subject + " successfully");
                if (fp.IsUserInRole("Reviewer"))
                {
                    if (statusId > 2)
                    {
                        btnApprove.Enabled = false;
                    }
                }
                else
                {
                    if (statusId > 3)
                    {
                        btnApprove.Enabled = false;
                        btnReject.Enabled = true;
                    }
                }
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

    private void SavePackage(StringBuilder package, decimal id, string countryCode, int year)
    {
        Guid newUid = new Guid();
        var outgoingCBC = new OutGoingCBCDeclarations()
        {
            Id = id,
            Country = countryCode,
            NSCBCData = package.ToString(),
            StatusId = 3,
            Year = year,
            CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
        };
        decimal saved = DatabaseWriter.SaveOutgoingCBC(outgoingCBC, ref newUid);
    }


    private bool SameApprover(Sars.Models.CBC.OutGoingCBCDeclarations outCBC)
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
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        var countryCode = Request.QueryString["xCountry"].ToString();
        int statusId = string.IsNullOrEmpty(Request.QueryString["StatusId"]) ? 0 : int.Parse(Request.QueryString["StatusId"].ToString());
        if (fp.IsUserInRole("Reviewer") && statusId == 2)
        {
            btnApprove.Visible = true;
        }

        if (fp.IsUserInRole("Approver") && statusId == 3)
        {
            btnApprove.Visible = true;
            btnReject.Visible = true;
        }
        foreach (GridViewRow gvRow in gvCBC.Rows)
        {
            Label lblDestination = gvRow.FindControl("lblDestinationCountry") as Label;
            string period = gvRow.Cells[4].Text;
            string FileName = lblDestination.Text;
            Session["CanApprove"] = "CanApprove";
            string fullURL = "window.open('download.aspx?&xCountry=" + countryCode + "&fname=" + FileName + ",true,xml &period= " + period + "', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (fp.IsUserInRole("Approver"))
        {

            var countryCode = Request.QueryString["xCountry"].ToString();
            var email = ADUser.CurrentUser.Mail;
            string[] senderEmail = { email };
            foreach (GridViewRow gvRow in gvCBC.Rows)
            {

               string reportingPeriod = gvRow.Cells[4].Text;
                var outCBC = Common.GetDeclarations(countryCode, reportingPeriod);
                ModalPopupExtender1.Show();
            }

        }
    }

    protected void btnDownloadXcel_Click(object sender, EventArgs e)
    {

        if (Request.QueryString["xCountry"] != null & Request.QueryString["Period"] != null)
        {
           var countryCode = Request.QueryString["xCountry"].ToString();
           var reportingPeriod = Request.QueryString["Period"].ToString();
           var   outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, reportingPeriod);
            int statusId = string.IsNullOrEmpty(Request.QueryString["StatusId"]) ? 0 : int.Parse(Request.QueryString["StatusId"].ToString());
            if (fp.IsUserInRole("Reviewer") && statusId == 2)
            {
                btnApprove.Visible = true;
            }

            if (fp.IsUserInRole("Approver") && statusId == 3)
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }
            if (outCBC.UID != null)
            {
                string fullURL = "window.open('../queueMonitors/MANUAL/CbCReport.aspx?UID=" + outCBC.UID + "&src=OUT ', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
                //string fullURL = "window.open('CbCReport.aspx?UID=" + outCBC.UID + "&src=OUT ', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                // Response.Redirect(string.Format("~/queueMonitors/MANUAL/CbCReport.aspx?UID={0}&src=OUT", outCBC.UID));
            }
        }

     
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (ViewState["prevPage"] != null)
        {
            var prevPage = ViewState["prevPage"].ToString();
            string[] tmp = prevPage.Split('.');
            /* Response.Redirect(string.Format("{0}?xCountry={1}&Period={2}", prevPage,
                 Request.QueryString["xCountry"].ToString(), Request.QueryString["Period"]));
             return;*/
            Response.Redirect(ViewState["prevPage"].ToString());
        }

        Response.Redirect(ViewState["prevPage"].ToString());
    }



    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void gvCBCHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBCHistory.NextPage(Session["subhistorydata"], e.NewPageIndex);
    }

    protected void btnSendToReviewer_Click(object sender, EventArgs e)
    {

    }
}