//$(function () {
//    $("#popup").dialog({
//        autoOpen: false,
//        title: "DETAILS",
//        height: 550,
//        width: 500,
//    });
//});

function SubmitDates(url,url2) {
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var personId = $('#perId').val();
    console.log(fromDate + ' to ' + toDate);
    if (fromDate != '' && fromDate <= toDate && personId != -1 && personId != null) {
        if (toDate == '') {
            toDate = fromDate;
        }
        $('#loader').show();
        $.ajax({
            url: url,
            data: { id: personId, fromDate: fromDate, toDate: toDate },
            success: function (data) {
                if (url == "/SalesReport/GetPersonReportByDates" || url == "/ThirdSaleReport/GetPersonReportByDates")
                {
                    var a = '../' + data[0];
                    a.replace(/\\/g, "/");
                    $('#excelDownload').fadeIn().attr('href', a);
                    data = data[1];
                }
                //prevPage = $('#reportPage').html();
                //console.log(prevPage);
                ItemstableBind(data['items']);
                saleReportBind(data['salesReport']);
                $('#loader').fadeOut();
                //$('#mainContent').html(data);
               
                //$('#emailButton').show();
            }
        });
        var type = $('#personType').val();;
        if (url2 != "" && (type=="SO" || type=="SR"))
        {
            $("#promotorSale").show();
            $("#promotorItem").show();
            $("#promotorSaleReportTbl").empty();
            $("#promotorItemTbl").empty();
            var actionVal = 1;
            var total = 0;
            var promotorTotal = 0;
            $.ajax({
                url: url2,
                data: { id: personId, actionVal: actionVal, fromDate: fromDate, toDate: toDate },
                success: function (data) {
                    $("#promotorSaleReportTbl").append(
                        '<thead><tr class="info" style="cursor:pointer;">' +
                        '<th>Sales Id</th>' +
                        '<th>Date</th>' +
                        '<th>Person Name</th>' +
                        '<th>Reatailer Name</th>' +
                        '<th>Total Quantity (Ltrs)</th>' +
                        '<th> Actions </th>' +
                        '</tr></thead></thead>'
                        );
                    $("#promotorItemTbl").append(
                        '<thead>' +
                                '<tr class="info" style="cursor:pointer;">' +
                                    '<th class="center">Promotor Name</th>' +
                                    '<th class="center">Total Quantity</th>' +
                                '</tr>' +
                            '</thead>'
                        );
                    for (var i in data[0]) {
                        total = total + parseFloat(data[0][i]["totalQuantity"]);
                        var date = data[0][i]["date"].split(' ')[0];

                        var content = '<tr>' +
                              '<td>' + data[0][i]["salesId"] + '</td>' +
                              '<td>' + date + '</td>' +
                              '<td>' + data[0][i]["personName"] + '</td>' +
                              '<td>' + data[0][i]["retailerName"] + '</td>' +
                              '<td>' + data[0][i]["totalQuantity"] + '</td>' +
                              '<td><a onclick="view(' + data[0][i]["salesId"] + ')"><i class="fa fa-file-text" style="font-size:120%"></i></a> | ' +
                              '<a data-ajax="true" data-ajax-begin="SavePage(\'mainContent\')" data-ajax-loading="#loader" data-ajax-mode="replace" data-ajax-update="#mainContent" href="/sales/edit/' + data[0][i]["salesId"] + '?type=2"><i class="fa fa-pencil" style="font-size:120%;"></i></a> |' +
                              '<a onClick="deleteEntry(this,' + data[0][i]["salesId"] + ')"><i class="fa fa-trash" style="font-size:120%"></i></a>' +
                              '</td>' +
                        '</tr>';
                        $('#promotorSaleReportTbl').append(content);
                    }
                    $('#promotorSaleReportTbl').append('<tr class="info" style="cursor:pointer;">' +
                        '<td>Total</td><td></td><td></td><td></td><td>' + total + '</td><td></td></tr>');

                    for (var j in data[1]) {
                        promotorTotal = promotorTotal + parseFloat(data[1][j]["totalQuantity"]);
                        var contnt = '<tr>' +
                              '<td>' + data[1][j]["personName"] + '</td>' +
                              '<td>' + data[1][j]["totalQuantity"] + '</td>' +
                              '</td>' +
                        '</tr>';
                        $('#promotorItemTbl').append(contnt);
                    }

                    $('#promotorItemTbl').append('<tr class="info" style="cursor:pointer;">' +
                        '<td>Total</td><td>' + promotorTotal + '</td><td></td></tr>');
                }
            });
        }
    } else {
        alert("Enter Correct values");
    }
}
function deleteEntry(obj,id) {
    debugger;
    $('#loader').show();
    var actionVal = 3;
    if (confirm("ARE YOU SURE?")) {
        $.ajax({
            url: '/SalesReport/deletePromotorReportByDates',
            data: { id: id, actionVal: actionVal },
            success: function (data) {
                debugger;
                if (data == 'Done') {
                    ShowMessage("Entry Deleted..!! ", "info");
                    SubmitDates('/SalesReport/GetPersonReportByDates', '/SalesReport/GetPromotorReportByDates');
                    //var i = obj.parentNode.parentNode.rowIndex;
                    //document.getElementById("promotorSaleReportTbl").deleteRow(i);
                    $('#loader').fadeOut();
                }
            },
            error: function () {
                ShowMessage("ERROR", "error");
                $('#loader').fadeOut();
            }
        });
    }
}
function view(salesId)
{
    debugger;
    $('#itemmodal').modal('show');
    $("#ItemsDetailTable").empty();
    var actionVal=2;
    $.ajax({
        url: '/SalesReport/GetPromotorReportByDates',
        data: { id: salesId, actionVal: actionVal },
        success: function (data) {
            debugger;
            $("#ItemsDetailTable").append(
                '<thead>'+
                        '<tr class="info" style="cursor:pointer;">' +
                            '<th class="center">Sales Id</th>'+
                            '<th class="center">Product Name</th>'+
                            '<th class="center">Product Quantity</th>' +
                            '<th class="center">Pieces</th>' +
                            '<th class="center">Total Quantity</th>'+
                            '<th class="center">Cost</th>'+
                            '<th class="center">Total Cost</th>'+
                        '</tr>'+
                    '</thead>'
                );
            for (var i in data[0]) {
                var content = '<tr>' +
                      '<td>' + data[0][i]["salesId"] + '</td>' +
                      '<td>' + data[0][i]["productName"] + '</td>' +
                      '<td>' + data[0][i]["productQuantity"] + '</td>' +
                      '<td>' + data[0][i]["pieces"] + '</td>' +
                      '<td>' + data[0][i]["totalQuantity"] + '</td>' +
                      '<td>' + data[0][i]["cost"] + '</td>' +
                      '<td>' + data[0][i]["totalCost"] + '</td>' +
                '</tr>';
                $('#ItemsDetailTable').append(content);
            }
        }
    });
}
function fnExcelReport(element) {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById(element); // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Report.xls");
    }
    else                 //other browser not tested on IE 11
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}