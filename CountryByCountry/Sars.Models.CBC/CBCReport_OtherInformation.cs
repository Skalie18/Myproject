using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_OtherInformation")]

    public class CBCReport_OtherInformation : Sars.Systems.Data.DataAccessObject
    {
        public CBCReport_OtherInformation()
        {
        }
        public CBCReport_OtherInformation(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID{get; set;}
        [Column(Name = "AdditionalInformationId"), DBQueryParameter("@AdditionalInformationId")]
        public decimal AdditionalInformationId {get; set;}
        [Column(Name = "FurtherBriefInformation"), DBQueryParameter("@FurtherBriefInformation")]
        public string FurtherBriefInformation{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }
}
