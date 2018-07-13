<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="IncomingFileStatuses.aspx.cs" Inherits="pages_IncomingFileStatuses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
   <%--     $(document).ready(function () {
            $('#<%=btnSearch.ClientID%>').click(function () {
                var startDate = $('#<%=txtStartDate.ClientID%>');
                var endDate = $('#<%=txtEndDate.ClientID%>');

                if ($(startDate).val() === '' && $(endDate).val() !== '') {
                    alert('Please enter start date');
                    return false;
                }

                if ($(endDate).val() === '' && $().val(startDate) !== '') {
                    alert('Please enter end date');
                    return false;
                }
            });


        });--%>

    </script>
    <style>
        .ui-widget-content a {
            display: block !important;
        }
    </style>
    <div class="panel panel-primary">
        <div class="panel-heading">
            INCOMING PACKAGES STATUSES
        </div>
        <div class="panel-body">
            <div class="page-container">
                <fieldset>
                    <table style="width: 100%">
                        <tr>
                            <td><span>Country:</span></td>
                            <td>
                                <asp:DropDownList ID="ddlcountry" CssClass="form-control" runat="server"
                                    Width="300px" AutoPostBack="true">
                                </asp:DropDownList>

                            </td>
                            <td></td>
                            <td></td>
                        </tr>



                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                
                                <%--Start Date:--%>
                            </td>
                            <td>
                                 <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                             <%--   <sars:SarsCalendar ID="txtStartDate" Width="300px" runat="server" MaximumDate="0" />--%>
                            </td>
                            <td><%--End Date:--%></td>
                            <td>
                             <%--   <sars:SarsCalendar ID="txtEndDate" Width="300px" runat="server" MaximumDate="0" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                               <%-- <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />--%>

                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset>


                    <asp:GridView runat="server" OnPageIndexChanging="gvIncomingCBCStatus_PageIndexChanging" CssClass="documents" ID="gvIncomingCBCStatus" AllowPaging="true" PageSize="15" AutoGenerateColumns="False" EmptyDataText="NO FILE STATUS AVAILABLE" GridLines="Horizontal" DataKeyNames="MessageSpec_ID">
                        <Columns>
                            <asp:TemplateField HeaderText="Transmitting Country">
                                <ItemTemplate>
                                    <%#Eval("TransmittingCountry") %> - <%#Eval("Country") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="FinalStatus" HeaderText="Status" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Date Recieved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:TemplateField>
                                <ItemTemplate>

                                    <asp:LinkButton runat="server" Text="View Reasons" ID="lnkViewErrors" OnClick="lnkViewErrors_Click"
                                        Visible='<%# IsFileRejected(Eval("FinalStatus").ToString()) %>'
                                        CommandArgument='<%# Eval("MessageSpec_ID")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                </fieldset>

                <div>
                    <table>
                        <tr>
                            <td>

                            <td>

                            <td></td>
                        </tr>
                    </table>
                </div>



            </div>
        </div>
    </div>
</asp:Content>
