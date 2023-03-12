function fnHideAndShowTabs(sideMenu, menu) {
    if (menu == "divLoanOptions") {
        $('#divLoanOptions').removeAttr('style');
    }

    if (menu == "divViewContract") {
        $('#divViewContract').removeAttr('style');
    }

    $("#divMenu1").fadeOut();
    $("#divExpensestab").fadeOut();
    $("#divEmploymentDetails").fadeOut();
    $("#divBankDetails").fadeOut();
    $("#divLoanOptions").fadeOut();
    $("#divDocuments").fadeOut();
    $("#divViewContract").fadeOut();
    $("#divConfirmation").fadeOut();
    $("#divSideMenu1").removeClass("clsHighlightMenus");
    $("#divSideMenu2").removeClass("clsHighlightMenus");
    $("#divSideMenu3").removeClass("clsHighlightMenus");
    $("#divSideMenu4").removeClass("clsHighlightMenus");
    $("#divSideMenu5").removeClass("clsHighlightMenus");
    $("#divSideMenu6").removeClass("clsHighlightMenus");
    $("#divSideMenu7").removeClass("clsHighlightMenus");
    $("#divSideMenu8").removeClass("clsHighlightMenus");
    $("#" + sideMenu).addClass("clsHighlightMenus");
    $("#" + menu).delay("slow").fadeIn();

}
function getBaseUrl() {
    var re = new RegExp(/^.*\/\/[^\/]+/);

    if ($(location).attr('href').toString().toLowerCase().indexOf('cellc') > 0) {
        return re.exec(window.location.href) + "/cellc";
    }
    else {
        return re.exec(window.location.href);

    }
}

