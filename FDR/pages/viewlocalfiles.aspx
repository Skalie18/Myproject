<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="viewlocalfiles.aspx.cs" Inherits="pages_viewlocalfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="panel panel-primary">
        <div class="panel-heading" >
            MNE - WITH LOCAL FILES</div>
        <div class="panel-body" >
              <fieldset>
                <asp:GridView runat="server" AutoGenerateColumns="False" ID="gvMNE"  CssClass="documents" AllowPaging="True" OnPageIndexChanging="gvMNE_PageIndexChanging" GridLines="Horizontal" PageSize="20" OnRowCommand="gvMNE_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="Row #" />
                        <asp:BoundField DataField="TaxpayerReferenceNumber" HeaderText="Tax Ref No" />
                        <asp:BoundField DataField="RegistrationNumber" HeaderText="Reg No" />
                        <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewFiles" runat="server" CommandName="ViewFiles" CommandArgument='<%# Bind("Id") %>' Text="View Files"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </fieldset>
            
            
        </div>
    </div>
</asp:Content>

