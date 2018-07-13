using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
using System.IO;
using System.Xml;
using Sars.Systems.Utilities;
using System.Data;
using System.Text;
using Sars.Systems.Extensions;
using Sars.Systems.Data;
public partial class pages_downloadXml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["mspecId"] == null &&  Request.QueryString["country"]==null)
            {
                return;
            }
            var mspecId = decimal.Parse(Request.QueryString["mspecId"].ToString());
            downLoadXml(mspecId, Request.QueryString["country"].ToString());
        }
    }

    private void downLoadXml(decimal mspecId, string country)
    {
        var dsData = DBReadManager.GetIncomingForeignXml(mspecId);


        if (dsData != null)
        {
            var doc = new XmlDocument();
            var messageSpecDoc = new XmlDocument();

            var oecd = doc.CreateNode(XmlNodeType.Element, "CBC_OECD", null);
            var messagespec = doc.CreateNode(XmlNodeType.Element, "MessageSpec", null);
            messageSpecDoc.LoadXml(dsData.Tables[0].Rows[0][1].ToString());
            var msgSpecification = messageSpecDoc.SelectSingleNode("MessageSpec_Type");
            messagespec.InnerXml = msgSpecification.InnerXml;
            oecd.AppendChild(messagespec);

           /* var oecd = newxmlDoc.CreateNode(XmlNodeType.Element, "CBC_OECD", null);
            var messageSpec = newxmlDoc.CreateNode(XmlNodeType.Element, "MessageSpec", null);
            var msgSpecification = mspecDoc.SelectSingleNode("MessageSpec_Type");
            messageSpec.InnerXml = msgSpecification.InnerXml;
            oecd.AppendChild(messageSpec);*/
            foreach (DataRow row in dsData.Tables[0].Rows)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(row[0].ToString());
                var cbcBody = doc.CreateNode(XmlNodeType.Element, "CbcBody", null);
                var cbcReport = xmlDoc.SelectSingleNode("CbcBody_Type");
                cbcBody.InnerXml = cbcReport.InnerXml;
                oecd.AppendChild(cbcBody);
            }
            doc.AppendChild(oecd);
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

                doc.WriteTo(writer);
                writer.Flush();
                Response.Clear();
                byte[] byteArray = stream.ToArray();
                Response.AppendHeader("Content-Disposition", "attachment;" + string.Format("filename ={0}.xml", country));
                Response.AppendHeader("Content-Length", byteArray.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(byteArray);
                Response.Flush();
                Response.End();
                writer.Close();
            }

        }
    }
}