using System;
using System.Collections;
using System.ServiceProcess;
using System.Threading;
//using IBM.WMQ;
//using SARSMQ_Util;
using FDR.DataLayer;
namespace FDRWinService
{
    partial class FinancialDataReportingService : ServiceBase
    {
        private Timer oFDRPollingTimer;
        private Timer oMneEnquireTimer;
        public FinancialDataReportingService()
        {
            InitializeComponent();
            Enquire(null);
            GetSupportingDocs();
        }

        protected override void OnStart(string[] args)
        {
            var oCbcReportTimerDelegate = new TimerCallback(PollingTimerTick);
            oFDRPollingTimer = new Timer(oCbcReportTimerDelegate, null, AppConfig.RequestDueTime,
                AppConfig.RequestPeriod);


            var enquireTimeDelegate = new TimerCallback(Enquire);
            oMneEnquireTimer = new Timer(Enquire, null, AppConfig.RequestDueTime, AppConfig.RequestPeriod);
        }

        private void Enquire(object state)
        {
            //ReaqEnquireQueue();
            //var x = new FDRWebService {UseDefaultCredentials = true};
            var equireResults = DBReadManager.EnquireMne("3198510568");

            var xml =
                Sars.Systems.Serialization.XmlObjectSerializer.GetXml(equireResults, true);

            //SendResutstoEFL();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        private static void PollingTimerTick(object state)
        {
            //try
            //{
            //    var serviceSettings = Db.GetServiceSettings();
            //    if ( serviceSettings.InterceptResponses )
            //    {
            //        InterceptValidations();
            //        InterceptDirectives();
            //        InterceptCancellations();
            //        InterceptRotResponses();
            //    }
            //}
            //catch ( Exception e )
            //{
            //    LogEvent(string.Format("Error : {0} - log : {0}", e));
            //}
            //var oProcessRequestsDelegate = new ProcessRequestsDelegate(ProcessRequests);
            //var results = oProcessRequestsDelegate.BeginInvoke(null, null);
            //oProcessRequestsDelegate.EndInvoke(results);
        }


        private static void GetSupportingDocs()
        {
            //var connectionDetails = new MQConnectionDetail
            //{
            //    //Channel = ConfigurationManager.AppSettings["MQChannel"],
            //    //HostName = ConfigurationManager.AppSettings["MQHostName"],
            //    //Port = Convert.ToInt16(ConfigurationManager.AppSettings["MQPort"]),
            //    //QueueManager = ConfigurationManager.AppSettings["MQQueueManager"],
            //    //RequestQueue = ConfigurationManager.AppSettings["MQRequestQueue"],
            //    //ResponseQueue = ConfigurationManager.AppSettings["MQResponseQueue"]

            //    Channel = "FDR.SVRCONN",
            //    HostName = "10.30.6.152",
            //    Port = 1412,
            //    QueueManager = "QMGEN",
            //    RequestQueue = "FDR.NOTIFYSUPPDOCSFDR.REQ",
            //    ResponseQueue = "FDR.NOTIFYSUPPDOCSFDR.RES"
            //};

            //try
            //{
            //    var hashtable = new Hashtable
            //    {
            //        {"channel", connectionDetails.Channel},
            //        {"hostname", connectionDetails.HostName},
            //        {"port", connectionDetails.Port},
            //        {"transport", "MQSeries Managed Client"}
            //    };
            //    byte[] array = new byte[0];
            //    var manager = new MQQueueManager(connectionDetails.QueueManager, hashtable);
            //    var queue = manager.AccessQueue(connectionDetails.RequestQueue, 8192);
            //    var mqMessage = new MQMessage();
            //    var mQGetMessageOptions = new MQGetMessageOptions {Options = MQC.MQGMO_WAIT, MatchOptions = 2};
            //    queue.Get(mqMessage, mQGetMessageOptions);
            //    var reusult = mqMessage.ReadString(mqMessage.MessageLength);
            //    array = mqMessage.MessageId;
            //    var correlationId = array;
            //    queue.Close();


            //    //TODO: Process results
            //}
            //catch (MQException exception)
            //{
            //    Console.Write(exception.ToString());
            //}
        }
    }
}
