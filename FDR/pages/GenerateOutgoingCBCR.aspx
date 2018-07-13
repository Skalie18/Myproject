<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GenerateOutgoingCBCR.aspx.cs" Inherits="pages_Incomingcbcdeclataions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
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
    <script>
        var caller = 0;

        $(document).on("click", "[id*=btnGenerateSingle]", function (event) {
            // event.preventDefault();
            caller = 1;
            var rowContents = $(this).closest('tr').find('td');
            var country = $(rowContents[0]).html().split('-')[0].trim();
            var year = parseInt($(rowContents[2]).html());
            var period = $(rowContents[1]).html()
            if (!ValidateRequiredFields(period))
                return false;

            if (!confirm('Are you sure you want to generate a package for ' + country + '?')) {
                event.preventDefault();
                return false;
            }
            else {
                $(this).attr('disabled', 'disabled');
            }
        });


        $(document).on("click", "[id*=btnVerifyPackage]", function (event) {
                var rowContents = $(this).closest('tr').find('td');
                var country = $(rowContents[0]).html().split('-')[0].trim();
                var period = $(rowContents[1]).html();
                if ( period === '&nbsp;'){
                    if (!ValidateRequiredFields())
                        return false;
                }
               
        });

        function ValidateRequiredFields(period) {
            var reportingPeriod = $('#<%=ddlReportingPeriod.ClientID%> option:selected').val();
            var periodYear = reportingPeriod.substring(0, 4);
            if (reportingPeriod === '-99999' && period ==='') {
                alert('Please select reporting period');
                return false;
            }

            return true;
        }

        $(document).on("click", "[id*=btnGenerate]", function (event) {
            var reportingPeriod = $('#<%=ddlReportingPeriod.ClientID%> option:selected').val();
            event.stopPropagation();
            if (caller === 0) {
                if (reportingPeriod === '-99999') {
                    alert('Please enter reporting period');
                    return false;
                }
                if ($('[id*=gvCBC] input:checkbox:checked').length > 0) {
                    if (!confirm('Are you sure you want to generate packages for the selected Destination Countries?')) {
                        return false;
                    }
                    else {
                        $(this).attr('disabled', 'disabled');
                    }
                }
                else {
                    alert('You must at least select one country');
                    return false;
                }
            }

        });


    </script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            GENERATE PACKAGE
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdateProgress ID="upDownloading" AssociatedUpdatePanelID="upDownload" runat="server">
                    <ProgressTemplate>
                        <div id="Div2" class="modal">
                            <div class="center">
                                <asp:Image ID="imgLoading1" runat="server" ImageUrl="~/Images/loading1.gif" alt="progress bar" />Please wait processing....
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upDownload" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="pnlUpdate" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnCount" runat="server" />
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
                                                <asp:DropDownList ID="ddlReportingPeriod" runat="server" CssClass="form-control"
                                                    OnTextChanged="ddlReportingPeriod_TextChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:DropDownList ID="ddlYears" runat="server" AutoPostBack="True" Visible="false"
                                                    OnSelectedIndexChanged="ddlYears_SelectedIndexChanged" CssClass="form-control">
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:GridView runat="server" ID="gvCBC" OnRowDataBound="gvCBC_RowDataBound" AllowPaging="true"
                                        OnPageIndexChanging="gvCBC_PageIndexChanging" ShowHeader="true"
                                        AutoGenerateColumns="False" CssClass="documents" DataKeyNames="Country,Period" EmptyDataText="NO DATA TO GENERATE PACKAGES">
                                        <HeaderStyle BackColor="#df5015" Font-Bold="true" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="Country" HeaderText="Destination Country" />
                                            <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                                            <asp:BoundField DataField="Period" HeaderText="Tax Year" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="Date" HeaderText="Date Actioned" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("StatusId") %>' ID="lblStatusId" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ActionId") %>' ID="lblActionId" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ReturningStatus") %>' ID="lblReturningStatus" runat="server" />
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
                                                    <asp:Button ID="btnGenerateSingle" OnClick="btnGenerateSingle_Click" runat="server" Text="Generate" />
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
                                    <br />
                                    <table style="width: 98%;">
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td align="right">&nbsp;</td>
                                            <td align="right">
                                                <asp:Button ID="btnGenerate" Visible="false" runat="server" Text="Generate" OnClick="btnGenerate_Click" /></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>


