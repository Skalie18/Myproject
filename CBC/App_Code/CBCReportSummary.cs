using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sars.Systems.Controls;
/// <summary>
/// Summary description for CBCReportSummary
/// </summary>
namespace CBCControls
{
    [Designer(typeof(CBCControlDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CBCReportSummary runat=\"server\"> </{0}:CBCReportSummary>")]
    public class CBCReportSummary : CompositeControl
    {
        public CBCReportSummary()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Unit DropdownListWith { get
            {
                return new Unit(100, UnitType.Percentage);
            }
        }
        CountryList _ResidentCountry;
        CountryList _ResedentCurrency;
        MoneyField _ProfitLossBeforeIT;
        CountryList _ProfitLossBeforeITCurrencyCode;

        MoneyField _ITPaid;
        CountryList _ITPaidCurrencyCode;

        MoneyField _ITAccrued;
        CountryList _ITAccruedCurrencyCode;

        MoneyField _StatedCapital;
        CountryList _StatedCapitalCurrencyCode;

        MoneyField _AccumulatedEarnings;
        CountryList _AccumulatedEarningsCurrencyCode;

        MoneyField _AssetsEarnings;
        CountryList _AssetsEarningsCurrencyCode;

        NumberField _NoOfEmployees;

        public CountryList ResidentCountry { get { return _ResidentCountry; } }
        public CountryList ResedentCurrency { get { return _ResedentCurrency; } }
        public MoneyField ProfitLossBeforeIT { get { return _ProfitLossBeforeIT; } }
        public CountryList ProfitLossBeforeITCurrencyCode { get { return _ProfitLossBeforeITCurrencyCode; } }

        public MoneyField ITPaid { get { return _ITPaid; } }
        public CountryList ITPaidCurrencyCode { get { return _ITPaidCurrencyCode; } }

        public MoneyField ITAccrued { get { return _ITAccrued; } }
        public CountryList ITAccruedCurrencyCode { get { return _ITAccruedCurrencyCode; } }

        public MoneyField StatedCapital { get { return _StatedCapital; } }
        public CountryList StatedCapitalCurrencyCode { get { return _StatedCapitalCurrencyCode; } }

        public MoneyField AccumulatedEarnings { get { return _AccumulatedEarnings; } }
        public CountryList AccumulatedEarningsCurrencyCode { get { return _AccumulatedEarningsCurrencyCode; } }

        public MoneyField AssetsEarnings { get { return _AssetsEarnings; } }
        public CountryList AssetsEarningsCurrencyCode { get { return _AssetsEarningsCurrencyCode; } }

        public NumberField NumberOfEmployees { get { return _NoOfEmployees; } }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();


            _ResidentCountry = new CountryList() { Width = new Unit(98, UnitType.Percentage) };
            _ResedentCurrency = new CountryList() { CodeType = CodeType.CurrencyCode, Width = DropdownListWith }; 
            _ProfitLossBeforeIT = new MoneyField() {  };
            _ProfitLossBeforeITCurrencyCode = new CountryList() { CodeType = CodeType.CurrencyCode, Width= DropdownListWith };

            _ITPaid = new MoneyField();
            _ITPaidCurrencyCode = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith };

            _ITAccrued = new MoneyField();
            _ITAccruedCurrencyCode = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith };

            _StatedCapital = new MoneyField();
            _StatedCapitalCurrencyCode = new CountryList() { CodeType = CodeType.CurrencyCode, Width = DropdownListWith };

