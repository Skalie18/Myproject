using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Models.CBC;

public partial class pages_ConfigureNotificationSms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var templates =
                DBReadManager.GetMasterLocalFileNotificationSmsBodyTemplates();
            if (templates != null)
            {
                txtFileAccepted.Text = templates.AcceptedBody;
                txtFileAcceptedWithWarnings.Text = templates.AcceptedWithWarningsBody;
                txtFileReceived.Text = templates.ReceivedBody;
                txtFileRejected.Text = templates.RejectedBody;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var fileReceivedBody = ( txtFileReceived.Text);
        var fileAcceptedBody = (txtFileAccepted.Text);
        var fileRejectedBody = (txtFileRejected.Text);
        var fileAcceptedWithWarningsBody = (txtFileAcceptedWithWarnings.Text);

        var saved =DBWriteManager.SaveMasterLocalFileNotificationSmsBodyTemplates( new MasterLocalFileNotificationSmsBodyTemplate
        {
            Id = 0,
            AcceptedBody = fileAcceptedBody,
            AcceptedWithWarningsBody = fileAcceptedWithWarningsBody,
            RejectedBody = fileRejectedBody,
            ReceivedBody = fileReceivedBody,
            CreatedBy = Sars.Systems.Security.ADUser.CurrentSID,
            DateLastModified = DateTime.Now,
            LastModifiedBy = Sars.Systems.Security.ADUser.CurrentSID
        } );
        MessageBox.Show(saved > 0
            ? "Master/Local File Notification SMS Templates saved successfully"
            : "Issues reported during saving");

        btnSave.Enabled = false;
    }
}