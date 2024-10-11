
var valuesArray = new Array();
var arrays = new Array();
var arr = new Array();
var groupBy = '';
var group = '';
var countArr = new Array();
var count = 0;
function GetAllSaleReport() {
    debugger;
    $('#path').html('');
    valuesArray.length = 0;
    arr.length = 0;
    groupBy = '';
    group = '';
    var fromdate = $('#fromdate').val();
    var todate = $('#todate').val();
    var category = $('#category').val();
    var procedure = $('#Procedure').val();
    var itemType = $('#itemType').val();
    groupBy = $('#category option:selected').text();
    if (fromdate !== '' && todate !== '' && category !== '-1') {
        $('#loader').show();
        $.ajax({
            url: '/SalesReport/GetAllSalesReport',
            type: 'GET',
            data: { fromdate: fromdate, todate: todate, category: category, itemType: itemType , procedure: procedure},
            success: function (data) {
                arrays = new Array();
                if (procedure === "GetPromoterSaleReport") {
                    $('#excelDownload').fadeIn().attr('href', data[0]);
                    data = data[1];
                }
                else {
                    $('#excelDownload').fadeIn();
                }
                count = data.length;
                $('#count').html('Count:' + count);
                if (category === '1') {
                    TableBind('State', data, 'state');
                }
                if (category === '2') {
                    TableBind('Zone', data, 'zone');
                }
                if (category === '3') {
                    TableBind('Area', data, 'area');
                }
                if (category === '4') {
                    
                    TableBind('Retailer', data, 'retailer');
                }
                if (category === '5') {
                    TableBind('Person', data, 'person');
                    //    $('#ParentRecord').empty();
                    //    var header = '<thead><tr class="info" style="cursor:pointer;">' +
                    //     '<th>Person</th>' +
                    //      '<th>Total Qunatity(LTRS)</th>' +
                    //         '<th>Total Shops</th>' +
                    //         '<th>Running</th>' +
                    //       '<th>Non-Running</th>' +
                    //         '<th>Below 25LTRS</th>' +
                    //         '<th>Above 25LTRS </th>' +
                    //     '<th>New</th>' +
                    //      '<th>ReBorn</th>' +
                    //  '<th>Last Month Covered</th>' +
                    //      '</tr></thead><tbody>';
                    //    $('#ParentRecord').append(header);
                    //    var totalQuantity = 0;
                    //    for (var i in data) {
                    //        totalQuantity += data[i]["totalQuantity"];
                    //        var content = '<tr>' +
                    //             '<td>' + data[i]["personName"] + '</td>' +
                    //             '<td class="colapsable"><a href="#" onclick=getNextreport(this)>' + data[i]["totalQuantity"] + '</a></td>' +
                    //             '<td class="colapsable">' + data[i]["totalShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["runningShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["nonRunningShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["belowSaleShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["aboveSaleShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["newShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["rebornShops"] + '</td>' +
                    //             '<td class="colapsable">' + data[i]["lastMonthCoveredShops"] + '</td>' +
                    //        '</tr>';
                    //        $('#ParentRecord').append(content);
                    //        $('#loader').fadeOut();
                    //        $('#ParentRecord').tablesorter();
                    //    }

                    //    var lastRow = '<thead><tr class="info" style="cursor:pointer;">' +
                    //         '<td> TOTAL </td>' +
                    //            '<td colspan="9">' + totalQuantity + '</td>' +
                    //          '</tr></thead><tbody>';
                    //    $('#ParentRecord').append(lastRow);
                }
                if (category === '6') {
                    TableBind('Item', data, 'item');
                }

                if (category === '7') {
                    TableBind('personGroup', data, 'personGroup');
                }
                if (category === '8') {
                    TableBind('itemGroup', data, 'itemGroup');
                }
                if (category === '9') {
                    TableBind('parentName', data, 'parentName');
                }
                $('#loader').fadeOut();
                ShowMessage('Excel has been cooked, you may download it!! :)', 'info');
            },
            error: function () {
                $('#loader').fadeOut();
                ShowMessage('Error', 'error');
            }
        });
    }
    else {
        ShowMessage("Please fill all the fields correctly", 'error');
        $('#loader').fadeOut();
    }
}

var tableToExcel = (function () {

    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        if (!table.nodeType) table = document.getElementById(table);
        var ctx = {
            worksheet: name || 'Worksheet', table: table.innerHTML, table: table.innerHTML.replace(/<\s*input[^>]*>/gi, '')
        };
        window.location.href = uri + base64(format(template, ctx));
    };
})();

