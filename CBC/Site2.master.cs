using System;

public partial class Site2 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Session["TaxUserID"] == null )
        {
            Session.Abandon();
            Response.Redirect(ApplicationConfigurations.LogonURL);
        }
    }
}
