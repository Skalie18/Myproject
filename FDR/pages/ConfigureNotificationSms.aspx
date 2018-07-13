<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ConfigureNotificationSms.aspx.cs" Inherits="pages_ConfigureNotificationSms" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style>
        select {
            padding: 0;
            height: 20px;
        }
        #welcoming input[type=button], input[type=submit], input[type=reset] {
            background-color: #428bca;
            border: none;
            color: white;
            padding: 5px !important;
            text-decoration: none;
            margin: 0 !important;
            cursor: pointer;
            border-radius: 2px;
            font-size: 100% !important;
            width: 100px !important;
        }
        #tblconfigemail fieldset {
            width: 922px !important
        }
          #tblconfigemail textarea {
              width: 963px !important;
              padding: 15px !important;
              border-radius: 10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            CONFIGURE NOTIFICATION EMAILS
        </div>
        <div class="panel-body">
            <div class="page-container">
            <table style="width: 99%;" id="tblconfigemail">
                <tr>
                    <td>
                        <asp:TabContainer runat="server" ID="tcMain" Width="100%" ActiveTabIndex="0" CssClass="Tab">
                            <asp:TabPanel runat="server" HeaderText="File Received" ID="pnlFileReceived">
                                <ContentTemplate>
                                    <br/>
                                    <fieldset>File Received - Template Body</fieldset>
                                    <br/>
                                    <asp:TextBox ID="txtFileReceived" runat="server" TextMode="MultiLine" Rows="7" onkeyup="CountChars(this, 300)"/>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" HeaderText="File Accepted" ID="pnlFileAccepted">
                                <ContentTemplate>
                                    <br />
                                   <fieldset> File Accepted - Template Body</fieldset>
                                    <br/>
                                    <asp:TextBox ID="txtFileAccepted" runat="server"  TextMode="MultiLine"  Rows="7" onkeyup="CountChars(this, 3100)"/>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" HeaderText="File Rejected" ID="pnlFileRejected">
                                <ContentTemplate>
                                    <br />
                                    <fieldset>File Rejected - Template Body</fieldset>
                                    <br/>
                                    <asp:TextBox ID="txtFileRejected" runat="server"  TextMode="MultiLine" Rows="7"  onkeyup="CountChars(this, 300)"></asp:TextBox>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" HeaderText="File Accepted With Warnings" ID="pnlFileAcceptedWithWarnings">
                                <ContentTemplate>
                                    <br/>
                                    <fieldset>File Accepted With Warnings -  Template Body</fieldset>
                                    <br />
                                    <asp:TextBox ID="txtFileAcceptedWithWarnings" runat="server"  TextMode="MultiLine" Rows="7"  onkeyup="CountChars(this, 300)"/>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save Email Templates" Style="width: 400px !important;" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
            </div>
        </div>
    </div>
</asp:Content>

