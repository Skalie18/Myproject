<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IncomingPackage.aspx.cs" Inherits="pages_IncomingPackage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            INCOMING PACKAGES
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="pnlUpdate" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnCount" runat="server" />
                        <fieldset>
                            <table style="width: 80%">
                                <tr>
                                    <td><span>Country:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>Reporting Period<span>:</span>

                                    </td>
                                    <td style="width: 45%">
                                         <asp:DropDownList ID="ddlReportingPeriod" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlReportingPeriod_SelectedIndexChanged">
                                                </asp:DropDownList>
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound" AllowPaging="true"
                                OnPageIndexChanging="gvCBC_PageIndexChanging" ShowHeader="true"
                                AutoGenerateColumns="False" CssClass="documents" DataKeyNames="TransmittingCountry,TaxYear" EmptyDataText="NO INCOMING CBC DECLARATIONS">
                                <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="MessageSpec_ID" HeaderText="Id" Visible="false" />
                                    <asp:BoundField DataField="TransmittingCountry" HeaderText="Transmitting Country" />
                                    <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Year" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="TimeStamp" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnVerifyPackage" OnClick="btnVerifyPackage_Click" runat="server" Text="View" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusId") + "|" + Eval("MessageSpec_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


