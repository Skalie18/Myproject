using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sars.Systems.Data;
using Sars.Systems.Security;

namespace FRD_MNE_Services
{
    public static class DbWriter
    {
        public static int SaveMneRequest(string messageId, string message,string taxRefNo, string year)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageID", messageId},
                {"@Message", message},
                {"@TaxRefNo", taxRefNo},
                {"@Year", year}
            };
            using (var command = new DBCommand("[dbo].[uspINSERT_MneEnquireRequests]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }


        public static int SaveMneResponse(string messageId, string message)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageID", messageId},
                {"@Message", message}
            };
            using (var command = new DBCommand("[dbo].[uspINSERT_MneEnquireResponse]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int UpdateMneViaFileNotification(string taxRefNo, int assessmentYear)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxpayerReferenceNumber", taxRefNo},
                {"@YearofAssessment", assessmentYear}
            };
            using (var command = new DBCommand("[dbo].[usp_UPDATE_MultiNationalEntityListViaDocNotification]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static decimal InsertFileSubmission(string taxRefNo, int assessmentYear)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@Year", assessmentYear},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_Insert_FileSubmissions]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Return_Value"))
                {
                    return Convert.ToDecimal(ht["@Return_Value"]);
                }
                return -1;
            }
        }

        public static int SaveFile(decimal submissionId, string taxRefNo, int fileCategoryId, string objectName, string documentId, string contentLocation, 
            string format, string yearOfAssessment, string path, string sourceChannel, string dateOfReceipt,string classification, string uniqueId)
        {

            var oParams = new DBParamCollection
            {
                {"@SubmissionId", submissionId},
                {"@Taxrefno",taxRefNo},
                {"@FileCategoryID",fileCategoryId},
                {"@FileName", objectName},
                {"@ObjectID", documentId},
                {"@Location", contentLocation },
                {"@Extension", format},
                {"@AssessmentYear", yearOfAssessment},
                {"@Path", path},
                {"@SourceChannel", sourceChannel},
                {"@DateOfReceipt", dateOfReceipt},
                {"@Classification", classification},
                {"@UniqueId", uniqueId}
                //,{"@SecLocation", AppConfig.SecondaryFileLocation}
            };

            using (var command = new DBCommand("[dbo].[ups_INSERT_Files]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        internal static void LogIncomingFiles(decimal submissionId, string xmlMessage)
        {
            using (var command = new DBCommand("[dbo].[uspINSERT_IncomingFileData]", QueryType.StoredProcedure,
                new DBParamCollection
                {
                    {"@SubmissionId", submissionId},
                    {"@Data", xmlMessage}
                }
                ))
            {
                command.Execute();
            }
        }

        internal static int SaveSentSmsCommunications(string taxRefNo, string text, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@TaxYear", year},
                {"@SMSBody", text}
            };
            using (var command = new DBCommand("[dbo].[usp_INSERT_SMSCommunication]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

    }
}
