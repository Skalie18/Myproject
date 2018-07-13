using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_ConstituentEntity")]
    public class CBCReport_ConstituentEntity : Sars.Systems.Data.DataAccessObject
    {

        public CBCReport_ConstituentEntity()
        {
        }
        public CBCReport_ConstituentEntity(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID{get; set;}
        [Column(Name = "CBCReportId"), DBQueryParameter("@CBCReportId")]
        public decimal CBCReportId{get; set;}
        [Column(Name = "NumberOfContituentEntities"), DBQueryParameter("@NumberOfContituentEntities")]
        public int NumberOfContituentEntities{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }
}
