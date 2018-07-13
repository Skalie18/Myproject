<%@ Page Language="C#"  MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="viewCaseFiles.aspx.cs" Inherits="pages_viewCaseFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
        CASE FILES
        </div>
        <div class="panel-body">
            <div class="page-container">
                

                   <fieldset>
                                <asp:GridView runat="server" CssClass="documents" ID="gvCaseFiles" AutoGenerateColumns="False" EmptyDataText="NO CASE FILES AVAILABLE" GridLines="Horizontal" DataKeyNames="Id">
                                    <Columns>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CaseNo" HeaderText="Case No." />
                                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax Ref No" />
                                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                                         <asp:BoundField DataField="Timestamp" HeaderText="Date Saved" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                        <asp:TemplateField HeaderText="View Files">
                                            <ItemTemplate>
                                               <asp:LinkButton ID="btnDownloadMasterFiles" runat="server" Text="Open File" CommandArgument='<%# Eval("ObjectID") %>' OnCommand="FileOpenCommand"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="document-pager"></PagerStyle>
                                </asp:GridView>
                            </fieldset>

             

                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" Text="Back" ID="btnBack"  /></td>
                                <td>
                                  
                                <td>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>


          
            </div>
        </div>
    </div>
</asp:Content>