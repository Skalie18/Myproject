using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_OutgoingFileRecordErrors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["uid"] != null)
            {

                var uid = Request.QueryString["uid"];
                //var countryCode = Request.QueryString["country"].ToString();
               // GetOutgoingFileRecordErrors
                var errors = DatabaseReader.GetOutgoingFileErrors(uid);
                var auditTrail = DatabaseReader.OutgoingPackageAuditTrailByUid(uid);

                Session["recordError"] = errors;
                gvOutGoingCBCErrors.Bind(errors);
                gvPackageHistory.Bind(auditTrail);

            }
        }
    }

    protected void gvIncomingCBCErrors_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutGoingCBCErrors.NextPage(Session["recordError"], e.NewPageIndex);
    }

    protected void lnkViewErrors_OnClick(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if (btn != null)
        {
            var row = btn.NamingContainer as GridViewRow;
            if (row != null)
            {
                gvOutGoingCBCErrors.SelectedIndex = row.RowIndex;
                if (gvOutGoingCBCErrors.SelectedDataKey != null)
                {

                    var messageUid = btn.CommandArgument;

                    Response.Redirect("OutGoingPackagesRecordErrors.aspx?uid=" + messageUid);
                }
            }
        }
    }



    protected void gvPackageHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPackageHistory.NextPage(DatabaseReader.OutgoingPackageAuditTrailByUid(Request["uid"]), e.NewPageIndex);
    }
}