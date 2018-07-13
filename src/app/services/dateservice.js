//dateservices
(function () {
    // 'use strict';
    angular.module('cbcproject').factory('dateservices', dateservices);

    function dateservices($q) {
        return {
            isLeapYear: isLeapYear,
            isFutureDate: isFutureDate,
            isWithinTaxYear: isWithinTaxYear,
            disabilityDateNotGreaterThanTaxYear: disabilityDateNotGreaterThanTaxYear,
            isModerateSevere_Date_Check: isModerateSevere_Date_Check,
            isStartLessThanEndDate: isStartLessThanEndDate,
            isPeriodWorkedLessThanAssessment: isPeriodWorkedLessThanAssessment,
            isValidDate: isValidDate,
            isValidPeriodDate: isValidPeriodDate

        }

        function startsBeforeEndDate(startDate, endDate) {
            var valid = true;

            return valid;

        }


        function isLeapYear(year) {

            var promise = $q(function (resovle, reject) {
                var year = Number(year);
                var isLeap = (year % 100 === 0) ? (year % 400 === 0) : (year % 4 === 0);
                if (!isLeap) {
                    reject(year);
                } else {
                    resovle(year);
                }
            })
            return promise;

        }





        function isFutureDate(date) {
              
            var promise = $q(function (resovle, reject) {
                var year = date.substr(0, 4);
                var month = date.substr(4, 2);
                var day = date.substr(6, 2);
    
                var currentDate = new Date();
    
                var dateObject = getDateObject(date);
                if (dateObject > currentDate) {
                    resovle(date);
                }else{
                    reject(parameter);
                }

            })
            return promise;
            
        }


        function isWithinTaxYear(yearsOfAssessment, period) {

            var promise = $q(function (resovle, reject) {
                var assessmentStartDate = new Date((Number(yearsOfAssessment) - 1), 2, 1);
                var assessmentEndDate = new Date(Number(yearsOfAssessment), 2, 1);
                var isValid = true;
                var period = getDateObject(period);
                if (period < assessmentStartDate) {
                    reject();
                }
    
                if (period > assessmentEndDate) {
                    reject();
                }
                if (isValid) {
                    resovle();
                } 
            })
            return promise;
        }

        function disabilityDateNotGreaterThanTaxYear(yearsOfAssessment, period) {

            var promise = $q(function (resovle, reject) {
                var assessmentStartDate = new Date((Number(yearsOfAssessment) - 1), 2, 1);
                var assessmentEndDate = new Date(Number(yearsOfAssessment), 2, 1);
                var isValid = true;
                var period = getDateObject(period);
                if (period > assessmentEndDate) {
                    reject();
                }else {
                    resovle();
                }
            })
            return promise;
           
        }

        function isModerateSevere_Date_Check(yearsOfAssessment, period, graceNoOfYears) {

            var promise = $q(function (resovle, reject) {

                var assessmentStartDate = new Date((Number(yearsOfAssessment) - 1), 2, 1);
                var gracePeriod = new Date((Number(yearsOfAssessment - graceNoOfYears) - 1), 2, 1)
                var assessmentEndDate = new Date(Number(yearsOfAssessment), 2, 1);
                var isValid = true;
                period = getDateObject(period);

                if (period < gracePeriod) {
                    reject();
                }
    
                if (period > assessmentEndDate) {
                    reject();
                }
                if (isValid){
                    resovle();
                }
            })
            return promise;
           
        }

        function isStartLessThanEndDate(startDate, endDate) {

            var promise = $q(function (resovle, reject) {

                if (!startDate && !endDate) {
                    reject();
                }
    
                if (startDate.length !== 8 || endDate.length !== 8) {
                    reject();
                }
    
                startDate = getDateObject(startDate);
                endDate = getDateObject(endDate);
                resovle (startDate < endDate);
            })
            return promise;
            
        }

        function isPeriodWorkedLessThanAssessment(periodsWorked, periodsAssessed) {

            var promise = $q(function (resovle, reject) {
                if (periodsWorked === "null" || periodsWorked === "" || periodsWorked === undefined || periodsWorked === "undefined") {
                    reject();
                }
    
                if (periodsAssessed === "null" || periodsAssessed === "" || periodsAssessed === undefined || periodsAssessed === "undefined") {
                    reject();
                }
    
                resovle (Number(periodsWorked) <= Number(periodsAssessed));


            })
            return promise;
        }

        function isValidPeriodDate(parameter) {
            var promise = $q(function (resovle, reject) {
                //var regEx = RegExp("/^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/");
                //var isValid = regEx.test(parameter);
                var validatePattern = /^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/;
                var isValid = parameter.match(validatePattern) //&& !function isFutureDate(parameter);

                if (!isValid) {
                    reject(parameter);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

        function isValidDate(parameter) {
            var promise = $q(function (resovle, reject) {
                //var regEx = RegExp("/^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/");
                //var isValid = regEx.test(parameter);
                var validatePattern = /^(?:(?:19\d{2}|18[89]\d|20[0-4]\d)([-\/\\.\s]?)(?:(?:0[13578]|1[02])(?:\1)31|(?:0[13456789]|1[012])(?:\1)(?:30|29)|(?:0[1-9]|1[012])(?:\1)(?:0[1-9]|1\d|2[0-8])))|(?:18(?:8[048]|9[26])|19(?:0[48]|[2468][048]|[13579][26])|20(?:0[048]|[24][048]|[13][26]))([-\/\\.\s]?)02(?:\2)29$/;
                var isValid = parameter.match(validatePattern) && !isFutureDate(parameter);

                if (!isValid) {
                    reject(parameter);
                } else {
                    resovle(parameter);
                }
            })
            return promise;
        }

    }

    function getDateObject(dateString) {
        if (dateString !== null) {
            if(dateString.length==8){
            var year = dateString.substr(0, 4);
            var month = dateString.substr(4, 2);
            var day = dateString.substr(6, 2);
            return new Date(year, (month - 1), day);
            }
            else{
                if(dateString.length==10){
                    var year = dateString.substr(0, 4);
                    var month = dateString.substr(5, 2);
                    var day = dateString.substr(8, 2);
                    return new Date(year, (month - 1), day);
                    }
                    else{

                        return new Date(1900, 1, 1, 12, 0, 0, 0);
                    }

            }
        } else {
            return new Date(1900, 1, 1, 12, 0, 0, 0);
        }
    }
})();

