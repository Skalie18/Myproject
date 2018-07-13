(function(){
    
        angular.module('cbcproject').directive('sarsMinlength', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModelCtrl) {
                  var minlength = Number(attrs.sarsMinlength);
                  function fromUser(text) {
                      if (text.length < minlength) {
                        var transformedInput = text.substring(0, minlength);
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                        return transformedInput;
                      } 
                      return text;
                  }
                  ngModelCtrl.$parsers.push(fromUser);
                }
              }; 
        });
    
    })();