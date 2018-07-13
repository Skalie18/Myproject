using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "ReportEntityContactPersonDetails")]
    public class ReportEntityContactPersonDetails : Sars.Systems.Data.DataAccessObject
    {
        public ReportEntityContactPersonDetails()
        {
        }
        public ReportEntityContactPersonDetails(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set; }
        [Column(Name = "FirstNames"), DBQueryParameter("@FirstNames")]
        public string FirstNames { get; set; }
        [Column(Name = "Surname"), DBQueryParameter("@Surname")]
        public string Surname { get; set; }
        [Column(Name = "BusTelNo1"), DBQueryParameter("@BusTelNo1")]
        public string BusTelNo1 { get; set; }
        [Column(Name = "BusTelNo2"), DBQueryParameter("@BusTelNo2")]
        public string BusTelNo2 { get; set; }
        [Column(Name = "CellNo"), DBQueryParameter("@CellNo")]
        public string CellNo { get; set; }
        [Column(Name = "EmailAddress"), DBQueryParameter("@EmailAddress")]
        public string EmailAddress { get; set; }
        [Column(Name = "TimeStamp"), DBQueryParameter("@TimeStamp")]
        public DateTime? TimeStamp { get; set; }
    }
}