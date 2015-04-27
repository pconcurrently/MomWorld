'use strict';

var profileApp = angular.module('profileApp', ['firebase', 'angularFileUpload', 'infinite-scroll', 'angularMoment']);

profileApp.controller('profileCtrl', ['$scope', '$firebase', '$http', '$firebaseObject', '$firebaseArray', 'FileUploader', '$window', '$scrollArray','amMoment', 
     function ($scope, $firebase, $http, $firebaseObject, $firebaseArray, FileUploader, $window, $scrollArray, amMoment) {
    amMoment.changeLocale('vn');
    // Get User from Local Storage
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        $scope.currentUsername = $scope.currentUser.Username;

        var userStatus = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername);
        /*  $scope.statusFirebase = $firebaseArray(userStatus); */
        var scrollRef = new Firebase.util.Scroll(userStatus, 'createdDate');
        $scope.statusFirebase = $firebaseArray(scrollRef);
        $scope.statusFirebase.scroll = scrollRef.scroll;
        $scope.user = {};

        $scope.statusFirebase.scroll.next(3);

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
            Facebook: $scope.user.Facebook || "",
            Google: $scope.user.Google || ""

        }

        $http.put("http://localhost:4444/api/User/Put/" + $scope.currentUsername, sentData).
              success(function (data, status, headers, config) {
                  // TODO: UPdate Css
                  $('#modalSuccess').modal('show');
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
            CreatedDate: Firebase.ServerValue.TIMESTAMP,
            NumLike: {
                Count: 0,
                User: []
            },
            NumComment: 0
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
   
        // Increase Number of Comment
        var p = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername + "/" + _status.$id );        
        var NumComment = p.child('NumComment');
        NumComment.once('value', function (snapshot) {
            NumComment.set(snapshot.val() + 1);
        });


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

    // ------- Social Function
    $scope.like = function (_status) {
        var p = new Firebase("https://momworld.firebaseio.com/Status/" + $scope.currentUsername + "/" + _status.$id + "/NumLike");
        //var NumLike = p.child('NumComment');
        //NumComment.once('value', function (snapshot) {
        //    NumComment.set(snapshot.val() + 1);
        //});
        var numLike = $firebaseObject(p);
        numLike.$loaded(function (data) {
            data.Count++;
            data.$save().then();
        },
            function (err) { });

    }

    $scope.getBadge = function () {
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/");
        $scope.badgeFire = $firebaseArray(userFireTmp);
    }


    /* ------------- Helper Methods ---------------------- */
    $scope.loadMore = function () {

    }

    /* ------------- Panel Methods ---------------------- */
    var o = new Firebase("https://momworld.firebaseio.com/UserList/");
    $scope.listUser = $firebaseArray(o);
    
}]);

profileApp.factory('$scrollArray', function ($firebaseArray) {
    return function (ref, field) {
        // create a special scroll ref
        var scrollRef = new Firebase.util.Scroll(ref, field);
        // generate a synchronized array with the ref
        var list = $firebaseArray(scrollRef);
        // store the scroll namespace on the array for easy ref
        list.scroll = scrollRef.scroll;

        return list;
    }
});

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
}])
// just some scroll magic to show the bottom of the list as new records are loaded

.directive('scrollToBottom', function () {
    var unbind;
    return {
        restrict: 'A',
        scope: { scrollToBottom: "=" },
        link: function (scope, element) {
            if (unbind) { unbind(); }
            unbind = scope.$watchCollection('scrollToBottom', function () {
                // assumes we have jQuery, which handles the pretty animation
                // otherwise, just use this code instead:
                // element[0].scrollTop = element[0].scrollHeight;
                $(element).animate({ scrollTop: element[0].scrollHeight });
            });
        }
    }
});


profileApp.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});



