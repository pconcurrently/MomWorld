'use strict';

var videoApp = angular.module('videoApp2', ['firebase']);

videoApp.controller('videoCtrl2', ['$scope', '$http',  '$window','$firebaseArray',
function ($scope, $http, $window, $firebaseArray) {

    // Get User from Local Storage
    $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    $scope.currentUsername = $scope.currentUser.Username;

    alert("2 " + $scope.videoID2);

    var tmp = new Firebase("https://momworld.firebaseio.com/Video");
    $scope.videoFire = $firebaseArray(tmp);

    $http.get("http://localhost:4444/api/User/Get/" + $scope.currentUsername).
        success(function (data, status, headers, config) {
            $scope.user = data;
        }).
        error(function (data, status, headers, config) {

        });

    $scope.initVideoDetail = function (tmp) {
        alert("sss");
        $scope.videoID = tmp;
        alert("22" +  $scope.videoID);
    }
   



}]);