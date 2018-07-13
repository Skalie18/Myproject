using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using FDR.DataLayer;
using FDRService.CbcXML;
using Sars.Systems.Data;
using Sars.Systems.Serialization;

public partial class queueMonitors_MANUAL_updateadditionalinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var _sql =
            @"SELECT TOP 1 [MessageSpec_ID] ,[CBCReport]   FROM [FDR].[dbo].[X_MessageSpec] where [MessageSpec_ID] not in (select [MessageSpec_ID] from [dbo].[X_AdditionalInfo]) and [CBCReport] is not null";
        using (var data = new RecordSet(_sql, QueryType.TransectSQL, null))
        {
            if (data.HasRows)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var messageSpecId = Convert.ToDecimal(row["MessageSpec_ID"]);
                    var cbcXml = row["CBCReport"].ToString();
                    var cbcMessage = XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXml);
                    if (cbcMessage == null)
                    {
                        continue;
                    }

                    foreach (var cbcBodyType in cbcMessage.CbcBody)
                    {
                        if (cbcBodyType.AdditionalInfo != null)
                        {
                            foreach (var addInfo in cbcBodyType.AdditionalInfo)
                            {
                                var additionalInfoId = 0M;
                                additionalInfoId = DBWriteManager.Save_X_AdditionalInfo(
                                    additionalInfoId
                                    , addInfo.DocSpec.DocTypeIndic.ToString()
                                    , addInfo.DocSpec.DocRefId
                                    , addInfo.DocSpec.CorrMessageRefId
                                    , addInfo.DocSpec.CorrDocRefId
                                    , addInfo.OtherInfo
                                    , messageSpecId
                                    );
                                if (additionalInfoId > 0)
                                {
                                    if (addInfo.ResCountryCode != null)
                                    {
                                        foreach (var countryCodeType in addInfo.ResCountryCode)
                                        {
                                            DBWriteManager.X_AdditionalInfo_ResCountryCode(
                                                0M
                                                , countryCodeType.ToString()
                                                , additionalInfoId
                                                );
                                        }
                                    }
                                    if (addInfo.SummaryRef != null)
                                    {
                                        foreach (var cbcSummary in addInfo.SummaryRef)
                                        {
                                            DBWriteManager.Save_X_AdditionalInfo_SummaryRef(
                                                0M
                                                , cbcSummary.ToString()
                                                , additionalInfoId
                                                );
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
    }

    protected void btnUpdateReportingPeriod_Click(object sender, EventArgs e)
    {
        var _sql =
            "SELECT [Id], [CBC], [TaxRefNo], [TaxYear] FROM [dbo].[CBCDeclarations] WHERE [ReportingPeriod] IS NULL AND [CBC] IS NOT NULL";
        var _sqlUpdate =
            @"UPDATE [dbo].[CBCDeclarations] SET [ReportingPeriod] = @ReportingPeriod, [NumReports] =  @NumReports WHERE [Id] = @Id";
        using (var data = new RecordSet(_sql, QueryType.TransectSQL, null))
        {
            if (data.HasRows)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var xml = row["CBC"].ToString();
                    var id = row["Id"].ToString();

                    try
                    {
                        var cbcXmldoc = new XmlDocument();
                        cbcXmldoc.LoadXml(xml);
                        var cbcXmlNodeList = cbcXmldoc.GetElementsByTagName("CBC_OECD", "*");
                        if (cbcXmlNodeList.Count > 0)
                        {
                            var cbcXml = cbcXmlNodeList[0].OuterXml;
                            var cbcMessage = XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXml);
                            if (cbcMessage == null)
                            {
                                continue;
                            }

                            var oParams = new DBParamCollection
                            {
                                {"@ReportingPeriod", cbcMessage.MessageSpec.ReportingPeriod},
                                {
                                    "@NumReports",
                                    cbcMessage.CbcBody[0].CbcReports != null
                                        ? cbcMessage.CbcBody[0].CbcReports.Length
                                        : 0
                                },
                                {"@Id", id}
                            };

                            using (var command = new DBCommand(_sqlUpdate, QueryType.TransectSQL, oParams))
                            {
                                command.Execute();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }
            }
        }
    }

    protected void btnUpdateCorrections_Click(object sender, EventArgs e)
    {
        const string sql =@"SELECT  [MessageSpec_ID] ,[IsCorrection] , [CBCReport] FROM [FDR].[dbo].[X_MessageSpec] WHERE [CBCReport] IS NOT NULL AND [IsCorrection]= 0";
        const string sqlUpdate = @"UPDATE [FDR].[dbo].[X_MessageSpec] SET [IsCorrection] = 1, [OriginalMessageRefId] = @OriginalMessageRefId WHERE [MessageSpec_ID] = @MessageSpec_ID";
        using (var data = new RecordSet(sql, QueryType.TransectSQL, null))
        {
            if (data.HasRows)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    var xml = row["CBCReport"].ToString();
                    var messageSpecId = row["MessageSpec_ID"].ToString();

                    var cbcXmldoc = new XmlDocument();
                    cbcXmldoc.LoadXml(xml);
                    var cbcXmlNodeList = cbcXmldoc.GetElementsByTagName("CBC_OECD", "*");
                    if (cbcXmlNodeList.Count > 0)
                    {
                        var cbcXml = cbcXmlNodeList[0].OuterXml;
                        var cbcMessage = XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXml);
                        if (cbcMessage == null)
                        {
                            continue;
                        }
                        var correctedData = 0;
                        var deletedData = 0;
                        var docTypeIndics = new List<OECDDocTypeIndic_EnumType>();
                        string originalMessageRefId = null;
                        foreach (var cbcBodyType in cbcMessage.CbcBody)
                        {
                            if (cbcBodyType.ReportingEntity != null)
                            {
                                var docSpec = cbcBodyType.ReportingEntity.DocSpec;
                                if (docSpec != null)
                                {
                                    docTypeIndics.Add(docSpec.DocTypeIndic);
                                    if (originalMessageRefId == null)
                                    {
                                        if (docSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD2 ||
                                            docSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD3 ||
                                            docSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD12 ||
                                            docSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD13)
                                        {
                                            originalMessageRefId =
                                                DBReadManager.GetOriginalMessageRefId(docSpec.CorrDocRefId,
                                                    cbcMessage.MessageSpec.TransmittingCountry.ToString());
                                        }
                                    }
                                }
                            }
                            if (cbcBodyType.CbcReports != null)
                            {
                                foreach (var cbcReport in cbcBodyType.CbcReports)
                                {
                                    var reportDocSpec = cbcReport.DocSpec;
                                    if (reportDocSpec != null)
                                    {
                                        docTypeIndics.Add(reportDocSpec.DocTypeIndic);
                                        if (originalMessageRefId == null)
                                        {
                                            if (reportDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD2 ||
                                                reportDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD3 ||
                                                reportDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD12 ||
                                                reportDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD13)
                                            {
                                                originalMessageRefId =
                                                    DBReadManager.GetOriginalMessageRefId(reportDocSpec.CorrDocRefId,
                                                        cbcMessage.MessageSpec.TransmittingCountry.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            if (cbcBodyType.AdditionalInfo != null)
                            {
                                foreach (var info in cbcBodyType.AdditionalInfo)
                                {
                                    var addInfoDocSpec = info.DocSpec;

                                    if (addInfoDocSpec != null)
                                    {
                                        docTypeIndics.Add(addInfoDocSpec.DocTypeIndic);
                                        if (originalMessageRefId == null)
                                        {
                                            if (addInfoDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD2 ||
                                                addInfoDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD3 ||
                                                addInfoDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD12 ||
                                                addInfoDocSpec.DocTypeIndic == OECDDocTypeIndic_EnumType.OECD13)
                                            {
                                                originalMessageRefId =
                                                    DBReadManager.GetOriginalMessageRefId(addInfoDocSpec.CorrDocRefId,
                                                        cbcMessage.MessageSpec.TransmittingCountry.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }

                      
                        var mixedDocTypeIndicators = docTypeIndics.Distinct().ToList();
                        foreach (var docTypeInd in mixedDocTypeIndicators)
                        {
                            switch (docTypeInd)
                            {
                                case OECDDocTypeIndic_EnumType.OECD2:
                                case OECDDocTypeIndic_EnumType.OECD12:
                                    {
                                        correctedData++;
                                        break;
                                    }
                                case OECDDocTypeIndic_EnumType.OECD3:
                                case OECDDocTypeIndic_EnumType.OECD13:
                                    {
                                        deletedData++;
                                        break;
                                    }
                            }
                        }
                        if (deletedData > 0 || correctedData > 0)
                        {
                            var oParams = new DBParamCollection
                            {
                                {"@MessageSpec_ID", messageSpecId}
                              , {"@OriginalMessageRefId", originalMessageRefId ?? "" }
                            };
                            using (var command = new DBCommand(sqlUpdate, QueryType.TransectSQL, oParams))
                            {
                                command.Execute();
                            }
                        }
                    }
                }
            }
        }
    }
}