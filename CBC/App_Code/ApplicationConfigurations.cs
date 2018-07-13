using System;

/// <summary>
/// Summary description for ApplicationConfigurations
/// </summary>
public static class ApplicationConfigurations
{
    public static short MasterFileDefaultNumberOrItems
    {
        get { return Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["master-file-default-number"]) ; }
    }
    public static short LocalFileDefaultNumberOrItems
    {
        get { return Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["local-file-default-number"]); }
    }

    

    public static bool DevTesting
    {
        get { return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["dev-testing"]); }
    }
    public static bool QATesting
    {
        get { return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["QA-testing"]); }
    }
    public static string LogonURL { get { return string.Concat( System.Configuration.ConfigurationManager.AppSettings["logonURL"], "&ProcessCode=Login&ActionCode=Load"); } }
}