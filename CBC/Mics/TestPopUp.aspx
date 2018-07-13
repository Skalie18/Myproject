<%@ Page Title="" Language="C#" EnableEventValidation="true" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TestPopUp.aspx.cs" Inherits="Mics_TestPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="../Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="../Scripts/jquery-3.1.1.js"></script>
    <script src="../Scripts/jquery-ui-1.12.1.js"></script>
  <%--  <script src="../Scripts/cal.js"></script>--%>
   <input type="button" value="Open Form" id="open-frm" />
    <script>
        var numTimes=0;
        $(function () {
            $("#dialog-message").dialog(
            {
                autoOpen: false,
                modal: true,
                height: "auto",
                width: "1000",
                buttons: {
                    "Submit": function () {
                        $("#dialog-message").dialog("close");
                        <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.Button1))%>;
                    },
                    "Cancel": function () {
                        $("#dialog-message").dialog("close");                        
                        alert("Canceled.");
                    }
                }
            });
            $("#dialog-message").parent().appendTo(jQuery("form:first")).css({ "z-index": "101" });
            $("#open-frm").click(function () {
                if (numTimes >= 5) {
                    alert("You cannot add more than 5");
                    return;
                }
                numTimes++;
                $("#dialog-message").dialog("open");
            });
        });
    </script>
    <div id="dialog-message" title="ADD MORE JURISDICTION DETAILS"  style="display: none">
       
        <div class="form-region">
            <div>
                <div class="form-sub-heading">
                    Summary 
                </div>
                <table>
                    <tr>
                        <td>
                            <CBC:CBCReportSummary runat="server" ID="ctrCBCReportSummary"></CBC:CBCReportSummary>
                        </td>
                    </tr>
                </table>

            </div>

              <div>
                <div class="form-sub-sub-heading">
                    Revenues 
                </div>

                <table>
                    <tr>
                        <td>
                            <CBC:CBCRevenues runat="server" ID="ctrCBCRevenues"></CBC:CBCRevenues>
                        </td>
                    </tr>
                </table>
            </div>

            <div>
                <div class="form-sub-heading">
                    Constituent Entity 
                </div>
                   <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>Number of Constituent Entities in this Tax Jurisdiction</td>
                                        <td>
                                            <sars:NumberField ID="txtNumConstituentEntitiesInTaxJurisdiction" runat="server" MaxLength="3"></sars:NumberField>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

              
                <table>
                    <tr>
                        <td>
                            <CBC:CBCConstituentEntityDetails runat="server" ID="ctrCBCConstituentEntityDetails"></CBC:CBCConstituentEntityDetails>
                        </td>
                    </tr>
                </table>

             
                <div class="form-sub-sub-heading">
                    Address 
                </div>
                <table>
                    <tr>
                        <td>
                            <CBC:CBCAddress runat="server" ID="ctrCBCAddress"></CBC:CBCAddress>
                        </td>
                    </tr>
                </table>
         
               
                <div class="form-sub-sub-heading">
                    Business Activity 
                </div>
                <table>
                    <tr>
                        <td>
                            <CBC:CBCBusinessActivity runat="server" ID="ctrCBCBusinessActivity"></CBC:CBCBusinessActivity>
                        </td>
                    </tr>
                </table>
           
            </div>
        <asp:Button ID="Button1" runat="server" Text="Submit"  OnClick="Button1_Click1"   />
             <%--   <asp:Button ID="Button2" runat="server" Text="Cancel"  OnClick="Button2_Click" />--%>
        </div>
       
         
    </div>

     <script type="text/javascript">   
         //$(function () {
         //    $("web-method").click(function () {
         //        DeleteKartItems();
         //    });
         //});
         function DeleteKartItems() {
             debugger;
                  $.ajax({
                      type: "POST",
                      url: '../Default.aspx/GetUserDetails',
                      data: "",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (msg) {
                          debugger;
                          $("#divResult").html("success");
                      },
                      error: function (e) {
                          $("#divResult").html("Something Wrong.");
                      }
                  });
              }
              </script> 
         <div id="divResult">

        </div>
    <input type="button" id="web-method" value="Call Web Method" onclick="DeleteKartItems();" />

</asp:Content>

