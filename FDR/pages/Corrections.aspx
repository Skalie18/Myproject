<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Corrections.aspx.cs" Inherits="pages_comments" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <link href="../Styles/survey.css" rel="stylesheet" />
    <link href="../Styles/toolBars.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" />
    <link rel="icon" href="../Images/favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="../Images/favicon.ico" type="image/x-icon" />
    <style type="text/css">
        .dvhdr1 {
            background: #CCCCCC;
            font-size: 9px;
            font-weight: bold;
            border-top: 1px solid #000000;
            border-left: 1px solid #000000;
            border-right: 1px solid #000000;
            border-bottom: 1px solid #000000;
            padding: 2px;
            width: 400px;
        }

        .dvbdy1 {
            background: #FFFFFF;
            font-size: 10px;
            border-left: 1px solid #000000;
            border-right: 1px solid #000000;
            border-bottom: 1px solid #000000;
            padding: 2px;
            width: 400px;
        }

        body {
            margin-top: 0;
            margin-bottom: 0;
        }
    </style>

    <script src="../Scripts/jquery-1.6.2.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#btnBack').click(function () {
                window.history.back();
                location.reload();
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary" id="myBody">
            <div class="panel-heading">
                CORRECTED CBC REPORTS
            </div>
            <div class="panel-body">
                <div class="page-container">
                    <fieldset>
                        <asp:GridView runat="server" ID="gvCBC"  AllowPaging="true"
                            OnPageIndexChanging="gvCBC_PageIndexChanging" ShowHeader="true"
                            AutoGenerateColumns="False" CssClass="documents" DataKeyNames="Country,ReportingPeriod" EmptyDataText="NO DATA TO GENERATE PACKAGES">
                            <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Country" HeaderText="Destination Country" />
                                <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                                <asp:BoundField DataField="Year" HeaderText="Tax Year" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Date" HeaderText="Date Actioned" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("StatusId") %>' ID="lblStatusId" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select All" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeader" runat="server" AutoPostBack="true" OnCheckedChanged="chkHeader_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCorrectSingle" OnClick="btnCorrectSingle_Click" runat="server" Text="Correct" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerStyle CssClass="document-pager"></PagerStyle>
                        </asp:GridView>
                        <table style="width: 80%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnCorrect" runat="server" OnClick="btnCorrect_Click" Text="Correct" Visible="False" /></td>
                                <td>
                                    <asp:Button ID="btnBack" runat="server" Text="Back" /></td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
