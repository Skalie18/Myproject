using System;
using System.Collections.Generic;
using Sars.Systems.Data;
using Sars.Systems.Security;

namespace FRD_MNE_Services
{
    public static class DbReader
    {
        public static RecordSet Enquire(string taxRefNo, string year)
        {
            long _year;
            if (!long.TryParse(year, out _year))
            {
                return new RecordSet();
            }
            return new RecordSet("[dbo].[uspEnquireMNE]", QueryType.StoredProcedure,
                new DBParamCollection {{"@TaxRefNo", taxRefNo}, {"@Year", year}});
        }

        public static string GetNextMessageId()
        {
            using (var command = new DBCommand("usp_GetMessageId", QueryType.StoredProcedure, null))
            {
                var id = command.ExecuteScalar();

                if (id != null)
                {
                    return id.ToString();
                }
                var random = new Random(1000);
                return random.Next(int.MaxValue, int.MaxValue).ToString();
            }
        }

        public static int SaveCBCRequest(string cbddata, string taxRefNo, int year, string surname, string firstName, string businessTel, string cellNo, string emailAddress, string postalAddress)
        {
            var oParams = new DBParamCollection
            {
                {"@Data", cbddata},
                {"@TaxRefNo", taxRefNo},
                {"@TaxYear", year},
                {"@Surname", surname},
                {"@FirstNames", firstName},
                {"@BusTelNo", businessTel},
                {"@CellNo", cellNo},
                {"@EmailAddress", emailAddress},
                {"@PostalAddress", postalAddress}
            };
            using (var command = new DBCommand("[dbo].[usp_INSERT_CBCDeclarations]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }

        public static IEnumerable<Users> GetUsersInARole(string roleName)
        {
            return
                new Users("uspGetUsersByRole", new Dictionary<string, object> { { "@RoleName", roleName } })
                    .GetRecordsLazy<Users>();
        }

        public static bool CanSubmitCbcDeclaration(string taxRefNo, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@Year", year}
            };
            using (var command = new DBCommand("[dbo].[uspGetCBCDeclarationVersion]", QueryType.StoredProcedure, oParams))
            {
                var version = command.ExecuteScalar();
                if (version != null)
                    return Convert.ToInt32(version) <= AppConfig.CbcRevisionCount;
                return false;
            }
        }
        public static bool CanSubmitFileDeclaration(string taxRefNo, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@TaxRefNo", taxRefNo},
                {"@Year", year}
            };
            using (var command = new DBCommand("[dbo].[uspGetFileSubmissionRevisions]", QueryType.StoredProcedure, oParams))
            {
                var version = command.ExecuteScalar();
                if (version != null)
                    return Convert.ToInt32(version) <= AppConfig.FileRevisionCount; 
                return false;
            }
        }

        public static RecordSet GetRolesToNotify()
        {
            return new RecordSet("[dbo].[uspGetUsersByRole]", QueryType.StoredProcedure, new DBParamCollection { { "@RoleName" , AppConfig.NotifyRole} });
        }
    }
}
