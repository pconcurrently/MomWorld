/* 
* TrungNM - View other Profile Controller
* 
*/

'use strict';

var otherProfileApp = angular.module('otherProfileApp', ['firebase', 'angularMoment']);

otherProfileApp.controller('otherProfileCtrl', ['$scope', '$http', '$firebaseObject', '$firebaseArray', '$window', 'amMoment',
function ($scope, $http, $firebaseObject, $firebaseArray, $window, amMoment) {

    amMoment.changeLocale('vn');
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

                  // Load Badge
                  $scope.getBadge();
              }).
              error(function (data, status, headers, config) {

              });
    }

    /* ---------------- Functional Method ----------------------- */

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

    // ------- Social Function
    $scope.like = function (_status) {
        var p = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.viewUsername + "/" + _status.$id + "/NumLike");
        var numLike = $firebaseObject(p);

        numLike.$loaded(function (data) {
            console.log("Data:  " + data.User);
            data.Count++;
            if (!data.User) {
                data.User = [];
                data.User.push($scope.currentUsername);
            } else {
                if (!$scope.isInArray($scope.currentUsername, data.User)) {
                    data.User.push($scope.currentUsername);
                }
            }

            data.$save().then();
        },
            function (err) { });

    }

    // Load Badge From Firebase
    $scope.getBadge = function () {
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.viewUsername + "/Badge/");
        $scope.badgeFire = $firebaseArray(userFireTmp);
    }

    /* ---------------- Helper Method ------------------ */
    $scope.isInArray = function (value, array) {
        if (array) {
            return array.indexOf(value) > -1;
        } else {
            return false;
        }

    }

}]);
