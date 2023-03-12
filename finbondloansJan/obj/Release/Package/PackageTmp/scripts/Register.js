
$(function () {

    $("#txtdateofbirth").datepicker({ dateFormat: 'yy/mm/dd', changeYear: true,changeMonth:true, yearRange: '-80:+0' });
    var myPlugin = $("[id$='txtPassword']").password_strength();

    $('.input_class_checkbox').each(function () {
        $(this).hide().after('<div class="class_checkbox" />');

    });


    $('.input_class_checkbox_address').each(function () {
        $(this).hide().after('<div class="class_checkbox" onclick="fnPostalAddSameAsResAdd();" />');

    });

    $('.class_checkbox').on('click', function () {
        $(this).toggleClass('checked').prev().prop('checked', $(this).is('.checked'))
    });
    $(".clsContactWhitediv").css('background-color','#F8F8F8');
});

function ValidateRegister()
{
    //debugger
    if ($("#chkCreditCheck").is(':checked') == false)
    {
        fnDisplayErrorMessageFromJS("Please accept the credit check");
        $("#divCreditCheck").css("display", "block");
        return false;
    }
    else
    {
        $("#divCreditCheck").css("display", "none");
        fnRemoveErrorMessageFromJS();
        return true;
    }
    
}


function fnPopulateDateOfBirth() {
   // if (Page_IsValid) {

        var idno = $("#txtIDNumber").val();
        var year = parseInt($("#txtIDNumber").val().substring(0, 2));
        if (year > 40)
            year = "19" + year;
        else
            year = "20" + year;
        var month = $("#txtIDNumber").val().substring(2, 4);
        var day = $("#txtIDNumber").val().substring(4, 6);
        $("#txtdateofbirth").val("" + year + "/" + month + "/" + day + "");
   // }
}

function fnPostalAddSameAsResAdd() {
    
   // debugger
   // alert($("#ddlPostalCountry").val());
    $("#chkAddress").toggleClass('checked').prev().prop('checked', $("#chkAddress").is('.checked'))
    if ($("#chkAddress").is(':checked') == false) {
        $("#txtPostalStreet").val($("#txtResStreet").val());
        $("#txtPostalSuburb").val($("#txtResSuburb").val());
        $("#ddlPostalCountry").val($("#ddlCountry").val());
        $("#ddlPostalProvince").val($("#ddlProvince").val());
        $("#txtPostalCode").val($("#txtResCode").val());
    }
    else {

        $("#txtPostalStreet").val("");
        $("#txtPostalSuburb").val("");
        //$("#ddlPostalCountry").val("");
        //$("#ddlPostalProvince").val("");
        $("#txtPostalCode").val("");

    }

}