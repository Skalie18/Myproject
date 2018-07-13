using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Summary description for ThirdPartyDataActivityManagementResponse
/// </summary>
public class ThirdPartyDataActivityManagementResponse
{
    public ThirdPartyDataActivityManagementResponse()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [XmlElement("CBC")]
    public CbcReturnDetails CBC { get; set; }
}

public class CbcReturnDetails
{
    [XmlElement]
    public string SubmitMasterAndLocalFileInd { get; set; }
    [XmlElement]
    public string SubmitCbCDeclarationInd { get; set; }
}