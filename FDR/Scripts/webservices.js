selectUser = function (selectedRow, txt, hiddenField) {
    var searchBox = document.getElementById(txt);
    var hf = document.getElementById(hiddenField);
    if (selectedRow) {
        selectedRow.className = "selectedRow";
        var sid = selectedRow.cells[1].innerHTML;
        if (hf) {
            hf.value = sid;
        }
        var fullName = selectedRow.cells[0].innerHTML;// + ' ' + selectedRow.cells[1].innerHTML;
        var selectedUser = fullName + ' | ' + sid;
        searchBox.value = selectedUser;
        $("#gvUsers.documents").hide();
    } 
};

function findAdUser(txt, div, hf) {
    var searchtext = document.getElementById(txt).value;
    var hiddenField = hf;
    if (searchtext.length > 5) {
        SearchActiveDirectoryUser(searchtext, div, txt, hiddenField);
    }
}
function SearchActiveDirectoryUser(searchName, container, txt, hiddenField) {
    container.innerHTML = '';
    window.SurveyService.SearchUsers(
        searchName, function (result) {
            processServiceResults(eval(result), container, txt, hiddenField);
        }, RequestFailedCallback);
}

processServiceResults = function (data, container, txt, hiddenField) {
    var content = '<table style="width:100%;" class="documents" id="gvUsers">' +
        '<tr>' +
            '<th>Full Name</th>' +
            '<th>SID</th>' +
        '</tr>';
    var hfId = '' + hiddenField.id + '';
    if (data) {

        for (var i = 0, len = data.length; i < len; ++i) {
            var html = createRow(data[i], txt, hfId);
            content = content + ' ' + html;

        }
    }
    else {
        content = '<table style="width:100%;" class="tableSytle"><tr><td>THERE IS NO USER FOUND FOR YOUR CRITERIA</td></tr> </table>';
    }
    content = content + ' </table>';
    container.innerHTML = content;
};


createRow = function (aduser, txt, hiddenField) {
    var html = '<tr id=\'selected-row\' onmouseover="style.cursor=\'cursor\'" onclick="selectUser(this,\'' + txt + '\', \'' + hiddenField + '\');" >' +
        '<td>' + aduser.Name + ' ' + aduser.Surname + '</td>' +
        '<td>' + aduser.SID + '</td>' +
        '</tr>';

    return html;

};




function RequestFailedCallback(error) {
    var stackTrace = error.get_stackTrace();
    var message = error.get_message();
    var statusCode = error.get_statusCode();
    var exceptionType = error.get_exceptionType();
    var timedout = error.get_timedOut();


    var errorMessage = "Stack Trace: " + stackTrace + "<br/>" +
                 "Service Error: " + message + "<br/>" +
                 "Status Code: " + statusCode + "<br/>" +
                 "Exception Type: " + exceptionType + "<br/>" +
                 "Timeout: " + timedout;

    alert(errorMessage);
}