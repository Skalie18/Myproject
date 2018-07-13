<%@ WebHandler Language="C#" Class="ViewDocument" %>
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using FDR.DataLayer;
using Sars.Systems.Data;

public class ViewDocument : IHttpHandler{
    public void ProcessRequest( HttpContext context ){
        var fileData = GetFile(context.Request["oId"], context);
        if (fileData != null){
            if (fileData.Length > 0)
            {
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-length", fileData.Length.ToString(CultureInfo.InvariantCulture));
                context.Response.OutputStream.Write(fileData, 0, fileData.Length);
                context.Response.End();
            }
        }
    }
    public byte[] GetFile(string objectId, HttpContext context)
    {
        if (context.Request.PhysicalApplicationPath == null){
            return  null;
        }
        var universalUniqueID = Guid.NewGuid().ToString();
        var messageSeqNo = DBReadManager.GetNextMessageId(universalUniqueID);
        var messageBuilder = new StringBuilder();
        var message = File.ReadAllText(Path.Combine(context.Request.PhysicalApplicationPath, "GetFile.xml"));
        messageBuilder.AppendFormat
            (
                  message
                , messageSeqNo
                , DateTime.Now.ToString("s")
                , context.Request["refNo"]
                , universalUniqueID
                , Configurations.DocumentumUserName
                , Configurations.DocumentumPassword
                , objectId
            );
        var myReq = WebRequest.Create(Configurations.DocumentumApiUrl) as HttpWebRequest;
        if (myReq != null){
            myReq.Method = "POST";
            myReq.ContentType = "text/xml";
            myReq.Accept = "text/xml";
            var arr = Encoding.ASCII.GetBytes(messageBuilder.ToString());
            using (var streamWriter = myReq.GetRequestStream()){
                streamWriter.Write(arr, 0, arr.Length);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse) myReq.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())){
                var results = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(results)){
                    //var xmlNode = FdrCommon.RemoveAllNamespaces(doc);
                    const string _namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8";
                    var element = FdrCommon.SelectNode(results, "NS115", _namespace, "ContentManagementResponse");// xmlNode.SelectSingleNode("//ContentManagementResponse");
                    if (element == null)
                    {
                        return null;
                    }
                    var x = element.ChildNodes[0].InnerXml;
                    using (var data = new RecordSet()){
                        data.ReadXml(new StringReader(x));
                        if (data.HasRows && data.Tables["Content"] != null && data.Tables["Content"].Columns.Contains("BinaryData")){
                            return Convert.FromBase64String(data.Tables["Content"].Rows[0]["BinaryData"].ToString());
                        }
                    }
                    //using (var data = new RecordSet()){
                    //    data.ReadXml(new StringReader(results));
                    //    if (data.HasRows && data.Tables["Content"] != null){
                    //        return Convert.FromBase64String(data.Tables["Content"].Rows[0]["BinaryData"].ToString());
                    //    }
                    //}
                }
                return null;
            }
        }
        return null;
    }
        
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}