(function () {
    angular.module('cbcproject').factory('companyregservice', function () {

        return {
            validateCompanyRegNo: function (companyRegNumber) {

                if(companyRegNumber.length == 9) {
                    if(registeredNo.data.substring(0,1) != "K") {
                        return false;
                    }
                    if(isNaN(Number(companyRegNumber.substring(1)))) {
                        return false;
                    }
                    return true;
                }else{
                    var now = new Date();
                    var yyyy = now.getFullYear();
                    var minYear = 1800;
                    var firstChar = companyRegNumber.toString().substr(0, 1);
                    var firstFourChars = companyRegNumber.toString().substr(0, 4);
                    var firstSlashFifthChar = companyRegNumber.toString().substr(4, 1);
                    var sixToElvenChars = companyRegNumber.toString().substr(5, 6);
                    var secondSlashTwlvChar = companyRegNumber;
                    var charField = secondSlashTwlvChar.substr(11,1);
                    var twlvToThirteenChars = companyRegNumber.toString().substr(12, 2);
                    
                    if ((isNaN(Number(firstFourChars))) || (isNaN(Number(sixToElvenChars)))) {
                        return false;
                    }
                    
                    if (Number(firstFourChars) > yyyy || Number(firstFourChars) < minYear) {
                        return false;
                    }
                    
                    if (firstSlashFifthChar != "/") {
                        return false;
                    }
                    
                    if (charField != "/") {
                        return false;
                    }
                    
                    if (registeredNo.data.length > 14) {
                        return false;
                    }
                    
                    return checkLastTwoDigits(twlvToThirteenChars);

                }
                
            }
        }

        function checkLastTwoDigits(value) {
			var lastTwoDigitsArray = ["06", "07", "08", "09", "10", "11", "20", "21", "22", "23", "24", "25", "26","30","31"];
			return lastTwoDigitsArray.contains(value);
		}

    })
    
}());