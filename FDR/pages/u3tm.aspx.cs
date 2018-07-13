using System;
using System.Web.UI.WebControls;

public partial class pages_u3tm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !IsPostBack )
        {
            string script = "$(document).ready(function () { $('[id*=btnSearch_Click]').click(); });";
            ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(5000);
        if (txtTaxRefNo.FieldValue == null)
        {
            MessageBox.Show("Tax Reference Number is required.");
            return;
        }
        var queue = new FDRQueueService();
        var doc = queue.EnquireRegistration(txtTaxRefNo.Text);
        if (doc != null)
        {
         txtXml.SetValue(doc.OuterXml);   
        }
    }
}