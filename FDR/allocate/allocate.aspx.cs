using System;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class allocate_allocate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlRoles.Bind(DBReadManager.GetFdrUsers(), "FullName", "SID");
        }
        if (string.IsNullOrEmpty(Request["submissionId"]))
        {
            btnAllocate.Enabled = false;
            ddlRoles.Enabled = false;
        }
        else
        {
            btnAllocate.Enabled = true;
            ddlRoles.Enabled = true;
        }
    }

    protected void btnAllocate_Click(object sender, EventArgs e)
    {
        if (ddlRoles.SelectedIndex == 0)
        {
            MessageBox.Show("Please select user");
            return;
        }
        var submissionId = Request["submissionId"];
        var sid = ddlRoles.SelectedValue;
        int allocationId = DBWriteManager.AllocateCase(submissionId);
        if (allocationId > 0)
        {
            DBWriteManager.SaveAllocationDetails(allocationId, submissionId, sid);
            MessageBox.Show("Case allocated successfully");
        }
        else
        {
            MessageBox.Show("Could not allocate case");
        }
    }
}