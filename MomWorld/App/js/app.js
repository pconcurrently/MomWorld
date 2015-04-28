'use strict';

var app = angular.module('quizApp', ['ui.router', 'ngResource', 'ui.bootstrap', 'angular-md5', 'firebase', '720kb.socialshare'])
    // Routing has been added to keep flexibility in mind. This will be used in future.
    

      .config(['$urlRouterProvider', '$stateProvider', function ($urlRouterProvider, $stateProvider) {
          $urlRouterProvider.otherwise('/');
          $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '../../App/js/templates/quiz.html',
                controller: 'quizCtrl'
            })
            .state('review', {
                url: '/review',
                templateUrl: '../../App/js/templates/review.html',
                controller: 'reviewCtrl'
            })
            .state('result', {
                url: '/result',
                templateUrl: '../../App/js/templates/result.html',
                controller: 'quizCtrl'
            })
            .state('contact', {
                url: '/contact',
                templateUrl: '../../App/js/contact.html',
                controller: 'quizCtrl'
            })
            .state('create', {
                url: '/create',
                templateUrl: '../../App/js/templates/create.html',
                controller: 'createCtrl'
            })
      }])