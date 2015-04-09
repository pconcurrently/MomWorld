var chatApp = angular.module('chatApp', ['firebase']);

chatApp.controller('chatCtrl', ['$scope', '$http', '$firebaseArray', '$firebaseObject',
    function ($scope, $http, $firebaseArray, $firebaseObject) {

        /* ------------------- Init variables --------------------- */
        
        // Get User from local storage
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        var currentUsername = $scope.currentUser.Username;
        $scope.chatUsername = {};

        // Get Chat of User from Firebase
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername);
        $scope.chatFire = $firebaseObject(userFireTmp);

        /* Init Chat */
        $scope.initChat = function (chatUser) {
            // Set closest coversation
            $scope.chatUsername = chatUser;

            // Set Chat conversation with chatUser
            $scope.getChatFrom(chatUser);

        }



        /* Get Chat between User with Receiver */
        $scope.getChatFrom = function (receiver) {

            // Set Profile of Sender
            var u = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername + "/" + receiver);
            

            $scope.sUser = $firebaseObject(u);
            

            // If this is 1st time --> Add receiver informationth
            if ($scope.sUser.Username == "") {
                $scope.sUser.Username = receiver;
                $scope.sUser.Avatar = "http://localhost:4444/App/uploads/avatar/" + receiver + ".png";
                $scope.sUser.$save();
            }

            var r = new Firebase("https://momworld.firebaseio.com/Chat/" + receiver + "/" + currentUsername);
            $scope.rUser = $firebaseObject(r);
            console.log($scope.rUser);

            if ($scope.rUser.Username == "") {
                $scope.rUser.Username = currentUsername;
                $scope.rUser.Avatar = "http://localhost:4444/App/uploads/avatar/" + currentUsername + ".png";
                $scope.rUser.$save();
            }
           
           

            // Get Chat content from Sender and Receiver
            var senderFire = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername+ "/" + receiver + "/Content");
            $scope.chatContent = $firebaseArray(senderFire);
            $scope.chatContent.$loaded()
                .then(function (data) {
                    

                })
                .catch(function (error) {
                    alert(error);
                });
            var receiverFire = new Firebase("https://momworld.firebaseio.com/Chat/" + receiver + "/" + currentUsername + "/Content");
            $scope.receiverContent = $firebaseArray(receiverFire);
  
        }


        /* Send Messages */
        $scope.sendMessage = function () {
            // Create Data to add to Chat conversatinon
            var sentData = {
                Sender: currentUsername,
                Content: $scope.replyMess,
                createdDate: Firebase.ServerValue.TIMESTAMP
            }

            // Add data to Coversation content
            $scope.chatContent.$add(sentData).then(function () {
                $scope.replyMess = "";
            });
            $scope.receiverContent.$add(sentData);
        }
        
    
  }]); 