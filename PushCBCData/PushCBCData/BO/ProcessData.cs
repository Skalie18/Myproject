using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using Sars.Models.CBC;
using PushCBCData.BLL;
namespace PushCBCData.BO
{
    public class ProcessData
    {

        public static void GetData()
        {
            try
            {
                var unprocessedData = dbManager.getData();
                ProcessCBCData(unprocessedData);
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetData");
            }
        }

        private static void ProcessCBCData(DataSet unprocessedData)
        {
            int counter = 0;
            try
            {
                foreach (DataRow row in unprocessedData.Tables[0].Rows)
                {
                    var results = row["CBC"].ToString();
                    var id = decimal.Parse(row["Id"].ToString());
                    var newXML = Utils.RemoveAllNameSpaces(results);
                    InterogateXML(newXML, id);
                    counter++;
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : ProcessCBCData");
            }

        }

        private static void InterogateXML(string xml, decimal id)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var cbcFormData = doc.Descendants("SubmitCountryByCountryDeclarationRequest").Select(el => el.LastNode);
                long tPeriod = 0;
                var cbcData = cbcFormData.ToList()[0];
                var reportingPeriod = GetReportingPeriod(doc);
                var taxRefNo = GetTaxReference(doc);
                var taxYear = GetReportingPeriod(doc).Substring(0, 4);//GetYear(doc);
                if (reportingPeriod.Length > 8)
                {
                    tPeriod = long.Parse(DateTime.ParseExact(reportingPeriod, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
                }
                else
                {
                    tPeriod = long.Parse(reportingPeriod);
                }
                var cbcForm = new CBC01Data()
                {
                    TaxRefNo = taxRefNo,
                    ReportingPeriod = tPeriod,
                    FormData = cbcData.ToString(),
                    TaxYear = int.Parse(taxYear)

                };
                dbManager.SaveCBC01FormData(cbcForm);
                dbManager.UpdateCBCDeclarations(id);
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : InterogateXML");
            }

        }

        private static string GetXml(string xml)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc.Value.ToString();
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetXml");
            }
            return string.Empty;
        }

        private static string GetTaxReference(XDocument doc)
        {
            try
            {
                var taxRefNo = doc.Descendants("MessageIdentification")
                    .Select(x => new
                    {
                        TaxRef = x.Element("externalReferenceID").Value
                    });
                if (taxRefNo != null)
                    return taxRefNo.ToList()[0].TaxRef.ToString();
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetTaxReference");
            }
            return string.Empty;
        }

        private static string GetReportingPeriod(XDocument doc)
        {
            try
            {
                var reportingPeriod = doc.Descendants("MessageSpec")
                    .Select(x => new
                    {
                        Period = x.Element("ReportingPeriod").Value
                    });
                if (reportingPeriod != null)
                    return reportingPeriod.ToList()[0].Period.ToString();
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetReportingPeriod");
            }
            return string.Empty;
        }


        private static int GetYear(XDocument doc)
        {
            try
            {
                var taxYear = doc.Descendants("FormInfo")
                    .Select(x => new
                    {
                        Year = x.Element("TaxYear").Value
                    });

                if (taxYear != null)
                    return int.Parse(taxYear.ToList()[0].Year.ToString());
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetYear");
            }
            return 0;
        }
    }
}
