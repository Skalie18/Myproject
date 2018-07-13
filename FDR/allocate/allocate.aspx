<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="allocate.aspx.cs" Inherits="allocate_allocate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style>
        table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="panel panel-primary">
        <div class="panel-heading" >
            ALLOCATE CASE</div>
        <div class="panel-body">    
            <div class="page-container">
                <table>
                      <tr>
                        <td>
                            <label for="<%=ddlRoles.ClientID %>">Select User</label></td>
                    </tr>
                    <tr>
                        <td>
                            <sars:DropDownField runat="server" ID="ddlRoles" AutoPostBack="True"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <span onclick="javasctript: return confirm('Are you sure you want to allocate this case?');">
                            <asp:Button ID="btnAllocate" runat="server" Text="Allocate" OnClick="btnAllocate_Click" /></span>
                        </td>
                    </tr>
                </table>
            </div>        
        </div>
    </div>
</asp:Content>

