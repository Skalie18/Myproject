<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="showletter.aspx.cs" Inherits="admin_showletter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading" >
            LETTER XML</div>
        <div class="panel-body">  
            <div class="page-container">
                <asp:TextBox runat="server" TextMode="MultiLine" Rows="30" Width="100%" ID="txtXml"></asp:TextBox>
            </div>          
        </div>
    </div>
</asp:Content>

