<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EnquireCBCRStatus.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
<%--        $(document).ready(function () {
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
            ENQUIRE PACKAGE STATUSES
        </div>
        <div class="panel-body">
            <div class="page-container">

                <fieldset>
                    <table style="width: 100%">
                        <tr>
                            <td><span>Country:</span></td>
                            <td>
                                <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server"
                                    Width="300px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                       
                        <tr>
                             <td></td>
                             <td>
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" /></td>
                           
                            <td></td>
                            <td></td>
                           
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset>
                    <asp:GridView runat="server" ID="gvCBC" OnPageIndexChanging="gvCBC_OnPageIndexChanged" AllowPaging="true" PageSize="15"
                        AutoGenerateColumns="False" CssClass="documents" DataKeyNames="UID,Year" EmptyDataText="NO PACKAGES TO ENQUIRE">
                        <Columns>

                            <asp:TemplateField HeaderText="Country">
                                <ItemTemplate>
                                    <%#Eval("Country") %> - <%#Eval("Country Code") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                              <asp:BoundField DataField="Year" HeaderText="Tax Year" />
                            <asp:BoundField DataField="TimeStamp" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date" />
                             <asp:BoundField DataField="ReturningStatus" HeaderText="Response Status" NullDisplayText="Pending Foreign status" />
                            <asp:TemplateField>
                                
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" Text="Details" ID="lnkViewErrors" OnClick="lnkViewErrors_OnClick"
                                        CommandArgument='<%# Eval("UID")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right">&nbsp;</td>
                        </tr>
                    </table>
                </fieldset>
                   <%--Visible='<%# IsFileRejected(Eval("status").ToString()) %>'--%>
            </div>
        </div>
    </div>
</asp:Content>


