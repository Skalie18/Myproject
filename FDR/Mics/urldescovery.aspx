<%@ Page Title="APP PAGES" MasterPageFile="~/Site.master" Language="C#" AutoEventWireup="true"
    CodeFile="urldescovery.aspx.cs" Inherits="UrlDescovery" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    
        <div class="panel panel-primary">
        <div class="panel-heading">VIEW ALL - APPLICATION PAGES</div>
        <div class="panel-body">
            <fieldset>
       
        <table width="100%">
            <tr runat="server" id="row_message" visible="False">
                <td>
                    <fieldset>
                        <asp:Label ID="lblError" runat="server"  Font-Italic="True" ForeColor="Red"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset>
                    <asp:GridView ID="gvUrls" runat="server" AutoGenerateColumns="False" CssClass="documents" GridLines="None" OnRowDataBound="RowDataBound" >
                        <Columns>
                            
                              <asp:TemplateField>
                                <ItemTemplate>
                                    <img src='<%=Page.ClientScript.GetWebResourceUrl( typeof(Sars.Systems.Controls.SideNav), "Sars.Systems.page.png" ) %>' style="height: 25px; width: 25px" />
                                </ItemTemplate>
                                  <ItemStyle Width="25px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FriendlyName" HeaderText="Friendly Name">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PageUrl" HeaderText="Page URL">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Mapped">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkMapped" Enabled="False" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Map Function" Visible="True">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFunctions"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="FunctionSelectedIndexChanged">
                                       
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Map Function" Visible="False">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hptMapFunction" runat="server">Map A Function</asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </fieldset>
                </td>
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
