using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_viewCaseFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["taxRefNo"] != null & Request.QueryString["CaseNo"] != null)
            {


                var taxRefNo = Request.QueryString["taxRefNo"].ToString();
                var CaseNo = Request.QueryString["CaseNo"].ToString();

                var files = DatabaseReader.getUploadedCaseFiles(taxRefNo, CaseNo);
                gvCaseFiles.Bind(files);
            }
        }

        else {

        }
    }

    protected void FileOpenCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument != null)
        {
            var btn = sender as LinkButton;
            if (btn == null)
            {
                return;
            }
            var row = btn.NamingContainer as GridViewRow;
            if (row == null) { return; }
            var gv = row.NamingContainer as GridView;
            if (gv == null) { return; }
            gv.SelectedIndex = row.RowIndex;
            if (gv.SelectedDataKey != null)
            {
               
                var CaseNo = row.Cells[1].Text;
                var TaxRefNo = row.Cells[2].Text;
           
                var fileId = gv.SelectedDataKey.Value;
                Response.Redirect(string.Format("uploadedDocuments.aspx?oId={0}&fId={1}&TaxRefNo={2}&CaseNo={3}", e.CommandArgument, fileId,TaxRefNo,CaseNo));
            }
        }
    }
}