<%@ Page Language="C#" MasterPageFile="~/Site.master" Title="::ERROR OCCURED::" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="Logging_ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <div class="panel panel-primary">
        <div class="panel-heading">
            FDR ENCOUNTERED AN ERROR
        </div>
        <div class="panel-body">
            <div class="panel-body">
              <%--  <table border="1" class="documents">
                    <tr>
                        <td><b>Server Variable</b></td>
                        <td><b>Value</b></td>
                    </tr>
                    <% foreach (string strKey in Request.ServerVariables)
                       { %>
                    <tr>
                        <td><%= strKey %></td>
                        <td><%= Request.ServerVariables[strKey] %></td>
                    </tr>
                    <% } %>
                </table>--%>
                <table style="width: 100%; padding: 0px; border-collapse: collapse;">
                   
                    <tr>
                        <td>
                                
                                <div>
                                    <strong><em>Good day,</em></strong><br />
                                    <em>An Error occured in the system, all support personnel have been informed about this error
                                    <br />
                                        Your Correlation Id :<b><%=Session.SessionID %></b>
                                    </em>
                                </div>

                        </td>
                    </tr>
                 
                </table>
            </div>

        </div>
</asp:Content>
