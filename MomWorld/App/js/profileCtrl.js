'use strict';

var profileApp = angular.module('profileApp', ['angular-md5']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', function ($scope, $http, md5) {
    
    /* Get Prfile request URL */
    var gravatar = 'http://www.gravatar.com/' + md5.createHash("trungnm0512@gmail.com" || '') + '.json';

    $scope.user = {
    }

    $scope.loadProfile = function () {
        /* Get user Profile from User API */
        $http.get("http://localhost:4444/api/User/user1").
              success(function (data, status, headers, config) {
                  $scope.user = data;
              }).
              error(function (data, status, headers, config) {
                  
              });
    }


    /* Update Profile  */
    $scope.updateProfile = function () {
        
        //$http.post("http://postcatcher.in/catchers/55179cecdcfd5a0300000a69", { firstName: "Nam", lastName: "Thai Hoang" }).
        //$http.post("http://localhost:4444/api/User/user1", { firstName: "Nam", lastName: "Thai Hoang" }).
        var data = $scope.user;
        console.log(JSON.stringify(data));
        $http.put("http://localhost:4444/api/User/user1", data).
              success(function (data, status, headers, config) {
                  alert("Update Ok");
                  console.log(JSON.stringify(data));
              }).
              error(function (data, status, headers, config) {
                  alert("Update CL");
              });

    }






  
}]);
    