using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace PushCBCData.BO
{
    public class UploadFile
    {
        public static void ProcessFile()
        {
            try
            {
                IFileParser fileParser = new DefaultFileParser();
                string destinationTableName = "MultiNationalEntityListTemp";

                string sourceFileFullName = GetFileFullName();
                IEnumerable<DataTable> dataTables = fileParser.GetFileData(sourceFileFullName);
                List<DataTable> tables = new List<DataTable>();
                //int count = 0;
                foreach (DataTable tbl in dataTables)
                {
                    /*tbl.TableName = tbl.TableName + count;
                    tables.Add(tbl);*/
                    fileParser.WriteChunkData(tbl, destinationTableName, Map());
                    //count++;
                }

               /* foreach (var table in tables)
                {
                    fileParser.WriteChunkData(table, destinationTableName, Map());
                }*/

                EventLogging.LogInformation("Files uploaded Successfully");

            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : CreateEmptyDataTable");
            }
        }
        private static string GetFileFullName()
        {
            try
            {
                return AppConfigs.FilePath;
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : CreateEmptyDataTable");
            }
            return null;

        }

        [DebuggerStepThrough]
        private static IList<KeyValuePair<string, string>> Map()
        {
            try{
            return new List<KeyValuePair<string, string>> 
            {
                {new KeyValuePair<string, string>("PartyID", "PartyID")},
                {new KeyValuePair<string, string>("TaxRefNo", "TaxpayerReferenceNumber")},
                {new KeyValuePair<string, string>("AssessmentYear", "YearofAssessment")},
                {new KeyValuePair<string, string>("RegisteredName", "RegisteredName")},
                {new KeyValuePair<string, string>("TradingName", "TradingName")},
                {new KeyValuePair<string, string>("RegistrationNo", "RegistrationNumber")},
                {new KeyValuePair<string, string>("FinancialYearEndDate", "FinancialYearEnd")},
                {new KeyValuePair<string, string>("SalesAmt", "TurnoverAmount")},
                {new KeyValuePair<string, string>("HoldingCompanyName", "NameUltimateHoldingCo")},
                {new KeyValuePair<string, string>("HoldingCompanyResidentOutsideSAInd", "UltimateHoldingCompanyResOutSAInd")},
                {new KeyValuePair<string, string>("HoldingCompanyResidencyCode", "TaxResidencyCountryCodeUltimateHoldingCompany")},
                {new KeyValuePair<string, string>("HoldingCompanyTaxRefNo", "UltimateHoldingCOIncomeTaxRefNo")},
                {new KeyValuePair<string, string>("MasterLocalFileRequiredInd", "MasterLocalFileRequiredInd")},
                {new KeyValuePair<string, string>("CBCReportRequired", "CbCReportRequiredInd")},
                {new KeyValuePair<string, string>("Datestamp", "Datestamp")},
            };
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : CreateEmptyDataTable");
            }
            return null;
        }
    }
}
