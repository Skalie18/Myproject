using System;
using System.Drawing;
using Sars.Models.CBC;
using Sars.Systems.Controls;
using Sars.Systems.Data;
using System.Web.UI.WebControls;

[Form("CBC01", "[dbo].[usp_READ_FORMAD_RequestDATA]", AllowEdit = true)]
public partial class CBC01 : FormBase
{
    protected const string FormName = "CBC01";
    protected string FullName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulateDropDowns();
            PopulateFormData();
            PopulateGrids();
        }
    }

    private void GetEntityData(string taxUserId)
    {
        var entityData = db.GetReportEntityByUserID(taxUserId);
        if (entityData != null)
        {
            SessionStorage.reportEntityId = entityData.ID.ToString();
            txtReportingPeriod.SetValue(entityData.ReportingPeriod);
            txtREgisteredName.SetValue(entityData.RegisteredName);
            txtREgisteredName.SetValue(entityData.RegisteredName);
            ddlCompanyRegIssuedByCountry.SetValue(entityData.CompanyRegNoIssuedByCountry);
            txtTexRefNo.SetValue(entityData.TaxRefNo);
            ddlTexRefNoIssuedByCountry.SetValue(entityData.TaxRefNoIssuedByCountry);
            txtGIINNo.SetValue(entityData.GIINNo);
            rbtnGIINNoAvailable.SetValue(entityData.GIINNoIndicator);
            ddlGIINNoIssuedByCountry.SetValue(entityData.GIINNoIssuedByCountry);
            txtUniqueNo.SetValue(entityData.UniqueNo);
            ddlRole.SetValue(entityData.ReportingRoleId);
            ddlResCountry.SetValue(entityData.ResidentCountryCode);
            SessionStorage.entityAddressId = entityData.ReportEntityAddressId.ToString();
            SessionStorage.contactPersonDetailsId = entityData.ReportEntityContactPersonDetailsId.ToString();
        }
    }

    private void GetEntityAddress()
    {
        var entityAddress = db.GetEntityAddressByID(SessionStorage.entityAddressId);
        if (entityAddress != null)
        {
             ctrEntityAddress.AddressType.SetValue(entityAddress.AddressTypeCode);
             ctrEntityAddress.AddressOption.SetValue(entityAddress.PostalServiceID);
             ctrEntityAddress.PostOffice.SetValue(entityAddress.PostOffice);
             ctrEntityAddress.OtherSpecify.SetValue(entityAddress.OtherPOSpecialService);
             ctrEntityAddress.AddressNumber.SetValue(entityAddress.Number);
             ctrEntityAddress.PostOfficePostCode.SetValue(entityAddress.PAPostalCode);
             ctrEntityAddress.ResPostalCode.SetValue(entityAddress.RESPostalCode);
             ctrEntityAddress.PostOfficeCountryCode.SetValue(entityAddress.PACountryCode);
             ctrEntityAddress.ResAddressCountryCode.SetValue(entityAddress.RESCountryCode);
             ctrEntityAddress.UnitNumber.SetValue(entityAddress.UnitNo);
             ctrEntityAddress.ComplexName.SetValue(entityAddress.Complex);
             ctrEntityAddress.StreetNo.SetValue(entityAddress.StreetNo);
             ctrEntityAddress.StreetOrFarmName.SetValue(entityAddress.StreetName);
             ctrEntityAddress.SuburbOrDistrict.SetValue(entityAddress.Suburb);
             ctrEntityAddress.CityOrTownName.SetValue(entityAddress.City);
        }
    }

    private void GetContactPerson()
    {
        var contactPerson = db.GetContactPersonByID(SessionStorage.contactPersonDetailsId);
        if (contactPerson != null)
        {
             txtContactPersonFirstName.SetValue(contactPerson.FirstNames);
             txtContactPersonSurname.SetValue(contactPerson.Surname);
            txtContactPersonBusinessTel1.SetValue(contactPerson.BusTelNo1);
            txtContactPersonBusinessTel2.SetValue(contactPerson.BusTelNo2);
            txtContactPersonEmailAddress.SetValue(contactPerson.CellNo);
            txtContactPersonEmailAddress.SetValue(contactPerson.EmailAddress);
        }
    }

    private void GetCBCReport()
    {
        var cbcReport = db.GetCBCReportByID(SessionStorage.reportEntityId);
        if (cbcReport != null)
        {
            //.SetValue(cbcReport.NumberOfTaxJurisdictions);
            txtNumberOfTaxJurisdiction.SetValue(cbcReport.NumberOfTaxJurisdictions);
            txtCBCReportUniqueNo.SetValue(cbcReport.UniqueNo);
            SetRecordStatus(cbcReport.RecordStatusId);
            ddlCBCReportCountryCode.SetValue(cbcReport.ResidentCountryCode);
            ddlCBCReportCurrencyCode.SetValue(cbcReport.CurrencyCode);
            SessionStorage.cbcReportId = cbcReport.ID.ToString();

        }

       
    }

    private void GetSummaryReport()
    {

    }


    private void PopulateGrids()
    {
        gvSummary.Bind(db.GetReportSummaryByID(SessionStorage.cbcReportId));
        gvSummary.DataBind();
    }
    private void PopulateFormData()
    {
        var taxUserId = CommonFunctions.TaxUserId;
        var taxPayerId = CommonFunctions.TaxPayerId;
        var formId = DirectiveFormId;
        GetEntityData(taxUserId);
        if (SessionStorage.entityAddressId != null)
        {
            //GetEntityAddress();
        }
        if (SessionStorage.contactPersonDetailsId != null)
        {
            GetContactPerson();
        }

        if (SessionStorage.reportEntityId != null)
        {
            GetCBCReport();
        }
    }
    private void PopulateDropDowns()
    {
        //Reporting Roles
        ddlRole.Bind(db.GetAllReportingRoles(), "Description", "RoleID");

        //Summary Reference Codes
        ddlSummaryCode1.Bind(db.GetAllSummaryRefCodes(), "Description", "Code");
        ddlSummaryCode2.Bind(db.GetAllSummaryRefCodes(), "Description", "Code");

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        base.ChangeAllFieldBackColor(Color.Empty);


        var taxUserId = CommonFunctions.TaxUserId;
        var taxPayerId = CommonFunctions.TaxPayerId;
        var formId = DirectiveFormId;

        var contactPerson = new ReportEntityContactPersonDetails
        {
            FirstNames = txtContactPersonFirstName.Text,
            Surname = txtContactPersonSurname.Text,
            BusTelNo1 = txtContactPersonBusinessTel1.Text,
            BusTelNo2 = txtContactPersonBusinessTel2.Text,
            CellNo = txtContactPersonEmailAddress.Text,
            EmailAddress = txtContactPersonEmailAddress.Text,
            ID = 0

        };

       
        var entityAddress = new EntityAddress
        {

            AddressTypeCode = ctrEntityAddress.AddressType.SelectedValue,
            PostalServiceID = ctrEntityAddress.AddressOption.SelectedValue,
            PostOffice = ctrEntityAddress.PostOffice.Text,
            OtherPOSpecialService = ctrEntityAddress.OtherSpecify.Text,
            Number = ctrEntityAddress.AddressNumber.Text,
            PAPostalCode = ctrEntityAddress.PostOfficePostCode.Text,
            RESPostalCode = ctrEntityAddress.ResPostalCode.Text,
            PACountryCode = ctrEntityAddress.PostOfficeCountryCode.SelectedValue,
            RESCountryCode = ctrEntityAddress.ResAddressCountryCode.SelectedValue,
            UnitNo = ctrEntityAddress.UnitNumber.Text,
            Complex = ctrEntityAddress.ComplexName.Text,
            StreetNo = ctrEntityAddress.StreetNo.Text,
            StreetName = ctrEntityAddress.StreetOrFarmName.Text,
            Suburb = ctrEntityAddress.SuburbOrDistrict.Text,
            City = ctrEntityAddress.CityOrTownName.Text,
            ID = 0
            // PostalServiceID = ctrEntityAddress.AddressOption.SelectedValue
        };

        SaveReportEntity();
    }

    private bool IsDateBefore1stOctober2007(DateTime date)
    {
        var firstOctober2007 = new DateTime(2007, 10, 1);
        if (date < firstOctober2007)
        {
            return true;
        }
        return false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        //TODO: ADD CODE TO SAVE
        SaveReportEntity();
        if (true)
        {
            Response.Redirect("cbcreport.aspx");
        }
    }


    public void SaveReportEntity()
    {
        decimal contactPersonDetailsId = SessionStorage.contactPersonDetailsId == null ? 0 : decimal.Parse(SessionStorage.contactPersonDetailsId);
        decimal entityAddressId = SessionStorage.entityAddressId == null ? 0 : decimal.Parse(SessionStorage.entityAddressId);
        decimal reportEntityId = SessionStorage.reportEntityId == null ? 0 : decimal.Parse(SessionStorage.reportEntityId);

        base.ChangeAllFieldBackColor(Color.Empty);


        var taxUserId = CommonFunctions.TaxUserId;
        var taxPayerId = CommonFunctions.TaxPayerId;
        var formId = DirectiveFormId;

        var contactPerson = new ReportEntityContactPersonDetails
        {
            FirstNames = txtContactPersonFirstName.Text,
            Surname = txtContactPersonSurname.Text,
            BusTelNo1 = txtContactPersonBusinessTel1.Text,
            BusTelNo2 = txtContactPersonBusinessTel2.Text,
            CellNo = txtContactPersonEmailAddress.Text,
            EmailAddress = txtContactPersonEmailAddress.Text,
            ID = contactPersonDetailsId

        };

        var entityAddress = new EntityAddress
        {

            AddressTypeCode = ctrEntityAddress.AddressType.SelectedValue,
            PostalServiceID = ctrEntityAddress.AddressOption.SelectedValue,
            PostOffice = ctrEntityAddress.PostOffice.Text,
            OtherPOSpecialService = ctrEntityAddress.OtherSpecify.Text,
            Number = ctrEntityAddress.AddressNumber.Text,
            PAPostalCode = ctrEntityAddress.PostOfficePostCode.Text,
            RESPostalCode = ctrEntityAddress.ResPostalCode.Text,
            PACountryCode = ctrEntityAddress.PostOfficeCountryCode.SelectedValue,
            RESCountryCode = ctrEntityAddress.ResAddressCountryCode.SelectedValue,
            UnitNo = ctrEntityAddress.UnitNumber.Text,
            Complex = ctrEntityAddress.ComplexName.Text,
            StreetNo = ctrEntityAddress.StreetNo.Text,
            StreetName = ctrEntityAddress.StreetOrFarmName.Text,
            Suburb = ctrEntityAddress.SuburbOrDistrict.Text,
            City = ctrEntityAddress.CityOrTownName.Text,
            ID = entityAddressId
        };
        contactPersonDetailsId = db.SaveReportEntityContactPersonDetails(contactPerson);
        if (contactPersonDetailsId > 0)
        {
            SessionStorage.contactPersonDetailsId = contactPersonDetailsId.ToString();
        }
        entityAddressId = db.SaveReportEntityAddress(entityAddress);
        if (entityAddressId > 0)
        {
            SessionStorage.entityAddressId = entityAddressId.ToString();
        }

        string dt = Request.Form[txtReportingPeriod.UniqueID];

        contactPersonDetailsId = SessionStorage.contactPersonDetailsId == null ? 0 : decimal.Parse(SessionStorage.contactPersonDetailsId);
        entityAddressId = SessionStorage.entityAddressId == null ? 0 : decimal.Parse(SessionStorage.entityAddressId);
        var repEntity = new ReportEntity()
        {
            ID = reportEntityId,
            ReportingPeriod = dt,
            CompanyRegNo = txtREgisteredName.Text,
            RegisteredName = txtREgisteredName.Text,
            CompanyRegNoIssuedByCountry = ddlCompanyRegIssuedByCountry.SelectedValue,
            TaxRefNo = txtTexRefNo.Text,
            TaxRefNoIssuedByCountry = ddlTexRefNoIssuedByCountry.SelectedValue,
            GIINNo = txtGIINNo.Text,
            GIINNoIndicator = rbtnGIINNoAvailable.SelectedValue,
            GIINNoIssuedByCountry = ddlGIINNoIssuedByCountry.SelectedValue,
            UniqueNo = txtUniqueNo.Text,
            ReportingRoleId = ddlRole.SelectedValue,
            ResidentCountryCode = ddlResCountry.SelectedValue,
            ReportEntityAddressId = entityAddressId,
            ReportEntityContactPersonDetailsId = contactPersonDetailsId,
            RecordStatusId = GetRecordStatus(),
            TaxUserID = taxUserId
        };

        reportEntityId = db.SaveReportEntity(repEntity);
        if (reportEntityId > 0)
        {
            SessionStorage.reportEntityId = reportEntityId.ToString();
        }

        SaveCBCReport();
    }

    private void SaveCBCReport()
    {
        decimal cbcReportId = SessionStorage.cbcReportId == null ? 0 : decimal.Parse(SessionStorage.cbcReportId);
        decimal reportEntityId = SessionStorage.reportEntityId == null ? 0 : decimal.Parse(SessionStorage.reportEntityId);
        if (reportEntityId > 0)
        {
            var cbcReport = new CBCReports()
            {
                ID = cbcReportId,
                ReportEntityId = reportEntityId,
                NumberOfTaxJurisdictions = 0,
                UniqueNo = txtCBCReportUniqueNo.Text,
                RecordStatusId = GetRecordStatus(),
                ResidentCountryCode = ddlCBCReportCountryCode.SelectedValue,
                CurrencyCode = ddlCBCReportCurrencyCode.SelectedValue
            };

            cbcReportId = db.SaveCBCReport(cbcReport);
            if (cbcReportId > 0)
            {
                SessionStorage.cbcReportId = cbcReportId.ToString();
            }
        }
        else
        {

        }
    }

    private void SaveOtherInfo()
    {
        decimal otherInfoId = SessionStorage.otherInfoId == null ? 0 : decimal.Parse(SessionStorage.otherInfoId);
        decimal reportEntityId = SessionStorage.reportEntityId == null ? 0 : decimal.Parse(SessionStorage.reportEntityId);
        if (otherInfoId > 0)
        {
            var otherInfo = new CBCReport_OtherInformation()
            {
                ID = 0,
                AdditionalInformationId = 0,
                FurtherBriefInformation = txtComment.Text
            };

            otherInfoId = db.SaveOtherInfo(otherInfo);
            if (otherInfoId > 0)
            {
                SessionStorage.otherInfoId = otherInfoId.ToString();
            }
        }
    }

    private void SaveAdditionalInfo()
    {
        decimal otherInfoId = SessionStorage.otherInfoId == null ? 0 : decimal.Parse(SessionStorage.otherInfoId);
        decimal reportEntityId = SessionStorage.reportEntityId == null ? 0 : decimal.Parse(SessionStorage.reportEntityId);

        if (reportEntityId > 0)
        {
            var additionalInfo = new ReportEntityAdditionalInformation()
            {
                ID = 0,
                ReportEntitytId = reportEntityId,
                RecordStatusId = GetRecordStatus(),
                UniqueNo = txtUniqueNo.Text
            };

            otherInfoId = db.SaveAdditionalInfo(additionalInfo);
            if (otherInfoId > 0)
            {
                SessionStorage.otherInfoId = otherInfoId.ToString();
            }
        }
    }

    private void SaveCountrySummaryCodes()
    {
        decimal otherInfoId = SessionStorage.otherInfoId == null ? 0 : decimal.Parse(SessionStorage.otherInfoId);
        decimal countrySummaryCodeId = SessionStorage.countrySummaryCodeId == null ? 0 : decimal.Parse(SessionStorage.countrySummaryCodeId);

        if (otherInfoId > 0)
        {
            var CountrySummaryCode = new CBCReport_CoutryCodeSummaryRefCode()
            {
                ID = countrySummaryCodeId,
                OtherInformationId = otherInfoId,
                ResidenceCounryCode = "",
                SummaryRefCode = ""
            };

            otherInfoId = db.SaveCountrySummaryCodes(CountrySummaryCode);
            if (otherInfoId > 0)
            {
                SessionStorage.otherInfoId = otherInfoId.ToString();
            }
        }
    }
    private int GetRecordStatus()
    {
        if (rbtnRecodStatus.Items[0].Selected)
        {
            return 2;
        }
        else if (rbtnRecodStatus.Items[0].Selected)
        {
            return 3;
        }
        else
        {
            return 1;
        }
    }

    private void SetRecordStatus(int status)
    {
        if (status==2)
        {
            rbtnRecodStatus.Items[0].Selected =true;
        }
        else if (status == 3)
        {
            rbtnRecodStatus.Items[1].Selected = true;
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }



    protected void rbtnGIINNoAvailable_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnGIINNoAvailable.SelectedValue == "N")
        {
            txtGIINNo.Text = "";
            txtGIINNo.Enabled = false;
        }
        else
        {
            txtGIINNo.Enabled = true;
        }
    }

    protected void gvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        //var currencyCode = Sars.Systems.Da
    }

    protected void gvSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
}