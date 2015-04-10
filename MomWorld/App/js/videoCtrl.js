'use strict';

var videoApp = angular.module('videoApp', ['angularFileUpload']);

videoApp.controller('videoCtrl', ['$scope', '$http', 'FileUploader', '$window',
function ($scope, $http, FileUploader, $window) {

    /* */

    $scope.user = {
    }

    var uploader = $scope.uploader = new FileUploader({
        url: "http://localhost:4444/api/User/UploadVideo",
        formData: [{ Username: "user1" }]
    });

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
        console.info('onSuccessItem', fileItem, response, status, headers);



        $window.location.reload();

    };

    uploader.onCompleteAll = function () {
        console.info('onCompleteAll');
    };

    uploader.onErrorItem = function (fileItem, response, status, headers) {
        console.info('onErrorItem', fileItem, response, status, headers);
    };


    /* Update Profile  */
    $scope.uploadVideo = function () {

        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

        }

        $scope.uploader.uploadAll(function () {

        });

    }



}]);