using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sars.Models.CBC
{
    public class MessageObject
    {
        public string Content { get; set; }
        public string QueuName { get; set; }
        public string ManagerName { get; set; }
        public string Host { get; set; }
        public string MessageId { get; set; }
        public string MessageCorrelationId { get; set; }
        public bool Processed { get; set; }
    }
}
