'use strict';

var app = angular.module('quizApp', ['ui.router', 'ngResource', 'ui.bootstrap', 'angular-md5'])
    // Routing has been added to keep flexibility in mind. This will be used in future.
    

      .config(['$urlRouterProvider', '$stateProvider', function ($urlRouterProvider, $stateProvider) {
          $urlRouterProvider.otherwise('/');
          $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '../../Content/js/templates/quiz.html',
                controller: 'quizCtrl'
            })
            .state('review', {
                url: '/review',
                templateUrl: '../../Content/js/templates/review.html',
                controller: 'reviewCtrl'
            })
            .state('result', {
                url: '/result',
                templateUrl: '../../Content/js/templates/result.html',
                controller: 'quizCtrl'
            })
            .state('contact', {
                url: '/contact',
                templateUrl: '../../Content/js/contact.html',
                controller: 'quizCtrl'
            })
            .state('create', {
                url: '/create',
                templateUrl: '../../Content/js/templates/create.html',
                controller: 'createCtrl'
            })
      }])