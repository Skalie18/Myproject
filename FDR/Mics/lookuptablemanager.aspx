<%@ Page Title="" MasterPageFile="~/Site.master" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="lookuptablemanager.aspx.cs" Inherits="lookuptablemanager" %>
<%@ Register TagPrefix="sars" Namespace="Sars.Systems.Controls" Assembly="Sars.Systems, Version=4.5.0.0, Culture=neutral, PublicKeyToken=6269130e95be942f" %>
<%@ Import Namespace="Sars.Systems.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        .column-name {
            width: 250px!important;
        }
        .noclose .ui-dialog-titlebar-close {
            display: none;
        }
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <script   src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="http://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function showmodalpopup() {
            $("#popupdiv").dialog({
                title: "LOOKUP MANAGEMENT",
                resizable: true,
                width: '1000',
                height: 'auto',
                modal: true,
                buttons: {
                    Close: function() {
                        //$(this).dialog('close');
                    }
                },
                open: function(type, data) {
                    $(this).parent().appendTo("form");
                    $(".ui-dialog-buttonset").hide();
                }
            });
        };

        function showMessageBox() {
            $("#dialog").dialog({
                width: 'auto',
                height: 'auto',
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
        <div class="panel panel-primary">
        <div class="panel-heading">
            LOOK UP TABLE MANAGER
        </div>
        <div class="panel-body">
            <div class="page-container">
                <table style="width: 100%;">
                    <tr>
                        <td><label>Select a table</label></td>
                    </tr>
                     <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlTables" Width="100%" OnSelectedIndexChanged="ddlTables_SelectedIndexChanged" AutoPostBack="True">
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                   
                </table>
                <fieldset>
                    <asp:GridView  runat="server" GridLines="None" ID="gvData" CssClass="documents" OnRowDataBound="gvData_RowDataBound" OnSelectedIndexChanged="gvData_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging">
                        <PagerStyle CssClass="document-pager" />
                        <SelectedRowStyle CssClass="selectedRow" />
                        
                    </asp:GridView>
                </fieldset>
                 
                <div id="popupdiv" title="Basic modal dialog" style="display: none;">
                    
                    <asp:GridView ShowHeader="False" DataKeyNames="DatabaseName,Schema,TableName,Name" runat="server" GridLines="None" ID="gvColumns" CssClass="documents" AutoGenerateColumns="False" OnRowDataBound="gvColumns_RowDataBound1" ShowFooter="True" OnSelectedIndexChanged="gvColumns_SelectedIndexChanged">
                        <columns>
                            <asp:BoundField DataField="DatabaseName" HeaderText="Database Name" Visible="False" />
                            <asp:BoundField DataField="Schema" HeaderText="Schema" Visible="False" />
                            <asp:BoundField DataField="TableName" HeaderText="Table Name" Visible="False" />
                          

                            <asp:TemplateField HeaderText="Name" >
                                <ItemTemplate>
                                     <asp:Label ID="lblName" runat="server" Text='<%#  Eval("Name") %>' Visible="False"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Text='<%#  Eval("Name").ToString().SplitString()  %>'></asp:Label>
                                </ItemTemplate>

                                <HeaderStyle CssClass="column-name" Width="250px" />
                                <ItemStyle CssClass="column-name" />

                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtValue"></asp:TextBox>
                                      <sars:SarsCalendar runat="server" ID="txtDate" Visible="False" Width="200px"></sars:SarsCalendar>
                                    <asp:CheckBox ID="chkAnswer" runat="server" Visible="False"></asp:CheckBox>
                                </ItemTemplate>
                               <%-- <FooterTemplate>
                                    <asp:Button runat="server" ID="btnSave" Text="SAVE" OnClick="btnSave_Click"/>
                                </FooterTemplate>--%>
                                 <HeaderStyle Width="500px" />
                            </asp:TemplateField>
                        </columns>
                        <SelectedRowStyle CssClass="selectedRow" />
                    </asp:GridView>
                 <asp:Button runat="server" ID="btnSave" Text="SAVE" OnClick="btnSave_Click"/>
                 <asp:Button runat="server" ID="btnCancel" Text="CANCEL" OnClick="Cancel"/>
                </div>

                <div id="dialog" style="display: none">
                    Record Updated Successfully.
                </div>
            </div>
        </div>
    </div>
</asp:Content>

