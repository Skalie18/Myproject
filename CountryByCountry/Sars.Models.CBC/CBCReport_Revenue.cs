using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_Revenue")]
    public class CBCReport_Revenue : Sars.Systems.Data.DataAccessObject
    {
        public CBCReport_Revenue()
        {
        }
        public CBCReport_Revenue(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID {get; set;}
        [Column(Name = "CbCReportSummaryId"), DBQueryParameter("@CbCReportSummaryId")]
        public decimal CbCReportSummaryId { get; set;}
        [Column(Name = "Unrelated"), DBQueryParameter("@Unrelated")]
        public decimal Unrelated {get; set;}
        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public string UnrelatedCurrencyCode {get; set;}
        [Column(Name = "Related"), DBQueryParameter("@Related")]
        public decimal Related {get; set;}
        [Column(Name = "RelatedCurrencyCode"), DBQueryParameter("@RelatedCurrencyCode")]
        public string RelatedCurrencyCode {get; set;}
        [Column(Name = "Total"), DBQueryParameter("@Total")]
        public decimal Total {get; set;}
        [Column(Name = "TotalCurrencyCode"), DBQueryParameter("@TotalCurrencyCode")]
        public string TotalCurrencyCode {get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp {get; set;}
    }
}
