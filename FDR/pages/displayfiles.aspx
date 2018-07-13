<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="displayfiles.aspx.cs" Inherits="pages_displayfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            UPLOADED FILES
        </div>
        <div class="panel-body">
            <div class="page-container">
                <div class="scroll-div-">


                    <asp:TabContainer runat="server" ID="tbMain" Width="100%" ActiveTabIndex="0"  CssClass="Tab" >
                        <asp:TabPanel runat="server" ID="tpMaster">
                            <HeaderTemplate>
                                Master Files
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView runat="server" CssClass="documents" ID="gvMasterFiles" AutoGenerateColumns="False" OnRowDataBound="gvMasterFiles_RowDataBound" EmptyDataText="NO MASTER FILES AVAILABLE" GridLines="Horizontal" DataKeyNames="FileId">
                                    <Columns>
                                        
                                          <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                                        <asp:BoundField DataField="Year" HeaderText="Assessment Year" />
                                        <asp:BoundField DataField="Classification" HeaderText="Classified As" />
                                        <asp:TemplateField HeaderText="View Files">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDownloadMasterFiles" runat="server" Text="Open File" CommandArgument='<%# Eval("ObjectID") %>' OnCommand="FileOpenCommand"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Outcome">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlOutcome" runat="server"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                          <asp:TemplateField HeaderText="Outcome reason">
                                            <ItemTemplate>
                                                <asp:TextBox MaxLength="100" ID="txtOutcomeReason" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="document-pager"></PagerStyle>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel runat="server" ID="tpLocal">
                            <HeaderTemplate>
                                Local Files
                            </HeaderTemplate>
                            <ContentTemplate>

                                <asp:GridView runat="server" GridLines="Horizontal" EmptyDataText="NO LOCAL FILES AVAILABLE" CssClass="documents" ID="gvLocalFiles" AutoGenerateColumns="False" OnRowDataBound="gvMasterFiles_RowDataBound" DataKeyNames="FileId">
                                    <Columns>
                                        
                                          <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                                        <asp:BoundField DataField="Year" HeaderText="Assessment Year" />
                                        <asp:BoundField DataField="Classification" HeaderText="Classified As" />
                                        <asp:TemplateField HeaderText="View Files">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDownloadLocalFile" runat="server" Text="Open File" CommandArgument='<%# Eval("ObjectID") %>' OnCommand="FileOpenCommand"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Outcome">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlOutcome" runat="server"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Outcome reason">
                                            <ItemTemplate>
                                                <asp:TextBox MaxLength="100" ID="txtOutcomeReason" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="document-pager"></PagerStyle>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>

                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" Text="Back" ID="btnBack" OnClick="btnBack_Click" /></td>
                                <td>
                                    <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" /></td>
                                <td>
                                    <span onclick="return confirm('Are you sure you want to submit?');">
                                        <asp:Button runat="server" Text="Submit" ID="btnSubmit" OnClick="btnSubmit_Click" /></span>
                                </td>
                            </tr>
                        </table>
                    </div>


                </div>
            </div>
        </div>
    </div>
</asp:Content>

