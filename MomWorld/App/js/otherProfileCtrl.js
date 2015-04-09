/* 
* TrungNM - View other Profile Controller
* 
*/

'use strict';

var otherProfileApp = angular.module('otherProfileApp', ['angular-md5', 'firebase', 'angularFileUpload']);

otherProfileApp.controller('otherProfileCtrl', ['$scope', '$http', 'md5', '$firebaseObject', '$firebaseArray', 'FileUploader', '$window',
function ($scope, $http, md5, $firebaseObject, $firebaseArray, FileUploader, $window) {

    // Get User from Local Storage
    $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    $scope.currentUsername = $scope.currentUser.Username;
    $scope.viewUsername = "";
    $scope.user = {};


    /* ---------------- Initial Functions ----------------------- */
    // Load Profile
    $scope.loadProfile = function (username) {

        // Set viewUsername Scope   
        $scope.viewUsername = username;

        // Get user Status from Firebase
        var userStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.viewUsername);
        $scope.statusFirebase = $firebaseArray(userStatus);

        // Get user Profile from User API
        $http.get("http://localhost:4444/api/User/Get/" + username).
              success(function (data, status, headers, config) {
                  $scope.user = data;
              }).
              error(function (data, status, headers, config) {

              });
    }

    // Load Badge From Firebase
    $scope.getBadge = function () {
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.viewUsername + "/Badge/");
        $scope.badgeFire = $firebaseArray(userFireTmp);
    }


    /* ---------------- Functional Method ----------------------- */

    /* Update Profile  */
    $scope.updateProfile = function () {

        // Prepare Data for Update
        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

        }

        // Update user profile using User API
        $http.put("http://localhost:4444/api/User/Put/" + $scope.currentUsername, sentData).
              success(function (data, status, headers, config) {
                  alert("Update Ok");
                  console.log(JSON.stringify(data));
              }).
              error(function (data, status, headers, config) {
                  alert("Update Fail");
              });

    }

    

    /* Create new Status using Firebase API */
    $scope.createStatus = function () {

        //  Prepare data
        var sentData = {
            Content: $scope.Content,
            CreatorName: $scope.currentUser.FirstName + " " + $scope.currentUser.LastName,
            CreatorUsername: $scope.currentUsername,
            CreatorAvatar: "http://localhost:4444/App/uploads/avatar/" + $scope.currentUsername + ".png",
            createdDate: Firebase.ServerValue.TIMESTAMP
        }

        // Add new Status to Firebase
        $scope.statusFirebase.$add(sentData).then(
             $scope.Content = ""
        );

    }

    /* Remove Selected Status using Status API */
    $scope.removeStatus = function (_status) {

        $scope.statusFirebase.$remove(_status).then(

        );

    }


    /* Create new Commment using Status API */
    $scope.addComment = function (_status, _comment) {

        // Prepare data
        var sentData = {
            Content: _comment,
            CreatorName: $scope.currentUser.FirstName + " " + $scope.currentUser.LastName,
            CreatorUsername: $scope.currentUsername,
            CreatorAvatar: "http://localhost:4444/App/uploads/avatar/" + $scope.currentUsername + ".png",
            createdDate: Firebase.ServerValue.TIMESTAMP
        }

        // Get Status's comments from Firebase API 
        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.viewUsername + "/" +_status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        // Add comment to Firebase
        commentFirebase.$add(sentData).then(
             $scope.commentContent = ""
        );

    }

    /* Delete Selected Commment*/
    $scope.removeComment = function (_user, _status) {
        
        // Get Comment from Firebase
        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.viewUsername + "/" + _status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        // Remove comment using Firebase
        commentFirebase.$remove(_status).then(

        );

    }

    

    /* ------------------- Upload File Helper Methods ------------------------------*/
    var uploader = $scope.uploader = new FileUploader({
        url: "http://localhost:4444/api/User/UploadAvatar",
        formData: [{ Username: $scope.currentUsername }]
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
    $scope.updateAvatar = function () {
        // Prepare data
        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

        }

        // Update avatar link using Firebase
        $scope.uploader.uploadAll(function () {

        });

    }

}]);

/* ----------- Directives ---------------------*/
otherProfileApp.directive('ngThumb', ['$window', function ($window) {
    var helper = {
        support: !!($window.FileReader && $window.CanvasRenderingContext2D),
        isFile: function (item) {
            return angular.isObject(item) && item instanceof $window.File;
        },
        isImage: function (file) {
            var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    };

    return {
        restrict: 'A',
        template: '<canvas/>',
        link: function (scope, element, attributes) {
            if (!helper.support) return;

            var params = scope.$eval(attributes.ngThumb);

            if (!helper.isFile(params.file)) return;
            if (!helper.isImage(params.file)) return;

            var canvas = element.find('canvas');
            var reader = new FileReader();

            reader.onload = onLoadFile;
            reader.readAsDataURL(params.file);

            function onLoadFile(event) {
                var img = new Image();
                img.onload = onLoadImage;
                img.src = event.target.result;
            }

            function onLoadImage() {
                var width = params.width || this.width / this.height * params.height;
                var height = params.height || this.height / this.width * params.width;
                canvas.attr({ width: width, height: height });
                canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
            }
        }
    };
}]);