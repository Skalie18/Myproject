(function () {
   // 'use strict';
    angular.module('cbcproject').factory('fieldtypeservevices', fieldtypeservevices);

    function fieldtypeservevices($q) {

        return {
            isAlphaNumericField: isAlphaNumericField,
            isAlphaField: isAlphaField,
            isMinFourPostalCode: isMinFourPostalCode,
            isCurrencyFieldValidation: isCurrencyFieldValidation,
            IdNumberFieldValidation: IdNumberFieldValidation,
            PassportNoNoSpacesDashesField: PassportNoNoSpacesDashesField,
            isEmailAddressField: isEmailAddressField,
            taxReferenceNumberVAlidation: taxReferenceNumberVAlidation,
            isValidDate: isValidDate,
            isMinNineTelPhone:isMinNineTelPhone,
            isGIINNoField:isGIINNoField
        }

        function isValidDate(parameter) {
            var promise = $q(function (resovle, reject) {
                //var regEx = RegExp("/^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/");
                //var isValid = regEx.test(parameter);
                var validatePattern = /^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/;
                var isValid = parameter.match(validatePattern); //&& !function isFutureDate(parameter);

                if (!isValid) {
                    reject(parameter);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }


        function isEmailAddressField(parameter) {
            var promise = $q(function (resovle, reject) {
                // Livhu changed the regex due to the form capitilizes the user input, commented line below was the orininal
                // var regEx = RegExp("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$");
                var regEx = RegExp("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$");
                var isValid = regEx.test(parameter.value);
                if (!isValid) {
                    reject(parameter.name);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function isAlphaNumericField(parameter) {
            var promise = $q(function (resovle, reject) {
                var regEx = RegExp("^[ a-zA-Z0-9\-]*$");
                var isValid = regEx.test(parameter.value);
                if (!isValid) {
                    reject(parameter.name);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function isAlphaField(parameter) {

            var promise = $q(function (resovle, reject) {
                var regEx = RegExp("^[a-zA-Z]*$");
                var isValid = regEx.test(parameter);
                if (!isValid) {
                    reject(parameter);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function isMinFourPostalCode(parameter) {

            var promise = $q(function (resovle, reject) {
                var regEx = RegExp("^[ a-zA-Z0-9]{4,10}$");
                var isValid = regEx.test(parameter.value);
                if (!isValid) {
                    reject(parameter.name);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function isCurrencyFieldValidation(parameter) {

            var promise = $q(function (resovle, reject) {
                var regEx = RegExp("^[0-9]*$");
                var isValid = regEx.test(parameter);
                if (!isValid) {
                    reject(parameter);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function IdNumberFieldValidation(control) {


            var promise = $q(function (resovle, reject) {
                var idNumber = control;
                // assume everything is correct and if it later turns out not to be, just set this to false
                var correct = true;

                if (control !== '') {
                    //Ref: http://www.sadev.co.za/content/what-south-african-id-number-made
                    // SA ID Number have to be 13 digits, so check the length
                    if (idNumber.length !== 13) {
                        correct = false;
                    }

                    // get first 6 digits as a valid date
                    var tempDate = new Date(idNumber.substring(0, 2), idNumber.substring(2, 4) - 1, idNumber.substring(4, 6));

                    var id_date = tempDate.getDate();
                    var id_month = tempDate.getMonth();
                    // var id_year = tempDate.getFullYear();
                    id_month = Number(id_month) + 1;

                    //  var fullDate = id_date + "-" + id_month.toString() + "-" + id_year;

                    if (!((tempDate.getYear() === Number(idNumber.substring(0, 2))) && (id_month === Number(idNumber.substring(2, 4))) && (id_date === Number(idNumber.substring(4, 6))))) {
                        correct = false;
                    }

                    // get the gender
                    //   var genderCode = idNumber.substring(6, 10);
                    // var gender = parseInt(genderCode) < 5000 ? "Female" : "Male";

                    // get country ID for citzenship
                    // var citzenship = parseInt(idNumber.substring(10, 11)) === 0 ? "Yes" : "No";

                    // apply Luhn formula for check-digits
                    var tempTotal = 0;
                    var checkSum = 0;
                    var multiplier = 1;
                    for (var i = 0; i < 13; ++i) {
                        tempTotal = parseInt(idNumber.charAt(i)) * multiplier;
                        if (tempTotal > 9) {
                            tempTotal = parseInt(tempTotal.toString().charAt(0)) + parseInt(tempTotal.toString().charAt(1));
                        }
                        checkSum = checkSum + tempTotal;
                        multiplier = (multiplier % 2 === 0) ? 1 : 2;
                    }
                    if ((checkSum % 10) !== 0) {
                        correct = false;
                    }

                }
                if (!correct) {
                    reject(control);
                } else {
                    resovle(control);
                }
            })
            return promise;
            // first clear any left over error messages


        }

        function PassportNoNoSpacesDashesField(control) {
            var promise = $q(function (resovle, reject) {
                var regEx = RegExp("^[a-zA-Z0-9]{8,18}$");
                var isValid = regEx.test(control);
                if (!isValid) {
                    reject(control);
                } else {
                    resovle(control);
                }
            })
            return promise;
        }

        function taxReferenceNumberVAlidation(newValue) {

            var promise = $q(function (resovle, reject) {

                if (newValue.length > 0) {
                    // check that the given number is ten digits long
                    if (newValue.length != 10) {
                        reject();
                        return;
                    }

                    // check that the first digit is either 0, 1, 2 or 3
                    var firstDigitString = newValue.substr(0, 1);
                    var firstDigitNumber = Number(firstDigitString);

                    if (firstDigitNumber != 9) {

                        reject();
                        return;
                    }


                    var s = newValue;
                    var i = "."

                    if (s.indexOf(i) > -1) {
                        //trace("contains fullstop");
                        reject();
                        return;
                    }

                    if (newValue == "2222222222" || newValue == "0000000000" || newValue == "9999999999") {

                        reject();
                        return;
                    }

                    // finally check that the control digit is valid
                    var firstNineDigits = newValue.substr(0, 9);
                    var tenthDigit = newValue.substr(9, 1);
                    var controlDigit = generateControlDigit(firstNineDigits);

                    if ((controlDigit + Number(tenthDigit)) % 10 != 0) {
                        reject();
                        return;
                    }

                } else {
                    resovle();

                }


            })

            return promise
        }

        function isMinNineTelPhone(value) {

            var promise = $q(function (resolve, reject) {
                var regEx = RegExp("^[0-9]{10,15}$");
                var isValid = regEx.test(value);
                if (!isValid) {
                    reject();
                } else {
                    resolve();
                }
            })
            return promise;
        }
        
    }

    function generateControlDigit(firstNineDigits) {

        var controlDigit = 0;

        var totalA = addDigits((Number(firstNineDigits.substring(0, 1)) * 2).toString()) + addDigits((Number(firstNineDigits.substring(2, 3)) * 2).toString()) + addDigits((Number(firstNineDigits.substring(4, 5)) * 2).toString()) + addDigits((Number(firstNineDigits.
            substring(6, 7)) * 2).toString()) + addDigits((Number(firstNineDigits.substring(8, 9)) * 2).toString());
        var totalB = Number(firstNineDigits.substring(1, 2)) + Number(firstNineDigits.substring(3, 4)) + Number(firstNineDigits.substring(5, 6)) + Number(firstNineDigits.substring(7, 8));

        controlDigit = totalA + totalB;

        return controlDigit;

    }
    function addDigits(number) {
        var total = 0;

        for (var i = 0; i < number.length; i++) {
            total += Number(number.substring(i, (i + 1)));
        }

        return total;
    }
    
    function isGIINNoField(parameter) {
        var isValid = false;
        if((parameter.length > 0 && parameter.length === 19) && (parameter.indexOf('.') >= 6)){
            var chars = parameter.split('.');
            if(chars[0].length === 6 && chars[1].length === 5 && chars[2].length === 2 && chars[3].length === 3){
                isValid = true;
            }
            if(isValid){
                var regEx = RegExp("^([A-Z0-9]{6})+\.([A-Z0-9]{5})+\.([A-Z0-9]{2})+\.([A-Z0-9]{3})$");
                return regEx.test(parameter);
            }
            
        }
       
        return false;
        
    }


})();