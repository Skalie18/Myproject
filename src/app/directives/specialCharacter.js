(function () {
    angular.module('cbcproject').directive('specialCharacter', function () {

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function(viewValue) {

            
                   // var reg = /^[^`~!@#$%\^&*()_+={}|[]]*$/;
                    var reg = /^[^|¦ĀāĂăĄąĆćĈĉĊēĔĖėĘęĚĜĜħħĨĩĪīĬĭĮįİİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńņŇňŉŊŋōŎŏŐőŔŖŗŘŚśŜŝŞşŠŢţŤťŦŧŨŪūŬŭŮůŰŲųŴŵŶŷŹźŻżŽſƀƁƂƃƄƅƇƈƉƊƋƌƎƏƐƑƒƓƕƖƗƥƦƺƼƽƾƿǀǁǃǄǅǆǇǉǊǋǌǍǎǏǔǕǖǗǘǙǛǜǝǞǟǠǡǢǤǥǦǧǨǩǪǫǬǭǮǰǱǲǳǴɘəɚɜɮɯɰɲ]*$/;
                    //  var reg = /^[|¦ĀāĂăĄąĆćĈĉĊēĔĖėĘęĚĜĜħħĨĩĪīĬĭĮįİİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńņŇňŉŊŋōŎŏŐőŔŖŗŘŚśŜŝŞşŠŢţŤťŦŧŨŪūŬŭŮůŰŲųŴŵŶŷŹźŻżŽſƀƁƂƃƄƅƇƈƉƊƋƌƎƏƐƑƒƓƕƖƗƥƦƺƼƽƾƿǀǁǃǄǅǆǇǉǊǋǌǍǎǏǔǕǖǗǘǙǛǜǝǞǟǠǡǢǤǥǦǧǨǩǪǫǬǭǮǰǱǲǳǴɘəɚɜɮɯɰɲ]*$/;
                    // if view values matches regexp, update model value
                    if (viewValue.match(reg)) {
                      return viewValue;
                    }
                    // keep the model value as it is
                    var transformedValue = ngModel.$modelValue;
                    ngModel.$setViewValue(transformedValue);
                    ngModel.$render();
                    return transformedValue;
                  });
            }
        };

    })

})();