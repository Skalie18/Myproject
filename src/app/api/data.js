var  formTestApi = (function(){
    'use strict';
    function getLocalFormData() {
        var data = formDataModule.getFormData();
        angular.element(results).val(data);
    }
    
    function setAngularFormData() {
        var data = angular.element(request).val();
        formDataModule.loadFormData(data, "xml", false);
    }
    
    function setFormData(data, datatype, readyOnly) {
        sessionStorage.clear();
        if (datatype === "json") {
           formDataModule.loadFormData(data, 'json', false);  
        } 
        
        else {
            if (datatype === "xml") {
                formDataModule.loadFormData(data, 'xml', false);
            }
        }
    }

    return{
        setFormDataTest:setFormData,
        setAngularFormData:setAngularFormData,
        getLocalFormData:getLocalFormData
    }
    
})();
