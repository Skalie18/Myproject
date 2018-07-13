using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using FDR.DataLayer;
using FDRService.CbcStatusMessage;
using Sars.Systems.Data;
using Sars.Systems.Messaging;
using Sars.Systems.Utilities;

public partial class queueMonitors_FDR_UPDATECBCREPORTSTATUS_RES : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var incomingMessage = new IncomingMessage(HttpContext.Current);
        //var data = Request["_text"].Base64ToString();
        //var _queue = Request["_queue"].Base64ToString();
        //var _manager = Request["_manager"].Base64ToString();
        //var _host = Request["_host"].Base64ToString();
        //var _messageID = Request["_messageID"].Base64ToString();
        //var _correlationID = Request["_correlationID"].Base64ToString();


        //var data = incomingMessage.Message;
        //var _queue = incomingMessage.QueuName;
        //var _manager = incomingMessage.ManagerName;
        //var _host = incomingMessage.Host;
        //var _messageID = incomingMessage.MessageId;
        //var _correlationID = incomingMessage.MessageCorrelationId;

        var message = incomingMessage.Message;
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        message = message.Replace("ns0:", "esb:");

        var dataset = new RecordSet();
        dataset.ReadXml(new StringReader(message));

        if (dataset.HasRows)
        {
            if (dataset.Tables.Contains("MessageIdentification") &&
                dataset.Tables.Contains("ApplicationInformationResult"))
            {
                var messageIdentification = dataset.Tables["MessageIdentification"];
                var applicationInformationResult = dataset.Tables["ApplicationInformationResult"];

                Guid universalUniqueId;
                if (Guid.TryParse(messageIdentification.Rows[0]["universalUniqueID"].ToString(), out universalUniqueId))
                {
                    FDR.DataLayer.DBWriteManager.Insert_OutgoingPackageAuditTrail
                        (
                            universalUniqueId,
                            null,
                            string.Format("Queue({0}) - {1}", incomingMessage.QueuName,
                                applicationInformationResult.Rows[0]["Description"])
                        );

                }
            }
        }
    }

}