<%@ Page Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Report1.aspx.cs" Inherits="Logging_Report1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="panel panel-primary">
        <div class="panel-heading">
            SWIMS ENCOUNTERED AN ERROR
        </div>
        <div class="panel-body">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnAddContact"/>
                </Triggers>
                <ContentTemplate>
                    <table style="width: 100%; padding: 0px; border-collapse: collapse;">

                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label Text="" runat="server" ID="lblError" ForeColor="Red" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="label-column">From Date:</td>
                                <td class="field-column">
                                    <asp:TextBox runat="server" ID="txtFrom" CssClass="dates"   Width="250px" />
                                   <asp:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFrom" Format="yyyy-MM-dd" >
                                </asp:CalendarExtender>

                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="label-column">To Date:</td>
                                <td class="field-column">
                                    <asp:TextBox runat="server" ID="txtTo" CssClass="dates"   Width="250px" />
                                  
                                    
                                     <asp:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtTo" Format="yyyy-MM-dd" >
                                </asp:CalendarExtender>

                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="label-column">&nbsp;</td>
                                <td class="field-column">
                                    <asp:Button ID="btnAddContact" runat="server" CssClass="buttons" Text="View Report" OnClick="ViewReport"   Width="150px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            

            
        </div>
    </div>

</asp:Content>
