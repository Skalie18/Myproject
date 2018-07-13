using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;
public partial class mnedetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    private string GetCheckBoxValue(CheckBox chkName)
    {
        if (chkName.Checked)
        {
            return "Y";
        }
        else
        {
            return "N";
        }
    }

    private void SetCheckBoxValue(CheckBox chkName, string value)
    {
        if (value == "Y")
        {
            chkName.Checked = true;
        }
        else
        {
            chkName.Checked = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateControls())
        {
            var mne = new MultiNationalEntity()
            {
                Id = 0,
                PartyId = Convert.ToDecimal(txtPartyId.Text),
                TaxpayerReferenceNumber = txtTaxRefNo.Text,
                YearofAssessment = Convert.ToInt16(txtYear.Text),
                RegisteredName = txtRegisteredName.Text,
                TradingName = txtTradingName.Text,
                RegistrationNumber = txtRegistrationNo.Text,
                FinancialYearEnd = Convert.ToDateTime(txtFinancialYearEnd.Text),
                TurnoverAmount = Convert.ToDecimal(txtTurnoverAmount.Text),
                NameUltimateHoldingCo = txtUltimateHoldingCo.Text,
                /*--------------------------*/
                Datestamp = DateTime.Now,
                UltimateHoldingCompanyResOutSAInd = GetCheckBoxValue(chkUltimateCompanyResideOutSA),
                TaxResidencyCountryCodeUltimateHoldingCompany = ddlCountries.SelectedValue,
                UltimateHoldingCOIncomeTaxRefNo = txtUltimateHoldingTaxRefNo.Text,
                MasterLocalFileRequiredInd = GetCheckBoxValue(chkMasterLocalFileRequired),
                CbCReportRequiredInd = GetCheckBoxValue(chkCBCRequired),
                CreatedBy = Sars.Systems.Security.ADUser.CurrentSID
            };


            if (!mne.MNEExists())
            {
                if (mne.Save() > 0)
                {
                    MessageBox.Show("Multinational Entity saved successfully");
                }
            }
            else
            {
                MessageBox.Show(string.Format("Multinational Entity with tax reference: {0} number already exists", txtTaxRefNo.Text));

            }
        }
    }

    private bool ValidateControls()
    {
        if (txtTaxRefNo.Text == "")
        {
            MessageBox.Show("Please enter Tax Reference Number");
            return false;
        }
        if (txtYear.Text == "")
        {
            MessageBox.Show("Please enter Assement Year");
            return false;
        }
        if (txtRegisteredName.Text == "")
        {
            MessageBox.Show("Please enter Registered Name");
            return false;
        }
        /* if (txtTradingName.Text == "")
         {
             MessageBox.Show("Please enter Trading Name");
             return false;
         }

         if (txtRegistrationNo.Text == "")
         {
             MessageBox.Show("Please enter Registered Number");
             return false;
         }*/
        if (txtFinancialYearEnd.Text == "")
        {
            MessageBox.Show("Please enter Financial Year End");
            return false;
        }
        if (txtTurnoverAmount.Text == "")
        {
            MessageBox.Show("Please enter Turnover Amount");
            return false;
        }
        if (txtUltimateHoldingCo.Text == "")
        {
            MessageBox.Show("Please enter Ultimate Holding Company Name");
            return false;
        }
        if (ddlCountries.SelectedIndex < 1)
        {
            MessageBox.Show("Please select Tax Residency CountryCode of UltimateHolding Company");
            return false;
        }

        if (txtUltimateHoldingTaxRefNo.Text == "")
        {
            MessageBox.Show("Please enter Ultimate Holding Company Tax Reference Number");
            return false;
        }

        return true;
    }
}