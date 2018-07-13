using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace PushCBCData.BO
{
    public class EventLogging
    {
        private const string _source = "pushCBCDataSource";
        private const string _log = "pushCBCDataLog";
        public static void LogInformation(string message)
        {
            LogNotifyServiceEvent(message, EventLogEntryType.Information);
        }

        public static void LogWarning(string message)
        {
            LogNotifyServiceEvent(message, EventLogEntryType.Warning);
        }

        public static void LogError(string message)
        {
            LogNotifyServiceEvent(message, EventLogEntryType.Error);
        }

        private static void LogNotifyServiceEvent(string message, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(_source))
            {
                EventLog.CreateEventSource(_source, _log);
            }
            var eLog = new EventLog { Source = _source };
            eLog.WriteEntry(message, type);
        }
    }
}
