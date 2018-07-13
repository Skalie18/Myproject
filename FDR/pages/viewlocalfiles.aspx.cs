using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_viewlocalfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack ) { LoadMNEList(); }
    }
    public void LoadMNEList(string texrefNo = null)
    {
        Session["mne-list"] = DatabaseReader.GetMultiNationalEntities(texrefNo);
        gvMNE.Bind(Session["mne-list"]);
    }
    protected void gvMNE_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var command = e.CommandName;
        var id = e.CommandArgument;
        if ( command.Equals("ViewFiles", StringComparison.InvariantCultureIgnoreCase) )
        {
            Response.Redirect(string.Format("localfilelist.aspx?Id={0}", id));
        }
    }
    protected void gvMNE_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMNE.NextPage(Session["mne-list"], e.NewPageIndex);
    }
}