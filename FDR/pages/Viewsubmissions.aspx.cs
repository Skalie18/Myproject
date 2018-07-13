using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class pages_Viewsubmissions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var data = DBReadManager.GetSubmissionsByStatus(1);
            Session["subdata"] = data;
            gvMNE.Bind(data);
        }
    }

    protected void gvMNE_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMNE.NextPage(Session["subdata"], e.NewPageIndex);
    }

    protected void gvMNE_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var command = e.CommandName;
        var id = e.CommandArgument;
        if ( command.Equals("ViewFiles", StringComparison.InvariantCultureIgnoreCase) )
        {
            Response.Redirect(string.Format("displayfiles.aspx?submissionId={0}", id));
        }
    }

    protected void gvMNE_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
        {
            return;
        }
        var regName = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
        var sb = new StringBuilder();
        sb.AppendFormat(
            "cssbody=[dvbdy1] " +
            "cssheader=[dvhdr1] " +
            "header=[<b><font color='blue'>Registered/Trading Name</font></b>] " +
            "body=[<font color='green' style='font-style:italic;word-wrap: break-word!important;'><b>{0}</b></font>]",
            regName);
        e.Row.Attributes.Add("title", sb.ToString());
        e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(( Control ) sender, "Select$" + e.Row.RowIndex));

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var data = DBReadManager.GetSubmissionsByStatus(1, !string.IsNullOrEmpty(txtTaxRefNo.Text) ? txtTaxRefNo.Text : null,
            !string.IsNullOrEmpty(txtYear.Text) ? txtYear.Text : null);
        if (!data.HasRows)
            MessageBox.Show("No File Submission found for Tax Ref No: " + txtTaxRefNo.Text);
        Session["subdata"] = data;
            gvMNE.Bind(data);
       
    }
}