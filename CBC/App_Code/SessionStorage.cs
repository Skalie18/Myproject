using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionStorage
/// </summary>
public class SessionStorage
{
    public SessionStorage()
    {

    }

    public static string contactPersonDetailsId
    {
        get
        {
            return HttpContext.Current.Session["contactPersonDetailsId"] as string;
        }
        set
        {
            HttpContext.Current.Session["contactPersonDetailsId"] = value;
        }
    }

    public static string reportEntityId
    {
        get
        {
            return HttpContext.Current.Session["reportEntityId"] as string;
        }
        set
        {
            HttpContext.Current.Session["reportEntityId"] = value;
        }
    }
    public static string entityAddressId
    {
        get
        {
            return HttpContext.Current.Session["entityAddressId"] as string;
        }
        set
        {
            HttpContext.Current.Session["entityAddressId"] = value;
        }
    }

    public static string cbcReportId
    {
        get
        {
            return HttpContext.Current.Session["cbcReportId"] as string;
        }
        set
        {
            HttpContext.Current.Session["cbcReportId"] = value;
        }
    }

    public static string reportSummaryId
    {
        get
        {
            return HttpContext.Current.Session["reportSummaryId"] as string;
        }
        set
        {
            HttpContext.Current.Session["reportSummaryId"] = value;
        }
    }

    public static string revenueId
    {
        get
        {
            return HttpContext.Current.Session["revenueId"] as string;
        }
        set
        {
            HttpContext.Current.Session["revenueId"] = value;
        }
    }

    public static string constituentEntityId
    {
        get
        {
            return HttpContext.Current.Session["constituentEntityId"] as string;
        }
        set
        {
            HttpContext.Current.Session["constituentEntityId"] = value;
        }
    }

    public static string constituentEntityDataId
    {
        get
        {
            return HttpContext.Current.Session["constituentEntityDataId"] as string;
        }
        set
        {
            HttpContext.Current.Session["constituentEntityDataId"] = value;
        }
    }

    public static string addressId
    {
        get
        {
            return HttpContext.Current.Session["addressId"] as string;
        }
        set
        {
            HttpContext.Current.Session["addressId"] = value;
        }
    }

    public static string businessActivityId
    {
        get
        {
            return HttpContext.Current.Session["businessActivityId"] as string;
        }
        set
        {
            HttpContext.Current.Session["businessActivityId"] = value;
        }
    }

    public static string otherInfoId
    {
        get
        {
            return HttpContext.Current.Session["otherInfoId"] as string;
        }
        set
        {
            HttpContext.Current.Session["otherInfoId"] = value;
        }
    }

    public static string countrySummaryCodeId
    {
        get
        {
            return HttpContext.Current.Session["countrySummaryCodeId"] as string;
        }
        set
        {
            HttpContext.Current.Session["countrySummaryCodeId"] = value;
        }
    }
}