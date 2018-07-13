<%@ Page Title="" Language="C#" MasterPageFile="~/SiteNoMenu.master" AutoEventWireup="true" CodeFile="cbcForm.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        iframe {
            width: 100%;
            height: 1200px;
        }

        #output {
            width: 100%;
        }

        .lgnd {
            margin-left: auto;
            margin-right: auto;
            padding-top: 8px;
            border: 1px solid silver;
            border-radius: 4px;
            background-color: #f0e68c;
            height: 30px;
            width: 350px;
            text-align: center;
            vertical-align: central;
            color: #31708f;
            font-size: 12px;
            font-weight: bold;
            background: linear-gradient(to bottom, #428bca, #ffffff);
        }
    </style>
    <script src="../Scripts/jquery-1.6.2.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="output">

        <div class="panel panel-primary">
            <div class="panel-heading">
                VIEW SUBMITTED CBC
            </div>
            <div class="panel-body">
                <fieldset style="width: 510px;" runat="server" id="fsVerify">
                    <legend class="lgnd">Verify this report</legend>
                    <div>
                        <table style="width: 500px;">
                            <tr>
                                <td>
                                    <sars:RadioButtonListField runat="server" ID="rdbVerify">
                                        <asp:ListItem Value="1">ACCEPTED</asp:ListItem>
                                        <asp:ListItem Value="0">REJECTED</asp:ListItem>
                                    </sars:RadioButtonListField>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit Verification" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <asp:HiddenField ID="hdnApproved" runat="server" />
                <div class="page-container">
                    <asp:HiddenField ID="hdnJsonData" runat="server" />
                    <iframe name="myIframe" src="../dist/index.html" id="myIframe" frameborder="0"
                        runat="server"></iframe>
                    <table style="width: 97%;">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" /></td>
                        </tr>
                    </table>

                </div>


            </div>
        </div>


    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var iframe = document.getElementById('MainContent_myIframe');
            var loaded = 0;
            var message = "";
            $('#<%=btnSubmit.ClientID%>').click(function (e) {
                var selectedValue = 1;
                e.preventDefault();
                var conMessage = "Are you sure you want to Approve this CBC report?";
                var loggedInUser = '<%=userId%>';

                if (!$('#MainContent_rdbVerify_0').is(':checked') && !$('#MainContent_rdbVerify_1').is(':checked')) {
                    alert('You must select Approve or Reject');
                    return false;
                }
                if ($('#MainContent_rdbVerify_1').is(':checked')) {
                    conMessage = 'Are you sure you want to Reject this CBC report?';
                    selectedValue = 0;
                }

                if (!confirm(conMessage)) {
                    return false;
                }
                else {
                    approverejectCBC(selectedValue, loggedInUser);
                    if (selectedValue === 0) {
                        message = 'CBC report rejected successfully';
                    }
                    else {
                        message = 'CBC report approved successfully';
                    }
                    $('#<%=hdnApproved.ClientID%>').val(selectedValue);
                    alert(message);
                    // queryDataFromService();

                }

            });

            function succeeded(results, status, req) {
                $('#<%=fsVerify.ClientID%>').hide();
                // location.reload(true);
            }

            function unsucceeded(msg) {
                alert("Service Error");
                //location.reload(true);
            }

            $(iframe).hide();
            queryDataFromService();

            function queryDataFromService() {
                var apiUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["apiUrl"].ToString()%>';
                var tRefNo = getUrlVars()["refno"];
                var taxYear = getUrlVars()["year"];
                var isIncLocal = getUrlVars()["incLocal"];
                var msId = getUrlVars()["mspecId"];
                var params = { incLocal: isIncLocal, taxRefNo: tRefNo, year: taxYear, mspecId:msId };
                if (tRefNo === '' && taxYear === '') {
                    alert('An error occured. Please try again');
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: apiUrl + 'GetCBCDataByTaxRefNo',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    success: successful,
                    error: failed
                });
            }

            function approverejectCBC(selectedValue, userID) {
                var apiUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["apiUrl"].ToString()%>';
                var tRefNo = getUrlVars()["refno"];
                var taxYear = getUrlVars()["year"];
                var params = { taxRefNo: tRefNo, year: taxYear, approved: selectedValue, userId: userID };
                if (tRefNo === '' && taxYear === '') {
                    alert('An error occured. Please try again');
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: apiUrl + 'ApproveCBCReport',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    success: succeeded,
                    error: unsucceeded
                });
            }

            function setData(data) {
                data = JSON.parse(data);
                var jsonData = JSON.stringify(data);

                $('#<%=hdnJsonData.ClientID%>').val(jsonData);
                if (loaded === 1) {
                    populateData();
                }

            }

            function DisableInput() {
                $(iframe).contents().find('input,textarea,select').removeAttr('required');
                $(iframe).contents().find('input,textarea,select').removeAttr('ng-required');
                $(iframe).contents().find('input,button,textarea,select').attr('disabled', 'disabled');
                $(iframe).contents().find('.dropdown-menu').removeAttr('required');
                $(iframe).contents().find('.dropdown-menu').removeAttr('ng-required');
                $(iframe).contents().find('.dropdown-menu').css('display', 'none');
                $(iframe).contents().find('span').attr('disabled', 'disabled');
                $(iframe).contents().find('i,ul,li,a').attr('disabled', 'disabled');
                $(iframe).contents().find('table:input').attr('disabled', 'disabled');
                $(iframe).contents().find('div').attr('disabled', 'disabled');
                $(iframe).contents().find('label').attr('disabled', 'disabled');


            }
            function successful(results, status, req) {
                if (results.d === '') {
                    alert('no CBC form data found');
                    $(iframe).hide();
                    $('#<%=fsVerify.ClientID%>').hide();
                    return;
                }
                $(iframe).show();
                setData(results.d);
            }

            function failed(msg) {
                alert("Service Error");

            }

            $(window).error(function (e) {
                console.log(e.error);
                e.preventDefault();
                return;
            });

            function getUrlVars() {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            }

            $(iframe).load(function () {
                loaded = 1;
                populateData();


                $(this).contents().find('a').click(function (event) {
                    DisableInput();
                    return false;
                    event.preventDefault();
                });

                $(this).contents().find('span').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('label').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('div').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('button').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('select').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('.dropdown-menu').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('ul').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });

                $(this).contents().find('li').click(function (event) {
                    DisableInput();
                    event.preventDefault();
                });
            });


            function populateData() {
                var jData = $('#<%=hdnJsonData.ClientID%>').val();
                var rows = null;
                if (jData !== '') {
                    var body = $(iframe).get(0).contentWindow.document.body;
                    var container = $(body).children();
                    if ($(container).length > 0) {
                        rows = $(container[0]).children();
                    }
                    else {
                        container = $('.container');
                        rows = $(container[0]).children();
                    }
                    var targetTextbox = $(rows[1]).children();
                    try {
                        $(targetTextbox[0]).text(jData);
                        var targetButton = $(rows[2]).children();
                        $(targetButton[1]).trigger('click');
                    }
                    catch (err) {
                        if (!err.message.indexOf('@currCode') === -1)
                            alert(err.message);
                    }
                    finally {
                        $(container[0]).hide();
                        DisableInput();
                        $(targetTextbox[0]).text('');
                        $('#<%=hdnJsonData.ClientID%>').val('');
                        tRefNo = '';
                        taxYear = '';
                    }
                }
            }

            function myHandler(msg, url, line) {
                console.log(msg);
            }

            //hook in all frames...
            function addErrorHandler(win, handler) {
                win.onerror = handler;
                for (var i = 0; i < win.frames.length; i++) {
                    addErrorHandler(win.frames[i], handler);
                }
            }
            //start with this window... and add handler recursively
            addErrorHandler(window, myHandler);
            $(iframe).contents().find('.container').hide();
        });


    </script>
</asp:Content>

