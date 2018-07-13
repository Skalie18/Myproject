using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDR.DataLayer;

public partial class pages_UserProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var sid = txtUserName.SID;
        if (string.IsNullOrEmpty(sid))
        {
            MessageBox.Show("Please search for user");
            return;
            
        }

        var user = Sars.Systems.Security.ADUser.GetAdUser(sid);
        if (user != null)
        {
            txtEmail.SetValue(user.Mail);
            txtName.SetValue(user.FullName);
            txtTel.SetValue(user.Telephone);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var updated =DBWriteManager.UpdateUserProfile(
            txtUserName.SID,
           txtTel.Text
            );
        MessageBox.Show(updated > 0
            ? "User profile updated successfully."
            : "There was a problem updating user profile.");
    }
}