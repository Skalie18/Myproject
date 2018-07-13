using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Sars.Systems.Data;
using System.Collections.Generic;

namespace Sars.Models.CBC
{
    [Table(Name = "MasterLocalFileNotificationSMSBodyTemplates")]
    public class MasterLocalFileNotificationSmsBodyTemplate : Sars.Systems.Data.DataAccessObject
    {
        public MasterLocalFileNotificationSmsBodyTemplate()
        {
        }

        public MasterLocalFileNotificationSmsBodyTemplate(string procedure, Dictionary<string, object> parameters)
            : base(procedure, parameters)
        {
        }

        [Column(Name = "Id"), DBQueryParameter("@Id")]
        public int Id { get; set; }

        [Column(Name = "Timestamp"), DBQueryParameter("@Timestamp")]
        public DateTime Timestamp { get; set; }

        [Column(Name = "CreatedBy"), DBQueryParameter("@CreatedBy")]
        public string CreatedBy { get; set; }

        [Column(Name = "ReceivedBody"), DBQueryParameter("@ReceivedBody")]
        public string ReceivedBody { get; set; }

        [Column(Name = "AcceptedBody"), DBQueryParameter("@AcceptedBody")]
        public string AcceptedBody { get; set; }

        [Column(Name = "RejectedBody"), DBQueryParameter("@RejectedBody")]
        public string RejectedBody { get; set; }

        [Column(Name = "AcceptedWithWarningsBody"), DBQueryParameter("@AcceptedWithWarningsBody")]
        public string AcceptedWithWarningsBody { get; set; }

        [Column(Name = "DateLastModified"), DBQueryParameter("@DateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [Column(Name = "LastModifiedBy"), DBQueryParameter("@LastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [Column(Name = "IsActive"), DBQueryParameter("@IsActive")]
        public bool IsActive { get; set; }
    }
}