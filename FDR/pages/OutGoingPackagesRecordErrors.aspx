<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OutGoingPackagesRecordErrors.aspx.cs" Inherits="pages_OutGoingPackagesRecordErrors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            OUTGOING PACKAGE RECORD ERRORS
        </div>
        <div class="panel-body">
            <div class="page-container">

                <fieldset>
                    <asp:GridView runat="server" CssClass="documents" ID="gvOutGoingCBCErrors" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" EmptyDataText="NO RECORD ERROR AVAILABLE" GridLines="Horizontal" DataKeyNames="Id">
                        <Columns>
                        
                            <asp:BoundField DataField="ErrorCode" HeaderText="Error Code" />
                            <asp:BoundField DataField="Details" HeaderText="Details" />
                            <asp:BoundField DataField="DocRefID" HeaderText="Reference Id" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Date Recieved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />

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
