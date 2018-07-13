namespace Sars.Systems.Correspondence
{
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    [System.Xml.Serialization.XmlRootAttribute("CorrespondenceManagementRequest",Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9", IsNullable = false)]
    public partial class CorrespondenceManagementRequestStructure
    {

        private RequestOperation requestOperationField;

        private TaxRefStructure taxRefField;

        private CorrespondenceManagementRequestStructureLetter[] lettersField;

        private CorrespondenceManagementRequestStructureEmail emailField;

        private CorrespondenceManagementRequestStructureSMS sMSField;

        private string caseNoField;

        private string batchTypeField;

        private int batchSequenceField;

        private bool batchSequenceFieldSpecified;

        private string taxPeriodField;

        private string taxYearField;

        private System.DateTime taxDateField;

        private bool taxDateFieldSpecified;

        private CorrespondenceManagementRequestStructureOutChannel outChannelField;

        private bool outChannelFieldSpecified;

        private ApplicationMetadataStructure metadataField;

        /// <remarks/>
        public RequestOperation RequestOperation
        {
            get { return this.requestOperationField; }
            set { this.requestOperationField = value; }
        }

        /// <remarks/>
        public TaxRefStructure TaxRef
        {
            get { return this.taxRefField; }
            set { this.taxRefField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Letter", IsNullable = false)]
        public CorrespondenceManagementRequestStructureLetter[] Letters
        {
            get { return this.lettersField; }
            set { this.lettersField = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementRequestStructureEmail Email
        {
            get { return this.emailField; }
            set { this.emailField = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementRequestStructureSMS SMS
        {
            get { return this.sMSField; }
            set { this.sMSField = value; }
        }

        /// <remarks/>
        public string CaseNo
        {
            get { return this.caseNoField; }
            set { this.caseNoField = value; }
        }

        /// <remarks/>
        public string BatchType
        {
            get { return this.batchTypeField; }
            set { this.batchTypeField = value; }
        }

        /// <remarks/>
        public int BatchSequence
        {
            get { return this.batchSequenceField; }
            set { this.batchSequenceField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BatchSequenceSpecified
        {
            get { return this.batchSequenceFieldSpecified; }
            set { this.batchSequenceFieldSpecified = value; }
        }

        /// <remarks/>
        public string TaxPeriod
        {
            get { return this.taxPeriodField; }
            set { this.taxPeriodField = value; }
        }

        /// <remarks/>
        public string TaxYear
        {
            get { return this.taxYearField; }
            set { this.taxYearField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime TaxDate
        {
            get { return this.taxDateField; }
            set { this.taxDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaxDateSpecified
        {
            get { return this.taxDateFieldSpecified; }
            set { this.taxDateFieldSpecified = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementRequestStructureOutChannel OutChannel
        {
            get { return this.outChannelField; }
            set { this.outChannelField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OutChannelSpecified
        {
            get { return this.outChannelFieldSpecified; }
            set { this.outChannelFieldSpecified = value; }
        }

        /// <remarks/>
        public ApplicationMetadataStructure Metadata
        {
            get { return this.metadataField; }
            set { this.metadataField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public enum RequestOperation
    {

        /// <remarks/>
        GENERATE_CORRESPONDENCE,

        /// <remarks/>
        ISSUE_CORRESPONDENCE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(
        Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
    public partial class TaxRefStructure
    {

        private TypeOfTaxType typeOfTaxField;

        private string taxRefNoField;

        /// <remarks/>
        public TypeOfTaxType TypeOfTax
        {
            get { return this.typeOfTaxField; }
            set { this.typeOfTaxField = value; }
        }

        /// <remarks/>
        public string TaxRefNo
        {
            get { return this.taxRefNoField; }
            set { this.taxRefNoField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
    public enum TypeOfTaxType
    {

        /// <remarks/>
        INCOME_TAX,

        /// <remarks/>
        PAYE,

        /// <remarks/>
        VAT,

        /// <remarks/>
        CUSTOMS,

        /// <remarks/>
        EXCISE,

        /// <remarks/>
        UIF,

        /// <remarks/>
        SDL,

        /// <remarks/>
        DIESEL,

        /// <remarks/>
        PROVISIONAL_TAX,

        /// <remarks/>
        DIVIDENDS_TAX,

        /// <remarks/>
        APT,

        /// <remarks/>
        ASSESSED_TAX,

        /// <remarks/>
        ADMIN_PENALTY,

        /// <remarks/>
        TRANSFER_DUTY,

        /// <remarks/>
        CORPORATE_TAX,

        /// <remarks/>
        ESTATE_TAX,

        /// <remarks/>
        STC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class TemplateDetailsStructure
    {

        private string nameField;

        private LanguageType languageField;

        /// <remarks/>
        public string Name
        {
            get { return this.nameField; }
            set { this.nameField = value; }
        }

        /// <remarks/>
        public LanguageType Language
        {
            get { return this.languageField; }
            set { this.languageField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/GMD/BaseTypes/xml/schemas/version/54.12")]
    public enum LanguageType
    {

        /// <remarks/>
        ENGLISH,

        /// <remarks/>
        AFRIKAANS,

        /// <remarks/>
        NDEBELE,

        /// <remarks/>
        XHOSA,

        /// <remarks/>
        ZULU,

        /// <remarks/>
        SESOTHO_LEBOA,

        /// <remarks/>
        SESOTHO,

        /// <remarks/>
        SETSWANA,

        /// <remarks/>
        SISWATI,

        /// <remarks/>
        TSHIVENDA,

        /// <remarks/>
        XITSONGA,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementRequestStructureLetter
    {

        private string typeField;

        private string contentField;

        private string filenameField;

        private string documentMasterNameField;

        private string statusField;

        private TemplateDetailsStructure templateDetailsField;

        private string[] caseNosField;

        /// <remarks/>
        public string Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        public string Content
        {
            get { return this.contentField; }
            set { this.contentField = value; }
        }

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        public string DocumentMasterName
        {
            get { return this.documentMasterNameField; }
            set { this.documentMasterNameField = value; }
        }

        /// <remarks/>
        public string Status
        {
            get { return this.statusField; }
            set { this.statusField = value; }
        }

        /// <remarks/>
        public TemplateDetailsStructure TemplateDetails
        {
            get { return this.templateDetailsField; }
            set { this.templateDetailsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CaseNo", IsNullable = false)]
        public string[] CaseNos
        {
            get { return this.caseNosField; }
            set { this.caseNosField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementRequestStructureEmail
    {

        private string[] toAddressesField;

        private string[] cCAddressesField;

        private string fromAddressField;

        private string subjectField;

        private string bodyField;

        private CorrespondenceManagementRequestStructureEmailAttachment[] attachmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ToAddress", IsNullable = false)]
        public string[] ToAddresses
        {
            get { return this.toAddressesField; }
            set { this.toAddressesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CCAddress", IsNullable = false)]
        public string[] CCAddresses
        {
            get { return this.cCAddressesField; }
            set { this.cCAddressesField = value; }
        }

        /// <remarks/>
        public string FromAddress
        {
            get { return this.fromAddressField; }
            set { this.fromAddressField = value; }
        }

        /// <remarks/>
        public string Subject
        {
            get { return this.subjectField; }
            set { this.subjectField = value; }
        }

        /// <remarks/>
        public string Body
        {
            get { return this.bodyField; }
            set { this.bodyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public CorrespondenceManagementRequestStructureEmailAttachment[] Attachments
        {
            get { return this.attachmentsField; }
            set { this.attachmentsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementRequestStructureEmailAttachment
    {

        private string filenameField;

        private byte[] contentField;

        private DocumentType typeField;

        private TemplateDetailsStructure templateDetailsField;

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Content
        {
            get { return this.contentField; }
            set { this.contentField = value; }
        }

        /// <remarks/>
        public DocumentType Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        public TemplateDetailsStructure TemplateDetails
        {
            get { return this.templateDetailsField; }
            set { this.templateDetailsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public enum DocumentType
    {

        /// <remarks/>
        PDF,

        /// <remarks/>
        PDF_FLAT,

        /// <remarks/>
        XML,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementRequestStructureSMS
    {

        private string[] cellularNosField;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CellularNo", IsNullable = false)]
        public string[] CellularNos
        {
            get { return this.cellularNosField; }
            set { this.cellularNosField = value; }
        }

        /// <remarks/>
        public string Message
        {
            get { return this.messageField; }
            set { this.messageField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public enum CorrespondenceManagementRequestStructureOutChannel
    {

        /// <remarks/>
        DCTM,

        /// <remarks/>
        LTT,

        /// <remarks/>
        EFL,

        /// <remarks/>
        IFL,

        /// <remarks/>
        EMAIL,

        /// <remarks/>
        SMS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    [System.Xml.Serialization.XmlRootAttribute("CorrespondenceManagementResponse",Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9", IsNullable = false)]
    public partial class CorrespondenceManagementResponseStructure
    {

        private TaxRefStructure taxRefField;

        private CorrespondenceManagementResponseStructureLetter[] lettersField;

        private CorrespondenceManagementResponseStructureEmail emailField;

        private CorrespondenceManagementResponseStructureSMS sMSField;

        private string caseNoField;

        private string batchTypeField;

        private int batchSequenceField;

        private bool batchSequenceFieldSpecified;

        private string taxPeriodField;

        private string taxYearField;

        private System.DateTime taxDateField;

        private bool taxDateFieldSpecified;

        private CorrespondenceManagementResponseStructureOutChannel outChannelField;

        private bool outChannelFieldSpecified;

        private ApplicationMetadataStructure metadataField;

        /// <remarks/>
        public TaxRefStructure TaxRef
        {
            get { return this.taxRefField; }
            set { this.taxRefField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Letter", IsNullable = false)]
        public CorrespondenceManagementResponseStructureLetter[] Letters
        {
            get { return this.lettersField; }
            set { this.lettersField = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementResponseStructureEmail Email
        {
            get { return this.emailField; }
            set { this.emailField = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementResponseStructureSMS SMS
        {
            get { return this.sMSField; }
            set { this.sMSField = value; }
        }

        /// <remarks/>
        public string CaseNo
        {
            get { return this.caseNoField; }
            set { this.caseNoField = value; }
        }

        /// <remarks/>
        public string BatchType
        {
            get { return this.batchTypeField; }
            set { this.batchTypeField = value; }
        }

        /// <remarks/>
        public int BatchSequence
        {
            get { return this.batchSequenceField; }
            set { this.batchSequenceField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BatchSequenceSpecified
        {
            get { return this.batchSequenceFieldSpecified; }
            set { this.batchSequenceFieldSpecified = value; }
        }

        /// <remarks/>
        public string TaxPeriod
        {
            get { return this.taxPeriodField; }
            set { this.taxPeriodField = value; }
        }

        /// <remarks/>
        public string TaxYear
        {
            get { return this.taxYearField; }
            set { this.taxYearField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime TaxDate
        {
            get { return this.taxDateField; }
            set { this.taxDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaxDateSpecified
        {
            get { return this.taxDateFieldSpecified; }
            set { this.taxDateFieldSpecified = value; }
        }

        /// <remarks/>
        public CorrespondenceManagementResponseStructureOutChannel OutChannel
        {
            get { return this.outChannelField; }
            set { this.outChannelField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OutChannelSpecified
        {
            get { return this.outChannelFieldSpecified; }
            set { this.outChannelFieldSpecified = value; }
        }

        /// <remarks/>
        public ApplicationMetadataStructure Metadata
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementResponseStructureLetter
    {

        private string typeField;

        private byte[] contentField;

        private string filenameField;

        private string documentMasterNameField;

        private string statusField;

        private TemplateDetailsStructure templateDetailsField;

        /// <remarks/>
        public string Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Content
        {
            get { return this.contentField; }
            set { this.contentField = value; }
        }

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        public string DocumentMasterName
        {
            get { return this.documentMasterNameField; }
            set { this.documentMasterNameField = value; }
        }

        /// <remarks/>
        public string Status
        {
            get { return this.statusField; }
            set { this.statusField = value; }
        }

        /// <remarks/>
        public TemplateDetailsStructure TemplateDetails
        {
            get { return this.templateDetailsField; }
            set { this.templateDetailsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementResponseStructureEmail
    {

        private string[] toAddressesField;

        private string[] cCAddressesField;

        private string fromAddressField;

        private string subjectField;

        private string bodyField;

        private CorrespondenceManagementResponseStructureEmailAttachment[] attachmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ToAddress", IsNullable = false)]
        public string[] ToAddresses
        {
            get { return this.toAddressesField; }
            set { this.toAddressesField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CCAddress", IsNullable = false)]
        public string[] CCAddresses
        {
            get { return this.cCAddressesField; }
            set { this.cCAddressesField = value; }
        }

        /// <remarks/>
        public string FromAddress
        {
            get { return this.fromAddressField; }
            set { this.fromAddressField = value; }
        }

        /// <remarks/>
        public string Subject
        {
            get { return this.subjectField; }
            set { this.subjectField = value; }
        }

        /// <remarks/>
        public string Body
        {
            get { return this.bodyField; }
            set { this.bodyField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attachment", IsNullable = false)]
        public CorrespondenceManagementResponseStructureEmailAttachment[] Attachments
        {
            get { return this.attachmentsField; }
            set { this.attachmentsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementResponseStructureEmailAttachment
    {

        private string filenameField;

        private byte[] contentField;

        private DocumentType typeField;

        private TemplateDetailsStructure templateDetailsField;

        /// <remarks/>
        public string Filename
        {
            get { return this.filenameField; }
            set { this.filenameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Content
        {
            get { return this.contentField; }
            set { this.contentField = value; }
        }

        /// <remarks/>
        public DocumentType Type
        {
            get { return this.typeField; }
            set { this.typeField = value; }
        }

        /// <remarks/>
        public TemplateDetailsStructure TemplateDetails
        {
            get { return this.templateDetailsField; }
            set { this.templateDetailsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public partial class CorrespondenceManagementResponseStructureSMS
    {

        private string[] cellularNosField;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CellularNo", IsNullable = false)]
        public string[] CellularNos
        {
            get { return this.cellularNosField; }
            set { this.cellularNosField = value; }
        }

        /// <remarks/>
        public string Message
        {
            get { return this.messageField; }
            set { this.messageField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true,Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/CorrespondenceManagement/xml/schemas/version/1.9")]
    public enum CorrespondenceManagementResponseStructureOutChannel
    {

        /// <remarks/>
        DCTM,

        /// <remarks/>
        LTT,

        /// <remarks/>
        EFL,

        /// <remarks/>
        IFL,

        /// <remarks/>
        EMAIL,

        /// <remarks/>
        SMS,
    }
}