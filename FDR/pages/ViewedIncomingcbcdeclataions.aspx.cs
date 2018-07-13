using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;

public partial class Pages_ViewedIncomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCBC();
        }
    }
    private void LoadCBC()
    {
        var declarations = DBReadManager.GetViewedCbcDeclarations();
        Session["declarations"] = declarations;
        gvCBC.Bind(declarations);
    }

    protected void lnkViewForm_Click(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn == null) return;
        var row = btn.NamingContainer as GridViewRow;
        if (row == null) return;
        gvCBC.SelectedIndex = row.RowIndex;
        if (gvCBC.SelectedDataKey == null) return;

        var year = gvCBC.SelectedDataKey.Value;
        Response.Redirect
            (
                string.Format("~/pages/cbcForm.aspx?incLocal={0}&refno={1}&year={2}&bck={3}&mspecId={4}"
                , 1
                , btn.CommandArgument
                , year
                , Request.Url.PathAndQuery.ToBase64String()
                , 0)
            );
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var taxRefNo = txtTexRefNo.FieldValue != null ? txtTexRefNo.Text :
        null;
        var year = !string.IsNullOrEmpty(txtYear.Text) ? Convert.ToInt32(txtYear.Text) : 0;
        var declarations = DBReadManager.GetViewedCbcDeclarations(taxRefNo, year);
        gvCBC.Bind(declarations);
    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["declarations"], e.NewPageIndex);
    }

    protected void lnkViewReport_OnClick(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn == null) return;
        var row = btn.NamingContainer as GridViewRow;
        if (row == null) return;
        gvCBC.SelectedIndex = row.RowIndex;
        if (gvCBC.SelectedDataKey == null) return;

        if (btn.CommandArgument != null)
        {
            string fullURL = "window.open('../queueMonitors/MANUAL/CbCReport.aspx?UID=" + btn.CommandArgument + "&src=MNE ', '_blank', 'status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no,titlebar=no' );";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            // Response.Redirect(string.Format("~/queueMonitors/MANUAL/CbCReport.aspx?UID={0}&src=IN", results.UID));
        }
    }
}