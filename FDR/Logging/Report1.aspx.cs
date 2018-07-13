using System;
using Sars.Systems.Extensions;

public partial class Logging_Report1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {       
    }
    protected void ViewReport(object sender, EventArgs e)
    {        
        var startDate = string.IsNullOrEmpty(txtFrom.Text.Trim()) ? null : txtFrom.Text;
        var endDate = string.IsNullOrEmpty(txtTo.Text.Trim()) ? null : txtTo.Text;
        if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
        {
            var errors = DatabaseReader.GetErrorReport(startDate, endDate, "");
            if (errors.HasRows)
            {
                errors.Tables[0].ToExcel("", null);
            }
        }
        else
        {
            var errors = DatabaseReader.GetErrorReport( );
            if (errors.HasRows)
            {
                errors.Tables[0].ToExcel( "", null );
            }
        }
    }
}