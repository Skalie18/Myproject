using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReports")]
    public class CBCReports : Sars.Systems.Data.DataAccessObject
    {
        public CBCReports()
        {
        }
        public CBCReports(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set;}
        [Column(Name = "ReportEntityId"), DBQueryParameter("@ReportEntityId")]
        public decimal ReportEntityId {get; set;}
        [Column(Name = "NumberOfTaxJurisdictions"), DBQueryParameter("@NumberOfTaxJurisdictions")]
        public int NumberOfTaxJurisdictions{get; set;}
        [Column(Name = "UniqueNo"), DBQueryParameter("@UniqueNo")]
        public string UniqueNo{get; set;}
        [Column(Name = "RecordStatusId"), DBQueryParameter("@RecordStatusId")]
        public int RecordStatusId{get; set;}
        [Column(Name = "ResidentCountryCode"), DBQueryParameter("@ResidentCountryCode")]
        public string ResidentCountryCode{get; set;}
        [Column(Name = "CurrencyCode"), DBQueryParameter("@CurrencyCode")]
        public string CurrencyCode{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }
}
