$(function () {

    $('#typeahead').typeahead({
        name: 'songs',
        valueKey: 'Subject',
        remote: {
            url: '/Registration/Autocomplete?term=%QUERY'
        }
    });

    $('#Title').keyup(function () {
        var title = $('#Title').val();
        var amount = $('#Amount').val();
        if (amount == '')
            amount = "@CommonResources.GoogleAdHeadlineTempData";

        if (title == '')
            title = "@CommonResources.GoogleAdLine3TempData";

        var sideAdHtmlToDisplay = title + "<br/>";

        var sideAdHtmlToDisplay = title + "<br/>";
        $('#googleAdLine3').html(sideAdHtmlToDisplay);

        var topAdHtmlToDisplay = "<br/><small>@CommonResources.GoogleTopAd</small> <br/><a href='#' data-toggle='tooltip' " +
            "title='@CommonResources.GoogleAdLinkTooltip'><strong>" + "@CommonResources.GoogleAdHeadlinePrefix" + amount +
            " @CommonResources.GoogleAdHeadlineSuffix - " + title + "</strong></a> <br/>" + "<span class='text-success'>" +
            "@CommonResources.GoogleAdLine2" + "</span><br/>";
        $('#googleTopAdHeadlineAndLine2').html(topAdHtmlToDisplay);

    });

    $('#Amount').keyup(function () {
        var amount = $('#Amount').val();
        var title = $('#Title').val();
        if (title == '')
            title = "@CommonResources.GoogleAdLine3TempData";

        if (amount == '')
            amount = "@CommonResources.GoogleAdHeadlineTempData";

        var sideAdHtmlToDisplay = "<small>@CommonResources.GoogleSideAd</small> <br/><a href='#' data-toggle='tooltip' " + 
            "title='@CommonResources.GoogleAdLinkTooltip'><strong>" + "@CommonResources.GoogleAdHeadlinePrefix" + amount +
            " @CommonResources.GoogleAdHeadlineSuffix" + "</strong></a> <br/>" + "<span class='text-success'>" +
            "@CommonResources.GoogleAdLine2" + "</span><br/>";

        var topAdHtmlToDisplay = "<br/><small>@CommonResources.GoogleTopAd</small> <br/><a href='#' data-toggle='tooltip' " +
            "title='@CommonResources.GoogleAdLinkTooltip'><strong>" + "@CommonResources.GoogleAdHeadlinePrefix" + amount +
            " @CommonResources.GoogleAdHeadlineSuffix - " + title + "</strong></a> <br/>" + "<span class='text-success'>" +
            "@CommonResources.GoogleAdLine2" + "</span><br/>";

        $('#googleAdHeadlineAndLine2').html(sideAdHtmlToDisplay);
        $('#googleTopAdHeadlineAndLine2').html(topAdHtmlToDisplay);
    });

    $(".AddSubject").click(function () {
        // Get the id from the link
        var subjectToAdd = $('#typeahead').val();
        if (subjectToAdd != '') {
            var tablePrefix = '<table><tr>';
            var tableSuffix = '</tr></table>';
            var CommaDelimitedSubjects = $('#CommaDelimitedSubjects').val();
            if (CommaDelimitedSubjects != '') {

                // Return items where each value is not equal to subjectToAdd
                var array = CommaDelimitedSubjects.split(',');
                if (array.length <= 4)
                {
                    array = $.grep(array, function (value) {
                        return value.toLowerCase() != subjectToAdd.toLowerCase();
                    });

                    array.push(subjectToAdd);
                    CommaDelimitedSubjects = array.join(",");
                }

                var subjectsListHtml = '';
                var subjectsListWithoutHtml = '';
                jQuery.each(array, function () {
                    //subjectsListHtml = subjectsListHtml + "<td><span class='label label-primary'>" + this + "</span>" + "<span class='input-group-btn'>" +
                    //"<a href='#' class='AddSubject' data-id=''><span class='btn btn-success'>x</span></a>" + "</span>" + "</td>";

                    //subjectsListHtml = subjectsListHtml + "<td><span class='label label-primary'>" + this + "</span>" +
                    //    "<a href='#' class='RemoveSubject' data-id=" + this + "><span class='label label-danger'>x</span></a>" + "</td>";

                    //subjectsListHtml = subjectsListHtml + "<td><span class='label label-primary'>" + this + "</span>" +
                    //    "<a href='/NewQuestion/Create?subjectToRemove=" + this + "&CommaDelimitedSubjects=" + CommaDelimitedSubjects +
                    //    "' target='_blank' class='RemoveSubject' data-id=" + this + "><span class='label label-danger'>x</span></a>" + "</td>";

                    subjectsListHtml = subjectsListHtml + "<td><span class='label label-primary'>" + this + "</span></td>";
                    subjectsListWithoutHtml = subjectsListWithoutHtml + this + " ";
                });
                subjectsListHtml = tablePrefix + subjectsListHtml + tableSuffix;
                $('#subjectData').html(subjectsListHtml);
                $('#CommaDelimitedSubjects').val(CommaDelimitedSubjects);
                $('#googleAdLine4').html(subjectsListWithoutHtml.substring(0, @Size.GoogleDescrLineMaxCharacters+0).trim());
                $('#googleTopAdLine4').html(subjectsListWithoutHtml.substring(0, @Size.GoogleDescrLineMaxCharacters+0).trim());
            }
            else {
                $('#CommaDelimitedSubjects').val(subjectToAdd);

                @*subjectsListHtml = "<span class='input-group-btn'><a href='#' class='AddSubject' data-id=''><span class='btn btn-success'>@CommonResources.BtnAddQuestiongSubject</span></a></span>"*@
                //subjectsListHtml = "<td><span class='label label-primary'>" + subjectToAdd + "</span>" +
                //    "<a href='/NewQuestion/Create?subjectToRemove=" + subjectToAdd + "&CommaDelimitedSubjects=" + $('#CommaDelimitedSubjects').val() +
                //    "' target='_blank' class='RemoveSubject' data-id=" + subjectToAdd + "><span class='label label-danger'>x</span></a>" + "</td>";

                subjectsListHtml = "<td><span class='label label-primary'>" + subjectToAdd + "</span></td>";
                subjectsListHtml = tablePrefix + subjectsListHtml + tableSuffix;
                $('#subjectData').html(subjectsListHtml);
                $('#googleAdLine4').html(subjectToAdd.substring(0, @Size.GoogleDescrLineMaxCharacters+0).trim());
                $('#googleTopAdLine4').html(subjectToAdd.substring(0, @Size.GoogleDescrLineMaxCharacters+0).trim());
            }
        }
    });

    $(".RemoveSubject").click(function () {
        // Get the id from the link
        alert("one");
        var subjectToRemove = $(this).attr("data-id");
        var tablePrefix = '<table><tr>';
        var tableSuffix = '</tr></table>';
        if (subjectToRemove != '') {
            var CommaDelimitedSubjects = $('#CommaDelimitedSubjects').val();
            if (CommaDelimitedSubjects != '') {

                var array = CommaDelimitedSubjects.split(',');
                array = $.grep(array, function (value) {
                    return value != subjectToRemove;
                });

                var subjectsListHtml = '';
                jQuery.each(array, function () {
                    subjectsListHtml = subjectsListHtml + "<td><span class='label label-info'>" + this + "</span>" +
                    "<a href='#' class='RemoveSubject' data-id='@this'><span class='label label-important'><i class='icon-remove icon-white'></span></a>" + "</td>";

                });
                subjectsListHtml = tablePrefix + subjectsListHtml + tableSuffix;
                $('#subjectData').html(subjectsListHtml);
                $('#CommaDelimitedSubjects').val(CommaDelimitedSubjects);
            }
            else {
                subjectsListHtml = tablePrefix + "<td></td> " + tableSuffix;
                $('#subjectData').html(subjectsListHtml);
            }
        }
    });

});
