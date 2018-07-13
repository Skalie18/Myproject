<%@ Page Title="REGENERATE SECURED MENUS" MasterPageFile="~/Site.master" Language="C#" AutoEventWireup="true" CodeFile="refreshmenu.aspx.cs" Inherits="fromdll_refreshmenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="panel panel-primary">
        <div class="panel-heading">
            REGENERATE SECURED MENUS
        </div>
        <div class="panel-body">
            <fieldset>
  <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblError" ForeColor="red"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRefresh" runat="server" Text="REFRESH MENU" style="width:100%!important;" OnClick="btnRefresh_Click" /></td>
                </tr>

            </table>
            </fieldset>
            <fieldset>
                 <table>
                <tr>
                    <td>
                    <asp:HyperLink runat="server" ID="hlMainMenu" NavigateUrl="~/secureurl/menu.aspx" CssClass="main-menu-link">
                        <img src='<%=Page.ClientScript.GetWebResourceUrl( typeof(Sars.Systems.Controls.SideNav), "Sars.Systems.Menu.png" ) %>' style="height: 16px; width: 16px" />
                        Main Menu
                    </asp:HyperLink>
                    </td>
                </tr>
            </table>
            </fieldset>
          
        </div>
    </div>





</asp:Content>

