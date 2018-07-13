(function () {
    'use strict';

    angular.module('cbcproject').directive('runOnLoad', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attributes) {
                scope.$watch(attributes.ngModel, function (newVal, oldVal) {
                   
                    if (newVal !== "" && newVal !== undefined) {
                        element.triggerHandler("blur");
                    }
                        //console.log("value -> " + newVal);
                     
                });
            }
        };
    });

})();