            _AccumulatedEarnings = new MoneyField();
            _AccumulatedEarningsCurrencyCode = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith };

            _AssetsEarnings = new MoneyField();
            _AssetsEarningsCurrencyCode = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith };

            _NoOfEmployees = new NumberField();

            Controls.Add(_ResidentCountry);
            Controls.Add(_ResedentCurrency);

            Controls.Add(_ProfitLossBeforeIT);
            Controls.Add(_ProfitLossBeforeITCurrencyCode);
            Controls.Add(_ITPaid);
            Controls.Add(_ITPaidCurrencyCode);
            Controls.Add(_ITAccrued);
            Controls.Add(_ITAccruedCurrencyCode);
            Controls.Add(_StatedCapital);
            Controls.Add(_StatedCapitalCurrencyCode);
            Controls.Add(_AccumulatedEarnings);
            Controls.Add(_AccumulatedEarningsCurrencyCode);
            Controls.Add(_AssetsEarnings);
            Controls.Add(_AssetsEarningsCurrencyCode);
            Controls.Add(_NoOfEmployees);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // start table
            CBCConrolsHelper.CreateRow4Columns(writer, ResidentCountry, ResedentCurrency, "Resident Country code", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, ProfitLossBeforeIT, ProfitLossBeforeITCurrencyCode, "Profit/Loss before Income Tax", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, StatedCapital, StatedCapitalCurrencyCode, "Stated Capital", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, ITPaid, ITPaidCurrencyCode, "Income Tax Paid", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, AccumulatedEarnings, AccumulatedEarningsCurrencyCode, "Accumulated Earnings", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, ITAccrued, ITAccruedCurrencyCode, "Income Tax Accrued", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, AssetsEarnings, AssetsEarningsCurrencyCode, "Assets", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, NumberOfEmployees, new Literal(), "No. of Employees", "");
            writer.RenderEndTag(); // End Table
        }
    }

    [Designer(typeof(CBCControlDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CBCRevenues runat=\"server\"> </{0}:CBCRevenues>")]
    public class CBCRevenues : CompositeControl
    {
         MoneyField _Unrelated;
         CountryList _UnrelatedCurrency;
         MoneyField _Related;
         CountryList _RelatedCurrency;
         MoneyField _Total;
         CountryList _TotalCurrency;

        public MoneyField Unrelated { get { return _Unrelated; } }
        public CountryList UnrelatedCurrency { get { return _UnrelatedCurrency; } }
        public MoneyField Related { get { return _Related; } }
        public CountryList RelatedCurrency { get { return _RelatedCurrency; } }
        public MoneyField Total { get { return _Total; } }
        public CountryList TotalCurrency { get { return _TotalCurrency; } }
        public Unit DropdownListWith
        {
            get
            {
                return new Unit(100, UnitType.Percentage);
            }
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            _Unrelated = new MoneyField() { ID= "txtUnrelated" };
            _UnrelatedCurrency = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith, ID= "ddlUnrelatedCurrency" };

            _Related = new MoneyField() { ID = "txtRelated" };
            _RelatedCurrency = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith, ID= "ddlRelatedCurrency" };

            _Total = new MoneyField { ID = "txtTotal" };
            _TotalCurrency = new CountryList { CodeType = CodeType.CurrencyCode, Width = DropdownListWith, ID= "TotalCurrency" };

            Controls.Add(_Unrelated);
            Controls.Add(_UnrelatedCurrency);
            Controls.Add(_Related);
            Controls.Add(_RelatedCurrency);
            Controls.Add(_Total);
            Controls.Add(_TotalCurrency);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // start table
            CBCConrolsHelper.CreateRow4Columns(writer, Unrelated, UnrelatedCurrency, "Unrelated", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, Related, RelatedCurrency, "Related", "Currency Code");
            CBCConrolsHelper.CreateRow4Columns(writer, Total, TotalCurrency, "Total", "Currency Code");
            writer.RenderEndTag(); // End Table
        }
    }


    [Designer(typeof(CBCControlDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CBCConstituentEntityDetails runat=\"server\"> </{0}:CBCConstituentEntityDetails>")]
    public class CBCConstituentEntityDetails : CompositeControl
    {
        TextField _txtRegisterdName;
        TextField _txtCompanyRegNumber;
        CountryList _txtCompanyRegNumberIssuedByCountry;
        NumberField _txtTexRefNo;
        CountryList _ddlTexRefNoIssuedByCountry;

        public TextField   txtRegisteredName { get { return _txtRegisterdName; } }
        public TextField txtCompanyRegNumber { get { return _txtCompanyRegNumber; } }
        public CountryList ddlCompanyRegNumberIssuedByCountry { get { return _txtCompanyRegNumberIssuedByCountry; } }
        public NumberField txtTaxRefNo { get { return _txtTexRefNo; } }
        public CountryList ddlTaxRefNoIssuedByCountry { get { return _ddlTexRefNoIssuedByCountry; } }

        public Unit DropdownListWith
        {
            get
            {
                return new Unit(100, UnitType.Percentage);
            }
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            _txtRegisterdName = new TextField { MaxLength = 100 };
            _txtCompanyRegNumber = new TextField { MaxLength = 15 };
            _txtCompanyRegNumberIssuedByCountry = new CountryList { Width = DropdownListWith };
            _txtTexRefNo = new NumberField { NumberTypes = NumberFieldTypes.CIT, MaxLength = 10 };
            _ddlTexRefNoIssuedByCountry = new CountryList { Width = DropdownListWith };

            Controls.Add(_txtRegisterdName);
            Controls.Add(_txtCompanyRegNumber);
            Controls.Add(_txtCompanyRegNumberIssuedByCountry);
            Controls.Add(_txtTexRefNo);
            Controls.Add(_ddlTexRefNoIssuedByCountry);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // start table

            CBCConrolsHelper.CreateRow4Columns(writer, txtRegisteredName, new Literal(), "Registered Name", "");
            CBCConrolsHelper.CreateRow4Columns(writer, txtCompanyRegNumber, ddlCompanyRegNumberIssuedByCountry, "Company Reg No.", "Issued by Country");
            CBCConrolsHelper.CreateRow4Columns(writer, txtTaxRefNo, ddlTaxRefNoIssuedByCountry, "Tax Ref No.", "Issued by Country");

            writer.RenderEndTag(); // End Table
        }
    }


    [Designer(typeof(CBCAddressDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CBCAddress runat=\"server\"> </{0}:CBCAddress>")]
    public class CBCAddress : CompositeControl
    {
        DropDownField _txtAddressType;
        DropDownField _ddlAddressOption;
        TextField _txtOtherSpecify;
        TextField _txtAddressNumber;
        TextField _txtPostOffice;
        TextField _txtPostOfficePostCode;
        CountryList _ddlPostOfficeCountryCode;
        TextField _txtUnitNumber;
        TextField _txtComplexName;
        TextField _txtStreetNo;
        TextField _txtStreetOrFarmName;
        TextField _txtSuburbOrDistrict;
        TextField _txtCityOrTownName;
        TextField _txtResPostalCode;
        CountryList _ddlResAddressCountryCode;
        public Unit DropdownListWith
        {
            get
            {
                return new Unit(100, UnitType.Percentage);
            }
        }
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            _txtAddressType = new DropDownField();
            _txtAddressType.Items.AddRange(new[]
            {
                new ListItem("SELECT", ""),
                new ListItem("Residential Or Business", "OECD301"),
                new ListItem("Residential", "OECD302"),
                new ListItem("Business", "OECD303"),
                new ListItem("Registered Office", "OECD304")
            });
            _ddlAddressOption = new DropDownField() { AutoPostBack = true};
            _ddlAddressOption.Items.AddRange(new[]
          {
                new ListItem("SELECT", ""),
                new ListItem("PO Box", "01"),
                new ListItem("Private Bag", "02"),
                new ListItem("Other PO Special Service (specify)", "03")
            });
            _ddlAddressOption.SelectedIndexChanged += TxtAddressOption_SelectedIndexChanged;
            _txtOtherSpecify = new TextField() { MaxLength = 10, Enabled=false };
            _txtAddressNumber = new TextField { MaxLength = 8 };
            _txtPostOffice = new TextField { MaxLength = 22 };
            _txtPostOfficePostCode = new TextField { MaxLength = 10 };
            _ddlPostOfficeCountryCode = new CountryList { Width = DropdownListWith };
            _txtUnitNumber = new TextField() { MaxLength = 5 };
            _txtComplexName = new TextField() { MaxLength = 26 };
            _txtStreetNo = new TextField() { MaxLength = 8 };
            _txtStreetOrFarmName = new TextField { MaxLength = 24 };
            _txtSuburbOrDistrict = new TextField { MaxLength = 33 };
            _txtCityOrTownName = new TextField { MaxLength = 33 };
            _txtResPostalCode = new TextField { MaxLength = 10 };
            _ddlResAddressCountryCode = new CountryList { Width = DropdownListWith };

            Controls.Add(_txtAddressType);
            Controls.Add(_ddlAddressOption);
            Controls.Add(_txtOtherSpecify);
            Controls.Add(_txtAddressNumber);
            Controls.Add(_txtPostOffice);
            Controls.Add(_txtPostOfficePostCode);
            Controls.Add(_ddlPostOfficeCountryCode);
            Controls.Add(_txtUnitNumber);
            Controls.Add(_txtComplexName);
            Controls.Add(_txtStreetNo);
            Controls.Add(_txtStreetOrFarmName);
            Controls.Add(_txtSuburbOrDistrict);
            Controls.Add(_txtCityOrTownName);
            Controls.Add(_txtResPostalCode);
            Controls.Add(_ddlResAddressCountryCode);
        }

        private void TxtAddressOption_SelectedIndexChanged(object sender, EventArgs e)
        {
           if( _ddlAddressOption == null ) { EnsureChildControls(); }
           if( _ddlAddressOption.SelectedValue.Equals("03") ) { _txtOtherSpecify.Enabled = true; } else { _txtOtherSpecify.Enabled = false; _txtOtherSpecify.Clear(); }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // start table
            CBCConrolsHelper.CreateRow4Columns(writer, _txtAddressType, new Literal(), "Address type", "");
            CBCConrolsHelper.CreateRow4Columns(writer, _ddlAddressOption, _txtOtherSpecify, "PO Box / Private Bag", "Other PO Special Service (specify)");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtAddressNumber, _txtPostOffice, "Number", "Post Office");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtPostOfficePostCode, _ddlPostOfficeCountryCode, "Postal Code", "Country Code");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtUnitNumber, _txtComplexName, "Unit No.", "Complex (if applicable)");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtStreetNo, _txtStreetOrFarmName, "Street No.", "Street / Farm Name");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtSuburbOrDistrict, _txtCityOrTownName, "Suburb / District", "City / Town");
            CBCConrolsHelper.CreateRow4Columns(writer, _txtResPostalCode, _ddlResAddressCountryCode, "Postal Code", "Country Code");
            writer.RenderEndTag(); // End Table
         
        }

        public DropDownField AddressType { get { return _txtAddressType; ; } }
        public DropDownField AddressOption { get { return _ddlAddressOption; } }
        public TextField OtherSpecify { get { return _txtOtherSpecify; } }
        public TextField AddressNumber { get { return _txtAddressNumber; } }
        public TextField PostOffice { get { return _txtPostOffice; } }
        public TextField PostOfficePostCode { get { return _txtPostOfficePostCode; } }
        public CountryList PostOfficeCountryCode { get { return _ddlPostOfficeCountryCode; } }
        public TextField UnitNumber { get { return _txtUnitNumber; } }
        public TextField ComplexName { get { return _txtComplexName; } }
        public TextField StreetNo { get { return _txtStreetNo; } }
        public TextField StreetOrFarmName { get { return _txtStreetOrFarmName; } }
        public TextField SuburbOrDistrict { get { return _txtSuburbOrDistrict; } }
        public TextField CityOrTownName { get { return _txtCityOrTownName; } }
        public TextField ResPostalCode { get { return _txtResPostalCode; } }
        public CountryList ResAddressCountryCode { get { return _ddlResAddressCountryCode; } }
    }

    [Designer(typeof(CBCControlDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:CBCBusinessActivity runat=\"server\"> </{0}:CBCBusinessActivity>")]
    public class CBCBusinessActivity : CompositeControl
    {
        public DropDownField ddlMainBusinessActivity;
        public TextField txtOtherEntityInformation;
    
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();

            ddlMainBusinessActivity = new DropDownField { Width=new Unit(300, UnitType.Pixel)};
            txtOtherEntityInformation = new TextField { MaxLength = 4000, TextMode = TextBoxMode.MultiLine };

            ddlMainBusinessActivity.Bind(db.GetAllBusinessActivities(), "Description", "Code");
            Controls.Add(ddlMainBusinessActivity);
            Controls.Add(txtOtherEntityInformation);
           
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // start table
            CBCConrolsHelper.CreateRow2Columns(writer, ddlMainBusinessActivity, "Main Business Activities");
            CBCConrolsHelper.CreateRow2Columns(writer, txtOtherEntityInformation,  "Other Entity Information");

            writer.RenderEndTag(); // End Table
        }
    }
}