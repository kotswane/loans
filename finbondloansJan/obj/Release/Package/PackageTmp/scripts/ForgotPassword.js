$(function () {

    $(".clsContactWhitediv").css('background-color', '#F8F8F8');
});

function displayForgotPasswordOTPPopup(data) {
    if (data == "OTP sent") {
        $("#divNegativeDiv").hide();

        $("#btnValidateOTP").show();
        $("#divValidateOTPError").hide();


        $("#hfIDNumber").val($("#txtIDNumber").val());

        $("#txtOTP").val("");
        $("#divLoadDisable").css("display", "block");
        $('#divForgotPasswordOTPPopup').modal('show');
    }
    else {
        $("#divNegativeDiv").fadeIn();
        $("#divNegativeText").html(data);
    }
}

function handleOTPValidationResult(data) {
    if (data == "Success") {
        $('#divForgotPasswordOTPPopup').modal('hide');

        $("#divError").hide();
        clearForgotPasswordOTPBackground();

        $("#txtIDNumber").val("");


        $("#divSuccessDiv").fadeIn();
        $("#divSuccessText").html("The link to reset your password has been emailed to you.");
    }
    else if (data == "Error, failed attempt limit exceeded") {
        //$("#btnValidateOTP").attr("disabled", true);
        $("#divNegativeDiv").fadeIn();
        $("#divNegativeText").html(data);

        $('#divForgotPasswordOTPPopup').modal('hide');
        $("#divLoadDisable").css("display", "none");
    }
    else if (data == "Error, incorrect OTP") {
        $("#divValidateOTPError").fadeIn();
        $("#divValidateOTPErrorText").html(data);
        $("#txtOTP").val("");
        $("#divForgotPasswordOTPPopup").css("height", "245px");
    }
}

function clearForgotPasswordOTPBackground() {
    $("#divLoadDisable").css("display", "none");
}