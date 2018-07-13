using System;
using System.Web.UI.WebControls;

public partial class pages_modifymnedetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack ) { GetEntity(); }
    }

    private void GetEntity()
    {
        if ( string.IsNullOrEmpty(Request["Id"]) ) { return; }
        var entity = DatabaseReader.GetMultiNationalEntity(Request["Id"]);

        if ( !( entity == null ) )
        {
            lblRegName.SetValue(entity.RegisteredName);
            lblTradingName.SetValue(entity.TradingName);
            lblRegistrationNumber.SetValue(entity.RegistrationNumber);

            txtNameOfAltimateHoldingCo.SetValue(entity.NameUltimateHoldingCo);
            rbtnUltimateHoldingCoResOutSAInd.SelectItemByValue(entity.UltimateHoldingCompanyResOutSAInd);
            rbtnMasterLocalInd.SelectItemByValue(entity.MasterLocalFileRequiredInd);
            rbtnCBCInd.SelectItemByValue(entity.CbCReportRequiredInd);
        }
    }

    protected void btnSubmitChanges_Click(object sender, EventArgs e)
    {
        var entity = DatabaseReader.GetMultiNationalEntity(Request["Id"]);

        if ( entity != null )
        {
            if ( rbtnCBCInd.Equals(entity.CbCReportRequiredInd) && rbtnMasterLocalInd.Equals(entity.MasterLocalFileRequiredInd) )
            {
                MessageBox.Show("No changes made.");
                return;
            }

            decimal saved = DatabaseWriter.UpdateMne(rbtnCBCInd.SelectedValue, rbtnMasterLocalInd.SelectedValue,Request["Id"] );
            if ( saved > 0 )
            {
                MessageBox.Show("Changes Saved successfully.");
                btnSubmitChanges.Visible = false;
            }
        }
    }
    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageMNE.aspx");
    }
}