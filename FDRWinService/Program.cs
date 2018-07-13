using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FDRWinService
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
            {
                new FinancialDataReportingService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
