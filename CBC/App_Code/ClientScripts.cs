using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sars.Systems.Controls;

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
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "_alert", script, false);
        }
    }

 

    public static void ShowModal(string div)
    {
        var script = " function () {" +
                     "$(\"#" + div + "\").dialog({" +
                     "title: \"Personal Information\"," +
                     "width: 500," +
                     "height: 450," +
                     "modal: true," +
                     "buttons:" +
                     "{" +
                     "Close: function() {" +
                     "$(this).dialog('close');" +
                     "}" +
                     "}" +
                     "});" +
                     "};";
        var page = HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("_modal"))
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "_modal", script, true);
        }
    }

   

    public static void ScrollToView(string controlId)
    {
        var script = "<script type=\"text/javascript\">document.getElementById('" + controlId +
                     "').scrollIntoView(true);</script>";
        var page = HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("_scroll"))
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "_scroll", script, false);
        }
    }
}
