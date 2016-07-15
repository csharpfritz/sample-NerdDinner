(function () {
  'use strict';

  angular
      .module('nerdDinner')
      .factory('errorSvc', errorService)

  errorService.$inject = ['$resource'];

  function errorService($resource) {
    return {
      all: $resource('/api/errorlog'),
    };
  }

})();