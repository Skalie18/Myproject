using Sars.Systems.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDatalayer
{
    public static class DatabaseWriter
    {
        public static int ApproveOutgoingCBC(string countryCode, int year, int statusId, string userId)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@Year", year},
                {"@StatusId",statusId},
                {"@SID",userId}
            };
            using (var command = new DBCommand("[dbo].[uspUPDATE_ValidationStatus]", QueryType.StoredProcedure, oParams))
            {
                return command.Execute();
            }
        }
    }
}
