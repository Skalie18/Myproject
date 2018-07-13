using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "ReportEntityAddress")]
    public class EntityAddress : Sars.Systems.Data.DataAccessObject
    {
        public EntityAddress()
        {
        }
        public EntityAddress(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set; }
        [Column(Name = "AddressTypeCode"), DBQueryParameter("@AddressTypeCode")]
        public string AddressTypeCode { get; set; }
        [Column(Name = "PostalServiceID"), DBQueryParameter("@PostalServiceID")]
        public string PostalServiceID { get; set; }
        [Column(Name = "OtherPOSpecialService"), DBQueryParameter("@OtherPOSpecialService")]
        public string OtherPOSpecialService { get; set; }
        [Column(Name = "Number"), DBQueryParameter("@Number")]
        public string Number { get; set; }
        [Column(Name = "PostOffice"), DBQueryParameter("@PostOffice")]
        public string PostOffice { get; set; }
        [Column(Name = "PAPostalCode"), DBQueryParameter("@PAPostalCode")]
        public string PAPostalCode { get; set; }
        [Column(Name = "PACountryCode"), DBQueryParameter("@PACountryCode")]
        public string PACountryCode { get; set; }
        [Column(Name = "UnitNo"), DBQueryParameter("@UnitNo")]
        public string UnitNo { get; set; }
        [Column(Name = "Complex"), DBQueryParameter("@Complex")]
        public string Complex { get; set; }
        [Column(Name = "StreetNo"), DBQueryParameter("@StreetNo")]
        public string StreetNo { get; set; }
        [Column(Name = "StreetName"), DBQueryParameter("@StreetName")]
        public string StreetName { get; set; }
        [Column(Name = "Suburb"), DBQueryParameter("@Suburb")]
        public string Suburb { get; set; }
        [Column(Name = "City"), DBQueryParameter("@City")]
        public string City { get; set; }
        [Column(Name = "RESPostalCode"), DBQueryParameter("@RESPostalCode")]
        public string RESPostalCode { get; set; }
        [Column(Name = "RESCountryCode"), DBQueryParameter("@RESCountryCode")]
        public string RESCountryCode { get; set; }
        [Column(Name = "TimeStamp"), DBQueryParameter("@TimeStamp")]
        public DateTime? TimeStamp { get; set; }
    }
}