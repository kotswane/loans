$(window).bind('beforeunload', function (event) {
    InitializeRequest();
});
var link = 0;
$(document).click(function (event) {
    //alert('he');
    if ($(event.target).hasClass("clsDownloadCourseButton")) {
        
        var couseId = $(event.target)[0].id;
        //$("#btnstartCourseDsiable_" + couseId).css("display", "none");
        //$("#btnstartCourse_" + couseId).css("display", "block");
        $("#" + couseId).css("display", "none");
        //$("#downloadDisabled_" + couseId).css("display", "block");
        //var timer = setTimeout(function () {
        //    //window.location = 'http://example.com'
        //    //alert('ff');
        //    window.location.href = getBaseUrl() + "/home";
        //}, 3000);
        //alert('hasclass');
    }
    if ($(event.target).hasClass("download")) {
        link = 1;
    }
    else {
        link = 0;
    }

});


function InitializeRequest(sender, args) {

    try {

        if (link == 0) {
            ShowWait();
        }
        else {
            Sys.WebForms.PageRequestManager.getInstance().remove_initializeRequest(InitializeRequest);//prevents multipul slideups
        }
    }
    catch (err) {
    }
}
function ShowWait() {

    $(window).scrollTop(0);
    document.body.style.overflow = "hidden";
    $("#divLoadDisable").css("display", "block");
    $("#divLoadIcon").css("display", "block");

}
function EndRequest() {

    $("#divLoadDisable").css("display", "none");
    $("#divLoadIcon").css("display", "none");
    document.body.style.overflow = "visible";
    // aunchIntoFullscreen(document.documentElement);
}