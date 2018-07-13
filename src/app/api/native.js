var formDataModule = (function () {
    'use strict';

    var loadFormData = function (data, dataType, readOnly) {

        if (angular.isUndefined(data) || data === "" || data === null || data === "null") {
            return;
        }

        if (angular.isUndefined(dataType) || dataType === "" || dataType === null || dataType === "null") {
            return;
        }
        angular.element(mainForm).scope().setFormData(data, readOnly, true);


        // if (readOnly === "true") {
        //     $("#fieldSet").prop("disabled", true);

        // }
    };

    var loadForm = function (data, dataType, readOnly) {
        loadFormData(data, dataType, readOnly);
    };

    var isFormValid = function () {
        // var json = angular.element('mainForm').scope().getAngularData();
        var isValid =  mandatoryIsValid(); 
        return isValid;
    };

    var getFormData = function () {
        var dataObject = angular.element(mainForm).scope().getAngularData();

        var dataString = "";
        if (dataObject) {
       
            //dataString = JSON.stringify(dataObject);
            dataString = angular.toJson(dataObject);

        } else {
            dataString = "No data found";
        }
        return dataString;
    }


    var mandatoryIsValid = function(){

        var valid = angular.element(mainForm).scope().isFormValid();

        return valid;

     };

     var isEverythingValid= function  () {
        //var isValid = sumThreeTotalFieldsOnAllIRP5() && runValidationFringeBenefitForEPVOperatingLease() && runLocalFarmingOperationsValidation() && runLocalFarmingOperationsAllowableExpenseValidation() && runLocalFarmingOperationsCapitalImprovementsValidation() && runLocalFarmingOperationsTotalExpensesValidation() && runFarmingPartnershipValidation() && sumRetirementAnnuityContributions();
       var isValid  =  mandatoryIsValid();
        return isValid;
    }

    return {
        loadForm: loadForm,
        isFormValid: isFormValid,
        getFormData: getFormData,
        loadFormData: loadFormData,
        mandatoryIsValid: mandatoryIsValid,
        isEverythingValid:isEverythingValid
    }
})();

