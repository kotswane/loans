function fnShowHidePasswordText() {
   // alert('df');
    var d = document;
    var tag = d.getElementById("txtPassword");
    var tag2 = d.getElementById("showhide");
   
    var passImg = $("#showhide").css("background-image");
    //alert(passImg.toString().indexOf('show_password.png'));
    if (passImg.toString().indexOf('show_password.png') >= 0) {
        $("#showhide").removeClass("btnShowPassword");
        $("#showhide").addClass("btnHidePassword");
        
        tag.setAttribute('type', 'password');
    } else {
        $("#showhide").removeClass("btnHidePassword");
        $("#showhide").addClass("btnShowPassword");
        
        tag.setAttribute('type', 'text');
    }
}
$(document).ready(function () {
    //$("#txtPassword").focus(function () {

    //    $("#showhide").removeClass("btnPasswordMargin");
    //    //$(this).animate({
    //    //    "margin-right": "30px", //g
    //    //});
    //    //$("#showhide").re("margin-right","=50px !important");
    //});
    //$("#txtPassword").focusout(function () {
    //    $("#showhide").addClass("btnPasswordMargin");
    //    return true;
    //});
});