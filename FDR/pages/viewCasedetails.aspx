<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="viewCasedetails.aspx.cs" Inherits="pages_viewCasedetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
         CASE DETAILS
        </div>
        <div class="panel-body">
            <div class="page-container">
                
                        <fieldset>
                    <table>
                        <tr>
                            <td><label for="<%= txtTexRefNo.ClientID%>">Tax Reference No.*</label></td>
                            <td>
                                <sars:NumberField runat="server" ID="txtTexRefNo" NumberTypes="CIT" Width="200px"  />
                            </td>
                        </tr>
                     
                        
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"   />
                            </td>
                        </tr>
                    </table>
                </fieldset>

                   <fieldset>
                                <asp:GridView runat="server" CssClass="documents" ID="gvCasedetails" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" OnPageIndexChanging="gvCasedetails_PageIndexChanging"  EmptyDataText="NO CASE DETAILS AVAILABLE" GridLines="Horizontal" DataKeyNames="TaxRefNo,CaseNo">
                                    <Columns>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CaseNo" HeaderText="Case No." />
                                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                                        <asp:BoundField DataField="EntityName" HeaderText="Entity Name" />
                                        <asp:BoundField DataField="Year" HeaderText="Year" />
                                        <asp:BoundField DataField="CountryName" HeaderText="Country" />
                                         <asp:BoundField DataField="DateCreated" HeaderText="Date Captured" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                        <asp:TemplateField HeaderText="View Files">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkViewDocuments" runat="server" Text="View Files" OnClick="lnkViewDocuments_Click" CommandArgument='<%# Eval("TaxRefNo") +"|" + Eval("CaseNo") %>' ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="document-pager"></PagerStyle>
                                </asp:GridView>
                            </fieldset>

                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" Text="Back" ID="btnBack" OnClick="btnBack_Click"  /></td>
                                <td>
                                  
                                <td>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>


          
            </div>
        </div>
    </div>
</asp:Content>

