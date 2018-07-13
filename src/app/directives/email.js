(function () {
    angular.module('cbcproject').directive('emailvalidator', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, control) {
                control.$parsers.push(function (viewValue) {
                    var newEmail = control.$viewValue;
                    control.$setValidity("invalidEmail", true);
                    if ( angular.isObject(newEmail)|| newEmail == "") return newEmail;  // pass through if we clicked date from popup  typeof newEmail === "object" 
                    if (!newEmail.match(/^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$/))
                        control.$setValidity("invalidEmail", false);
                    return viewValue;
                });
            }
        };
    });
})();