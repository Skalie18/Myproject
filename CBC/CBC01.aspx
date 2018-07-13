<%@ Page Title="::CBC01 FORM::" MasterPageFile="~/ProdMaster.master" MaintainScrollPositionOnPostback="true" Language="C#" AutoEventWireup="true" CodeFile="CBC01.aspx.cs" Inherits="CBC01" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <script src="../Scripts/jquery-3.1.1.js"></script>
    <script src="../Scripts/jquery-ui-1.12.1.js"></script>--%>   <%# Eval("ERROR-NO") %>
    <div class="form-container">
        <div style="display: flex; width: 99%;">
            <div class="form-header-text">
                Country by Country Reporting (CbC)
            </div>
            <div class="form-title">CbC01</div>
        </div>
        <div runat="server" id="DivRequestErrors" visible="False">
            <div class="form-heading">ERRORS</div>
            <div class="form-region">
                <asp:Repeater runat="server" ID="rptErrors">
                    <HeaderTemplate>
                        <table class="dir-error">
                            <tr>
                                <th scope="col" style="width: 40px; text-align: left;">Error No
                                </th>
                                <th scope="col" style="width: 120px; text-align: left;">Error Description
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 80px; text-align: left;"><%# Eval("ERROR-NO") %></td>
                            <td style="width: 120px; text-align: left; color: red;"><%# Eval("ERROR-TEXT") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>


            </div>
        </div>
        <div class="form-heading">Reporting Entity</div>
        <div class="form-region">
            <table>
                <tr>
                    <td>Reporting Period<b>:</b> </td>
                    <td>

                        <sars:SarsCalendar ID="txtReportingPeriod" runat="server" MaximumDate="-1D" MaxLength="8" BoundField="RPT-PERIOD" Width="" IsMandatory="True" MinimumDate="" />

                    </td>
                    <td>Registered Name:</td>
                    <td>

                        <sars:TextField ID="txtREgisteredName" runat="server" MaxLength="100" BoundField="RE-REG-NAME"></sars:TextField>

                    </td>
                </tr>
                <tr>
                    <td>Company Reg. No.</td>
                    <td>

                        <sars:TextField ID="txtCompanyRegNo" runat="server" MaxLength="15" BoundField="RE-COMP-REG-NO" IsMandatory="True"></sars:TextField>

                    </td>
                    <td>Issued by:</td>
                    <td>

                        <sars:CountryList ID="ddlCompanyRegIssuedByCountry" runat="server" Width="98%" AddJQuery="False" BoundField="RE-COMP-REGNO-ISSUEDBY" />

                    </td>
                </tr>
                <tr>
                    <td>Tax Ref No.</td>
                    <td>

                        <sars:NumberField ID="txtTexRefNo" runat="server" MaxLength="10" BoundField="RE-TAXREFNO" NumberTypes="CIT"></sars:NumberField>

                    </td>
                    <td>Issued by Country:</td>
                    <td>

                        <sars:CountryList ID="ddlTexRefNoIssuedByCountry" runat="server" Width="98%" AddJQuery="False" BoundField="RE-TAXREFNO-ISSUEDBY" />

                    </td>
                </tr>
                <tr>
                    <td>GIIN No. Available?</td>
                    <td>

                        <sars:RadioButtonListField ID="rbtnGIINNoAvailable" runat="server" RepeatLayout="Flow" Width="100%" RepeatDirection="Horizontal" BoundField="RE-GIIN-NO-IND" AutoPostBack="True" OnSelectedIndexChanged="rbtnGIINNoAvailable_SelectedIndexChanged">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </sars:RadioButtonListField>

                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td>GIIN No.</td>
                    <td>
                        <sars:TextField ID="txtGIINNo" runat="server" MaxLength="19" BoundField="RE-GIIN-NO" Enabled="False" />
                    </td>
                    <td>Issued by Country:</td>
                    <td>
                        <sars:CountryList ID="ddlGIINNoIssuedByCountry" runat="server" Width="98%" AddJQuery="False" BoundField="RE-GIIN-NO-ISSUEDBY" />
                    </td>
                </tr>
                <tr>
                    <td>Reporting Role:</td>
                    <td>
                        <sars:DropDownField ID="ddlRole" runat="server" RepeatLayout="Flow" BoundField="RE-ROLE" AutoPostBack="True">
                        </sars:DropDownField>
                    </td>
                    <td>Resident Country code (e.g. South Africa = ZA)</td>
                    <td>
                        <sars:CountryList ID="ddlResCountry" runat="server" Width="98%" AddJQuery="False" BoundField="RE-RES-COUNTRY-CODE" />
                    </td>
                </tr>
                <tr>
                    <td>Unique No.</td>
                    <td>

                        <sars:TextField ID="txtReportingEntityUniqueNo" runat="server" MaxLength="16" BoundField="RE-UNIQUE-NO" />
                    </td>
                    <td>Record Status:</td>
                    <td>

                        <sars:RadioButtonListField ID="rbtnRecodStatus" runat="server" RepeatLayout="Flow" Width="100%" RepeatDirection="Horizontal" BoundField="RE-REC-STATUS">
                            <asp:ListItem Value="0">Correction</asp:ListItem>
                            <asp:ListItem Value="1">Deletion</asp:ListItem>
                        </sars:RadioButtonListField>

                    </td>
                </tr>



            </table>

            <div>
                <div class="form-sub-heading">
                    Contact Person Details 
                </div>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>First Names</td>
                                    <td>
                                        <sars:TextField ID="txtContactPersonFirstName" runat="server" Width="99%" BoundField="RE-FIRST-NAME" MaxLength="53"></sars:TextField>
                                    </td>
                                    <td>Surname</td>
                                    <td>
                                        <sars:TextField ID="txtContactPersonSurname" runat="server" Width="99%" BoundField="RE-SURNAME" MaxLength="53"></sars:TextField>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Bus Tel No. 1</td>
                                    <td>
                                        <sars:TelephoneField ID="txtContactPersonBusinessTel1" runat="server" Width="99%" BoundField="RE-BUS-TEL1" />
                                    </td>
                                    <td>Bus Tel No. 2</td>
                                    <td>
                                        <sars:TelephoneField ID="txtContactPersonBusinessTel2" runat="server" Width="99%" BoundField="RE-BUS-TEL2" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>Cell No.</td>
                                    <td>
                                        <sars:TelephoneField ID="txtContactPersonCellNo" runat="server" Width="99%" BoundField="RE-CELL" />
                                    </td>
                                    <td>Email Address</td>
                                    <td>
                                        <sars:TextField ID="txtContactPersonEmailAddress" runat="server" TextValidationType="EmailAddress" Width="99%" BoundField="RE-EMAIL" MaxLength="80"></sars:TextField>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </div>

            <div>
                <div class="form-sub-heading">
                    Address
                </div>
                <table>
                    <tr>
                        <td>
                            <%# Eval("ERROR-TEXT") %>
                            <CBC:CBCAddress ID="ctrEntityAddress" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="form-heading">CBC Reports</div>
        <div class="form-region">
            <table>
                <tr>
                    <td><b>Record Status: </b></td>
                    <td>
                        <sars:RadioButtonListField ID="rbtnResidentialIndicator1" runat="server" BoundField="CBC-REC-STATUS" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%">
                            <asp:ListItem Value="0">Correction</asp:ListItem>
                            <asp:ListItem Value="1">Deletion</asp:ListItem>
                        </sars:RadioButtonListField>
                    </td>
                    <td><b>Unique No.:</b></td>
                    <td>
                        <sars:TextField ID="txtCBCReportUniqueNo" runat="server" MaxLength="16" BoundField="CBC-UNIQUE-NO" IsMandatory="True"></sars:TextField>
                    </td>
                </tr>

                <tr>
                    <td><b>Number of Tax Jurisdictions to report:</b></td>
                    <td>

                        <sars:NumberField ID="txtNumberOfTaxJurisdiction" runat="server" BoundField="CBC-NUM-JURISD" Enabled="False" MaxLength="3" Width="100px"></sars:NumberField>

                    </td>
                    <td>&nbsp;</td>
                    <td>

                        <sars:CountryList ID="ddlCBCReportCurrencyCode" runat="server" Width="98%" AddJQuery="False" BoundField="RE-TAXREFNO-ISSUEDBY" />

                    </td>
                </tr>


                <tr>
                    <td><b>Resident Country code
                        <br />
                        (e.g South Africa = ZA):</b></td>
                    <td>

                        <sars:CountryList ID="ddlCBCReportCountryCode" runat="server" Width="98%" AddJQuery="False" BoundField="RE-COMP-REGNO-ISSUEDBY" />

                    </td>
                    <td><b>Currency Code:</b></td>
                    <td>&nbsp;</td>
                </tr>


                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnAddMore" runat="server" OnClick="btnAddMore_Click" Text="ADD TAX JURISDICTION" />
                    </td>
                    <td></td>
                    <td>
                        <%# Eval("ERROR-TEXT") %>
                    </td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>


                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>


                <tr>
                    <td colspan="4">
                        <asp:GridView runat="server" ID="gvSummary" CssClass="documents" Width="100%"
                            DataKeyNames="Id,CBCReportId" AutoGenerateColumns="False"
                            OnRowDataBound="gvSummary_RowDataBound" GridLines="None" ClientIDMode="Static"
                            OnPageIndexChanging="gvSummary_PageIndexChanging" AllowPaging="True"
                            EmptyDataText="YOU HAVE NO CASES FOR THIS SYSTEM" OnRowCommand="gvSummary_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CBCReportId" HeaderText="CBCReport Id" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProfitLossBeforeIncomeTax" HeaderText="Profit/Loss Before IT">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Profit/Loss Before IT CC">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlProfitLossBeforeIncomeTaxCurrencyCode" runat="server"  Enabled="False" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                               <%-- <asp:BoundField DataField="ProfitLossBeforeIncomeTaxCurrencyCode" HeaderText="Profit/Loss BeforeIncome Tax Currency Code">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="StatedCapital" HeaderText="Stated Capital">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Stated Capital CC">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlStatedCapitalCurrencyCode" runat="server"  Enabled="true" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="StatedCapitalCurrencyCode" HeaderText="Stated Capital Currency Code">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="IncomeTaxPaid" HeaderText="ITax Paid">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="IT Paid Currency Code">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlIncomeTaxPaidCurrencyCode" runat="server"  Enabled="False" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="IncomeTaxPaidCurrencyCode" HeaderText="Income Tax Paid Currency Code">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="AccumulatedEarnings" HeaderText="Accumulated Earnings">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="AE Currency Code">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlAccumulatedEarningsCurrencyCode" runat="server"  Enabled="False" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="AccumulatedEarningsCurrencyCode" HeaderText="Accumulated Earnings Currency Code">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="IncomeTaxAccrued" HeaderText="Income Tax Accrued">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="IT Accrued CC">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlIncomeTaxAccruedCurrencyCode" runat="server"  Enabled="False" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="IncomeTaxAccruedCurrencyCode" HeaderText="IncomeTaxAccruedCurrencyCode">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Assets" HeaderText="Assets">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Assets CC">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlAssetsCurrencyCode" runat="server"  Enabled="False" Width="200px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="AssetsCurrencyCode" HeaderText="Assets Currency Code">
                                    <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="NoOfEmployees" HeaderText="No Of Employees">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Visible="true" Text="Edit" Width="50px" CommandArgument='<%# Eval("Id") %>' CommandName="btnEdit" Height="30px"></asp:Button>

                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EmptyDataRowStyle ForeColor="Red" />
                            <PagerStyle HorizontalAlign="Left" CssClass="document-pager" />
                        </asp:GridView>
                    </td>
                </tr>

            </table>



            <%# Eval("ERROR-TEXT") %><%# Eval("ERROR-TEXT") %>
        </div>

        <div class="form-heading">Additional Information</div>
        <div class="form-region">
            <table>
                <tr>
                    <td><b>Unique No.</b> </td>
                    <td>
                        <sars:TextField ID="txtUniqueNo" runat="server" BoundField="ADDINFO-UNIQUE-NO" MaxLength="16"></sars:TextField>
                    </td>
                    <td><b>Record Status:</b></td>
                    <td>

                        <sars:RadioButtonListField ID="rbtnResidentialIndicator2" runat="server" RepeatLayout="Flow" Width="100%" RepeatDirection="Horizontal" BoundField="ADDINFO-REC-STATUS">
                            <asp:ListItem Value="0">Correction</asp:ListItem>
                            <asp:ListItem Value="1">Deletion</asp:ListItem>
                        </sars:RadioButtonListField>

                    </td>
                </tr>
            </table>

            <div>
                <div class="form-sub-heading">
                    Other Information
                </div>

                <table>
                    <tr>
                        <td>Please include any further brief information or explanation you consider necessary or that would facilitate the understanding of the compulsory information provided in the Country-by-Country Report</td>
                        <td colspan="3">

                            <sars:TextField TextMode="MultiLine" runat="server" ID="txtComment" Width="99%" Rows="5"></sars:TextField>
                        </td>
                    </tr>
                    <tr>
                        <td>Resident Country Code (e.g. South Africa = ZA)</td>
                        <td>

                            <sars:CountryList ID="ddlOtherInfoCountryCode" runat="server" Width="98%" AddJQuery="False" BoundField="RE-COMP-REGNO-ISSUEDBY" />

                        </td>
                        <td>Summary Ref. Code:</td>
                        <td>
                            <sars:DropDownField ID="ddlSummaryCode1" runat="server" RepeatLayout="Flow" BoundField="RE-ROLE" AutoPostBack="True">
                            </sars:DropDownField>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>

                            <sars:CountryList ID="ddlOtherInfoCountryCode0" runat="server" Width="98%" AddJQuery="False" BoundField="RE-COMP-REGNO-ISSUEDBY" />

                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <sars:DropDownField ID="ddlSummaryCode2" runat="server" RepeatLayout="Flow" BoundField="RE-ROLE" AutoPostBack="True">
                            </sars:DropDownField>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>


        <div class="declaration-panel">
            <table>
                <tr>
                    <td><b>Declaration</b></td>
                </tr>
                <tr>
                    <td>I declare that:<br />
                        The information furnished in this form is true and correct in every respect; and
                        <br />
                        I have disclosed in full the amounts during the period covered by this declaration.
                        <br />
                        I have the necessary records to support all the declarations on this form.
                        <br />
                        <br />
                        <sars:CheckBoxField runat="server" ID="chkIDeclare" BoundField="DECLARATION-IND" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <label style="text-wrap: none;">
                            Date&nbsp;<%# Eval("ERROR-TEXT") %><b>
                                    
                            </b>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;"></td>
                </tr>
            </table>
        </div>

        <table>
            <tr>
                <td></td>
                <td style="white-space: nowrap">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="200px" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit To SARS" Width="200px" OnClick="btnSubmit_Click" Visible="False" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

    </div>


    <script>
        $(function () {
            $("#cbc-report").dialog(
            {
                autoOpen: false,
                modal: true,
                height: "auto",
                width: "1000",
                buttons: {
                    "Submit": function () {
                        $("#cbc-report").dialog("close");

                    },
                    "Cancel": function () {
                        $("#cbc-report").dialog("close");
                        alert("Canceled.");
                    }
                }
            });
            $("#cbc-report").parent().appendTo(jQuery("form:first")).css({ "z-index": "101" });

            $("#cbc-report-opener").click(function () {
                $("#cbc-report").dialog("open");
            });
        });
    </script>
</asp:Content>
