
(function () {
    'use strict';

    var isOnGitHub = window.location.hostname === 'blueimp.github.io',
        url = isOnGitHub ? '//jquery-file-upload.appspot.com/' : 'http://localhost:4444/api/User/UploadVideo';

    angular.module('demo', [
        'blueimp.fileupload',
        'angularUUID2',
        'firebase',
        "com.2fdevs.videogular",
		"com.2fdevs.videogular.plugins.controls",
		"com.2fdevs.videogular.plugins.poster"
    ])
        .config([
            '$httpProvider', 'fileUploadProvider',
            function ($httpProvider, fileUploadProvider) {
                delete $httpProvider.defaults.headers.common['X-Requested-With'];
                fileUploadProvider.defaults.redirect = window.location.href.replace(
                    /\/[^\/]*$/,
                    '/cors/result.html?%s'
                );

            }
        ])

        .controller('DemoFileUploadController', [
            '$scope', '$http', '$filter', '$window', 'uuid2', '$firebaseArray', '$firebaseObject', '$sce', '$timeout', 
            function ($scope, $http, $filter, $window, uuid2, $firebaseArray, $firebaseObject, $sce, $timeout) {

                // Get User from Local Storage
                $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
                $scope.currentUsername = $scope.currentUser.Username;

                // ------------- Video Loadmore
                var tmp = new Firebase("https://momworld.firebaseio.com/Video");

                var baseRef = new Firebase('https://fbutil.firebaseio.com/paginate');
                var scrollRef = new Firebase.util.Scroll(tmp, 'number');

                $scope.videoFire = $firebaseArray(scrollRef);
                $scope.videoFire.scroll = scrollRef.scroll;

                $scope.videoFire.scroll.next(3);
                $scope.nVideo = "";
                // ------- Video Upload

                $scope.options = {
                    url: url,
                };

                $scope.uuid = uuid2.newuuid();

                $scope.uploadToSever = function (videoName) {
                    $scope.nVideo = videoName;
                }

                $scope.$on('fileuploadsubmit', function (event, files) {
                    $.each(files.files, function (index, file) {
                        files.formData = {
                            Username: $scope.currentUsername,
                            VideoName: $scope.nVideo,
                            VideoID: $scope.uuid
                        };
                    });


                    // Add File URL to Firebase
                    if (files.files[0].type == "video/mp4") {
                        var a = new Firebase("https://momworld.firebaseio.com/Video/" + $scope.uuid);
                        var v = $firebaseObject(a);
                        v.Username = $scope.currentUsername;
                        v.VideoName = $scope.nVideo;
                        v.VideoID = $scope.uuid;
                        v.VideoURL = "http://localhost:4444/App/uploads/video/" + $scope.uuid + ".mp4";
                        v.VideoThumbnail = "http://localhost:4444/App/uploads/video/thumbnail/" + $scope.uuid + ".png";

                        v.$save().then(function () {
                            $scope.files = "";
                        }, function (err) {

                        });
                    }
                });

                // ------------- Video Playper

                var controller = this;
                controller.state = null;
                controller.API = null;
                controller.currentVideo = 0;

                controller.onPlayerReady = function (API) {
                    controller.API = API;
                };

                controller.onCompleteVideo = function () {
                    controller.isCompleted = true;

                    controller.currentVideo++;

                    if (controller.currentVideo >= controller.videos.length) controller.currentVideo = 0;

                    controller.setVideo(controller.currentVideo);
                };

                controller.videos = [
                {
                    sources: [
                        { src: $sce.trustAsResourceUrl("http://localhost:4444/App/uploads/video/" + $scope.tmpVideo + ".mp4"), type: "video/mp4" },
                    ]
                }
                ];

                controller.config = {
                    preload: "none",
                    autoHide: false,
                    autoHideTime: 3000,
                    autoPlay: false,
                    sources: controller.videos[0].sources,
                    theme: {
                        url: "http://www.videogular.com/styles/themes/default/latest/videogular.css"
                    },
                    plugins: {
                        poster: "http://www.videogular.com/assets/images/videogular.png"
                    }
                };

                controller.setVideo = function (index) {
                    $scope.tmpVideo = index;
                    controller.API.stop();
                    controller.currentVideo = index;
                    controller.config.sources = { src: $sce.trustAsResourceUrl("http://localhost:4444/App/uploads/video/" + index + ".mp4"), type: "video/mp4" };
                   // controller.config.sources = controller.videos[index].sources;
                    
                    $timeout(controller.API.play.bind(controller.API), 100);
                };



            }])

        .controller('FileDestroyController', [
            '$scope', '$http',
            function ($scope, $http) {
                var file = $scope.file,
                    state;
                if (file.url) {
                    file.$state = function () {
                        return state;
                    };
                    file.$destroy = function () {
                        state = 'pending';
                        return $http({
                            url: file.deleteUrl,
                            method: file.deleteType
                        }).then(
                            function () {
                                state = 'resolved';
                                $scope.clear(file);
                            },
                            function () {
                                state = 'rejected';
                            }
                        );
                    };
                } else if (!file.$cancel && !file._index) {
                    file.$cancel = function () {
                        $scope.clear(file);
                    };
                }
            }
        ]);

}());
