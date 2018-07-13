<%@ Page Title="" Language="C#" MasterPageFile="~/SiteNoScriptManager.master" AutoEventWireup="true" CodeFile="outgoingfiledetails.aspx.cs" Inherits="pages_outgoingfiledetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="../Scripts/boxover.js"></script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            FILE DETAILS
        </div>
        <div class="panel-body">
            <style>
                embed {
                    width: 600px;
                    width: 99%;
                    margin-left: auto;
                    margin-right: auto;
                    min-height: 700px;
                    overflow: scroll;
                }
            </style>

            <fieldset>
                <SCS:Toolbar ID="Toolbar1" runat="server" OnButtonClicked="Toolbar1_ButtonClicked"
                    EnableClientApi="False" CssClass="toolbar" Width="99%">
                    <Items>
                        <SCS:ToolbarButton CausesValidation="True" Visible="false" CommandName="SAVE" Text="SAVE OUTCOME" ImageUrl="~/Images/Icons/save.png" />
                        <SCS:ToolbarButton CausesValidation="True" CommandName="BACK" Text="BACK" ImageUrl="~/Images/Icons/Back.png" />
                    </Items>
                    <ButtonCssClasses CssClass="button" CssClassEnabled="button_enabled" CssClassSelected=""
                        CssClassDisabled="button_disabled"></ButtonCssClasses>
                </SCS:Toolbar>
            </fieldset>
            <fieldset>
                <asp:GridView runat="server" ID="gvDetails" CssClass="documents" AutoGenerateColumns="False" OnRowDataBound="gvDetails_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Classification" HeaderText="Classified As" />
                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax No" />
                        <asp:BoundField DataField="Year" HeaderText="Year" />
                        <asp:TemplateField HeaderText="Outcome" Visible="false">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlOutcome" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Outcome reason" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox MaxLength="100" ID="txtOutcomeReason" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--  <table class="documents">
                    <tr>
                        <td style="width: 250px !important;">File Name</td>
                        <td >
                            <label runat="server" id="lblFileName" class="file-details"></label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>File Category</td>
                        <td>
                            <label runat="server" id="lvlCategory" class="file-details"></label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>File Classification</td>
                        <td> <label runat="server" id="lblClassification" class="file-details"></label></td>
                    </tr>
                    
                    <tr>
                        <td>Tax Reference Number</td>
                        <td>
                            <label runat="server" id="lblTaxRefNo" class="file-details"></label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>Year</td>
                        <td>
                            <label runat="server" id="lblYear" class="file-details"></label>
                        </td>
                    </tr>
                </table>--%>
            </fieldset>
            <fieldset>
                <%--                <embed  title="FDR FILE" runat="server" ID="embeddiv" src='<%= string.Format("viewdoc.ashx?oId={0}", Request["oId"]) %>' width="600" height="500" alt="FDR FILE" style="width: 99%; margin-left: auto;margin-right: auto; min-height: 700px;" />--%>
                <embed title="FDR FILE" alt="FDR FILE" src='<%= string.Format("viewdoc.ashx?oId={0}&refNo={1}", Request["oId"], Request["refNo"]) %>' />

            </fieldset>
        </div>
    </div>
</asp:Content>

