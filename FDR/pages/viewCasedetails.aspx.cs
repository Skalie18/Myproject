using FDR.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_viewCasedetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {

            LoadCaseDetails(null);
        }
       
      
    }

    private void LoadCaseDetails(string cases)
    {
        //var declarations = DatabaseReader.GetNewCbcDeclarations();
        var caseDetails = DatabaseReader.getCasedetails(cases);
        Session["caseDetails"] = caseDetails;
        gvCasedetails.Bind(caseDetails);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var taxRefNo = txtTexRefNo.FieldValue != null ? txtTexRefNo.Text : null;
        var files = DatabaseReader.getCasedetails(taxRefNo);

        if (files ==null )
            MessageBox.Show("No Case Details found for Tax Ref No " + taxRefNo);
        gvCasedetails.Bind(files);

    }

    protected void lnkViewDocuments_Click(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn != null)
        {
            var row = btn.NamingContainer as GridViewRow;
            if (row != null)
            {
                gvCasedetails.SelectedIndex = row.RowIndex;
                if (gvCasedetails.SelectedDataKey != null)
                {
                   
                    var taxRefNo = btn.CommandArgument.Split('|');
                    
                    Response.Redirect("viewCaseFiles.aspx?taxRefNo=" + taxRefNo[0] + "&CaseNo=" + taxRefNo[1]);
                }
            }
        }
    }

    protected void gvCasedetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCasedetails.NextPage(Session["caseDetails"], e.NewPageIndex);
    }
}