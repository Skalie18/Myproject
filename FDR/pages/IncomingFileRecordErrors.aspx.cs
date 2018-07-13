using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class pages_IncomingFileRecordErrors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

          
            if (Request.QueryString["id"] != null)
            {
               
                var id =(Request.QueryString["id"].ToString());
                //var countryCode = Request.QueryString["country"].ToString();
                var fileErrors=   DBReadManager.Read_X_CBC_FileValidations(id);
                var recordErrors = DBReadManager.Read_X_CBC_RecordValidations(id);
                //var errors = DatabaseReader.getIncomingFileRecordErrors(countryCode, id);
                Session["FileError"] = fileErrors;
                Session["RecordError"] = recordErrors;
                gvIncomingCBCErrors.Bind(fileErrors);
                gvIncomingCBCRecordErrors.Bind(recordErrors);
            }
        }
    }
    protected void gvIncomingCBCErrors_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIncomingCBCErrors.NextPage(Session["FileError"], e.NewPageIndex);
    }

    protected void gvIncomingCBCRecordErrors_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIncomingCBCRecordErrors.NextPage(Session["FileError"], e.NewPageIndex);
    }
}