
(function () {
    'use strict';

    var isOnGitHub = window.location.hostname === 'blueimp.github.io',
        url = isOnGitHub ? '//jquery-file-upload.appspot.com/' : 'http://localhost:4444/api/User/UploadVideo';

    angular.module('demo', [
        'blueimp.fileupload',
        'angularUUID2',
        'firebase'
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
            '$scope', '$http', '$filter', '$window', 'uuid2', '$firebaseArray',
            function ($scope, $http, $filter, $window, uuid2, $firebaseArray) {

                // Get User from Local Storage
                $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
                $scope.currentUsername = $scope.currentUser.Username;


                var tmp = new Firebase("https://momworld.firebaseio.com/Video");
                $scope.videoFire = $firebaseArray(tmp);

                $http.get("http://localhost:4444/api/User/Get/" + $scope.currentUsername).
                    success(function (data, status, headers, config) {
                        $scope.user = data;
                    }).
                    error(function (data, status, headers, config) {

                    });


                $scope.options = {
                    url: url,
                };

                $scope.uuid = uuid2.newuuid() ;

                $scope.$on('fileuploadsubmit', function (event, files) {
                    
                        $.each(files.files, function (index, file) {
                            files.formData = {
                                Username: $scope.currentUsername,
                                VideoName: "Video Name",
                                VideoID: $scope.uuid,
                                Content: "Content Ne",

                            };
                        });

                        $scope.uploadData = {
                            Username: "TrungNM4",
                            VideoName: "Video Name",
                            VideoID: $scope.uuid,
                            VideoURL: "http://localhost:4444/App/uploads/video/" + $scope.uuid + ".mp4",
                            Content: "Content Ne"
                        }

                        $scope.videoFire.$add($scope.uploadData).then(function () {
                            alert("Da Firebase 1")
                        });
                });

            }
        ])

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
