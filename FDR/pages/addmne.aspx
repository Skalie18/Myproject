<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="addmne.aspx.cs" Inherits="pages_addmne" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        #maintbl tr td {
            width:25%!important;
        }
        .auto-style1 {
            height: 34px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="panel panel-primary">
        <div class="panel-heading" >
            ADD NEW MNE</div>
        <div class="panel-body">
             
            <table style="width:99%; margin-left:auto; margin-right:auto;" id ="maintbl">
                <tr>
                    <td class="auto-style1">Party Id:</td>
                    <td style="width: 300px;">
                        <sars:NumberField ID="txtPartyID" runat="server" MaxLength="15" NumberTypes="TaxYear"></sars:NumberField>
                    </td>

                    <td class="auto-style1">Year Of Assessment:</td>
                    <td class="auto-style1">
                        <sars:NumberField ID="txtYear" runat="server" NumberTypes="TaxYear" MaxLength="4"></sars:NumberField>
                    </td>
                </tr>
                <tr>
                    <td>Ref No:</td>
                    <td>
                        <sars:NumberField ID="txtTPRefNo" runat="server" MaxLength="10" NumberTypes="CIT"></sars:NumberField>
                    </td>

                    <td>Trading Name:</td>
                    <td>
                        <sars:TextField ID="txtTradingName" runat="server" MaxLength="120"></sars:TextField>
                    </td>
                </tr>
                <tr>
                    <td>Reg Name:</td>
                    <td>
                        <sars:TextField ID="txtRegName" runat="server" MaxLength="120"></sars:TextField>
                    </td>

                    <td>Financial Year End</td>
                    <td>
                        <sars:SarsCalendar ID="txtFinancialYearEnd" runat="server" MinimumDate="0D" Width="" />
                    </td>
                </tr>
                <tr>
                    <td>Turnover Amount:</td>
                    <td>
                        <sars:MoneyField ID="txtTurnoverAmount" runat="server" RandsMaxLength="18" Width="300px" />
                    </td>

                    <td>Ultimate Holding Company:</td>
                    <td>
                        <sars:TextField ID="txtNameUltimateHoldingCo" runat="server" MaxLength="120"></sars:TextField>
                    </td>
                </tr>
                <tr>
                    <td>Ultimate Holding&nbsp; Company Res Out SA Indicator:</td>
                    <td>
                        <sars:RadioButtonListField ID="rbtnUltimateHoldingCompanyResOutSAInd" runat="server">
                            <asp:ListItem>Yes</asp:ListItem>
                            <asp:ListItem>No</asp:ListItem>
                        </sars:RadioButtonListField>
                    </td>

                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>

                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            
        &nbsp;&nbsp;&nbsp;
            
        </div>
    </div>
</asp:Content>

