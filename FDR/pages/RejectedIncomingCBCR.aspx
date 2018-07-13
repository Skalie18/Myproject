<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RejectedIncomingCBCR.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   
    <style>
        #tblComments td {
            text-align: center;
            vertical-align: middle;
        }
        .ui-dialog{
            width:40%!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    <script>
        $(document).on('click', '[id*=btnBack]', function () {
            location.reload(true);
        });

        $(document).on("click", "[id*=lnkView]", function () {
            var rowContents = $(this).closest('tr').find('td');
           /* var cellText = $(rowContents[5]).find('messageSpec_ID');
            var id = $(cellText).val();*/
            var id = $(rowContents[5]).find("input[name$=messageSpec_ID]").val();

            var params = { packageId: id };
            $.ajax({
                type: "POST",
                url: './RejectedIncomingCBCR.aspx/GetCommentsById',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(params),
                success: successful,
                error: failed
            });

            function successful(results, status, req) {
                if (results.d === '') {
                    alert('no comments found');
                    return;
                }
                setData(results.d);
            }

            function failed(msg) {
                alert(msg.responseText);

            }

            function createTable(dialog) {

                var table = $('<table id="comments"></table>');
                var th = $('<thead></thead>');
                var tr = $('<tr></tr>');
                var td = $('<th></th>');
                $(td).appendTo($(tr));
                $(td).appendTo($(tr));
                $(tr).appendTo($($(th)))
                $(th).appendTo($(table));
                $(table).appendTo(dialog[0]);
                return table;

            }
            function setData(data) {
                data = JSON.parse(data);
                var table = createTable($('#dialog'));
                $(table).attr('width', '100%');
                $(table).attr('id', 'tblComments');

                $(function () {
                    $.each(data, function (index, value) {
                        var row = $("<tr></tr>").appendTo($(table));
                        $("<td></td>").text(value.Notes).appendTo($(row));
                    });
                });
                $("#dialog").dialog({
                    title: "Comments",
                    buttons: {
                        Ok: function () {
                            $(this).dialog('close');
                            houseKeeping(table);
                        }
                    },
                    modal: true
                });
                return false;
            }

            function houseKeeping(table) {

                $(table).remove("tr:gt(0)");
                $(table).remove();
                $('#dialog').empty();
                $('#dviId').children().each(function(){
                    //$(this).remove();
                });
                $("#Form1")[0].reset();
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
        })
    </script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            REJECTED PACKAGES
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="pnlUpdate" runat="server">
                    <ContentTemplate>

                        <fieldset>
                            <sars:CountryList ID="tsxts" runat="server"  Visible="false"/>
                            <table style="width:80%">
                                <tr>
                                    <td><span>Country:</span></td>
                                    <td style="width: 45%">
                                        <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server"  AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged"></asp:DropDownList>
                                    <td><span>Reporting Period:</span></td>
                                    <td style="width: 45%">
                                         <asp:DropDownList ID="ddlReportingPeriod" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlReportingPeriod_SelectedIndexChanged">
                                                </asp:DropDownList>

                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound"
                                AllowPaging="true" OnPageIndexChanging="gvCBC_PageIndexChanging"
                                AutoGenerateColumns="False" CssClass="documents" DataKeyNames="TransmittingCountry,TaxYear" EmptyDataText="NO REJECTED PACKAGES">
                                <Columns>
                                    <%--<asp:BoundField DataField="MessageSpec_ID" HeaderText="Id" Visible="false" />--%>
                                    <asp:BoundField DataField="TransmittingCountry" HeaderText="Transmitting Country" />
                                    <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Year" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="TimeStamp" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:BoundField DataField="MessageSpec_ID" HeaderText="Id" Visible="false" />
                                     <asp:TemplateField HeaderText="IDM" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("MessageSpec_ID") %>' ID="lblId" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton Text="View Comments" ID="lnkView" runat="server" />
                                            <input type="hidden" runat="server" id="messageSpec_ID" value='<%#Eval("MessageSpec_ID") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus"  runat="server" Text='<%# Eval("StatusId")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnVerifyPackage" OnClick="btnVerifyPackage_Click" runat="server" Text="View" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle CssClass="document-pager"></PagerStyle>
                            </asp:GridView>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                        <div id="dviId">
                            <div id="dialog" style="display: none; width:50%!important">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


