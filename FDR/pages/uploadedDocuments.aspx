<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SiteNoScriptManager.master" CodeFile="uploadedDocuments.aspx.cs" Inherits="pages_uploadedDocuments" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script src="../Scripts/boxover.js"></script>
        <div class="panel panel-primary">
        <div class="panel-heading" >
            FILE DETAILS</div>
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
                <asp:GridView runat="server" ID="gvUploadedFiles" CssClass="documents" AutoGenerateColumns="False" >
                    <Columns>
                         <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                      
                        <asp:BoundField DataField="TaxRefNo" HeaderText="Tax No" />
                        <asp:BoundField DataField="CaseNo" HeaderText="Case No" />
                          <asp:BoundField DataField="FileName" HeaderText="File Name"  />
                           <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                        <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" />
                        <asp:BoundField DataField="Timestamp" HeaderText="Date Uploaded" DataFormatString="{0:yyyy-MM-dd HH:mm}"/>
                         
                                     
                    </Columns>
                </asp:GridView>
           
            </fieldset>         
            <fieldset>
                               <embed  title="FDR FILE" alt="FDR FILE" src='<%= string.Format("documents.ashx?oId={0}", Request["oId"]) %>' />

            </fieldset>
        </div>
    </div>
</asp:Content>


