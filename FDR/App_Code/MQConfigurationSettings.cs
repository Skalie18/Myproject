using System;
using System.Configuration;


public static class MQConfigurationSettings
{
    public static string CorrespondenceInQManagerName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-IN-QManager"]; }
    }

    public static string CorrespondenceInQChannelName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-IN-QChannel"]; }
    }

    public static string CorrespondenceInHostName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-IN-QHost"]; }
    }

    public static string CorrespondenceInQName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-IN-QName"]; }
    }

    public static int CorrespondenceInPortNumber
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["CORRESPONDENCE-IN-Port"]); }
    }

    public static string CorrespondenceOutQManagerName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-QManager"]; }
    }

    public static string CorrespondenceOutQChannelName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-QChannel"]; }
    }

    public static string CorrespondenceOutHostName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-QHost"]; }
    }

    public static string CorrespondenceOutQName
    {
        get { return ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-QName"]; }
    }

    public static int CorrespondenceOutPortNumber
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-Port"]); }
    }

    public static int JmsIbmEncoding
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["JmsIbmEncoding"]); }
    }



#region REGISTRATION

    public static string RegistrationOutQManagerName
    {
        get { return ConfigurationManager.AppSettings["REGISTRATION-OUT-QManager"]; }
    }

    public static string RegistrationOutQChannelName
    {
        get { return ConfigurationManager.AppSettings["REGISTRATION-OUT-QChannel"]; }
    }

    public static string RegistrationOutHostName
    {
        get { return ConfigurationManager.AppSettings["REGISTRATION-OUT-QHost"]; }
    }

    public static string RegistrationOutQName
    {
        get { return ConfigurationManager.AppSettings["REGISTRATION-OUT-QName"]; }
    }

    public static int RegistrationOutPortNumber
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["CORRESPONDENCE-OUT-Port"]); }
    }

#endregion

 



    public static int JmsIbmCharacterSet
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["JmsIbmCharacterSet"]); }
    }

    public static int DeliveryMode
    {
        get { return Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryMode"]); }
    }

    public static string WmqProviderVersion
    {
        get { return ConfigurationManager.AppSettings["WmqProviderVersion"]; }
    }
}