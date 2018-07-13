using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "ReportEntityAdditionalInformation")]
    public class ReportEntityAdditionalInformation : Sars.Systems.Data.DataAccessObject
    {
        public ReportEntityAdditionalInformation()
        {
        }
        public ReportEntityAdditionalInformation(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID{get; set;}
        [Column(Name = "ReportEntitytId"), DBQueryParameter("@ReportEntitytId")]
        public decimal ReportEntitytId{get; set;}
        [Column(Name = "UniqueNo"), DBQueryParameter("@UniqueNo")]
        public string UniqueNo{get; set;}
        [Column(Name = "RecordStatusId"), DBQueryParameter("@RecordStatusId")]
        public int RecordStatusId{get; set;}

        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
