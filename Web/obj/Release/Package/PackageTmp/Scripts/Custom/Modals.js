
$(function () {
    $("#dlgUpdatePage").dialog({
        modal: true,
        minWidth: 400,
        width: 400,
        maxWidth: 400,
        title: '',
        overlay: { opacity: 0.5, background: "black" },
        show: "slide",
        hide: "slide",
        autoResize: true,
        autoOpen: false,
        closeOnEscape: false,
        dialogClass: "dlg-no-title"
    });
    $("#dlgPost").dialog({
        modal: true,
        minWidth: 400,
        width: 400,
        maxWidth: 400,
        title: '',
        overlay: { opacity: 0.5, background: "black" },
        show: "slide",
        hide: "slide",
        autoResize: true,
        autoOpen: false,
        closeOnEscape: false,
        dialogClass: "dlg-no-title"
    });
});
