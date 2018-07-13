using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
//ing Sars.Systems.Security;
public partial class pages_ManageMNE : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            LoadMNEList();
        }
    }

    protected void gvMNE_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMNE.NextPage(Session["mne-list"], e.NewPageIndex);
    }

    public void LoadMNEList(string texrefNo = null)
    {
        Session["mne-list"] = DatabaseReader.GetMultiNationalEntities(texrefNo);
        gvMNE.Bind(Session["mne-list"]);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if ( !string.IsNullOrEmpty(txtSearch.Text) ) { LoadMNEList(txtSearch.Text); }
    }

    protected void gvMNE_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var command = e.CommandName;
        var id = e.CommandArgument;
        if(command.Equals("Modify", StringComparison.InvariantCultureIgnoreCase) )
        {
            Response.Redirect(string.Format("modifymnedetails.aspx?Id={0}", id));
        }

        if (command.Equals("Remove", StringComparison.InvariantCultureIgnoreCase))
        {
            var sid =  Sars.Systems.Security.ADUser.CurrentSID;
            DBWriteManager.DeleteMultinational( Convert.ToDecimal(id), sid);
            MessageBox.Show("Record Deleted successfully");
            LoadMNEList();
        }
    }
   
}