<%@ WebService Language="C#" Class="fdrdata" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using FDR.DataLayer;

[WebService(Namespace = "http://fdr.gov.za/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class fdrdata : System.Web.Services.WebService
{

    [WebMethod]
    public string GetNextMessageId(string uid)
    {
        return DBReadManager.GetNextMessageId(uid);
    }
    [WebMethod]
    public string GetStatusXml(string uid)
    {
        return DBReadManager.GetStatusXml(uid);
    }
}