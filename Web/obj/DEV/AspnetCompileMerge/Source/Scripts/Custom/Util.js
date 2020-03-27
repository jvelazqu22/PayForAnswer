(function ($) {
    LoadHtmlContent = function (contentUrl, divName) {
        $("#"+divName).html('<object data=' + contentUrl + '>');
    };
})(jQuery);

(function ($) {
    LoadHtmlContentInTable = function (contentUrl, divName) {
        $("#" + divName).html('<object data=' + contentUrl + '>');
    };
})(jQuery);

