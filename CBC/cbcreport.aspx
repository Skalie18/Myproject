<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="cbcreport.aspx.cs" Inherits="cbcreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div  class="page-container">
    <div id="cbc-report" >
        <div>
            <div class="form-sub-heading">
                Summary 
            </div>
            <table>
                <tr>
                    <td>
                       
                        <CBC:CBCReportSummary runat="server" ID="ctrCBCReportSummary"></CBC:CBCReportSummary>
                    </td>
                </tr>
            </table>

        </div>

        <div>
            <div class="form-sub-sub-heading">
                Revenues 
            </div>

            <table>
                <tr>
                    <td>
                        <CBC:CBCRevenues runat="server" ID="ctrCBCRevenues"></CBC:CBCRevenues>
                    </td>
                </tr>
            </table>
        </div>

        <div>
            <div class="form-sub-heading">
                Constituent Entity 
            </div>

            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>Number of Constituent Entities in this Tax Jurisdiction</td>
                                <td>
                                    <sars:NumberField ID="txtNumConstituentEntitiesInTaxJurisdiction" runat="server" MaxLength="3"></sars:NumberField>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>
                        <CBC:CBCConstituentEntityDetails runat="server" ID="ctrCBCConstituentEntityDetails"></CBC:CBCConstituentEntityDetails>
                    </td>
                </tr>
            </table>
        </div>

        <div>
            <div class="form-sub-sub-heading">
                Address 
            </div>
            <table>
                <tr>
                    <td>
                        <CBC:CBCAddress runat="server" ID="ctrCBCAddress"></CBC:CBCAddress>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div class="form-sub-sub-heading">
                Business Activity 
            </div>
            <table>
                <tr>
                    <td>
                        <CBC:CBCBusinessActivity runat="server" ID="ctrCBCBusinessActivity"></CBC:CBCBusinessActivity>
                    </td>
                </tr>
            </table>
        </div>
        <div style="border-top: 5px solid silver; border-radius: 5px;border-bottom: 5px solid silver; text-align:right;">
            <span onclick="javascript: return confirm('Are you sure you want to cancel this transaction?');">
            <asp:Button Text="CANCEL" runat="server" ID="btnCancel" OnClick="btnCancel_Click" /></span>
            <asp:Button Text="SAVE" runat="server" ID="btnSaveReport" OnClick="btnSaveReport_Click" />
        </div>
    </div>
        </div>
</asp:Content>

