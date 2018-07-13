using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name = "CBCReport_Address")]
    public class CBCReport_BusinessActivities : Sars.Systems.Data.DataAccessObject
    {
        public CBCReport_BusinessActivities()
        {
        }
        public CBCReport_BusinessActivities(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "ID"), DBQueryParameter("@ID")]
        public decimal ID{get; set;}
        [Column(Name = "CBCReport_ConstituentEntityDataId"), DBQueryParameter("@CBCReport_ConstituentEntityDataId")]
        public decimal CBCReport_ConstituentEntityDataId{get; set;}
        [Column(Name = "BusinessActivitiesCode"), DBQueryParameter("@BusinessActivitiesCode")]
        public string BusinessActivitiesCode{get; set;}
        [Column(Name = "OtherEntityInfo"), DBQueryParameter("@OtherEntityInfo")]
        public string OtherEntityInfo{get; set;}
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp{get; set;}
    }

   
}
