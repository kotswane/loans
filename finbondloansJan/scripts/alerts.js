var dhdnShowSuccess;
var dhdnShowSuccessMsgDesc;
var dhdnShowErrorMsg;
var dhdnShowErrorMsgDesc;

//var dhdnContextShowSuccess;
//var dhdnContextShowSuccessMsgDesc;
//var dhdnContextShowErrorMsg;
//var dhdnContextShowErrorMsgDesc;
$(document).ready(function () {
    dhdnShowSuccess = $("#hdnShowSuccessMsg").val();
    dhdnShowSuccessMsgDesc = $("#hdnShowSuccessMsgDesc").val();

    dhdnShowErrorMsg = $("#hdnShowErrorMsg").val();
    dhdnShowErrorMsgDesc = $("#hdnShowErrorMsgDesc").val();

    //dhdnContextShowSuccess = $("#hdnContextShowSuccessMsg").val();
    //dhdnContextShowSuccessMsgDesc = $("#hdnContextShowSuccessMsgDesc").val();

    //dhdnContextShowErrorMsg = $("#hdnContextShowErrorMsg").val();
    //dhdnContextShowErrorMsgDesc = $("#hdnContextShowErrorMsgDesc").val();

    if (dhdnShowSuccess == 'True') {
        fnDisplaySuccessMessage();
        fnIncreaseButtonMargin();
    }
    else {
        $("#divSuccessDiv").fadeOut();
    }

    if (dhdnShowErrorMsg == 'True') {
        fnDisplayErrorMessage();
        fnIncreaseButtonMargin();
    }
    else {
        $("#divNegativeDiv").fadeOut();
    }

    //if (dhdnContextShowSuccess == 'True') {
    //    fnDisplayContextSuccessMessage();
    //}
    //else {
    //    $("#divContextSuccessDiv").fadeOut();
    //}

    //if (dhdnContextShowErrorMsg == 'True') {
    //    fnDisplayContextErrorMessage();
    //}
    //else {
    //    $("#divContextNegativeDiv").fadeOut();
    //}
});

function fnDisplaySuccessMessage() {
    $("#divSuccessDiv").fadeIn().css("display", "inline-block");;
    setTimeout(function () { $("#divSuccessDiv").fadeOut(); }, 15000);
    setTimeout(function () { fnDecreaseButtonMargin(); }, 15000);
    // alert(hdnShowSuccessMsgDesc);
    $("#divSuccessText").text(dhdnShowSuccessMsgDesc);
}

function fnDisplayErrorMessage() {
    $("#divNegativeDiv").fadeIn().css("display", "inline-block");;
    //setTimeout(function () { $("#divNegativeDiv").fadeOut(); }, 15000);
    //setTimeout(function () { fnDecreaseButtonMargin(); }, 15000);
    $("#divNegativeText").html(dhdnShowErrorMsgDesc);
    //fnDecreaseButtonMargin();
}

function fnDisplayContextSuccessMessage() {
    $("#divContextSuccessDiv").fadeIn();
    setTimeout(function () { $("#divContextSuccessDiv").fadeOut(); }, 15000);
    setTimeout(function () { fnDecreaseButtonMargin(); }, 15000);
    $("#divContextSuccessText").text(dhdnContextShowSuccessMsgDesc);
}

function fnDisplayContextErrorMessage() {
    // alert(dhdnContextShowErrorMsgDesc);
    $("#divContextNegativeDiv").fadeIn();
    //setTimeout(function () { $("#divContextNegativeDiv").fadeOut(); }, 15000);
    //setTimeout(function () { fnDecreaseButtonMargin(); }, 15000);
    $("#divContextNegativeText").html(dhdnContextShowErrorMsgDesc);
    //fnDecreaseButtonMargin();
    //   alert('kk');
}

function fnDisplayErrorMessageFromJS(strError) {
    $("#divNegativeDiv").fadeIn().css("display", "inline-block");;
    $("#divNegativeText").html(strError);
    $(".clsContainerForHeaderButtons").css("padding-top", '45px');
}
function fnDisplaySuccessMessageFromJS(strError) {
    $("#divSuccessDiv").fadeIn().css("display", "inline-block");;
    $("#divSuccessText").html(strError);
    $(".clsContainerForHeaderButtons").css("padding-top", '45px');
}

function fnRemoveErrorMessageFromJS() {
    $("#divNegativeDiv").fadeOut();
    $(".clsContainerForHeaderButtons").css("padding-top", '0px');
}

function fnIncreaseButtonMargin() {
    $(".clsCustomHeaderButton1").css('margin-top', '45px');
    $(".clsCustomHeaderButton2").css('margin-top', '45px');
}

function fnDecreaseButtonMargin() {
    $(".clsCustomHeaderButton1").css('margin-top', '0px');
    $(".clsCustomHeaderButton2").css('margin-top', '0px');
}


function formatMoney(amt) {
   // debugger
  //  var amt = $(sender).val();
    var dotIndex = amt.toString().indexOf(".");
    amt = amt.toString();
    var stringLength = $.trim(amt).length;
    //var stringLength = amt.toString().trim().length;
    if (dotIndex == -1)
        amt = amt + ".00";
    else if (stringLength - dotIndex == 2)
        amt = amt + "0";

    if (amt.toString().indexOf("R ") == -1)
        return "R " + amt;
    else
        return amt;
}

function unformatMoney(sender) {
    if ($(sender).val() == "")
        return;

    var valTmp = $(sender).val().toLowerCase().replace('r', '').trim();
    var val = "";
    for (var i = 0; i < valTmp.length; i++) {
        if (valTmp[i].trim().length != 0)
            val = val + valTmp[i];
    }

    var value = parseFloat(val);
    $(sender).val(value);
    if (value <= 0)
        $(sender).val('');
}

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