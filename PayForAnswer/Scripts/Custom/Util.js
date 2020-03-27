(function ($) {
    LoadHtmlContent = function (contentUrl, divName) {
        $("#"+divName).html('<object data=' + contentUrl + '>');
    };
})(jQuery);

(function ($) {
    LoadHtmlContentInTable = function (contentUrl, divName) {
        //var tableStart = "<table>";
        //var tableStart = "<table class=\"table table-bordered table-striped\">";
        //var tableEnd = "</table>";
        //var content = '<object data=' + contentUrl + '>'
        //var html = tableStart + content + tableEnd;

        //$("#" + divName).html(contentUrl);
        $("#" + divName).html('<object data=' + contentUrl + '>');
    };
})(jQuery);

