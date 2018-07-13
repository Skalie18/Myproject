using System;
using System.Collections;
using System.Data;
using Sars.Models.CBC;
using Sars.Systems.AdhocQueries;
using Sars.Systems.Data;

namespace FDR.DataLayer
{ 
    public static class DBWriteManager
    {
        #region DB UPDATES



        public static int SaveFormData(DataTable requestDateTable, decimal requestId, string formId, string taxUserId,
            string taxPayerId, string procedureName, string formName)
        {
            using (var data = new RecordSet("FORMADSET"))
            {
                data.Tables.Add(requestDateTable);
                var xml = data.GetXml();
                var oParams = new DBParamCollection
                {
                    {"@DirectiveRequestID", requestId},
                    {"@FormID", formId},
                    {"@TaxUserID", taxUserId},
                    {"@TaxPayerID", taxPayerId},
                    {"@DirectiveTypeCode", formName},
                    {"@FormContetnts", xml}
                };
                using (var command = new DBCommand(procedureName, QueryType.StoredProcedure, oParams))
                {
                    return command.Execute();
                }
            }
        }



        public static int AllocateCase(string submissionId)
        {
            var oParams = new DBParamCollection
            {
                {"@SubmissionId", submissionId},
                {"@CreatedBy", Sars.Systems.Security.ADUser.CurrentSID},
                {"@ReturnValue", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[uspINSERT_CaseAllocation]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@ReturnValue"))
                {
                    return Convert.ToInt32(ht["@ReturnValue"]);
                }
                return -1;
            }
        }

        public static int SubmitRequest(decimal requestId, string formId)
        {
            var oParams = new DBParamCollection
            {
                {"@RequestID", requestId},
                {"@FormID", formId}
            };
            using (var command = new DBCommand("[dbo].[usp_SubmitRequst]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int DuplicateRequest(string formId, string requestId, string userId)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxUserID", userId},
                {"@FormId", formId},
                {"@RequestId", requestId}
            };

            using (
                var oCommand = new DBCommand("[dbo].[Duplicate_Directive_Request]", QueryType.StoredProcedure, oParams))
            {
                return oCommand.Execute();
            }
        }

        public static int ArchieveDirective(string formId, string requestId, string userId)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxUserID", userId},
                {"@FormId", formId},
                {"@RequestId", requestId}
            };

            using (var oCommand = new DBCommand("[dbo].[Archive_Directive_Request]", QueryType.StoredProcedure, oParams)
                )
            {
                return oCommand.Execute();
            }
        }

