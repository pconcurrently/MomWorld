app.controller('quizCtrl', ['$scope', '$http', 'helperService', '$location', '$firebaseArray', '$firebaseObject',
    function ($scope, $http, helper, $location, $firebaseArray, $firebaseObject) {

        // Get User from local storage
        $scope.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        $scope.currentUsername = $scope.currentUser.Username;

    $scope.initQuizShow = function () {

        var userFireTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/");
        $scope.badgeFire = $firebaseArray(userFireTmp);
    }

    $scope.quizData = [
    {
        "id": "0",
        "name": "Dinh Dưỡng",
        "quizData": "../../app/js/data/dinhduong.js",
        "quizName": "dinhduong",
        "status": "done"
    },
    {
        "id": "1",
        "name": "sinh con",
        "quizData": "../../app/js/data/sinhcon.js",
        "quizName": "sinhcon",
        "status": "new"
    },
    {
        "id": "2",
        "name": "Chăm sóc",
        "quizData": "../../app/js/data/chamsoc.js",
        "quizName": "chamsoc",
        "status": "new"
    },
    {
        "id": "3",
        "name": "Y Tế",
        "quizData": "../../app/js/data/yte.js",
        "quizName": "yte",
        "status": "new"
    },
    {
        "id": "4",
        "name": "Giới Tính",
        "quizData": "../../app/js/data/gioitinh.js",
        "quizName": "gioitinh",
        "status": "new"
    }];


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
        var userTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/badge/" + $scope.quizNameTemp);
        $scope.badgeFirebase = $firebaseObject(userTmp);
    }

    $scope.initQuizDo = function (quizId, username) {
        $scope.quizName = $scope.quizData[quizId].quizData;
        $scope.quizNameTemp = $scope.quizData[quizId].quizName;
        $scope.quizNameReal = $scope.quizData[quizId].name;

        $scope.currentQuizId = quizId;

        $scope.loadQuiz($scope.quizName);

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
        // TODO: Code Huan Chuong
        if (score >= 75) {
            var userTmp = new Firebase("https://momworld.firebaseio.com/User/" + $scope.currentUsername + "/Badge/" + $scope.currentQuizId);
            $scope.badgeFirebase = $firebaseObject(userTmp);

            // Give user badge
            $scope.badgeFirebase.Image = 'http://localhost:4444/Content/images/badge/' + $scope.quizNameTemp + '.png';
            $scope.badgeFirebase.Score = score;
            $scope.badgeFirebase.Status = 'done';
            $scope.badgeFirebase.createdDate = Firebase.ServerValue.TIMESTAMP;

            $scope.badgeFirebase.$save().then(
                  $('#modalCompletedQuizz').modal('show')
            )
        } else {
            alert('Fail ' + $scope.currentUsername);
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
  }]);