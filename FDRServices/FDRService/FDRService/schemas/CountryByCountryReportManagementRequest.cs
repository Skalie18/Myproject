using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDRService
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.domain.com/test")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/SARSCountryByCountryReportManagement/xml/schemas/version/1.1", IsNullable = false)]
    public class CountryByCountryReportManagementRequest
    {
        public string RequestOperation;

        public string DestinationCountry;

        public string Filename;

        public string FileContent;
    }
}
