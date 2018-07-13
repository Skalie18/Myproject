﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 

namespace Sars.ContentManagement
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8")]
    [System.Xml.Serialization.XmlRootAttribute("ContentManagementRequest",Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/version/1.8", IsNullable = false)]
    public partial class SupportingDocumentManagementRequestStructure
    {

        private RequestOperationType requestOperationField;

        private SupportingDocumentStructure[] contentsField;

        /// <remarks/>
        public RequestOperationType RequestOperation
        {
            get { return this.requestOperationField; }
            set { this.requestOperationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Content", IsNullable = false)]
        public SupportingDocumentStructure[] Contents
        {
            get { return this.contentsField; }
            set { this.contentsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    public enum RequestOperationType
    {

        /// <remarks/>
        RECEIVE_NEW_DOCUMENT,

        /// <remarks/>
        RECEIVE_COPY_DOCUMENT,

        /// <remarks/>
        NOTIFY_SUPPORTING_DOCUMENT,

        /// <remarks/>
        NOTIFY_UNSOLICITED_SUPPORTING_DOCUMENT,

        /// <remarks/>
        NOTIFY_TRANSACTION_WITH_METADATA,

        /// <remarks/>
        APPEND_DOCUMENT_METADATA,

        /// <remarks/>
        ENQUIRE_DOCUMENT,

        /// <remarks/>
        SEARCH,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    public partial class SupportingDocumentStructure
    {

        private string requestorIdentifierField;

        private string yearOfAssessmentField;

        private string periodField;

        private string[] caseNumbersField;

        private string[] contentCategories1Field;

        private string[] contentCategories2Field;

        private string[] contentTypesField;

        private IdentifierStructure[] entityIdentifiersField;

        private System.DateTime dateOfDigitalizationField;

        private bool dateOfDigitalizationFieldSpecified;

        private string contentLocationField;

        private string[] productsField;

        private string informationSubClassField;

        private string informationSubClass2Field;

        private string informationSubClass3Field;

        private SupportingDocumentStructureSpecialUsersGroup[] specialUsersGroupsField;

        private string sourceChannelField;

        private string sourceIDField;

        private string subjectAreaField;

        private int restrictedContentField;

        private bool restrictedContentFieldSpecified;

        private System.DateTime dateOfReceiptField;

        private bool dateOfReceiptFieldSpecified;

        private byte[] binaryDataField;

        private string objectTypeField;

        private string objectNameField;

        private string pathField;

        private string formatField;

        private string usernameField;

        private string passwordField;

        private SupportingDocumentStructureTransferMode transferModeField;

        private bool transferModeFieldSpecified;

        private string aclNameField;

        private string documentIDField;

        private string batchIDField;

        private string classificationMethodField;

        private string dataExtractMethodField;

        private string processStatusField;

        private string actualPageListField;

        private int expectedPageCountField;

        private bool expectedPageCountFieldSpecified;

        private IdentifierStructure[] documentIdentifiersField;

        private string originatingLocationField;

        private string geoLocationField;

        private string procedureCategoryCodeField;

        private string[] uniqueIdentifiersField;

        private int primaryCopyIndField;

        private bool primaryCopyIndFieldSpecified;

        private string formIDField;

        private string boxNumberField;

        private string sourceNumberField;

        private string destinationNumberField;

        private System.DateTime callDateField;

        private bool callDateFieldSpecified;

        private System.DateTime callDurationField;

        private bool callDurationFieldSpecified;

        private string retainField;

        private string callReferenceNumberField;

        private MetadataStructure metadataField;

        /// <remarks/>
        public string RequestorIdentifier
        {
            get { return this.requestorIdentifierField; }
            set { this.requestorIdentifierField = value; }
        }

        /// <remarks/>
        public string YearOfAssessment
        {
            get { return this.yearOfAssessmentField; }
            set { this.yearOfAssessmentField = value; }
        }

        /// <remarks/>
        public string Period
        {
            get { return this.periodField; }
            set { this.periodField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CaseNumber", IsNullable = false)]
        public string[] CaseNumbers
        {
            get { return this.caseNumbersField; }
            set { this.caseNumbersField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ContentCategory1", IsNullable = false)]
        public string[] ContentCategories1
        {
            get { return this.contentCategories1Field; }
            set { this.contentCategories1Field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ContentCategory2", IsNullable = false)]
        public string[] ContentCategories2
        {
            get { return this.contentCategories2Field; }
            set { this.contentCategories2Field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ContentType", IsNullable = false)]
        public string[] ContentTypes
        {
            get { return this.contentTypesField; }
            set { this.contentTypesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("EntityIdentifier", IsNullable = false)]
        public IdentifierStructure[] EntityIdentifiers
        {
            get { return this.entityIdentifiersField; }
            set { this.entityIdentifiersField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DateOfDigitalization
        {
            get { return this.dateOfDigitalizationField; }
            set { this.dateOfDigitalizationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateOfDigitalizationSpecified
        {
            get { return this.dateOfDigitalizationFieldSpecified; }
            set { this.dateOfDigitalizationFieldSpecified = value; }
        }

        /// <remarks/>
        public string ContentLocation
        {
            get { return this.contentLocationField; }
            set { this.contentLocationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Product", IsNullable = false)]
        public string[] Products
        {
            get { return this.productsField; }
            set { this.productsField = value; }
        }

        /// <remarks/>
        public string InformationSubClass
        {
            get { return this.informationSubClassField; }
            set { this.informationSubClassField = value; }
        }

        /// <remarks/>
        public string InformationSubClass2
        {
            get { return this.informationSubClass2Field; }
            set { this.informationSubClass2Field = value; }
        }

        /// <remarks/>
        public string InformationSubClass3
        {
            get { return this.informationSubClass3Field; }
            set { this.informationSubClass3Field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SpecialUsersGroup", IsNullable = false)]
        public SupportingDocumentStructureSpecialUsersGroup[] SpecialUsersGroups
        {
            get { return this.specialUsersGroupsField; }
            set { this.specialUsersGroupsField = value; }
        }

        /// <remarks/>
        public string SourceChannel
        {
            get { return this.sourceChannelField; }
            set { this.sourceChannelField = value; }
        }

        /// <remarks/>
        public string SourceID
        {
            get { return this.sourceIDField; }
            set { this.sourceIDField = value; }
        }

        /// <remarks/>
        public string SubjectArea
        {
            get { return this.subjectAreaField; }
            set { this.subjectAreaField = value; }
        }

        /// <remarks/>
        public int RestrictedContent
        {
            get { return this.restrictedContentField; }
            set { this.restrictedContentField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RestrictedContentSpecified
        {
            get { return this.restrictedContentFieldSpecified; }
            set { this.restrictedContentFieldSpecified = value; }
        }

        /// <remarks/>
        public System.DateTime DateOfReceipt
        {
            get { return this.dateOfReceiptField; }
            set { this.dateOfReceiptField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateOfReceiptSpecified
        {
            get { return this.dateOfReceiptFieldSpecified; }
            set { this.dateOfReceiptFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] BinaryData
        {
            get { return this.binaryDataField; }
            set { this.binaryDataField = value; }
        }

        /// <remarks/>
        public string ObjectType
        {
            get { return this.objectTypeField; }
            set { this.objectTypeField = value; }
        }

        /// <remarks/>
        public string ObjectName
        {
            get { return this.objectNameField; }
            set { this.objectNameField = value; }
        }

        /// <remarks/>
        public string Path
        {
            get { return this.pathField; }
            set { this.pathField = value; }
        }

        /// <remarks/>
        public string Format
        {
            get { return this.formatField; }
            set { this.formatField = value; }
        }

        /// <remarks/>
        public string Username
        {
            get { return this.usernameField; }
            set { this.usernameField = value; }
        }

        /// <remarks/>
        public string Password
        {
            get { return this.passwordField; }
            set { this.passwordField = value; }
        }

        /// <remarks/>
        public SupportingDocumentStructureTransferMode TransferMode
        {
            get { return this.transferModeField; }
            set { this.transferModeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TransferModeSpecified
        {
            get { return this.transferModeFieldSpecified; }
            set { this.transferModeFieldSpecified = value; }
        }

        /// <remarks/>
        public string AclName
        {
            get { return this.aclNameField; }
            set { this.aclNameField = value; }
        }

        /// <remarks/>
        public string DocumentID
        {
            get { return this.documentIDField; }
            set { this.documentIDField = value; }
        }

        /// <remarks/>
        public string BatchID
        {
            get { return this.batchIDField; }
            set { this.batchIDField = value; }
        }

        /// <remarks/>
        public string ClassificationMethod
        {
            get { return this.classificationMethodField; }
            set { this.classificationMethodField = value; }
        }

        /// <remarks/>
        public string DataExtractMethod
        {
            get { return this.dataExtractMethodField; }
            set { this.dataExtractMethodField = value; }
        }

        /// <remarks/>
        public string ProcessStatus
        {
            get { return this.processStatusField; }
            set { this.processStatusField = value; }
        }

        /// <remarks/>
        public string ActualPageList
        {
            get { return this.actualPageListField; }
            set { this.actualPageListField = value; }
        }

        /// <remarks/>
        public int ExpectedPageCount
        {
            get { return this.expectedPageCountField; }
            set { this.expectedPageCountField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExpectedPageCountSpecified
        {
            get { return this.expectedPageCountFieldSpecified; }
            set { this.expectedPageCountFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DocumentIdentifier", IsNullable = false)]
        public IdentifierStructure[] DocumentIdentifiers
        {
            get { return this.documentIdentifiersField; }
            set { this.documentIdentifiersField = value; }
        }

        /// <remarks/>
        public string OriginatingLocation
        {
            get { return this.originatingLocationField; }
            set { this.originatingLocationField = value; }
        }

        /// <remarks/>
        public string GeoLocation
        {
            get { return this.geoLocationField; }
            set { this.geoLocationField = value; }
        }

        /// <remarks/>
        public string ProcedureCategoryCode
        {
            get { return this.procedureCategoryCodeField; }
            set { this.procedureCategoryCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UniqueIdentifier", IsNullable = false)]
        public string[] UniqueIdentifiers
        {
            get { return this.uniqueIdentifiersField; }
            set { this.uniqueIdentifiersField = value; }
        }

        /// <remarks/>
        public int PrimaryCopyInd
        {
            get { return this.primaryCopyIndField; }
            set { this.primaryCopyIndField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PrimaryCopyIndSpecified
        {
            get { return this.primaryCopyIndFieldSpecified; }
            set { this.primaryCopyIndFieldSpecified = value; }
        }

        /// <remarks/>
        public string FormID
        {
            get { return this.formIDField; }
            set { this.formIDField = value; }
        }

        /// <remarks/>
        public string BoxNumber
        {
            get { return this.boxNumberField; }
            set { this.boxNumberField = value; }
        }

        /// <remarks/>
        public string SourceNumber
        {
            get { return this.sourceNumberField; }
            set { this.sourceNumberField = value; }
        }

        /// <remarks/>
        public string DestinationNumber
        {
            get { return this.destinationNumberField; }
            set { this.destinationNumberField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime CallDate
        {
            get { return this.callDateField; }
            set { this.callDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CallDateSpecified
        {
            get { return this.callDateFieldSpecified; }
            set { this.callDateFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
        public System.DateTime CallDuration
        {
            get { return this.callDurationField; }
            set { this.callDurationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CallDurationSpecified
        {
            get { return this.callDurationFieldSpecified; }
            set { this.callDurationFieldSpecified = value; }
        }

        /// <remarks/>
        public string Retain
        {
            get { return this.retainField; }
            set { this.retainField = value; }
        }

        /// <remarks/>
        public string CallReferenceNumber
        {
            get { return this.callReferenceNumberField; }
            set { this.callReferenceNumberField = value; }
        }

        /// <remarks/>
        public MetadataStructure Metadata
        {
            get { return this.metadataField; }
            set { this.metadataField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    public partial class IdentifierStructure
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        public string Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        public string Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version" +
                    "/54.8")]
    public partial class DataStructure
    {

        private string nameField;

        private string valueField;

        private string gUIDField;

        private YesNoIndType lockedIndField;

        private bool lockedIndFieldSpecified;

        /// <remarks/>
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        public string Value
        {
            get { return this.valueField; }
            set { this.valueField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get { return this.gUIDField; }
            set { this.gUIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public YesNoIndType LockedInd
        {
            get { return this.lockedIndField; }
            set { this.lockedIndField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LockedIndSpecified
        {
            get { return this.lockedIndFieldSpecified; }
            set { this.lockedIndFieldSpecified = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.8")]
    public enum YesNoIndType
    {

        /// <remarks/>
        Y,

        /// <remarks/>
        N,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version" +
                    "/54.8")]
    public partial class ApplicationMetadataStructure
    {

        private DataStructure[] dataField;

        private string gUIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Data")]
        public DataStructure[] Data
        {
            get { return this.dataField; }
            set { this.dataField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GUID
        {
            get { return this.gUIDField; }
            set { this.gUIDField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version" +
                    "/54.8")]
    public partial class MetadataStructure
    {

        private ApplicationMetadataStructure[] applicationMetadataField;

        private MetadataStructureChangedInd changedIndField;

        private bool changedIndFieldSpecified;

        private string usernameField;

        private System.DateTime timeStampField;

        private bool timeStampFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ApplicationMetadata")]
        public ApplicationMetadataStructure[] ApplicationMetadata
        {
            get { return this.applicationMetadataField; }
            set { this.applicationMetadataField = value; }
        }

        /// <remarks/>
        public MetadataStructureChangedInd ChangedInd
        {
            get { return this.changedIndField; }
            set { this.changedIndField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChangedIndSpecified
        {
            get { return this.changedIndFieldSpecified; }
            set { this.changedIndFieldSpecified = value; }
        }

        /// <remarks/>
        public string Username
        {
            get { return this.usernameField; }
            set { this.usernameField = value; }
        }

        /// <remarks/>
        public System.DateTime TimeStamp
        {
            get { return this.timeStampField; }
            set { this.timeStampField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimeStampSpecified
        {
            get { return this.timeStampFieldSpecified; }
            set { this.timeStampFieldSpecified = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version" +
                    "/54.8")]
    public enum MetadataStructureChangedInd
    {

        /// <remarks/>
        NEW,

        /// <remarks/>
        UPDATED,

        /// <remarks/>
        DELETED,

        /// <remarks/>
        NO_CHANGE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    public enum SupportingDocumentStructureSpecialUsersGroup
    {

        /// <remarks/>
        NONE,

        /// <remarks/>
        VIP,

        /// <remarks/>
        LBC,

        /// <remarks/>
        PARLIMENTARIAN,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    public enum SupportingDocumentStructureTransferMode
    {

        /// <remarks/>
        BASE64,

        /// <remarks/>
        MTOM,

        /// <remarks/>
        UCF,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8")]
    [System.Xml.Serialization.XmlRootAttribute("ContentManagementResponse",
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ContentManagement/xml/schemas/ver" +
                    "sion/1.8", IsNullable = false)]
    public partial class SupportingDocumentManagementResponseStructure
    {

        private SupportingDocumentStructure[] contentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Content", IsNullable = false)]
        public SupportingDocumentStructure[] Contents
        {
            get { return this.contentsField; }
            set { this.contentsField = value; }
        }
    }

}