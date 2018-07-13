<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SentLetters.aspx.cs" Inherits="admin_SentLetters" %>
<%@ Import Namespace="Sars.Systems.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="panel panel-primary">
        <div class="panel-heading" >VIEW ALL LETTERS SENT TO EFILING</div>
        <div class="panel-body">   
            <DIV class="page-container">
                <table>
                        <tr>
                            <td>Tax reference number:</td>
                            <td>
                                <sars:NumberField runat="server" ID="txtTaxRefNo" NumberTypes="CIT" Width="300px" MaxLength="10"/>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td >
                                <asp:Button runat="server" Text="Search" ID="btnSearch" OnClick="btnSearch_Click"/>
                            </td>
                        </tr>
                    </table>
                        
                    <asp:GridView runat="server" AutoGenerateColumns="False" ID="gvMNE" CssClass="documents" AllowPaging="True" OnPageIndexChanging="gvMNE_PageIndexChanging" GridLines="Horizontal" EmptyDataText="NO LETTERS">
                        <Columns>

                            <asp:BoundField DataField="Timestamp" HeaderText="Date Submitted" DataFormatString="{0:yyyy-MM-dd HH:ss}" />

                            <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                            <asp:BoundField DataField="Year" HeaderText="Year" />
                            <asp:BoundField DataField="FullName" HeaderText="User Name" />
                           <%-- <asp:TemplateField HeaderText="Registered Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegisteredName" runat="server" Text='<%# Utils.getshortString(Eval("Name").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                     
                            
                                 <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkViewFiles" runat="server" NavigateUrl='<%#  string.Format("showletter.aspx?Id={0}&bck={1}", Eval("Id"), Request.Url.PathAndQuery.ToBase64String())  %>' Text="View&nbsp;XML" CssClass="link-buttons"  ></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <PagerStyle CssClass="document-pager"></PagerStyle>
                        <SelectedRowStyle CssClass="selectedRow" />
                    </asp:GridView>
            </DIV>         
        </div>
    </div>
    
</asp:Content>

