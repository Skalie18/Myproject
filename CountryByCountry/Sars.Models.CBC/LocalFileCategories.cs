using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;
namespace Sars.Models.CBC
{
    [Table(Name = "LocalFileCategories")]
    public class LocalFileCategories : Sars.Systems.Data.DataAccessObject
    {
        public LocalFileCategories()
        {
        }
        public LocalFileCategories(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "Id"), DBQueryParameter("@Id")]
        public int Id { get; set; }
        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }
        [Column(Name = "Description"), DBQueryParameter("@Description")]
        public string Description { get; set; }
        [Column(Name = "IsActive"), DBQueryParameter("@IsActive")]
        public bool IsActive { get; set; }
    }
}