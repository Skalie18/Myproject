(function(){
    angular.module('cbcproject').directive('modelDataGetter', function () {
        return {
            require: "ngModel",
          
            link:  function(scope, element, attrs, ngModelCtrl)
            {
                var getExpression = attrs.modelDataGetter;
        
                function updateViewValue(newValue, oldValue)
                {
                    if (newValue != ngModelCtrl.$viewValue)
                    {
                        ngModelCtrl.$setViewValue(newValue);
                        ngModelCtrl.$render();
                    }
                    
                    var updateExpression = attrs.ngModel + "=" + getExpression;
                    scope.$eval(updateExpression);
                }
                                    
                updateViewValue();    
                
                scope.$watch(getExpression, updateViewValue);
            }
        };

    })

}());