using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using Sars.Systems.Utilities;
using Sars.Systems.Data;

public partial class pages_Incomingcbcdeclataions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var request = Request.QueryString["New"] == null ? 0 : Convert.ToInt32(Request.QueryString["New"].ToString());
            LoadCBC(request);
        }
    }

    private void LoadCBC(int req)
    {
        //var declarations1 = DatabaseReader.GetNewCbcDeclarations();
        var declarations = DatabaseReader.GetCbcDeclarations(req);
        Session["declarations"] = declarations;
        gvCBC.Bind(declarations);
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

                    var year = gvCBC.SelectedDataKey[0].ToString();
                    var id = gvCBC.SelectedDataKey[1].ToString();
                    var taxRefNo = btn.CommandArgument.Split('|')[0];
                    var request = Request.QueryString["New"] == null ? 0 : Convert.ToInt32(Request.QueryString["New"]);
                    if (request == 0)
                        DatabaseWriter.UpdateViewedCBC(Convert.ToDecimal(id));

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

  
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var taxRefNo = txtTexRefNo.FieldValue != null ? txtTexRefNo.Text :
        null;
        var year = !string.IsNullOrEmpty(txtYear.Text) ? Convert.ToInt32(txtYear.Text) : 0;
        var declarations = DBReadManager.GetNewCbcDeclarations(taxRefNo, year);

        if (!declarations.HasRows)
            MessageBox.Show("No Declarations found for Tax Ref No " + taxRefNo);
        gvCBC.Bind(declarations);

    }

    protected void gvCBC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCBC.NextPage(Session["declarations"], e.NewPageIndex);
    }

  
}