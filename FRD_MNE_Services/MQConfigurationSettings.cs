using System;
using System.Configuration;

namespace FRD_MNE_Services
{
    public static class MQConfigurationSettings
    {
        public static string MNE_Equire_IN_QManagerName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-IN-QManager"]; }
        }

        public static string MNE_Equire_IN_QChannelName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-IN-QChannel"]; }
        }

        public static string MNE_Equire_IN_QConnection
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-IN-QConnection"]; }
        }

        public static string MNE_Equire_IN_QName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-IN-QName"]; }
        }

        public static int MNE_Equire_IN_PortNumber
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["mne-enquire-IN-Port"]); }
        }

        public static string MNE_Equire_OUT_QManagerName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-OUT-QManager"]; }
        }

        public static string MNE_Equire_OUT_QChannelName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-OUT-QChannel"]; }
        }

        public static string MNE_Equire_OUT_QConnection
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-OUT-QConnection"]; }
        }

        public static string MNE_Equire_OUT_QName
        {
            get { return ConfigurationManager.AppSettings["mne-enquire-OUT-QName"]; }
        }

        public static int MNE_Equire_OUT_PortNumber
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["mne-enquire-OUT-Port"]); }
        }

        public static int JMS_IBM_ENCODING
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["JMS_IBM_ENCODING"]); }
        }

        public static int JMS_IBM_CHARACTER_SET
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["JMS_IBM_CHARACTER_SET"]); }
        }

        public static int DELIVERY_MODE
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["DELIVERY_MODE"]); }
        }

        public static string WMQ_PROVIDER_VERSION
        {
            get { return ConfigurationManager.AppSettings["WMQ_PROVIDER_VERSION"]; }
        }

        public static string CBC_Declaration_IN_QName
        {
            get { return ConfigurationManager.AppSettings["CBC-IN-QName"]; }
        }
        public static string CBC_Declaration_OUT_QName
        {
            get { return ConfigurationManager.AppSettings["CBC-OUT-QName"]; }
        }
        public static string MasterLocalFile_IN_QName
        {
            get { return ConfigurationManager.AppSettings["master-local-IN-QName"]; }

        }

        public static string MasterLocalFile_OUT_QName
        {
            get { return ConfigurationManager.AppSettings["master-local-OUT-QName"]; }

        }
    }
}
