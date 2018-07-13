<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReviewVoid.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            REVIEW VOIDED PACKAGES
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="pnlUpdate" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <table style="width: 80%">
                                <tr>
                                    <td><span>Country:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Reporting period<span>:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlReportingPeriod" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlYears_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound"
                                AllowPaging="true" OnPageIndexChanging="gvCBC_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="documents" DataKeyNames="Country,Period" EmptyDataText="NO VOIDED PACKAGES TO RIVIEW">
                                <Columns>

                                    <asp:BoundField DataField="Country" HeaderText="Destination Country" />
                                    <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Period" HeaderText="Tax Year" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Date" HeaderText="Date Voided" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Button ID="btnGenerate" OnClick="btnGenerate_Click" runat="server" Text="Generate" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnVerifyPackage" OnClick="btnVerifyPackage_Click" runat="server" Text="View" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right">&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


