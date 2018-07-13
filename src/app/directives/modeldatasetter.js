(function(){
    angular.module('cbcproject').directive('modelDataSetter', function () {
        return {
            require: "ngModel",
            
            link:  function(scope, element, attrs, ngModelCtrl){
                var setExpression = attrs.modelDataSetter;
                function updateModelValue(e)
                {
                    scope.$value = ngModelCtrl.$viewValue;
                    scope.$eval(setExpression);
                    delete scope.$value;
                }                 
                scope.$watch(attrs.ngModel, updateModelValue);
            }
        };

    })

}());