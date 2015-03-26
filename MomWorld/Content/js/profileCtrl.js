'use strict';

var profileApp = angular.module('profileApp', ['angular-md5']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', function ($scope, $http, md5) {
    
    /* Get Prfile request URL */
    var gravatar = 'http://www.gravatar.com/' + md5.createHash("trungnm0512@gmail.com" || '') + '.json';

    $scope.user = {
    }

    $scope.loadProfile = function () {
        $http.get(gravatar).
              success(function (data, status, headers, config) {
                  $scope.user = data.entry[0];
                    
                  /* Get More Detail about User profile */
                  $scope.user.email = "trungnm0512@gmail.com";
                  $scope.user.phone = "+(84)1655757445";

              }).
              error(function (data, status, headers, config) {
                  
              });
    }


    /* Update Profile  */
    $scope.updateProfile = function () {

        $http.post("http://localhost:4444/Account/Manage", { msg: 'hello word!' }).
              success(function (data, status, headers, config) {
                  $scope.user = data.entry[0];

                  /* Get More Detail about User profile */
                  $scope.user.email = "trungnm0512@gmail.com";
                  $scope.user.phone = "+(84)1655757445";

              }).
              error(function (data, status, headers, config) {

              });

    }






  
}]);
    