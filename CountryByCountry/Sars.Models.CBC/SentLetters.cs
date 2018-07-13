using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;

namespace Sars.Models.CBC
{
    [Table(Name="SentLetters")]
    public class SentLetters : Sars.Systems.Data.DataAccessObject
    {
        public SentLetters()
        {
        }
        public SentLetters(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name="Id"), DBQueryParameter("@Id")]
        public decimal Id { get; set; }
        [Column(Name="Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }
        [Column(Name="XmlData"), DBQueryParameter("@XmlData")]
        public string XmlData { get; set; }
        [Column(Name="SubmissionId"), DBQueryParameter("@SubmissionId")]
        public decimal SubmissionId { get; set; }
        [Column(Name="SentBySid"), DBQueryParameter("@SentBySid")]
        public string SentBySid { get; set; }
        [Column(Name="TaxRefNo"), DBQueryParameter("@TaxRefNo")]
        public string TaxRefNo { get; set; }
        [Column(Name="Year"), DBQueryParameter("@Year")]
        public int? Year { get; set; }
        [Column]
        public string FullName { get; set; }
    }

    [Table(Name = "")]
    public class FdrUserDetails : Sars.Systems.Data.DataAccessObject
    {
        public FdrUserDetails()
        {
        }

        public FdrUserDetails(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column]
        public string FirstName { get; set; }
        [Column]
        public string LastName { get; set; }
        [Column]
        public string FullName { get; set; }
        [Column]
        public string SID { get; set; }
        [Column]
        public Guid UserIdGuid { get; set; }
        [Column]
        public Guid RuleId { get; set; }
    }
}