<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style>
        .tooltip {
            display: none;
            position: absolute;
            border: 1px solid #333;
            background-color: #428bca;
            border-radius: 5px;
            padding: 10px;
            color: #fff;
            font-size: 12px Arial;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            Maintain User
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 30%">
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSearchUser" ToolTip="Enter SID Or FirstName Or LastName"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                            </tr>
                        </table>
                        <table style="width: 100%">

                            <tr>
                                <td colspan="2">
                                    <asp:GridView runat="server" ID="gvUsers" AutoGenerateColumns="False"
                                        AllowPaging="true" OnPageIndexChanging="gvUsers_PageIndexChanging"
                                        OnRowDataBound="gvUsers_RowDataBound" OnRowCommand="gvUsers_RowCommand"
                                        CssClass="documents" DataKeyNames="UserName,UserId" EmptyDataText="NO USERS FOUND">
                                        <Columns>
                                            <asp:BoundField DataField="UserId" HeaderText="UserId" Visible="false" />
                                            <asp:BoundField DataField="UserName" HeaderText="SID" />
                                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
                                            <asp:TemplateField HeaderText="Role">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoleId" runat="server" Text='<%# Eval("RoleId")%>' Visible="false" />
                                                    <asp:DropDownList ID="ddlRole" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update"
                                                        CommandArgument='<%# Eval("UserId")%>'></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="document-pager"></PagerStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
             $('#<%=txtSearchUser.ClientID%>').hover(function(){
                // Hover over code
                var title = $(this).attr('title');
                $(this).data('tipText', title).removeAttr('title');
                $('<p class="tooltip"></p>')
                .text(title)
                .appendTo('body')
                .fadeIn('slow');
            }, function() {
                // Hover out code
                $(this).attr('title', $(this).data('tipText'));
                $('.tooltip').remove();
            }).mousemove(function(e) {
                var mousex = e.pageX + 20; //Get X coordinates
                var mousey = e.pageY + 10; //Get Y coordinates
                $('.tooltip')
                .css({ top: mousey, left: mousex })
            });
        });
        $(document).on("click", "[id*=btnUpdate]", function () {
            if (!confirm('Are you sure you want to change user role?')) {
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });
    </script>
</asp:Content>

