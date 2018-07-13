using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sars.Systems.Security;
using System.Data;
using Sars.Systems.Extensions;
using System.Configuration;
/// <summary>
/// Summary description for FDRPage
/// </summary>
public class FDRPage : System.Web.UI.Page
{
    public FDRPage()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string UserRole
    {
        get { return User.GetRole(); }
    }

    public DataSet GetUsersInRole(string role)
    {
        return Roles.GetAllUsersInRole(role);
    }

    public bool IsUserInRole(string role)
    {
        return User.InRole(role.ToLower());
    }

    public string Url(Statuses status)
    {
        switch (status)
        {
            case Statuses.Rejected:
                return Server.MapPath("~/letters/CBCReportRejected.html");
            case Statuses.Approved:
            case Statuses.Verified:
            case Statuses.Corrected:
            case Statuses.Voided:
                return Server.MapPath("~/letters/CBCReportVerified.html");
            case Statuses.DeletePackage:
                return Server.MapPath("~/letters/CBCPackageExists.htm");
                case Statuses.Accepted:
                return Server.MapPath("~/letters/ApprovedCBCFromMNE.html");
        }
        return "";
    }

    public enum Statuses
    {
        Approved,
        Rejected,
        Verified,
        Corrected,
        Voided,
        Accepted,
        DeletePackage,
    }

    
}