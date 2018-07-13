using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using PushCBCData.BO;

namespace PushCBCData
{
    public partial class PushService : ServiceBase
    {
        private Timer timer;
        public PushService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(AppConfigs.Duration);
            EventLogging.LogInformation("PushCBCData Service Started");
            try
            {
                if (AppConfigs.AlwaysRun)
                {
                    timer.Elapsed += new ElapsedEventHandler(RunProcess);
                    timer.Enabled = true;
                    timer.AutoReset = true;
                    timer.Interval = AppConfigs.Duration;    //set interval to 1minutes equivalent to 60000 milliseconds
                    timer.Start();
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : OnStart");
            }
        }

        protected override void OnStop()
        {
            EventLogging.LogInformation("PushCBCData Service Stopped");
            timer.Enabled = false;
        }

        protected void RunProcess(object sender, ElapsedEventArgs e)
        {
            try
            {
                var currentDate = DateTime.Now;
                var date = currentDate.ToString("yyyy-MM-dd");
                ProcessData.GetData();
                currentDate = DateTime.Parse(date);
                if (AppConfigs.LastRun < currentDate)
                {
                    UploadFile.ProcessFile();
                    AppConfigs.UpdateAppconfig();
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                                "Source : " + x.Source + "\n" +
                                "Event : RunProcess");
            }
        }

        public void StartProcess(){

            var currentDate = DateTime.Now;
            //ProcessData.GetData();
            var date = currentDate.ToString("yyyy-MM-dd");
            currentDate = DateTime.Parse(date);
            if (AppConfigs.LastRun < currentDate)
            {
               // UploadFile.ProcessFile();
                AppConfigs.UpdateAppconfig();
            }
        }

    }
}
