<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <table>
                    <tr>
                        <td>
                            <CBC:CBCRevenues runat="server" ID="ctrCBCRevenues"></CBC:CBCRevenues>
                        </td>
                    </tr>
                </table>
    
   <div class="panel panel-primary">
                            <div class="panel-heading">
                                PAGE CONTENT
                            </div>
                            <div class="panel-body">
                                <table style="width: 100%;">

                                    <tr>
                                        <td>
                                      

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
</asp:Content>

