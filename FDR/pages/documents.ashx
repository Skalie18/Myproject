<%@ WebHandler Language="C#" Class="documents" %>
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;

public class documents : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        var fileData = (context.Request["oId"]);
        if (fileData != null)
        {

            var url = System.Configuration.ConfigurationManager.AppSettings["document-service-url-get"];
            var myReq = WebRequest.Create(string.Format(url, fileData)) as HttpWebRequest;
            myReq.Method = "GET";
            var httpResponse = (HttpWebResponse)myReq.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var x = JsonConvert.DeserializeObject<DocumentResponse>(result.Replace("[", "").Replace("]", ""));
                var base64 = x.content;
                var buffer = Convert.FromBase64String(base64);
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-length", buffer.Length.ToString(CultureInfo.InvariantCulture));
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.End();
            }
        }
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    public class DocumentResponse
    {
        // public JArray properties { get; set; }
        public string contentType { get; set; }
        public string objectName { get; set; }
        public string content { get; set; }
        public string objectType { get; set; }
        //public string contentUrl { get; set; }
        public string author { get; set; }
        public string objectId { get; set; }

    }
}