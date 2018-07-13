using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_CoutryCodeSummaryRefCode")]
    public class CBCReport_CoutryCodeSummaryRefCode : Sars.Systems.Data.DataAccessObject
    {
        public CBCReport_CoutryCodeSummaryRefCode()
        {
        }
        public CBCReport_CoutryCodeSummaryRefCode(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID { get; set;}
        [Column(Name = "OtherInformationId"), DBQueryParameter("@OtherInformationId")]
        public decimal OtherInformationId {get; set;}
        [Column(Name = "ResidenceCounryCode"), DBQueryParameter("@ResidenceCounryCode")]
        public string ResidenceCounryCode{get; set;}
        [Column(Name = "SummaryRefCode"), DBQueryParameter("@SummaryRefCode")]
        public string SummaryRefCode{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }
}
