<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddComments.ascx.cs" Inherits="Controls_AddComments" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table style="width: 100%">
                    <tr>
                        <td>Comments:</td>
                        <td rowspan="4">
                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="form-control" Height="200px" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <table style="width:50%">
                                <tr>
                                    
                                    <td>
                                        <asp:Button ID="btnAddComments" runat="server" Text="Add Comments" OnClick="btnAddComments_Click"></asp:Button>

                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
