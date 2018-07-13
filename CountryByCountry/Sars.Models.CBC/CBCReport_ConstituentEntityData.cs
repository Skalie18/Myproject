using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_ConstituentEntityData")]
    public class CBCReport_ConstituentEntityData : Sars.Systems.Data.DataAccessObject
    {
        public CBCReport_ConstituentEntityData()
        {

        }
        public CBCReport_ConstituentEntityData(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID{get; set;}
        [Column(Name = "ConstituentEntityId"), DBQueryParameter("@ConstituentEntityId")]
        public decimal ConstituentEntityId {get; set;}
        [Column(Name = "RegisteredName"), DBQueryParameter("@RegisteredName")]
        public string RegisteredName{get; set;}
        [Column(Name = "CompanyRegNo"), DBQueryParameter("@CompanyRegNo")]
        public string CompanyRegNo{get; set;}
        [Column(Name = "CompanyRegNoIssuedByCountry"), DBQueryParameter("@CompanyRegNoIssuedByCountry")]
        public string CompanyRegNoIssuedByCountry{get; set;}
        [Column(Name = "TaxRefNo"), DBQueryParameter("@TaxRefNo")]
        public string TaxRefNo{get; set;}
        [Column(Name = "TaxRefNoIssuedByCountry"), DBQueryParameter("@TaxRefNoIssuedByCountry")]
        public string TaxRefNoIssuedByCountry{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }
}
