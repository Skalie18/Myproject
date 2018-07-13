<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageMNE.aspx.cs" Inherits="pages_ManageMNE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="panel panel-primary">
        <div class="panel-heading" >
            MANAGE MNE's</div>
        <div class="panel-body">
            <fieldset>
                <table>
                    <tr>
                        <td>Tax Reference Number:</td>
                        <td><sars:NumberField ID="txtSearch" NumberTypes="Any" runat="server"></sars:NumberField></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <asp:GridView runat="server" AutoGenerateColumns="False" ID="gvMNE"  CssClass="documents" AllowPaging="True" OnPageIndexChanging="gvMNE_PageIndexChanging" GridLines="Horizontal" PageSize="20" OnRowCommand="gvMNE_RowCommand">
                    <Columns>

                        <asp:BoundField DataField="RowNumber" HeaderText="Row #" />

                        <asp:BoundField DataField="TaxpayerReferenceNumber" HeaderText="Tax Ref No" />
                        <asp:BoundField DataField="RegistrationNumber" HeaderText="Reg No" />
                        <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                      <%--  <asp:BoundField DataField="TradingName" HeaderText="Trading Name" />--%>

                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Modify" CommandArgument='<%# Bind("Id") %>' Text="Manage"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" CommandName="Remove"
                                    OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                     CommandArgument='<%# Bind("Id") %>' Text="Remove"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle  CssClass="document-pager"></PagerStyle>
                </asp:GridView>
            </fieldset>
            
        </div>
    </div>
</asp:Content>

