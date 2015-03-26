'use strict';

var profileApp = angular.module('profileApp', ['angular-md5']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', function ($scope, $http, md5) {
    
    /* Get Prfile request URL */
    var gravatar = 'http://www.gravatar.com/' + md5.createHash("trungnm0512@gmail.com" || '') + '.json';

    $scope.user = {
    }

    $scope.loadProfile = function () {
        $http.get("http://localhost:4444/api/User/user1").
              success(function (data, status, headers, config) {
                  $scope.user = data;
              }).
              error(function (data, status, headers, config) {
                  
              });
    }


    /* Update Profile  */
    $scope.updateProfile = function () {

        $http.put("http://localhost:4444/api/User/user1", { firstName: "Nam", lastName: "Thai Hoang" }).
              success(function (data, status, headers, config) {
                  alert("Update Ok");
              }).
              error(function (data, status, headers, config) {

              });

    }






  
}]);
    