<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="modifymnedetails.aspx.cs" Inherits="pages_modifymnedetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style>
        label {
            font-weight: bold;
            padding: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            MANAGE MNE DETAILS
        </div>
        <div class="panel-body">

            <table style="width: 100%;">
                <tr>
                    <td>
                        <label for='<%=lblRegName.ClientID %>'>Registered Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="lblRegName" Enabled="False"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                        <label for='<%=lblTradingName.ClientID %>'>Trading Name</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="lblTradingName" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <label for='<%=lblRegistrationNumber.ClientID %>'>Registration Number</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="lblRegistrationNumber" Enabled="False"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                        <label for='<%=txtNameOfAltimateHoldingCo.ClientID %>'>Name of the ultimate holding company</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="txtNameOfAltimateHoldingCo" Enabled="False"></asp:TextBox></td>
                </tr>




                <tr>
                    <td>
                        <label for='<%=rbtnUltimateHoldingCoResOutSAInd.ClientID %>'>Ultimate holding company resident outside SA </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rbtnUltimateHoldingCoResOutSAInd" Enabled="False">
                            <asp:ListItem Value="N">No</asp:ListItem>
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>



                <tr>
                    <td>
                        <label for='<%=rbtnMasterLocalInd.ClientID %>'>Master/Local File Required Indicator </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rbtnMasterLocalInd" ForeColor="#3333FF">
                            <asp:ListItem Value="N">No</asp:ListItem>
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>



                 <tr>
                    <td>
                        <label for='<%=rbtnCBCInd.ClientID %>'>CBC Report RequiredIndicator </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rbtnCBCInd" ForeColor="#3333FF">
                            <asp:ListItem Value="N">No</asp:ListItem>
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>

                <tr>
                    <td>
                        <asp:Button  runat="server" ID="btnSubmitChanges" Text="Submit Changes" OnClick="btnSubmitChanges_Click" />
                        <asp:Button  runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
                             />
                     </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>

