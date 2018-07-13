<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="u3tm.aspx.cs" Inherits="pages_u3tm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

     <div class="panel panel-primary">
        <div class="panel-heading" >
            ENQUIRE U3TM</div>
        <div class="panel-body">       
            <div class="container">
                <fieldset>
                <table style="width:100%;">
                    <tr>
                        <th>
                            Tax Reference Number
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <sars:NumberField runat="server" ID="txtTaxRefNo" NumberTypes="CIT"/>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Button Text="Search" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                        </td>
                    </tr>

                </table>
                    </fieldset>
                <fieldset runat="server" Visible="True">
                    <table style="width:100%;">
                        <tr>
                            <td>
                                <asp:TextBox runat="server" TextMode="MultiLine" Rows="30" Width="100%" ID="txtXml"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>     
        </div>
    </div>
</asp:Content>

