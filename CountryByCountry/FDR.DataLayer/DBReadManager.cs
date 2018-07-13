using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Sars.Models.CBC;
using Sars.Systems.Data;
using Sars.Systems.Extensions;

namespace FDR.DataLayer
{
    public static class DBReadManager
    {
        #region READ FROM DB

        public static FormDefaults GetFormDefaults()
        {
            return new FormDefaults("[dbo].[usp_READ_FormDefaults]", null).GetRecord<FormDefaults>();
        }

        public static string GetLetterXml(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return string.Empty;
            }
            using (
                var command = new DBCommand("[dbo].[usp_GetLetterXML]", QueryType.StoredProcedure,
                    new DBParamCollection { { "@Id", id } }))
            {
                var xml = command.ExecuteScalar();
                return xml == null ? string.Empty : xml.ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            }
        }

        /*  public static object GetOutgoingCBCRStatuses(string countryCode, DateTime? startDate, DateTime? endDate)
          {
              var oParams = new DBParamCollection
              {
                  {"@CountryCode", countryCode},
                  {"@StartDate", startDate},
                  {"@EndDate", endDate}
              };
              return new RecordSet("[dbo].[uspREAD_OutgoingCBCRStatuses]", QueryType.StoredProcedure, oParams);
          }*/

        public static object GetOutgoingCBCRStatuses(string countryCode, string startDate, string endDate, int cbcrType)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@StartDate", startDate},
                {"@EndDate", endDate},
                {"@CBCRType", cbcrType}
            };
            return new RecordSet("[dbo].[uspREAD_OutgoingCBCRStatuses]", QueryType.StoredProcedure, oParams);
        }

        public static string GetFormVersion(string formName)
        {
            using (
                var oCommand = new DBCommand("usp_GetFormVersion", QueryType.StoredProcedure,
                    new DBParamCollection { { "@FormName", formName } }))
            {
                var version = oCommand.ExecuteScalar();
                if (version == null || version == DBNull.Value)
                {
                    return "v0.0";
                }
                return version.ToString();
            }
        }



        //public static UserDetails GetCurrentUserDetails()
        //{
        //    var TaxUserID = HttpContext.Current.Session["TaxUserID"];
        //    var TaxPayerID= HttpContext.Current.Session["TaxPayerID"];

        //    var userDetails = new UserDetails("[dbo].[usp_GetUserInfo]",
        //    new Dictionary<string, object> { { "@TaxUserID", TaxUserID }, { "@TaxPayerID", TaxPayerID } });
        //    var details = userDetails.GetRecord<UserDetails>();
        //    return details;
        //}
        public static List<RequestReasonCodes> GetRequestReasonCodes(string formName)
        {
            return
                new RequestReasonCodes().RunReadProcedureList<RequestReasonCodes>("[dbo].[usp_Read_RequestReasonCodes]",
                    new DBParamCollection() { { "@FormName", formName } });
        }

        public static string GetRequestErrorXml(decimal requestId)
        {
            using (var oCommand = new DBCommand("usp_READ_RequestErrors", QueryType.StoredProcedure,
                new DBParamCollection { { "@RequestId", requestId } }))
            {
                var xml = Convert.ToString(oCommand.ExecuteScalar());
                return xml;
            }
        }

        public static string GetRequestErrorXmlExtra(decimal requestId)
        {
            using (var oCommand = new DBCommand("ups_GET_MoreXML_Errors", QueryType.StoredProcedure,
                new DBParamCollection { { "@RequestId", requestId } }))
            {
                var xml = Convert.ToString(oCommand.ExecuteScalar());
                return xml;
            }
        }

        public static T SetFieldValue<T>(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (row[columnName] == null || row[columnName] == DBNull.Value)
                {
                    return default(T);
                }
                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)Boolean.Parse(row[columnName].ToString());
                }
                return (T)row[columnName];
            }
            return default(T);
        }

        public static DataRow ReadFormData(string requestId, string procedureName)
        {
            var oParams = new DBParamCollection
            {
                {"@RequestId", requestId}
            };
            using (var command = new DBCommand(procedureName, QueryType.StoredProcedure, oParams))
            {
                var xml = command.ExecuteScalar();
                if (xml != null)
                {
                    using (var reader = new StringReader(xml.ToString()))
                    {
                        var ds = new RecordSet();
                        ds.ReadXml(reader);
                        return ds.HasRows ? ds[0] : null;
                    }
                }
            }
            return null;
        }

        public static int RemoveRequest(decimal requestId, string procedure)
        {
            var oParams = new DBParamCollection
            {
                {"@RequestID", requestId}
            };
            using (var command = new DBCommand(procedure, QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }







        public static List<AddressType> GetAllAdressTypes()
        {
            return new AddressType().RunReadProcedureList<AddressType>("[dbo].[uspREAD_AllAddressTypes]", null);
        }

        public static List<BusinessActivities> GetAllBusinessActivities()
        {
            return
                new BusinessActivities().RunReadProcedureList<BusinessActivities>(
                    "[dbo].[uspREAD_AllBusinessActivities]", null);
        }

        public static List<PostalServices> GetAllPostalServices()
        {
            return new PostalServices().RunReadProcedureList<PostalServices>("[dbo].[uspREAD_AllPostalServices]", null);
        }

        public static List<ReportingRoles> GetAllReportingRoles()
        {
            return new ReportingRoles().RunReadProcedureList<ReportingRoles>("[dbo].[uspREAD_AllReportingRoles]", null);
        }

        public static List<SummaryReferenceCodes> GetAllSummaryRefCodes()
        {
            return
                new SummaryReferenceCodes().RunReadProcedureList<SummaryReferenceCodes>(
                    "[dbo].[uspREAD_AllSummaryReferenceCodes]", null);
        }

        public static List<MasterFileCategories> GetAllMasterFileCategories()
        {
            return
                new MasterFileCategories("[dbo].[uspREAD_MasterFileCategories]", null).GetRecords<MasterFileCategories>()
                    .ToList();
        }

        public static List<LocalFileCategories> GetAllLocalFileCategories()
        {
            return
                new LocalFileCategories("[dbo].[uspREAD_LocalFileCategories]", null).GetRecords<LocalFileCategories>()
                    .ToList();
        }


        public static IEnumerable<MultiNationalEntity> GetMultiNationalEntities(string taxRefNo = null)
        {
            return
                new MultiNationalEntity("usp_READ_MultiNationalEntityList",
                    new Dictionary<string, object> { { "@TaxNo", taxRefNo } }).GetRecords<MultiNationalEntity>();
        }

        public static MultiNationalEntity GetMultiNationalEntity(string Id)
        {
            return
                new MultiNationalEntity("usp_READ_MultiNationalEntityList", new Dictionary<string, object> { { "@Id", Id } })
                    .GetRecord<MultiNationalEntity>();
        }

        public static RecordSet GetErrorReport(string startDate, string endDate, string correlationId)
        {
            var oParams = new DBParamCollection
            {
                {"@FromDate", startDate},
                {"@EndDate", endDate},
                {"@CorrelationId", correlationId}
            };
            return new RecordSet("[dbo].[uspDownloadSystemErrorReport]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetErrorReport()
        {
            return new RecordSet("[dbo].[uspDownloadSystemErrorReport]", QueryType.StoredProcedure, null);
        }

        public static RecordSet EnquireMne(string taxRefNo)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo}
            };
            return new RecordSet("[dbo].[uspEnquireMNE]", QueryType.StoredProcedure, oParams);
        }

        public static IEnumerable<FileValidationOutcomeDetails> GetFileValidationOutcomeDetails(decimal submissionId)
        {
            return
                new FileValidationOutcomeDetails("[dbo].[usp_GetFileValidationOutcomeDetails]",
                    new Dictionary<string, object> { { "@SubmissionId", submissionId } })
                    .GetRecordsLazy<FileValidationOutcomeDetails>();
        }

        public static FileValidationOutcomeDetails GeFileValidationOutcomeDetailsByFileId(decimal fileId)
        {
            return
                new FileValidationOutcomeDetails("[dbo].[usp_GetFileValidationOutcomeDetailsByFileId]",
                    new Dictionary<string, object> { { "@FileId", fileId } })
                    .GetRecord<FileValidationOutcomeDetails>();
        }

        public static FileSubmission GeFileSubmissionById(decimal submissionId)
        {
            return
                new FileSubmission("[dbo].[ups_GetFileSubmissionsById]",
                    new Dictionary<string, object> { { "@SubmissionId", submissionId } })
                    .GetRecord<FileSubmission>();
        }

        public static RecordSet GetFilesPerSubmission(string submissionId)
        {
            return new RecordSet("[dbo].[usp_GetFilesPerSubmission]", QueryType.StoredProcedure,
                new DBParamCollection { { "@SubId", submissionId } });
        }


        public static string GetNextMessageId(string messageId)
        {
            using (var command = new DBCommand("usp_GetMessageId", QueryType.StoredProcedure, new DBParamCollection { { "@MessageId", messageId } }))
            {
                var id = command.ExecuteScalar();

                if (id != null)
                {
                    return id.ToString();
                }
                var random = new Random(1000);
                return random.Next(int.MaxValue, int.MaxValue).ToString();
            }
        }

        public static string GetStatusXml(string messageId)
        {
            using (var command = new DBCommand("SELECT [StatusMessage]  FROM [FDR].[dbo].[OutGoingCBCDeclarations] where [uid]=@UID", QueryType.TransectSQL, new DBParamCollection { { "@UID", messageId } }))
            {
                var xml = command.ExecuteScalar();

                if (xml != null)
                {
                    return xml.ToString();
                }
                return null;
            }
        }

        #endregion

        public static MasterLocalFileNotificationEmailBodyTemplate GetMasterLocalFileNotificationEmailBodyTemplates()
        {
            return
                new MasterLocalFileNotificationEmailBodyTemplate(
                    "[dbo].[uspREAD_MasterLocalFileNotificationEmailBodyTemplates]", null)
                    .GetRecord<MasterLocalFileNotificationEmailBodyTemplate>();
        }

        public static MasterLocalFileNotificationSmsBodyTemplate GetMasterLocalFileNotificationSmsBodyTemplates()
        {
            return
                new MasterLocalFileNotificationSmsBodyTemplate(
                    "[dbo].[uspREAD_MasterLocalFileNotificationSMSBodyTemplates]", null)
                    .GetRecord<MasterLocalFileNotificationSmsBodyTemplate>();
        }

        public static string GetMasterLocalFileNotificationEmailBodyTemplate(int eventType)
        {
            using (
                var command = new DBCommand("[dbo].[uspREAD_MasterLocalFileNotificationEmailBodyTemplateBytype]",
                    QueryType.StoredProcedure, new DBParamCollection { { "@Type", eventType } }))
            {
                var message = command.ExecuteScalar();
                return message?.ToString();
            }
        }
        public static string GetMasterLocalFileNotificationSmsBodyTemplate(int eventType)
        {
            using (
                var command = new DBCommand("[dbo].[uspREAD_MasterLocalFileNotificationSMSBodyTemplateBytype]",
                    QueryType.StoredProcedure, new DBParamCollection { { "@Type", eventType } }))
            {
                var message = command.ExecuteScalar();
                return message?.ToString();
            }
        }

        public static RecordSet GetFileById(decimal fileId)
        {
            return new RecordSet("[dbo].[usp_GetFileDetailsById]", QueryType.StoredProcedure, new DBParamCollection { { "@FileId", fileId } });
        }

        public static IEnumerable<FileValidationOutcome> GetFileValidationOutcomes()
        {
            return
                new FileValidationOutcome("[dbo].[usp_READ_FileValidationOutcomes]", null)
                    .GetRecordsLazy<FileValidationOutcome>();
        }

        public static RecordSet GetSubmissionsByStatus(int status, string taxRefNo = null, string year = null)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo },
                {"@Year", year },
                {"@StatusId", status}
            };
            return new RecordSet("[dbo].[uspREAD_FileSubmissionsByStatus]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet SearchSubmissions(string taxRefNo = null, string year = null)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo },
                {"@Year", year }
            };
            return new RecordSet("[dbo].[uspSearch_FileSubmissions]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet ReadSubmissions(string taxRefNo = null, string year = null)
        {
            return new RecordSet("[dbo].[uspGet_FileSubmissions]", QueryType.StoredProcedure, null);
        }


        public static RecordSet GetSubmissions(string taxRefNo = null, string year = null)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@Year", year}
            };
            using (var data = new RecordSet("[dbo].[uspREAD_FileSubmissions_All]", QueryType.StoredProcedure, oParams))
            {
                return data;
            }
        }

        public static RecordSet GetDeclinedFilesForSsubmission(decimal submissionId)
        {
            var oParams = new DBParamCollection
            {
                {"@SubmissionId", submissionId}
            };
            return new RecordSet("[dbo].[usp_GetRejectedFilesForSubmission]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetNewCbcDeclarations(string taxRefNo = null, int year = 0)
        {
            var _year = year <= 0 ? DBNull.Value : (object)year;
            return new RecordSet("[dbo].[upsGET_IncomingCBCDeclatations]", QueryType.StoredProcedure,
                new DBParamCollection { { "@TaxRefNo", taxRefNo }, { "@Year", _year } });
        }

        public static RecordSet GetViewedCbcDeclarations(string taxRefNo = null, int year = 0)
        {
            var _year = year <= 0 ? DBNull.Value : (object)year;
            return new RecordSet("[dbo].[upsGET_ViewedIncomingCBCDeclatations]", QueryType.StoredProcedure,
                new DBParamCollection { { "@TaxRefNo", taxRefNo }, { "@Year", _year } });
        }

        public static RecordSet GetResponse(string uniqueId)
        {
            return new RecordSet("[dbo].[usp_ReadQueueResponses]", QueryType.StoredProcedure, new DBParamCollection { { "@universalUniqueID", uniqueId } });
        }

        public static IEnumerable<SentLetters> GetSentLetters(string taxRefno = null)
        {
            if (string.IsNullOrEmpty(taxRefno))
            {
                taxRefno = null;
            }
            return
                new SentLetters("[dbo].[usp_GetSentLetters]", new Dictionary<string, object> { { "@TaxRefNo", taxRefno } })
                    .GetRecords<SentLetters>();
        }

        public static IEnumerable<FdrUserDetails> GetFdrUsers(string roleName = null)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                roleName = null;
            }
            return
                new FdrUserDetails("[dbo].[uspGetUsersPerRole]",
                    new Dictionary<string, object> { { "@RoleName", roleName } })
                    .GetRecords<FdrUserDetails>();
        }

        public static RecordSet GetAllUsers()
        {
            return new RecordSet("[dbo].[spGetAllSystemUsers]", QueryType.StoredProcedure, null);
        }

        public static RecordSet SearchUser(string searchText, int pageIndex, int pageSize)
        {
            var oParams = new DBParamCollection
            {
                {"@SearchTerm", searchText},
                {"@PageIndex", pageIndex},
                {"@PageSize", pageSize},
                {"@RecordCount", 5, System.Data.ParameterDirection.Output}
            };

            return new RecordSet("[dbo].[GetUsers_Pager]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetSecureRoles()
        {
            var oParams = new DBParamCollection
            {
                {"@SystemName", "FDR"}
            };

            return new RecordSet("[secure].[spGetActiveRoles]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetOutGoingCBCR(string countryCode, string reportingPeriod, int status)
        {
            var oParams = new DBParamCollection
            {
               {"@CountryCode", countryCode},
               {"@ReportingPeriod", reportingPeriod},
               {"@Status", status}
            };
            return new RecordSet("[dbo].[uspREAD_OutgoingCBCRDeclarations]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetOutgoingCBCRStatuses(string countryCode, DateTime sDate, DateTime eDate)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@StartDate", sDate},
                {"@EndDate", eDate}
            };
            return new RecordSet("[dbo].[uspREAD_OutgoingCBCRStatuses]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetNewOutGoingCBCR(string countryCode, string reportingPeriod)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod}
            };
            return new RecordSet("[dbo].[uspREAD_NewOutgoingCBCRDeclarations]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetNewIcomingCBCR(string countryCode, string period)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", period}
            };
            return new RecordSet("[dbo].[uspREAD_NewIncomingForeignCBCDeclarations]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet ValidationStatuses()
        {
            return new RecordSet("[dbo].[uspREAD_ValidationStatuses]", QueryType.StoredProcedure, null);
        }

        public static RecordSet GetOutgoinCBCReport(string country)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", country}
            };

            return new RecordSet("[dbo].[uspREAD_OutgoingCBCData]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetCorrectedReports(string country, string reportingPeriod)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", country},
                {"@ReportingPeriod", reportingPeriod}
            };

            return new RecordSet("[dbo].[uspREAD_OutgoingCBCReportsCorrectionData]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetOutgoinCBCReport4Package(string country, string reportingPeriod)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", country},
                {"@ReportingPeriod", reportingPeriod}
            };
            return new RecordSet("[dbo].[uspREAD_CBCR_By_PeriodAndCountry]", QueryType.StoredProcedure, oParams);
            //  return new RecordSet("[dbo].[uspREAD_OutgoingCBCData]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet ExecuteDynamicQuery(string tSql)
        {
            var oParams = new DBParamCollection
            {
                {"@tSQL", tSql},

            };

            return new RecordSet("[dbo].[uspDynamicExuteQuery]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetOldOutgoingCBCData(string country, string reportingPeriod)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", country},
                {"@ReportingPeriod", reportingPeriod}
            };

            return new RecordSet("[dbo].[uspREAD_OldOutgoingCBCData]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetCorrectedCBCReport()
        {
            return new RecordSet("[dbo].[usp_READ_CorrectedCBC]", QueryType.StoredProcedure, null);

        }

        public static RecordSet GetCorrectedCBCByCountryAndReportingPeriod(string countryCode, string ReportingPeriod)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
            {"@ReportingPeriod", ReportingPeriod}
        };
            return new RecordSet("[dbo].[usp_READ_CorrectedCBCBYCountryAndReportingPeriod]", QueryType.StoredProcedure, oParams);

        }
        public static RecordSet GetPackageForDeletion(string country, string reportingPeriod)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", country},
                {"@ReportingPeriod", reportingPeriod}
            };

            return new RecordSet("[dbo].[uspREAD_PackageForDeletion]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetPackagesToBeCorrected(string reportingPeriod, string taxRefNo)
        {
            var oParams = new DBParamCollection
            {
                {"@ReportingPeriod", reportingPeriod},
                {"@TaxRefNo", taxRefNo}
            };

            return new RecordSet("[dbo].[uspREAD_PackagesToBeCorrected]", QueryType.StoredProcedure, oParams);
        }

        public static int READOutGoingCBCDeclarationsID(string country, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", country},
                {"@Year", year}
            };

            var results = new RecordSet("[dbo].[uspREADOutGoingCBCDeclarationsID]", QueryType.StoredProcedure, oParams);
            if (results.HasRows)
            {
                return int.Parse(results.Tables[0].Rows[0]["Id"].ToString());
            }
            return 0;
        }

        public static RecordSet GetOutgoingCBCReportPackaged(string CountryCode, string reportingPeriod, int status = 0)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", CountryCode},
                {"@ReportingPeriod", reportingPeriod},
                {"@StatusId", status}
            };

            return new RecordSet("[dbo].[uspREAD_PackedCBCReports]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetOutgoingCBCReports(string countryCode, string period)
        {
            var oParams = new DBParamCollection
        {
            {"@countrycode", countryCode},
              {"@ReportingPeriod", period}
        };
            return new RecordSet("[dbo].[uspREAD_NewOutGoingCBCDeclarations]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetVerifiedOutGoingCBCs(string countryCode, string period)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
            {"@ReportingPeriod", period}
        };
            return new RecordSet("[dbo].[uspREAD_OutgoingCBCRDeclarations4Approval]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetSentPackages(string countryCode, int Year)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
              {"@Year", Year}
        };
            return new RecordSet("[dbo].[uspREAD_SentPackages]", QueryType.StoredProcedure, oParams);
        }



        public static RecordSet GetAllReportingPeriods(string countryCode = null)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode}
            };
            return new RecordSet("[dbo].[uspREAD_ALL_ReportingPeriods]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetAllIncomingReportingPeriods(string countryCode = null)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode}
            };
            return new RecordSet("[dbo].[uspREAD_ALL_IncomingReportingPeriods]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetPackagesToVoid(string countryCode, string reportingPeriod)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
              {"@ReportingPeriod", reportingPeriod}
        };
            return new RecordSet("[dbo].[uspREAD_GetPackagesToVoid]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetSentPackagesHistory(string countryCode, int Year)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
              {"@Year", Year}
        };
            return new RecordSet("[dbo].[uspREAD_SentPackages_HISTORY]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetRecordsForCorrections(string countryCode, string reportingPeriod)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
              {"@ReportingPeriod", reportingPeriod}
        };
            return new RecordSet("[dbo].[uspGetRecordsForCorrections]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetDataToCorrect(string countryCode, int Year)
        {
            var oParams = new DBParamCollection
        {
            {"@Country", countryCode},
            {"@Year", Year}
        };
            return new RecordSet("[dbo].[uspREAD_DataToCorrect]", QueryType.StoredProcedure, oParams);
        }


        public static RecordSet GetOutgoingCBCGenDownload(string countryCode, int Year)
        {
            var oParams = new DBParamCollection
        {
            {"@Country", countryCode},
            {"@Year", Year}
        };
            return new RecordSet("[dbo].[uspREAD_OutgoingCBCGenDownload]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GeneratedFileDownload(string countryCode, string reportingPeriod, bool blnOutgoing = false)
        {
            string procName = "[dbo].[uspREAD_IncomingCBCGenDownload]";
            if (blnOutgoing)
            {
                procName = "[dbo].[uspREAD_OutgoingCBCGenDownload]";
            }
            var oParams = new DBParamCollection
            {
                {"@Country", countryCode},
                {"@ReportingPeriod", reportingPeriod}
            };
            return new RecordSet(procName, QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetIncomingForeignDeclaration(int statusId, string countryCode = null, string reportingPeriod = null)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
            {"@ReportingPeriod", reportingPeriod},
             {"@StatusId", statusId}
        };
            return new RecordSet("[dbo].[uspREAD_IncomingForeignDeclaration]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetIncomingForeignCBCRPerPackage(string countryCode, int Year)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
            {"@Year", Year}
        };
            return new RecordSet("[dbo].[uspREAD_IncomingForeignCBCRPerPackage]", QueryType.StoredProcedure, oParams);
        }

        public static RecordSet GetDocRefIdUsed(string docRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@docRefID", docRefId}
            };

            return new RecordSet("[dbo].[uspREAD_DocRefIDUsed]", QueryType.StoredProcedure, oParams);
        }

        public static OutGoingCBCDeclarations IncomingCBCDeclaration(int statusId, string countryCode, string ReportingPeriod)
        {
            var oParams = new Dictionary<string, object>
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", ReportingPeriod},
                 {"@StatusId", statusId}
            };
            var result = new OutGoingCBCDeclarations("[dbo].[uspREAD_IncomingForeignDeclaration]", oParams);
            return result.GetRecord<OutGoingCBCDeclarations>();
        }

        public static OutGoingCBCDeclarations OutGoingCBCDeclarationsDetails(string countryCode, string reportingPeriod)
        {
            var result = new OutGoingCBCDeclarations("[dbo].[uspREAD_OutGoingCBCDeclarationsDetails]", new Dictionary<string, object> { { "@CountryCode", countryCode }, { "@ReportingPeriod", reportingPeriod } });
            return result.GetRecord<OutGoingCBCDeclarations>();
        }

        public static X_MessageSpec GetMessageSpecById(decimal mspecId)
        {
            var result = new X_MessageSpec("[dbo].[usp_READ_MessageSpec]", new Dictionary<string, object> { { "@MessageSpec_ID", mspecId } });
            return result.GetRecord<X_MessageSpec>();
        }

        public static string GenerateNewDocRefId(string countryCode, int year, string packageUID, string messageRefId, string eFilingDocRefID)
        {
            var oParams = new DBParamCollection
            {
                {"@ReceivingCountryCode", countryCode},
                {"@TaxYear", year},
                {"@PackageUID", packageUID},
                {"@DOCREFID", null, ParameterDirection.Output},
                {"@MessageRefId", messageRefId },
                {"@EFilingDocRefID", eFilingDocRefID }
            };
            using (var command = new DBCommand("[dbo].[usp_Generate_NewOutGoingPackageDocRefIDs]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@DOCREFID"))
                {
                    return ht["@DOCREFID"].ToString();
                }
                return null;
            }
        }

        public static RecordSet GetOutgoingPackageHistory(string countryCode, string reportingPeriod)
        {
            var oParams = new DBParamCollection
        {
            {"@CountryCode", countryCode},
            {"@ReportingPeriod", reportingPeriod}
        };
            return new RecordSet("[dbo].[uspREAD_OutgoingPackageHistory]", QueryType.StoredProcedure, oParams);
        }



        public static RecordSet GetIncomingPackagePerCBC(decimal id)
        {
            var oParams = new DBParamCollection
        {
            {"@MessageSpec_ID", id},
        };
            return new RecordSet("[dbo].[uspREAD_IncomingForeignPackageCBCR]", QueryType.StoredProcedure, oParams);

        }

        public static RecordSet GetIncomingForeignCBCR(decimal id)
        {
            var oParams = new DBParamCollection
        {
            {"@MessageSpec_ID", id},
        };
            return new RecordSet("[dbo].[uspREAD_IncomingForeignCBCReports]", QueryType.StoredProcedure, oParams);

        }

        public static RecordSet GetIncomingForeignXml(decimal id)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageSpec_ID", id},
            };
            return new RecordSet("[dbo].[uspREAD_IncomingForeignPackageXML]", QueryType.StoredProcedure, oParams);
        }

        public static int MessageRefIdAlreadyExists(string messageRefId, string countryCode)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageRefID", messageRefId},
                {"@From", countryCode},
                {"@Return_Value", null , ParameterDirection.ReturnValue }
            };
            using (var command = new DBCommand("[dbo].[usp_INSERT_X_MessageRefIDs]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Return_Value"))
                {
                    return Convert.ToInt32(ht["@Return_Value"]);
                }
                return 0;
            }
        }

        public static int DocRefIdAlreadyExists(string docRefId, string countryCode, string messageRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@DocRefID", docRefId},
                {"@From", countryCode},
                {"@MessageRefID", messageRefId},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_READ_X_DocRefIDs]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Return_Value"))
                {
                    return Convert.ToInt32(ht["@Return_Value"]);
                }
                return 0;
            }
        }

        public static int CheckIfDocRefIdExistsForGivenCorDocRefId(string corDocRefId, string countryCode, string messageRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@DocRefID", corDocRefId},
                {"@From", countryCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_Check_ExistanceOfDocRefIdFrom_X_DocRefIDs]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Return_Value"))
                {
                    return Convert.ToInt32(ht["@Return_Value"]);
                }
                return 0;
            }
        }

        public static RecordSet Read_X_CBC_RecordValidations(string messageSpecId)
        {
            return new RecordSet("usp_READ_X_CBC_RecordValidations", QueryType.StoredProcedure, new DBParamCollection { { "@MessageSpec_ID", messageSpecId } });
        }
        public static RecordSet Read_X_CBC_FileValidations(string messageSpecId)
        {
            return new RecordSet("usp_READ_X_CBC_FileValidations", QueryType.StoredProcedure, new DBParamCollection { { "@MessageSpec_ID", messageSpecId } });
        }
        public static RecordSet GetRolesToNotify(string roleName)
        {
            return new RecordSet("[dbo].[uspGetUsersByRole]", QueryType.StoredProcedure, new DBParamCollection { { "@RoleName", roleName } });
        }

        public static string GetOriginalMessageRefId(string corrDocRefId, string country)
        {
            var oParams = new DBParamCollection
            {
                {"@From", corrDocRefId}
                ,
                {"@CorrDocRefId", corrDocRefId}
            };
            using (var command = new DBCommand("[dbo].[usp_GET_OriginalMessageRefId]", QueryType.StoredProcedure, oParams))
            {
                var messageRefId = command.ExecuteScalar();
                if (messageRefId == null)
                {
                    return null;
                }
                return messageRefId.ToString();
            }
        }

        public static string GetOriginalDocRefId(string efilingCorrDocRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@CorrDocRefId", efilingCorrDocRefId}
            };
            using (var data = new RecordSet("[dbo].[usp_Get_OriginalDocRefID]", QueryType.StoredProcedure, oParams))
            {
                if (data.HasRows)
                {
                    return Convert.ToString(data[0]["DocRefID"]);
                }
                else
                    return null;
            }
        }
    }
}