        public static int SendCancelRequest
            (
            string userId
            , string taxpayerId
            , string formId
            , string requestId
            , string cReason
            , string cContactPerson
            , string cDialCode
            , string cTelNo
            , string req_Seq_Num
            )
        {
            var oParams = new DBParamCollection
            {
                {"@TaxUserID", userId},
                {"@TaxPayerID", taxpayerId},
                {"@FormId", formId},
                {"@RequestId", requestId},
                {"@Reason", cReason},
                {"@ContactPerson", cContactPerson},
                {"@DialCode", cDialCode},
                {"@TelNo", cTelNo},
                {"@req_Seq_Num", req_Seq_Num}
            };
            using (var command = new DBCommand("[dbo].[usp_RequstDirCancelation]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int MoveDirectiveToPending(string directiveRequestId)
        {
            var oParams = new DBParamCollection
            {
                {"@RequestID", directiveRequestId}
            };
            using (var oCommand = new DBCommand("[dbo].[MoveDirectiveToPending]", QueryType.StoredProcedure, oParams))
            {
                return oCommand.Execute();
            }
        }

        public static void SaveRequestDetails(
            string sessionId
            , string appRelativeCurrentExecutionFilePath
            , string filePath
            , string physicalApplicationPath
            , string rawUrl
            , string userAgent
            , bool isAuthenticated
            , string machineName
            , string authUser
            )
        {
            var oParams = new DBParamCollection
            {
                {"@SessionID", sessionId},
                {"@AppRelativeCurrentExecutionFilePath", appRelativeCurrentExecutionFilePath},
                {"@FilePath", filePath},
                {"@PhysicalApplicationPath", physicalApplicationPath},
                {"@RawUrl", rawUrl},
                {"@UserAgent", userAgent},
                {"@IsAuthenticated", isAuthenticated},
                {"@MachineName", machineName},
                {"@AuthUser", authUser}
            };

            using (
                var oCommand = new DBCommand("[dbo].[spINSERT_RequestDetails]", QueryType.StoredProcedure, oParams))
            {
                oCommand.Execute();
            }
        }

        public static void SaveSystemError(string sessionId, string message, string stacktrace, string systemUser)
        {
            var oParams = new DBParamCollection
            {
                {"@Message", message},
                {"@SessionId", sessionId},
                {"@StackTrace", stacktrace},
                {"@UserName", systemUser}
            };

            using (var oCommand = new DBCommand("[dbo].[spINSERT_SystemErrors]", QueryType.StoredProcedure, oParams))
            {
                oCommand.Execute();
            }
        }

        public static decimal SaveReportEntityContactPersonDetails(ReportEntityContactPersonDetails recpDetails)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", recpDetails.ID},
                {"@FirstNames", recpDetails.FirstNames},
                {"@Surname", recpDetails.Surname},
                {"@BusTelNo1", recpDetails.BusTelNo1},
                {"@BusTelNo2", recpDetails.BusTelNo2},
                {"@CellNo", recpDetails.CellNo},
                {"@EmailAddress", recpDetails.EmailAddress},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_ReportEntityContactPersonDetails]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static int UpdatePackageFailedSending(decimal packageId)
        {
            using (var command = new DBCommand("usp_UpdatePackageFailedSending", QueryType.StoredProcedure, new DBParamCollection { { "@Id", packageId } }))
            {
                return command.Execute();
            }
        }

        public static decimal SaveReportEntityAddress(EntityAddress entityAddress)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", entityAddress.ID},
                {"@AddressTypeCode", entityAddress.AddressTypeCode},
                {"@PostalServiceID", entityAddress.PostalServiceID},
                {"@OtherPOSpecialService", entityAddress.OtherPOSpecialService},
                {"@Number", entityAddress.Number},
                {"@PostOffice", entityAddress.PostOffice},
                {"@PAPostalCode", entityAddress.PAPostalCode},
                {"@PACountryCode", entityAddress.PACountryCode},
                {"@UnitNo", entityAddress.UnitNo},
                {"@Complex", entityAddress.Complex},
                {"@StreetNo", entityAddress.StreetNo},
                {"@StreetName", entityAddress.StreetName},
                {"@Suburb", entityAddress.Suburb},
                {"@City", entityAddress.City},
                {"@RESPostalCode", entityAddress.RESPostalCode},
                {"@RESCountryCode", entityAddress.RESCountryCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_ReportEntityAddress]", QueryType.StoredProcedure, oParams)
                )
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveReportEntity(ReportEntity reportingEntity)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", reportingEntity.ID},
                {"@RegisteredName", reportingEntity.RegisteredName},
                {"@CompanyRegNo", reportingEntity.CompanyRegNo},
                {"@CompanyRegNoIssuedByCountry", reportingEntity.CompanyRegNoIssuedByCountry},
                {"@TaxRefNo", reportingEntity.TaxRefNo},
                {"@TaxRefNoIssuedByCountry", reportingEntity.TaxRefNoIssuedByCountry},
                {"@GIINNoIndicator", reportingEntity.GIINNoIndicator},
                {"@GIINNo", reportingEntity.GIINNo},
                {"@GIINNoIssuedByCountry", reportingEntity.GIINNoIssuedByCountry},
                {"@ReportingRoleId", reportingEntity.ReportingRoleId},
                {"@ResidentCountryCode", reportingEntity.ResidentCountryCode},
                {"@UniqueNo", reportingEntity.UniqueNo},
                {"@RecordStatusId", reportingEntity.RecordStatusId},
                {"@ReportEntityContactPersonDetailsId", reportingEntity.ReportEntityContactPersonDetailsId},
                {"@ReportEntityAddressId", reportingEntity.ReportEntityAddressId},
                {"@ReportingPeriod", reportingEntity.ReportingPeriod},
                {"@TaxUserID", reportingEntity.TaxUserID},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (var oCommand = new DBCommand("[dbo].[uspUPSERT_ReportEntity]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveCBCReport(CBCReports cbcReports)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", cbcReports.ID},
                {"@ReportEntityId", cbcReports.ReportEntityId},
                {"@NumberOfTaxJurisdictions", cbcReports.NumberOfTaxJurisdictions},
                {"@UniqueNo", cbcReports.UniqueNo},
                {"@RecordStatusId", cbcReports.RecordStatusId},
                {"@ResidentCountryCode", cbcReports.ResidentCountryCode},
                {"@CurrencyCode", cbcReports.CurrencyCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReports]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveReportSummary(CBCReport_Summary reportSummary)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", reportSummary.ID},
                {"@CBCReportId", reportSummary.CBCReportId},
                {"@ProfitLossBeforeIncomeTax", reportSummary.ProfitLossBeforeIncomeTax},
                {"@ProfitLossBeforeIncomeTaxCurrencyCode", reportSummary.ProfitLossBeforeIncomeTaxCurrencyCode},
                {"@StatedCapital", reportSummary.StatedCapital},
                {"@StatedCapitalCurrencyCode", reportSummary.StatedCapitalCurrencyCode},
                {"@IncomeTaxPaid", reportSummary.IncomeTaxPaid},
                {"@IncomeTaxPaidCurrencyCode", reportSummary.IncomeTaxPaidCurrencyCode},
                {"@AccumulatedEarnings", reportSummary.AccumulatedEarnings},
                {"@AccumulatedEarningsCurrencyCode", reportSummary.AccumulatedEarningsCurrencyCode},
                {"@IncomeTaxAccrued", reportSummary.IncomeTaxAccrued},
                {"@IncomeTaxAccruedCurrencyCode", reportSummary.IncomeTaxAccruedCurrencyCode},
                {"@Assets", reportSummary.Assets},
                {"@AssetsCurrencyCode", reportSummary.AssetsCurrencyCode},
                {"@NoOfEmployees", reportSummary.NoOfEmployees},
                {"@TaxUserID", reportSummary.TaxUserID},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_Summary]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveRevenues(CBCReport_Revenue revenues)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", revenues.ID},
                {"@CbCReportSummaryId", revenues.CbCReportSummaryId},
                {"@Unrelated", revenues.Unrelated},
                {"@UnrelatedCurrencyCode", revenues.UnrelatedCurrencyCode},
                {"@Related", revenues.Related},
                {"@RelatedCurrencyCode", revenues.RelatedCurrencyCode},
                {"@Total", revenues.Total},
                {"@TotalCurrencyCode", revenues.TotalCurrencyCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_Revenue]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveConstituentEntity(CBCReport_ConstituentEntity constituentEntity)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", constituentEntity.ID},
                {"@CBCReportId", constituentEntity.CBCReportId},
                {"@NumberOfContituentEntities", constituentEntity.NumberOfContituentEntities},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_ConstituentEntity]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveConstituentEntityData(CBCReport_ConstituentEntityData entityData)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", entityData.ID},
                {"@ConstituentEntityId", entityData.ConstituentEntityId},
                {"@RegisteredName", entityData.RegisteredName},
                {"@CompanyRegNo", entityData.CompanyRegNo},
                {"@CompanyRegNoIssuedByCountry", entityData.CompanyRegNoIssuedByCountry},
                {"@TaxRefNo", entityData.TaxRefNo},
                {"@TaxRefNoIssuedByCountry", entityData.TaxRefNoIssuedByCountry},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_ConstituentEntityData]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveCBCReportAddress(CBCReport_Address reportAddress)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", reportAddress.ID},
                {"@AddressTypeCode", reportAddress.AddressTypeCode},
                {"@PostalServiceID", reportAddress.PostalServiceID},
                {"@OtherPOSpecialService", reportAddress.OtherPOSpecialService},
                {"@Number", reportAddress.Number},
                {"@PostOffice", reportAddress.PostOffice},
                {"@PAPostalCode", reportAddress.PAPostalCode},
                {"@PACountryCode", reportAddress.PACountryCode},
                {"@UnitNo", reportAddress.UnitNo},
                {"@Complex", reportAddress.Complex},
                {"@StreetNo", reportAddress.StreetNo},
                {"@StreetName", reportAddress.StreetName},
                {"@Suburb", reportAddress.Suburb},
                {"@City", reportAddress.City},
                {"@RESPostalCode", reportAddress.RESPostalCode},
                {"@RESCountryCode", reportAddress.RESCountryCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_Address]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveBusinessActivities(CBCReport_BusinessActivities businessActivities)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", businessActivities.ID},
                {"@CBCReport_ConstituentEntityDataId", businessActivities.CBCReport_ConstituentEntityDataId},
                {"@BusinessActivitiesCode", businessActivities.BusinessActivitiesCode},
                {"@OtherEntityInfo", businessActivities.OtherEntityInfo},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_BusinessActivities]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveAdditionalInfo(ReportEntityAdditionalInformation additionalInfo)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", additionalInfo.ID},
                {"@ReportEntitytId", additionalInfo.ReportEntitytId},
                {"@UniqueNo", additionalInfo.UniqueNo},
                {"@RecordStatusId", additionalInfo.RecordStatusId},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_ReportEntityAdditionalInformation]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveOtherInfo(CBCReport_OtherInformation otherInfo)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", otherInfo.ID},
                {"@AdditionalInformationId", otherInfo.AdditionalInformationId},
                {"@FurtherBriefInformation", otherInfo.FurtherBriefInformation},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_OtherInformation]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static decimal SaveCountrySummaryCodes(CBCReport_CoutryCodeSummaryRefCode countrySummaryCodes)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", countrySummaryCodes.ID},
                {"@OtherInformationId", countrySummaryCodes.OtherInformationId},
                {"@ResidenceCounryCode", countrySummaryCodes.ResidenceCounryCode},
                {"@SummaryRefCode", countrySummaryCodes.SummaryRefCode},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_CBCReport_CoutryCodeSummaryRefCode]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static void SaveCBC01FormData(CBC01Data formData)
        {
            var oParams = new DBParamCollection
            {
                {"@RegisteredName", formData.RegisteredName},
                {"@CompanyRegNo", formData.CompanyRegNo},
                {"@TaxRefNo", formData.TaxRefNo},
                {"@ReportingPeriod", formData.ReportingPeriod},
                {"@FormData", formData.FormData},

            };
            using (var oCommand = new DBCommand("[dbo].[uspINSERT_CBC01Data]", QueryType.StoredProcedure, oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                // return scopeIdentity;
            }
        }

        #endregion

        public static int SaveMasterLocalFileNotificationEmailBodyTemplates(
            MasterLocalFileNotificationEmailBodyTemplate template)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", template.Id},
                {"@CreatedBy", template.CreatedBy},
                {"@ReceivedBody", template.ReceivedBody},
                {"@AcceptedBody", template.AcceptedBody},
                {"@RejectedBody", template.RejectedBody},
                {"@AcceptedWithWarningsBody", template.AcceptedWithWarningsBody},
                {"@DateLastModified", template.DateLastModified},
                {"@LastModifiedBy", template.LastModifiedBy}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_MasterLocalFileNotificationEmailBodyTemplates]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int SaveMasterLocalFileNotificationSmsBodyTemplates(
            MasterLocalFileNotificationSmsBodyTemplate template)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", template.Id},
                {"@CreatedBy", template.CreatedBy},
                {"@ReceivedBody", template.ReceivedBody},
                {"@AcceptedBody", template.AcceptedBody},
                {"@RejectedBody", template.RejectedBody},
                {"@AcceptedWithWarningsBody", template.AcceptedWithWarningsBody},
                {"@DateLastModified", template.DateLastModified},
                {"@LastModifiedBy", template.LastModifiedBy}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_MasterLocalFileNotificationSMSBodyTemplates]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int SaveFileValidationOutcome(FileValidationOutcomeDetails outcomeDetails)
        {
            var oParams = new DBParamCollection
            {
                {"@submissionId", outcomeDetails.SubmissionId},
                {"@FileId", outcomeDetails.FileId},
                {"@ValidationOutcomeId", outcomeDetails.ValidationOutcomeId},
                {"@SID", outcomeDetails.SID},
                {"@OutcomeReason", outcomeDetails.OutcomeReason}
            };
            using (
                var oCommand = new DBCommand("[dbo].[uspINSERT_FileValidationOutcomeDetails]", QueryType.StoredProcedure,
                    oParams))
            {
                return oCommand.Execute();
            }
        }

        public static int ChangeSubmissionStatus(decimal submissionId, int status)
        {
            var oParams = new DBParamCollection
            {
                {"@submissionId", submissionId},
                {"@StatusID", status},
                {"@SID", Sars.Systems.Security.ADUser.CurrentSID}
            };
            using (var oCommand = new DBCommand("[dbo].[uspChangeSubmissionStatus]", QueryType.StoredProcedure, oParams)
                )
            {
                return oCommand.Execute();
            }
        }

        public static int UpdateUserProfile(string sid, object telephone)
        {
            var oParams = new DBParamCollection
            {
                {"@SID", sid},
                {"@TelNo", telephone}
            };
            using (var command = new DBCommand("[dbo].[uspINSERT_UserProfiles]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int SaveSentSmsCommunications(string taxRefNo, string text, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@TaxYear", year},
                {"@SMSBody", text}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_SMSCommunication]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static void TruncateMNEList()
        {
            using (var command = new DBCommand("[dbo].[uspCleanTempMNEListTemp]", QueryType.StoredProcedure, null))
            {
                command.Execute();
            }
        }

        public static int SaveLetter(decimal submissionId, string text, string taxRefNo, int year, string sId)
        {
            var oParams = new DBParamCollection
            {
                {"@SubmissionId", submissionId},
                {"@XmlData", text},
                {"@TaxRefNo", taxRefNo},
                {"@TaxYear", year},
                {"@SentBySid", sId},
            };
            using (var command = new DBCommand("[dbo].[usp_Insert_SentLetters]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int SaveCorrespondanceTrail(string messageId, string taxRefNo, int year, string message = null)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageId", messageId},
                {"@TaxRefNo", taxRefNo},
                {"@Year", year},
                {"@Message", message}
            };
            using (
                var command = new DBCommand("[dbo].[uspInsert_CorrespondanceTrail]", QueryType.StoredProcedure, oParams)
                )
            {
                return command.Execute();
            }
        }



        public static int SaveAllocationDetails(int allocationId, string submissionId, string sid)
        {
            var oParams = new DBParamCollection
            {
                {"@AllocationId", allocationId},
                {"@SubmissionId", submissionId},
                {"@AllocatedTo", sid}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_AllocationDetails]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int UpdateUserRole(string userId, string roleId)
        {
            var oParams = new DBParamCollection
            {
                {"@UserId", userId},
                {"@RoleId", roleId}
            };
            using (var command = new DBCommand("[dbo].[uspChangeUserRole]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
            //
        }

        public static int SaveMultinationalEntity(MultiNationalEntity mne)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", mne.ID},
                {"@PartyID", mne.PartyID},
                {"@TaxpayerReferenceNumber", mne.TaxpayerReferenceNumber},
                {"@YearofAssessment", mne.YearofAssessment},
                {"@RegisteredName", mne.RegisteredName},
                {"@TradingName", mne.TradingName},
                {"@RegistrationNumber", mne.RegistrationNumber},
                {"@FinancialYearEnd", mne.FinancialYearEnd},
                {"@TurnoverAmount", mne.TurnoverAmount},
                {"@NameUltimateHoldingCo", mne.NameUltimateHoldingCo},
                {"@UltimateHoldingCompanyResOutSAInd", mne.UltimateHoldingCompanyResOutSAInd},
                {"@TaxResidencyCountryCodeUltimateHoldingCompany", mne.TaxResidencyCountryCodeUltimateHoldingCompany},
                {"@UltimateHoldingCOIncomeTaxRefNo", mne.UltimateHoldingCOIncomeTaxRefNo},
                {"@MasterLocalFileRequiredInd", mne.MasterLocalFileRequiredInd},
                {"@CbCReportRequiredInd", mne.CbCReportRequiredInd},
                {"@CorrectCbCDeclarationInd", mne.CorrectCbCDeclarationInd},
                {"@CorrectMasterAndLocalFileInd", mne.CorrectMasterAndLocalFileInd},
                {"@CreatedBy", mne.CreatedBy}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_MultiNationalEntityList]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static void DeleteMultinational(decimal id, string sid)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", id},
                {"@Sid", sid}
            };
            using (
                var command = new DBCommand("[dbo].[uspDELETE_MultinationalEntity]", QueryType.StoredProcedure, oParams)
                )
            {
                command.Execute();
            }
        }

        public static decimal SaveIncomingCBCDeclaration(decimal id, string country, int taxyear, string cbcdata)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", id},
                {"@Country", country},
                {"@TaxYear", taxyear},
                {"@CBCData", cbcdata},
                {"@Return_Value", null, ParameterDirection.ReturnValue}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPSERT_IncomingCBCDeclarations]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable oHashTable;
                var scopeIdentity = 0L;
                oCommand.Execute(out oHashTable);

                if (oHashTable.Count > 0)
                {
                    scopeIdentity = Int64.Parse(oHashTable["@Return_Value"].ToString());
                }
                return scopeIdentity;
            }

        }

        public static void SaveDocRefId(decimal incomingCBCDeclerationsId, string docRefId, string docTypeIndic)
        {
            var oParams = new DBParamCollection
            {
                {"@IncomingCBCDeclerationsId", incomingCBCDeclerationsId},
                {"@DocRefId", docRefId},
                {"@DocTypeIndic", docTypeIndic}

            };
            using (var command = new DBCommand("[dbo].[uspINSERT_DocrefId]", QueryType.StoredProcedure, oParams))
            {
                command.Execute();
            }
        }

        public static void UpdateIncomingCBCDeclarationsStatusMessage(decimal incomingCBCDeclerationsId,
            string statusMessage, string messageRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@IncomingCBCDeclarationsId", incomingCBCDeclerationsId},
                {"@StatusMessage", statusMessage},
                {"@MessageRefId", messageRefId}

            };
            using (
                var command = new DBCommand("[dbo].[uspUPDATE_IncomingCBCDeclarationsStatusMessage]",
                    QueryType.StoredProcedure, oParams))
            {
                command.Execute();
            }
        }

        public static void SaveIncomingFileStatuses(decimal incomingCBCDeclerationsId, string messageRefId,
            string errorCode, string status)
        {
            var oParams = new DBParamCollection
            {
                {"@IncomimingCBCDeclarationId", incomingCBCDeclerationsId},
                {"@MessageRefId", messageRefId},
                {"@ErrorCode", errorCode},
                {"@Status", status}

            };
            using (
                var command = new DBCommand("[dbo].[usp_InsertIncomingFileStatuses]", QueryType.StoredProcedure, oParams)
                )
            {
                command.Execute();
            }
        }

        public static void SaveIncomingFileStatusesRecordErrors(decimal incomingCBCDeclerationsId, string docRefId,
            string errorCode, string status)
        {
            var oParams = new DBParamCollection
            {
                {"@IncomimingCBCDeclarationId", incomingCBCDeclerationsId},
                {"@DocRefId", docRefId},
                {"@ErrorCode", errorCode},
                {"@Status", status}

            };
            using (
                var command = new DBCommand("[dbo].[usp_InsertIncomingFileStatusesRecordErrors]",
                    QueryType.StoredProcedure, oParams))
            {
                command.Execute();
            }
        }

        public static void SaveOutgoingFileStatuses(decimal outgoingCBCDeclarationId, string messageRefId,
            string errorCode, string status)
        {
            var oParams = new DBParamCollection
            {
                {"@OutGoingCBCDeclarationId", outgoingCBCDeclarationId},
                {"@MessageRefId", messageRefId},
                {"@ErrorCode", errorCode},
                {"@Status", status}

            };
            using (
                var command = new DBCommand("[dbo].[usp_InsertOutGoingFileStatuses]", QueryType.StoredProcedure, oParams)
                )
            {
                command.Execute();
            }
        }

        public static void SaveOutgoingFileStatusesRecordErrors(decimal outgoingCBCDeclarationId, string docRefId,
            string errorCode, string status)
        {
            var oParams = new DBParamCollection
            {
                {"@OutGoingCBCDeclarationId", outgoingCBCDeclarationId},
                {"@DocRefId", docRefId},
                {"@ErrorCode", errorCode},
                {"@Status", status}

            };
            using (
                var command = new DBCommand("[dbo].[usp_InsertOutGoingFileStatusesRecordErrors]",
                    QueryType.StoredProcedure, oParams))
            {
                command.Execute();
            }
        }

        public static void ApproveOutgoingPackage(decimal Id, string countryCode, string reportingPeriod, int statusId,
            string userId)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", Id},
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod},
                {"@StatusId", statusId},
                {"@SID", userId}
            };

            using (
                var oCommand = new DBCommand("[dbo].[uspUPDATE_ValidationStatus]", QueryType.StoredProcedure, oParams))
            {
                oCommand.Execute();
            }
        }

        public static int ApproveOutgoingCBC(string countryCode, int year, int statusId, string userId, decimal packageId)
        {
            var oParams = new DBParamCollection
            {
                {"@Id", packageId},
                {"@CountryCode", countryCode},
                //{"@Year", year},
                {"@StatusId", statusId},
                {"@SID", userId}
            };
            using (var command = new DBCommand("[dbo].[uspUPDATE_ValidationStatus]", QueryType.StoredProcedure, oParams)                )
            {
                return command.Execute();
            }
        }

        public static int Insert_OutgoingPackageAuditTrail(Guid packageUid, string actionedBySid, string auditAction)
        {
            var oParams = new DBParamCollection
            {
                {"@packageUid", packageUid.ToString()},
                {"@actionedBySid", actionedBySid},
                {"@auditAction", auditAction}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_OutgoingPackageAuditTrail]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static void ApproveForeignPackage(int statusId, decimal msgSpecId, string userId)
        {
            var oParams = new DBParamCollection
            {
                {"@StatusID", statusId},
                {"@MessageSpecId", msgSpecId},
                {"@Sid", userId}
            };
            using (var command = new DBCommand("[dbo].[uspApproveForeignPackage]", QueryType.StoredProcedure, oParams))
            {
                command.Execute();
            }
        }

        public static int UpdatePackageWithStatusFromeOtherCountries(Guid packageUid, string statusXml, string status)
        {
            var oParams = new DBParamCollection
            {
                {"@UID", packageUid.ToString()},
                {"@StatusXML", statusXml},
                {"@Status", status}
            };
            using (
                var command = new DBCommand("[dbo].[uspUpdatetGoingCBCDeclarationsWithStatusMessage]",
                    QueryType.StoredProcedure, oParams))
            {
                var numRecords = command.Execute();
                return numRecords;
            }
        }

        public static int Insert_OutgoingPackage_File_ReturnErrors(Guid packageUid, string errorCode,
            string errorDetails)
        {
            var oParams = new DBParamCollection
            {
                {"@OrigionalMessageRefId", packageUid.ToString()},
                {"@ErrorCode", errorCode},
                {"@Details", errorDetails}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_OutgoingPackage_File_ReturnErrors]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int Insert_OutgoingPackage_Record_ReturnErrors(Guid packageUid, string errorCode,
            string errorDetails, string docRefID)
        {
            var oParams = new DBParamCollection
            {
                {"@OrigionalMessageRefId", packageUid.ToString()},
                {"@ErrorCode", errorCode},
                {"@Details", errorDetails},
                {"@DocRefID", docRefID}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_OutgoingPackage_Record_ReturnErrors]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int Insert_IncomingPackage_Record_ReturnErrors(Guid packageUid, string errorCode,
            string errorDetails, string docRefID)
        {
            var oParams = new DBParamCollection
            {
                {"@OrigionalMessageRefId", packageUid.ToString()},
                {"@ErrorCode", errorCode},
                {"@Details", errorDetails},
                {"@DocRefID", docRefID}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_IncomingPackageAuditTrail]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static decimal Insert_CTSSenderFileMetadata(string CTSSenderCountryCd, string CTSReceiverCountryCd,
            string CTSCommunicationTypeCd, string SenderFileId, string FileFormatCd, string FileCreateTs, string TaxYear)
        {
            var oParams = new DBParamCollection
            {
                {"@CTSSenderCountryCd", CTSSenderCountryCd},
                {"@CTSReceiverCountryCd", CTSReceiverCountryCd},
                {"@CTSCommunicationTypeCd", CTSCommunicationTypeCd},
                {"@SenderFileId", SenderFileId},
                {"@FileFormatCd", FileFormatCd},
                {"@FileCreateTs", FileCreateTs},
                {"@TaxYear", TaxYear},
                {"@Return_Value", null,ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_CTSSenderFileMetadata]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable ht;
                var ret = command.Execute(out ht);
                if (ht.ContainsKey("@Return_Value")){
                    return Convert.ToDecimal(ht["@Return_Value"]);
                }
                return -1;
            }
        }

        public static decimal Save_X_MessageSpec(
            decimal MessageSpec_ID
            , string MessageSpec
            , string TransmittingCountry
            , string ReceivingCountry
            , string MessageType
            , string Language
            , string Warning
            , string Contact
            , string MessageRefId
            , DateTime ReportingPeriod
            , string MessageTypeIndic
            , DateTime MessageTimestamp
            , string cbcXml

            )
        {
            var oParams = new DBParamCollection
            {
                {"@MessageSpec_ID", MessageSpec_ID}
                ,
                {"@MessageSpec", MessageSpec}
                ,
                {"@TransmittingCountry", TransmittingCountry}
                ,
                {"@ReceivingCountry", ReceivingCountry}
                ,
                {"@MessageType", MessageType}
                ,
                {"@Language", Language}
                ,
                {"@Warning", Warning}
                ,
                {"@Contact", Contact}
                ,
                {"@MessageRefId", MessageRefId}
                ,
                {"@ReportingPeriod", ReportingPeriod}
                ,
                {"@MessageTypeIndic", MessageTypeIndic}
                ,
                {"MessageTimestamp", MessageTimestamp}
                ,
                {"@cbcXml", cbcXml}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_UPSERT_X_MessageSpec]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_CBC_Bodies(decimal cbcBodyId, decimal messageSpecId, string cbcBodyData)
        {
            var oParams = new DBParamCollection
            {
                {"@CbcBody_ID", cbcBodyId}
                ,
                {"@MessageSpec_ID", messageSpecId}
                ,
                {"@CbcBodyData", cbcBodyData}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_UPSERT_X_CBC_Bodies]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_CBC_Bodies(
            decimal cbcBodyId
            , decimal messageSpecId
            , string cbcBodyData
            , string taxRefNo
            , int numCbCReports
            , int numConstEntities
            , string countryFrom
            , string regName
            , int taxYear
            , DateTime reportingPeriod

            )
        {
            var oParams = new DBParamCollection
            {
                {"@CbcBody_ID", cbcBodyId},
                {"@MessageSpec_ID", messageSpecId},
                {"@CbcBodyData", cbcBodyData},

                {"@TIN", taxRefNo},
                {"@NumCbCReports", numCbCReports},
                {"@numConstEntities", numConstEntities},

                {"@CountryFrom", countryFrom},
                {"@RegName", regName},
                {"@TaxYear", taxYear},
                {"@ReportingPeriod", reportingPeriod},
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[usp_UPSERT_X_CBC_Bodies]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ReportingEntity(
            decimal reportingEntityID
            , decimal cbcBodyId
            , decimal messageSpecId
            , string tin
            , string TIN_IssuedBy
            , string reportingRole
            , string docTypeIndic
            , string docRefId
            , string corrMessageRefId
            , string corrDocRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@ReportingEntityID", reportingEntityID}
                ,
                {"@CbcBody_ID", cbcBodyId}
                ,
                {"@MessageSpec_ID", messageSpecId}
                ,
                {"@TIN", tin}
                ,
                {"@TIN_IssuedBy", TIN_IssuedBy}
                ,
                {"@ReportingRole", reportingRole}
                ,
                {"@DocTypeIndic", docTypeIndic}
                ,
                {"@DocRefId", docRefId}
                ,
                {"@CorrMessageRefId", corrMessageRefId}
                ,
                {"@CorrDocRefId", corrDocRefId}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ReportingEntity]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ReportingEntity_Entity_ResCountryCode(
            decimal id
            , string code
            , decimal reportingEntityID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@Code", code}
                ,
                {"@ReportingEntityID", reportingEntityID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ReportingEntity_Entity_ResCountryCode]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ReportingEntity_Entity_Identification_Numbers(
            decimal id
            , string IN
            , string issuedBy
            , string INType
            , decimal reportingEntityID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@IN", IN}
                ,
                {"@IssuedBy", issuedBy}
                ,
                {"@INType", INType}
                ,
                {"@ReportingEntityID", reportingEntityID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ReportingEntity_Entity_Identification_Numbers]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ReportingEntity_Entity_Names(
            decimal id
            , string name
            , decimal reportingEntityID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@Name", name}
                ,
                {"@ReportingEntityID", reportingEntityID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ReportingEntity_Entity_Names]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_CbcReports( 
            decimal CbcReportID
            , string docSpec_DocTypeIndic
            , string docSpec_DocRefId
            , string docSpec_CorrMessageRefId
            , string docSpecCorrDocRefId
            , string resCountryCode
            , decimal messageSpec_ID
            , string reportData
            , decimal cbcBodyId
            )
        {
            var oParams = new DBParamCollection
            {
                {"@CbcReportID", CbcReportID}
                ,
                {"@DocSpec_DocTypeIndic", docSpec_DocTypeIndic}
                ,
                {"@DocSpec_DocRefId", docSpec_DocRefId}
                ,
                {"@DocSpec_CorrMessageRefId", docSpec_CorrMessageRefId}
                ,
                {"@DocSpecCorrDocRefId", docSpecCorrDocRefId}
                ,
                {"@ResCountryCode", resCountryCode}
                ,
                {"@MessageSpec_ID", messageSpec_ID}
                ,
                {"@ReportData", reportData}
                , {"@CbcBody_ID", cbcBodyId}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("uspUPSERT_X_CbcReports", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_Summaries(
            decimal CbcReportSummaryID
            , decimal CbcReportID
            , string ProfitOrLoss
            , string TaxPaid
            , string TaxAccrued
            , string Capital
            , string Earnings
            , string NbEmployees
            , string Assets
            , string Revenues_Unrelated
            , string Revenues_Related
            , string Revenues_Total
            )
        {
            var oParams = new DBParamCollection
            {
                {"@CbcReportSummaryID", CbcReportSummaryID}
                ,
                {"@CbcReportID", CbcReportID}
                ,
                {"@ProfitOrLoss", ProfitOrLoss}
                ,
                {"@TaxPaid", TaxPaid}
                ,
                {"@TaxAccrued", TaxAccrued}
                ,
                {"@Capital", Capital}
                ,
                {"@Earnings", Earnings}
                ,
                {"@NbEmployees", NbEmployees}
                ,
                {"@Assets", Assets}
                ,
                {"@Revenues_Unrelated", Revenues_Unrelated}
                ,
                {"@Revenues_Related", Revenues_Related}
                ,
                {"@Revenues_Total", Revenues_Total}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("uspUPSERT_X_Summaries", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            } 
        }

        public static decimal X_AdditionalInfo_ResCountryCode(decimal id, string countryCodeType, decimal additionalInfoId)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id},
                {"@ResCountryCode", countryCodeType},
                {"@AdditionalInfoID", additionalInfoId},
                {"@Retur_Values", null, ParameterDirection.ReturnValue }
            };
            using (var command = new DBCommand("[dbo].[uspUPSERT_X_AdditionalInfo_ResCountryCode]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_AdditionalInfo_SummaryRef(decimal id, string summary, decimal additionalInfoId)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id},
                {"@ResCountryCode", summary},
                {"@AdditionalInfoID", additionalInfoId},
                {"@Retur_Values", null, ParameterDirection.ReturnValue }
            };
            using (var command = new DBCommand("[dbo].[uspUPSERT_X_AdditionalInfo_SummaryRef]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_AdditionalInfo(decimal additionalInfoId, string docTypeIndic, string docRefId, string corrMessageRefId, string corrDocRefId, string otherInfo, decimal messageSpecId)
        {
            var oParams = new DBParamCollection
            {
                {"@AdditionalInfoID", additionalInfoId},
                {"@DocTypeIndic", docTypeIndic},
                {"@DocRefId", docRefId},
                {"@CorrMessageRefId", corrMessageRefId},
                {"@CorrDocRefId", corrDocRefId},
                {"@OtherInfo", otherInfo},
                {"@MessageSpec_ID", messageSpecId },
                {"@Retur_Values", null, ParameterDirection.ReturnValue }

            };
            using (var command = new DBCommand("[dbo].[uspUPSERT_X_AdditionalInfo]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ConstEntities(
            decimal ConstEntitYID
            , decimal CbcReportID
            , string TIN
            , string IncorpCountryCode
            , string OtherEntityInfo
            )
        {
            var oParams = new DBParamCollection
            {
                {"@CbcReportID", CbcReportID}
                ,
                {"@ConstEntitYID", ConstEntitYID}
                ,
                {"@TIN", TIN}
                ,
                {"@IncorpCountryCode", IncorpCountryCode}
                ,
                {"@OtherEntityInfo", OtherEntityInfo}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (var command = new DBCommand("[dbo].[uspUPSERT_X_ConstEntities]", QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ConstEntities_ConstEntity_ResCountryCode(decimal id, string resCountryCode,
            decimal ConstEntitYID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@ResCountryCode", resCountryCode}
                ,
                {"@ConstEntitYID", ConstEntitYID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ConstEntities_ConstEntity_ResCountryCode]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ConstEntities_ConstEntity_Identification_Numbers(decimal id, string IN,
            string IssuedBy, string INType, decimal ConstEntitYID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@IN", IN}
                ,
                {"@IssuedBy", IssuedBy}
                ,
                {"@INType", INType}
                ,
                {"@ConstEntitYID", ConstEntitYID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ConstEntities_ConstEntity_Identification_Numbers]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ConstEntities_ConstEntity_Names(decimal id, string name, decimal ConstEntitYID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@Name", name}
                ,
                {"@ConstEntitYID", ConstEntitYID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ConstEntities_ConstEntity_Names]",
                    QueryType.StoredProcedure, oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static decimal Save_X_ConstEntities_BizActivities(decimal id, string ActivityName, decimal ConstEntitYID)
        {
            var oParams = new DBParamCollection
            {
                {"@ID", id}
                ,
                {"@ActivityName", ActivityName}
                ,
                {"@ConstEntitYID", ConstEntitYID}
                ,
                {"@Retur_Values", null, ParameterDirection.ReturnValue}
            };
            using (
                var command = new DBCommand("[dbo].[uspUPSERT_X_ConstEntities_BizActivities]", QueryType.StoredProcedure,
                    oParams))
            {
                Hashtable ht;
                command.Execute(out ht);
                if (ht.ContainsKey("@Retur_Values"))
                {
                    return Convert.ToDecimal(ht["@Retur_Values"]);
                }
                return 0;
            }
        }

        public static int Update_X_MessageSpec_FinalStatus(decimal messageSpecId, string finalStatus, bool isCorrection, string originalMessageRefId)
        {
            var oParams = new DBParamCollection
            {
                  {"@MessageSpec_ID", messageSpecId}
                , {"@FinalStatus", finalStatus}
                , {"@IsCorrection", isCorrection }
                , {"@OriginalMessageRefId", originalMessageRefId }
            };
            using (
                var command = new DBCommand("[dbo].[usp_UPDATE_X_MessageSpec_FinalStatus]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static int Update_X_MessageSpec_FinalStatus(decimal messageSpecId, string finalStatus)
        {
            var oParams = new DBParamCollection
            {
                  {"@MessageSpec_ID", messageSpecId}
                , {"@FinalStatus", finalStatus}
                , {"@IsCorrection", false }
                , {"@OriginalMessageRefId",  null}
            };
            using (
                var command = new DBCommand("[dbo].[usp_UPDATE_X_MessageSpec_FinalStatus]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static int Insert_X_CBC_RecordValidations(decimal messageSpecId, string errorCode, string docRefId,
            string errorMessage)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageSpec_ID", messageSpecId}
                ,
                {"@DocRefID", docRefId}
                ,
                {"@ErrorCode", errorCode}
                ,
                {"@Mesasge", errorMessage}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_X_CBC_RecordValidations]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

        public static int Insert_X_CBC_FileValidations(decimal messageSpecId, string errorCode, string errorMessage)
        {
            var oParams = new DBParamCollection
            {
                {"@MessageSpec_ID", messageSpecId}
                ,
                {"@ErrorCode", errorCode}
                ,
                {"@Mesasge", errorMessage}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_X_CBC_FileValidations]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }

    public static int Insert_X_Masages_Failed_Validation(string From, string XML, string MessageRefID, string SenderFileId,string reportingPeriod, string Reason)
        {
            var oParams = new DBParamCollection
            {
                {"@From", From}   ,
                {"@XML", XML}                ,
                {"@MessageRefID", MessageRefID},
                {"@SenderFileId", SenderFileId}  ,
                {"@ReportingPeriod",reportingPeriod },
                {"@Reason", Reason}
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_X_Masages_Failed_Validation]", QueryType.StoredProcedure,
                    oParams))
            {
                return command.Execute();
            }
        }


        public static int UpdatePackageWithNewMessage(Guid messageUid, string newXmlPacage, string messageRefId)
        {
            var oParams = new DBParamCollection
            {
                {"@UID", messageUid}
                ,
                {"@Xml", newXmlPacage}
                ,
                {"@MessageRefId", messageRefId}
            };
            using (
                var command = new DBCommand("[dbo].[usp_UPDATE_OutGoingCBCDeclarations_WithNewPackage]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static int InsertOutgoingCBCAudit(OutGoingCBCDeclarations package)
        {
            var oParams = new DBParamCollection
            {
                {"@Country", package.Country}
                ,
                {"@StatusId", package.StatusId}
                ,
                {"@Year", package.Year}
                ,
                {"@ActionedBy", package.UpdatedBy}
                ,
                {"@DateActioned", package.DateUpdated}
                ,
                {"@CBCData ", package.NMCBC}
                ,
                {"@ReportingPeriod", package.ReportingPeriod}
                ,
                {"@ReturningStatus", package.ReturningStatus}
                ,
                {"@ActionId", package.ActionId},
            };
            using (
                var command = new DBCommand("[dbo].[usp_INSERT_Audit_OutGoingCBCDeclarations]",
                    QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

    }
}