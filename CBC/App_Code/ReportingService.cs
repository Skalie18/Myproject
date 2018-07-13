using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using Sars.Systems.Data;

namespace Sars.Systems.ReportingServices
{
    public static class ReportingService
    {
        private static void Test()
        {
            const string connection = "Data Source=LPT472DB;Initial Catalog=SurveyProduction02May2012; UID=sa;PWD=P@ssw0rd";
            const string reportName = @"/AssessmentTool/User Report";
            const string url = "http://wks014bp/Reportserver/ReportExecution2005.asmx";

            using (var data = new RecordSet("SELECT TOP 100 AssessmentId, SID, RoleId FROM Assessment.EmployeeAssessments", QueryType.TransectSQL, null, connection))
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var fileName = string.Format(@"D:\RssProxy\UR\{0}_{1}_{2}.html", row["SID"], row["AssessmentId"], row["AssessmentId"]);
                    var parameters = new Dictionary<string, string>
                                         {
                                             {"AssessmentId", row["AssessmentId"].ToString()},
                                             {"RoleId", row["AssessmentId"].ToString()},
                                             {"SID", row["SID"].ToString()}
                                         };
                    var results = Render(url, ExportFormat.PDF, parameters, reportName, null);
                    using (var stream = File.OpenWrite(fileName))
                    {
                        stream.Write(results, 0, results.Length);
                    }
                }
            }
        }

        public static byte[] Render(string ssrUrl, ExportFormat exportFormat, Dictionary<string, string> parametersAndValues, string reportName, ICredentials credentials )
        {
            using (var reportExecution = new ReportExecutionService(ssrUrl) { Credentials = credentials ?? CredentialCache.DefaultCredentials })
            {
                string encoding;
                string mimeType;
                string extension;
                Warning[] warnings;
                string[] streamIDs;

                reportExecution.LoadReport(reportName, null);
                if (parametersAndValues.Count > 0)
                {
                    var parameters = new ParameterValue[parametersAndValues.Count];
                    var i = 0;
                    foreach (var paramsAndValues in parametersAndValues)
                    {
                        parameters[i] = new ParameterValue
                                            {Name = paramsAndValues.Key, Value = paramsAndValues.Value};
                        i++;
                    }
                    reportExecution.SetExecutionParameters(parameters, "en-us");
                }
                var results = reportExecution.Render(exportFormat.ToString(), null, out extension, out mimeType,
                                                     out encoding, out warnings, out streamIDs);
                return results;
            }
        }

        public static void ExportReport(string ssrUrl, ExportFormat exportFormat, Dictionary<string, string> parametersAndValues, string reportName, CredentialCache credentials)
        {
            if (System.Web.HttpContext.Current != null)
            {
                var response = System.Web.HttpContext.Current.Response;
                var results = Render(ssrUrl, exportFormat, parametersAndValues, reportName, credentials);
                switch (exportFormat)
                {
                    case ExportFormat.PDF:
                        {
                            response.Clear();
                            response.ContentType = "application/pdf";
                            response.AddHeader("content-length", results.Length.ToString(CultureInfo.InvariantCulture));
                            response.BinaryWrite(results);
                            response.End();
                            break;
                        }

                    case ExportFormat.EXCEL:
                        {
                            response.ClearContent();
                            response.ContentType = "application/vnd.ms-excel";
                            response.AddHeader("content-length", results.Length.ToString(CultureInfo.InvariantCulture));
                            response.BinaryWrite(results);
                            response.End();
                            break;
                        }
                    case ExportFormat.WORD:
                        {
                            response.ClearContent();
                            response.ContentType = "application/msword";
                            response.AddHeader("content-length", results.Length.ToString(CultureInfo.InvariantCulture));
                            response.BinaryWrite(results);
                            response.End();
                            break;
                        }

                    case ExportFormat.IMAGE:
                        {
                            response.ClearContent();
                            response.ContentType = "image/bmp";
                            response.AddHeader("content-length", results.Length.ToString(CultureInfo.InvariantCulture));
                            response.BinaryWrite(results);
                            response.End();
                            break;
                        }
                }
            }
        }

        public enum ExportFormat
        {
            PDF,
            EXCEL,
            WORD,
            IMAGE
        }
    }
}
