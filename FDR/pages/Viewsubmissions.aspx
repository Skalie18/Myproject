<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Viewsubmissions.aspx.cs" Inherits="pages_Viewsubmissions" %>
<%@ Import Namespace="Sars.Systems.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
             NEW MNE SUBMISSIONS
        </div>
        <div class="panel-body">
            <div class="page-container"> 
                    <table>
                        <tr>
                            <td>Tax reference number:</td>
                            <td>
                                <sars:NumberField runat="server" ID="txtTaxRefNo" NumberTypes="CIT" Width="300px" MaxLength="10"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Assessment year
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtYear" Width="300px" MaxLength="6"/>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td >
                                <asp:Button runat="server" Text="Search" ID="btnSearch" OnClick="btnSearch_Click"/>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView runat="server" AutoGenerateColumns="False" ID="gvMNE" CssClass="documents" AllowPaging="True" OnPageIndexChanging="gvMNE_PageIndexChanging" GridLines="Horizontal" PageSize="10" OnRowCommand="gvMNE_RowCommand" OnRowDataBound="gvMNE_RowDataBound" EmptyDataText="NO FILE SUBMISSIONS">
                        <Columns>
                            <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                            <asp:BoundField DataField="Year" HeaderText="Year" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:TemplateField HeaderText="Registered Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegisteredName" runat="server" Text='<%# Utils.getshortString(Eval("Name").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkViewFiles" runat="server" NavigateUrl='<%#  string.Format("displayfiles.aspx?submissionId={0}&bck={1}", Eval("submissionId"), Request.Url.PathAndQuery.ToBase64String())  %>' Text="View&nbsp;Files" CssClass="link-buttons"  ></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                        <SelectedRowStyle CssClass="selectedRow" />
                    </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