function TableBind(categoryHeader, data, categoryBind) {
    debugger;
    $('#ParentRecord').empty();


    var totalQuantity = 0;
    var totalBoxes = 0;
    var schemeQuantity = 0;
    var totalTarget = 0;
    var totalPersonTargetBoxes = 0;
    var productiveTarget = 0;
    var productiveShops = 0;
    var header = '';
    if (categoryHeader === 'Retailer') {

        header = '<thead><tr class="info" style="cursor:pointer;">' +
              '<th style="display:none;">rId</th>' +
              '<th>' + categoryHeader + '</th>' +
            '<th>Total Quantity(LTRS)</th>' +
            '<th>Person Target(Boxes)</th>' +
            '<th>BOXES</th>' +
            '<th>Prod Target</th>' +
            '<th>Prod Shops</th>' +
            '<th>Scheme Quantity(LTRS)</th>' +
            '<th>Retailer Target</th>' +
            '</tr></thead><tbody>';
        $('#ParentRecord').append(header);
        for (var i in data) {
            totalQuantity += data[i]["totalQuantity"];
            totalBoxes += data[i]["boxes"];
            schemeQuantity += data[i]["schemeQuantity"];
            totalTarget += data[i]["target"];
            productiveTarget += data[i]["productiveTarget"];
            productiveShops += data[i]["productiveShops"];
            totalPersonTargetBoxes += data[i]["personTargetBoxes"];
            var content = '<tr>' +
                '<td>' + data[i][categoryBind] + '</td>' +
                '<td style="display:none;">' + data[i]["retailerId"] + '</td>' +
                '<td class="colapsable"><a href="#" onclick=getNextreport(this)>' + data[i]["totalQuantity"] + '</a></td>'+
                '<td class="colapsable" > ' + data[i]["personTargetBoxes"] + '</td >' +
                '<td class="colapsable" > ' + data[i]["boxes"].toFixed(2) + '</td >' +
                '<td class="colapsable" > ' + data[i]["productiveTarget"] + '</td >' +
                '<td class="colapsable" > ' + data[i]["productiveShops"] + '</td >' +
                '<td class="colapsable">' + data[i]["schemeQuantity"] + '</td>' +
                '<td>' + data[i]["target"] + '</td>' +
                  '</tr>';
            $('#ParentRecord').append(content);
        }
    }
    else {
        header = '<thead><tr class="info" style="cursor:pointer;">' +
            '<th>' + categoryHeader + '</th>' +
            '<th>Total Quantity(LTRS)</th>' +
            '<th>Person Target(Boxes)</th>' +
            '<th>BOXES</th>' +
            '<th>Prod Target</th>' +
            '<th>Prod Shops</th>' +
            '<th>Scheme Quantity(LTRS)</th>' +
            '<th>Retailer Target</th>' +
        '</tr></thead><tbody>';
            $('#ParentRecord').append(header);
        for (var j in data) {
            totalQuantity += data[j]["totalQuantity"];
            totalBoxes += data[j]["boxes"];
            schemeQuantity += data[j]["schemeQuantity"];
            totalTarget += data[j]["target"];
            productiveTarget += data[j]["productiveTarget"];
            productiveShops += data[j]["productiveShops"];
            totalPersonTargetBoxes += data[j]["personTargetBoxes"];
            var content1 = '<tr>' +
                '<td>' + data[j][categoryBind] + '</td>' +
                '<td class="colapsable"><a href="#" onclick=getNextreport(this)>' + data[j]["totalQuantity"] + '</a></td>' +
                '<td class="colapsable" > ' + data[j]["personTargetBoxes"] + '</td >' +
                '<td class="colapsable">' + data[j]["boxes"].toFixed(2) + '</td>' +
                '<td class="colapsable" > ' + data[j]["productiveTarget"] + '</td >' +
                '<td class="colapsable" > ' + data[j]["productiveShops"] + '</td >' +
                '<td class="colapsable">' + data[j]["schemeQuantity"] + '</td>' +
                '<td>' + data[j]["target"] + '</td>' +
                '</tr>';
            $('#ParentRecord').append(content1);
        }
    }

    $('#loader').fadeOut();


    var lastRow = '<thead><tr class="info" style="cursor:pointer;">' +
   '<td> TOTAL QUANTITY</td>' +
        '<td>' + totalQuantity + '</td>' +
        '<td>' + totalPersonTargetBoxes + '</td>' +
        '<td>' + totalBoxes.toFixed(2) + '</td>' +
        '<td>' + productiveTarget + '</td>' +
        '<td>' + productiveShops + '</td>' +
        '<td>' + schemeQuantity + '</td>' +
   '<td>' + totalTarget + '</td>' +
    '</tr></thead><tbody>';
    $('#ParentRecord').append(lastRow);
    $('#ParentRecord').tablesorter();
}

