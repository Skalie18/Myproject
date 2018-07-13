using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Sars.Systems.Frd.Registration;

namespace RegistrationData
{
    /// <summary>
    /// Summary description for RegData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RegData : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetRegData(string taxRefNo)
        {
            var reg = new RegistrationManagementRequestStructure
            {
                RequestOperation = RegistrationManagementRequestStructureRequestOperation.RETRIEVE_ENTITY_DETAILS,
                PartyIdentifiers = new[]
                {
                    new RegistrationManagementRequestStructurePartyIdentifier
                    {
                        IdentifierType = "REFERENCE_NO",
                        Value = taxRefNo
                    }
                }
            };


            return null;
        }
    }



}
