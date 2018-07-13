using System;

public partial class EndSession : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        //Response.Redirect(ApplicationConfigurations.LogonURL);
    }
}