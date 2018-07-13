<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="pages_UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
   
        <div class="panel panel-primary">
            <div class="panel-heading">
                CHANGE USER PROFILE
            </div>
            <div class="panel-body">
 <div class="page-container">

                <fieldset>
                    <table style="width: 99%;">
                        <tr>
                            <td style="width: 350px;">Search User:</td>
                            <td>
                                <sars:SearchAdUser ID="txtUserName" runat="server" Width="350px" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                        </tr>
                        
                    </table>
                </fieldset>
                <fieldset>
                    <table style="width: 99%;">
                        <tr>
                            <td style="width: 350px;">Name:</td>
                            <td>
                                <sars:TextField runat="server" ID="txtName" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 350px;">Email Address:</td>
                            <td>
                                <sars:TextField runat="server" ID="txtEmail" TextValidationType="EmailAddress" Enabled="False" /></td>
                        </tr>

                        <tr>
                            <td style="width: 350px;">Phone:</td>
                            <td>

                                <sars:TelephoneField runat="server" ID="txtTel" />

                            </td>
                        </tr>

                        <tr>
                            <td style="width: 350px;">&nbsp;</td>
                            <td>

                            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>


            </div>
        </div>

    </div>
</asp:Content>

