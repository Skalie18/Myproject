using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "MultiNationalEntityList")]
    public class MultiNationalEntity : Sars.Systems.Data.DataAccessObject
    {
        public MultiNationalEntity()
        {
        }
        public MultiNationalEntity(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set; }
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }
        [Column(Name = "PartyID"), DBQueryParameter("@PartyID")]
        public decimal? PartyID { get; set; }
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

        [Column(Name = "RowNumber"), DBQueryParameter("@RowNumber")]
        public long RowNumber { get; set; }
    }
}