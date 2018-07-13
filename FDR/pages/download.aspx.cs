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
public partial class pages_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["xCountry"] != null && Request.QueryString["fname"] != null)
            {
                var countryCode = Request.QueryString["xCountry"].ToString();
                string[] reqString = Request.QueryString["fname"].ToString().Split(',');
                string  period = Request.QueryString["period"].ToString();
                if (reqString.Length > 3)
                {
                    var fileName = reqString[0];
                    var isOutgoing = reqString[2];
                    var fileType = reqString[3];
                    LoadForm(countryCode, fileName, isOutgoing, fileType, period);
                }
                else
                {
                    var fileName = reqString[0];
                    var isOutgoing = reqString[1];
                    var fileType = reqString[2];
                    LoadForm(countryCode, fileName, isOutgoing, fileType, period);
                }
            }
        }
    }

    private void LoadForm(string countryCode, string fileName, string generationType, string fileType, string period)
    {
        DataSet dsData = new DataSet();


        if (fileType.Trim() != "xml")
        {
            dsData = DBReadManager.GeneratedFileDownload(countryCode, period, Convert.ToBoolean(generationType));
            if (dsData != null)
            {
                gvCBC.Bind(dsData);
                ExportToExcel(fileName, period);
            }
        }
        else
        {
            if (Convert.ToBoolean(generationType))
            {
                dsData = DBReadManager.GetOutgoingCBCReports(countryCode, period);
            }
            else
            {
                dsData = DBReadManager.GetNewIcomingCBCR(countryCode, period);
            }

            if (dsData != null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(dsData.Tables[0].Rows[0][0].ToString());
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

                    doc.WriteTo(writer);
                    writer.Flush();
                    Response.Clear();
                    byte[] byteArray = stream.ToArray();
                    Response.AppendHeader("Content-Disposition", "attachment;" + string.Format("filename ={0}_{1}.xml", fileName, period));
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



    protected void gvCBC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //  GridDecorator.MergeRows(gvCBC);
        }
    }

    private void ExportToExcel(string country, string period)
    {
        Response.ClearContent();
        Response.AddHeader("content-disposition", string.Format("attachment;filename={0}_{1}.xls", country, period));
        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        using (System.IO.StringWriter sWriter = new StringWriter())
        {
            using (System.Web.UI.HtmlTextWriter htmlWriter = new HtmlTextWriter(sWriter))
            {
                gvCBC.RenderControl(htmlWriter);
                Response.Write(sWriter);
            }
        }
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {



    }
}