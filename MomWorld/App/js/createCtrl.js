app.controller('createCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.quizName = '../../App/js/emptyQuiz.js';
    $scope.quizNum = "";
    $scope.goTo = function (index) {
        if (index > 0 && index <= $scope.totalItems) {
            $scope.currentPage = index;
            $scope.mode = 'quiz';
        }
    }

    $scope.filteredQuestions = [];

    $scope.onSelect = function (option) {
        $scope.questions[$scope.currentPage - 1].selected = option;
        $scope.questions[$scope.currentPage - 1].answered = option.Id;
    }

    $scope.onSave = function () {
        var ques = JSON.stringify($scope.questions, undefined, 2);
        //TODO: Code Pop-up quiz
        alert("Bạn đã tạo quiz thành công, xin đợi người quản lí xác nhận !")
        //$scope.mode = 'result';
    }
    $scope.itemsPerPage = 1;

    $scope.pageCount = function () {
        return Math.ceil($scope.questions.length / $scope.itemsPerPage);
    };

    //If you wish, you may create a separate factory or service to call loadQuiz. To keep things simple, i have kept it within controller.
    $scope.loadQuiz = function (file) {
        $http.get(file)
         .then(function (res) {
             $scope.quiz = res.data.quiz;
             $scope.questions = res.data.questions;
             $scope.totalItems = $scope.questions.length;
             $scope.currentPage = 1;
             $scope.mode = 'quiz';

             $scope.$watch('quizNum', function () {

                 for (var i = 0; i < $scope.quizNum; i++) {
                     $scope.filteredQuestions.push({});
                 }               
                
             });
         });
    }
    $scope.loadQuiz($scope.quizName);

    $scope.isAnswered = function (index) {
        var answered = 'Not Answered';
        $scope.questions[index].options.forEach(function (element, index, array) {
            if (element.selected == true) {
                answered = 'Answered';
                return false;
            }
        });
        return answered;
    };

    $scope.isCorrect = function (question) {
        var result = 'correct';
        var options = question.options || [];
        options.forEach(function (option, index, array) {
            if ($scope.toBool(option.selected) != option.isAnswer) {
                result = 'wrong';
                return false;
            }
        });
        return result;
    };

    //If you wish, you may create a separate utility or helper service to implement toBool. To keep things simple, i have kept it within controller.
    $scope.toBool = function (val) {
        if (val == 'undefined' || val == null || val == '' || val == 'false' || val == 'False')
            return false;
        else if (val == true || val == 'true' || val == 'True')
            return true;
        else
            return 'Not Identified';
    };
  }]);