<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OutgoinCBCHistory.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>
<%@ MasterType VirtualPath="~/Site.master"%>
<script runat="server">

    protected void btnVerify_Click(object sender, EventArgs e)
    {

    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            PACKAGE HISTORY
        </div>
        <div class="panel-body">
            <div class="page-container">
                <fieldset>
                    <table style="width: 100%">
                                <tr>
                                    <td><span>Country:</span></td>
                                    <td>
                                        <sars:CountryList ID="ddlCountryList" Width="300px" runat="server" CodeType="CountryCode" 
                                            OnOptionChanged="ddlCountryList_OptionChanged" AutoPostBack="true" />
                                    </td>
                                    <td><span>Year:</span></td>
                                    <td>
                                        <asp:DropDownList ID="ddlYears" runat="server" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged"></asp:DropDownList>

                                    </td>
                                </tr>
                            </table>
                    <asp:GridView runat="server" ID="gvCBC" 
                        AutoGenerateColumns="False" CssClass="documents"  EmptyDataText="NO PACKAGES HISTORY">
                        <Columns>
                            <asp:BoundField DataField="Country" HeaderText="Destination Country" />
                            <asp:BoundField DataField="Year" HeaderText="Tax Year" />
                            <asp:BoundField DataField="StatusDescription" HeaderText="StatusDescription" />
                            <asp:BoundField DataField="DateActioned" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date Actioned" />
                            <asp:BoundField DataField="Name" HeaderText="Actioned By" />
                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                    <table style="width:80%">
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>&nbsp;</td>
                            <td><asp:Button ID="btnBack" OnClientClick="javascript:window.history.back()" runat="server" Text="Back" Width="190px" /></td>
                        </tr>
                    </table>
                </fieldset>

            </div>
        </div>
    </div>
</asp:Content>

