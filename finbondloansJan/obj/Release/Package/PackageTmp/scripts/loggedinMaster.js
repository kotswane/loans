
function fnDisplayMobiIcon() {
    $("#divMobiMenus").fadeIn();
    $("#divMobiMenus").removeClass("clsMobi");
    //  $("#divMobiMenuIcon").removeClass("clsMobi");
    $("#divMobiMenuIcon").css("display", "none");
    $("#divMobiMenuClose").css("display", "block");
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