using System.ServiceProcess;

namespace FRD_MNE_Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new FDRMultiNationalEnterpriseDataService() 
			};
            ServiceBase.Run(ServicesToRun);           
        }

    }
}
