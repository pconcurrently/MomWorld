app.controller('userCtrl', ['$scope', '$http', 'md5', function ($scope, $http, md5) {

    $scope.user = { 
        "email": "trungnm0512@gmail.com",
        "badge": [
            {
                "name": "dinh duong",
                "image":"http://localhost:8000/content/images/dinhduong.png"
            },
            {
                "name": "sinh con",
                "image":"http://localhost:8000/content/images/default.png"
            },
            {
                "name": "cham soc",
                "image":"http://localhost:8000/content/images/default.png"
            },
            {
                "name": "y te",
                "image":"http://localhost:8000/content/images/default.png"
            },
            {
                "name": "gioi tinh",
                "image":"http://localhost:8000/content/images/default.png"
            }
        ]


    
    };

    var gravatar = 'http://www.gravatar.com/' + md5.createHash("trungnm0512@gmail.com" || '') + '.json';

    /*
        Get Profile from gravatar
        TrungNM
        */
        $http.get(gravatar)
        .success(function (data, status, headers, config) {
            $scope.user = data.entry[0];
            $scope.user.email = "trungnm0512@gmail.com";
            $scope.user.phone = "+(84)1655757445";




        })
        .error(function (data, status, headers, config) {
            console.log(JSON.stringify(data));
        }).then(function(){
            $scope.feeds = [
            {
                user: 'Khoa',
                status: "đã cập nhật một trạng thái mới",
                time: '7h30',
                content: 'On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain.',
                social: {
                    like: 50,
                    comment: [
                    {
                        name: "PhoTH",
                        avatar: "Content/images/peter-avatar.jpg",
                        content: "These cases are perfectly simple and easy to distinguish.",
                        time: "April 1 at 16:26"
                    },
                    {
                        name: "So Kate",
                        avatar: "Content/images/peter-avatar.jpg",
                        content: "In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best",
                        time: "April 1 at 16:26"
                    },
                    {
                        name: "an desu",
                        avatar: "Content/images/peter-avatar.jpg",
                        content: "These cases are perfectly simple and easy to distinguish.",
                        time: "April 1 at 16:26"
                    },

                    ]
                }
            },

            {
                user: 'Khoa',
                status: "thêm một ảnh mới",
                time: '7h30',
                content: 'Chomcim chim pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain.',
                social: {
                    like: 50,
                    comment: 30
                }
            }
            ];

        }
    );






}]);