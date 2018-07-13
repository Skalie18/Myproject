<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="VerifyOutgoingCBC.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/AddComments.ascx" TagPrefix="asp" TagName="AddComments" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style>
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 50%;
            border: 3px solid #0DA9D0;
            padding: 0;
        }

            .modalPopup .header {
                background-color: #2FBDF1;
                height: 30px;
                color: White;
                width: 98%;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .body {
                min-height: 120px;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
                margin-bottom: 5px;
            }

        .modal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 400px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center img {
                height: 128px;
                width: 128px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:HiddenField ID="hdnDownloaded" runat="server" />
    <script>
        $(document).off("click", "[id*=btnApprove]", function (e) {
            e.stopImmediatePropagation();
            if (!confirm('Are you sure you want to approve package?')) {
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });

        $(document).off("click", "[id*=btnReject]", function (e) {
            if (!confirm('Are you sure you want to reject package?')) {
                e.stopImmediatePropagation();
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });

        $(document).off("click", "[id*=btnAddComments]", function (e) {
            e.stopImmediatePropagation();
            if (!confirm('Are you sure you want to add comments?')) {
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });

    </script>
    <div class="panel panel-primary" id="myBody">
        <div class="panel-heading">
            <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
            PACKAGE
        </div>
        <div class="panel-body">
            <div class="page-container">
               <%-- <asp:UpdateProgress ID="upDownloading" AssociatedUpdatePanelID="upDownload" runat="server">
                    <ProgressTemplate>
                        <div id="Div2" class="modal">
                            <div class="center">
                                <asp:Image ID="imgLoading1" runat="server" ImageUrl="~/Images/loading1.gif" alt="progress bar" />Please wait processing....
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                <asp:UpdatePanel ID="upDownload" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <fieldset>
                            <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound" AllowPaging="true"
                                OnPageIndexChanging="gvCBC_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="documents" DataKeyNames="TaxReferenceNo,Year" EmptyDataText="NO PACKAGES">
                                <Columns>
                                    <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                                    <asp:BoundField DataField="TradingName" HeaderText="Trading Name" />
                                    <asp:BoundField DataField="TaxReferenceNo" HeaderText="Tax Reference No" />
                                    <asp:BoundField DataField="ReportingCountry" HeaderText="Reporting Country" />
                                    <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Year" HeaderText="Tax Year" />
                                    <asp:BoundField DataField="NumberOfCBCR" HeaderText="No of CBC Reports" />
                                    <asp:BoundField DataField="Entity" HeaderText="No of Constituent Entities" />

                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestinationCountry" runat="server" Text='<%# Eval("DestinationCountry") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewForm" runat="server" Text="View CBC Report" OnClick="lnkViewForm_Click" CommandArgument='<%# Eval("TaxReferenceNo") + "|" + Eval("Year") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>

                            <h4>History</h4>

                            <asp:GridView runat="server" ID="gvCBCHistory" AllowPaging="true" OnPageIndexChanging="gvCBCHistory_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="documents" EmptyDataText="NO PACKAGE HISTORY ">
                                <Columns>
                                    <asp:BoundField DataField="Action" HeaderText="Action" />
                                    <asp:BoundField DataField="Timestamp" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date Actioned" />
                                    <asp:BoundField DataField="Name" HeaderText="Actioned By" />
                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>
                            <br />
                            <br />
                            <br />
                            <br />

                            <table style="width: 80%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDownload" OnClick="btnDownload_Click" runat="server" Text="Download XML"  /></td>
                                    <td>
                                        <asp:Button ID="btnDownloadXcel" OnClick="btnDownloadXcel_Click" runat="server" Text="Download Excel"  /></td>
                                    <td>
                                        <asp:Button ID="btnApprove" OnClick="btnApprove_Click" runat="server" Text="Approve"  /></td>
                                    <td>
                                        <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click" Text="Reject"  /></td>
                                    <td>
                                        <asp:HiddenField ID="hdnDone" runat="server" Value="" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" Text="Back"  OnClick="btnBack_Click" /></td>
                                </tr>
                            </table>
                        </fieldset>
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
                            PopupControlID="pnlPopup" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground"
                            CancelControlID="btnHide">
                        </asp:ModalPopupExtender>

                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                            <div class="header">
                                Add Comments
                            </div>
                            <div class="body">
                                <asp:AddComments runat="server" ID="AddComments" />
                                <br />
                                <div style="display: none">
                                    <asp:Button ID="btnHide" runat="server" Text="Hide Modal Popup" />
                                </div>

                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

