'use strict';

var videoApp = angular.module('videoApp', []);

videoApp.controller('videoCtrl', ['$scope', '$http',
function ($scope, $http) {

    //var userStatus = new Firebase("https://momworld.firebaseio.com/Status/user1");
    //$scope.statusFirebase = $firebaseArray(userStatus);

    //$scope.user = {
    //}

    //var uploader = $scope.uploader = new FileUploader({
    //    url: "http://localhost:4444/api/Video/UploadVideo",
    //    formData: [{ Username: "user1" }]
    //});

    //uploader.onSuccessItem = function (fileItem, response, status, headers) {
    //    console.info('onSuccessItem', fileItem, response, status, headers);
    //    $window.location.reload(); 

    //};

    //uploader.onCompleteAll = function () {
    //    console.info('onCompleteAll');
    //};

    //uploader.onErrorItem = function (fileItem, response, status, headers) {
    //    console.info('onErrorItem', fileItem, response, status, headers);
    //};


    ///* Update Profile  */
    //$scope.updateAvatar = function () {

    //    var sentData = {
    //        FirstName: $scope.user.FirstName,
    //        LastName: $scope.user.LastName,
    //        PhoneNumber: $scope.user.PhoneNumber,

    //    }

    //    $scope.uploader.uploadAll(function () {

    //    });

    //}

 

}]);