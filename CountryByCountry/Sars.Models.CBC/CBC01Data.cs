using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "CBC01Data")]
    public class CBC01Data : Sars.Systems.Data.DataAccessObject
    {
        public CBC01Data()
        {
        }
        public CBC01Data(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set; }
        [Column(Name = "RegisteredName"), DBQueryParameter("@RegisteredName")]
        public string RegisteredName { get; set; }
        [Column(Name = "CompanyRegNo"), DBQueryParameter("@CompanyRegNo")]
        public string CompanyRegNo { get; set; }
        [Column(Name = "TaxRefNo"), DBQueryParameter("@TaxRefNo")]
        public string TaxRefNo { get; set; }
        [Column(Name = "ReportingPeriod"), DBQueryParameter("@ReportingPeriod")]
        public string ReportingPeriod { get; set; }
        [Column(Name = "FormData"), DBQueryParameter("@FormData")]
        public string FormData { get; set; }
        [DBQueryParameter("@Year")]
        public int? TaxYear { get; set; }
        [Column(Name = "TimeStamp"), DBQueryParameter("@TimeStamp")]
        public DateTime? TimeStamp { get; set; }
    }
}