using Sars.Models.CBC;
using Sars.Systems.Data;
using System.Collections;
using FDR.DataLayer;
using System.Data;
using PushCBCData.BO;
using System;

namespace PushCBCData
{
    class dbManager
    {
        public static void SaveCBC01FormData(CBC01Data formData)
        {
            try
            {
                var oParams = new DBParamCollection
                {
                    {"@RegisteredName", formData.RegisteredName},
                    {"@CompanyRegNo", formData.CompanyRegNo},
                    {"@TaxRefNo", formData.TaxRefNo},
                    {"@ReportingPeriod", formData.ReportingPeriod},
                    {"@FormData", formData.FormData},
                    {"@Year", formData.TaxYear}
                };
                using (var oCommand = new DBCommand("[dbo].[uspINSERT_CBC01Data]", QueryType.StoredProcedure, oParams))
                {
                    oCommand.Execute();
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetYear");
            }
        }

        public static void UpdateCBCDeclarations(decimal id)
        {
            try
            {
                var oParams = new DBParamCollection
            {
                {"@Id", id},
            };
                using (var oCommand = new DBCommand("[dbo].[uspUpdateCBCDeclarationsProcessed]", QueryType.StoredProcedure, oParams))
                {
                    oCommand.Execute();
                }
            }
            catch (Exception x)
            {
                EventLogging.LogError("Error Msg : " + x.Message + "\n" +
                               "Source : " + x.Source + "\n" +
                               "Event : GetYear");
            }
        }
        public static DataSet getData()
        {
            return new RecordSet("[dbo].[uspGetUnprocessedCBCData]", QueryType.StoredProcedure, null);
        }
    }
}