function getNextreport(obj) {
    debugger;
    countArr.push(count);
    var types = '';
    var value = $(obj).parent().prev().html();
    var type = $(obj).parent().parent().parent().parent().children(':first-child').children(':first-child').children(':eq(0)').html();
    types = $(obj).parent().prev().prev().html();
    if (types === undefined) {
        types = $(obj).parent().prev().html();
    }
    var tempArray = new Array();
    tempArray.push(type);
    tempArray.push(value);
    var tArray = new Array();
    tArray.push(type);
    if (types !== '' && types !== null) {
        tArray.push(types);
    }
    else {
        tArray.push(value);
    }

    var gb = $('#category option:selected').text();
    var category = $('#category').val();

    if (gb !== groupBy && category !== '-1') {
        arrays.push(tArray);
        valuesArray.push(tempArray);
        groupBy = gb;
        if (gb === 'RETAILER') gb = 'rId';
        if (gb === 'PERSON') gb = 'personId';
        if (gb === 'Item Group') gb = 'itemGroup';
        if (gb === 'ASM') gb = 'parentName';
        if (gb === 'Person Group') gb = 'personGroup';
        $('#loader').fadeIn();
        arr.push($('#ParantDiv').html());
        $.ajax({
            url: '/SalesReport/GetNextSaleReport',
            type: 'POST',
            data: { clause: JSON.stringify(valuesArray), groupBy: gb },
            success: function (data) {


                count = data.length;

                $('#count').html('Count:' + count);
                if (category === '1') {
                    TableBind('State', data, 'Name');
                }
                if (category === '2') {
                    TableBind('Zone', data, 'Name');
                }
                if (category === '3') {
                    TableBind('Area', data, 'Name');
                }
                if (category === '4') {
                    TableBind('Retailer', data, 'Name');
                }
                if (category === '5') {
                    TableBind('Person', data, 'Name');
                    //  TableBind('Retailer', data, 'Name');
                    //  $('#ParentRecord').empty();
                    //  var header = '<thead><tr class="info" style="cursor:pointer;">' +
                    //   '<th>Person</th>' +
                    //    '<th>Total Qunatity(LTRS)</th>' +
                    //       '<th>Total Shops</th>' +
                    //       '<th>Running</th>' +
                    //     '<th>Non-Running</th>' +
                    //       '<th>Below 25LTRS</th>' +
                    //       '<th>Above 25LTRS </th>' +
                    //   '<th>New</th>' +
                    //    '<th>ReBorn</th>' +
                    //'<th>Last Month Covered</th>' +
                    //     '</tr></thead><tbody>';
                    //  $('#ParentRecord').append(header);
                    //  var totalQuantity = 0;
                    //  for (var i in data) {
                    //      totalQuantity += data[i]["totalQuantity"];
                    //      var content = '<tr>' +
                    //          '<td>' + data[i]["personName"] + '</td>' +
                    //        '<td class="colapsable"><a href="#" onclick=getNextreport(this)>' + data[i]["totalQuantity"] + '</a></td>' +
                    //          '<td class="colapsable">' + data[i]["totalShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["runningShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["nonRunningShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["belowSaleShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["aboveSaleShops"] + '</td>' +
                    //            '<td class="colapsable">' + data[i]["newShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["rebornShops"] + '</td>' +
                    //           '<td class="colapsable">' + data[i]["lastMonthCoveredShops"] + '</td>' +
                    //      '</tr>';
                    //      $('#ParentRecord').append(content);

                    //      $('#loader').fadeOut();


                    //  }
                    //  var lastRow = '<thead><tr class="info" style="cursor:pointer;">' +
                    //      '<td> TOTAL</td>' +
                    //         '<td colspan="9">' + totalQuantity + '</td>' +
                    //       '</tr></thead><tbody>';
                    //  $('#ParentRecord').append(lastRow);
                    //  $('#ParentRecord').tablesorter();
                }
                if (category === '6') {
                    TableBind('Item', data, 'Name');
                }
                if (category === '7') {
                    TableBind('PersonGroup', data, 'Name');
                }
                if (category === '8') {
                    TableBind('ItemGroup', data, 'Name');
                }
                if (category === '9') {
                    TableBind('parentName', data, 'Name');
                }
                createPath();

            },
            error: function (e) {
                ShowMessage('Error', 'error');
                $('#loader').fadeOut();
            }
        });
    }
    else {
        ShowMessage('Please Select next drill!', 'warning');
    }
}

function BackButton() {
    
    var prevdata = arr.pop();
    valuesArray.pop();
    arrays.pop();
    $('#ParantDiv').html(prevdata);
    createPath();
    var counts = countArr.pop();
    
    if (Math.floor(counts) === counts && $.isNumeric(counts)) {
        $('#count').html('Count:' + counts);
    }
   
    groupBy = '';
}

function createPath() {
    
    var pathArray = new Array();
    pathArray = JSON.parse(JSON.stringify(arrays));
    var path = "";
    pathArray.forEach(function (val) {
        //val.pop();
        path += val.pop() + ' -> '
    });
    
    $('#path').html(path);
}
