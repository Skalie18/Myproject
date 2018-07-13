using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Xml;

namespace PushCBCData.BO
{
    public class AppConfigs
    {
        public static int Duration
        {
            get
            {
                if (ConfigurationManager.AppSettings["Duration"] != null)
                    return int.Parse(ConfigurationManager.AppSettings["Duration"].ToString());
                throw new System.ArgumentNullException();

            }
        }

        public static bool AlwaysRun
        {
            get
            {
                if (ConfigurationManager.AppSettings["AlwaysRun"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["AlwaysRun"].ToString());
                throw new System.ArgumentNullException();

            }
        }

        public static string FilePath
        {
            get
            {
                if (ConfigurationManager.AppSettings["filePath"] != null)
                    return ConfigurationManager.AppSettings["filePath"].ToString();
                else
                    throw new System.NullReferenceException();
            }
        }

        public static string ConnextionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["FDRConnection"] != null)
                    return ConfigurationManager.ConnectionStrings["FDRConnection"].ToString();
                else
                    throw new System.NullReferenceException();
            }
        }

        public static void UpdateAppconfig()
        {
            try
            {

                var date = DateTime.Now;
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                var found = false;
                foreach (XmlElement element in xmlDoc.DocumentElement)
                {
                    if (element.Name.Equals("appSettings"))
                    {
                        foreach (XmlNode node in element.ChildNodes)
                        {
                            if (node.Attributes[0].Value.Equals("lastRun"))
                            {
                                node.Attributes[1].Value = date.ToString("yyyy-MM-dd");
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        EventLogging.LogInformation("Updated Last Run");
                        xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                        ConfigurationManager.RefreshSection("appSettings");
                        break;
                    }
                }
                

            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : UpdateAppconfig");
            }
        }

        public static DateTime LastRun
        {
            get
            {
                if (ConfigurationManager.AppSettings["lastRun"] != null)
                    return DateTime.Parse(ConfigurationManager.AppSettings["lastRun"].ToString());
                else
                    throw new System.NullReferenceException();
            }
        }
    }
}
