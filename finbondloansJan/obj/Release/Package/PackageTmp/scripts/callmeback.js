function fnSaveCallMeBack()
{
    
    $.ajax({
        type: "POST",
        url: "/Home/SaveCallMeBack/",
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: '{"strTitle": "' + $("#drpContactTitle").val() + '","strFullName": "' + $("#txtfooName").val() + '","strPhone": "' + $("#txtfooMobileNumber").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            window.scrollTo(0, 0);
            fnDisplaySuccessMessageFromJS("Call me back request sent successfully");
            //$("#divLoadIcon").css("display", "none");
            //if (msg != "") {
            //    if (msg == "Success") {
            //        window.location.href = "/LoanCompletion/index";
            //    }
            //    else {
            //        fnDisplayErrorMessageFromJS(msg);
            //        $("#divLoadIcon").css("display", "none");
            //        $("#divLoadDisable").css("display", "none");
            //    }
            //}
            //else {
            //    fnDisplayErrorMessageFromJS("Technical Problem, Please try again later - " + msg + "");
            //    $("#divLoadIcon").css("display", "none");
            //    $("#divLoadDisable").css("display", "none");
            //}

            //$("#hdnShowSuccessMsg").val("true");
            //$("#hdnShowSuccessMsgDesc").val("Document uploaded successfully");
            //fnDisplaySuccessMessage();
            //window.location.href = "/dashboard";


        }
    });
}