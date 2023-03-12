$(document).ready(function () {
  

    var pageURL = window.location;
    //alert(pageURL);
    $("#divHomeMenu").removeClass("clsSelectedMenuColor");
    $("#divHowToApply").removeClass("clsSelectedMenuColor");
    $("#divFAQ").removeClass("clsSelectedMenuColor");
    $("#divContactUS").removeClass("clsSelectedMenuColor");
    $("#divLogin").removeClass("clsSelectedMenuColor");
    if (pageURL.toString().indexOf('home') > 0) {
        $("#divHomeMenu").addClass("clsSelectedMenuColor");
    }
    else if (pageURL.toString().indexOf('howitworks') > 0) {
        $("#divHowToApply").addClass("clsSelectedMenuColor");
    }
    else if (pageURL.toString().indexOf('faq') > 0) {
        $("#divFAQ").addClass("clsSelectedMenuColor");
    }
    else if (pageURL.toString().indexOf('contactus') > 0) {
        $("#divContactUS").addClass("clsSelectedMenuColor");
    }
    else if (pageURL.toString().indexOf('login') > 0) {
        $("#divLogin").addClass("clsSelectedMenuColor");
    }


});

function fnDisplayMobiIcon() {
    $("#divMobiMenus").fadeIn();
    $("#divMobiMenus").removeClass("clsMobi");
    //  $("#divMobiMenuIcon").removeClass("clsMobi");
    $("#divMobiMenuIcon").css("display","none");
    $("#divMobiMenuClose").css("display","block");
    //$("#divMobiMenuIcon").fadeOut();
    //$("#divMobiMenuClose").fadeIn();
}
function fnHideMobiIcon() {
    $("#divMobiMenus").fadeOut();
    $("#divMobiMenus").removeClass("clsMobi");
    //$("#divMobiMenuClose").removeClass("clsMobi");
    //$("#divMobiMenuIcon").fadeIn();
    //$("#divMobiMenuClose").fadeOut();
    $("#divMobiMenuIcon").css("display", "block");
    $("#divMobiMenuClose").css("display", "none");
}