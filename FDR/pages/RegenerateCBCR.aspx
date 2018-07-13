<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RegenerateCBCR.aspx.cs" Inherits="pages_RegenerateCBCR" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        $(document).on("click", "[id*=btnGenerateSingle]", function () {
            var rowContents = $(this).closest('tr').find('td');
            var country = $(rowContents[0]).html().split('-')[0].trim();
            var year = parseInt($(rowContents[1]).html());
            if (!ValidateRequiredFields(year))
                return false;

            if (!confirm('Are you sure you want to generate a package for ' + country + '?')) {
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });

        function ValidateRequiredFields(year) {
            var reportingPeriod = $('#<%=dpReportingPeriod.ClientID%>').val();
            var periodYear = reportingPeriod.substring(0, 4);
            if (reportingPeriod === '') {
                $('#<%=dpReportingPeriod.ClientID%>').first().focus();
                alert('Please enter reporting period');
                return false;
            }
            if (parseInt(periodYear) != year) {
                alert("Reporting Period Year should be the same as Year");
                return false;
            }

            return true;
        }
    </script>

    <div class="panel panel-primary">
        <div class="panel-heading">
            GENERATE PACKAGE
        </div>
        <div class="panel-body">
            <div class="page-container">
                <fieldset>
                    <table style="width: 80%">
                        <tr>
                            <td><span>Country:</span></td>
                            <td style="width: 45%">
                                <asp:DropDownList ID="ddlCountryList" CssClass="form-control" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td><span>Reporting Period:</span></td>
                            <td style="width: 45%">
                                <sars:SarsCalendar ID="dpReportingPeriod" runat="server" MaximumDate="0D" CanBeInFuture="false" />
                                <asp:TextBox ID="txtReportingPeriod" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtReportingPeriod_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="gvCBC_PageIndexChanging"
                        AutoGenerateColumns="False" CssClass="documents" DataKeyNames="Country" EmptyDataText="NO DATA TO GENERATE PACKAGES">
                        <Columns>
                            <asp:BoundField DataField="Country" HeaderText="Country" />
                            <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="Period" HeaderText="Tax Year" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="TimeStamp" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Date" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnGenerate" OnClick="btnGenerate_Click" runat="server" Text="Generate" />
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
                    <table>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>

