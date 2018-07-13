<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="IncomingFileRecordErrors.aspx.cs" Inherits="pages_IncomingFileRecordErrors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            INCOMING PACKAGES 
           
        </div>
        <div class="panel-body">
            <div class="page-container">

                <fieldset>
                    <h4>File Errors</h4>

                    <asp:GridView runat="server" OnPageIndexChanging="gvIncomingCBCErrors_PageIndexChanging" CssClass="documents" ID="gvIncomingCBCErrors" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" EmptyDataText="NO FILE ERROR AVAILABLE" GridLines="Horizontal" DataKeyNames="MessageSpec_ID">
                        <Columns>


                            <asp:BoundField DataField="ErrorCode" HeaderText="Error Code" />
                            <asp:BoundField DataField="Mesasge" HeaderText="Message" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Date Recieved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />


                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                </fieldset>


                <fieldset>
                    <h4>Record Errors</h4>
                    <asp:GridView runat="server" ID="gvIncomingCBCRecordErrors" OnPageIndexChanging="gvIncomingCBCRecordErrors_OnPageIndexChanging" CssClass="documents" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" EmptyDataText="NO RECORD ERROR AVAILABLE" GridLines="Horizontal" DataKeyNames="MessageSpec_ID">
                        <Columns>

                            <asp:BoundField DataField="ErrorCode" HeaderText="Error Code" />
                            <asp:BoundField DataField="Mesasge" HeaderText="Message" />
                              <asp:BoundField DataField="DocRefID" HeaderText="Doc Ref Id" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Date Recieved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />


                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>
                </fieldset>

                <div>
                    <table>
                        <tr>
                            <td>
                                <%--    <asp:Button runat="server" Text="Back" ID="btnBack" OnClick="btnBack_Click" /></td>--%>
                            <td>

                            <td></td>
                        </tr>
                    </table>
                </div>



            </div>
        </div>
    </div>
</asp:Content>

