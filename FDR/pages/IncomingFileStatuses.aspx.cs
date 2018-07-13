using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class pages_IncomingFileStatuses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.GetAllCountries(ddlcountry);
            LoadIncomingFileStatuses(null);
        }

    }

    private void LoadIncomingFileStatuses(string countryCode)
    {

        var incomingFileStatustes = DatabaseReader.Get_X_MessageSpec_ByCountry(countryCode);

           // .getIncomingFileStatuses(countryCode);//, startDate, endDate);
        Session["fileStatuses"] = incomingFileStatustes;
        gvIncomingCBCStatus.Bind(incomingFileStatustes);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var country = ddlcountry.SelectedIndex > 0 ?  ddlcountry.SelectedValue : null;
        //var startDate = !string.IsNullOrEmpty(txtStartDate.Text)  ? txtStartDate.Text : null;
        //var endDate = !string.IsNullOrEmpty(txtEndDate.Text)  ? txtEndDate.Text : null;
        var files = DatabaseReader.Get_X_MessageSpec_ByCountry(country);//, startDate, endDate);

        if (files == null)
            MessageBox.Show("No File Status found for Country " + country);
        gvIncomingCBCStatus.Bind(files);

    }
    protected void lnkViewErrors_Click(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn != null)
        {
            var row = btn.NamingContainer as GridViewRow;
            if (row != null)
            {
                gvIncomingCBCStatus.SelectedIndex = row.RowIndex;
                if (gvIncomingCBCStatus.SelectedDataKey != null)
                {

                    var incomingCbcId = btn.CommandArgument;
                   

                    Response.Redirect("IncomingFileRecordErrors.aspx?id=" + incomingCbcId);
                }
            }
        }
    }

    protected bool IsFileRejected(string status)
    {
        return status.Equals("Rejected", StringComparison.InvariantCultureIgnoreCase);
    }
    
    protected void gvIncomingCBCStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIncomingCBCStatus.NextPage(Session["fileStatuses"], e.NewPageIndex);
    }
}