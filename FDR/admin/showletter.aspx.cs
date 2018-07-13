using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class admin_showletter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var xml = DBReadManager.GetLetterXml(Request["Id"]);
            txtXml.SetValue(FdrCommon.FormatXml(xml) );
        }
    }
}