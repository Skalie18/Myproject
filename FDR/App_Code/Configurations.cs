using System;
using System.Configuration;
using System.IO;
using System.Web;
using Sars.Systems.Data;


public static class Configurations
{

    public static bool UseTam
    {
        get
        {
            return ConfigurationManager.AppSettings["use_tam"] != null &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["use_tam"]);
        }
    }

    public static string CurrentEnvironment
    {
        get { return ConfigurationManager.AppSettings["Current-Environment"] ?? string.Empty; }
    }

    public static string AppName
    {
        get { return ConfigurationManager.AppSettings["applicationName"] ?? string.Empty; }
    }

    public static string AppVersion
    {
        get { return ConfigurationManager.AppSettings["version"] ?? string.Empty; }
    }

    public static string ReportServer
    {
        get { return ConfigurationManager.AppSettings["report_server"] ?? string.Empty; }
    }

    public static string SurveyUrl
    {
        get { return ConfigurationManager.AppSettings["surveyURL"] ?? string.Empty; }
    }

    public static string ScriptFolder
    {
        get { return ConfigurationManager.AppSettings["scripts"] ?? "Scripts"; }
    }

    public static string StylesFolder
    {
        get { return ConfigurationManager.AppSettings["css"] ?? "Styles"; }
    }

    public static string LDAPUrl
    {
        get { return ConfigurationManager.AppSettings["ldap"]; }
    }

    public static string DocumentumApiUrl
    {
        get { return ConfigurationManager.AppSettings["file-rest-service"]; }
    }

    public static bool SendToEmails
    {
        get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send-emails"]); }
    }

    public static bool SendToSms
    {
        get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send-sms"]); }
    }

    public static bool SendToEfiling
    {
        get { return Convert.ToBoolean(ConfigurationManager.AppSettings["send-elf"]); }
    }

    public static string AcceptanceLetterTemplate
    {
        get { return ConfigurationManager.AppSettings["Acceptance-Letter"] ?? string.Empty; }
    }

    public static string RejectionLetterTemplate
    {
        get { return ConfigurationManager.AppSettings["Rejection-Letter"] ?? string.Empty; }
    }

    public static int QueueResponseTime
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["queue-response-time"]); }
    }

    public static string QueueTimeoutMessage
    {
        get { return ConfigurationManager.AppSettings["queue-timeout-message"] ?? string.Empty; }
    }


    public static string DocumentumUserName
    {
        get
        {
            return Sars.Systems.Security.SecureConfig.DecryptString(ConfigurationManager.AppSettings["DCTM-UID"],
                "P@ssw0rd");
        }
    }

    public static string DocumentumPassword
    {
        get
        {
            return Sars.Systems.Security.SecureConfig.DecryptString(ConfigurationManager.AppSettings["DCTM-PWD"],
                "P@ssw0rd");
        }
    }

    public static string CbCStatusValidationServiceID
    {
        get { return ConfigurationManager.AppSettings["CBC-OECD-STATUS-VALIDATION-SERVICE-ID"]; }
    }

    public static string CBCREPORTMGT_CHANNELID
    {
        get { return ConfigurationManager.AppSettings["CBCREPORTMGT.RES-CHANNELID"]; }
    }

    public static string RECEIVECBCREPORT_CHANNELID
    {
        get { return ConfigurationManager.AppSettings["RECEIVECBCREPORT.RES-CHANNELID"]; }
    }


    public static string UPDATECBCREPORTSTATUS_CHANNELID
    {
        get { return ConfigurationManager.AppSettings["UPDATECBCREPORTSTATUS.REQ-CHANNELID"]; }
    }


    public static string CbCValidationServiceID
    {
        get { return ConfigurationManager.AppSettings["CbC-Validation-Service-ID"]; }
    }

    public static string CountryByCountryReportManagementValidationServiceID
    {
        get { return ConfigurationManager.AppSettings["CountryByCountryReportManagement-Validation-Service-ID"]; }
    }

    public static string CTSSenderFileMetadata
    {
        get { return ConfigurationManager.AppSettings["CTSSenderFileMetadata-Validation-Service-ID"]; }
    }

    public static CBCEnvironment CBCEnvironment
    {
        get
        {
            var env = CurrentEnvironment.ToUpper();
            switch (env)
            {
                case "QA":
                    return CBCEnvironment.QA;
                case "PREPROD":
                    return CBCEnvironment.PREPROD;
                case "DEV":
                    return CBCEnvironment.DEV;
                case "PRODUCTION":
                    return CBCEnvironment.PRODUCTION;
                default:
                    return CBCEnvironment.DEV;
            }
        }
    }

    public static string MessagePath
    {
        get { return System.Configuration.ConfigurationManager.AppSettings["X-CBC-Message-Path"]; }
    }

    public static string FromAddress
    {
        get { return System.Configuration.ConfigurationManager.AppSettings["from-email"]; }
    }

    public static string EmailTemplateBaseFolder
    {
        get
        {
            if (HttpContext.Current.Request != null)
            {
                var path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "letters");
                return path;
            }

            return string.Empty;
        }
    }

    public static string[] CountriesUsingHub
    {
        get
        {
            var countries = System.Configuration.ConfigurationManager.AppSettings["Countrie-using-hub"];
            if (string.IsNullOrEmpty(countries))
            {
                return null;
            }
            return countries.Split("|".ToCharArray());
        }
    }
}

public enum CBCEnvironment
{
    QA,
    PREPROD,
    DEV,
    PRODUCTION
}