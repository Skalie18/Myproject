using Sars.Systems.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDatalayer
{
    public static class DatabaseReader
    {
        public static OutGoingCBCDeclarations OutGoingCBCDeclarationsDetails(string countryCode, int year)
        {
            var oParams = new DBParamCollection
            {
                {"@CountryCode", countryCode},
                {"@Year", year}
            };
            var result = new OutGoingCBCDeclarations("[dbo].[uspREAD_OutGoingCBCDeclarationsDetails]", new Dictionary<string, object> { { "@CountryCode", countryCode }, { "@Year", year } });
            return result.GetRecord<OutGoingCBCDeclarations>();
        }
    }
}
