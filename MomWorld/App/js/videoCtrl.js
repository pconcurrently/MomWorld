'use strict';

var videoApp = angular.module('videoApp2',
    [
        'firebase',
        "ngSanitize",
		"com.2fdevs.videogular",
		"com.2fdevs.videogular.plugins.controls",
		"com.2fdevs.videogular.plugins.poster"
    ]);

videoApp.controller('videoCtrl2', ['$scope', '$http',  '$window','$firebaseArray', '$location','$sce',
function ($scope, $http, $window, $firebaseArray, $location, $sce) {

    // Get User from Local Storage
    $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    $scope.currentUsername = $scope.currentUser.Username;

    var url = $location.absUrl().substring(35, $location.absUrl().length);

    $scope.videoURL = "http://localhost:4444/App/uploads/video/" + url + ".mp4";

    var tmp = new Firebase("https://momworld.firebaseio.com/Video");
    $scope.videoFire = $firebaseArray(tmp);

    $http.get("http://localhost:4444/api/User/Get/" + $scope.currentUsername).
        success(function (data, status, headers, config) {
            $scope.user = data;
        }).
        error(function (data, status, headers, config) {

        });

    $scope.initVideoDetail = function (tmp) {
        $scope.videoID = tmp;
    }

    this.config = {
        sources: [
            { src: $sce.trustAsResourceUrl($scope.videoURL), type: "video/mp4" }
        ],
        tracks: [
            {
                src: "http://www.videogular.com/assets/subs/pale-blue-dot.vtt",
                kind: "subtitles",
                srclang: "en",
                label: "English",
                default: ""
            }
        ],
        theme: "http://localhost:4444/App/bower_components/videogular-themes-default/videogular.css",
        plugins: {
            poster: "http://localhost:4444/App/uploads/video/thumbnail/" + url + ".png"
        }
    };
   


}]);

videoApp.directive('dynamicUrl', function () {
    return {
        restrict: 'A',
        link: function postLink(scope, element, attr) {
            element.attr('src', attr.dynamicUrlSrc);
        }
    };
});