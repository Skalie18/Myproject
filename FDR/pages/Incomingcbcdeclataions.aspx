<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Incomingcbcdeclataions.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="panel panel-primary">
        <div class="panel-heading" >
           NEW CBC DECLARATIONS LIST</div>
        <div class="panel-body">   
            <div class="page-container">
                
                <fieldset>
                    <table>
                        <tr>
                            <td><label for="<%= txtTexRefNo.ClientID%>">Tax Reference No.*</label></td>
                            <td>
                                <sars:NumberField runat="server" ID="txtTexRefNo" NumberTypes="CIT" Width="200px"  />
                            </td>
                        </tr>
                        
                        <tr>
                            <td><label for="<%= txtTexRefNo.ClientID%>">Year:</label></td>
                            <td>
                                <sars:NumberField runat="server" ID="txtYear" NumberTypes="TaxYear" Width="200px" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"/>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            <fieldset>
                <asp:GridView runat="server" ID="gvCBC" AutoGenerateColumns="False" CssClass="documents" DataKeyNames="TaxYear,ID"
                     EmptyDataText="NO DECLARATIONS SUBMISSIONS" AllowPaging="True" OnPageIndexChanging="gvCBC_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID"  Visible="false"/>
                    <asp:BoundField DataField="Timestamp" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date Received" />
                    <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Year" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Contact Name" HeaderText="Contact Name" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Contact Email" />
                    <asp:BoundField DataField="Contact Number" HeaderText="Contact Number" />
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkViewForm" runat="server" Text="View Form" OnClick="lnkViewForm_Click" CommandArgument='<%# Eval("TaxRefNo") + "|" + Eval("ID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="document-pager"></PagerStyle>
            </asp:GridView>
            </fieldset>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
            </div>         
        </div>
    </div>
</asp:Content>

