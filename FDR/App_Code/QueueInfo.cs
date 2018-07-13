using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class QueueInfo
    {
        public QueueInfo(string hostServerName, string channel, string managerName, int portNumber, string queueName)
        {
            HostName = hostServerName;
            Manager = managerName;
            Channel = channel;
            Port = portNumber;
            UseManagerName = string.IsNullOrEmpty(Manager);
            QueueName = queueName;
        }
        public QueueInfo(){}
        public string HostName { get; set; }
        public string Manager { get; set; }
        public string Channel { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }

        public string ConnectionInfo
        {
            get
            {
                if (Port < 0 || string.IsNullOrEmpty(HostName))
                    return string.Empty;
                return string.Format("{0}({1})", HostName, Port);
            }
        }

        public string CorrelationId { get; set; }
        public string Message { get; set; }
        public bool UseManagerName { get; set; }

        public string NoManager
        {
            get
            {
                if (UseManagerName && string.IsNullOrEmpty(Manager))
                {
                    throw new Exception("There is manager supplied ");
                }
                return UseManagerName ? Manager : string.Empty;
            }
        }

        public string QueueConnection
        {
            get
            {
               return  string.Format("queue://{0}/{1}", NoManager, QueueName);
            }
        }
    }
