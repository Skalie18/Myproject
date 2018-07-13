﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace FDRService.CbcXML.CountryByCountryReportManagement
{
    using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.81.0.
// 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/" +
                    "xml/schemas/version/1.2")]
    [System.Xml.Serialization.XmlRootAttribute("CountryByCountryReportManagementRequest",
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/" +
                    "xml/schemas/version/1.2", IsNullable = false)]
    public partial class CountryByCountryReportManagementRequestStructure
    {

        private CountryByCountryReportManagementRequestStructureRequestOperation requestOperationField;

        private bool requestOperationFieldSpecified;

        private string destinationField;

        private string filenameField;

        private string fileContentField;

        /// <remarks/>
        public CountryByCountryReportManagementRequestStructureRequestOperation RequestOperation
        {
            get { return this.requestOperationField; }
            set { this.requestOperationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RequestOperationSpecified
        {
            get { return this.requestOperationFieldSpecified; }
            set { this.requestOperationFieldSpecified = value; }
        }

        /// <remarks/>
        public string Destination
        {
            get { return this.destinationField; }
            set { this.destinationField = value; }
        }

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        public string FileContent
        {
            get { return this.fileContentField; }
            set { this.fileContentField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/" +
                    "xml/schemas/version/1.2")]
    public enum CountryByCountryReportManagementRequestStructureRequestOperation
    {

        /// <remarks/>
        SUBMIT_REPORT,

        /// <remarks/>
        UPDATE_REPORT_STATUS,

        /// <remarks/>
        RECEIVE_REPORT,

        /// <remarks/>
        ISSUE_REPORT_STATUS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/" +
                    "xml/schemas/version/1.2")]
    [System.Xml.Serialization.XmlRootAttribute("CountryByCountryReportManagementResponse",
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CountryByCountryReportManagement/" +
                    "xml/schemas/version/1.2", IsNullable = false)]
    public partial class CountryByCountryReportManagementResponseStructure
    {

        private string filenameField;

        private string fileContentField;

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        public string FileContent
        {
            get { return this.fileContentField; }
            set { this.fileContentField = value; }
        }
    }
}