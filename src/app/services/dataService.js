(function(){
angular.module('cbcproject').factory('dataService',function(){

    return{
        getData: function(formData){
          if (formData) {
            (function filter(obj) {
                angular.forEach(obj, function ( value ,key) {
                    if (value === "" || value === 'null' || value === null) {
                        delete obj[key];
                    } else if (angular.isObject(value)) {
                        filter(value);
                    } else if (angular.isArray(value)) {
                        angular.forEach(value, function (v, k) { filter(v); });
                    }
                });
            })(formData);
            return formData;
        }
        return "";
        }
    }
});
})();