(function () {

    angular.module('cbcproject').directive('taxReference', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attr, ctrl) {

                scope.$watch(attr['ngModel'], function (newValue) {
                    // the value is empty going out of the function 
                    if (newValue === "" ||  newValue == undefined) {
                        return;
                    }



                    if (newValue.length > 0) {
                        // check that the given number is ten digits long
                        if (newValue.length != 10) {
                            ctrl.$setValidity('invalidLength', false);
                            return;
                        }

                        // check that the first digit is either 0, 1, 2 or 3
                        var firstDigitString = newValue.substr(0, 1);
                        var firstDigitNumber = Number(firstDigitString);

                        if (firstDigitNumber != 9) {

                            ctrl.$setValidity('invalidLength', false);
                            return;
                        }


                        var s = newValue;
                        var i = "."

                        if (s.indexOf(i) > -1) {
                            //trace("contains fullstop");
                            ctrl.$setValidity('invalidTaxReference', false);
                            return;
                        }

                        if (newValue == "2222222222" || newValue == "0000000000" || newValue == "9999999999") {

                            ctrl.$setValidity('invalidTaxReference', false);
                            return;
                        }

                        // finally check that the control digit is valid
                        var firstNineDigits = newValue.substr(0, 9);
                        var tenthDigit = newValue.substr(9, 1);
                        var controlDigit = generateControlDigit(firstNineDigits);

                        if ((controlDigit + Number(tenthDigit)) % 10 != 0) {

                            ctrl.$setValidity('invalidTaxReference', false);
                            return;
                        }


                    } 

                    function generateControlDigit(firstNineDigits){

                        var controlDigit = 0;

                        var totalA = addDigits((Number(firstNineDigits.substring(0, 1)) * 2).toString()) + addDigits((Number(firstNineDigits.substring(2, 3)) * 2).toString()) + addDigits((Number(firstNineDigits.substring(4, 5)) * 2).toString()) + addDigits((Number(firstNineDigits.
                            substring(6,7)) *2). toString()) + addDigits((Number(firstNineDigits.substring(8, 9)) * 2).toString());
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

                })
            }
        };
    })
})();