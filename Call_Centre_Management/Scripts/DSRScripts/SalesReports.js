//$(function () {
//    $("#popup").dialog({
//        autoOpen: false,
//        title: "DETAILS",
//        height: 500,
//        width: 650,
//    });
//});
var prevPage;

function ReportByRetailer(url) {
    
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var state = $('#stateId').val();
    var zone = $('#zoneId').val();
    var distributor = $('#distributors').val();
    var personGroup = $('#PersonGroup').val();
    
    
    if (zone === null) zone = -1;
    if (distributor === null) distributor = -1;
    if (toDate === '') toDate = fromDate;
    if (fromDate !== "" && new Date(fromDate) <= new Date(toDate)) {
        $('#loader').show();
        $.ajax({
            type: "POST",
            datatype: "json",
            traditional: true,
            url: url,
            data: { fromDate: fromDate, toDate: toDate, state: state, zone: zone, distributor: distributor, PersonGroup: personGroup },
            success: function (data) {
                $('#excelDownload').fadeIn().attr('href', data[0]);
                data=data[1];
                saleReportBind(data['salesReport']);
                ItemstableBind(data['items']);
                LocationTableBind(data['locationData'], data);
                $('#loader').hide();
                $('#emailButton').show();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    } else {
        alert("Please Enter Correct Dates!!");
    }
}
function GetReport(url) {
    
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var state = $('#stateId').val();
    var zone = $('#zoneId').val();
    var distributor = $('#distributors').val();
    var personGroup = $('#PersonGroup').val();
    
    
    if (zone === null) zone = -1;
    if (distributor === null) distributor = -1;
    if (toDate === '') toDate = fromDate;
    //if (fromDate !== "" && new Date(fromDate) <=new Date(toDate)) {
      if (fromDate !== "" && fromDate <=toDate) {
        $('#loader').show();
        $.ajax({
            url: url,
            data: { fromDate: fromDate, toDate: toDate, state: state, zone: zone, distributor: distributor, PersonGroup: personGroup },
            success: function (data) {
                if (url === "/ThirdSaleReport/GetReportByRetailer")
                {
                    var a = '../' + data[0];
                    a.replace(/\\/g, "/");
                    
                    $('#excelDownload').fadeIn().attr('href', a);
                    data = data[1];
                }
                saleReportBind(data['salesReport']);
                ItemstableBind(data['items']);
                LocationTableBind(data['locationData'], data);
                $('#loader').hide();
                $('#emailButton').show();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    } else {
        alert("Please Enter Correct Dates!!");
    }
}
function GetPersonReport(fromdate, todate, objVal, url) {
    
    $('#loader').show();
    $.ajax({
        url: url,
        data: { id: objVal, fromDate: fromdate, toDate: todate },
        success: function (data) {
            $('#loader').hide();
            prevPage = $('#reportPage').html();
            console.log(prevPage);
            $('#reportPage').html(data);
           
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}
function OpenPopUp() {
    
    $("#dialog").modal('show');
    //$("#popup").dialog('open');
}
function PrevPage() {
    $('#reportPage').html(prevPage);
    $('#loading').hide();
}
function Delete(obj, salesId, url) {
    var x = confirm("Are you sure you want to delete?");
    if (x) {
        $('#loader').fadeIn();
        $.ajax({
            url: url,
            data: { id: salesId },
            success: function (data) {
                $('#loader').fadeOut();
                if (+data === 1) {
                    var parent = $(obj).parent().parent();
                    var val = parent.children('td:eq(5)').html();
                    var items = $('#items');
                    var total = items.children('tbody').children('tr:last').children('td:last').html();

                    total = total - val;
                    items.children('tbody').children().empty();
                    items.append('<tr class="info bold"><td>Total</td><td>' + total + '</td></tr>');
                    parent.remove();
                    ShowMessage('Entry Deleted', 'info');
                }
                else if (data === 0) {
                    ShowMessage('Oops.. Something wrong Happened!! Maybe you don\'t have the rights to delete stuff', 'error');
                }

            },
            error: function () {
                $('#loader').fadeOut();
                ShowMessage('Oops.. Something wrong Happened!!', 'error');
            }
        });
    }
}
//@Ajax.ActionLink("DETAILS", "GetIndividualReport", new { id = item.salesId },
//                     new AjaxOptions { HttpMethod = "POST", InsertionMode = InsertionMode.Replace, UpdateTargetId = "popup", OnSuccess = "OpenPopUp", })

function GetDetails(salesId, url) {
    
    $('#loader').show();
    $.ajax({
        url: url,
        data: { id: salesId },
        success: function (data) {
            
            $('#popup').html(data);
            OpenPopUp();
            $('#loader').hide();
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}
function GetReportByPerson() {
    
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var state = $('#stateId').val();
    var zone = $('#zoneId').val();
    var personGroup = $('#PersonGroup').val();
    var procedure=$('#procedure').val();
    
    if (zone === null) zone = -1;
    if (state === null) state = -1;
    if (toDate === '') toDate = fromDate;
    if (fromDate !== "" && new Date(fromDate) <= new Date(toDate)) {
        $('#loader').show();
        $.ajax({
            type: "POST",
            datatype: "json",
            traditional: true,
            url: '/SalesReport/GetReportsByPersonV2',
            data: { fromDate: fromDate, toDate: toDate, state: state, zone: zone, personGroup: personGroup,procedure:procedure },
            success: function (data) {
                
                SOReportTableBind(data['person']);
                GetChartVal(data['person']);
                $('#totalQtySpan').html(data['totalQuantity']);
                $('#loader').hide();
                $('#emailButton').show();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    } else {
        alert("Please Enter Correct Dates!!");
    }
}
function GetItemInfo(fromDate, toDate, locationId, locationType, url) {
    
    $('#loader').show();
    $.ajax({
        url: url,
        data: { fromDate: fromDate, toDate: toDate, parent: locationId, parentType: locationType },
        success: function (data) {
            
            $('#popup').html(data);
            $('#dialog').modal('show');
            //$('#popup').dialog('open');
            $('#loader').hide();
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}
function GetRetailerReport() {
    var value, type;
    var state = $('#stateId').val();
    var zone = $('#zoneId').val();
    var person = $('#personId').val();
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    if (fromDate === "") {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd
        }
        if (mm < 10) {
            mm = '0' + mm
        }
        today = yyyy + '-' + mm + '-' + dd;
        fromDate = today;
        toDate = today
    }
    
    if (person !== null && person !== -1) {
        value = person;
        type = "person";
    }
    else if (zone !== null && zone !== -1) {
        value = zone;
        type = "zone";
    }
    else if (state !== null && state !== -1) {
        value = state;
        type = "state";
    }
    else {
        value = "";
        type = "";
    }
    $('#loader').show();
    $.ajax({
        url: '/SalesReport/GetRetailerReport',
        type: "POST",
        data: { fromDate: fromDate, toDate: toDate, value: value, type: type },
        success: function (data) {
            $('#mainContent').html(data);
            $('#loader').hide();
            $('#emailButton').show();
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}
function GetRetailerReportNoSale() {
    value = "";
    type = "noSale";
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    if (fromDate === "") {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd
        }
        if (mm < 10) {
            mm = '0' + mm
        }
        today = yyyy + '-' + mm + '-' + dd;
        fromDate = today;
        toDate = today
    }
    $('#loader').show();
    $.ajax({
        url: '/SalesReport/GetRetailerReport',
        type: "POST",
        data: { fromDate: fromDate, toDate: toDate, value: value, type: type },
        success: function (data) {
            $('#mainContent').html(data);
            $('#loader').hide();
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}

function GetChartVal(data) {
    var header = new Array();
    var pieces = new Array();
    var qty = new Array();
    var shops = new Array();
    var dat = data["person"];
    if (dat !== null) {
        data = dat;
    }
    for (x in data) {
        header.push(data[x]["personName"]);
        pieces.push(data[x]["pieces"]);
        qty.push(data[x]["totalQuantity"]);
        shops.push(parseInt(data[x]["shopsCovered"]));
    }
    $('#Chart').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'SO Chart'
        },
        subtitle: {
            text: 'Source: DSR'
        },
        xAxis: {
            categories: header
        },
        yAxis: {
            min: 0,
            title: {
                text: 'QTY'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'TOTAL LTRS',
            data: qty

        }, {
            name: 'PIECES',
            data: pieces

        }, {
            name: 'SHOPS-COVERED',
            data: shops

        }]
    });
}
  var tablesToExcel = (function () {
        var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>'
        , templateend = '</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head>'
        , body = '<body>'
        , tablevar = '<table>{table'
        , tablevarend = '}</table>'
        , bodyend = '</body></html>'
        , worksheet = '<x:ExcelWorksheet><x:Name>'
        , worksheetend = '</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>'
        , worksheetvar = '{worksheet'
        , worksheetvarend = '}'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
        , wstemplate = ''
        , tabletemplate = '';

        return function (table, name, filename) {
            var tables = table;

            for (var i = 0; i < tables.length; ++i) {
                wstemplate += worksheet + worksheetvar + i + worksheetvarend + worksheetend;
                tabletemplate += tablevar + i + tablevarend;
            }

            var allTemplate = template + wstemplate + templateend;
            var allWorksheet = body + tabletemplate + bodyend;
            var allOfIt = allTemplate + allWorksheet;

            var ctx = {};
            for (var j = 0; j < tables.length; ++j) {
                ctx['worksheet' + j] = name[j];
            }

            for (var k = 0; k < tables.length; ++k) {
                var exceltable;
                if (!tables[k].nodeType) exceltable = document.getElementById(tables[k]);
                ctx['table' + k] = exceltable.innerHTML;
            }

            //document.getElementById("dlink").href = uri + base64(format(template, ctx));
            //document.getElementById("dlink").download = filename;
            //document.getElementById("dlink").click();

            window.location.href = uri + base64(format(allOfIt, ctx));

        }
    })();
