(function () {
    angular.module('cbcproject').directive('cbcMoneyFieldValidator', function () {
        return {
//cbc-money-field-validator
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                scope.$watch(attr['ngModel'], function (newVal, oldVal) {

                    pattern=/^-?[0-9]*$/g;

                    if(element.val().match(pattern) != null){
                       
                    }
                    else
                  {
                    element.val() === "";
                   ngModel.$setViewValue(undefined);
                   
                        //ngModel.$processModelValue();
                        ngModel.$render();
                         scope.ngModel = undefined;
                    }
                  
                });
            }
        }
    })
})();
