'use strict';

var panelApp = angular.module('panelApp', ['firebase']);

panelApp.controller('panelCtrl', ['$scope', '$http', '$firebaseObject', '$firebaseArray', '$window',
function ($scope, $http, $firebaseObject, $firebaseArray, $window) {
    // Get User from Local Storage
    $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    $scope.currentUsername = $scope.currentUser.Username;
    alert("sss");

    var userStatus = new Firebase("https://momworld.firebaseio.com/User/");
    $scope.userFire = $firebaseArray(userStatus);

    $scope.yolo = "sdsds";

}]);
