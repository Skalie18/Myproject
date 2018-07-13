using System;
using System.Collections;
using System.Data;
using Sars.Models.CBC;
using Sars.Systems.Data;

public static class DatabaseWriter
{


    public static int UpdateMne(string cbcRequired, string masterLocalRequired, string mneID)
    {
        var oParams = new DBParamCollection
        {
            {"@CBCRequired", cbcRequired},
            {"@MasterLocalRequired", masterLocalRequired},
            {"@EntityID", mneID}
        };
        using (var command = new DBCommand("usp_UPDATE_MultiNationalEntityList", QueryType.StoredProcedure, oParams))
        {
            return command.Execute();
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

    public static RecordSet UpdateCBCStatus(int status, string taxRefNo, int year, string userId)
    {
        var oParams = new DBParamCollection
        {
            {"@Status", status},
            {"@TaxRefNo", taxRefNo},
            {"@TaxYear", year},
            {"@Sid", userId}
        };

        return new RecordSet("[dbo].[uspApproveCBC]", QueryType.StoredProcedure, oParams);

        /*using (var oCommand = new DBCommand("[dbo].[uspApproveCBC]", QueryType.StoredProcedure, oParams))
        {
            oCommand.Execute();
        }*/
    }

    public static int SaveNewEntity(decimal partyId, string taxpayerReferenceNumber, int yearofAssessment, string registeredName,
        string tradingName, string registrationNumber, DateTime financialYearEnd, decimal turnoverAmount,
        string nameUltimateHoldingCo, string ultimateHoldingCompanyResOutSaInd,
        string taxResidencyCountryCodeUltimateHoldingCompany, string ultimateHoldingCoIncomeTaxRefNo,
        string masterLocalFileRequiredInd, string cbCReportRequiredInd, DateTime datestamp)
    {
        var oParams = new DBParamCollection
        {
            {"@PartyId", partyId},
            {"@TaxpayerReferenceNumber", taxpayerReferenceNumber},
            {"@YearofAssessment", yearofAssessment},
            {"@RegisteredName", registeredName},
            {"@TradingName", tradingName},
            {"@RegistrationNumber", registrationNumber},
            {"@FinancialYearEnd", financialYearEnd},
            {"@TurnoverAmount", turnoverAmount},
            {"@NameUltimateHoldingCo", nameUltimateHoldingCo},
            {"@UltimateHoldingCompanyResOutSAInd", ultimateHoldingCompanyResOutSaInd},
            {"@TaxResidencyCountryCodeUltimateHoldingCompany", taxResidencyCountryCodeUltimateHoldingCompany},
            {"@UltimateHoldingCOIncomeTaxRefNo", ultimateHoldingCoIncomeTaxRefNo},
            {"@MasterLocalFileRequiredInd", masterLocalFileRequiredInd},
            {"@CbCReportRequiredInd", cbCReportRequiredInd},
            {"@Datestamp", datestamp},
            {"@Return_Value", null, ParameterDirection.ReturnValue}
        };
        using (
            var oCommand = new DBCommand("[dbo].[uspINSERT_MultiNationalEntityList]", QueryType.StoredProcedure, oParams)
            )
        {
            Hashtable oHashTable;
            var scope_identity = 0;
            scope_identity = oCommand.Execute(out oHashTable);
            if (oHashTable.Count > 0 && oHashTable.ContainsKey("@Return_Value"))
            {
                scope_identity = int.Parse(oHashTable["@Return_Value"].ToString());
            }
            return scope_identity;
        }
    }

    public static void UpdateViewedCBC(decimal Id)
    {
        var oParams = new DBParamCollection
        {
            {"@ID", Id}
        };

        using (var oCommand = new DBCommand("[dbo].[uspUpdateViewCBC01Declarations]", QueryType.StoredProcedure, oParams))
        {
            oCommand.Execute();
        }
    }

    public static void UpdateOutgoingCBCMsgSpec(decimal Id, string XML)
    {
        var oParams = new DBParamCollection
        {
            {"@ID", Id},
            {"@CbcData", XML}
        };

        using (var oCommand = new DBCommand("[dbo].[uspUPDATE_OutgoingCBCMsgSpec]", QueryType.StoredProcedure, oParams))
        {
            oCommand.Execute();
        }
    }

    public static decimal SaveOutgoingCBC(OutGoingCBCDeclarations cbcDeclarations, ref Guid newUid)
    {
        var oParams = new DBParamCollection
            {
                {"@Id", cbcDeclarations.Id},
                {"@Country", cbcDeclarations.Country},
                {"@StatusId",cbcDeclarations.StatusId },
                {"@CBCData", cbcDeclarations.CBCData},
                {"@CreatedBy", cbcDeclarations.CreatedBy},
                {"@CBCWithNaspace", cbcDeclarations.NSCBCData},
                {"@ActionId",cbcDeclarations.ActionId },
                {"@Year", cbcDeclarations.Year },
                {"@ReportingPeriod", cbcDeclarations.ReportingPeriod},
                {"@NewIdentity", null, ParameterDirection.Output},
                {"@UID", null, ParameterDirection.Output}
            };
        using (var command = new DBCommand("[dbo].[uspUPSERT_OutGoingCBCDeclarations]", QueryType.StoredProcedure, oParams))
        {
            Hashtable oHashTable;
            var scopeIdentity = 0L;
            command.Execute(out oHashTable);

            if (oHashTable.Count > 0)
            {
                if (!string.IsNullOrEmpty(oHashTable["@NewIdentity"].ToString()))
                    scopeIdentity = long.Parse(oHashTable["@NewIdentity"].ToString());
                if (!string.IsNullOrEmpty(oHashTable["@UID"].ToString()))
                    newUid = Guid.Parse(oHashTable["@UID"].ToString());
            }
            return scopeIdentity;
            //return command.Execute();
        }
    }

    public static int B(decimal id, string countryCode, string reportingPeriod, int statusId, string userId)
    {
        var oParams = new DBParamCollection
            {
                 {"@Id", id},
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod},
                {"@StatusId",statusId},
                {"@SID",userId}
            };
        using (var command = new DBCommand("[dbo].[uspUPDATE_ValidationStatus]", QueryType.StoredProcedure, oParams))
        {
            return command.Execute();
        }
    }

    public static int ApproveIncomingCBC(string countryCode, string reportingPeriod, int statusId, string userId)
    {
        var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod},
                {"@StatusId",statusId},
                {"@SID",userId}
            };
        using (var command = new DBCommand("[dbo].[uspUPDATE_IncomingValidationStatus]", QueryType.StoredProcedure, oParams))
        {
            return command.Execute();
        }
    }
    public static int LogEmailToDB(EmailLogs email)
    {
        var oParams = new DBParamCollection
            {
                {"@SendBy", email.SendBy},
                {"@SendTo",email.SendTo},
                {"@EmailMessage",email.EmailMessage},
                {"@EmailSubject", email.EmailSubject}
            };
        using (var command = new DBCommand("[dbo].[upsINSERT_EmailLogs]", QueryType.StoredProcedure, oParams))
        {
            return command.Execute();
        }
    }


    public static int SaveCaseDetails(CaseDetails casedetails)
    {
        var oParams = new DBParamCollection
            {
           {"@TaxRefNo",casedetails.TaxRefNo},
           {"@Year",casedetails.Year},
           {"@CaseNotes",casedetails.CaseNotes},
           {"@DateCreated",casedetails.DateCreated},
           {"@CaseNo",casedetails.CaseNo},
           {"@EntityName",casedetails.EntityName},
           {"@RequestorUnit",casedetails.RequestorUnit},
           {"@DateRequested",casedetails.DateRequested},
           {"@CountryName",casedetails.CountryName},
           {"@CountryCode",casedetails.CountryCode},
           {"@DateRecieved",casedetails.DateRecieved},

            };
        using (var command = new DBCommand("[dbo].[usp_CaseDetailsInsert]", QueryType.StoredProcedure, oParams))

        {
            return command.Execute();
        }
    }

    public static int SaveUploadedFiles(CaseDetailsUploadedFiles uploads)
    {
        var oParams = new DBParamCollection
            {
           {"@TaxRefNo",uploads.TaxRefNo},
           {"@CaseNo",uploads.CaseNo},
           {"@FileName",uploads.FileName},
           {"@ObjectId",uploads.ObjectId},
           {"@FilePath",uploads.FilePath},
           {"@FileSize",uploads.FileSize},
           {"@Message",uploads.Message},
           {"@Owner",uploads.Owner},
           {"@DocumentumDate",uploads.DocumentumDate},
           {"@UploadedBy",uploads.UploadedBy},
           {"@Timestamp",uploads.Timestamp}


            };
        using (var command = new DBCommand("[dbo].[usp_CaseDetailsUploadedFilesInsert]", QueryType.StoredProcedure, oParams))

        {
            return command.Execute();
        }
    }

    public static int SaveComments(Comments comments, int type)
    {
        var oParams = new DBParamCollection
        {
            {"@OutGoingCBCDeclarationsID", comments.OutGoingCBCDeclarationsID},
            {"@Notes", comments.Notes},
            {"@AddedBy", comments.AddedBy},
            {"@Type", type}
        };

        using (var oCommand = new DBCommand("[dbo].[uspINSERT_COMMENTS]", QueryType.StoredProcedure, oParams))
        {
            return oCommand.Execute();
        }
    }

    public static int UpdateCBCRS_To_Process(string taxRefNo, string reportingPeriod)
    {
        var oParams = new DBParamCollection
        {
            {"@TaxRefNo", taxRefNo},
            {"@ReportingPeriod", reportingPeriod}

        };

        using (var oCommand = new DBCommand("[dbo].[uspUPDATE_CBCRS_To_ProcessedIndicator]", QueryType.StoredProcedure, oParams))
        {
            return oCommand.Execute();
        }
    }

}