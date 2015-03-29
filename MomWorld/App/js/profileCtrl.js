'use strict';

var profileApp = angular.module('profileApp', ['angular-md5']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', function ($scope, $http, md5) {

    /* Get Prfile request URL */
    var gravatar = 'http://www.gravatar.com/' + md5.createHash("trungnm0512@gmail.com" || '') + '.json';

    $scope.user = {
    }

    $scope.statuses = [
        {
            CreatorName: "TrungNM", Content: "YOloo 111",
            Comment: [{}]
        },
        { CreatorName: "PhoHT", Content: "YOloo 222" },

    ]

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


        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName
        }

        console.log(JSON.stringify(sentData));

        $http.put("http://localhost:4444/api/User/user1", sentData).
              success(function (data, status, headers, config) {
                  alert("Update Ok");
                  console.log(JSON.stringify(data));
              }).
              error(function (data, status, headers, config) {
                  alert("Update CL");
              });

    }







}]);
