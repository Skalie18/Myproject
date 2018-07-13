using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// Displays the ie alert message box
/// </summary>
public static class MessageBox
{
    /// <summary>
    /// Shows the message to the user.
    /// </summary>
    /// <param name="message">Message to display.</param>
    public static void Show(string message)
    {
        var escapeChars = message.Replace("'", "\\'");
        var script = "<script type=\"text/javascript\">alert('" + escapeChars + "');</script>";
        var page = HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "_alert", script, false);
    }

    public static void Show(object queueTimeoutMessage)
    {
        throw new NotImplementedException();
    }
}

public static class ClientScriptManager
{
    public static void ScrolToBottom( )
    {

        const string script = "<script type=\"text/javascript\">window.scrollTo(0,document.body.Height);</script>";
        var page = HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered( "_scrolDown" ))
            ScriptManager.RegisterClientScriptBlock( page, page.GetType( ), "_scrolDown", script, false );
        //window.scrollTo(0,document.body.scrollHeight);
    }
}
