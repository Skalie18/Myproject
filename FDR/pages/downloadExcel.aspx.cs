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
using Microsoft.Office.Interop.Excel;
using System.Collections;

public partial class pages_downloadXml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["mspecId"] == null && Request.QueryString["country"] == null)
            {
                return;
            }
            var mspecId = decimal.Parse(Request.QueryString["mspecId"].ToString());
            downLoadExcel(mspecId, Request.QueryString["country"].ToString());
        }
    }

    private void downLoadExcel(decimal mspecId, string country)
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
        }
    }

    public void addCustomXMLPart(string test)
    {

        //Microsoft.Office.Interop.Excel.Application APexcel = null;
        //Microsoft.Office.Interop.Excel.Workbook MyBook = null;
        //Microsoft.Office.Interop.Excel.Worksheet MySheet = null;
        //try
        //{
        //    APexcel = new Microsoft.Office.Interop.Excel.Application();
        //    APexcel.Visible = false;
        //    MyBook = APexcel.Workbooks.Open("C:\\example.xml", Type.Missing, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    MySheet = (Microsoft.Office.Interop.Excel.Worksheet)MyBook.ActiveSheet;
        //    //Libro.SaveAs("c:\\example.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
        //}
        //catch (Exception ex)
        //{
        //    string Error = ex.ToString();
        //}
    }

}