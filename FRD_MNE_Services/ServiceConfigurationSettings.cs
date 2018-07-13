using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FRD_MNE_Services
{
    public static class ServiceConfigurationSettings
    {
        public static string CurrentEnvironment
        {
            get { return ConfigurationManager.AppSettings["Current-Environment"]; }
        }
    }
}
