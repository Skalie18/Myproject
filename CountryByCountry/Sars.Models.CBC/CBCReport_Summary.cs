using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_Summary")]
    public class CBCReport_Summary : Sars.Systems.Data.DataAccessObject
    {

        public CBCReport_Summary()
        {
        }
        public CBCReport_Summary(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set;}
        [Column(Name = "CBCReportId"), DBQueryParameter("@CBCReportId")]
        public decimal CBCReportId {get; set;}
        [Column(Name = "ProfitLossBeforeIncomeTax"), DBQueryParameter("@ProfitLossBeforeIncomeTax")]
        public decimal ProfitLossBeforeIncomeTax {get; set;}
        [Column(Name = "ProfitLossBeforeIncomeTaxCurrencyCode"), DBQueryParameter("@ProfitLossBeforeIncomeTaxCurrencyCode")]
        public string ProfitLossBeforeIncomeTaxCurrencyCode {get; set;}
        [Column(Name = "StatedCapital"), DBQueryParameter("@StatedCapital")]
        public decimal StatedCapital {get; set;}
        [Column(Name = "StatedCapitalCurrencyCode"), DBQueryParameter("@StatedCapitalCurrencyCode")]
        public string StatedCapitalCurrencyCode {get; set;}
        [Column(Name = "IncomeTaxPaid"), DBQueryParameter("@IncomeTaxPaid")]
        public decimal IncomeTaxPaid {get; set;}
        [Column(Name = "IncomeTaxPaidCurrencyCode"), DBQueryParameter("@IncomeTaxPaidCurrencyCode")]
        public string IncomeTaxPaidCurrencyCode {get; set;}
        [Column(Name = "AccumulatedEarnings"), DBQueryParameter("@AccumulatedEarnings")]
        public decimal AccumulatedEarnings {get; set;}
        [Column(Name = "AccumulatedEarningsCurrencyCode"), DBQueryParameter("@AccumulatedEarningsCurrencyCode")]
        public string AccumulatedEarningsCurrencyCode {get; set;}
        [Column(Name = "IncomeTaxAccrued"), DBQueryParameter("@IncomeTaxAccrued")]
        public decimal IncomeTaxAccrued {get; set;}
        [Column(Name = "IncomeTaxAccruedCurrencyCode"), DBQueryParameter("@IncomeTaxAccruedCurrencyCode")]
        public string IncomeTaxAccruedCurrencyCode {get; set;}
        [Column(Name = "Assets"), DBQueryParameter("@Assets")]
        public decimal Assets {get; set;}
        [Column(Name = "AssetsCurrencyCode"), DBQueryParameter("@AssetsCurrencyCode")]
        public string AssetsCurrencyCode {get; set;}
        [Column(Name = "NoOfEmployees"), DBQueryParameter("@NoOfEmployees")]
        public decimal NoOfEmployees {get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp {get; set;}
        [Column(Name = "TaxUserID"), DBQueryParameter("@TaxUserID")]
        public string TaxUserID {get; set;}
    }
}
