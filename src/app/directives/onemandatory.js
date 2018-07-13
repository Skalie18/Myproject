(function(){
    angular.module('cbcproject').directive('oneMandatory', function () {


        return{
            require: "ngModel",

            link: function(scope, element, attrs, ngModelCtrl){

                angular.array.forEach(element,function(el) {

                    if(el.value != ""){


                    }
                    
                });
            }
        }
    })

}());