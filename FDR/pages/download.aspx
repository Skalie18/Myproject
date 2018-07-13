<%@ Page Language="C#" AutoEventWireup="true" CodeFile="download.aspx.cs" Inherits="pages_download" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .documents {
            border-collapse: collapse;
            border-spacing: 0;
            padding: 5px;
            font-size: .8em;
            border: #428bca;
            width: 98%;
            border-radius: 4px;
        }

            .documents tr th:first-child {
                border: solid 0px #428bca;
                border-radius: 8px 0 0 0;
            }

            .documents tr th {
                background: #428bca;
                text-align: left;
                padding: 8px;
                color: #000 !important;
                margin: 5px;
                font-size: 12px !important;
                background: -webkit-linear-gradient(to bottom, #428bca, #ffffff);
                background: -o-linear-gradient(to bottom, #428bca, #ffffff);
                background: -moz-linear-gradient(to bottom, #428bca, #ffffff);
                background: linear-gradient(to bottom, #428bca, #ffffff);
            }


        input[type=checkbox] {
            padding: 4px;
            border-style: dashed !important;
        }

        .documents tr td {
            text-align: left;
            border-top: dotted 1px #428bca;
            border-left: 0 !important;
            border-right: 0 !important;
            border-bottom: dotted 1px #428bca;
            color: #000000;
            padding: 10px;
            font-size: 12px !important;
        }

        td {
            vertical-align: top;
            margin-left: 40px;
        }

        input[type=button], input[type=submit], input[type=reset] {
            background-color: #428bca;
            border: none;
            color: white;
            padding: 10px;
            text-decoration: none;
            margin: 4px 2px;
            cursor: pointer;
            border-radius: 4px;
            border-bottom: 4px double #fff !important;
            border-right: 4px double #fff !important;
            font-weight: bold;
            font-size: 100%;
            letter-spacing: 3px !important;
            font-family: Tahoma;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <asp:GridView ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound" runat="server" AllowPaging="false"
                AutoGenerateColumns="False" CssClass="documents"  EmptyDataText="NO CBC DECLARATIONS">
                <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="ReceivingCountry" HeaderText="Receiving Country" />
                     <asp:BoundField DataField="TransmittingCountry" HeaderText="Transmitting Country" />
                    <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                    <asp:BoundField DataField="ResidentCountry" HeaderText="Resident Country" />
                     <asp:BoundField DataField="ConstituentEntity" HeaderText="Constituent Entity" />
                    <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Reference No" />                 
                    <asp:BoundField DataField="Unrelated" HeaderText="Unrelated" />
                    <asp:BoundField DataField="Related" HeaderText="Related" />
                    <asp:BoundField DataField="Total" HeaderText="Total" />
                </Columns>
            </asp:GridView>
    </form>
</body>
</html>
