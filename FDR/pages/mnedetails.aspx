<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="mnedetails.aspx.cs" Inherits="mnedetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            MULTINATIONAL ENTITY
        </div>
        <div class="panel-body">
            <div class="page-container">
              
                    <table>
                        <tr>
                            <td>Tax Reference Number:</td>
                            <td>
                                <sars:NumberField runat="server" ID="txtTaxRefNo" NumberTypes="CIT"  MaxLength="10"/>
                            </td>
                            <td>
                                Assessment Year:
                            </td>
                            <td>
                                <sars:NumberField runat="server" ID="txtYear"  MaxLength="4"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Party Id:
                            </td>
                            <td>
                                <sars:NumberField runat="server" ID="txtPartyId"  MaxLength="10"/>
                            </td>
                            <td>
                                Registered Name:
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtRegisteredName"  MaxLength="100"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Trading Name:
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtTradingName"  MaxLength="100"/>
                            </td>
                            <td>
                                Registered Number:
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtRegistrationNo"  MaxLength="25"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Financial Year End:
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtFinancialYearEnd"  MaxLength="10"/>
                            </td>
                            <td>
                                Turnover Amount:
                            </td>
                            <td>
                                <sars:MoneyField runat="server" ID="txtTurnoverAmount"  Width="350px" MaxLength="18"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ultimate Holding Company Name:
                            </td>
                            <td>
                                <sars:TextField runat="server" ID="txtUltimateHoldingCo"  MaxLength="100"/>
                            </td>
                            <td>
                                Tax Residency CountryCode UltimateHolding Company:
                            </td>
                            <td>
                                <sars:CountryList runat="server" Width="350px" ID="ddlCountries" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ultimate Holding Company Tax Ref No:
                            </td>
                            <td>
                                <sars:NumberField runat="server" NumberTypes="CIT" ID="txtUltimateHoldingTaxRefNo"  MaxLength="10"/>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkMasterLocalFileRequired" Text="Master / Local File Required?"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="chkUltimateCompanyResideOutSA"  Text="Ultimate Holding Company Resides Out of SA?" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkCBCRequired"  Text="CBC Report is Required?" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkCorrectCBCDeclaration"  Text="Correct CBC Declaration?" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkCorrectMasterLocalFile"  Text="Correct Master / Local File Required?" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                           
                            <td></td>
                            <td >
                                &nbsp;</td>
                             <td>
                                <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click"/>
                            </td>
                            <td >
                                <asp:Button runat="server" Text="Cancel" ID="btnCanel" OnClick="btnCancel_Click"/>
                            </td>
                        </tr>
                    </table>
                        
                
            </div>
        </div>
    </div>
</asp:Content>

