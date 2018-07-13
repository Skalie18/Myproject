using System;
using System.Collections.Generic;
using System.Linq;
using Sars.Systems.Data;
using Sars.Models.CBC;

public static class DatabaseReader
{
    public static List<MultiNationalEntity> GetMultiNationalEntities(string taxRefNo = null)
    {
        return
            new MultiNationalEntity("usp_READ_MultiNationalEntityList",
                new Dictionary<string, object> { { "@TaxNo", taxRefNo } }).GetRecordsLazy<MultiNationalEntity>().ToList();
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

    public static RecordSet GetCBCDataByTaxRefNo(string searchText, int year)
    {
        var oParams = new DBParamCollection
        {
            {"@SearchText", searchText},
            {"@Year", year},
        };

        return new RecordSet("[dbo].[uspSearch_CBC01DataByTRC]", QueryType.StoredProcedure, oParams);
    }

    public static int CheckIfCBCApproved(string taxRefNo, int year)
    {
        int approved = 0;
        var oParams = new DBParamCollection
        {
            {"@TaxRefNo", taxRefNo},
            {"@TaxYear", year},
        };

        var results = new RecordSet("[dbo].[uspCheckIfApproved]", QueryType.StoredProcedure, oParams);
        if (results.HasRows)
        {
            approved = int.Parse(results.Tables[0].Rows[0]["IsApproved"].ToString());
        }
        return approved;
    }

    public static RecordSet GetAllCbcData()
    {
        return new RecordSet("[dbo].[uspREAD_CBC01Data]", QueryType.StoredProcedure, null);
    }

    public static RecordSet GetUnprocessedCbcData()
    {
        return new RecordSet("[dbo].[uspGetUnprocessedCBCData]", QueryType.StoredProcedure, null);
    }




    public static RecordSet GetRolesToNotify(string role)
    {
        return new RecordSet("[dbo].[uspGetUsersByRole]", QueryType.StoredProcedure, new DBParamCollection { { "@RoleName", role } });
    }

    public static RecordSet GetCbcDeclarations(int viewed)
    {
        var _viewed = viewed <= 0 ? DBNull.Value : (object)viewed;
        return new RecordSet("[dbo].[uspGetNewCBC01Declarations]", QueryType.StoredProcedure,
            new DBParamCollection { { "@Viewed", viewed } });
    }

    public static OutGoingCBCDeclarations OutGoingCBCDeclarationsDetails(string countryCode, string reportingPeriod)
    {
        var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod}
            };
        var result = new OutGoingCBCDeclarations("[dbo].[uspREAD_OutGoingCBCDeclarationsDetails]", new Dictionary<string, object> { { "@CountryCode", countryCode }, { "@ReportingPeriod", reportingPeriod } });
        if (result.Id>0)
        return result.GetRecord<OutGoingCBCDeclarations>();
        else
            return null;
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

    public static RecordSet ForeignIncomingPackage(string countryCode, string period, int statusId = 0)
    {
        var oParams = new DBParamCollection
            {
                {"@TransmittingCountry", countryCode},
                {"@ReportingPeriod", period},
                {"@StatusId", statusId}
            };
        return new RecordSet("[dbo].[uspREAD_ForeignIncomingPackage]", QueryType.StoredProcedure, oParams);

    }

