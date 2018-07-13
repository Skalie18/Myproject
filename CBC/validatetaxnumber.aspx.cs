using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class validatetaxnumber : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NumberField1.TargetCalendarID = SarsCalendar1.ClientID+ "_sars_calender";
    }

    protected void DropDownField1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}