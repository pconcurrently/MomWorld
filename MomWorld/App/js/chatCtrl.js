var chatApp = angular.module('chatApp', ['firebase']);

chatApp.controller('chatCtrl', ['$scope', '$http', '$firebaseArray', '$firebaseObject',
    function ($scope, $http, $firebaseArray, $firebaseObject) {

        /* ------------------- Init variables --------------------- */
        
        

        
        // Get User from local storage
        
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        var currentUsername = $scope.currentUser.Username;


        // Get Chat of User from Firebase
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername);
        $scope.chatFire = $firebaseObject(userFireTmp);

        
        /* Init Chat */
        $scope.initChat = function () {
            // Set closest coversation

        }

        /* Get Chat between User with Receiver */
        $scope.getChatFrom = function (receiver) {

            // Set Current Revercier
            var r = new Firebase("https://momworld.firebaseio.com/User/" + receiver);
            $scope.currentReceiver = $firebaseObject(r);

            // Get Chat content from Sender and Receiver
            var senderFire = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername+ "/" + receiver + "/Content");
            $scope.chatContent = $firebaseArray(senderFire);

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
            $scope.chatContent.$add(sentData);
            $scope.receiverContent.$add(sentData);
        }
        
    
  }]); 