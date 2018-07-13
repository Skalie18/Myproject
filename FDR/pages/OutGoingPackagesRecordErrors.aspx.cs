using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_OutGoingPackagesRecordErrors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["uid"] != null)
            {

                var uid = Request.QueryString["uid"];
                var errors = DatabaseReader.GetOutgoingReecordErrors(uid);
                Session["recordError"] = errors;
                gvOutGoingCBCErrors.Bind(errors);
              
            }
        }
    }
}