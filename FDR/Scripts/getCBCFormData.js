function QueryCBCFormData() {
    var tRefNo = GetParameterValues('id');
    if (tRefNo !== undefined) {
        $.ajax({
            type: "POST",
            url: 'http://localhost/FDR/mnelistservice.asmx/GetCBCDataByTaxRefNo',
            data: '{"taxRefNo": "' + tRefNo + '"}',
            // data: '',
            dataType: "xml",
            success: successful,
            error: failed
        });
    }
}

function successful(results, status, req) {
    alert(results.d);

}

function failed(xhr) {
    alert(xhr.responseText);
}

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] === param) {
            alert(urlparam[1]);
            return urlparam[1];
        }
    }
}