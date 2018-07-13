<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CaptureCaseDetails.aspx.cs" Inherits="CaptureCaseDetials" %>

<%@ Import Namespace="System.IO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        input[type=file], input[type=file]  {
            display: inline-block;
            background-color: #0094ff;
            border: 1px solid gray;
            font-size: 15px;
            width: 100%;
            padding: 4px;
        }

            input[type=file] + input {
                padding: 13px;
                /* background-color: #00b7cd;*/
            }

        ::-webkit-file-upload-button {
            -webkit-appearance: none;
            background-color: #00b7cd;
            border: 1px solid gray;
            font-size: 15px;
            padding: 8px;
        }

        ::-ms-browse {
            background-color: #ffffff;
            border: 1px solid gray;
            font-size: 15px;
            padding: 8px;
        }

        input[type=file]::-ms-value {
            border: none;
        }

        .dvUpload {
            width: 100%;
        }

        .inputclass {
            /*width: 100% !important;*/
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script>
        $(function () {
            $('#<% =btnSave.ClientID%>').click(function () {


                var TaxRefNo = $("#<%=txtTaxRefNo.ClientID%>");
                var CaseNo = $("#<%=txtCaseNo.ClientID%>");
                var Year = $("#<%=txtYear.ClientID%>");
                var Country = $("#<%=ddlCountryList.ClientID%>");
                var RequestorandUnit = $("#<%=txtRequestorUnit.ClientID%>");
                var EntityName = $("#<%=txtEntityName.ClientID%>");
                var DateRequested = $("#<%=txtDateRequested.ClientID%>");
                var DateRecieved = $("#<%=txtDateRecieved.ClientID%>");
                var Notes = $("#<%=txtNotes.ClientID%>");

                if (TaxRefNo.val() === '' && CaseNo.val() === '' && Year.val() === '' && Country.val() === '' && RequestorandUnit.val() === ''
                    && EntityName.val() === '' && DateRequested.val() === '' && DateRecieved.val() === '' && Notes.val() === '') {
                    alert("Enter All Fields");
                    return false;
                }

                if (TaxRefNo.val() === '') {
                    alert("Please Enter Tax Reference Number");
                    return false;
                }

                if (CaseNo.val() === 0) {
                    alert("Please Enter Case Number");
                    return false;
                }

                if (Year.val() === '') {
                    alert("Please Enter Tax Year");
                    return false;
                }

                if (Country.val() === '') {
                    alert("Please Select Country");
                    return false;
                }

                if (RequestorandUnit.val() === '') {
                    alert("Please Enter Requestor and Unit");
                    return false;
                }

                if (EntityName.val() === '') {
                    alert("Please Enter The Entity Name");
                    return false;

                }

                if (DateRequested.val() === '') {
                    alert("Please Select The Date Requested");
                    return false;
                }

                if (DateRecieved.val() === '') {
                    alert("Please Select The Date Recieved ");
                    return false;
                }

                if (Notes.val() === '') {
                    alert("Case Note Is Required");
                    return false;
                }

                if (DateRequested.val() > DateRecieved.val()) {
                    alert("Date Requested Cannot Be Greater Than Date Recieved");

                    DateRequested.val = "";
                    DateRecieved.val = "";
                    return false;
                }

                return true;
            });


        });




    </script>
    <div class="panel panel-primary">
        <div class="panel-heading">
            CAPTURE CASE DETAILS
        </div>
        <div class="panel-body">
            <div class="page-container">

                <div>
                    <table style="width: 99%;">
                        <tr>
                            <td>
                                <label for="<%=txtTaxRefNo.ClientID %>">Tax Ref No</label>
                            </td>
                            <td>
                                   <label for="<%=txtCaseNo.ClientID %>"><span>Case No</span></label>
                              </td>
                        </tr>
                        <tr>
                            <td>
                                <sars:NumberField runat="server" ID="txtTaxRefNo" CssClass="inputclass" NumberTypes="CIT">

                                </sars:NumberField>
                            </td>
                            <td>

                                <sars:TextField ID="txtCaseNo" runat="server" CssClass="inputclass"></sars:TextField>

                            </td>

                        </tr>




                        <tr>
                            <td> 
                                 <label for="<%=txtYear.ClientID %>">Year</label>
                            </td>
                            <td>
                                 <label for="<%=ddlCountryList.ClientID %>">Country</label>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <sars:NumberField runat="server" ID="txtYear" CssClass="inputclass" NumberTypes="TaxYear"></sars:NumberField>
                            </td>
                            <td>
                                <sars:DropDownField ID="ddlCountryList" runat="server" width="400px" AutoPostBack="false"
                                    CssClass="inputclass">
                                </sars:DropDownField>
                            </td>

                        </tr>





                        <tr>
                            <td>
                                 <label for="<%=txtRequestorUnit.ClientID %>">  Requestor and Unit </label>
                              

                            </td>
                            <td>
                                <label for="<%=txtEntityName.ClientID %>"> Entity Name </label>
                         

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <sars:TextField ID="txtRequestorUnit" runat="server" CssClass="inputclass"></sars:TextField>
                            </td>
                            <td>

                                <sars:TextField ID="txtEntityName" runat="server" CssClass="inputclass"></sars:TextField>

                            </td>

                        </tr>




                        <tr>
                            <td>
                               
                                  <label for="<%=txtDateRequested.ClientID %>"> Date Requested</label>
                            </td>
                            <td>
                                  <label for="<%=txtDateRecieved.ClientID %>">  Date Recieved</label>
                               </td>
                        </tr>
                        <tr>
                            <td>

                                <sars:SarsCalendar ID="txtDateRequested" runat="server"  width="400px" MaximumDate="0+" CanBeInFuture="false" />


                            </td>
                            <td>

                                <sars:SarsCalendar ID="txtDateRecieved" runat="server" width="400px" MaximumDate="0+" CanBeInFuture="false" />


                            </td>

                        </tr>
                        <tr>
                            <td>
                                  <label for="<%=txtNotes.ClientID %>">Notes </label>
                                </td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <sars:TextField TextMode="MultiLine" runat="server" Height="200px" Width="99%" ID="txtNotes"></sars:TextField>
                            </td>

                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td> <label for="<%=fileUpload.ClientID %>"><span>Select File</span> </label></td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <asp:FileUpload ID="fileUpload" AllowMultiple="true" enctype="MULTIPART/FORM-DATA" runat="server" />

                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"></asp:Button>

                            </td>
                            <td>&nbsp;</td>

                        </tr>
                    </table>

                </div>
                <div>

                    <asp:GridView runat="server" CssClass="documents" ID="gvUploadedDocs" AutoGenerateColumns="False" EmptyDataText="NO FILES UPLOADED" GridLines="Horizontal" DataKeyNames="FileName">
                        <Columns>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="list" ImageUrl="~/Images/Icons/list.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name">

                                <ItemTemplate>
                                    <%# Path.GetFileName(Eval("FileName").ToString())%>
                                  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="File Name" Visible="false" />
                            <asp:BoundField DataField="ContentType" HeaderText="Content Type" />
                            <asp:BoundField DataField="ContentLength" HeaderText="Size" />

                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" Text="Delete File" ID="lnkDeleteFile" OnClientClick="javascript:return confirm('Are you sure you want to delete this file?');" CommandArgument='<%# Path.GetFileName(Eval("FileName").ToString())%>' OnCommand="lnkDeleteFile_Command"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="document-pager"></PagerStyle>
                    </asp:GridView>

                </div>


                <fieldset>
                    <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnSave_Click" />
                </fieldset>

            </div>

        </div>


    </div>
</asp:Content>

