(function(jQuery) {

    jQuery.fn.jquizzy = function(settings) {
        var defaults = {
            questions: null,
            categoryid: 1,
            categorytitle: '',
            catname: '',
            startText: 'Let\'s get started!',
            endText: 'Finished!',
            splashImage: 'Content/images/quiz-icon/start.png',
            twittershow: true,
            twitterUsername: '',
            twitterStatus: 'I scored {score}% on this awesome! Check it out!',
            fbshow: true,
            gplusshow: true,
            linkedshow: true,
            email: false,
            user: false,
            guest: false,
            total: 10,
            time: true,
            timer: true,
            langQuestion: 'Question #',
            langCorrect: 'Correct',
            langIncorrect: 'Incorrect',
            langYouscored: 'You scored',
            langNext: 'Next',
            langPrev: 'Prev',
            langFinish: 'Finish',
            langNotattempted: 'Not attempted',
            langTimeout: 'Time out',
            langResultEmailed: 'Your result details has been mailed.',
            langName: 'Name',
            langEmail: 'Email',
            langSubmitdetails: 'Submit Details',
            langEntername: 'Please enter your name.',
            langEnteremail: 'Please enter your email address.',
            langEntervalidemail: 'Enter a valid email address.',
            langSelectOption: 'Please select an option',
            langparseques: 'Failed to parse questions.',
            langrightans: 'Right Answer : ',
            langwrongans: 'Wrong Answer :',
            languserans: 'User Answer :',
            resultshow: true,
            modclasssfx: '',
            resultComments: {
                perfect: 'Perfect!',
                excellent: 'Excellent!',
                good: 'Good!',
                average: 'Acceptable!',
                bad: 'Disappointing!',
                poor: 'Poor!',
                worst: 'Nada!',
                wkpath: ''
            }
        };


        var config = jQuery.extend(defaults, settings);
        var siteurl = jQuery(location).attr('href');
        console.log(config.langSelectOption);
        console.log(config.startText);

        if (config.questions === null) {
            jQuery(this).html('<div class="intro-container slide-container"><h2 class="qTitle">' + config.langparseques + '</h2></div>');
            return
        }
        var superContainer = jQuery(this),
            answers = [],
            introFob = '<div class="intro-container slide-container"><div class="question-number">' + config.startText + '</div><a class="nav-start" href="javascript:void(0);"><img src="' + config.splashImage + '" /></a></div>',

            guestFob = '<div class="guest-container slide-container"><div><div id="namediv"><label>' + config.langName + '</label><input id="user_name" type="text" name="guest-name" /><div class="name_validation"></div></div><br/><div id="emaildiv"><label>' + config.langEmail + '</label><input id="user_email" type="text" name="guest-email" /><div class="email_validation"></div></div></div><a class="submit-start" href="javascript:void(0);">' + config.langSubmitdetails + '</a></div>',

            exitFob = '<div class="results-container slide-container"><div class="question-number">' + config.endText + '</div><div class="result-keeper"></div></div><div class="notice">' + config.langSelectOption + '</div><div id="shortly' + config.modclasssfx + '" class="shortly-class"><span class="countdown_row countdown_show3"><span class="countdown_section"><span class="countdown_amount">0</span>Hours</span><span class="countdown_section"><span class="countdown_amount">0</span>Minutes</span><span class="countdown_section"><span class="countdown_amount">0</span>Seconds</span></span></div><div class="progress-keeper" ><div class="progress"></div></div>',

            contentFob = '';
        superContainer.addClass('main-quiz-holder');
        for (questionsIteratorIndex = 0; questionsIteratorIndex < config.questions.length; questionsIteratorIndex++) {
            contentFob += '<div class="slide-container"><div class="question-number">' + (questionsIteratorIndex + 1) + '/' + config.questions.length + '</div><div class="question">' + config.questions[questionsIteratorIndex].question + '</div><ul class="answers">';

            for (answersIteratorIndex = 0; answersIteratorIndex < config.questions[questionsIteratorIndex].answers.length; answersIteratorIndex++) {
                contentFob += '<li>' + config.questions[questionsIteratorIndex].answers[answersIteratorIndex] + '</li>'
            }
            contentFob += '</ul><div class="nav-container">';
            if (questionsIteratorIndex !== 0) {
                contentFob += '<div class="prev"><a class="nav-previous" href="#">' + config.langPrev + '</a></div>'
            }
            if (questionsIteratorIndex < config.questions.length - 1) {
                contentFob += '<div class="next"><a class="nav-next" href="#">' + config.langNext + '</a></div>'
            } else {
                contentFob += '<div class="next final"><a class="nav-show-result" href="#">' + config.langFinish + '</a></div>'
            }
            contentFob += '</div></div>';
            answers.push(config.questions[questionsIteratorIndex].correctAnswer)
        }
        if (config.email == true && config.guest == true) {
            superContainer.html(introFob + guestFob + contentFob + exitFob);
        } else {
            superContainer.html(introFob + contentFob + exitFob);
        }
        var progress = superContainer.find('.progress'),
            progressKeeper = superContainer.find('.progress-keeper'),
            shortlyclass = superContainer.find('.shortly-class'),
            notice = superContainer.find('.notice'),
            progressWidth = progressKeeper.width(),
            userAnswers = [],
            questionLength = config.questions.length,
            slidesList = superContainer.find('.slide-container');

        function checkAnswers() {
            var resultArr = [],
                flag = false;
            for (i = 0; i < answers.length; i++) {
                if (answers[i] == userAnswers[i]) {
                    flag = true
                } else {
                    flag = false
                }
                resultArr.push(flag)
            }
            return resultArr
        }

        function roundReloaded(num, dec) {
            var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
            return result
        }

        function judgeSkills(score) {
            var returnString;
            if (score == 100) return config.resultComments.perfect;
            else if (score > 90) return config.resultComments.excellent;
            else if (score > 70) return config.resultComments.good;
            else if (score > 50) return config.resultComments.average;
            else if (score > 35) return config.resultComments.bad;
            else if (score > 20) return config.resultComments.poor;
            else return config.resultComments.worst
        }

        function liftOff() {
            quit();
        }

        progressKeeper.hide();
        shortlyclass.hide();
        notice.hide();
        slidesList.hide().first().fadeIn(500);

        superContainer.find('li').click(function() {
            var thisLi = jQuery(this);
            if (thisLi.hasClass('selected')) {
                thisLi.removeClass('selected')
            } else {
                thisLi.parents('.answers').children('li').removeClass('selected');
                thisLi.addClass('selected')
            }
        });

        superContainer.find('.submit-start').click(function() {
            jQuery(".name_validation").hide();
            jQuery(".email_validation").hide();
            var username = jQuery("#user_name").val();
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            var emailaddressVal = jQuery("#user_email").val();
            jQuery(".name_validation").html('');
            jQuery(".email_validation").html('');

            if (username == '') {
                jQuery(".name_validation").html(config.langEntername);
                jQuery(".name_validation").fadeIn(300);
            } else if (emailaddressVal == '') {
                jQuery(".email_validation").html(config.langEnteremail);
                jQuery(".email_validation").fadeIn(300);
            } else if (!emailReg.test(emailaddressVal)) {
                jQuery(".email_validation").html(config.langEntervalidemail);
                jQuery(".email_validation").fadeIn(300);
            } else {
                jQuery(this).parents('.slide-container').fadeOut(500, function() {
                    jQuery(this).next().fadeIn(500);
                    progressKeeper.fadeIn(500);
                    if (config.timer == false) {
                        if (config.time == false) {
                            shortlyclass.hide();
                        } else {
                            shortlyclass.show();
                        }
                    } else {
                        shortlyclass.show();
                    }
                });
                if (config.timer == false) {
                    jQuery('#shortly' + config.modclasssfx).countdown({
                        until: config.total,
                        layout: '{hnn}{sep}{mnn}{sep}{snn}',
                        onExpiry: liftOff
                    });
                } else {
                    jQuery('#shortly' + config.modclasssfx).countdown({
                        since: new Date(),
                        layout: '{hnn}{sep}{mnn}{sep}{snn}'
                    });
                }
                return false
            }
        });

        superContainer.find('.nav-start').click(function() {
            jQuery(this).parents('.slide-container').fadeOut(500, function() {
                jQuery(this).next().fadeIn(500);
                if (config.email == false || config.guest == false) {
                    progressKeeper.fadeIn(500);
                    if (config.timer == false) {
                        if (config.time == false) {
                            shortlyclass.hide();
                        } else {
                            shortlyclass.show();
                        }
                    } else {
                        shortlyclass.show();
                    }
                    if (config.timer == false) {
                        jQuery('#shortly' + config.modclasssfx).countdown({
                            until: config.total,
                            layout: '{hnn}{sep}{mnn}{sep}{snn}',
                            onExpiry: liftOff
                        });
                    } else {
                        jQuery('#shortly' + config.modclasssfx).countdown({
                            since: new Date(),
                            layout: '{hnn}{sep}{mnn}{sep}{snn}'
                        });
                    }
                }
            });
            return false
        });
        superContainer.find('.next').click(function() {
            if (jQuery(this).parents('.slide-container').find('li.selected').length === 0) {
                notice.fadeIn(300);
                return false
            }
            notice.hide();
            jQuery(this).parents('.slide-container').fadeOut(500, function() {
                jQuery(this).next().fadeIn(500)
            });
            progress.animate({
                    width: progress.width() + Math.round(progressWidth / questionLength)
                },
                500);
            return false
        });
        superContainer.find('.prev').click(function() {
            notice.hide();
            jQuery(this).parents('.slide-container').fadeOut(500, function() {
                jQuery(this).prev().fadeIn(500)
            });
            progress.animate({
                    width: progress.width() - Math.round(progressWidth / questionLength)
                },
                500);
            return false
        });
        superContainer.find('.final').click(function() {




            pause();
            if (jQuery(this).parents('.slide-container').find('li.selected').length === 0) {
                notice.fadeIn(300);
                return false
            }
            superContainer.find('li.selected').each(function(index) {
                userAnswers.push(jQuery(this).parents('.answers').children('li').index(jQuery(this).parents('.answers').find('li.selected')) + 1)
            });
            progressKeeper.hide();
            var results = checkAnswers(),
                resultrow = '',
                resultview = '',
                trueCount = 0,
                shareButton = '',
                score;
            for (var i = 0, toLoopTill = results.length; i < toLoopTill; i++) {
                if (results[i] === true) {
                    trueCount++;
                    isCorrect = true
                }
                resultrow += '<div class="result-row">' + config.langQuestion + (i + 1) + (results[i] === true ? '<div class="correct"><span>' + config.langCorrect + '</span></div>' : '<div class="wrong"><span>' + config.langIncorrect + '</span></div>');
                resultrow += '<div class="resultsview-qhover">' + config.questions[i].question;
                resultrow += "<ul>";
                for (answersIteratorIndex = 0; answersIteratorIndex < config.questions[i].answers.length; answersIteratorIndex++) {
                    var classestoAdd = '';
                    if (config.questions[i].correctAnswer == answersIteratorIndex + 1) {
                        classestoAdd += 'right'
                    }
                    if (userAnswers[i] == answersIteratorIndex + 1) {
                        classestoAdd += ' selected'
                    }
                    resultrow += '<li class="' + classestoAdd + '">' + config.questions[i].answers[answersIteratorIndex] + '</li>'
                }
                resultrow += '</ul></div></div>'
            }
            resultview = resultrow;

            score = roundReloaded(trueCount / questionLength * 100, 2);

            /*if(config.twittershow == true || config.fbshow == true || config.gplusshow == true || config.linkedshow == true){
			shareButton = '<div class="social_body">';
			}
			if(config.twittershow == true) {
			shareButton += '<div class="social_tab"><a href="http://twitter.com/share" data-text="' + config.twitterStatus.replace("{score}", score) + '" class="twitter-share-button" data-count="vertical" data-via="' + config.twitterUsername + '"></a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"><\/script></div>';
			}			
			if(config.fbshow == true) {
			shareButton += '<div class="social_tab"><iframe src="//www.facebook.com/plugins/like.php?href=' + siteurl + '&send=false&layout=box_count&width=50&show_faces=true&action=like&colorscheme=light&font&height=62" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:50px; height:62px;" allowTransparency="true"></iframe></div>';
			}			
			if(config.gplusshow == true) {
			shareButton += '<div class="social_tab"><div class="g-plus" data-action="share" data-annotation="vertical-bubble" data-height="60" data-href="' + siteurl + '" ></div><script type="text/javascript">(function() {   var po = document.createElement("script"); po.type = "text/javascript"; po.async = true;   po.src = "https://apis.google.com/js/plusone.js";   var s = document.getElementsByTagName("script")[0];s.parentNode.insertBefore(po, s);  })(); </script></div>';
			}*/

            // shareButton +='<div
            // class="social_tab'+config.modclasssfx+'"><script
            // src="//platform.linkedin.com/in.js"
            // type="text/javascript"></script><script type="IN/Share"
            // data-url="http://ayush.com/developmentj3/index.php/nl/"
            // data-counter="top"></script></div>';
            if (config.linkedshow == true) {


                shareButton += '<div class="social_tab' + config.modclasssfx + '"><script src="//platform.linkedin.com/in.js" type="text/javascript"></script><script type="IN/Share" data-url="http://ayush.com/developmentj3/index.php/nl/" data-counter="top"></script></div>';




            }
            if (config.twittershow == true || config.fbshow == true || config.gplusshow == true || config.linkedshow == true) {
                shareButton += '</div>';
            }
            if (config.resultshow == true) {
                resultSet = '<h2 class="qTitle">' + judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%</h2>' + shareButton + resultrow + '<div class="jquizzy-clear"></div>';
            } else {
                resultSet = '<h2 class="qTitle">' + judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%</h2>' + shareButton + '<div class="jquizzy-clear"></div>';
            }
            var username = jQuery("#user_name").val();
            var emailaddressVal = jQuery("#user_email").val();
            var totaltime = '';

            var remaintime = '';

            if (config.timer == false) {
                totaltime = Array(0, 0, 0, 0, 0, 0, 0);

                remaintime = jQuery('#shortly' + config.modclasssfx).countdown('getTimes');
            } else {

                remaintime = Array(0, 0, 0, 0, 0, 0, 0);

                totaltime = jQuery('#shortly' + config.modclasssfx).countdown('getTimes');

            }

            pscore = score;
            store = '';
            store += config.langrightans + trueCount + '<br/>';
            wrongcount = questionLength - trueCount;
            store += config.langwrongans + wrongcount;
            jQuery.ajax({
                url: "?option=com_ajaxquiz&task=storedata&no_html=1&username=" + username + "&useremail=" + emailaddressVal + "&catname=" + config.catname + "&cid=" + config.categoryid + "",
                datatype: "html",
                type: "POST",
                data: {
                    store: store,
                    score: pscore,
                    result: resultview,
                    totaltime: totaltime,
                    remaintime: remaintime
                }
            });
            if (config.email == true) {
                var resultarr = new Array();
                resultmail = '';
                resultmail += judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%. <br/>';
                for (var i = 0, toLoopTill = results.length; i < toLoopTill; i++) {
                    resultarr[i] = new Array(3);
                    var userans = '';
                    var rightans = '';
                    if (results[i] === true) {
                        trueCount++;
                        isCorrect = true
                    }
                    resultmail += config.questions[i].question + ': <br/>';
                    resultarr[i][0] = config.questions[i].question;
                    for (answersIteratorIndex = 0; answersIteratorIndex < config.questions[i].answers.length; answersIteratorIndex++) {
                        if (config.questions[i].correctAnswer == answersIteratorIndex + 1) {
                            rightans = config.questions[i].answers[answersIteratorIndex];
                            resultmail += 'Right Answer : ' + rightans + '<br/>';
                        }
                        if (userAnswers[i] == answersIteratorIndex + 1) {
                            userans = config.questions[i].answers[answersIteratorIndex];
                            resultmail += config.languserans + userans + '<br/>';
                        }
                    }
                    resultarr[i][1] = rightans;
                    resultarr[i][2] = userans;
                }
                mailques = resultmail;
            }
            if (config.email == true) {
                resultSet = '<h2 class="qTitle">' + config.langResultEmailed + '<br/><br/> ' + shareButton + '</h2>';
                if (config.resultshow == true) {
                    resultSet += resultrow + '<div class="jquizzy-clear">';
                }
                superContainer.find('.result-keeper').html(resultSet).show(500);
            }
            var username = jQuery("#user_name").val();
            var emailaddressVal = jQuery("#user_email").val();
            if (config.email == true) {
                jQuery.ajax({
                    url: "index.php?option=com_ajaxquiz&task=mailresult&no_html=1&username=" + username + "&useremail=" + emailaddressVal + "&categorytitle=" + config.categorytitle + "",
                    type: "POST",
                    data: {
                        mydata: resultarr,
                        score: pscore,
                        totaltime: totaltime,
                        remaintime: remaintime
                    }
                });
            }
            superContainer.find('.result-keeper').html(resultSet).show(500);
            superContainer.find('.resultsview-qhover').hide();
            superContainer.find('.result-row').hover(function() {
                    jQuery(this).find('.resultsview-qhover').show()
                },
                function() {
                    jQuery(this).find('.resultsview-qhover').hide()
                });
            jQuery(this).parents('.slide-container').fadeOut(500, function() {
                jQuery(this).next().fadeIn(500)
            });
        })

        function quit() {
            if (jQuery(this).parents('.slide-container').find('li.selected').length === 0) {
                superContainer.find('li.selected').each(function(index) {
                    userAnswers.push(jQuery(this).parents('.answers').children('li').index(jQuery(this).parents('.answers').find('li.selected')) + 1)
                });
            }
            // shortlyclass.hide();

            shortlyclass.show();

            shortlyclass.css('right', '85px');
            progressKeeper.hide();
            var results = checkAnswers(),
                resultrow = '',
                trueCount = 0,

                shareButton = '',
                score;
            for (var i = 0, toLoopTill = superContainer.find('li.selected').length; i < toLoopTill; i++) {
                if (results[i] === true) {
                    trueCount++;
                    isCorrect = true
                }
                resultrow += '<div class="result-row">' + config.langQuestion + (i + 1) + (results[i] === true ? '<div class="correct"><span>' + config.langCorrect + '</span></div>' : '<div class="wrong"><span>' + config.langIncorrect + '</span></div>');
                resultrow += '<div class="resultsview-qhover">' + config.questions[i].question;
                resultrow += "<ul>";
                for (answersIteratorIndex = 0; answersIteratorIndex < config.questions[i].answers.length; answersIteratorIndex++) {
                    var classestoAdd = '';
                    if (config.questions[i].correctAnswer == answersIteratorIndex + 1) {
                        classestoAdd += 'right'
                    }
                    if (userAnswers[i] == answersIteratorIndex + 1) {
                        classestoAdd += ' selected'
                    }
                    resultrow += '<li class="' + classestoAdd + '">' + config.questions[i].answers[answersIteratorIndex] + '</li>'
                }
                resultrow += '</ul></div></div>'
            }
            for (var i = superContainer.find('li.selected').length; i < results.length; i++) {
                resultrow += '<div class="result-row">' + config.langQuestion + (i + 1) + ("<div class='notattempted'>" + config.langNotattempted + "</div>");
                resultrow += '</div>';
            }
            resultview = resultrow;
            score = roundReloaded(trueCount / questionLength * 100, 2);

            if (config.twittershow == true || config.fbshow == true || config.gplusshow == true || config.linkedshow == true) {
                shareButton = '<div id="social_body">';
            }
            if (config.twittershow == true) {
                shareButton += '<div class="social_tab"><a href="http://twitter.com/share" data-text="' + config.twitterStatus.replace("{score}", score) + '" class="twitter-share-button" data-count="vertical" data-via="' + config.twitterUsername + '"></a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"><\/script></div>';
            }
            if (config.fbshow == true) {
                shareButton += '<div class="social_tab"><iframe src="//www.facebook.com/plugins/like.php?href=' + siteurl + '&send=false&layout=box_count&width=50&show_faces=true&action=like&colorscheme=light&font&height=62" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:50px; height:62px;" allowTransparency="true"></iframe></div>';
            }
            if (config.gplusshow == true) {
                shareButton += '<div class="social_tab"><div class="g-plus" data-action="share" data-annotation="vertical-bubble" data-height="60" data-href="' + siteurl + '" ></div><script type="text/javascript">(function() {   var po = document.createElement("script"); po.type = "text/javascript"; po.async = true;   po.src = "https://apis.google.com/js/plusone.js";   var s = document.getElementsByTagName("script")[0];s.parentNode.insertBefore(po, s);  })(); </script></div>';
            }
            if (config.linkedshow == true) {
                console.log("call");
                shareButton += '<script src="//platform.linkedin.com/in.js" type="text/javascript"></script>';
                shareButton += '<div class="social_tab"><script type="IN/Share" data-url="' + siteurl + '" data-counter="top"></script></div>';

                shareButton += '<script src="//platform.linkedin.com/in.js" type="text/javascript"></script>';
                shareButton += '<div class="social_tab"><script type="IN/Share" data-url="' + siteurl + '" data-counter="top"></script></div>';
            }
            if (config.twittershow == true || config.fbshow == true || config.gplusshow == true || config.linkedshow == true) {
                shareButton += '</div>';
            }
            if (config.resultshow == true) {
                resultSet = '<h2 class="qTitle">' + ' ' + config.langTimeout + ' </h2>' + '<h2 class="qTitle">' + judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%</h2>' + shareButton + resultrow + '<div class="jquizzy-clear"></div>';
            } else {
                resultSet = '<h2 class="qTitle">' + ' ' + config.langTimeout + ' </h2>' + '<h2 class="qTitle">' + judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%</h2>' + shareButton + '<div class="jquizzy-clear"></div>';
            }

            var username = jQuery("#user_name").val();
            var emailaddressVal = jQuery("#user_email").val();
            var totaltime = '';
            var remaintime = '';
            if (config.timer == false) {
                totaltime = Array(0, 0, 0, 0, 0, 0, 0);
                remaintime = jQuery('#shortly' + config.modclasssfx).countdown('getTimes');
            } else {
                remaintime = Array(0, 0, 0, 0, 0, 0, 0);
                totaltime = jQuery('#shortly' + config.modclasssfx).countdown('getTimes');
            }

            pscore = score;
            store = '';
            store += config.langrightans + trueCount + '<br/>';
            wrongcount = questionLength - trueCount;
            store += config.langwrongans + wrongcount;
            jQuery.ajax({
                url: "?option=com_ajaxquiz&task=storedata&no_html=1&username=" + username + "&useremail=" + emailaddressVal + "&catname=" + config.catname + "&cid=" + config.categoryid + "",
                datatype: "html",
                type: "POST",
                data: {
                    store: store,
                    score: pscore,
                    result: resultview,
                    totaltime: totaltime,
                    remaintime: remaintime
                }
            });

            if (config.email == true) {
                var resultarr = new Array();
                resultmail = '';
                resultmail += judgeSkills(score) + ' ' + config.langYouscored + ' ' + score + '%. <br/>';
                for (var i = 0, toLoopTill = superContainer.find('li.selected').length; i < toLoopTill; i++) {
                    resultarr[i] = new Array(3);
                    var userans = '';
                    var rightans = '';
                    if (results[i] === true) {
                        trueCount++;
                        isCorrect = true
                    }
                    resultmail += config.questions[i].question + ': <br/>';
                    resultarr[i][0] = config.questions[i].question;
                    for (answersIteratorIndex = 0; answersIteratorIndex < config.questions[i].answers.length; answersIteratorIndex++) {
                        if (config.questions[i].correctAnswer == answersIteratorIndex + 1) {
                            rightans = config.questions[i].answers[answersIteratorIndex];
                            resultmail += 'Right Answer : ' + rightans + '<br/>';
                        }
                        if (userAnswers[i] == answersIteratorIndex + 1) {
                            userans = config.questions[i].answers[answersIteratorIndex];
                            resultmail += config.languserans + userans + '<br/>';
                        }
                    }
                    resultarr[i][1] = rightans;
                    resultarr[i][2] = userans;
                }
                for (var i = superContainer.find('li.selected').length; i < results.length; i++) {
                    resultmail += 'Question ' + (i + 1) + ': ' + config.langNotattempted + ' <br/>';
                }
                mailques = resultmail;
            }
            if (config.email == true) {
                resultSet = '<h2 class="qTitle">' + config.langTimeout + ' <br/><br/> ' + config.langResultEmailed + '<br/><br/> ' + shareButton + ' </h2>';
                if (config.resultshow == true) {
                    resultSet += resultrow + '<div class="jquizzy-clear">';
                }
                superContainer.find('.result-keeper').html(resultSet).show(500);
            }
            var username = jQuery("#user_name").val();
            var emailaddressVal = jQuery("#user_email").val();
            if (config.email == true) {
                jQuery.ajax({
                    url: "index.php?option=com_ajaxquiz&task=mailresult&no_html=1&username=" + username + "&useremail=" + emailaddressVal + "&categorytitle=" + config.categorytitle + "",
                    type: "POST",
                    data: {
                        mydata: resultarr,
                        score: pscore,
                        totaltime: totaltime,
                        remaintime: remaintime
                    }
                });
            }

            superContainer.find('.slide-container').html(resultSet);
            superContainer.find('.result-keeper').html(resultSet).show(500);
            superContainer.find('.resultsview-qhover').hide();
            superContainer.find('.result-row').hover(function() {
                    jQuery(this).find('.resultsview-qhover').show()
                },
                function() {
                    jQuery(this).find('.resultsview-qhover').hide()
                });
            jQuery(this).parents('.slide-container').fadeOut(500, function() {
                jQuery(this).next().fadeIn(500)
            });
        }

        function pause() {
            jQuery('#shortly' + config.modclasssfx).countdown('pause');
            // shortlyclass.hide();

            shortlyclass.show();

            shortlyclass.css('right', '85px');

        }
    }
})(jQuery);