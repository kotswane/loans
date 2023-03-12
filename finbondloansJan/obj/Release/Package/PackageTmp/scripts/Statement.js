
$(document).ready(function () {
    var responsiveHelper = undefined;
    var breakpointDefinition = {
        tablet: 1024,
        phone: 480
    };
    var tableElement = $('#gdStatements');
    var oTable = $('#gdStatements').dataTable({

        "bServerSide": true,

        "sAjaxSource": "/Statements/GetStatements",

        "oLanguage": {
            "sEmptyTable": "There is no statements on your account",
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
                            "sName": "LoanAppNumber",
                            "sWidth": "40%",
                            "sClass": "expand"
                            //"bSearchable": false,
                            //"bSortable": false,
                            //   "fnRender": function (oObj) {
                            //return '<a href=\"Company/Details/' + oObj.aData[0] + '\">View</a>';
                            // }
                        },
                       // { "sName": "SettlementDate", "sWidth": "85px" },
                        { "sName": "LoanAmount", "sWidth": "30%" },
                        { "sName": "View", "sWidth": "30%" }

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
});
function fntest()
{
   
}

function ViewStatementOfLoanApp()
{
    var result = 1;
    $.ajax({
        type: "POST",
        url: "/Statements/ViewStatement/",
        headers: {
            '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val()
        },
        data: '{"strLoanApplicationNumber": "' + result + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#divLoadIcon").css("display", "none");
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