using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sars.Systems.Data;
using Sars.Systems.Security;
using FDR.DataLayer;
public partial class Controls_AddComments : System.Web.UI.UserControl, IUserControl
{
    public event EventHandler MyEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAddComments_Click(object sender, EventArgs e)
    {
        var hiddenValue = Parent.FindControl("hdnDone") as HiddenField;
        var mpeMe = Parent.FindControl("ModalPopupExtender1") as AjaxControlToolkit.ModalPopupExtender;
        hiddenValue.Value = txtComments.Text;
        var gvCBC = Parent.FindControl("gvCBC") as GridView;
        if (!string.IsNullOrEmpty(txtComments.Text))
        {
            var email = ADUser.CurrentUser.Mail;
            string[] senderEmail = { email };
            string Subject = "";
            if (Request.QueryString["idx"] == null)
            {
                return;
            }

            if (Request.QueryString["idx"] == "0")
            {
                var countryCode = Request.QueryString["xCountry"].ToString();
                
                if (gvCBC != null)
                {

                    string ReportingPeriod = gvCBC.Rows[0].Cells[4].Text;
                    var outCBC = DBReadManager.OutGoingCBCDeclarationsDetails(countryCode, ReportingPeriod);
                    var comments = new Comments()
                    {
                        OutGoingCBCDeclarationsID = outCBC.Id,
                        Notes = txtComments.Text,
                        AddedBy = ADUser.CurrentSID
                    };
                    if (SaveComments(comments, 2) > 0)
                    {
                        DBWriteManager.ApproveOutgoingPackage(outCBC.Id, countryCode, ReportingPeriod, 4, ADUser.CurrentSID);
                        Subject = "Outgoing CBC Package has been rejected";
                        FDR.DataLayer.DBWriteManager.Insert_OutgoingPackageAuditTrail(outCBC.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Outgoing Package for {0} has been rejected", outCBC.CountryName));
                        Common.SendEmailToRole("Reviewer", outCBC.CountryName, Subject, FDRPage.Statuses.Rejected, senderEmail);
                        MessageBox.Show(string.Format("Outgoing Package for {0} has been successfully rejected", outCBC.CountryName));
                    }

                }
            }
            else
            {
                if (Request.QueryString["idx"] != null)
                {
                    string ReportingPeriod = gvCBC.Rows[0].Cells[6].Text;
                    decimal msgSpecId = decimal.Parse(Request.QueryString["idx"].ToString());
                    var comments = new Comments()
                    {
                        OutGoingCBCDeclarationsID = msgSpecId,
                        Notes = txtComments.Text,
                        AddedBy = ADUser.CurrentSID
                    };
                    if (SaveComments(comments, 1) > 0)
                    {

                        DBWriteManager.ApproveForeignPackage(4, msgSpecId, ADUser.CurrentSID);
                        var mspec = DBReadManager.GetMessageSpecById(msgSpecId);
                        DBWriteManager.Insert_OutgoingPackageAuditTrail(mspec.UID, Sars.Systems.Security.ADUser.CurrentSID, string.Format("Incoming Package for {0} from {1} rejected",
                       gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(), gvCBC.Rows[0].Cells[3].Text.Split('-')[0].Trim()));
                        Subject = string.Format("Incoming Package for {0} has been rejected", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim());
                        Common.SendEmailToRole("Reviewer", gvCBC.Rows[0].Cells[4].Text.Split('-')[0].Trim(), Subject, FDRPage.Statuses.Approved, senderEmail);
                        MessageBox.Show(string.Format("Incoming Foreign Package for {0} has been rejected successfully", gvCBC.Rows[0].Cells[3].Text.Split('-')[0]));
                    }

                }
            }
            txtComments.Text = "";
            var btnApprove = Parent.FindControl("btnApprove") as Button;
            var btnReject = Parent.FindControl("btnReject") as Button;
            btnReject.Enabled = false;
            btnApprove.Enabled = false;
        }

        else
        {
            MessageBox.Show("Please Enter comments");
            if (mpeMe != null)
            {
                mpeMe.Show();
            }
        }
    }
    private int SaveComments(Comments comments, int type)
    {
        return DatabaseWriter.SaveComments(comments, type);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtComments.Text = "";
        /* var mpeMe = Parent.FindControl("ModalPopupExtender1") as AjaxControlToolkit.ModalPopupExtender;
         if (mpeMe != null)
         {
             mpeMe.Hide();
         }*/
    }

    public Button btnAdd
    {
        get { return btnAddComments; }
    }

    public TextBox txtNewComments
    {
        get { return ViewState["txtComments"] as TextBox; }
        set { ViewState["txtComments"] = value; }
    }
    public Button btnAddNewComments
    {
        get { return ViewState["btnAddComment"] as Button; }
        set { ViewState["btnAddComment"] = value; }
    }
}

public interface IUserControl
{
    TextBox txtNewComments { get; set; }
    Button btnAddNewComments { get; set; }
}