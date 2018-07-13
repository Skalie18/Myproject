using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Sars.Systems.Data;
using System.Data;
using System.Xml;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Sars.Systems.Security;

[WebService(Namespace = "http://www.sars.gov.za/enterpriseMessagingModel/ThirdPartyData/ThirdPartyDataActivityManagement/xml/schemas/version/1.0", Name = "FDR Web Service", Description = "This service provides a list of MNE's")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class mnelistservice : WebService
{
    [WebMethod(Description = "Get a list of MNE's")]
    public List<MultiNationalEntity> GetList()
    {
        return DatabaseReader.GetMultiNationalEntities().ToList();
    }
    [WebMethod(Description = "Get a specific entity given the tax reference number")]
    public MultiNationalEntity GetEntity(string taxRefNo)
    {
        var refno = string.IsNullOrEmpty(taxRefNo) ? null : taxRefNo;
        var entities = DatabaseReader.GetMultiNationalEntities(refno).ToList();
        if (entities.Any())
        {
            return entities.First();
        }
        return null;
    }
    [WebMethod]
    public int AddNewEntity(MultiNationalEntity mne)
    {
        if (mne == null) { return -100; }
        return mne.Save();
    }
    [WebMethod]
    public int AddNewMne(
        decimal partyId
        , string taxpayerReferenceNumber
        , int yearofAssessment
        , string registeredName
        , string tradingName
        , string registrationNumber
        , DateTime financialYearEnd
        , decimal turnoverAmount
        , string nameUltimateHoldingCo
        , string ultimateHoldingCompanyResOutSaInd
        , string taxResidencyCountryCodeUltimateHoldingCompany
        , string ultimateHoldingCoIncomeTaxRefNo
        , string masterLocalFileRequiredInd
        , string cbCReportRequiredInd
        , DateTime datestamp)
    {
        return DatabaseWriter.SaveNewEntity(
            partyId,
            taxpayerReferenceNumber,
            yearofAssessment,
            registeredName,
            tradingName,
            registrationNumber,
            financialYearEnd,
            turnoverAmount,
            nameUltimateHoldingCo,
            ultimateHoldingCompanyResOutSaInd,
            taxResidencyCountryCodeUltimateHoldingCompany,
            ultimateHoldingCoIncomeTaxRefNo,
            masterLocalFileRequiredInd,
            cbCReportRequiredInd,
            datestamp
            );
    }
    [WebMethod(Description = "Inquire MNE submission status")]
    public ThirdPartyDataActivityManagementResponse Enquire(
        // string date, int year, string period, 
        string taxrefno)
    {
        var data = DatabaseReader.EnquireMne(taxrefno);
        if (data.HasRows)
        {
            var row = data[0];
            return new ThirdPartyDataActivityManagementResponse
            {
                CBC = new CbcReturnDetails
                {
                    SubmitMasterAndLocalFileInd = row["MasterLocalFileRequiredInd"].ToString(),
                    SubmitCbCDeclarationInd = row["CbCReportRequiredInd"].ToString()
                }
            };
        }
        return new ThirdPartyDataActivityManagementResponse { CBC = new CbcReturnDetails { } };
    }

    [WebMethod(Description = "Gets CBCFormData by Tax Reference No")]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object GetCBCDataByTaxRefNo(int incLocal,string taxRefNo=null, int year=0, decimal mspecId=0)
    {
        string xml = "";
        string jsonString = "";
        try
        {
            var dsResults = DatabaseReader.GetCBCDataByTaxRefNo(incLocal,taxRefNo, year, mspecId);
            if (dsResults.HasRows())
            {
                if (incLocal == 1)
                    xml = Common.GetIncomingCBCR(dsResults.Tables[0].Rows[0][0].ToString());
                else
                    xml = Common.GetForeignIncomingCBCR(dsResults.Tables[0].Rows[0][0].ToString(), dsResults.Tables[0].Rows[0][1].ToString());
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                jsonString = JsonConvert.SerializeXmlNode(doc);
            }

        }
        catch (Exception x)
        {
            throw new Exception(x.Message);
        }
        return jsonString;
    }


    [WebMethod(Description = "Gets All CBCFormData")]
    public DataSet GetAllCBCData()
    {
        return DatabaseReader.GetAllCbcData();
    }

    [WebMethod(Description = "Save CBCFormData")]
    public DataSet GetUnprocessedCBCData()
    {
        return DatabaseReader.GetUnprocessedCbcData();

    }

    [WebMethod(Description = "Approve/Reject CBC Form")]
    public void ApproveCBCReport(string taxRefNo, int year, int approved, string userId)
    {
        var result = DatabaseWriter.UpdateCBCStatus(approved, taxRefNo, year, userId);

        
        if (approved == 1)
        {
            string Subject = "";
            var email = string.IsNullOrEmpty(ADUser.CurrentUser.Mail) ? "fdr@sars.gov.za" : ADUser.CurrentUser.Mail;
            string[] senderEmail = { email };
            if (result.HasRows)
            {
                DateTime repperiod = Convert.ToDateTime(result.Tables[0].Rows[0]["ReportingPeriod"].ToString());
                var reportingPeriod = repperiod.ToString("yyyy-MM-dd");
                Subject = string.Format("New data came in for the reporting period {0} ", reportingPeriod);
                Common.SendEmailToUsers(result, reportingPeriod, Subject, FDRPage.Statuses.DeletePackage, senderEmail);
                Common.SendEmailToRole("Approver", reportingPeriod, Subject, FDRPage.Statuses.DeletePackage, senderEmail);
            }
            else
            {
                // DatabaseWriter.ApproveOutgoingCBC(outCBC.Id, countryCode, year, statusId, ADUser.CurrentSID);
                
                Subject = string.Format("CBC for Tax No: {0} has been Accepted ", taxRefNo);
                Common.SendEmailToRole("Reviewer", taxRefNo, Subject, FDRPage.Statuses.Accepted, senderEmail);
            }
        }
    }
}