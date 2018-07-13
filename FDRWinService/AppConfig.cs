using System;
using System.Configuration;
using System.Net;
using Sars.Systems.Security;

namespace FDRWinService
{
    public static class AppConfig
    {
        public static string RequestsPath
        {
            get { return ConfigurationManager.AppSettings["requests-location"]; }
        }

        public static bool Testing
        {
            get
            {
                return Convert.ToBoolean(
                    Convert.ToInt16(ConfigurationManager.AppSettings["testing"])
                );
            }
        }

        public static bool RemoveFilesWithErrors
        {
            get
            {
                return Convert.ToBoolean(
                    Convert.ToInt16(ConfigurationManager.AppSettings["remove-files-with-errors"])
                    );
            }
        }
   public static bool ReprocessDBFilesForPendingDirectives
        {
            get
            {
                return Convert.ToBoolean(
                    Convert.ToInt16(ConfigurationManager.AppSettings["reprocess-db-files-for-pending-dirs"])
                    );
            }
        }


        public static bool ProcessCancellations
        {
            get
            {
                return Convert.ToBoolean(
                    ConfigurationManager.AppSettings["process-cancellations"]
                );
            }
        }

        public static string ExternalSystemId
        {
            get { return ConfigurationManager.AppSettings["EXT-SYS"]; }
        }

        public static string InfoSubType
        {
            get { return ConfigurationManager.AppSettings["INFO-SUBTYPE"]; }
        }

        public static string GetInterfaceVersionNumber(string form)
        {
            return ConfigurationManager.AppSettings[form];
        }

        public static string Response_File_Location
        {
            get { return ConfigurationManager.AppSettings["Response-file-location"]; }
        }

        public static string Response_Error_File_Location
        {
            get { return ConfigurationManager.AppSettings["error-file-dir"]; }
        }

        public static string Processed_Response_File_Location
        {
            get { return ConfigurationManager.AppSettings["Processed-Processed-Directive-Response-file-location"]; }
        }

        public static string Validation_file_location
        {
            get { return ConfigurationManager.AppSettings["Response-file-location"]; }
        }

        public static string Processed_Validation_Response_File_Location
        {
            get { return ConfigurationManager.AppSettings["Processed-response-file-location"]; }
        }

        public static int NumberOfCharactersInFileSequencing
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["num-chars-file-sequence"]); }
        }

        public static short NumRecordsPerRequestFile(string formName)
        {
            switch (formName)
            {
                case "FORMAD":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-AD"]);
                case "FORMB":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-B"]);
                case "FORMC":                                                                          
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-C"]);
                case "FORME":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-E"]);
                case "ROT01":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-IRP3A"]);
                case "ROT02":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-IRP3S"]);
                case "IRP3A":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-ROT01"]);
                case "IRP3S":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-ROT02"]);
                case "IRP3CRQ":
                    return Convert.ToInt16(ConfigurationManager.AppSettings["num-records-per-request-file-IRP3CRQ"]);
                default:
                    return 2000;
            } 
        }

        public static ICredentials NHECredentials
        {
            get
            {
                return
                    new NetworkCredential(
                        SecureConfig.DecryptString(ConfigurationManager.AppSettings["ehn-acc-name"], "P@ssw0rd"),
                        SecureConfig.DecryptString(ConfigurationManager.AppSettings["ehn-acc-pin"], "P@ssw0rd"),
                        ConfigurationManager.AppSettings["ehn-forest"]);
            }
        }

        public static int RequestDueTime
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["request-service-start"]); }
        }

        public static int RequestPeriod
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["request-service-period"]); }
        }


        public static int ResponseDueTime
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["response-service-start"]); }
        }

        public static int ResponsePeriod
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["response-service-period"]); }
        }



        public static string ValidationFileName
        {
            get { return ConfigurationManager.AppSettings["validation-files-name"]; }
        }
        public static string ResponseFileName
        {
            get { return ConfigurationManager.AppSettings["response-files-name"]; }
        }
    }
}
