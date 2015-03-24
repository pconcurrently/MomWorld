app.controller('quizCtrl', ['$scope', '$http', 'helperService', '$location', function ($scope, $http, helper, $location) {

    $scope.quizName = '';

    $scope.quizData = [
    {
        "id": "0",
        "name": "Dinh Dưỡng",
        "quizData": "../../content/js/data/dinhduong.js",
        "image": "http://localhost:61754/Content/images/badge/dinhduong.png",
        "status": "done"
    },
    {
        "id": "1",
        "name": "sinh con",
        "quizData": "../../content/js/data/sinhcon.js",
        "image": "http://localhost:61754/Content/images/badge/default.png",
        "status": "new"
    },
    {
        "id": "2",
        "name": "Chăm sóc",
        "quizData": "../../content/js/data/chamsoc.js",
        "image": "http://localhost:61754/Content/images/badge/default.png",
        "status": "new"
    },
    {
        "id": "3",
        "name": "Y Tế",
        "quizData": "../../content/js/data/yte.js",
        "image": "http://localhost:61754/Content/images/badge/default.png",
        "status": "new"
    },
    {
        "id": "4",
        "name": "Giới Tính",
        "quizData": "../../content/js/data/gioitinh.js",
        "image": "http://localhost:61754/Content/images/badge/default.png",
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

    $scope.initQuizDo = function (quizId, username) {
        $scope.quizName = $scope.quizData[quizId].quizData;
        $scope.currentUserName = username;

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


        // TODO: Code Huan Chuong
        if (correctAns / answers.length >= 0.75){
            alert('Huan Chuong cho ' + $scope.currentUserName);
            // Post Data to the server to give Badge to User 
            var badge = {
                "UserName": $scope.currentUserName,
                "badge": $scope.quizName
            }

            $http.post('api/Quiz/setBagde', badge).success(function (data, status) {
                alert(data);
                // TODO: Code Display Badge for User;

                
            });

        } else {
            alert('Fail vcc test 1' + $scope.currentUserName);
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