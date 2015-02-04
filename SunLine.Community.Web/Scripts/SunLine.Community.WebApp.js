SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.WebApp = SunLine.Community.WebApp || {};

var webApp;
SunLine.Community.WebApp = function () {

    /* Method which show action confirmation message. */
    var showMessage = function (wasSuccessful, title, message) {

        var opts = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        if (wasSuccessful) {
            toastr.success(message, title, opts);
        } else {
            toastr.error(message, title, opts);
        }

    };

    var showPageLoader = function() {
    	$('.page-loader').show();
    };

    var hidePageLoader = function() {
    	$('.page-loader').hide();
    };

    return {
        ShowMessage: showMessage,
        ShowPageLoader: showPageLoader,
        HidePageLoader: hidePageLoader
    };
};

/* Initialize WebApp when page loads */
$(function() {
    webApp = SunLine.Community.WebApp();
});