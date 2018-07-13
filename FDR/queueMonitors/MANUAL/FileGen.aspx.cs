using System;
using System.Data;
using System.IO;
using Sars.Systems.Data;
using Sars.Systems.Utilities;
public partial class queueMonitors_MANUAL_FileGen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var _sql = @"
            SELECT [Message] FROM [MessagingManager].[dbo].[IncomingMessages]  
where FORMAT( [Timestamp], 'yyyyMMdd') = '20180626' and QueueName='FDR.RECEIVECBCREPORT.REQ'";
        var recordSet = new RecordSet(_sql, QueryType.TransectSQL, null);

        if (recordSet.HasRows)
        {
            var folder = @"D:\saRS\prEPROCESSCBC";
            var i = 0;
            foreach (DataRow row in recordSet.Tables[0].Rows)
            {
                i++;
                var fileName = string.Format("{0}_{1}.CBCIN", i, DateTime.Now.ToFileTime());
                var fullFileName = Path.Combine(folder, fileName);

                var content = (byte[]) row["Message"];
                var base64String = Convert.ToBase64String(content);
                File.WriteAllText(fullFileName, base64String.Base64ToString());
            }
        }
    }
}