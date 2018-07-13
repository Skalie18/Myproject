<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CorrectPackages.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style>
        #tblComments td {
            text-align: center;
            vertical-align: middle;
        }

        .ui-dialog {
            width: 60% !important;
            height: auto !important;
        }

        #gvHistory {
            width: 100%;
            border-collapse: collapse;
            border: 1px solid silver;
            background-color: ghostwhite;
        }

        #gvHistory td, th {
            text-align: left;
        }

        #gvHistory td {
            padding: 10px;
            border: 1px solid silver;
            font-size: x-small;
            font-family: verdana;
        }

        #gvHistory th {
            background-color: silver;
            padding: 15px;
            border: 1px solid silver;
            font-size: x-small;
            font-family: verdana;
        }

        #gvHistory tr:hover {
            background-color: gray;
            color: #fff;
            cursor: pointer;
        }
        #dviId{
            overflow-y:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script>
        $(document).on("click", "[id*=lnkViewHistory]", function () {
            var rowContents = $(this).closest('tr').find('td');
            var desCountry = $(rowContents[0]).html().split('-')[1].trim();
            var period = $(rowContents[1]).html();
            var params = { countryCode: desCountry, reportingPeriod: period };
            $.ajax({
                type: "POST",
                url: './CorrectPackage.aspx/LoadCBCHistory',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(params),
                success: successful,
                error: failed
            });

            function successful(results, status, req) {

                if (results.d === '') {
                    alert('no package history found');
                    return;
                }
                LoadData(results.d);
            }

            function failed(msg) {
                alert(msg.responseText);

            }



            function LoadData(data) {
                if (data !== 'undefined') {
                    var table = createTable($('#dialog'));
                    var mdata = JSON.parse(data);
                    for (var i in mdata) {
                        var row = $("<tr></tr>").appendTo($(table));
                        $("<td></td>").text(mdata[i].Action).appendTo($(row));
                        $("<td></td>").text(mdata[i].Name).appendTo($(row));
                        $("<td></td>").text(ConvertJsonDateString(mdata[i].Timestamp)).appendTo($(row));
                    }

                    $("#dialog").dialog({
                        title: "History",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        },
                        modal: true
                    });
                }
            }

            function createTable(dialog) {

                var table = $('<table id="gvHistory"></table>');
                var th = $('<thead></thead>');
                var tr = $('<tr></tr>');
                var td = $('<th>Action</th>');
                $(td).appendTo($(tr));
                td = $('<th>Actioned By</th>');
                $(td).appendTo($(tr));
                td = $('<th>TimeStamp</th>');
                $(td).appendTo($(tr));
                $(tr).appendTo($($(th)))
                $(th).appendTo($(table));
                $(table).appendTo(dialog[0]);
                return table;

            }

            function ConvertJsonDateString(jsonDate) {
                var shortDate = null;
                if (jsonDate) {
                    var regex = /-?\d+/;
                    var matches = regex.exec(jsonDate);
                    var dt = new Date(parseInt(matches[0]));
                    var month = dt.getMonth() + 1;
                    var monthString = month > 9 ? month : '0' + month;
                    var day = dt.getDate();
                    var dayString = day > 9 ? day : '0' + day;
                    var year = dt.getFullYear();
                    shortDate = dayString + '/' + monthString + '/' + year;
                }

                return shortDate;
            };
        });

        $(document).on("click", "[id*=btnCorrection]", function () {
            var rowContents = $(this).closest('tr').find('td');
            var country = $(rowContents[0]).html().split('-')[0].trim();
            if (!confirm('Are you sure you want to correct package for ' + country + '?')) {
                event.preventDefault();
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });

    </script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            CORRECT PACKAGE
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="pnlUpdate" runat="server">
                    <ContentTemplate>
                        <sars:CountryList ID="tsxts" runat="server"  Visible="false"/>
                        <fieldset>
                            <table style="width:80%">
                                <tr>
                                    <td><span>Country:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server"  AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged"></asp:DropDownList></td>
                                    <td>Reporting Period<span>:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlReportingPeriod" runat="server" CssClass="form-control"
                                                    OnTextChanged="ddlReportingPeriod_TextChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <br />
                             <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound" AllowPaging="true"
                                OnPageIndexChanging="gvCBC_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="documents" DataKeyNames="TaxReferenceNo,Year" EmptyDataText="NO PACKAGES">
                                <Columns>
                                    <asp:BoundField DataField="RegisteredName" HeaderText="Registered Name" />
                                    <asp:BoundField DataField="TradingName" HeaderText="Trading Name" />
                                    <asp:BoundField DataField="TaxReferenceNo" HeaderText="Tax Reference No" />
                                    <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="DestinationCountry" HeaderText="Destination Country" />
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
                                            <asp:Button ID="btnView" runat="server" Text="View Package" OnClick="btnView_Click"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewForm" runat="server" Text="View CBC Report" OnClick="lnkViewHistory_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>
                            <br />
                            <div id="dviId">
                                <div id="dialog" style="display: none; width: 80%!important">
                                </div>
                            </div>
                            <br />
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


