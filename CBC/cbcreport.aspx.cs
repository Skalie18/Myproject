using System;
using Sars.Models.CBC;

public partial class cbcreport : FormBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("cbc01.aspx");
    }

    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        var taxUserId = CommonFunctions.TaxUserId;
        var taxPayerId = CommonFunctions.TaxPayerId;
        var formId = DirectiveFormId;
        PersistData(taxPayerId, taxPayerId);
    }

    private void PersistData(string taxUserID, string taxPayerID)
    {
        //report summary object
        decimal cbcReportId = SessionStorage.cbcReportId == null ? 0 : decimal.Parse(SessionStorage.cbcReportId);
        decimal reportSummaryId = SessionStorage.reportSummaryId == null ? 0 : decimal.Parse(SessionStorage.reportSummaryId);
        if (cbcReportId > 0)
        {
            var reportSummary = new CBCReport_Summary()
            {
                ID = reportSummaryId,
                CBCReportId = cbcReportId,
                ProfitLossBeforeIncomeTax = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.ProfitLossBeforeIT.Text) ? "0" : ctrCBCReportSummary.ProfitLossBeforeIT.Text),
                ProfitLossBeforeIncomeTaxCurrencyCode = ctrCBCReportSummary.ProfitLossBeforeITCurrencyCode.SelectedValue,
                StatedCapital = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.StatedCapital.Text) ? "0" : ctrCBCReportSummary.StatedCapital.Text),
                StatedCapitalCurrencyCode = ctrCBCReportSummary.StatedCapitalCurrencyCode.SelectedValue,
                IncomeTaxPaid = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.ITPaid.Text) ? "0" : ctrCBCReportSummary.ITPaid.Text),
                IncomeTaxPaidCurrencyCode = ctrCBCReportSummary.ITPaidCurrencyCode.SelectedValue,
                AccumulatedEarnings = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.AccumulatedEarnings.Text) ? "0" : ctrCBCReportSummary.AccumulatedEarnings.Text),
                AccumulatedEarningsCurrencyCode = ctrCBCReportSummary.AccumulatedEarningsCurrencyCode.SelectedValue,
                IncomeTaxAccrued = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.ITAccrued.Text) ? "0" : ctrCBCReportSummary.ITAccrued.Text),
                IncomeTaxAccruedCurrencyCode = ctrCBCReportSummary.ITAccruedCurrencyCode.SelectedValue,
                Assets = decimal.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.AssetsEarnings.Text) ? "0" : ctrCBCReportSummary.AssetsEarnings.Text),
                AssetsCurrencyCode = ctrCBCReportSummary.AssetsEarningsCurrencyCode.SelectedValue,
                NoOfEmployees = int.Parse(string.IsNullOrEmpty(ctrCBCReportSummary.NumberOfEmployees.Text) ? "0" : ctrCBCReportSummary.NumberOfEmployees.Text),
                TaxUserID = taxUserID
            };
            reportSummaryId = db.SaveReportSummary(reportSummary);
            if (reportSummaryId > 0)
            {
                SessionStorage.reportSummaryId = reportSummaryId.ToString();
            }


            //revenue object
            decimal revenueId = SessionStorage.revenueId == null ? 0 : decimal.Parse(SessionStorage.revenueId);
            reportSummaryId = SessionStorage.reportSummaryId == null ? 0 : decimal.Parse(SessionStorage.reportSummaryId);
            if (reportSummaryId > 0)
            {
                var revenues = new CBCReport_Revenue()
                {
                    ID = revenueId,
                    CbCReportSummaryId = reportSummaryId,
                    Unrelated = decimal.Parse(string.IsNullOrEmpty(ctrCBCRevenues.Unrelated.Text) ? "0" : ctrCBCRevenues.Unrelated.Text),
                    UnrelatedCurrencyCode = ctrCBCRevenues.UnrelatedCurrency.SelectedValue,
                    Related = decimal.Parse(string.IsNullOrEmpty(ctrCBCRevenues.Unrelated.Text) ? "0" : ctrCBCRevenues.Related.Text),
                    RelatedCurrencyCode = ctrCBCRevenues.RelatedCurrency.SelectedValue,
                    Total = decimal.Parse(string.IsNullOrEmpty(ctrCBCRevenues.Total.Text) ? "0" : ctrCBCRevenues.Total.Text),
                    TotalCurrencyCode = ctrCBCRevenues.TotalCurrency.SelectedValue,
                };

                revenueId = db.SaveRevenues(revenues);
                if (revenueId > 0)
                {
                    SessionStorage.revenueId = revenueId.ToString();
                }
            }

            decimal constituentEntityId = SessionStorage.constituentEntityId == null ? 0 : decimal.Parse(SessionStorage.constituentEntityId);
            var constituentEntity = new CBCReport_ConstituentEntity()
            {
                ID = constituentEntityId,
                CBCReportId = cbcReportId,
                NumberOfContituentEntities = 0
            };
            constituentEntityId = db.SaveConstituentEntity(constituentEntity);
            if (constituentEntityId>0)
            {
                SessionStorage.constituentEntityId = constituentEntityId.ToString();
            }
        }
    }

    private void PersistConstituentEntityData(string taxUserID, string taxPayerID)
    {
        decimal constituentEntityId = SessionStorage.constituentEntityId == null ? 0 : decimal.Parse(SessionStorage.constituentEntityId);
        decimal constituentEntityDataId = SessionStorage.constituentEntityDataId == null ? 0 : decimal.Parse(SessionStorage.constituentEntityDataId);

        if (constituentEntityId > 0)
        {
            var constituentEntityData = new CBCReport_ConstituentEntityData()
            {
                ID = constituentEntityDataId,
                ConstituentEntityId = constituentEntityId,
                RegisteredName = ctrCBCConstituentEntityDetails.txtRegisteredName.Text,
                CompanyRegNo = ctrCBCConstituentEntityDetails.txtCompanyRegNumber.Text,
                CompanyRegNoIssuedByCountry = ctrCBCConstituentEntityDetails.ddlCompanyRegNumberIssuedByCountry.SelectedValue,
                TaxRefNo = ctrCBCConstituentEntityDetails.txtTaxRefNo.Text,
                TaxRefNoIssuedByCountry = ctrCBCConstituentEntityDetails.ddlTaxRefNoIssuedByCountry.SelectedValue
            };

            constituentEntityDataId = db.SaveConstituentEntityData(constituentEntityData);
            if (constituentEntityDataId > 0)
            {
                SessionStorage.constituentEntityDataId = constituentEntityDataId.ToString();
            }


            var addressId = SessionStorage.addressId == null ? 0 : decimal.Parse(SessionStorage.addressId);
            var address = new CBCReport_Address()
            {
                ID = addressId,
                CBCReport_ConstituentEntityDataId = constituentEntityDataId,
                AddressTypeCode = ctrCBCAddress.AddressType.SelectedValue,
                PostalServiceID = 1,
                OtherPOSpecialService = ctrCBCAddress.OtherSpecify.Text,
                Number = ctrCBCAddress.AddressNumber.Text,
                PostOffice = ctrCBCAddress.PostOffice.Text,
                PAPostalCode = ctrCBCAddress.ResPostalCode.Text,
                PACountryCode = ctrCBCAddress.PostOfficeCountryCode.SelectedText,
                UnitNo = ctrCBCAddress.UnitNumber.Text,
                Complex = ctrCBCAddress.ComplexName.Text,
                StreetNo = ctrCBCAddress.StreetNo.Text,
                StreetName = ctrCBCAddress.StreetOrFarmName.Text,
                Suburb = ctrCBCAddress.SuburbOrDistrict.Text,
                City = ctrCBCAddress.CityOrTownName.Text,
                RESPostalCode = ctrCBCAddress.ResPostalCode.Text,
                RESCountryCode = ctrCBCAddress.ResAddressCountryCode.SelectedValue
            };
            addressId = db.SaveCBCReportAddress(address);
            if (addressId>0)
            {
                SessionStorage.addressId = addressId.ToString();
            }

            decimal businessActivityId = SessionStorage.businessActivityId == null ? 0 : decimal.Parse(SessionStorage.businessActivityId);
            var businessActivity = new CBCReport_BusinessActivities()
            {
                ID = businessActivityId,
                CBCReport_ConstituentEntityDataId= 0,
                BusinessActivitiesCode = ctrCBCBusinessActivity.ddlMainBusinessActivity.SelectedValue,
                OtherEntityInfo = ctrCBCBusinessActivity.txtOtherEntityInformation.Text
            };

            businessActivityId = db.SaveBusinessActivities(businessActivity);
            if (businessActivityId > 0)
            {
                SessionStorage.businessActivityId = businessActivityId.ToString();
            }
        }
    }
}