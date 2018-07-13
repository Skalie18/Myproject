using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Sars.Systems.Data;
using Sars.Systems.Security;


public partial class fromdll_refreshmenu : System.Web.UI.Page
{
    protected override void OnPreInit( EventArgs e ){
        MasterPageFile = SARSDataSettings.Settings.SecondaryMasterPageFile;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnRefresh_Click( object sender, EventArgs e )
    {
        //try
        //{
        //    var roles = Roles.GetActiveRoles();
        //    if (roles != null && roles.Any())
        //    {

        //        foreach (var role in roles)
        //        {
        //            GetXml(role);
        //        }
        //        RefreshUserAccess();
        //        RefreshFunctions();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblError.SetValue(ex.ToString());
        //}

        try
        {
            Roles.RefreshSystemAccess();
            lblError.SetValue("System menu refreshed successfully.");
        }
        catch (SecureException exception)
        {
            lblError.SetValue(exception.ToString());
        }
      
    }
    private void GetXml( string role )
    {
        try
        {
            if (!string.IsNullOrEmpty( role ))
            {
                var menuBuilder = new StringBuilder( );
                var data = Functions.GetFunctionsWithGroupsPerRole( role );
                if (data != null)
                {
                    var paretnColumn = data.Tables[1].Columns["GroupId"];
                    var childColumn = data.Tables[0].Columns["GroupId"];
                    var relation = data.Relations.Add( "group_function", paretnColumn, childColumn );
                    menuBuilder.Append( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    menuBuilder.Append( "<Menu>" );
                    var userRestrictedZones = Page.User.GetUserRestrictedZones( );

                    foreach (DataRow rGroup in data.Tables[1].Rows)
                    {
                        var groupName = rGroup["Description"].ToString( );
                        menuBuilder.AppendFormat( "<Head title=\"{0}\" url=\"#\" roles=\"{1}\">", groupName, role );
                        foreach (var rFunction in rGroup.GetChildRows( relation ))
                        {
                            var title = rFunction["Description"].ToString( ).Replace( "&", "&amp;" );
                            var url = rFunction["functionurl"].ToString( );

                            if (userRestrictedZones != null && userRestrictedZones.Any( ))
                            {
                                if (userRestrictedZones.Contains( url ))
                                {
                                    continue;
                                }
                            }
                            menuBuilder.AppendFormat( "<MenuItem title=\"{0}\" url=\"{1}\" />", title, url );
                        }
                        menuBuilder.Append( "</Head>" );
                    }
                    menuBuilder.Append( "</Menu>" );
                }
                if (!string.IsNullOrEmpty( menuBuilder.ToString( ) ))
                {
                    var dir = string.Format( @"{0}{1}", Page.Request.PhysicalApplicationPath, "SystemMenu" );
                    if (!Directory.Exists( dir ))
                    {
                        Directory.CreateDirectory( dir );
                    }
                    var fullMenuPath = Path.Combine( dir, string.Format( "{0}.menu", role ) );
                    var fullMenu = menuBuilder.ToString( );

                    var file = File.CreateText( fullMenuPath );
                    file.Write( fullMenu );
                    file.Flush( );
                    file.Close( );
                    lblError.SetValue("Menus Generated successfully");
                }
            }
        }
        catch (Exception ex)
        {
            lblError.SetValue( string.Format( "<div id=\"error\"> <ul> <li>Please make sure that your</li>  <li> Roles </li> <li>  Groups </li> <li>  Functions </li><li>are already added to the database! </li> <li> Error :  <br />{0}</li></ul> </div> ", ex.Message ));
        }
    }

    private void RefreshUserAccess( )
    {
        var data = RolesConfiguration.GetRefreshedAccess( );
        if (data != null && data.Any( ))
        {
            var dir = string.Format( @"{0}{1}", Page.Request.PhysicalApplicationPath, "AccessMenu" );
            if (!Directory.Exists( dir ))
            {
                Directory.CreateDirectory( dir );
            }
            var fullPath = Path.Combine( dir, "Menu.exe" );
            Sars.Systems.Serialization.XmlObjectSerializer.Persist<List<CacheUserAccessDetails>>( data, fullPath );
        }
    }
    private void RefreshFunctions( )
    {
        var functions = Functions.GetStronglyTypedFunctions( );
        if (functions != null && functions.Any( ))
        {

            var dir = string.Format( @"{0}{1}", Page.Request.PhysicalApplicationPath, "Functions" );
            if (!Directory.Exists( dir ))
            {
                Directory.CreateDirectory( dir );
            }
            var fullPath = Path.Combine( dir, "func.exe" );
            Sars.Systems.Serialization.XmlObjectSerializer.Persist<List<Functions>>( functions,
                fullPath );
        }
        else
        {
            lblError.Text += "<br /> There are no functions in the system.";
        }
    }
}