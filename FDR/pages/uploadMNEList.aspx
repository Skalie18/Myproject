<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="uploadMNEList.aspx.cs" Inherits="uploadMNEList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading" >
            UPLOAD MNE LIST</div>
        <div class="panel-body">
            <fieldset>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnGetMNEList" runat="server" Text="Upload MNE List" OnClick="btnGetMNEList_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
                        
        </div>
    </div>
</asp:Content>

