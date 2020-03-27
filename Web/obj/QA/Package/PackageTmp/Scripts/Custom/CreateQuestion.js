
// Title changes
(function ($) {
    TitleChanges = function (GoogleAdHeadlineTempData, GoogleAdLine3TempData, GoogleTopAd, GoogleAdLinkTooltip,
                                GoogleAdHeadlinePrefix, GoogleAdHeadlineSuffix, GoogleAdLine2)
    {
        $('#Title').keyup(function () {
            var title = $('#Title').val();
            var amount = $('#Amount').val();
            if (amount == '')
                amount = GoogleAdHeadlineTempData;

            if (title == '')
                title = GoogleAdLine3TempData;

            var sideAdHtmlToDisplay = title + "<br/>";

            var sideAdHtmlToDisplay = title + "<br/>";
            $('#googleAdLine3').html(sideAdHtmlToDisplay);

            var topAdHtmlToDisplay = "<br/><small>"+GoogleTopAd+"</small> <br/><a href='#' data-toggle='tooltip' " +
                "title='"+GoogleAdLinkTooltip+"'><strong>" + GoogleAdHeadlinePrefix + amount + " " +
                GoogleAdHeadlineSuffix + " - " + title + "</strong></a> <br/>" + "<span class='text-success'>" +
                GoogleAdLine2 + "</span><br/>";
            $('#googleTopAdHeadlineAndLine2').html(topAdHtmlToDisplay);

        });
    };
})(jQuery);

// Amount changes
(function ($) {
    AmountChanges = function (GoogleAdLine3TempData,GoogleAdHeadlineTempData, GoogleSideAd, GoogleAdLinkTooltip,
                                GoogleAdHeadlinePrefix, GoogleAdHeadlineSuffix, GoogleAdLine2, GoogleTopAd) {
        $('#Amount').keyup(function () {
            var amount = $('#Amount').val();
            var title = $('#Title').val();
            if (title == '')
                title = GoogleAdLine3TempData;

            if (amount == '')
                amount = GoogleAdHeadlineTempData;

            var sideAdHtmlToDisplay = "<small>" + GoogleSideAd + "</small> <br/><a href='#' data-toggle='tooltip' " +
                "title='"+GoogleAdLinkTooltip+"'><strong>" + GoogleAdHeadlinePrefix + amount + " " +
                GoogleAdHeadlineSuffix + "</strong></a> <br/>" + "<span class='text-success'>" +
                GoogleAdLine2 + "</span><br/>";

            var topAdHtmlToDisplay = "<br/><small>" + GoogleTopAd + "</small> <br/><a href='#' data-toggle='tooltip' " +
                "title='" + GoogleAdLinkTooltip + "'><strong>" + GoogleAdHeadlinePrefix + amount + " " +
                GoogleAdHeadlineSuffix + " - " + title + "</strong></a> <br/>" + "<span class='text-success'>" +
                GoogleAdLine2 + "</span><br/>";

            $('#googleAdHeadlineAndLine2').html(sideAdHtmlToDisplay);
            $('#googleTopAdHeadlineAndLine2').html(topAdHtmlToDisplay);
        });
    };
})(jQuery);

// Subjects related functions setup
(function ($) {
    SubjectsRelatedFunctionsSetUp = function (GoogleDescrLineMaxCharacters, GoogleAdLine4TempData) {

        var t = $("#tag").tagging();

        $('#newQuestionForm').submit(function () {
            var commaDelimitedSubjects = $.trim(t[0].tagging("getTags")).toLowerCase();
            $("#CommaDelimitedSubjects").val(commaDelimitedSubjects);
            $("#dlgUpdatePage").show().dialog("open");
            return true;  // <- false will cancel submit
        });

        $("#btnSubjects").click(function () {

            var commaDelimitedSubjects = $.trim(t[0].tagging("getTags")).toLowerCase();

            if (commaDelimitedSubjects != '') {
                // Return items where each value is not equal to subjectToAdd
                var array = commaDelimitedSubjects.split(',');

                var subjectsListSeparatedByWhiteSpaces = '';
                jQuery.each(array, function () {
                    subjectsListSeparatedByWhiteSpaces = subjectsListSeparatedByWhiteSpaces + this + " ";
                });
                $('#googleAdLine4').html(subjectsListSeparatedByWhiteSpaces.substring(0, GoogleDescrLineMaxCharacters).trim());
                $('#googleTopAdLine4').html(subjectsListSeparatedByWhiteSpaces.substring(0, GoogleDescrLineMaxCharacters).trim());
            }
            else {
                $('#googleAdLine4').html(GoogleAdLine4TempData.substring(0, GoogleDescrLineMaxCharacters).trim());
                $('#googleTopAdLine4').html(GoogleAdLine4TempData.substring(0, GoogleDescrLineMaxCharacters).trim());
            }
        });

    };
})(jQuery);

// LoadComments
(function ($) {
    LoadComments = function (commentsUrl) {
        $("#includedContent").html('<object data=' + commentsUrl + '>');
    };
})(jQuery);

