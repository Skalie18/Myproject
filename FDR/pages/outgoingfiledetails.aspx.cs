using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Models.CBC;

public partial class pages_outgoingfiledetails : System.Web.UI.Page
{
    private FileSubmission _fileSubmission = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
      
        if (string.IsNullOrEmpty(Request["fId"]))
        {
            MessageBox.Show("File ID parameter is missing");
            return;
        }
        if ( string.IsNullOrEmpty(Request["oId"]) )
        {
            MessageBox.Show("Object ID parameter is missing");
            return;
        }

        if (string.IsNullOrEmpty(Request["submissionId"]))
        {
            MessageBox.Show("Submission ID parameter is missing");
            return;
        }
        _fileSubmission = DBReadManager.GeFileSubmissionById(Convert.ToDecimal(SubmissionId));
        if ( _fileSubmission != null )
        {

            if ( _fileSubmission.SubmissionStatusId == 3 ||
                _fileSubmission.SubmissionStatusId == 4 ||
                _fileSubmission.SubmissionStatusId == 5 )
            {
                Toolbar1.Items[0].Visible = false;
            }
            else
            {
                Toolbar1.Items[0].Visible = true;
            }
        }
        var fileId = Convert.ToDecimal(Request["fId"]);
        using (var data = DBReadManager.GetFileById(fileId))
        {
            if (data.HasRows)
            {
                gvDetails.Bind(data);
            }
        }

    }
    public string SubmissionId
    {
        get { return Request["submissionId"]; }
    }

    protected void gvDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if ( e.Row.RowType != DataControlRowType.DataRow ) { return; }
        var ddlOutcome = e.Row.FindControl("ddlOutcome") as DropDownList;
        var txtOutcomeReason = e.Row.FindControl("txtOutcomeReason") as TextBox;
        if ( ddlOutcome != null && txtOutcomeReason != null)
        {
            ddlOutcome.Bind(DBReadManager.GetFileValidationOutcomes(), "Description", "ID");
            var fileId = DataBinder.Eval(e.Row.DataItem, "FileId");
            if ( fileId != null ){
                var details = DBReadManager.GeFileValidationOutcomeDetailsByFileId(Convert.ToDecimal(fileId));
                if ( details != null ){
                    ddlOutcome.SelectItemByValue(details.ValidationOutcomeId);
                    txtOutcomeReason.SetValue(details.OutcomeReason);
                }
            }
            if ( _fileSubmission != null ){
                if ( _fileSubmission.SubmissionStatusId == 3 ||
                    _fileSubmission.SubmissionStatusId == 4 ||
                    _fileSubmission.SubmissionStatusId == 5 ){
                    ddlOutcome.Enabled = false;
                }
                else{
                    ddlOutcome.Enabled = true;
                }
            }
        }

        var category = DataBinder.Eval(e.Row.DataItem, "Category").ToString();
        var fileName = DataBinder.Eval(e.Row.DataItem, "FileName").ToString();

        var body = new StringBuilder();
        body.Append("<table style='width:100%;padding:5px'><tr style='background:khaki;'>");
        body.Append("<td><b>Category:</b></td>");
        body.AppendFormat("<td>{0}</td>", category);
        body.Append("</tr>");

        body.Append("<tr>");
        body.Append("<td><b>File Name:</b></td>");
        body.AppendFormat("<td>{0}</td>", fileName);
        body.Append("</tr>");

        body.Append("</table>");
        e.Row.Attributes.Add("title", "cssbody=[dvbdy1] cssheader=[dvhdr1] header=[<b><font color='blue'>More Details</font></b>] body=[" + body + "]");
    }

    protected void Toolbar1_ButtonClicked(object sender, SCS.Web.UI.WebControls.ButtonEventArgs e)
    {
        if ( e.CommandName.Equals("BACK")){
            Response.Redirect(string.Format("displayoutgoingfiles.aspx?submissionId={0}", SubmissionId));
        }
        if(e.CommandName.Equals("SAVE")){
            if (gvDetails.Rows.Count == 0){
                MessageBox.Show("There is no file details.");
                return;
            }
            var row = gvDetails.Rows[0];
            var ddlOutcome =row.FindControl("ddlOutcome") as DropDownList;
            var txtOutcomeReason = row.FindControl("txtOutcomeReason") as TextBox;
            if (ddlOutcome != null && txtOutcomeReason  != null)
            {
                var detailse = new FileValidationOutcomeDetails{
                    Id = 0,
                    SubmissionId = Convert.ToDecimal(SubmissionId),
                    FileId = Convert.ToDecimal(Request["fId"]),
                    ValidationOutcomeId = Convert.ToInt32(ddlOutcome.SelectedValue),
                    SID = Sars.Systems.Security.ADUser.CurrentSID,
                    OutcomeReason = string.IsNullOrEmpty(txtOutcomeReason.Text) ? null :txtOutcomeReason.Text
                };
                var saved = DBWriteManager.SaveFileValidationOutcome(detailse);
                if (saved > 0){
                    MessageBox.Show(string.Format("Validation Saved as [{0}]", ddlOutcome.SelectedItem.Text));
                }
            }
        }
    }
}