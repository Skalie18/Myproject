using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sars.Models.CBC
{
    [Table(Name = "ReportingRoles")]
    public class ReportingRoles : Sars.Systems.Data.DataAccessObject
    {
        public ReportingRoles()
        {
        }
        public ReportingRoles(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }
        [Column(Name = "RoleID"), DBQueryParameter("@RoleID")]
        public string RoleID { get; set; }
        [Column(Name = "Description"), DBQueryParameter("@Description")]
        public string Description { get; set; }
        [Column(Name = "IsActive"), DBQueryParameter("@IsActive")]
        public bool IsActive { get; set; }
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