    public static RecordSet OutgoingCBCRDeclarationsHistory(string countryCode, string reportingPeriod)
    {
        var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod}
            };
        return new RecordSet("[dbo].[uspREAD_OutgoingCBCRDeclarationsHistory]", QueryType.StoredProcedure, oParams);
    }

    public static RecordSet IncomingCBCRDeclarationsHistory(string countryCode, string reportingPeriod)
    {
        var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@ReportingPeriod", reportingPeriod}
            };
        return new RecordSet("[dbo].[uspREAD_IncomingCBCRDeclarationsHistory]", QueryType.StoredProcedure, oParams);
    }

    public static Comments GetCommentsByCBCRId(string OutGoingCBCDeclarationsID)
    {
        return
            new Comments("uspREAD_COMMENTSByCBCRId", new Dictionary<string, object> { { "@OutGoingCBCDeclarationsID", OutGoingCBCDeclarationsID } })
                .GetRecord<Comments>();
    }

    public static RecordSet GetCommentsForCountryByYear(decimal packageId, int type)
    {
        var oParams = new DBParamCollection
            {
                {"@Id", packageId},
                {"@Type", type}
            };
        return new RecordSet("[dbo].[uspREAD_COMMENTS_ForCountryANdYear]", QueryType.StoredProcedure, oParams);
    }

    public static RecordSet getCasedetails(string TaxRefNo)
    {

        var oParams = new DBParamCollection
            {
             { "@TaxRefNo", TaxRefNo }
            };

        return new RecordSet("[dbo].[usp_CaseDetailsSelect]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet getUploadedCaseFiles(string TaxRefNo, string CaseNo)
    {

        var oParams = new DBParamCollection
            {
                {"@TaxRefNo", TaxRefNo},
                   {"@CaseNo", CaseNo},
            };

        return new RecordSet("[dbo].[usp_CaseDetailsUploadedFilesSelect]", QueryType.StoredProcedure, oParams);


    }


    public static RecordSet getUploadedCaseFilesById(int Id)
    {

        var oParams = new DBParamCollection
            {
                {"@Id", Id}
            };

        return new RecordSet("[dbo].[usp_CaseDetailsUploadedFilesSelectById]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet GetAllCountries()
    {
        return new RecordSet("[dbo].[uspREAD_ALLCOUNRTY]", QueryType.StoredProcedure, null);

    }

    public static RecordSet GetAllSentPackageCountries()
    {
        return new RecordSet("[dbo].[uspREAD_ALL_SENT_PACKAGE_COUNRTY]", QueryType.StoredProcedure, null);

    }

    public static RecordSet GetAllForeignCountries()
    {
        return new RecordSet("[dbo].[usp_READ_ForeignCountries]", QueryType.StoredProcedure, null);

    }

    public static RecordSet getIncomingFileStatuses(string countryCode)//, string startDate, string endDate)
    {

        var oParams = new DBParamCollection
            {
              { "@CountryCode", countryCode }
             //{ "@StartDate", startDate },
             //{ "@EndDate", endDate }
            };

        return new RecordSet("[dbo].[usp_GetIncomingFileStatuses]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet getIncomingFileRecordErrors(string countryCode, decimal id)
    {

        var oParams = new DBParamCollection
            {
             { "@CountryCode", countryCode },
              { "@IncomimingCBCDeclarationId", id }
            };

        return new RecordSet("[dbo].[usp_GetIncomingFileRecordErrors]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet GetOutgoingFileStatuses(string countryCode)
    {
        var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},


                //{"@StartDate", startDate},
                //{"@EndDate", endDate}

            };
        return new RecordSet("[dbo].[usp_READOutgoingFileResponses]", QueryType.StoredProcedure, oParams);
    }

    public static RecordSet GetOutgoingFileErrors(string uid)
    {

        var oParams = new DBParamCollection
            {
             { "@OrigionalMessageRefId", uid },
              //{ "@OutGoingCBCDeclarationId", id }
            };

        return new RecordSet("[dbo].[usp_READ_OutgoingPackage_File_ReturnErrors]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet GetOutgoingReecordErrors(string packageUid)
    {

        var oParams = new DBParamCollection
            {
             { "@OrigionalMessageRefId", packageUid },

            };

        return new RecordSet("[dbo].[usp_READ_OutgoingPackage_Record_ReturnErrors]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet OutgoingPackageAuditTrailByUid(string uid)
    {

        var oParams = new DBParamCollection
            {
             { "@PackageUID", uid },
              //{ "@OutGoingCBCDeclarationId", id }
            };

        return new RecordSet("[dbo].[usp_READ_OutgoingPackageAuditTrail]", QueryType.StoredProcedure, oParams);


    }

    public static RecordSet GetCBCDataByTaxRefNo(int incomingForeign,string searchText=null, int year=0, decimal cbcBodyId = 0)
    {
        var oParams = new DBParamCollection
        {
            {"@SearchText", searchText},
            {"@Year", year},
        };

        string query = "[dbo].[uspSearch_CBC01DataByTRC]";
        if (incomingForeign == 0)
        {
            oParams = new DBParamCollection
        {
            {"@CbcBody_ID", cbcBodyId},
        };

            query = "[dbo].[uspREAD_IncomingForeignCBCReports]";
        }

        return new RecordSet(query, QueryType.StoredProcedure, oParams);
    }

    public static RecordSet Get_X_MessageSpec_ByCountry(string countryCode)
    {

        var oParams = new DBParamCollection
            {
              { "@CountryCode", countryCode }

            };

        return new RecordSet("[dbo].[usp_READ_IncomingFileStatuses]", QueryType.StoredProcedure, oParams);


    }


}