using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using FDR.DataLayer;
public partial class Default2 : System.Web.UI.Page
{
    static int pageSize = 10;
    static int pageIndex = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            bindData();
    }

    private void bindData(DataSet users = null)
    {
        if (users == null)
            users = DBReadManager.GetAllUsers();
        Session["userData"] = users;
        gvUsers.Bind(users);
    }

    [WebMethod]
    public static DataSet searchUser(string searchText) //, GridView grid)
    {
        DataSet dsUsers = new DataSet();
        dsUsers.DataSetName = "Users";
        var results = DBReadManager.SearchUser(searchText, pageIndex, pageSize);
        dsUsers = results.Copy();
        return dsUsers;
    }

    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlRole = (e.Row.FindControl("ddlRole") as DropDownList);
            if (ddlRole != null)
            {
                ddlRole.Bind(DBReadManager.GetSecureRoles(), "Description", "RoleId");
            }
            string roleId = (e.Row.FindControl("lblRoleId") as Label).Text;
            ddlRole.Items.FindByValue(roleId).Selected = true;
            e.Row.Cells[6].Enabled = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var data = searchUser(txtSearchUser.Text);
        Session["userData"] = data;
        bindData(data);
        txtSearchUser.Text = "";
    }

    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlRoles = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlRoles.Parent.Parent;
        if (ddlRoles.SelectedIndex > 0)
            row.Cells[6].Enabled = true;
        int idx = row.RowIndex;
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var userID = e.CommandArgument.ToString();

        string roleID = string.Empty;
        string message = string.Empty;
        if (e.CommandName != "Page")
        {
            GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            int index = Convert.ToInt16(row.RowIndex);
            DropDownList ddlRole = (DropDownList)row.FindControl("ddlRole");
            if (ddlRole != null)
            {
                roleID = ddlRole.SelectedItem.Value;

                if (DBWriteManager.UpdateUserRole(userID, roleID) > 0)
                {
                    var user = row.Cells[1].Text + "-" + row.Cells[2].Text + " " + row.Cells[3].Text;
                    message = string.Format("Updated {0}'s role successfully to {1}", user, ddlRole.SelectedItem.Text);
                    MessageBox.Show(message);
                    row.Cells[6].Enabled = false;
                    bindData();
                }

            }
        }
    }

    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUsers.NextPage(Session["userData"], e.NewPageIndex);
    }
}