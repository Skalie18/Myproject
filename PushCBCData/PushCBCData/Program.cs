using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PushCBCData
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
				new PushService() 
			};
            ServiceBase.Run(ServicesToRun);


            /*PushCBCData.PushService pushService = new PushCBCData.PushService();
            pushService.StartProcess();*/
        }
    }
}
