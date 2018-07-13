(function() {
 // 'use strict';

  angular
    .module('cbcproject')
    .run(runBlock);

  function runBlock($log) {

    $log.debug('runBlock end');
  }

})();
