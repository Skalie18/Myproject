using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "ReportEntity")]
    public class ReportEntity : Sars.Systems.Data.DataAccessObject
    {
        public ReportEntity()
        {
        }
        public ReportEntity(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set; }
        [Column(Name = "RegisteredName"), DBQueryParameter("@RegisteredName")]
        public string RegisteredName { get; set; }
        [Column(Name = "CompanyRegNo"), DBQueryParameter("@CompanyRegNo")]
        public string CompanyRegNo { get; set; }
        [Column(Name = "CompanyRegNoIssuedByCountry"), DBQueryParameter("@CompanyRegNoIssuedByCountry")]
        public string CompanyRegNoIssuedByCountry { get; set; }
        [Column(Name = "TaxRefNo"), DBQueryParameter("@TaxRefNo")]
        public string TaxRefNo { get; set; }
        [Column(Name = "TaxRefNoIssuedByCountry"), DBQueryParameter("@TaxRefNoIssuedByCountry")]
        public string TaxRefNoIssuedByCountry { get; set; }
        [Column(Name = "GIINNoIndicator"), DBQueryParameter("@GIINNoIndicator")]
        public string GIINNoIndicator { get; set; }
        [Column(Name = "GIINNo"), DBQueryParameter("@GIINNo")]
        public string GIINNo { get; set; }
        [Column(Name = "GIINNoIssuedByCountry"), DBQueryParameter("@GIINNoIssuedByCountry")]
        public string GIINNoIssuedByCountry { get; set; }
        [Column(Name = "ReportingRoleId"), DBQueryParameter("@ReportingRoleId")]
        public string ReportingRoleId { get; set; }
        [Column(Name = "ResidentCountryCode"), DBQueryParameter("@ResidentCountryCode")]
        public string ResidentCountryCode { get; set; }
        [Column(Name = "UniqueNo"), DBQueryParameter("@UniqueNo")]
        public string UniqueNo { get; set; }
        [Column(Name = "RecordStatusId"), DBQueryParameter("@RecordStatusId")]
        public int? RecordStatusId { get; set; }
        [Column(Name = "ReportEntityContactPersonDetailsId"), DBQueryParameter("@ReportEntityContactPersonDetailsId")]
        public decimal? ReportEntityContactPersonDetailsId { get; set; }
        [Column(Name = "ReportEntityAddressId"), DBQueryParameter("@ReportEntityAddressId")]
        public decimal ReportEntityAddressId { get; set; }
        [Column(Name = "ReportingPeriod"), DBQueryParameter("@ReportingPeriod")]
        public string ReportingPeriod { get; set; }
        [Column(Name = "TimeStamp"), DBQueryParameter("@TimeStamp")]
        public DateTime TimeStamp { get; set; }
        [Column(Name = "TaxUserID"), DBQueryParameter("@TaxUserID")]
        public string TaxUserID { get; set; }
    }
}