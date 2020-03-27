
$(function () {

    // Submit intersection
    $('#newAnswerForm').submit(function () {
        $("#dlgUpdatePage").show().dialog("open");
        return true;  // <- false will cancel submit
    });

    // Accept Answer
    $(".Accept").click(function () {
        var form = $('#newAnswerForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        // Get the id from the link
        var recordToUpdate = $(this).attr("data-id");
        if (recordToUpdate != '') {
            $("#dlgUpdatePage").show().dialog("open");
            $.post("/Answer/AcceptAnswer", { __RequestVerificationToken: token, "id": recordToUpdate },
                function (data) {
                    location = data.url;
                });
        }
    });

    // Decline Answer
    $(".Decline").click(function () {
        var form = $('#newAnswerForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        // Get the id from the link
        var recordToUpdate = $(this).attr("data-id");
        if (recordToUpdate != '') {
            $("#dlgUpdatePage").show().dialog("open");
            $.post("/Answer/DeclineAnswer", { __RequestVerificationToken: token, "id": recordToUpdate },
                function (data) {
                    location = data.url;
                });
        }
    });

    // Add Question Comment
    $(".AddQuestionComment").click(function () {
        var form = $('#newAnswerForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        // Get the id from the link
        var recordToUpdate = $(this).attr("data-id");
        var comment = $('#txtQuestionComment').val();
        if (recordToUpdate != '' && comment != '') {
            $("#dlgUpdatePage").show().dialog("open");
            $.post("/NewQuestion/AddQuestionComment", { __RequestVerificationToken: token, "questionId": recordToUpdate, "comment": comment },
                function (data) {
                    location = data.url;
                });
        }
    });

    // Add Answer Comment
    $(".AddAnswerComment").click(function () {
        var form = $('#newAnswerForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        // Get the id from the link
        var recordToUpdate = $(this).attr("data-id");
        var comment = $('#answer-comment-' + recordToUpdate).val();
        if (recordToUpdate != '' && comment != '') {
            $("#dlgUpdatePage").show().dialog("open");
            $.post("/Answer/AddAnswerComment", { __RequestVerificationToken: token, "answerId": recordToUpdate, "comment": comment },
                function (data) {
                    location = data.url;
                });
        }
    });
});

