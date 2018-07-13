<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MasterFileUpload.aspx.cs" Inherits="MasterFileUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            UPLOAD MASTER FILE
        </div>
        <div class="panel-body">
            <div class="page-container">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <asp:GridView Width="80%" runat="server" ID="gvFiles" AutoGenerateColumns="False" CssClass="documents" OnRowDataBound="gvFiles_RowDataBound">

                                <Columns>
                                    <asp:TemplateField HeaderText="Category">                                       
                                        <ItemTemplate>
                                           <sars:DropDownField ID="ddlCat" runat="server"></sars:DropDownField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upload File"> 
                                        <ItemTemplate>
                                            <asp:FileUpload ID="fuMaster" runat="server" Width="100%"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </fieldset>
                     <fieldset>
                         <asp:Button Text="Save" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                         <asp:Button Text="Submit" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                     </fieldset>
                    </ContentTemplate>
                  <Triggers>
                      <asp:PostBackTrigger ControlID="btnSave" />
                      <asp:PostBackTrigger ControlID="btnSubmit" />
                  </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

