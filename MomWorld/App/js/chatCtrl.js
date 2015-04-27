var chatApp = angular.module('chatApp', ['firebase', 'angularMoment']);

chatApp.controller('chatCtrl', ['$scope', '$http', '$firebaseArray', '$firebaseObject', 'amMoment', '$anchorScroll', '$location',
    function ($scope, $http, $firebaseArray, $firebaseObject, amMoment, $anchorScroll, $location) {
        amMoment.changeLocale('vn');

        /* ------------------- Init variables --------------------- */
        
        // Get User from local storage
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        var currentUsername = $scope.currentUser.Username;

        
        // Get Chat of User from Firebase
        var userFireTmp = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername);
        $scope.chatFire = $firebaseObject(userFireTmp);
    
        /* Init Chat */
        $scope.initChat = function (chatUser) {

            /* Get user Profile from User API */
            $http.get("http://localhost:4444/api/User/Get/" + chatUser).
                  success(function (data, status, headers, config) {
                      $scope.rUserAPI = data;

                      // Set Chat conversation with chatUser
                      $scope.getChatFrom(chatUser);

                      // Watch change to Scroll down
                      $scope.chatContent.$watch(function () { $("#chat-content").animate({ scrollTop: 1000000000000000 }, 1000); });

                  }).
                  error(function (data, status, headers, config) {

                  });
           
        }




        /* Get Chat between User with Receiver */
        $scope.getChatFrom = function (receiver) {

            $scope.receiverUsername = receiver;

            // Get Profile of Sender and Receiver
            var s = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername + "/" + receiver);
            var r = new Firebase("https://momworld.firebaseio.com/Chat/" + receiver + "/" + currentUsername);
            $scope.sUser = $firebaseObject(s)
            $scope.rUser = $firebaseObject(r);
        
            // Check if this is 1st chat
            $scope.sUser.$loaded().then(checkSUser);

            // Get Chat content from Sender and Receiver
            var senderFire = new Firebase("https://momworld.firebaseio.com/Chat/" + currentUsername + "/" + receiver + "/Content");
            var receiverFire = new Firebase("https://momworld.firebaseio.com/Chat/" + receiver + "/" + currentUsername + "/Content");
            $scope.chatContent = $firebaseArray(senderFire);
            $scope.receiverContent = $firebaseArray(receiverFire);
  
        }

        $scope.delConversation = function (receiver) {
            $scope.sUser.$remove();
            $scope.rUser.$remove();
        }


        /* Send Messages */
        $scope.sendMessage = function () {
            // Create Data to add to Chat conversatinon
            var createdDate = new Date();
            var sentData = {
                Sender: currentUsername,
                Content: $scope.replyMess,
                CreatedDate: Firebase.ServerValue.TIMESTAMP
            }
            // Add data to Coversation content
            $scope.chatContent.$add(sentData).then(function () {
                // Empty Reply Message Box
                $scope.replyMess = "";

                // Add Notification
                $scope.chatNoti();
               
            });
            $scope.receiverContent.$add(sentData);
        }

        /* Increase Chat Notification */
        $scope.chatNoti = function () {
            var r = new Firebase("https://momworld.firebaseio.com/Chat/" + $scope.receiverUsername + "/" + currentUsername);
            var numView = r.child('NumNoti');
            numView.once('value', function (snapshot) {
                numView.set(snapshot.val() + 1);
            });
        }

        


        

        
        /* ---------------------- Helper Methods ------------------------ */
        var checkSUser = function () {
            // If this is 1st time --> Add receiver informationth
            if ($scope.sUser.Username !== $scope.receiverUsername) {
                $scope.sUser.Username = $scope.receiverUsername;
                $scope.sUser.Avatar = "http://localhost:4444/App/uploads/avatar/" + $scope.receiverUsername + ".png";
                $scope.sUser.$save();

                $scope.rUser.Username = currentUsername;
                $scope.rUser.Avatar = "http://localhost:4444/App/uploads/avatar/" + currentUsername + ".png";
                $scope.rUser.$save();
            }
        }

        /* ------------- Panel Methods ---------------------- */
        var o = new Firebase("https://momworld.firebaseio.com/UserList/");
        $scope.listUser = $firebaseArray(o);
    
  }]); 