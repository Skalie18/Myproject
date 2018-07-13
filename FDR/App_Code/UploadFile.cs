using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Configuration;
namespace PushCBCData.BO
{
    public class UploadFile
    {
        private static string message = "";
        public static string Message { get { return message; } set { message = value; } }
        public static void ProcessFile()
        {
            try
            {
                IFileParser fileParser = new DefaultFileParser();
                string destinationTableName = "MultiNationalEntityList";

                string sourceFileFullName = GetFileFullName();
                IEnumerable<DataTable> dataTables = fileParser.GetFileData(sourceFileFullName);
                foreach (DataTable tbl in dataTables)
                {
                    if (tbl == null)
                    {
                        message = "Error: File does not Exists";
                        break; 
                    }
                    fileParser.WriteChunkData(tbl, destinationTableName, Map());
                }
                if (message == string.Empty)
                    message = "File uploaded Successfully";

            }
            catch (Exception x)
            {
                message = "Error Msg : " + x.Message;
            }
        }
        private static string GetFileFullName()
        {
            return ConfigurationManager.AppSettings["filePath"].ToString();
        }

        [DebuggerStepThrough]
        private static IList<KeyValuePair<string, string>> Map()
        {
            return new List<KeyValuePair<string, string>>
            {
                {new KeyValuePair<string,string>("PartyID", "PartyID")},
                {new KeyValuePair<string,string>("TaxRefNo", "TaxpayerReferenceNumber")},
                {new KeyValuePair<string,string>("AssessmentYear", "YearofAssessment")},
                {new KeyValuePair<string,string>("RegisteredName", "RegisteredName")},
                {new KeyValuePair<string,string>("TradingName", "TradingName")},
                {new KeyValuePair<string,string>("RegistrationNo", "RegistrationNumber")},
                {new KeyValuePair<string,string>("FinancialYearEndDate", "FinancialYearEnd")},
                {new KeyValuePair<string,string>("SalesAmt", "TurnoverAmount")},
                {new KeyValuePair<string,string>("HoldingCompanyName", "NameUltimateHoldingCo")},
                {new KeyValuePair<string,string>("HoldingCompanyResidentOutsideSAInd", "UltimateHoldingCompanyResOutSAInd")},
                {new KeyValuePair<string,string>("HoldingCompanyResidencyCode", "TaxResidencyCountryCodeUltimateHoldingCompany")},
                {new KeyValuePair<string,string>("HoldingCompanyTaxRefNo", "UltimateHoldingCOIncomeTaxRefNo")},
                {new KeyValuePair<string,string>("MasterLocalFileRequiredInd", "MasterLocalFileRequiredInd")},
                {new KeyValuePair<string,string>("CBCReportRequired", "CbCReportRequiredInd")},
                {new KeyValuePair<string,string>("Datestamp", "Datestamp")},
            };

        }
        
    }
}
