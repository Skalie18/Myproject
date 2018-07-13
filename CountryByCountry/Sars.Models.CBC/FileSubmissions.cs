using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

namespace Sars.Models.CBC
{
    [Table(Name = "FileSubmissions")]
    public class FileSubmission : Sars.Systems.Data.DataAccessObject
    {
        public FileSubmission()
        {
        }

        public FileSubmission(string procedure, Dictionary<string, object> parameters) : base(procedure, parameters)
        {
        }

        [Column(Name = "SubmissionId"), DBQueryParameter("@SubmissionId")]
        public decimal SubmissionId { get; set; }

        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }

        [Column(Name = "TaxRefNo"), DBQueryParameter("@TaxRefNo")]
        public string TaxRefNo { get; set; }

        [Column(Name = "Year"), DBQueryParameter("@Year")]
        public int Year { get; set; }

        [Column(Name = "Version"), DBQueryParameter("@Version")]
        public int? Version { get; set; }

        [Column(Name = "SubmissionStatusId"), DBQueryParameter("@SubmissionStatusId")]
        public int SubmissionStatusId { get; set; }

        [Column(Name = "LastModifiedBySID"), DBQueryParameter("@LastModifiedBySID")]
        public string LastModifiedBySID { get; set; }

        [Column(Name = "DateLastModified"), DBQueryParameter("@DateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [Column(Name = "StatusDescription")]
        public string StatusDescription { get; set; }

        [Column(Name = "ContactCell")]
        public string ContactMobileNumber { get; set; }
        [Column(Name = "ContactEmail")]
        public string ContactEmail { get; set; }
    }
}