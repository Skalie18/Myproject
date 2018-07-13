using System;
using System.IO;

public partial class Site : System.Web.UI.MasterPage
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
