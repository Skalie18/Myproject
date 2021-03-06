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
// This source code was auto-generated by xsd, Version=4.6.81.0.
// 

namespace FDRService.CbcXML.CTSSenderFileMetadata
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oecd:ctssenderfilemetadata")]
    [System.Xml.Serialization.XmlRootAttribute("CTSSenderFileMetadata", Namespace = "urn:oecd:ctssenderfilemetadata",
        IsNullable = false)]
    public partial class CTSSenderFileMetadataType
    {

        private CountryCode_Type cTSSenderCountryCdField;

        private CountryCode_Type cTSReceiverCountryCdField;

        private CTSCommunicationTypeCdType cTSCommunicationTypeCdField;

        private string senderFileIdField;

        private FileFormatCdType fileFormatCdField;

        private bool fileFormatCdFieldSpecified;

        private BinaryEncodingSchemeCdType binaryEncodingSchemeCdField;

        private bool binaryEncodingSchemeCdFieldSpecified;

        private System.DateTime fileCreateTsField;

        private bool fileCreateTsFieldSpecified;

        private string taxYearField;

        private bool fileRevisionIndField;

        private bool fileRevisionIndFieldSpecified;

        private string originalCTSTransmissionIdField;

        private string senderContactEmailAddressTxtField;

        /// <remarks/>
        public CountryCode_Type CTSSenderCountryCd
        {
            get { return this.cTSSenderCountryCdField; }
            set { this.cTSSenderCountryCdField = value; }
        }

        /// <remarks/>
        public CountryCode_Type CTSReceiverCountryCd
        {
            get { return this.cTSReceiverCountryCdField; }
            set { this.cTSReceiverCountryCdField = value; }
        }

        /// <remarks/>
        public CTSCommunicationTypeCdType CTSCommunicationTypeCd
        {
            get { return this.cTSCommunicationTypeCdField; }
            set { this.cTSCommunicationTypeCdField = value; }
        }

        /// <remarks/>
        public string SenderFileId
        {
            get { return this.senderFileIdField; }
            set { this.senderFileIdField = value; }
        }

        /// <remarks/>
        public FileFormatCdType FileFormatCd
        {
            get { return this.fileFormatCdField; }
            set { this.fileFormatCdField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FileFormatCdSpecified
        {
            get { return this.fileFormatCdFieldSpecified; }
            set { this.fileFormatCdFieldSpecified = value; }
        }

        /// <remarks/>
        public BinaryEncodingSchemeCdType BinaryEncodingSchemeCd
        {
            get { return this.binaryEncodingSchemeCdField; }
            set { this.binaryEncodingSchemeCdField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BinaryEncodingSchemeCdSpecified
        {
            get { return this.binaryEncodingSchemeCdFieldSpecified; }
            set { this.binaryEncodingSchemeCdFieldSpecified = value; }
        }

        /// <remarks/>
        public System.DateTime FileCreateTs
        {
            get { return this.fileCreateTsField; }
            set { this.fileCreateTsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FileCreateTsSpecified
        {
            get { return this.fileCreateTsFieldSpecified; }
            set { this.fileCreateTsFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "gYear")]
        public string TaxYear
        {
            get { return this.taxYearField; }
            set { this.taxYearField = value; }
        }

        /// <remarks/>
        public bool FileRevisionInd
        {
            get { return this.fileRevisionIndField; }
            set { this.fileRevisionIndField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FileRevisionIndSpecified
        {
            get { return this.fileRevisionIndFieldSpecified; }
            set { this.fileRevisionIndFieldSpecified = value; }
        }

        /// <remarks/>
        public string OriginalCTSTransmissionId
        {
            get { return this.originalCTSTransmissionIdField; }
            set { this.originalCTSTransmissionIdField = value; }
        }

        /// <remarks/>
        public string SenderContactEmailAddressTxt
        {
            get { return this.senderContactEmailAddressTxtField; }
            set { this.senderContactEmailAddressTxtField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oecd:ties:isoctstypes:v1")]
    [System.Xml.Serialization.XmlRootAttribute("CTSSenderCountryCd", Namespace = "urn:oecd:ctssenderfilemetadata",
        IsNullable = false)]
    public enum CountryCode_Type
    {

        /// <remarks/>
        AF,

        /// <remarks/>
        AX,

        /// <remarks/>
        AL,

        /// <remarks/>
        DZ,

        /// <remarks/>
        AS,

        /// <remarks/>
        AD,

        /// <remarks/>
        AO,

        /// <remarks/>
        AI,

        /// <remarks/>
        AQ,

        /// <remarks/>
        AG,

        /// <remarks/>
        AR,

        /// <remarks/>
        AM,

        /// <remarks/>
        AW,

        /// <remarks/>
        AU,

        /// <remarks/>
        AT,

        /// <remarks/>
        AZ,

        /// <remarks/>
        BS,

        /// <remarks/>
        BH,

        /// <remarks/>
        BD,

        /// <remarks/>
        BB,

        /// <remarks/>
        BY,

        /// <remarks/>
        BE,

        /// <remarks/>
        BZ,

        /// <remarks/>
        BJ,

        /// <remarks/>
        BM,

        /// <remarks/>
        BT,

        /// <remarks/>
        BO,

        /// <remarks/>
        BQ,

        /// <remarks/>
        BA,

        /// <remarks/>
        BW,

        /// <remarks/>
        BV,

        /// <remarks/>
        BR,

        /// <remarks/>
        IO,

        /// <remarks/>
        BN,

        /// <remarks/>
        BG,

        /// <remarks/>
        BF,

        /// <remarks/>
        BI,

        /// <remarks/>
        KH,

        /// <remarks/>
        CM,

        /// <remarks/>
        CA,

        /// <remarks/>
        CV,

        /// <remarks/>
        KY,

        /// <remarks/>
        CF,

        /// <remarks/>
        TD,

        /// <remarks/>
        CL,

        /// <remarks/>
        CN,

        /// <remarks/>
        CX,

        /// <remarks/>
        CC,

        /// <remarks/>
        CO,

        /// <remarks/>
        KM,

        /// <remarks/>
        CG,

        /// <remarks/>
        CD,

        /// <remarks/>
        CK,

        /// <remarks/>
        CR,

        /// <remarks/>
        CI,

        /// <remarks/>
        HR,

        /// <remarks/>
        CU,

        /// <remarks/>
        CW,

        /// <remarks/>
        CY,

        /// <remarks/>
        CZ,

        /// <remarks/>
        DK,

        /// <remarks/>
        DJ,

        /// <remarks/>
        DM,

        /// <remarks/>
        DO,

        /// <remarks/>
        EC,

        /// <remarks/>
        EG,

        /// <remarks/>
        SV,

        /// <remarks/>
        GQ,

        /// <remarks/>
        ER,

        /// <remarks/>
        EE,

        /// <remarks/>
        ET,

        /// <remarks/>
        FK,

        /// <remarks/>
        FO,

        /// <remarks/>
        FJ,

        /// <remarks/>
        FI,

        /// <remarks/>
        FR,

        /// <remarks/>
        GF,

        /// <remarks/>
        PF,

        /// <remarks/>
        TF,

        /// <remarks/>
        GA,

        /// <remarks/>
        GM,

        /// <remarks/>
        GE,

        /// <remarks/>
        DE,

        /// <remarks/>
        GH,

        /// <remarks/>
        GI,

        /// <remarks/>
        GR,

        /// <remarks/>
        GL,

        /// <remarks/>
        GD,

        /// <remarks/>
        GP,

        /// <remarks/>
        GU,

        /// <remarks/>
        GT,

        /// <remarks/>
        GG,

        /// <remarks/>
        GN,

        /// <remarks/>
        GW,

        /// <remarks/>
        GY,

        /// <remarks/>
        HT,

        /// <remarks/>
        HM,

        /// <remarks/>
        VA,

        /// <remarks/>
        HN,

        /// <remarks/>
        HK,

        /// <remarks/>
        HU,

        /// <remarks/>
        IS,

        /// <remarks/>
        IN,

        /// <remarks/>
        ID,

        /// <remarks/>
        IR,

        /// <remarks/>
        IQ,

        /// <remarks/>
        IE,

        /// <remarks/>
        IM,

        /// <remarks/>
        IL,

        /// <remarks/>
        IT,

        /// <remarks/>
        JM,

        /// <remarks/>
        JP,

        /// <remarks/>
        JE,

        /// <remarks/>
        JO,

        /// <remarks/>
        KZ,

        /// <remarks/>
        KE,

        /// <remarks/>
        KI,

        /// <remarks/>
        KP,

        /// <remarks/>
        KR,

        /// <remarks/>
        KW,

        /// <remarks/>
        KG,

        /// <remarks/>
        LA,

        /// <remarks/>
        LV,

        /// <remarks/>
        LB,

        /// <remarks/>
        LS,

        /// <remarks/>
        LR,

        /// <remarks/>
        LY,

        /// <remarks/>
        LI,

        /// <remarks/>
        LT,

        /// <remarks/>
        LU,

        /// <remarks/>
        MO,

        /// <remarks/>
        MK,

        /// <remarks/>
        MG,

        /// <remarks/>
        MW,

        /// <remarks/>
        MY,

        /// <remarks/>
        MV,

        /// <remarks/>
        ML,

        /// <remarks/>
        MT,

        /// <remarks/>
        MH,

        /// <remarks/>
        MQ,

        /// <remarks/>
        MR,

        /// <remarks/>
        MU,

        /// <remarks/>
        YT,

        /// <remarks/>
        MX,

        /// <remarks/>
        FM,

        /// <remarks/>
        MD,

        /// <remarks/>
        MC,

        /// <remarks/>
        MN,

        /// <remarks/>
        ME,

        /// <remarks/>
        MS,

        /// <remarks/>
        MA,

        /// <remarks/>
        MZ,

        /// <remarks/>
        MM,

        /// <remarks/>
        NA,

        /// <remarks/>
        NR,

        /// <remarks/>
        NP,

        /// <remarks/>
        NL,

        /// <remarks/>
        NC,

        /// <remarks/>
        NZ,

        /// <remarks/>
        NI,

        /// <remarks/>
        NE,

        /// <remarks/>
        NG,

        /// <remarks/>
        NU,

        /// <remarks/>
        NF,

        /// <remarks/>
        MP,

        /// <remarks/>
        NO,

        /// <remarks/>
        OM,

        /// <remarks/>
        PK,

        /// <remarks/>
        PW,

        /// <remarks/>
        PS,

        /// <remarks/>
        PA,

        /// <remarks/>
        PG,

        /// <remarks/>
        PY,

        /// <remarks/>
        PE,

        /// <remarks/>
        PH,

        /// <remarks/>
        PN,

        /// <remarks/>
        PL,

        /// <remarks/>
        PT,

        /// <remarks/>
        PR,

        /// <remarks/>
        QA,

        /// <remarks/>
        RE,

        /// <remarks/>
        RO,

        /// <remarks/>
        RU,

        /// <remarks/>
        RW,

        /// <remarks/>
        BL,

        /// <remarks/>
        SH,

        /// <remarks/>
        KN,

        /// <remarks/>
        LC,

        /// <remarks/>
        MF,

        /// <remarks/>
        PM,

        /// <remarks/>
        VC,

        /// <remarks/>
        WS,

        /// <remarks/>
        SM,

        /// <remarks/>
        ST,

        /// <remarks/>
        SA,

        /// <remarks/>
        SN,

        /// <remarks/>
        RS,

        /// <remarks/>
        SC,

        /// <remarks/>
        SL,

        /// <remarks/>
        SG,

        /// <remarks/>
        SX,

        /// <remarks/>
        SK,

        /// <remarks/>
        SI,

        /// <remarks/>
        SB,

        /// <remarks/>
        SO,

        /// <remarks/>
        ZA,

        /// <remarks/>
        GS,

        /// <remarks/>
        SS,

        /// <remarks/>
        ES,

        /// <remarks/>
        LK,

        /// <remarks/>
        SD,

        /// <remarks/>
        SR,

        /// <remarks/>
        SJ,

        /// <remarks/>
        SZ,

        /// <remarks/>
        SE,

        /// <remarks/>
        CH,

        /// <remarks/>
        SY,

        /// <remarks/>
        TW,

        /// <remarks/>
        TJ,

        /// <remarks/>
        TZ,

        /// <remarks/>
        TH,

        /// <remarks/>
        TL,

        /// <remarks/>
        TG,

        /// <remarks/>
        TK,

        /// <remarks/>
        TO,

        /// <remarks/>
        TT,

        /// <remarks/>
        TN,

        /// <remarks/>
        TR,

        /// <remarks/>
        TM,

        /// <remarks/>
        TC,

        /// <remarks/>
        TV,

        /// <remarks/>
        UG,

        /// <remarks/>
        UA,

        /// <remarks/>
        AE,

        /// <remarks/>
        GB,

        /// <remarks/>
        US,

        /// <remarks/>
        UM,

        /// <remarks/>
        UY,

        /// <remarks/>
        UZ,

        /// <remarks/>
        VU,

        /// <remarks/>
        VE,

        /// <remarks/>
        VN,

        /// <remarks/>
        VG,

        /// <remarks/>
        VI,

        /// <remarks/>
        WF,

        /// <remarks/>
        EH,

        /// <remarks/>
        YE,

        /// <remarks/>
        ZM,

        /// <remarks/>
        ZW,

        /// <remarks/>
        XK,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AF.00")] AF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AX.00")] AX00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AL.00")] AL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DZ.00")] DZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AS.00")] AS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AD.00")] AD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AO.00")] AO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AI.00")] AI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AQ.00")] AQ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AG.00")] AG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AR.00")] AR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AM.00")] AM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AW.00")] AW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AU.00")] AU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AT.00")] AT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AZ.00")] AZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BS.00")] BS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BH.00")] BH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BD.00")] BD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BB.00")] BB00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BY.00")] BY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BE.00")] BE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BZ.00")] BZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BJ.00")] BJ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BM.00")] BM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BT.00")] BT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BO.00")] BO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BQ.00")] BQ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BA.00")] BA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BW.00")] BW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BV.00")] BV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BR.00")] BR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IO.00")] IO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BN.00")] BN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BG.00")] BG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BF.00")] BF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BI.00")] BI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KH.00")] KH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CM.00")] CM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CA.00")] CA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CV.00")] CV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KY.00")] KY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CF.00")] CF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TD.00")] TD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CL.00")] CL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CN.00")] CN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CX.00")] CX00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CC.00")] CC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CO.00")] CO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KM.00")] KM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CG.00")] CG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CD.00")] CD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CK.00")] CK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CR.00")] CR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CI.00")] CI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HR.00")] HR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CU.00")] CU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CW.00")] CW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CY.00")] CY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CZ.00")] CZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DK.00")] DK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DJ.00")] DJ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DM.00")] DM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DO.00")] DO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("EC.00")] EC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("EG.00")] EG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SV.00")] SV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GQ.00")] GQ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ER.00")] ER00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("EE.00")] EE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ET.00")] ET00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FK.00")] FK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FO.00")] FO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FJ.00")] FJ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FI.00")] FI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FR.00")] FR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GF.00")] GF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PF.00")] PF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TF.00")] TF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GA.00")] GA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GM.00")] GM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GE.00")] GE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("DE.00")] DE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GH.00")] GH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GI.00")] GI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GR.00")] GR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GL.00")] GL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GD.00")] GD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GP.00")] GP00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GU.00")] GU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GT.00")] GT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GG.00")] GG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GN.00")] GN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GW.00")] GW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GY.00")] GY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HT.00")] HT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HM.00")] HM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VA.00")] VA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HN.00")] HN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HK.00")] HK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("HU.00")] HU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IS.00")] IS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IN.00")] IN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ID.00")] ID00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IR.00")] IR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IQ.00")] IQ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IE.00")] IE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IM.00")] IM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IL.00")] IL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("IT.00")] IT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("JM.00")] JM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("JP.00")] JP00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("JE.00")] JE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("JO.00")] JO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KZ.00")] KZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KE.00")] KE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KI.00")] KI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KP.00")] KP00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KR.00")] KR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KW.00")] KW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KG.00")] KG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LA.00")] LA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LV.00")] LV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LB.00")] LB00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LS.00")] LS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LR.00")] LR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LY.00")] LY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LI.00")] LI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LT.00")] LT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LU.00")] LU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MO.00")] MO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MK.00")] MK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MG.00")] MG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MW.00")] MW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MY.00")] MY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MV.00")] MV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ML.00")] ML00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MT.00")] MT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MH.00")] MH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MQ.00")] MQ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MR.00")] MR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MU.00")] MU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("YT.00")] YT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MX.00")] MX00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FM.00")] FM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MD.00")] MD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MC.00")] MC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MN.00")] MN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ME.00")] ME00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MS.00")] MS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MA.00")] MA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MZ.00")] MZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MM.00")] MM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NA.00")] NA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NR.00")] NR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NP.00")] NP00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NL.00")] NL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NC.00")] NC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NZ.00")] NZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NI.00")] NI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NE.00")] NE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NG.00")] NG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NU.00")] NU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NF.00")] NF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MP.00")] MP00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NO.00")] NO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("OM.00")] OM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PK.00")] PK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PW.00")] PW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PS.00")] PS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PA.00")] PA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PG.00")] PG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PY.00")] PY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PE.00")] PE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PH.00")] PH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PN.00")] PN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PL.00")] PL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PT.00")] PT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PR.00")] PR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("QA.00")] QA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RE.00")] RE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RO.00")] RO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RU.00")] RU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RW.00")] RW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BL.00")] BL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SH.00")] SH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("KN.00")] KN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LC.00")] LC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("MF.00")] MF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("PM.00")] PM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VC.00")] VC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("WS.00")] WS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SM.00")] SM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ST.00")] ST00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SA.00")] SA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SN.00")] SN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RS.00")] RS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SC.00")] SC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SL.00")] SL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SG.00")] SG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SX.00")] SX00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SK.00")] SK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SI.00")] SI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SB.00")] SB00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SO.00")] SO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ZA.00")] ZA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GS.00")] GS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SS.00")] SS00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ES.00")] ES00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("LK.00")] LK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SD.00")] SD00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SR.00")] SR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SJ.00")] SJ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SZ.00")] SZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SE.00")] SE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CH.00")] CH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SY.00")] SY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TW.00")] TW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TJ.00")] TJ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TZ.00")] TZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TH.00")] TH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TL.00")] TL00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TG.00")] TG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TK.00")] TK00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TO.00")] TO00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TT.00")] TT00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TN.00")] TN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TR.00")] TR00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TM.00")] TM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TC.00")] TC00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("TV.00")] TV00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UG.00")] UG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UA.00")] UA00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("AE.00")] AE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GB.00")] GB00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US.00")] US00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UM.00")] UM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UY.00")] UY00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UZ.00")] UZ00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VU.00")] VU00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VE.00")] VE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VN.00")] VN00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VG.00")] VG00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("VI.00")] VI00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("WF.00")] WF00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("EH.00")] EH00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("YE.00")] YE00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ZM.00")] ZM00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ZW.00")] ZW00,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("XK.00")] XK00,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oecd:ctssenderfilemetadata")]
    [System.Xml.Serialization.XmlRootAttribute("CTSCommunicationTypeCd", Namespace = "urn:oecd:ctssenderfilemetadata",
        IsNullable = false)]
    public enum CTSCommunicationTypeCdType
    {

        /// <remarks/>
        CRS,

        /// <remarks/>
        CRSStatus,

        /// <remarks/>
        ETR,

        /// <remarks/>
        ETRStatus,

        /// <remarks/>
        CBC,

        /// <remarks/>
        CBCStatus,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oecd:ctssenderfilemetadata")]
    [System.Xml.Serialization.XmlRootAttribute("FileFormatCd", Namespace = "urn:oecd:ctssenderfilemetadata",
        IsNullable = false)]
    public enum FileFormatCdType
    {

        /// <remarks/>
        XML,

        /// <remarks/>
        TXT,

        /// <remarks/>
        PDF,

        /// <remarks/>
        RTF,

        /// <remarks/>
        JPG,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oecd:ctssenderfilemetadata")]
    [System.Xml.Serialization.XmlRootAttribute("BinaryEncodingSchemeCd", Namespace = "urn:oecd:ctssenderfilemetadata",
        IsNullable = false)]
    public enum BinaryEncodingSchemeCdType
    {

        /// <remarks/>
        NONE,

        /// <remarks/>
        BASE64,
    }
}