using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Models.CBC;
using Sars.Systems.Data;
using Sars.Systems.Utilities;

public partial class pages_displayOutgoingFiles : System.Web.UI.Page
{
    FileSubmission _fileSubmission = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(SubmissionId))
        {
            MessageBox.Show("Could not retrieve documents - Submission ID is missing");
            return;
        }
        if (!IsPostBack)
        {
            _fileSubmission = DBReadManager.GeFileSubmissionById(Convert.ToDecimal(SubmissionId));
            LoadFiles();
            if ( _fileSubmission != null )
            {
                if ( _fileSubmission.SubmissionStatusId == 3 ||
                    _fileSubmission.SubmissionStatusId == 4 ||
                    _fileSubmission.SubmissionStatusId == 5 )
                {
                    btnSave.Enabled = false;
                    btnSubmit.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSubmit.Enabled = true;
                }
            }

            if ( gvLocalFiles.Rows.Count == 0 && gvMasterFiles.Rows.Count == 0 )
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
            }
        }
    }

    private void LoadFiles()
    {
        using (var files = DBReadManager.GetFilesPerSubmission(SubmissionId))
        {
            //if (!files.HasRows) return;
            var view = files.Tables[0].DefaultView;

            view.RowFilter = "FileCategoryID='1'";
            gvMasterFiles.Bind(view);
            view.RowFilter = "FileCategoryID='2'";
            gvLocalFiles.Bind(view);
        }
    }
    public string SubmissionId
    {
        get { return Request["submissionId"]; }
    }

    protected void gvMasterFiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }
        var ddlOutcome = e.Row.FindControl("ddlOutcome") as DropDownList;
        var txtOutcomeReason = e.Row.FindControl("txtOutcomeReason") as TextBox;
        if (ddlOutcome != null && txtOutcomeReason  != null)
        {
            ddlOutcome.Bind(DBReadManager.GetFileValidationOutcomes(), "Description", "ID");
            var fileId = DataBinder.Eval(e.Row.DataItem, "FileId");
            if (fileId != null)
            {
                var details = DBReadManager.GeFileValidationOutcomeDetailsByFileId(Convert.ToDecimal(fileId));
                if (details != null)
                {
                    ddlOutcome.SelectItemByValue(details.ValidationOutcomeId);
                    txtOutcomeReason.SetValue(details.OutcomeReason);
                }
            }

            if ( _fileSubmission == null )
            {
                _fileSubmission = DBReadManager.GeFileSubmissionById(Convert.ToDecimal(SubmissionId));
            }
            if ( _fileSubmission.SubmissionStatusId == 3 ||
                _fileSubmission.SubmissionStatusId == 4 ||
                _fileSubmission.SubmissionStatusId == 5 )
            {
                ddlOutcome.Enabled = false;
            }
            else
            {
                ddlOutcome.Enabled = true;
            }
        }

        var category = DataBinder.Eval(e.Row.DataItem, "Category").ToString();
        var fileName = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();

        var body = new StringBuilder();
        body.Append("<table style='width:100%;padding:5px'><tr style='background:khaki;'>");
        body.Append("<td><b>Category:</b></td>");
        body.AppendFormat ("<td>{0}</td>", category);
        body.Append("</tr>");

        body.Append("<tr style='background:khaki;'>");
        body.Append("<td><b>File Name:</b></td>");
        body.AppendFormat("<td>{0}</td>", fileName);
        body.Append("</tr>");

        body.Append("</table>");
        e.Row.Attributes.Add("title", "cssbody=[dvbdy1] cssheader=[dvhdr1] header=[<b><font color='blue'>More Details</font></b>] body=[" + body + "]");

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["bck"]))
        {
            var url = Request["bck"].Base64ToString();
            Response.Redirect( url);
        }
        else
        {
            Response.Redirect("Viewsubmissions.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        var listOfFiles = new List<FileValidationOutcomeDetails>();

        const int submissionStatus = 2; //In Progress
        foreach ( GridViewRow row in gvLocalFiles.Rows )
        {
            var ddlOutcome = row.FindControl("ddlOutcome") as DropDownList;
            var txtOutcomeReason = row.FindControl("txtOutcomeReason") as TextBox;
            if ( ddlOutcome != null && txtOutcomeReason != null )
            {
                ddlOutcome.BackColor = Color.Transparent;
                if ( ddlOutcome.SelectedIndex <= 0 )
                {
                    ddlOutcome.BackColor = Color.Red;
                    continue;
                }
                var dataKey = gvLocalFiles.DataKeys[row.RowIndex];
                if ( dataKey != null )
                {
                    var fileId = Convert.ToDecimal(dataKey.Value);
                    listOfFiles.Add(
                        new FileValidationOutcomeDetails
                        {
                            Id = 0,
                            SubmissionId = Convert.ToDecimal(SubmissionId),
                            FileId = fileId,
                            ValidationOutcomeId = Convert.ToInt32(ddlOutcome.SelectedValue),
                            SID = Sars.Systems.Security.ADUser.CurrentSID
                            ,
                            OutcomeReason = txtOutcomeReason.Text
                        }
                        );
                }
            }
        }
        foreach ( GridViewRow row in gvMasterFiles.Rows )
        {
            var ddlOutcome = row.FindControl("ddlOutcome") as DropDownList;
            var txtOutcomeReason = row.FindControl("txtOutcomeReason") as TextBox;
            if ( ddlOutcome != null  && txtOutcomeReason != null)
            {
                ddlOutcome.BackColor = Color.Transparent;
                if ( ddlOutcome.SelectedIndex <= 0 )
                {
                    ddlOutcome.BackColor = Color.Red;
                    continue;
                }
                var dataKey = gvMasterFiles.DataKeys[row.RowIndex];
                if ( dataKey != null )
                {
                    var fileId = Convert.ToDecimal(dataKey.Value);
                    listOfFiles.Add(
                        new FileValidationOutcomeDetails
                        {
                            Id = 0,
                            SubmissionId = Convert.ToDecimal(SubmissionId),
                            FileId = fileId,
                            ValidationOutcomeId = Convert.ToInt32(ddlOutcome.SelectedValue),
                            SID = Sars.Systems.Security.ADUser.CurrentSID,
                            OutcomeReason = txtOutcomeReason.Text
                        }
                        );
                }
            }
        }
        if ( listOfFiles.Any() )
        {
            listOfFiles.ForEach(detailse =>DBWriteManager.SaveFileValidationOutcome(detailse));
            MessageBox.Show("Validation saved successfully");
            DBWriteManager.ChangeSubmissionStatus(Convert.ToDecimal(SubmissionId), submissionStatus);
        }
        else
        {
            DBWriteManager.ChangeSubmissionStatus(Convert.ToDecimal(SubmissionId), submissionStatus);
            MessageBox.Show("File validations saved successfully.");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int j = 0, k = 0;
        var listOfFiles = new List<FileValidationOutcomeDetails>();
        foreach (GridViewRow row in gvMasterFiles.Rows){
            var ddlOutcome = row.FindControl("ddlOutcome") as DropDownList;
            var txtOutcomeReason = row.FindControl("txtOutcomeReason") as TextBox;
            if (ddlOutcome != null && txtOutcomeReason != null){
                ddlOutcome.BackColor = Color.Transparent;
                if (ddlOutcome.SelectedIndex <= 0){
                    ddlOutcome.BackColor = Color.Red;
                    k++;
                    break;
                }
                var dataKey = gvMasterFiles.DataKeys[row.RowIndex];
                if (dataKey != null){
                    var fileId = Convert.ToDecimal(dataKey.Value);
                    listOfFiles.Add(new FileValidationOutcomeDetails{
                            Id = 0,
                            SubmissionId = Convert.ToDecimal(SubmissionId),
                            FileId = fileId,
                            ValidationOutcomeId = Convert.ToInt32(ddlOutcome.SelectedValue),
                            SID = Sars.Systems.Security.ADUser.CurrentSID,
                            OutcomeReason = string.IsNullOrEmpty(txtOutcomeReason.Text) ? null : txtOutcomeReason.Text
                        }
                        );
                }
            }
        }

        foreach (GridViewRow row in gvLocalFiles.Rows){
            var ddlOutcome = row.FindControl("ddlOutcome") as DropDownList;
            var txtOutcomeReason = row.FindControl("txtOutcomeReason") as TextBox;
            if (ddlOutcome != null && txtOutcomeReason != null){
                ddlOutcome.BackColor = Color.Transparent;
                if (ddlOutcome.SelectedIndex <= 0){
                    ddlOutcome.BackColor = Color.Red;
                    j++;
                    break;
                }
                var dataKey = gvLocalFiles.DataKeys[row.RowIndex];
                if (dataKey != null){
                    var fileId = Convert.ToDecimal(dataKey.Value);
                    listOfFiles.Add(
                        new FileValidationOutcomeDetails{
                            Id = 0,
                            SubmissionId = Convert.ToDecimal(SubmissionId),
                            FileId = fileId,
                            ValidationOutcomeId = Convert.ToInt32(ddlOutcome.SelectedValue),
                            SID = Sars.Systems.Security.ADUser.CurrentSID,
                            OutcomeReason = string.IsNullOrEmpty(txtOutcomeReason.Text) ? null : txtOutcomeReason.Text
                        }
                        );
                }
            }
        }

        if (k > 0){
            tbMain.ActiveTabIndex = 0;
            MessageBox.Show("Please select validation outcome for all the files.");
            return;
        }
        if (j > 0){
            tbMain.ActiveTabIndex = 1;
            MessageBox.Show("Please select validation outcome for all the files.");
            return;
        }
        if (listOfFiles.Any()){
            var numSaved = 0;
            foreach (var detailse in listOfFiles){
                numSaved += DBWriteManager.SaveFileValidationOutcome(detailse);
            }
            if (_fileSubmission == null){
                _fileSubmission = DBReadManager.GeFileSubmissionById(Convert.ToDecimal(SubmissionId));
            }
            if (numSaved == (gvLocalFiles.Rows.Count + gvMasterFiles.Rows.Count)){
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                MessageBox.Show("Validation submitted successfully");
            }

            var status = 0;
            var accepted = listOfFiles.Count(a => a.ValidationOutcomeId == 1);
            var rejected = listOfFiles.Count(a => a.ValidationOutcomeId == 2);
            if (accepted == listOfFiles.Count()){
                // accepted = 3
                status = 3;
                try{
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactMobileNumber) &&_fileSubmission.ContactMobileNumber.IsValid( StringValidationType.CellularNumber)){
                        var smsBody = DBReadManager.GetMasterLocalFileNotificationSmsBodyTemplate(2);
                        if (!string.IsNullOrEmpty(smsBody)){
                            var service = new FDRQueueService();
                            service.SendSms(
                                    _fileSubmission.ContactMobileNumber
                                    , _fileSubmission.TaxRefNo
                                    , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                    , _fileSubmission.Year
                                );
                            DBWriteManager.SaveSentSmsCommunications(
                                _fileSubmission.TaxRefNo
                                , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                , _fileSubmission.Year
                                );
                        }
                    }
                }
                catch (Exception){
                    ;
                }
                try{
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactEmail))
                    {
                        byte[] attachment = null;//FdrCommon.GetFileAcceptenceLetter(SubmissionId);
                        var service = new FDRQueueService();
                        var messageBody = string.Format(DBReadManager.GetMasterLocalFileNotificationEmailBodyTemplate(2),_fileSubmission.TaxRefNo);
                        service.SendEmail
                            (
                                messageBody
                                , "CBC File validation outcome"
                                , _fileSubmission.ContactEmail
                                , null
                                , "CBC File validation outcome"
                                , _fileSubmission.TaxRefNo
                                , _fileSubmission.Year
                            );
                    }
                }
                catch (Exception){
                    ;
                }

                try{
                    var attachment = FdrCommon.GetEfilingAcceptanceLetter( _fileSubmission.Year);
                    if ( !string.IsNullOrEmpty(attachment) ){
                        var service = new FDRQueueService();
                        service.SendLetter
                            (
                                  _fileSubmission.SubmissionId
                                , _fileSubmission.TaxRefNo
                                , _fileSubmission.Year
                                , attachment
                                , "Acceptance of Master Files and Local Files"
                                , true
                                , Sars.Systems.Security.ADUser.CurrentSID
                            );
                    }
                }
                catch ( Exception )
                {
                    ;
                }
            }
            else if (rejected == listOfFiles.Count()){
                //rejected = 4
                status = 4;
                try{
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactMobileNumber) &&_fileSubmission.ContactMobileNumber.IsValid(StringValidationType.CellularNumber)){
                        var smsBody = DBReadManager.GetMasterLocalFileNotificationSmsBodyTemplate(3);
                        if (!string.IsNullOrEmpty(smsBody)){
                            var service = new FDRQueueService();
                            service.SendSms
                                (
                                    _fileSubmission.ContactMobileNumber
                                    , _fileSubmission.TaxRefNo
                                    , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                    , _fileSubmission.Year
                                );

                            DBWriteManager.SaveSentSmsCommunications(
                                _fileSubmission.TaxRefNo
                                , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                , _fileSubmission.Year
                                );
                        }
                    }
                }
                catch (Exception){
                    ;
                }
                try{
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactEmail)){
                        //var attachment = FdrCommon.GetFileRejectionLetter(SubmissionId);
                        var service = new FDRQueueService();
                        var messageBody = string.Format(
                            DBReadManager.GetMasterLocalFileNotificationEmailBodyTemplate(3),
                            _fileSubmission.TaxRefNo);
                        service.SendEmail(
                                messageBody
                                , "CBC File validation outcome"
                                , _fileSubmission.ContactEmail
                                , null
                                , "CBC File validation outcome"
                                , _fileSubmission.TaxRefNo
                                , _fileSubmission.Year
                            );
                    }
                }
                catch (Exception){
                    ;
                }

                //SEND EFILING REJECTION LETTERS
                try{
                    var attachment = FdrCommon.GetEfilingRejectionLetter(_fileSubmission.TaxRefNo, _fileSubmission.Year, Convert.ToDecimal(SubmissionId));
                    if (!string.IsNullOrEmpty(attachment) ){
                        var service = new FDRQueueService();
                        service.SendLetter
                            (
                                _fileSubmission.SubmissionId
                                ,_fileSubmission.TaxRefNo
                                , _fileSubmission.Year
                                , attachment
                                , "Rejection of Master Files and Local Files"
                                , false
                                , Sars.Systems.Security.ADUser.CurrentSID
                            );
                    }
                }
                catch (Exception exception){
                    MessageBox.Show(exception.ToString());
                }
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactMobileNumber) &&_fileSubmission.ContactMobileNumber.IsValid(StringValidationType.CellularNumber))
                    {
                        var smsBody = DBReadManager.GetMasterLocalFileNotificationSmsBodyTemplate(4);
                        if (!string.IsNullOrEmpty(smsBody))
                        {
                            var service = new FDRQueueService();
                            service.SendSms
                                (
                                    _fileSubmission.ContactMobileNumber
                                    , _fileSubmission.TaxRefNo
                                    , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                    , _fileSubmission.Year
                                );
                            DBWriteManager.SaveSentSmsCommunications(
                                _fileSubmission.TaxRefNo
                                , string.Format(smsBody, _fileSubmission.TaxRefNo)
                                , _fileSubmission.Year
                                );
                        }
                    }
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    if (!string.IsNullOrEmpty(_fileSubmission.ContactEmail))
                    {
                        //var attachment = FdrCommon.GetFileRejectionLetter(SubmissionId);
                        var service = new FDRQueueService();
                        var messageBody = string.Format(
                            DBReadManager.GetMasterLocalFileNotificationEmailBodyTemplate(4),
                            _fileSubmission.TaxRefNo);
                        service.SendEmail(
                            messageBody
                            , "CBC File validation outcome"
                            , _fileSubmission.ContactEmail
                            , null
                            , "CBC File validation outcome"
                            , _fileSubmission.TaxRefNo
                            , _fileSubmission.Year
                            );
                    }
                }
                catch (Exception)
                {
                    ;
                }

                //SEND EFILING ACCEPTANCE LETTERS

                try
                {
                    var attachment = FdrCommon.GetEfilingRejectionLetter( _fileSubmission.TaxRefNo, _fileSubmission.Year, Convert.ToDecimal(SubmissionId));
                    if ( !string.IsNullOrEmpty(attachment) )
                    {
                        var service = new FDRQueueService();
                        service.SendLetter
                            (
                                 _fileSubmission.SubmissionId
                                , _fileSubmission.TaxRefNo
                                , _fileSubmission.Year
                                , attachment
                                , "Rejection of Master Files and Local Files"
                                , false
                                , Sars.Systems.Security.ADUser.CurrentSID
                            );
                    }
                }
                catch ( Exception )
                {
                    ;
                }
                //Accepted With Warnings = 5
                status = 5;
            }
            if (status != 0)
            {
                DBWriteManager.ChangeSubmissionStatus(Convert.ToDecimal(SubmissionId), status);
            }
        }
        else
        {
            MessageBox.Show("No validation was submitted");
        }
    }

    protected void FileOpenCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument != null)
        {
            var btn = sender as LinkButton;
            if (btn == null){
                return;
            }
            var row = btn.NamingContainer as GridViewRow;
                if(row == null ) { return;}
            var gv = row.NamingContainer as GridView;
            if(gv == null ) { return;}
            gv.SelectedIndex = row.RowIndex;
            if (gv.SelectedDataKey != null)
            {
                if ( _fileSubmission == null)
                {
                    _fileSubmission = DBReadManager.GeFileSubmissionById(Convert.ToDecimal(SubmissionId));
                }
                var fileId = gv.SelectedDataKey.Value;
                Response.Redirect(string.Format("outgoingfiledetails.aspx?oId={0}&fId={1}&submissionId={2}&refNo={3}", e.CommandArgument, fileId, SubmissionId, _fileSubmission.TaxRefNo));
            }
        }
    }
}