using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mics_TestPopUp : FormBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "key", "alert('Button Clicked')", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        //GetControls(ctrCBCReportSummary);
        MessageBox.Show("Submitted");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Canceled");
    }
}