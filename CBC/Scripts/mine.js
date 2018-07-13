/// <reference path="jquery-3.1.1.js" />
/// <reference path="jquery-ui-1.12.1.js" />

$(function () {
    $("#dialog").dialog(
    {
        autoOpen: false,
        modal: true,
        //draggable: false,
        buttons: {
            "Submit": function () {
                $("#dialog").dialog("close");
                $("#diag-decision").text("Submitted successfully")
            },
            "Cancel": function () {
                $("#dialog").dialog("close");
                $("#diag-decision").text("Submition cancelled.")
            }
        }
    });
    $("#opn-diag").click(function () {
        $("#dialog").dialog("open");
    });    
});