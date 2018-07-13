function setAllDivisions(obj1, obj2, thisField) {
    debugger;
        obj1.disabled = thisField.checked;
        obj2.disabled = thisField.checked;
}

function CountChars(field, maxlimit) 
{
    if (field.value.length > maxlimit)
     {
        field.value = field.value.substring(0, maxlimit);
    }
}


var upper = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ,.\/\"\'`!@#$%^&*()-+_|<> ';
var number = '0123456789.';
var lower = 'abcdefghijklmnopqrstuvwxyz,.\/\"\'`!@#$%^&*()-+_|<> ';
var money = '0123456789.';


function isValid(value, option) {
    if (value == "") {
        return true;
    }
    for (i = 0; i < value.length; i++) {
        var index = option.indexOf(value.charAt(i), 0);
        if (index == -1) {
            return false;
        }
    }
    return true;
}


isLower = function (field) {
    var v = trim(field.value);
    return isValid(v, lower);
}


isAlphanum = function (field) {
    debugger;
    var v = trim(field.value);
    var x = isValid(v, lower + '' + upper + '' + number);
    if (x == false) {

        field.focus();
        alert('This field requires alpha numerics');
        field.style.backgroundColor = "red";
        field.value = "";
    }
    else {
        field.style.backgroundColor = "transparent";
    }
}


isAlpha = function (field) {
    debugger;
    var v = trim(field.value);
    var x = isValid(v, lower + upper);
    if (x == false) {

        field.focus();
        alert('This field requires ONLY alphabets');
        field.style.backgroundColor = "red";
        field.value = "";
    }
    else {
        field.style.backgroundColor = "transparent";
    }
}
    isUpper = function (field) {
    return isValid(field.value, upper);
}


isNumber = function (field) {
    //debugger;
    var v = trim(field.value);
    var x = isValid(v, number);
    if (x == false) {

        field.focus();
        alert('This field requires only numbers');
        field.style.backgroundColor = "red";
        field.value = "";
    }
    else {
        field.style.backgroundColor = "transparent";
    }
}
isMoney = function (field) {
    //debugger;
    var v = trim(field.value);
    var x = isValid(v, money);
    if (x == false) {

        field.focus();
        alert('This field requires only currency value');
        field.style.backgroundColor = "red";
        field.value = "";
    }
    else {
        field.style.backgroundColor = "transparent";
    }
}

function trim(str) {
    if (!str || typeof str != 'string') return null; return str.replace(/^[\s]+/, '').replace(/[\s]+$/, '').replace(/[\s]{2,}/, ' ');
}

function isNumberKey(evt, value) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    if (charCode == 46 && value == '')
        return false;
    if (charCode == 46 && value.indexOf('.') != -1)
        return false;

    return true;
}
function CountChars(field, maxlimit) {
    if (field.value.length > maxlimit) {
        field.value = field.value.substring(0, maxlimit);
    }
}