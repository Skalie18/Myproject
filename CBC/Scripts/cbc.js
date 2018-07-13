/// <reference path="jquery-3.1.1.js" />
/// <reference path="jquery-ui-1.12.1.js" />
$(function () {
    $("#dialog-message").dialog(
               {
                   autoOpen: false,
                   modal: true
               }
               );
    $("#btn").click(function () {
        $("#dialog-message").dialog("open");
    });
});