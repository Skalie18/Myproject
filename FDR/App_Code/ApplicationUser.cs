using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for SarsUserr
/// </summary>
public class ApplicationUser
{
    public static string UserName
    {
        get {return HttpContext.Current.User.Identity.Name; }
    }
    public static string UserMachine
    {
        get
        {     
            var host = Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
            var machineName = host.HostName.Split(new char[] { '.' })[0].ToUpper();
            return machineName;
        }
    }
}
