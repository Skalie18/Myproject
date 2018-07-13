<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OutgoingPackageStatus.aspx.cs" Inherits="pages_OutgoingFileRecordErrors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            OUTGOING PACKAGES
        </div>
        <div class="panel-body">
            <div class="page-container">

                
                <fieldset>
                     <h4>Errors</h4>
                    <asp:GridView runat="server" CssClass="documents" ID="gvOutGoingCBCErrors" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" EmptyDataText="NO FILE/RECORD ERROR" GridLines="Horizontal" DataKeyNames="Id">
                        <Columns>
                        
                            <asp:BoundField DataField="ErrorCode" HeaderText="Error Code" />
                            <asp:BoundField DataField="Details" HeaderText="Details" />
                            <asp:BoundField DataField="OrigionalMessageRefId" HeaderText="Reference Id" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Date Recieved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:TemplateField>
                                
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" Text="Details" ID="lnkViewErrors" OnClick="lnkViewErrors_OnClick"
                                        CommandArgument='<%# Eval("OrigionalMessageRefId")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                    
                     <h4>History</h4>

                            <asp:GridView runat="server" ID="gvPackageHistory" AllowPaging="true" 
                                AutoGenerateColumns="False" CssClass="documents" EmptyDataText="NO PACKAGE HISTORY " OnPageIndexChanging="gvPackageHistory_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Timestamp" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date Actioned" />
                                    <asp:BoundField DataField="AuditAction" HeaderText="Action" />
                                    <asp:BoundField DataField="ActionedBySID" HeaderText="Actioned By" NullDisplayText="FDR" />
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
