function GetTop5Sos(data) {
    var header = new Array();
    var pieces = new Array();
    var qty = new Array();
    var shops = new Array();
    var dat = data["person"];
    if (dat != null) {
        data = dat;
    }
    for (x in data) {
        header.push(data[x]["personName"]);
        pieces.push(data[x]["pieces"]);
        qty.push(data[x]["totalQuantity"]);
        shops.push(parseInt(data[x]["shopsCovered"]));
    }
    $('#top5Sos').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'TOP 5 SO\'s'
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

function GetBottom5Sos(data) {
    var header = new Array();
    var pieces = new Array();
    var qty = new Array();
    var shops = new Array();
    var dat = data["person"];
    if (dat != null) {
        data = dat;
    }
    for (x in data) {
        header.push(data[x]["personName"]);
        pieces.push(data[x]["pieces"]);
        qty.push(data[x]["totalQuantity"]);
        shops.push(parseInt(data[x]["shopsCovered"]));
    }
    $('#bottom5Sos').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'BOTTOM 10 SO\'s'
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

function GetItemWiseSale(data) {
    var header = new Array();
    var pieces = new Array();
    var qty = new Array();
    for (x in data) {
        header.push(data[x]["itemName"]);
        pieces.push(data[x]["pieces"]);
        qty.push(data[x]["quantity"]);
    }
    $('#itemWiseSale').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Items Sold'
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

        }]
    });
}

function GetLocationWiseSale(data) {
    var header = new Array();
    var pieces = new Array();
    var qty = new Array();
    for (x in data) {
        header.push(data[x]["location"]);
        pieces.push(data[x]["pieces"]);
        qty.push(data[x]["totalQuantity"]);
    }
    $('#locationWiseSale').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Location Wise Sale'
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

        }]
    });
}