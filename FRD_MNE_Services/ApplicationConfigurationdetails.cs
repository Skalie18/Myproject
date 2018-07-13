using System;
using System.Configuration;

namespace FRD_MNE_Services
{
    public static class CBCApplicationConfigurationdetails
    {
        public static string RequestManger { get { return ConfigurationManager.AppSettings["CBC-IN-QManager"]; } }
        public static string RequestChannel { get { return ConfigurationManager.AppSettings["CBC-IN-QChannel"]; } }
        public static string RequestHostName { get { return ConfigurationManager.AppSettings["CBC-IN-QHost"]; } }
        public static int RequestPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CBC-IN-Port"]); } }
        public static string RequestQueueName { get { return ConfigurationManager.AppSettings["CBC-IN-QName"]; } }
        public static string RequestConnection { get { return string.Format("{0}({1})", RequestHostName, RequestPort); } }

        public static string ResponseManger { get { return ConfigurationManager.AppSettings["CBC-OUT-QManager"]; } }
        public static string ResponseChannel { get { return ConfigurationManager.AppSettings["CBC-OUT-QChannel"]; } }
        public static string ResponseHostName { get { return ConfigurationManager.AppSettings["CBC-OUT-QHost"]; } }
        public static int ResponsePort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CBC-OUT-Port"]); } }
        public static string ResponseQueueName { get { return ConfigurationManager.AppSettings["CBC-OUT-QName"]; } }
    }
    public static class MNEApplicationConfigurationdetails
    {
        public static string RequestManger { get { return ConfigurationManager.AppSettings["MNE-IN-QManager"]; } }
        public static string RequestChannel { get { return ConfigurationManager.AppSettings["MNE-IN-QChannel"]; } }
        public static string RequestHostName { get { return ConfigurationManager.AppSettings["MNE-IN-QHost"]; } }
        public static int RequestPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["MNE-IN-Port"]); } }
        public static string RequestQueueName { get { return ConfigurationManager.AppSettings["MNE-IN-QName"]; } }
        public static string RequestConnection { get { return string.Format("{0}({1})", RequestHostName, RequestPort); } }
    

        public static string ResponseManger { get { return ConfigurationManager.AppSettings["MNE-OUT-QManager"]; } }
        public static string ResponseChannel { get { return ConfigurationManager.AppSettings["MNE-OUT-QChannel"]; } }
        public static string ResponseHostName { get { return ConfigurationManager.AppSettings["MNE-OUT-QHost"]; } }
        public static int ResponsePort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["MNE-OUT-Port"]); } }
        public static string ResponseQueueName { get { return ConfigurationManager.AppSettings["MNE-OUT-QName"]; } }
    }
    public static class LMApplicationConfigurationdetails
    {
        public static string RequestManger { get { return ConfigurationManager.AppSettings["LM-IN-QManager"]; } }
        public static string RequestChannel { get { return ConfigurationManager.AppSettings["LM-IN-QChannel"]; } }
        public static string RequestHostName { get { return ConfigurationManager.AppSettings["LM-IN-QHost"]; } }
        public static int RequestPort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["LM-IN-Port"]); } }
        public static string RequestQueueName { get { return ConfigurationManager.AppSettings["LM-IN-QName"]; } }
        public static string RequestConnection { get { return string.Format("{0}({1})", RequestHostName, RequestPort); } }

        public static string ResponseManger { get { return ConfigurationManager.AppSettings["LM-OUT-QManager"]; } }
        public static string ResponseChannel { get { return ConfigurationManager.AppSettings["LM-OUT-QChannel"]; } }
        public static string ResponseHostName { get { return ConfigurationManager.AppSettings["LM-OUT-QHost"]; } }
        public static int ResponsePort { get { return Convert.ToInt32(ConfigurationManager.AppSettings["LM-OUT-Port"]); } }
        public static string ResponseQueueName { get { return ConfigurationManager.AppSettings["LM-OUT-QName"]; } }
    }
    public static class AppConfig
    {
        public static string NotifyRole { get { return ConfigurationManager.AppSettings["notify-role"]; } }
        public static string FromAddress { get { return ConfigurationManager.AppSettings["from-email"]; } }
        public static int FileRevisionCount { get { return Convert.ToInt32(ConfigurationManager.AppSettings["Num-Files-Revisions"]); } }
        public static int CbcRevisionCount { get { return Convert.ToInt32(ConfigurationManager.AppSettings["Num-CBC-Revisions"]); } }
        public static string SecondaryFileLocation{get
        {
            return ConfigurationManager.AppSettings["Secondary-File-Location"].ToUpper();
        }}

        public static string SchemBaseFolder
        {
            get { return ConfigurationManager.AppSettings["schema-base-folder"]; }
        }
    }
}
