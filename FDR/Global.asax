<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    void Application_Error(object sender, EventArgs e)
    {
        var error = Server.GetLastError( );
        if ( error != null )
        {
                var sessionId = Session.SessionID;
                var message = error.Message;
                var stacktrace = error.StackTrace;
                var systemUser = ApplicationUser.UserName;

                DatabaseWriter.SaveRequestDetails
                     (
                         sessionId,
                         Request.AppRelativeCurrentExecutionFilePath,
                         Request.FilePath,
                         Request.PhysicalApplicationPath,
                         Request.RawUrl,
                         Request.UserAgent,
                         Request.IsAuthenticated,
                         Request.ServerVariables["HTTP_HOST"],
                         Request.ServerVariables["AUTH_USER"]
                     );
                DatabaseWriter.SaveSystemError
                     (
                         sessionId
                         , message
                         , stacktrace
                         , systemUser
                     );
                HttpContext.Current.Session["_err"] = error.InnerException.Message;
                Response.Redirect("~/Logging/ErrorPage.aspx");
           
            var httpError =error as HttpException ;
            {
                if ( httpError != null )
                {
                    if ( Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState )
                    {
                        HttpContext.Current.Session["_err"] = "File Not Found";
                    }

                    Response.Redirect("~/Logging/ErrorPage.aspx");
                }
            }
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
</script>
