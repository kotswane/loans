function displayLoader() {
    fnRemoveErrorMessageFromJS();
    $("#divLoadIcon").css("display", "block");
    $("#divLoadDisable").css("display", "block");
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

function displayErrorFromAjaxPostback(data) {
    // alert('dfd');
    fnDisplayErrorMessageFromJS(data);
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
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

            "bServerSide": true,

            "sAjaxSource": getBaseUrl() + "/CheckEligibility/GetTransactions",

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
                  //{
                  //    "sName": "Select",
                  //    "sWidth": "80px",
                  //    "sClass": "expand"
                  //},
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
        //fnHideAndShowTabs('divSideMenu3', 'divLoanOptions')
        $("#divExpensestab").css("display", "none");
        $('#divLoanOptions').removeAttr('style');
        $("#divLoanOptions").css("display", "block");
    }
    else {
        //*******************show error moessage to the user
        fnDisplayErrorMessageFromJS(data[1]);
    }
    $("#divLoadIcon").css("display", "none");
    $("#divLoadDisable").css("display", "none");
}
