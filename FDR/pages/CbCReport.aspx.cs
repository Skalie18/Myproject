using FDRService.CbcXML;
using Sars.Systems.Extensions;
using Sars.Systems.Messaging;
using Sars.Systems.Serialization;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Sars.Systems.Data;

public partial class pages_MANUAL_cBCrEPORT : System.Web.UI.Page
{
    DataTable data = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        var uid = Request["uid"].ToUpper();
        var source = Request["src"].ToUpper();
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(source))
        {
            Response.Write("PARAMETER MISSING");
            return;
        }

        Guid guid;
        if (!Guid.TryParse(uid, out guid))
        {
            Response.Write("UID PARAMETER IN NOT VALID");
            return;
        }
        var procedureName = string.Empty;
        switch (source)
        {
            case "IN":
            {
                procedureName = "usp_READ_ReportXml_IN";
                break;
            }
            case "OUT":
            {
                procedureName = "usp_READ_ReportXml_OUT";
                break;
            }
            case "MNE":
            {
                procedureName = "usp_READ_ReportXml_MNE";
                break;
            }
            default:
            {
                procedureName = null;
                break;
            }
        }
        if (string.IsNullOrEmpty(procedureName))
        {
            MessageBox.Show("PROCEDURE NAME IS NOT VALID");
            return;
        }
        var recordSet = new RecordSet(procedureName, QueryType.StoredProcedure, new DBParamCollection { { "@UID", guid.ToString() } });
        if (!recordSet.HasRows) { MessageBox.Show("THER IS NO DATA AVAILABLE"); return; }

        var xmlData = recordSet[0][0].ToString();
        var cbcXml = "";
        if (source.Equals("MNE", StringComparison.CurrentCultureIgnoreCase))
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlData);
            var cbCXmlNodeList = doc.GetElementsByTagName("CBC_OECD", "*");
            if (cbCXmlNodeList.Count > 0)
            {
                cbcXml = cbCXmlNodeList[0].OuterXml;
            }
        }
        else
        {
            cbcXml = xmlData;
        }

        var client = new ESBMessagingServiceClient("basic");
        //cbcXml = File.ReadAllText(@"D:\FDR\Report\CBC_OUT_KR.xml");
        cbcXml = FdrCommon.FormatXml(cbcXml);
        var cbcSchemaVal = client.ValidateSchema(Configurations.CbCValidationServiceID, cbcXml);

        if (!cbcSchemaVal.IsValid)
        {
            return;
        }
        cbcXml = cbcXml.Replace("<CBC_OECD>", "<CBC_OECD xmlns=\"urn:oecd:ties:cbc:v1\">");
        cbcXml = FdrCommon.FormatXml(cbcXml);
        var cbcMessage = XmlObjectSerializer.ConvertXmlToObject<CBC_OECD>(cbcXml);

        if (cbcMessage == null) return;

        var overAllReport = new StringBuilder();
        overAllReport.Append( CreateMessageSpec(cbcMessage));
        
        foreach (var cbcBody in cbcMessage.CbcBody)
        {
            var reportingEntity = cbcBody.ReportingEntity;
            var reportingEntityHeader = CreateReportingEntityHeader();
            var reportingEntityRowAndData = CreateReportingEntityRowAndData(reportingEntity);
            if (cbcBody.CbcReports != null && cbcBody.CbcReports.Any())
            {
                reportingEntityRowAndData.AppendFormat("<tr><td {0}></td><td colspan=\"11\">", GetColumnAttributes());
                var cbcReportHeaderRow = CreatCbCReportHeader();
                var cbcReportDataRows = CreateCbCReportRowsAndData(cbcBody);
                reportingEntityRowAndData.AppendFormat("<table width=\"100%\">{0}{1}</table></td></tr>",cbcReportHeaderRow, cbcReportDataRows);
            }
            overAllReport.Append(reportingEntityHeader);
            overAllReport.Append(reportingEntityRowAndData);

            //if (cbcBody.AdditionalInfo != null && cbcBody.AdditionalInfo.Any())
            //{
            //    var additionalInfoHeaderRow = CreatAdditionalInfoHeader();
            //    var additionalInfoDataRow = new StringBuilder();
            //    foreach (var infoType in cbcBody.AdditionalInfo)
            //    {
            //        additionalInfoDataRow.Append(CreatAdditionalInfoRowAndData(infoType));
            //    }

            //    overAllReport.AppendFormat("<table width=\"100%\">{0}{1}</table></td></tr>", additionalInfoHeaderRow, additionalInfoDataRow);
            //}

        }
        overAllReport.Append("</table></div>");
        Export(overAllReport);
    }
    private static string CreateMessageSpec(CBC_OECD cbcMessage)
    {
        var messageSpec = new StringBuilder();
        messageSpec.AppendFormat("<div><table {0}>", GetTableAttributes());

        messageSpec.Append("<tr>");
        messageSpec.Append("<td colspan=\"12\">");


        messageSpec.AppendFormat("<table {0}>", GetTableAttributes());
        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Transmitting Country</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.TransmittingCountry, GetColumnAttributes());
        messageSpec.Append("</tr>");
        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Message Type</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.MessageType, GetColumnAttributes());
        messageSpec.Append("</tr>");

        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Language</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.Language, GetColumnAttributes());
        messageSpec.Append("</tr>");

        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Warning</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.Warning, GetColumnAttributes());
        messageSpec.Append("</tr>");

        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Contact</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.Contact, GetColumnAttributes());
        messageSpec.Append("</tr>");

        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Message Ref Id</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.MessageRefId, GetColumnAttributes());
        messageSpec.Append("</tr>");

        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Message Type Indicator</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1}>{0}</td>", cbcMessage.MessageSpec.MessageTypeIndic, GetColumnAttributes());
        messageSpec.Append("</tr>");
        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Reporting Period</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1} class=\"sdate\">{0}</td>", cbcMessage.MessageSpec.ReportingPeriod, GetColumnAttributes());
        messageSpec.Append("</tr>");
        messageSpec.Append("<tr>");
        messageSpec.AppendFormat("<th nowrap {0}>Timestamp</th>", GetColumnAttributes());
        messageSpec.AppendFormat("<td nowrap {1} class=\"sdate\">{0}</td>", cbcMessage.MessageSpec.Timestamp, GetColumnAttributes());
        messageSpec.Append("</tr>");
        messageSpec.Append("<table>");
        messageSpec.Append("<td>"); 
        messageSpec.Append("</tr>");
        return messageSpec.ToString();
    }
    private static string CreateReportingEntityHeader()
    {
        var reportingEntity = new StringBuilder();
        reportingEntity.AppendFormat("<tr><td colspan=\"12\" {0}></td></tr>", GetEmptyRowStyle());
        reportingEntity.AppendFormat("<TR {0}>", GetHeaderRowAttributes());

        reportingEntity.AppendFormat("<TH nowrap {0}>Reporting Entity Name</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Res CountryCode</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Reporting Role</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>TIN</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>TIN Issued By</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>IN</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>IN Issued By</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Address</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Address Country Code</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Doc RefId</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Doc Type Indic</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("<TH nowrap {0}>Corr Doc RefId</TH>", GetColumnAttributes());
        reportingEntity.AppendFormat("</TR>");
        return reportingEntity.ToString();
    }
    private static StringBuilder CreateReportingEntityRowAndData(CorrectableReportingEntity_Type reportingEntity)
    {
        var reportingEntityBuilder = new StringBuilder();

        var trimCahrs = new[] {'|', ' '};

        var IN = string.Empty;
        var inIssuedBy = string.Empty;
        if (reportingEntity.Entity.IN != null && reportingEntity.Entity.IN.Any())
        {
            foreach (var _in in reportingEntity.Entity.IN)
            {
                IN = IN + _in.Value + "|";
                inIssuedBy = _in.issuedBySpecified ? inIssuedBy + _in.issuedBy + "|" : string.Empty;
            }
        }
        var entityName = string.Empty;
        if (reportingEntity.Entity.Name != null && reportingEntity.Entity.Name.Any())
        {
            foreach (var name in reportingEntity.Entity.Name)
            {
                entityName = entityName + name.Value + "| ";
            }
        }
        string entityAddressCountryCode;
        var entityAddress = ExtractAddress(reportingEntity, out entityAddressCountryCode);

        
        reportingEntityBuilder.AppendFormat("<tr {0}>", GetRowAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityName.Trim(trimCahrs), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>",string.Join(",", reportingEntity.Entity.ResCountryCode), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.ReportingRole, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.Entity.TIN != null? reportingEntity.Entity.TIN.Value : string.Empty, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.Entity.TIN != null ? (reportingEntity.Entity.TIN.issuedBySpecified ? reportingEntity.Entity.TIN.issuedBy.ToString() : string.Empty) : string.Empty, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", IN.TrimEnd(trimCahrs), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", inIssuedBy.TrimEnd(trimCahrs), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityAddress.Trim(trimCahrs), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityAddressCountryCode.Trim(trimCahrs), GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.DocSpec.DocRefId, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.DocSpec.DocTypeIndic, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", reportingEntity.DocSpec.CorrDocRefId, GetReportingEntityColumnAttributes());
        reportingEntityBuilder.Append("</tr>");
        return reportingEntityBuilder;
    }
    private static StringBuilder CreatCbCReportHeader()
    {
        var cbcReportHeader = new StringBuilder();
        cbcReportHeader.AppendFormat("<TR {0}>", GetHeaderRowAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Res CountryCode</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Unrelated</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Related</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Total</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>ProfitOrLoss</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>TaxPaid</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>TaxAccrued</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Capital</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Earnings</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Assets</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Nb Employees</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Doc RefId</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Doc Type Indic</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Corr Doc RefId</TH>", GetColumnAttributes());
        cbcReportHeader.Append("</TR>");

        return cbcReportHeader;
    }
    private static StringBuilder CreateCbCReportRowsAndData(CbcBody_Type cbcBody)
    {
        var reportRows = new StringBuilder();
        foreach (var report in cbcBody.CbcReports)
        {
            reportRows.AppendFormat("<tr {0}>", GetRowAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.ResCountryCode, GetColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Revenues.Unrelated.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Revenues.Related.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Revenues.Total.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.ProfitOrLoss.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.TaxPaid.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.TaxAccrued.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Capital.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Earnings.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.Assets.Value, GetCurrencyColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.Summary.NbEmployees, GetColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.DocSpec.DocRefId, GetColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.DocSpec.DocTypeIndic, GetColumnAttributes());
            reportRows.AppendFormat("<td nowrap {1}>{0}</td>", report.DocSpec.CorrDocRefId, GetColumnAttributes());
            reportRows.Append("</tr>");

           
            if (report.ConstEntities != null && report.ConstEntities.Any())
            {
                // var constEntityHeader = CreateConstEntityHeader();
                reportRows.AppendFormat("<tr><td {0}></td></tr>", GetEmptyRowStyle());
                reportRows.AppendFormat("<tr {0}>", GetRowAttributes());
                reportRows.Append("<td></td>");

                reportRows.Append("<td colspan=\"13\">");
                reportRows.Append("<table width=\"100%\">");
                reportRows.Append(CreateConstEntityHeader());
                foreach (var constEntity in report.ConstEntities) 
                {
                    reportRows.AppendFormat("{0}",  CreateConstEntityRowAndData(constEntity));
                }
                reportRows.Append("</table>");
                reportRows.Append("</td>");
                reportRows.Append("</tr>");
            }
        }
        return reportRows;
    }
    private static string CreateConstEntityHeader()
    {
        var constEntity = new StringBuilder();
        constEntity.AppendFormat("<TR {0}>", GetHeaderRowAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>Constituent Entity Name</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>Res CountryCode</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>TIN</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>TIN Issued By</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>IN</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>IN Issued By</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>Address</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>Address Country Code</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>IncorpCountryCode</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>BizActivities</TH>", GetColumnAttributes());
        constEntity.AppendFormat("<TH nowrap {0}>OtherEntityInfo</TH>", GetColumnAttributes());
        constEntity.AppendFormat("</TR>");
        return constEntity.ToString();
    }
    private static StringBuilder CreateConstEntityRowAndData(ConstituentEntity_Type constEntity)
    {
        var trimCahrs = new[] { '|', ' ' };
        var reportingEntityBuilder = new StringBuilder();
        var IN = string.Empty;
        var inIssuedBy = string.Empty;
        if (constEntity.ConstEntity.IN != null && constEntity.ConstEntity.IN.Any())
        {
            foreach (var _in in constEntity.ConstEntity.IN)
            {
                IN = IN + _in.Value + "|";
                inIssuedBy = _in.issuedBySpecified ? inIssuedBy + _in.issuedBy + "|" : string.Empty;
            }
        }

        var entityName = string.Empty;
        if (constEntity.ConstEntity.Name != null && constEntity.ConstEntity.Name.Any())
        {
            foreach (var name in constEntity.ConstEntity.Name)
            {
                entityName = entityName + name.Value + "| ";
            }
        }
        string entityAddressCountryCode;
        var entityAddress = ExtractAddress(constEntity, out entityAddressCountryCode);

        var bizActivities = string.Empty;
        if (constEntity.BizActivities != null && constEntity.BizActivities.Any())
        {
            foreach (var activity in constEntity.BizActivities)
            {
                bizActivities = bizActivities + activity + "| ";
            }
        }
       
      

        reportingEntityBuilder.AppendFormat("<tr {0}>", GetRowAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityName.Trim(trimCahrs), GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", string.Join(",", constEntity.ConstEntity.ResCountryCode), GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", constEntity.ConstEntity.TIN.Value,GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", constEntity.ConstEntity.TIN.issuedBySpecified ? constEntity.ConstEntity.TIN.issuedBy.ToString() : string.Empty,GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", IN.TrimEnd(trimCahrs), GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", inIssuedBy.TrimEnd(trimCahrs),GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityAddress.Trim(trimCahrs),GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", entityAddressCountryCode.Trim(trimCahrs),GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", constEntity.IncorpCountryCodeSpecified ? constEntity.IncorpCountryCode.ToString() : string.Empty,GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", bizActivities.Trim(trimCahrs),GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", constEntity.OtherEntityInfo,GetColumnAttributes());
        reportingEntityBuilder.Append("</tr>");
        return reportingEntityBuilder;
    }
    private static StringBuilder CreatAdditionalInfoHeader()
    {
        var cbcReportHeader = new StringBuilder();
        cbcReportHeader.AppendFormat("<TR {0}>", GetHeaderRowAttributes());

        cbcReportHeader.AppendFormat("<TH nowrap {0}>Doc RefId</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Doc Type Indic</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Corr Doc RefId</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Other Info</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Res Country Code</TH>", GetColumnAttributes());
        cbcReportHeader.AppendFormat("<TH nowrap {0}>Summary Ref</TH>", GetColumnAttributes());
        cbcReportHeader.Append("</TR>");

        return cbcReportHeader;
    }
    private static StringBuilder CreatAdditionalInfoRowAndData(CorrectableAdditionalInfo_Type additionalInfo)
    {
        var reportingEntityBuilder = new StringBuilder();
        var trimCahrs = new[] { '|', ' ' };
        var IN = string.Empty;
        var resCountyCode = string.Empty;
        if (additionalInfo.ResCountryCode != null && additionalInfo.ResCountryCode.Any())
        {
            foreach (var code in additionalInfo.ResCountryCode)
            {
                resCountyCode = resCountyCode + code  + "|"; 
            }
        }
        var summaryRef = string.Empty;
        if (additionalInfo.SummaryRef != null && additionalInfo.SummaryRef.Any())
        {
            foreach (var summary in additionalInfo.SummaryRef)
            {
                summaryRef = summaryRef + summary  + "| ";
            }
        }

        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", additionalInfo.DocSpec.DocRefId, GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", additionalInfo.DocSpec.DocTypeIndic, GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", additionalInfo.DocSpec.CorrDocRefId, GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", additionalInfo.OtherInfo, GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", resCountyCode.Trim(trimCahrs), GetColumnAttributes());
        reportingEntityBuilder.AppendFormat("<td nowrap {1}>{0}</td>", summaryRef.Trim(trimCahrs), GetColumnAttributes());
        reportingEntityBuilder.Append("</tr>");
        return reportingEntityBuilder;
    }
    private static string ExtractAddress(CorrectableReportingEntity_Type reportingEntity, out string addressCountryCode)
    {
        var entityAddress = string.Empty;
        addressCountryCode = string.Empty;
        var entityAddresses = reportingEntity.Entity.Address;
        foreach (var address in entityAddresses)
        {
            foreach (var addressItem in address.Items)
            {
                if (addressItem is AddressFix_Type)
                {
                    var addFixed = (AddressFix_Type) addressItem;
                    entityAddress = addFixed.Street;
                    entityAddress += string.Format(" {0}", addFixed.BuildingIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.SuiteIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.FloorIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.DistrictName);
                    entityAddress += string.Format(" {0}", addFixed.POB);
                    entityAddress += string.Format(" {0}", addFixed.PostCode);
                    entityAddress += string.Format(" {0}", addFixed.City);
                    entityAddress += string.Format(" {0}", addFixed.CountrySubentity);
                }
                else
                {
                    entityAddress = entityAddress + addressItem + "|";
                }
            }
            addressCountryCode += address.CountryCode + "|";
        }
        return entityAddress;
    }
    private static string ExtractAddress(ConstituentEntity_Type constEntity, out string addressCountryCode)
    {
        var entityAddress = string.Empty;
        addressCountryCode = string.Empty;
        var entityAddresses = constEntity.ConstEntity.Address;
        foreach (var address in entityAddresses)
        {
            foreach (var addressItem in address.Items)
            {
                if (addressItem is AddressFix_Type)
                {
                    var addFixed = (AddressFix_Type)addressItem;
                    entityAddress = addFixed.Street;
                    entityAddress += string.Format(" {0}", addFixed.BuildingIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.SuiteIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.FloorIdentifier);
                    entityAddress += string.Format(" {0}", addFixed.DistrictName);
                    entityAddress += string.Format(" {0}", addFixed.POB);
                    entityAddress += string.Format(" {0}", addFixed.PostCode);
                    entityAddress += string.Format(" {0}", addFixed.City);
                    entityAddress += string.Format(" {0}", addFixed.CountrySubentity);
                }
                else
                {
                    entityAddress = entityAddress + addressItem + "|";
                }
            }
            addressCountryCode += address.CountryCode + "|";
        }
        return entityAddress;
    }
    private static string GetTableAttributes()
    {
        return
            "cellspacing=\"2\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:silver; border-width:1px; border-style:solid; width:100%;\"";
    }
    private static string GetRowAttributes()
    {
        return
            "style=\"border-color:silver; border-width:1px;border-style:solid;font-family:Verdana;font-size:XX-Small;white-space:nowrap;\"";
    }
    private static string GetEmptyRowStyle()
    {
        return "style=\"font-weight: bold;font-style: inherit;text-left: center;color: red;\"";
    }
    private static string GetHeaderRowAttributes()
    {
        return
            "align=\"left\" style=\"color:black;background-color:khaki;border-color:silver;border-width:1px;border-style:solid;font-size:X-Small;font-weight:bold;height:25px;white-space:nowrap;\"";
    }
    private static string GetColumnAttributes()
    {
        return " class=\"text\" style=\"text-align:left;\"";
    }
    private static string GetReportingEntityColumnAttributes()
    {
        return " class=\"text\" style=\"text-align:left;font-weight: bold;color: blue;\"";
    }
    private static string GetCurrencyColumnAttributes()
    {
        return " class=\"currency\" style=\"text-align:left;\"";
    }
    private static string GetReporteaderStyle()
    {
        return
            @"<style> .text { mso-number-format:\@;} </style> <style> .sdate { mso-number-format:'Short Date'; } </style> <style> .ldate { mso-number-format:'Long Date'; } </style> <style> .percentage { mso-number-format:'Percent'; } </style> <style> .currency { mso-number-format:'Currency'; text-align:left;} </style>";
    }
    private void Export(StringBuilder html)
    {
        var gridView = new HtmlGenericControl {InnerHtml = html.ToString()};

        var response = HttpContext.Current.Response;
        var writer = new StringWriter();
        var writer2 = new HtmlTextWriter(writer);
        if (gridView.Page != null)
        {
            var child = gridView.Page.Form;
            if (child != null)
            {
                child.Controls.Clear();
                child.Controls.Add(gridView);
                gridView.Page.Controls.Add(child);
            }
            //writer2.WriteLine(reportHeading);
            //if (child != null)
            //{
            //    child.RenderControl(writer2);
            //}
        }
        else
        {
            //writer2.WriteLine(reportHeading);
            gridView.RenderControl(writer2);
        }
        response.Clear();
        response.Buffer = true;
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("content-disposition", string.Format("attachment;filename=Report_{0}.xls", DateTime.Now.ToFileTime()));
        response.Charset = "";
        response.Write(@"<style> .text { mso-number-format:\@; } </style> ");
        response.Write("<style> .sdate { mso-number-format:'Short Date'; } </style> ");
        response.Write("<style> .ldate { mso-number-format:'Long Date'; } </style> ");
        response.Write("<style> .percentage { mso-number-format:'Percent'; } </style> ");
        response.Write("<style> .currency { mso-number-format:'Currency'; text-align:left;} </style> ");
        response.Write(writer.ToString());
        response.End();
        
    }
}