(function () {
    angular.module('cbcproject').directive('requiredEmpty', function () {
        return {
            restrict: 'AE',
            require: 'ngModel',
            link: function (scope, element, attr, ctrl) {

                scope.$watch(attr['ngModel'], function (newValue) {
                    if (newValue === "") {
                        ctrl.$setValidity('required', true);
                        attr.$set("required", true);
                        element.css('box-shadow', '0 0 1.5px 2px red');
                    } else {
                        if (newValue != '' && newValue != undefined) {
                            ctrl.$setValidity('required', false);
                            attr.$set("required", false);
                            element.css('box-shadow', '0 0 1.5px 2px white');
                        }
                    }
                })
            }
        };
    });
})();