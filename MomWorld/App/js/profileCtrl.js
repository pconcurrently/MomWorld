'use strict';

var profileApp = angular.module('profileApp', ['angular-md5', 'firebase']);

profileApp.controller('profileCtrl', ['$scope', '$http', 'md5', '$firebaseObject', '$firebaseArray', function ($scope, $http, md5, $firebaseObject, $firebaseArray) {

    var userStatus = new Firebase("https://momworld.firebaseio.com/Status/user1");
    $scope.statusFirebase = $firebaseArray(userStatus);

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
       //

        var sentData = {
            FirstName: $scope.user.FirstName,
            LastName: $scope.user.LastName,
            PhoneNumber: $scope.user.PhoneNumber,

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

    /* Retrive all Status from API */
    $scope.loadStatus = function () {

    }

    /* Create new Status using Status API */
    $scope.createStatus = function () {
        
        var sentData = {
                Content: $scope.Content,
                CreatorName: $scope.user.LastName + " " + $scope.user.FirstName,
                CreatorUsername: "user1",
                CreatorAvatar: "http://meobeoi.com/wp-content/uploads/2014/06/hoanhtrang.jpg",
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
            CreatorUsername: "user1",
            CreatorAvatar: "http://meobeoi.com/wp-content/uploads/2014/06/hoanhtrang.jpg",
            createdDate: Firebase.ServerValue.TIMESTAMP
        }
        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/user1/" + _status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        commentFirebase.$add(sentData).then(
             $scope.commentContent = ""
        );

    }

    /* Delete Selected Commment*/
    $scope.removeComment = function (_user, _status) {

        var commentStatus = new Firebase("https://momworld.firebaseio.com/Status/user1/" + _status.$id + "/Comment");
        var commentFirebase = $firebaseArray(commentStatus);

        commentFirebase.$remove(_status).then(
             
        );

    }







}]);
