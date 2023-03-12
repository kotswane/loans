//function fnShowSignaturePopup(ImgID)
//{
//    $("#signature-pad").css("display", "block");
//}

function fnShowSignaturePopup(divID) {
    //resizeCanvas();
    //signaturePad.clear();
    //fnGetSingature();
    //alert('df');
    //debugger
    $("#hdnSelectedDivId").val(divID);
    $('#divSignPopup').modal('show');
}
$(document).ready(function () {
    //debugger
   // $("#divSignPopup").css("display", "none !important");

    $("#divSignPopup").css("display", "");
    //drawImage();
    //signaturePad.toDataURL()
});

function drawImage() {
    var ctx = $("canvas")[0].getContext("2d"),
        img = new Image();

    img.onload = function () {
        ctx.drawImage(img, 0, 0, 664, 320);
       // $("span").text("Loaded.");
    };
    img.src = "../Content/Images/signature.png";
   // $("span").text("Loading...");
}

function fnSavedocu() {
    var result = encodeURIComponent($("#divActualDoc").html());
    var docId = $("#hdnSelectedDocId").val();
    var stampNumber = "";
    if (docId == "2" || docId == "5")
    {
        stampNumber = $("#txtStampNumber").val();
        //alert(stampNumber);
    }
    //result = "ABCD";
    $.ajax({
        type: "POST",
        url: "/Documents/GeneratePDFFrmJ/",
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: '{"strHTM": "' + result + '","iDocId": "' + docId + '","strStampNumber": "' + stampNumber + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#divLoadIcon").css("display", "none");
            $("#hdnShowSuccessMsg").val("true");
            $("#hdnShowSuccessMsgDesc").val("Document uploaded successfully");
            fnDisplaySuccessMessage();
            //window.location.href = "/dashboard";
           

        }
    });
}


function fnCompletedDocument()
{
    document.body.style.overflow = "hidden";
    $("#divLoadDisable").css("display", "block");
    $("#divLoadIcon").css("display", "block");
    var docId = $("#hdnSelectedDocId").val();
    if(docId ==4)
    {
        if ($("#imgDocSIgn")[0] == undefined) {
            alert("Please sign the document before you proceed with Finish");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            var imgSrc = $("#imgDocSIgn")[0].src;
            if (imgSrc == "./Content/Images/Signin.png") {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                fnSavedocu();
                //$("#divLoadIcon").css("display", "none");
            }
        }
    }
    else if(docId==1)
    {
        //**********chkeck if used entered name and fspname
        if ($("#txtName").val() != "" && $("#txtFSPName").val()!="")
        {
            if ($("#imgDocSIgn")[0] == undefined) {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                var imgSrc = $("#imgDocSIgn")[0].src;
                if (imgSrc == "../Content/Images/Signin.png") {
                    alert("Please sign the document before you proceed with Finish");
                    $("#divLoadIcon").css("display", "none");
                    $("#divLoadDisable").css("display", "none");
                }
                else {
                    fnSavedocu();
                    //$("#divLoadIcon").css("display", "none");
                }
            }
        }
        else
        {
            alert("Please enter name and FSPName");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        
        
    }
    else if (docId == 2) {
        if ($("#txtStampNumber").val() != "" && $("#txtOperatorUSN").val() != "") {
            if ($("#txtOperatorUSN").val().toString().match(/[a-z]/i))
            {
                alert("Operator USN Contains invalid characters.");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
                return;
            }
            if ($("#imgDocSIgn")[0] == undefined) {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                var imgSrc = $("#imgDocSIgn")[0].src;
                if (imgSrc == "./Content/Images/Signin.png") {
                    alert("Please sign the document before you proceed with Finish");
                    $("#divLoadIcon").css("display", "none");
                    $("#divLoadDisable").css("display", "none");
                }
                else {
                    fnSavedocu();
                    //$("#divLoadIcon").css("display", "none");
                }
            }
        }
        else
        {
            alert("Please enter stampnumber and operator USN");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
    }
    else if (docId == 3) { //supervision document
        //debugger
        //alert('HELOW');

        if ($("#imgDocSIgn")[0] == undefined) {
            alert("Please sign the document before you proceed with Finish");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            var imgSrc = $("#imgDocSIgn")[0].src;
            if (imgSrc == "./Content/Images/Signin.png") {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                fnSavedocu();
                //$("#divLoadIcon").css("display", "none");
            }
        }
    }
    else if (docId == 5) {
        if ($("#txtStampNumber").val() != "" && $("#txtOperatorUSN").val() != "") {
            if ($("#txtOperatorUSN").val().toString().match(/[a-z]/i)) {
                alert("Operator USN Contains invalid characters.");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
                return;
            }
            if ($("#imgDocSIgn")[0] == undefined) {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                var imgSrc = $("#imgDocSIgn")[0].src;
                if (imgSrc == "./Content/Images/Signin.png") {
                    alert("Please sign the document before you proceed with Finish");
                    $("#divLoadIcon").css("display", "none");
                    $("#divLoadDisable").css("display", "none");
                }
                else {
                    fnSavedocu();
                    //$("#divLoadIcon").css("display", "none");
                }
            }
        }
        else {
            alert("Please enter stampnumber and operator USN");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
    }
    else if (docId == 6) {
        if ($("#txtHomeAddress").val() != "" && $("#txtPosition").val() != "") {
         
            if ($("#imgDocSIgn")[0] == undefined) {
                alert("Please sign the document before you proceed with Finish");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }
            else {
                var imgSrc = $("#imgDocSIgn")[0].src;
                if (imgSrc == "./Content/Images/Signin.png") {
                    alert("Please sign the document before you proceed with Finish");
                    $("#divLoadIcon").css("display", "none");
                    $("#divLoadDisable").css("display", "none");
                }
                else {
                    fnSavedocu();
                    //$("#divLoadIcon").css("display", "none");
                }
            }
        }
        else {
            alert("Please enter home address and position");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
    }
}