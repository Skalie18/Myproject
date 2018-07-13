<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }
    void Application_Error(object sender, EventArgs e)
    {
        try
        {
            var error = Server.GetLastError( );
        if (error != null)
        {
            if (error.InnerException != null)
            {
                var sessionId = Session.SessionID;
                var message = error.InnerException.Message;
                var stacktrace = error.InnerException.StackTrace;
                var systemUser = Session["TaxUserID"];

                db.SaveRequestDetails
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
                db.SaveSystemError
                    (
                        sessionId
                        , message
                        , stacktrace
                        , systemUser != null ? systemUser.ToString() : string.Empty
                    );

            }
        }
        }
        catch ( Exception )
        {
            
        }
        

        //Response.Redirect( "~/ErrorPage.aspx" );
    }

    void Session_Start(object sender, EventArgs e)
    {
        if (ApplicationConfigurations.DevTesting)
        {
            //marie
            //Session["TaxUserID"] = 2832049;//2832648;//2830954;//646328;//;
            //Session["TaxPayerID"] = 7105080;//7105577;//8067863;//2235794;//;

            //dkhoza
            //Session["TaxUserID"] = 2832648;//2832049;//2830954;//646328;//;
            //Session["TaxPayerID"] = 7105577;//7105080;//8067863;//2235794;//;


            //Session["TaxUserID"] = 2830954;//2832049;//646328;//;
            //Session["TaxPayerID"] = 8067863;//7105080;//2235794;//;

            /*
             * ################## PRE PROD - QA2############
             */

            //marie
            Session["TaxUserID"] = 2931656;
            Session["TaxPayerID"] = 7278227;
           
	
            //Danisa
            //Session["TaxUserID"] = 2929987;
            //Session["TaxPayerID"] = 7276868;

            //Danisa
            //Session["TaxUserID"] = 2929370;
            //Session["TaxPayerID"] = 7276109;

            Session["SelectedTaxUserID"] = 303;
        }
        Session["TaxUserID"] = 2931656;
            Session["TaxPayerID"] = 7278227;
    }

    void Session_End(object sender, EventArgs e)
    {
    }

</script>