function fnContractNext() {
    fnRemoveErrorMessageFromJS();
    document.body.style.overflow = "hidden";
    $("#divLoadDisable").css("display", "block");
    $("#divLoadIcon").css("display", "block");
    var signdoc1 = false;
    var signdoc2 = false;
    var signdoc3 = false;
    var signdoc4 = false;
    var signdoc5 = false;

    if ($('#divStaffLoan2').children('img')[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document(Consumer/Employee) section before you proceed with Finish");
        document.body.style.overflow = "auto";
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $('#divStaffLoan2').children('img')[0].src;
        if (imgSrc.toString().indexOf('/Content/Images/Signin.png') > 0) {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the document(Consumer/Employee) section before you proceed with Finish");
            document.body.style.overflow = "auto";
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            signdoc1 = true;
            //fnHideAndShowTabs('divSideMenu6', 'divConfirmation');
        }
    }

    //**************second signature
    if ($('#divStaffLoan4').children('img')[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document(Consumer/Employee) section before you proceed with Finish");
        document.body.style.overflow = "auto";
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $('#divStaffLoan4').children('img')[0].src;
        if (imgSrc.toString().indexOf('/Content/Images/Signin.png') > 0) {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the document(Consumer/Employee) section before you proceed with Finish");
            document.body.style.overflow = "auto";
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            signdoc2 = true;
            //fnHideAndShowTabs('divSideMenu6', 'divConfirmation');
        }
    }

    //**************third signature
    if ($('#divStaffLoan6').children('img')[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
        document.body.style.overflow = "auto";
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $('#divStaffLoan6').children('img')[0].src;
        if (imgSrc.toString().indexOf('/Content/Images/Signin.png') > 0) {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
            document.body.style.overflow = "auto";
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            signdoc3 = true;
            //fnHideAndShowTabs('divSideMenu6', 'divConfirmation');
        }
    }

    // //**************fourth signature
    if ($('#divStaffLoan7').children('img')[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
        document.body.style.overflow = "auto";
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $('#divStaffLoan7').children('img')[0].src;
        if (imgSrc.toString().indexOf('/Content/Images/Signin.png') > 0) {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
            document.body.style.overflow = "auto";
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            signdoc4 = true;
            //fnHideAndShowTabs('divSideMenu6', 'divConfirmation');
        }
    }

    // //**************FIFTH signature
    if ($('#divStaffLoan9').children('img')[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
        document.body.style.overflow = "auto";
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $('#divStaffLoan9').children('img')[0].src;
        if (imgSrc.toString().indexOf('/Content/Images/Signin.png') > 0) {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the document Consumer section before you proceed with Finish");
            document.body.style.overflow = "auto";
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            signdoc5 = true;
            //fnHideAndShowTabs('divSideMenu6', 'divConfirmation');
        }
    }

    if (signdoc1 == true && signdoc2 == true && signdoc3 == true && signdoc4 == true && signdoc5 == true) {
        $("#lblSalary").text($("#txtLatestSalary").val());
        $("#lblGroceries").text($("#txtGroceries").val());
        $("#lblTransport").text($("#txtTransport").val());
        $("#lblHousing").text($("#txtHousing").val());
        $("#lblClothing").text($("#txtClothing").val());
        $("#lblMedical").text($("#txtMedical").val());
        $("#lblElectricity").text($("#txtElectricity").val());
        $("#lblChildM").text($("#txtChildM").val());
        $("#lblEducation").text($("#txtEducation").val());
        $("#lblBankCharges").text($("#txtBankCharges").val());
        $("#lblOther").text($("#txtOther").val());

        //*********************loan summary**************************
        $("#lblTerm").text($("#divILoanTerm").text());
        $("#lblLoanOption").text($("#divIDesc").text());

        $("#lblCapital").text($("#divICapital").text());





        $("#lblIntitationFee").text($("#divIInitiationFee").text());
        $("#lblInstallmentAmt").text($("#divIInstallmentAmt").text());
        $("#lblServiceFee").text($("#divIServiceFee").text());
        //$("#lblTotalRepayment").text($("#txtOther").text());
        $("#lblFirstPayDate").text($("#divIFirstPayDate").text());
        $("#lblLastPayDate").text($("#divILastPayDate").text());
        $("#lnlPrincipalDebt").text($("#divIPrincipalDebt").text());
        $("#lblTotalRepayment").text(formatMoney($("#hdnTotalRepayment").val()));

        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
        document.body.style.overflow = "auto";

        fnHideAndShowTabs('divSideMenu8', 'divConfirmation');
    }
}

function fnFinishLoanRequest() {
    //****************************check user is signed the document******************************
    fnRemoveErrorMessageFromJS();
    document.body.style.overflow = "hidden";
    $("#divLoadDisable").css("display", "block");
    $("#divLoadIcon").css("display", "block");

    if ($("#imgDocSIgn")[0] == undefined) {
        //alert("Please sign the document before you proceed with Finish");
        fnDisplayErrorMessageFromJS("Please sign the document before you proceed with Finish");
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }
    else {
        var imgSrc = $("#imgDocSIgn")[0].src;
        if (imgSrc == "../Content/Images/Signin.png") {
            //alert("Please sign the document before you proceed with Finish");
            fnDisplayErrorMessageFromJS("Please sign the documents before you proceed with Finish");
            $("#divLoadIcon").css("display", "none");
            $("#divLoadDisable").css("display", "none");
        }
        else {
            fnSavedocu();
            //$("#divLoadIcon").css("display", "none");
        }
    }

}

function fnSavedocu() {

    var result = encodeURIComponent($("#divActualDoc").html());
    $.ajax({
        type: "POST",
        url: getBaseUrl() + "/MyAccount/CompleteLoanProcess/",
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: '{"strHTM": "' + result + '","iOfferID": "' + $("#hdnSelectedOfferID").val() + '","loanAmount": "' + $("#divICapital").text() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#divLoadIcon").css("display", "none");
            debugger
            if (msg != "") {
                if (msg == "Success") {
                    window.location.href = getBaseUrl() + "/LoanCompletion/index";
                }
                else {
                    fnDisplayErrorMessageFromJS(msg);
                    $("#divLoadIcon").css("display", "none");
                    $("#divLoadDisable").css("display", "none");
                }
            }
            else {
                fnDisplayErrorMessageFromJS("Technical Problem, Please try again later - " + msg + "");
                $("#divLoadIcon").css("display", "none");
                $("#divLoadDisable").css("display", "none");
            }

            //$("#hdnShowSuccessMsg").val("true");
            //$("#hdnShowSuccessMsgDesc").val("Document uploaded successfully");
            //fnDisplaySuccessMessage();
            //window.location.href = "/dashboard";


        }
    });
}
function fnUploadDocumentNext() {
    $("#divLoadIcon").css("display", "block");
    $("#divLoadDisable").css("display", "block");
    //alert($("#lblIDDocName").text);

    var uploadedDocCnt = 0;
    var actualDocCount = $("label[class*='lblDocIdentifier']").length;
    $("label[class*='lblDocIdentifier']").each(function (item) {
        if ($(this)[0].innerText != "") {
            uploadedDocCnt = parseInt(uploadedDocCnt) + 1;
        }
    });
    if (parseInt($("#hdnExpectedNumberOfDocumentUploads").val()) > parseInt(uploadedDocCnt)) {
        fnDisplayErrorMessageFromJS("Please upload all the documents");
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
        return;
    }

    //if ($("#lblIDDocName_4").text().trim() == "" || $("#lblIDDocName_2").text().trim() == "" || $("#lblIDDocName_3").text().trim() == "" || $("#lblSelfiDocName").text().trim() == "") {
    //    fnDisplayErrorMessageFromJS("Please upload all the documents");
    //    $("#divLoadIcon").css("display", "none");
    //    $("#divLoadDisable").css("display", "none");
    //    return;
    //}
    fnRemoveErrorMessageFromJS();
    fnHideAndShowTabs('divSideMenu7', 'divViewContract');
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}
function fnLoanOptionNext() {
   // alert(getBaseUrl());
    $("#divLoadIcon").css("display", "block");
    $("#divLoadDisable").css("display", "block");
    if ($("#hdnSelectedLaonId").val() == "" || $("#hdnSelectedOfferID").val() == "") {
        fnDisplayErrorMessageFromJS("Please select loan option");
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
        return; //uncomment later
    }
    //$("#hdnCREDITAMOUNT").val(4000);
    //***********************update some of the loan fields in the view contract document**************

    var contract = $("#divActualDoc").html();
    contract = contract.toString().replace('[LOANOPT]', $("#hdnLoanOption").val());
    contract = contract.toString().replace('[CAPITALAMT]', formatMoney($("#hdnCREDITAMOUNT").val()));
    contract = contract.toString().replace('[LOANTERM]', $("#hdnNOOFINSTALLMENTS").val());
    contract = contract.toString().replace('[PRINCIPALDEBT]', formatMoney($("#hdnPrincipalDebt").val()));
    contract = contract.toString().replace('[INITIATIONFEE]', formatMoney($("#hdnInitiationFee").val()));
    contract = contract.toString().replace('[SERVICEFEE]', formatMoney($("#hdnServiceFee").val()));
    //contract = contract.toString().replace('[INTERESTAMT]', $("#hdnCREDITAMOUNT").val());
    //contract = contract.toString().replace('[CAPITALINTEREST]', $("#hdnCREDITAMOUNT").val());
    contract = contract.toString().replace('[TOTALREPAYMENT]', formatMoney($("#hdnTotalRepayment").val()));
    contract = contract.toString().replace('[FIRSTPAYDATE]', $("#hdnFirstPayDate").val());
    contract = contract.toString().replace('[LASTPAYDATE]', $("#hdnLastPayDate").val());


    contract = contract.toString().replace('[CREDITCOSTMULTIPLE]', parseFloat($("#hdnCreditCostMultiple").val()).toFixed(2));
    //contract = contract.toString().replace('[CREDITCOSTMULTIPLE]', formatMoney(parseFloat($("#hdnCreditCostMultiple").val()).toFixed(2)));
    contract = contract.toString().replace('[INSTALLMENTAMT]', formatMoney($("#hdnINSTALLMENTAMOUNT").val()));

    contract = contract.toString().replace('[INTERESTRATE]', $("#hdnInterestRate").val() + "%");

    //contract = contract.toString().replace('[INTERESTRATE]', formatMoney($("#hdnInterestRate").val()));
    contract = contract.toString().replace('[VAT]', formatMoney($("#hdnVAT").val()));
    contract = contract.toString().replace('[INTERESTAMT]', formatMoney($("#hdnInterestAmt").val()));
    contract = contract.toString().replace('[CAPITALINTEREST]', formatMoney($("#hdnTOTALWITHINTEREST").val()));

    contract = contract.toString().replace('[FIRSTPAYDATE]', $("#hdnFirstPayDate").val());
    contract = contract.toString().replace('[INSTALLMENTAMT]', formatMoney($("#hdnINSTALLMENTAMOUNT").val()));
    contract = contract.toString().replace('[LOANTERM]', $("#hdnNOOFINSTALLMENTS").val());
    //contract = contract.toString().replace('[CREDITAMOUNT]', $("#hdnCREDITAMOUNT").val());
    //contract = contract.toString().replace('[CREDITAMOUNT]', $("#hdnCREDITAMOUNT").val());
    //contract = contract.toString().replace('[NOOFINSTALLMENTS]', $("#hdnNOOFINSTALLMENTS").val());
    //contract = contract.toString().replace('[TOTALWITHINTEREST]', $("#hdnTOTALWITHINTEREST").val());
    //contract = contract.toString().replace('[INSTALLMENTAMOUNT]', $("#hdnINSTALLMENTAMOUNT").val());
    //contract = contract.toString().replace('[INSTALLMENTAMOUNT]', $("#hdnINSTALLMENTAMOUNT").val());
    //contract = contract.toString().replace('[TOTALREPAYMENT]', $("#hdnTotalRepayment").val());
    $("#divActualDoc").html(contract);
    fnRemoveErrorMessageFromJS();
    fnHideAndShowTabs('divSideMenu4', 'divEmploymentDetails');
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}
function fnEmploymentDetailsNext(data) {
    fnRemoveErrorMessageFromJS();
    fnHideAndShowTabs('divSideMenu5', 'divBankDetails');
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}
function fnBankDetailsNext(data) {
    //debugger
    if (data[0] == "Success") {
        fnRemoveErrorMessageFromJS();
        fnHideAndShowTabs('divSideMenu6', 'divDocuments');
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");

        //***********************update some of the loan fields in the view contract document**************

        var contract = $("#divActualDoc").html();
        contract = contract.toString().replace('[BANKNAME]', $("#ddlBank").val());
        contract = contract.toString().replace('[SALARYDATE]', $("#ddlPaydayshift option:selected").text());
        contract = contract.toString().replace('[ACCOUNTNUMBER]', $("#txtBankAccountNumber").val());

        $("#divActualDoc").html(contract);
    }
    else {
        fnDisplayErrorMessageFromJS(data[0].toString());
        $("#divLoadIcon").css("display", "none");
        $("#divLoadDisable").css("display", "none");
    }

}
function fnradiocheck(rd, loanId, offerId, loanTerm, description, capital, initiationFee, serviceFee, principalDebt, firstPayDate, lastPayDate, installmentAmount, capitalPlusInterest, totalRepayment, interestRate, vat, interestAmount) {
    //debugger
    $('#gdLoans input[type="radio"]').each(function () {
        $(this).prop("checked", "");
    });
    $(rd).prop("checked", "checked");
    $("#divSelectedLoanInfo").fadeIn();
    $("#hdnSelectedLaonId").val(loanId);
    $("#hdnSelectedOfferID").val(offerId);
    //*******************display selected loan information**************************
    fnLoanSummary(loanTerm, description, capital, initiationFee, serviceFee, principalDebt, firstPayDate, lastPayDate, installmentAmount, capitalPlusInterest, totalRepayment, interestRate, vat, interestAmount);
    var amounts = getLoanAmounts(parseInt(capital) / 100);
    var items = "";

    amounts.forEach(function (item) {
        items += "<option value='" + item + "'>R " + item + "</option>";
    });

    $("select#ddlTest").empty().html(items);

}
function fnLoanSummary(loanTerm, description, capital, initiationFee, serviceFee, principalDebt, firstPayDate, lastPayDate, installmentAmount, capitalPlusInterest, totalRepayment, interestRate, vat, interestAmount) {
    //debugger
    $("#divILoanTerm").text(loanTerm);
    $("#divIDesc").text(description);

    $("#divICapital").text(formatMoney(parseInt(capital) / 100));
    $("#divIInitiationFee").text(formatMoney(parseInt(initiationFee) / 100));
    $("#divIServiceFee").text(formatMoney(parseInt(serviceFee) / 100));
    $("#divIPrincipalDebt").text(formatMoney(parseInt(principalDebt) / 100));

    //alert(firstPayDate.toString().substring(0, 4));
    //alert(firstPayDate.toString().substring(4, 6));
    //alert(firstPayDate.toString().substring(6, 8));
    $("#divIFirstPayDate").text(firstPayDate.toString().substring(0, 4) + "-" + firstPayDate.toString().substring(4, 6) + "-" + firstPayDate.toString().substring(6, 8));
    $("#divILastPayDate").text(lastPayDate.toString().substring(0, 4) + "-" + lastPayDate.toString().substring(4, 6) + "-" + lastPayDate.toString().substring(6, 8));
    $("#divIInstallmentAmt").text(formatMoney(parseInt(installmentAmount) / 100));
    $("#divITotalRepayment").text(formatMoney(parseInt(totalRepayment) / 100));
    //debugger
    //$("#divICreditcostMultiple").text(formatMoney(((parseInt(totalRepayment) / parseInt(capital)) / 100).toFixed(2)));

    $("#divICreditcostMultiple").text(((parseInt(totalRepayment)/100) / (parseInt(capital) / 100)).toFixed(2));
    $("#hdnCREDITAMOUNT").val(parseInt(capital) / 100);
    $("#hdnNOOFINSTALLMENTS").val(loanTerm);
    $("#hdnTOTALWITHINTEREST").val(parseInt(capitalPlusInterest) / 100);
    $("#hdnINSTALLMENTAMOUNT").val(parseInt(installmentAmount) / 100);
    $("#hdnTotalRepayment").val(parseInt(totalRepayment) / 100);


    $("#hdnLoanOption").val(description);
    $("#hdnPrincipalDebt").val(parseInt(principalDebt) / 100);
    $("#hdnInitiationFee").val(parseInt(initiationFee) / 100);
    $("#hdnServiceFee").val(parseInt(serviceFee) / 100);
    $("#hdnFirstPayDate").val(firstPayDate);
    $("#hdnLastPayDate").val(lastPayDate);
    $("#hdnCreditCostMultiple").val((parseInt(totalRepayment)/100) / (parseInt(capital) / 100));
    //debugger
    $("#hdnInterestRate").val(parseInt(interestRate) / 100);
    $("#hdnVAT").val(parseInt(vat) / 100);
    $("#hdnInterestAmt").val(parseInt(interestAmount) / 100);

}
function displayErrorFromAjaxPostback(data) {
    // alert('dfd');
    fnDisplayErrorMessageFromJS(data);
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}
function getLoanAmounts(maxLoanAmount) {
    var minLoanAmount = 500;
    var decrementedAmount = 1000;
    if (maxLoanAmount > 2000 && maxLoanAmount <= 10000)
        decrementedAmount = 500;
    else if (maxLoanAmount <= 2000)
        decrementedAmount = 100;

    var amounts = [];
    var itemAmount = maxLoanAmount;
    var i = 0;
    while (((itemAmount = maxLoanAmount - (decrementedAmount * i++)) >= minLoanAmount))
        amounts.push(itemAmount.toString());

    //if (!amounts.includes(minLoanAmount.toString()))
    //    amounts.push(minLoanAmount.toString());

    //debugger
    // alert(amounts.indexOf(minLoanAmount.toString()));
    if (amounts.indexOf(minLoanAmount.toString()) < 0)
        amounts.push(minLoanAmount.toString());

    return amounts;
}
function displayLoader() {
    fnRemoveErrorMessageFromJS();
    $("#divLoadIcon").css("display", "block");
    $("#divLoadDisable").css("display", "block");
}
function displayLoanOptionsFromAjaxPostback(data) {
    var responsiveHelper = undefined;
    var breakpointDefinition = {
        tablet: 1024,
        phone: 480
    };
    var tableElement = $('#gdLoans');
    if (data[0] == "Success") {
        var oTable = $('#gdLoans').dataTable({
            "autoWidth": false,
            "bServerSide": true,

            "sAjaxSource": getBaseUrl() + "/MyAccount/GetTransactions",

            "oLanguage": {
                "sEmptyTable": "There is no loan options",
                "oPaginate": { "sFirst": " ", "sLast": " ", "sNext": " ", "sPrevious": " " }
            },

            "bProcessing": true,

            //"fnServerParams": function (aoData) {
            //    aoData.push({ "name": "__RequestVerificationToken", "value": $('input[name=__RequestVerificationToken]').val() });
            //},

            "paging": true,
            "pagingType": "simple",
            "autoWidth": false,
            "bAutoWidth": false,
            "aoColumns": [
                  {
                      "sName": "Select",
                      "sWidth": "80px",
                      "sClass": "expand"
                  },
                            {
                                "sName": "Terms",
                                "sWidth": "80px",
                                "sClass": "expand"
                                //"bSearchable": false,
                                //"bSortable": false,
                                //   "fnRender": function (oObj) {
                                //return '<a href=\"Company/Details/' + oObj.aData[0] + '\">View</a>';
                                // }
                            },
                           // { "sName": "SettlementDate", "sWidth": "85px" },
                            { "sName": "Description", "sWidth": "200px" },
                            { "sName": "Capital", "sWidth": "120px" },
                            { "sName": "Intitation Fee", "sWidth": "150px" },
                            { "sName": "Service Fee", "sWidth": "50px" },
                            { "sName": "Principal Debt", "sWidth": "150px" },
                            { "sName": "First Payment Date", "sWidth": "150px" },
                            { "sName": "Last Payment Date", "sWidth": "150px" },
                            { "sName": "Installment Amount", "sWidth": "50px" }
                           // { "sName": "NettAmount", "sWidth": "50px" },
                            //{ "sName": "Export", "sWidth": "40px" }

            ],
            "bFilter": false,
            "bLengthChange": false,
            "fnServerParams": function (aoData) {
                //aoData.push({ "name": "strFilter,FromDate,ToDate,ToDate", "value": "'" + $("#drpFilter").val() + "','" + $("#dtFromDate").val() + "','" + $("#dtToDate").val() + "','" + $("#txtGridSearch").val() + "'" });
                //aoData.push({ "name": "strFilter", "value": $("#drpFilter").val() });
                //aoData.push({ "name": "FromDate", "value": $("#dtFromDate").val() });
                //aoData.push({ "name": "ToDate", "value": $("#dtToDate").val() });
                //aoData.push({ "name": "strSearch", "value": $("#txtGridSearch").val() });
            },
            //  "columns": [{ "width": "8%" }, { "width": "8%" }, { "width": "17%" }, { "width": "15%" }, { "width": "10%" }, { "width": "12%" }, { "width": "8%" }, { "width": "8%" }, { "width": "8%" }, { "width": "8%" }],
            language: {
                paginate: {
                    next: '&nbsp;', // or '→'
                    previous: '&nbsp;' // or '←' 
                }
            },
            preDrawCallback: function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper) {
                    responsiveHelper = new ResponsiveDatatablesHelper(tableElement, breakpointDefinition);
                }
            },
            rowCallback: function (nRow) {
                responsiveHelper.createExpandIcon(nRow);
            },
            drawCallback: function (oSettings) {
                responsiveHelper.respond();
            }
        });
        //$("#divSelectedLoanInfo").fadeIn();
        //var amounts = getLoanAmounts(5000);
        //var items = "";

        //amounts.forEach(function (item) {
        //    items += "<option value='" + item + "'>" + item + "</option>";
        //});

        //$("select#ddlTest").empty().html(items);

        //$('#gdLoans input[type="radio"]').click(function () {
        //    debugger
        //    alert('G');
        //    $('#gdLoans input[type="radio"]').each(function () {
        //        $(this).prop("checked", "");
        //    });
        //    $(this).prop("checked", "checked");
        //});
        fnHideAndShowTabs('divSideMenu3', 'divLoanOptions')
    }
    else {
        //*******************show error moessage to the user
        fnDisplayErrorMessageFromJS(data[1]);
    }
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}


$(document).ready(function () {


    $("#txtAppointmentDate").datepicker({ dateFormat: 'yy/mm/dd', changeYear: true, changeMonth: true, yearRange: '-80:+0' });
    $("#txtContractEndDate").datepicker({ dateFormat: 'yy/mm/dd', changeYear: true, changeMonth: true, yearRange: '-80:+30' });
    $("#hdnSelectedLaonId").val("");
    $("#hdnSelectedOfferID").val("");
    $("#gdLoans").removeClass('footable-loaded');
    $("#gdLoans").removeClass('phone');
    // alert($("#gdPayReceived_wrapper").hasClass('phone'));
    //$('[id*=gdPayReceived]').footable();
    $(".clsFooPayLink").footable();
    //$(".clsContactWhitediv").css('background-color', '#F8F8F8');
});


function fnGetDocument(docFile, lblFileError, lblFileUploadStatus, documentTypeID) {
    var files = document.getElementById(docFile).files;

    if (files == null || files == "null" || files.length == 0)
        return;


    $("#" + docFile).prop("disabled", false);

    var fsize = files[0].size / 1024 / 1024;

    if (parseInt(fsize) >= 5) {
        $("#" + lblFileError).text("Invalid file size.");
        $("#" + lblFileError).css("color", "red");
        return false;
    }

    var data = new FormData();
    //var files = $("#" + verificationFile)[0].files;//.get(0).files;    

    //alert(files.length);
    if (files.length > 0) {
        data.append("HelpSectionImages", files[0]);
        data.append("UploadedDocumentTypeID", documentTypeID);
    }
    else {
        $("#" + lblFileError).text("Please select the Document to Upload");
        $("#" + lblFileError).css("color", "red");

        $("#" + docFile).prop("value", "");
        return false;
    }
    var extension = $("#" + docFile).val().split('.').pop().toUpperCase();
    //alert(extension);
    //debugger
    if (extension != "PDF" && extension != "PNG" && extension != "JPG" && extension != "GIF" && extension != "JPEG") {
        $("#" + lblFileError).text("Invalid file format.");
        $("#" + lblFileError).css("color", "red");

        $("#" + docFile).prop("value", "");
        return false;
    }
    $.ajax({
        url: getBaseUrl() + '/MyAccount/UploadDocument', type: "POST", processData: false,
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: data, dataType: 'json',
        contentType: false,
        success: function (response) {
            if (response != null || response != '') {
                $("#" + lblFileUploadStatus).text(response);
                $("#" + lblFileUploadStatus).css("color", "green");
                $("#" + lblFileError).text("");
            }
            $("#" + docFile).val('');
        },
        error: function (er) {
            $("#" + lblFileError).text("File Upload Failed");
            $("#" + lblFileError).css("color", "red");
        }

    });

    return false;
}


function fnLoanAdjustment() {
    // $("#hdnSelectedOfferID").val(1);


    // alert($("#ddlTest").val());
    $.ajax({
        type: "POST",
        url: getBaseUrl() + "/MyAccount/performAdjustOffer/",
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: '{"offerId": "' + $("#hdnSelectedOfferID").val() + '","newAmount": "' + $("#ddlTest").val() + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.OfferID != 0) {

                fnLoanSummary(msg.LoanTerm, msg.LoanDescription, msg.Loan_Capital, msg.Loan_InitiationFee, msg.Loan_ServiceFee, msg.Loan_PrincipalDebt, msg.DateOfFirstDebit, msg.DateOfLastDebit, msg.Loan_InstallmentAmount, msg.Loan_CapitalPlusInterest, msg.Loan_TotalRepayment, msg.Loan_InterestRate, msg.Loan_VAT, msg.Loan_InterestAmount);
            }
            //debugger
            $("#divLoadIcon").css("display", "none");
            // $("#hdnShowSuccessMsg").val("true");
            //$("#hdnShowSuccessMsgDesc").val("Document uploaded successfully");
            //fnDisplaySuccessMessage();


        }
    });
}

function checkboxChange(checkbox) {
    if (checkbox.checked == true) {
        $("#BtnSave").prop('disabled', false)
    } else {
        $("#BtnSave").prop('disabled', true)
    }
}ss