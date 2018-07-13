using System;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

namespace Sars.Models.CBC
{
    [Table(Name = "FileValidationOutcomeDetails")]
    public class FileValidationOutcomeDetails : DataAccessObject
    {
        public FileValidationOutcomeDetails()
        {
        }

        public FileValidationOutcomeDetails(string procedure, Dictionary<string, object> parameters)
            : base(procedure, parameters)
        {
        }

        [Column(Name = "Id"), DBQueryParameter("@Id")]
        public decimal Id { get; set; }

        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }

        [Column(Name = "SubmissionId"), DBQueryParameter("@SubmissionId")]
        public decimal SubmissionId { get; set; }

        [Column(Name = "FileId"), DBQueryParameter("@FileId")]
        public decimal FileId { get; set; }

        [Column(Name = "ValidationOutcomeId"), DBQueryParameter("@ValidationOutcomeId")]
        public int ValidationOutcomeId { get; set; }

        [Column(Name = "SID"), DBQueryParameter("@SID")]
        public string SID { get; set; }

        [Column(Name = "LastUpdatedBySid"), DBQueryParameter("@LastUpdatedBySid")]
        public string LastUpdatedBySid { get; set; }

        [Column(Name = "DateLastUpdated"), DBQueryParameter("@DateLastUpdated")]
        public DateTime? DateLastUpdated { get; set; }

        [Column(Name = "OutcomeReason")]
        public string OutcomeReason { get; set; }
    }
}