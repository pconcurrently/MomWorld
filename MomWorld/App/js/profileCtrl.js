'use strict';

var profileApp = angular.module('profileApp', ['angular-md5', 'firebase', 'angularFileUpload']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', '$firebaseObject', '$firebaseArray', 'FileUploader', '$window',
function ($scope, $http, md5, $firebaseObject, $firebaseArray, FileUploader, $window) {
    // Get User from Local Storage
    $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    $scope.currentUsername = $scope.currentUser.Username;

    var userStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername);
    $scope.statusFirebase = $firebaseArray(userStatus);

    $scope.user = {
    }



    $scope.loadProfile = function () {
        /* Get user Profile from User API */
        $http.get("http://localhost:4444/api/User/Get/" + $scope.currentUsername).
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
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

        }

        $http.put("http://localhost:4444/api/User/Put/" + $scope.currentUsername, sentData).
              success(function (data, status, headers, config) {
                  alert("Update Ok");
                  console.log(JSON.stringify(data));
              }).
              error(function (data, status, headers, config) {
                  alert("Update Fail");
              });

    }

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

        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

        }

        $scope.uploader.uploadAll(function () {

        });

    }

    /* Retrive all Status from API */
    $scope.loadStatus = function () {

    }

    /* Create new Status using Status API */
    $scope.createStatus = function () {

        var sentData = {
            Content: $scope.Content,
            CreatorName: $scope.user.FirstName + " " + $scope.user.LastName,
            CreatorUsername: $scope.currentUsername,
            CreatorAvatar: "http://localhost:4444/App/uploads/avatar/" + $scope.currentUsername + ".png",
            createdDate: Firebase.ServerValue.TIMESTAMP
        }

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
    $scope.addComment = function (_user, _status, _comment) {

        var sentData = {
            Content: _comment,
            CreatorName: _user.FirstName + " " + _user.LastName,
            CreatorUsername: $scope.currentUsername,
            CreatorAvatar: "http://localhost:4444/App/uploads/avatar/" + $scope.currentUsername + ".png",
            createdDate: Firebase.ServerValue.TIMESTAMP
        }
        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername + "/" + _status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        commentFirebase.$add(sentData).then(
             $scope.commentContent = ""
        );

    }

    /* Delete Selected Commment*/
    $scope.removeComment = function (_user, _status) {

        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername + "/" + _status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        commentFirebase.$remove(_status).then(

        );

    }

    $scope.getBadge = function () {
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/");
        $scope.badgeFire = $firebaseArray(userFireTmp);
    }

}]);

profileApp.directive('ngThumb', ['$window', function ($window) {
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

profileApp.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});