﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sars.Systems.Security;

public partial class SiteMaster : MasterPage
{
    protected string UserFullName;
    protected string UserRoles;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.User.Identity.IsAuthenticated)
        {
            Response.Redirect( "~/SessionExpired.aspx" );
            return;
        }
        var thisUser = Session["thisUser"] == null ? ADUser.SearchAdUsersBySid( ApplicationUser.UserName ) : Session["thisUser"] as List<UserInfo>;
        if (thisUser != null && thisUser.Any( ))
        {
            if (Session["thisUser"] == null)
            {
                Session["thisUser"] = thisUser;
            }
                var roles = this.Page.User.GetRole( );

            if (!string.IsNullOrEmpty( roles ))
            {
                this.Page.Title = !string.IsNullOrWhiteSpace( roles ) ? roles.ToUpper( ) : Page.Title;
                UserFullName = !string.IsNullOrEmpty( roles )
                    ? string.Format( "{0}({1}) - {2}", thisUser[0].FullName, thisUser[0].SID, roles )
                    : string.Format( "{0}({1})", thisUser[0].FullName, thisUser[0].SID );
                welcome.InnerText = string.Format( "{0}", UserFullName );
            }
            else
            {
                try
                {
                    var userRole = Page.User.GetRole( );
                    if (string.IsNullOrEmpty( userRole ))
                    {
                        Page.User.AddToRole( "Users" );
                    }
                }
                catch (SecureException)
                {
                    // MessageBox.Show(exception.Message);
                }
            }
        }
    }   
}
