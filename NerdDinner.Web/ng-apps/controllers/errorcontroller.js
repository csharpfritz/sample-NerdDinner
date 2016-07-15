(function () {
  'use strict';

  angular
      .module('nerdDinner')
      .controller('errorController', errorController);

  errorController.$inject = ['$scope', '$location', 'errorSvc'];

  function errorController($scope, $location, errorService) {

    var vm = this;

    vm.initialize = function () {

      $scope.errors = errorService.all.query();

    }

    vm.initialize();


  }



})();
