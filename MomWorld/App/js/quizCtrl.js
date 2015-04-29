app.controller('quizCtrl', ['$scope', '$http', 'helperService', '$location', '$firebaseArray', '$firebaseObject',
    function ($scope, $http, helper, $location, $firebaseArray, $firebaseObject) {

        // Get User from local storage
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        $scope.currentUsername = $scope.currentUser.Username;

        // Facebook Sharing function
        window.fbAsyncInit = function () {
            FB.init({
                appId: '680603725400240', status: true, cookie: true,
                xfbml: true
            });
        };
        (function () {
            var e = document.createElement('script'); e.async = true;
            e.src = document.location.protocol +
            '//connect.facebook.net/en_US/all.js';
            document.getElementById('fb-root').appendChild(e);
        }());

        // Create QUiz
        $scope.createQuizInit = function () {
            alert("Yolo");
            $scope.mode = 'create';
        }

        // Get QuizData
        $scope.quizData = [
    {
        "id": "0",
        "name": "Dinh Dưỡng",
        "quizData": "../../app/js/data/dinhduong.js",
        "quizName": "dinhduong",
        "message": "Bạn rất giỏi về dinh dưỡng đó, hihi"
    },
    {
        "id": "1",
        "name": "Mang thai",
        "quizData": "../../app/js/data/sinhcon.js",
        "quizName": "mangthai",
        "message": "Bạn rất giỏi về dinh dưỡng đó, hihi"
    },
    {
        "id": "2",
        "name": "Sức khỏe",
        "quizData": "../../app/js/data/chamsoc.js",
        "quizName": "suckhoe",
        "message": "Sức khỏe rất quan trọng, và bạn đã làm chủ được "
    },
    {
        "id": "3",
        "name": "Y Tế",
        "quizData": "../../app/js/data/yte.js",
        "quizName": "yte",
        "message": "Giờ bạn đã là một bác sỹ rồi"
    },
    {
        "id": "4",
        "name": "Giới Tính",
        "quizData": "../../app/js/data/gioitinh.js",
        "quizName": "gioitinh",
        "message": "Message cho giới tính ...."
    },
    {
        "id": "5",
        "name": "Thể thao",
        "quizData": "../../app/js/data/gioitinh.js",
        "quizName": "thethao"
    },
        {
        "id": "6",
        "name": "Làm Đẹp",
        "quizData": "../../app/js/data/lamdep.js",
        "quizName": "lamdep"
    },
    {
        "id": "7",
        "name": "Tình Yêu",
        "quizData": "../../app/js/data/love.js",
        "quizName": "love"
    }
    ];

        /* Init Quiz List Page */
        $scope.initQuizShow = function () {
            var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge");
            $scope.badgeFire = $firebaseObject(userFireTmp);
        }

   
    
    $scope.defaultConfig = {
        'allowBack': true,
        'allowReview': true,
        'autoMove': false,  // if true, it will move to next question automatically when answered.
        'duration': 0,  // indicates the time in which quiz needs to be completed. post that, quiz will be automatically submitted. 0 means unlimited.
        'pageSize': 1,
        'requiredAll': false,  // indicates if you must answer all the questions before submitting.
        'richText': false,
        'shuffleQuestions': false,
        'shuffleOptions': false,
        'showClock': false,
        'showPager': true,
        'theme': 'none'
    }

    $scope.initQuiz = function () {
        var userTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/" + $scope.quizNameTemp);
        $scope.badgeFirebase = $firebaseObject(userTmp);
    }

    $scope.initQuizDo = function (quizId) {
        // Get Initial Information
        var quizData = $scope.quizData[quizId].quizData;
        $scope.quizNameTemp = $scope.quizData[quizId].quizName;
        $scope.quizNameReal = $scope.quizData[quizId].name;
        $scope.quizMess = $scope.quizData[quizId].message;
        $scope.currentQuizId = quizId;
        
        // Load Quiz
        $scope.loadQuiz(quizData);

    }


    $scope.goTo = function (index) {
        if (index > 0 && index <= $scope.totalItems) {
            $scope.currentPage = index;
            $scope.mode = 'quiz';
        }
    }

    $scope.onSelect = function (question, option) {
        if (question.QuestionTypeId == 1) { 
            question.Options.forEach(function (element, index, array) {
                if (element.Id != option.Id) {
                    element.Selected = false;
                    question.Answered = element.Id;
                }
            });
        }

        if ($scope.config.autoMove == true && $scope.currentPage < $scope.totalItems)
            $scope.currentPage++;
    }

    $scope.onSubmit = function () {
        var answers = [];
        var correctAns = 0;

        $scope.questions.forEach(function (q, index) {
            
            if ($scope.isCorrect(q) == 'correct'){
                correctAns++;
            }  
            answers.push({ 
                'QuizId': $scope.quiz.Id, 
                'QuestionId': q.Id, 
                'Answered': q.Answered,
                'isCorrect': $scope.isCorrect(q)
            });
        });

        var score = correctAns / answers.length * 100;
        var userTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/" + $scope.quizNameTemp);
        $scope.badgeFirebase = $firebaseObject(userTmp);

        if (score >= 75) {
           
            // Prepare Badge Information to save in Firebase
            $scope.badgeFirebase.Id = $scope.quizNameTemp;
            $scope.badgeFirebase.Name = $scope.quizNameReal;
            $scope.badgeFirebase.Image = '~/Content/images/badge/' + $scope.quizNameTemp + '.png';
            $scope.badgeFirebase.Score = score;
            $scope.badgeFirebase.Status = 'done';
            $scope.badgeFirebase.Message = $scope.quizMess;
            $scope.badgeFirebase.createdDate = Firebase.ServerValue.TIMESTAMP;

            $scope.badgeFirebase.$save().then(
                  $('#modalCompletedQuizz').modal('show')
            )
        } else {
            $scope.badgeFirebase.$loaded().then(function () {
                if ($scope.badgeFirebase.Status != 'done') {
                    $('#modalCompletedQuizzFail').modal('show');
                }
            });

        }
        
        // Display Result Mode
        $scope.mode = 'result';
    }

    $scope.pageCount = function () {
        return Math.ceil($scope.questions.length / $scope.itemsPerPage);
    };

    /* Load quiz base on URL parameters */
    $scope.loadQuiz = function (file) {
        $http.get(file)
         .then(function (res) {
             $scope.quiz = res.data.quiz;
             $scope.config = helper.extend({}, $scope.defaultConfig, res.data.config);
             $scope.questions = $scope.config.shuffleQuestions ? helper.shuffle(res.data.questions) : res.data.questions;
             $scope.totalItems = $scope.questions.length;
             $scope.itemsPerPage = $scope.config.pageSize;
             $scope.currentPage = 1;
             $scope.mode = 'quiz';

             $scope.$watch('currentPage + itemsPerPage', function () {
                 var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
                   end = begin + $scope.itemsPerPage;

                 $scope.filteredQuestions = $scope.questions.slice(begin, end);
             });
         });
    }

    $scope.isAnswered = function (index) {
        var answered = 'Not Answered';
        $scope.questions[index].Options.forEach(function (element, index, array) {
            if (element.Selected == true) {
                answered = 'Answered';
                return false;
            }
        });
        return answered;
    };

    $scope.isCorrect = function (question) {
        var result = 'correct';
        question.Options.forEach(function (option, index, array) {
            if (helper.toBool(option.Selected) != option.IsAnswer) {
                result = 'wrong';
                return false;
            }
        });
        return result;
    };

    /* ------------ Social Functions ------------- */
    $scope.share = function () {
        FB.ui(
	    {
	        method: 'feed',
	        name: 'Chúc mừng bạn đã đạt huân chương ' + $scope.quizNameReal,
	        link: 'http://momworld.com/Profile/GetProfile/',
	        picture: 'https://momworld.firebaseapp.com/images/badge/' + $scope.quizNameTemp + ".png",
	        caption: "Huân chương caption",
	        description: 'This is fucking description',
	        message: ''
	    });
    }
  }]);