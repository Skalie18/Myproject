using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

[Table(Name = "MultiNationalEntityList")]
public class MultiNationalEntity : Sars.Systems.Data.DataAccessObject
{
    public MultiNationalEntity()
    {
    }
    public MultiNationalEntity(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
    {
    }

    [Column(Name = "Id"), DBQueryParameter("@Id")]
    public decimal Id { get; set; }
    [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
    public DateTime Timestamp { get; set; }
    [Column(Name = "PartyId"), DBQueryParameter("@PartyId")]
    public decimal? PartyId { get; set; }
    [Column(Name = "TaxpayerReferenceNumber"), DBQueryParameter("@TaxpayerReferenceNumber")]
    public string TaxpayerReferenceNumber { get; set; }
    [Column(Name = "YearofAssessment"), DBQueryParameter("@YearofAssessment")]
    public int? YearofAssessment { get; set; }
    [Column(Name = "RegisteredName"), DBQueryParameter("@RegisteredName")]
    public string RegisteredName { get; set; }
    [Column(Name = "TradingName"), DBQueryParameter("@TradingName")]
    public string TradingName { get; set; }
    [Column(Name = "RegistrationNumber"), DBQueryParameter("@RegistrationNumber")]
    public string RegistrationNumber { get; set; }
    [Column(Name = "FinancialYearEnd"), DBQueryParameter("@FinancialYearEnd")]
    public DateTime? FinancialYearEnd { get; set; }
    [Column(Name = "TurnoverAmount"), DBQueryParameter("@TurnoverAmount")]
    public decimal? TurnoverAmount { get; set; }
    [Column(Name = "NameUltimateHoldingCo"), DBQueryParameter("@NameUltimateHoldingCo")]
    public string NameUltimateHoldingCo { get; set; }
    [Column(Name = "UltimateHoldingCompanyResOutSAInd"), DBQueryParameter("@UltimateHoldingCompanyResOutSAInd")]
    public string UltimateHoldingCompanyResOutSAInd { get; set; }
    [Column(Name = "TaxResidencyCountryCodeUltimateHoldingCompany"), DBQueryParameter("@TaxResidencyCountryCodeUltimateHoldingCompany")]
    public string TaxResidencyCountryCodeUltimateHoldingCompany { get; set; }
    [Column(Name = "UltimateHoldingCOIncomeTaxRefNo"), DBQueryParameter("@UltimateHoldingCOIncomeTaxRefNo")]
    public string UltimateHoldingCOIncomeTaxRefNo { get; set; }
    [Column(Name = "MasterLocalFileRequiredInd"), DBQueryParameter("@MasterLocalFileRequiredInd")]
    public string MasterLocalFileRequiredInd { get; set; }
    [Column(Name = "CbCReportRequiredInd"), DBQueryParameter("@CbCReportRequiredInd")]
    public string CbCReportRequiredInd { get; set; }
    [Column(Name = "Datestamp"), DBQueryParameter("@Datestamp")]
    public DateTime? Datestamp { get; set; }

    [Column(Name = "CorrectMasterAndLocalFileInd"), DBQueryParameter("@CorrectMasterAndLocalFileInd")]
    public string CorrectMasterAndLocalFileInd { get; set; }
    [Column(Name = "CorrectCbCDeclarationInd"), DBQueryParameter("@CorrectCbCDeclarationInd")]
    public string CorrectCbCDeclarationInd { get; set; }
    [Column(Name = "CreatedBy"), DBQueryParameter("@CreatedBy")]
    public string CreatedBy { get; set; }
    [Column(Name = "UpdatedBy"), DBQueryParameter("@UpdatedBy")]
    public string UpdatedBy { get; set; }
    [Column(Name = "DateUpdated"), DBQueryParameter("@DateUpdated")]
    public DateTime DateUpdated { get; set; }

    [Column]
    public long RowNumber { get; set; }
    public int Save()
    {
        var oParams = new DBParamCollection{
            { "@PartyId", this.PartyId},
            {"@TaxpayerReferenceNumber", this.TaxpayerReferenceNumber},
            {"@YearofAssessment", this.YearofAssessment},
            {"@RegisteredName", this.RegisteredName},
            {"@TradingName", this.TradingName},
            {"@RegistrationNumber", this.RegistrationNumber},
            {"@FinancialYearEnd", this.FinancialYearEnd},
            {"@TurnoverAmount", this.TurnoverAmount},
            {"@NameUltimateHoldingCo", this.NameUltimateHoldingCo},
            {"@UltimateHoldingCompanyResOutSAInd", this.UltimateHoldingCompanyResOutSAInd},
            {"@TaxResidencyCountryCodeUltimateHoldingCompany", this.TaxResidencyCountryCodeUltimateHoldingCompany},
            {"@UltimateHoldingCOIncomeTaxRefNo", this.UltimateHoldingCOIncomeTaxRefNo},
            {"@MasterLocalFileRequiredInd", this.MasterLocalFileRequiredInd},
            {"@CbCReportRequiredInd", this.CbCReportRequiredInd},
            {"@Datestamp", this.Datestamp},
            {"@Return_Value", null, System.Data.ParameterDirection.ReturnValue}
        };

        using ( var oCommand = new DBCommand("[dbo].[uspINSERT_MultiNationalEntityList]", QueryType.StoredProcedure, oParams) )
        {
            System.Collections.Hashtable oHashTable;

            int scope_identity = 0;
            scope_identity = oCommand.Execute(out oHashTable);
            if ( oHashTable.Count > 0 && oHashTable.ContainsKey("@Return_Value") )
            {
                scope_identity = int.Parse(oHashTable["@Return_Value"].ToString());
            }
            return scope_identity;
        }
    }

    public bool MNEExists()
    {
        var results = new RecordSet("[DBO].[usp_READ_MNEExist]", QueryType.StoredProcedure, new DBParamCollection { { "@TaxRefNo", TaxpayerReferenceNumber } });
        var exists = Convert.ToInt16(results.Tables[0].Rows[0]["ExistRecord"].ToString());
        return (Convert.ToBoolean(exists));
    }
}