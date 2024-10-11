var locArray = new Array();
var directionsDisplay;
var directionsService;
var map;
$(function () {
    google.maps.event.addDomListener(window, 'load', initialize);
});

function initialize() {
    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();
    var j3 = new google.maps.LatLng(28.6432609, 77.1206487);
    var mapOptions = {
        zoom: 10,
        center: j3
    }
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    directionsDisplay.setMap(map);

}

function SplitRoute() {
    
    var batches = [];
    var itemsPerBatch = 10; // google API max - 1 start, 1 stop, and 8 waypoints
    var itemsCounter = 0;
    var stops = locArray;
    var wayptsExist = stops.length > 0;

    while (wayptsExist) {
        var subBatch = [];
        var subitemsCounter = 0;

        for (var j = itemsCounter; j < stops.length; j++) {
            subitemsCounter++;
            subBatch.push({
                location: stops[j],
                stopover: true
            });
            if (subitemsCounter === itemsPerBatch)
                break;
        }

        itemsCounter += subitemsCounter;
        batches.push(subBatch);
        wayptsExist = itemsCounter < stops.length;
        // If it runs again there are still points. Minus 1 before continuing to 
        // start up with end of previous tour leg
        itemsCounter--;
    }
    calcRoute(batches);
}

function calcRoute(batches) {
    
    var totalDistance = 0;
    var combinedResults = [{}];
    var unsortedResults = [{}]; // to hold the counter and the results themselves as they come back, to later sort
    var directionsResultsReturned = 0;

    for (var k = 0; k < batches.length; k++) {
        var lastIndex = batches[k].length - 1;
        var start = batches[k][0].location;
        var end = batches[k][lastIndex].location;

        // trim first and last entry from array
        var waypts = [];
        waypts = batches[k];
        waypts.splice(0, 1);
        waypts.splice(waypts.length - 1, 1);

        var request = {
            origin: start,
            destination: end,
            waypoints: waypts,
            travelMode: window.google.maps.TravelMode.DRIVING
        };
        var j = 1;
        (function (kk) {
            var summaryPanel = document.getElementById('directions_panel');
            summaryPanel.innerHTML = '';

            directionsService.route(request, function (result, status) {
                if (status == google.maps.DirectionsStatus.OK) {

                    var route = result.routes[0];

                    // For each route, display summary information.
                    for (var i = 0; i < route.legs.length; i++) {
                        //addMarker(route.legs[i].end_location, i, map);
                        var routeSegment = j++ + 1;
                        summaryPanel.innerHTML += '<b>Route: ' + routeSegment + '</b><br>';
                        summaryPanel.innerHTML += route.legs[i].start_address + '<br> <b>to</b> <br>';
                        summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                        summaryPanel.innerHTML += '<b>' + route.legs[i].distance.text + '</b><hr><br>';
                        var x = route.legs[i].distance.text.substr(-1);
                        totalDistance += route.legs[i].distance.value / 1000;

                    }
                    $('#totalDistance').html(totalDistance + "Kms");
                }
                if (status == window.google.maps.DirectionsStatus.OK) {

                    var unsortedResult = {
                        order: kk,
                        result: result
                    };
                    unsortedResults.push(unsortedResult);

                    directionsResultsReturned++;
                    debugger;
                    if (directionsResultsReturned == batches.length) // we've received all the results. put to map
                    {
                        // sort the returned values into their correct order
                        unsortedResults.sort(function (a, b) {
                            return parseFloat(a.order) - parseFloat(b.order);
                        });
                        var count = 0;
                        for (var key in unsortedResults) {
                            if (unsortedResults[key].result != null) {
                                if (unsortedResults.hasOwnProperty(key)) {
                                    if (count == 0) // first results. new up the combinedResults object
                                        combinedResults = unsortedResults[key].result;
                                    else {
                                        // only building up legs, overview_path, and bounds in my consolidated object. This is not a complete
                                        // directionResults object, but enough to draw a path on the map, which is all I need
                                        combinedResults.routes[0].legs = combinedResults.routes[0].legs.concat(unsortedResults[key].result.routes[0].legs);
                                        combinedResults.routes[0].overview_path = combinedResults.routes[0].overview_path.concat(unsortedResults[key].result.routes[0].overview_path);

                                        combinedResults.routes[0].bounds = combinedResults.routes[0].bounds.extend(unsortedResults[key].result.routes[0].bounds.getNorthEast());
                                        combinedResults.routes[0].bounds = combinedResults.routes[0].bounds.extend(unsortedResults[key].result.routes[0].bounds.getSouthWest());
                                    }
                                    count++;
                                }
                            }
                        }
                        directionsDisplay.setDirections(combinedResults);
                    }
                }
            });
        })(k);
        locArray.length = 0;
    }
}

function addMarker(routeC, i, map1) {

    var beachMarker = new google.maps.Marker({
        position: routeC,
        map: map1,
        icon: 'http://www.oxylab.com.ua/tools/gm_marker/plugin_gm_marker.php?color=d1b900&letter=' + i
    });
}

function GetGeoReport() {
    var personId = $('#personId').val();
    var date = $('#date').val();
    $('#loader').fadeIn();
    if (personId !== '' && date !== '') {
        initialize();
        $('#directions_panel').html('');
        $('#totalDistance').html('');
        $.ajax({
            url: '/GeoLocation/GetGeoReport',
            data: { personId: personId, date: date },
            success: function (data) {
                $('#loader').fadeOut();
                fillGeoTable(data);
            },
            error: function () {
                ShowMessage('Oops.. Something wrong happened!', 'error');
                $('#loader').fadeOut();
            }
        });
    }
}

function fillGeoTable(data) {
    var sno = 1;
    var reportTable = $('#geoReportsTable');
    reportTable.empty();
    var header = '<thead><tr class="info" style="cursor:pointer;">' +
        '<th>S.No.</th>' +
        '<th>Lctn</th>' +
        //'<th>DATE</th>' +
        '<th>Retailer </th>' +
        //'<th>Retailer</th>' +
        '<th>Beat</th>' +
        '<th>Address</th>' +
        '<th style="display:none;">PIECES</th>' +
        '<th>Order Ltrs</th>' +
        '<th>Scheme Ltrs</th>' +
        //'<th>STOCK</th>' +
        '<th>Status</th>' +
        '<th>Last Sale On</th>' +
        '<th>Last Sale Ltrs</th>' +
        '<th>Avg Sale </th>' +
        '<th>Hedge By</th>' +
        '<th>Time Duration</th>' +
        '<th>Time</th>' +
        '<th>Image</th>' +
        '<th>Actions</th>' +
        '<th>Shelf Images</th>' +
        '</tr></thead><tbody>';
    reportTable.append(header);
    var charCode = 65;

    var totalQuantity = 0;
    var totalAvgSale = 0;
    for (var report in data) {

        var countryArray = data[report]['timeDuration'].split(':');
        for (var i = 0; i < countryArray.length; i++) {
            countryArray[i] = parseInt(countryArray[i]);
        }
        var sNo;
        if ((data[report]['latitude'] === '0.0' || data[report]['latitude'] === '0') && (data[report]['longitude'] === '0' || data[report]['longitude'] === '0.0')) {
            sNo = "*";
        } else {
            sNo = String.fromCharCode(charCode++);
        }

        if (data[report]['retLat'] === '' || data[report]['retLat'] === '0.0') {
            data[report]['retLat'] = '0';
        }
        if (data[report]['retLong'] === '' || data[report]['retLong'] === '0.0') {
            data[report]['retLong'] = '0';
        }
        if (data[report]['latitude'] === '' || data[report]['latitude'] === '0.0') {
            data[report]['latitude'] = '0';
        }
        if (data[report]['longitude'] === '' || data[report]['longitude'] === '0.0') {
            data[report]['longitude'] = '0';
        }
        var checkLatitutde = 0;
        var checkLongitude = 0;
        var style = '';
        var timeStyle = 'Style="color:ForestGreen ;"';
        var images = '';
        if (data[report]['longitude'] < 0) {
            checkLongitude = 1;
            style = 'Style= "color:red;"';
        }
        if (data[report]['latitude'] < 0) {
            checkLatitutde = 1;
            style = 'Style= "color:red;"';
        }
        var allowed = data[report]['allowed'];
        var approveRejectButton = '';
        if (allowed === false) {
            style = 'Style= "color:red;"';

            var isDeleted = data[report]['deleted'];
            if (isDeleted === 1) {
                approveRejectButton = '|<a href="#stay" onclick="approveSales(' + data[report]['salesId'] + ', this)"><i class="fa fa-check" style="color: rgb(22, 147, 234);font-size:130%"></i></a>';
            }
        }
        if (countryArray[0] < 5) {
            timeStyle = 'Style="color:Tomato ;"';
        }
        if (data[report]['shelfImgCounter'] > 0) {
            images = '<td><a style="cursor:pointer;" onclick="getShelfImages(' + data[report]['salesId'] + ')">Show images</a></td>';
        }

        locationReport = "https://maps.google.com/maps?q=" + data[report]['latitude'] + ',' + data[report]['longitude'] + '" target="_blank"';
        var meter = '';
        if (data[report]['retLat'] !== '0' && data[report]['retLong'] !== '0' && data[report]['latitude'] !== '0' && data[report]['longitude'] !== '0' && checkLatitutde === 0 && checkLongitude === 0) {

            meter = GetDistance(data[report]['retLat'], data[report]['retLong'], data[report]['latitude'], data[report]['longitude']) + ' M';
            locationReport = '/geolocation/geodetailreport?retailerName=' + data[report]['retailerName'] + '&retLat=' + data[report]['retLat'] + '&retLong=' + data[report]['retLong'] + '&personName=' + data[report]['personName'] + '&personLat=' + data[report]['latitude'] + '&personLong=' + data[report]['longitude'] + '" target="_blank"';
        }
        totalQuantity += data[report]['quantity'];
        totalAvgSale += +data[report]['avgSale'];
        var personMeter = '';
        if (meter !== null && meter !== "") {
            personMeter = '(' + meter + ')';
        }
        var totalLtr = data[report]['quantity'];
        var schemeLtr = data[report]['schemeQuantity'];
        var orderLtr = totalLtr - schemeLtr;
        var content = '<tr>' +
            '<td>' + sno++ + '</td>' +
            '<td > <a ' + style + ' href="' + locationReport + 'title="Accuracy: ' + data[report]['accuracy'] + ' M">' + sNo + personMeter + '</a></td>' +
            //'<td>' + data[report]['date'] + '</td>' +

            //'<td> <a href="' + locationReport + 'title="Accuracy: ' + data[report]['accuracy'] + ' M">' + data[report]['retailerName'] + personMeter + '</a></td>' +
            '<td><a herf="#" style="cursor: pointer" onclick="senddata(' + data[report]['retailerId'] + ',\'' + $.trim(data[report]['retailerName']) + '\');">' + data[report]['retailerName'] + " (" + data[report]['retailerId'] + ")" + '</td>' +
            //'<td>' + data[report]['retailerName'] + '</td>' +
            '<td>' + data[report]['beat'] + '</td>' +
            '<td>' + data[report]['area'] + ', ' + data[report]['zone'] + '</td>' +
            '<td style="display:none;">' + data[report]['pieces'] + '</td>' +
            '<td>' + orderLtr + '</td>' +
            '<td>' + data[report]['schemeQuantity'] + '</td>' +
            //'<td>' + data[report]['stock'] + '</td>' +
            '<td>' + data[report]['status'] + '</td>' +
            '<td>' + data[report]['lastSaleDate'] + '</td>' +
            '<td>' + data[report]['lastSale'] + '</td>' +
            '<td>' + data[report]['avgSale'] + '</td>' +
            '<td>' + data[report]['hedgeBy'] + '</td>' +
            //'<td>'+data[report]['timeDuration']+'</td>'+
            '<td><font ' + timeStyle + 'title="Distance: ' + data[report]['distance'] + ' M">' + data[report]['timeDuration'] + '</font></td>' +
            //'<td><a href="#stay" onclick="AddLocationToRetailer(' + data[report]['retailerId'] + ',' + data[report]['latitude'] + ',' + data[report]['longitude'] + ')">HEDGE</a></td>' +
            '<td>' + data[report]['timeStamp'] + '</td>' +
            '<td><a href="/Uploads/Photo/' + data[report]['imagePath'] + '" target="_blank"><img src="/Uploads/Photo/' + data[report]['imagePath'] + '"  style="width:25px" /></a></td>' +
            //'<td><a href="#stay" onclick="GetDetails(' + data[report]['salesId'] + ', \'/SalesReport/GetIndividualReport\')">DETAILS</a>|<a href="#stay" onclick="Delete(this, ' + data[report]['salesId'] + ' , \'/salesreport/Delete\')">DELETE</a></td>' +
            '<td>' +
            '<a href="#" onclick="GetDetails(' + data[report]['salesId'] + ', \'/SalesReport/GetIndividualReport\')">' +
            //'<core-icon icon="open-in-new"></core-icon>' +
            '<i class="fa fa-external-link" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
            '</a>|  <a href="#" onclick="Delete(this, ' + data[report]['salesId'] + ' , \'/salesreport/Delete\')">' +
            //'<core-icon class="big" icon="delete"></core-icon>' +
            '<i class="fa fa-trash" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
            '</a>|<a href="#stay" onclick="AddLocationToRetailer(' + data[report]['retailerId'] + ',' + data[report]['latitude'] + ',' + data[report]['longitude'] + ',' + data[report]['salesId'] + ')">' +
            //'<core-icon class="big" icon="file-upload"></core-icon>' +
            '<i class="fa fa-upload" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
            '</a></a>' + approveRejectButton + '</td>' +
            images +
            '</tr>';
        if (data[report]['latitude'] !== 0 && checkLatitutde === 0 && checkLongitude === 0) {
            locArray.push(new google.maps.LatLng(data[report]['latitude'], data[report]['longitude']));
        }
        reportTable.append(content);
    }
    SplitRoute();
    $('#totalQuantity').html(totalQuantity + ' Ltrs');
    $('#totalAvgSale').html(totalAvgSale + ' Ltrs');
    reportTable.tablesorter();
}

function approveSales(salesId, obj) {
    if (confirm("Do you want to Approve the sale?")) {
    $.ajax({
        url: '/GeoLocation/approveSales?salesId=' + salesId,
        type: 'GET',
        success: function (data) {
            $(obj).remove();
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
    }
}

function senddata(retId, retName) {
    
    var fDate = $('#date').val();
    
    window.open("/SalesReport/SingleRetailerReportII?retailerId=" + retId + "&toDate=" + fDate + "");
}
function getShelfImages(salesId) {
    $.ajax({
        url: '/GeoLocation/getShelfImages',
        data: { salesId: salesId },
        type: "GET",
        success: function (data) {
            $("#shelfDialog").modal('show');
            $("#imgPopup").html("");
            $("#imgPopup").append("<table><thead><tr><th>Url</th></tr></thead><tbody>");
            for (var si = 0; si < data.length; si++) {
                $("#imgPopup").append("<tr><td><a href='../../uploads/Photo/" + data[si] + "' target=\"_blank\">" + data[si] + "</a></td></tr>");
            }
            $("#imgPopup").append("</tbody></table>");
        },
        error: function () {
            ShowMessage('ERROR', 'error');
        }
    });
}
function GeoDetailReport(retailerName, retLat, retLong, personName, personLat, personLong) {
    $.ajax({
        url: '/GeoLocation/GeoDetailReport',
        data: { retailerName: retailerName, retLat: reLat, retLong: retLong, personName: personName, personLat: personLat, personLong: personLong },
        type: "POST",
        success: function (data) {
        },
        error: function () {
            ShowMessage('ERROR', 'error');
        }
    });
}

function AddLocationToRetailer(retailerId, latitude, longitude, salesId) {
    if (confirm("Are you sure you want to hedge this location")) {
        $.ajax({
            url: '/Retailer/AddLocationToRetailer',
            data: { retailerId: retailerId, latitude: latitude, longitude: longitude, salesId: salesId },
            type: "POST",
            success: function (data) {
                ShowMessage('SUCCESS', 'info');
            },
            error: function () {
                ShowMessage('ERROR', 'error');
            }
        });
    }
}

function GetLocationReport() {
    var personId = $('#personId').val();
    var date = $('#date').val();
    var counter = 1;
    if (personId !== '' && date !== '') {
        $('#loader').fadeIn();
        $.ajax({
            url: '/GeoLocation/GetLocationReport',
            data: { personId: personId, date: date },
            success: function (data) {
                var reportTable = $('#locationReportsTable');
                reportTable.empty();
                var header = '<thead><tr class="info" style="cursor:pointer;">' +
                    '<th>S.No.</th>' + '<th>LCTN</th>' + '<th>LOCATION</th>' + '<th>BATTERY</th>' + '<th>TIMESTAMP</th>' + '<th>ACTIONS</th>' +
                     '</tr></thead><tbody>';
                reportTable.append(header);

                var sno = 0;
                var charCode = 65;
                for (var report in data) {
                    var sNo;
                    var locationButton = "";
                    if ((data[report]['latitude'] === '0.0' || data[report]['latitude'] === '0') && (data[report]['longitude'] === '0' || data[report]['longitude'] === '0.0')) {
                        sNo = "*";
                    } else {
                        sNo = String.fromCharCode(charCode++);
                    }

                    var retailerId = data[report]['retailerId'];
                    if (+retailerId === 0) {
                        locationButton = '<a href="https://maps.google.com/maps?q=' + data[report]['latitude'] + ',' + data[report]['longitude'] + '" target="_blank" title="Accuracy: ' + data[report]['accuracy'] + ' M">LOCATION</a>';
                    } else {
                        var locationReport = '/geolocation/geodetailreportV2?retailerId=' + data[report]['retailerId'] + '&retailerName=' + data[report]['retailerName'] + '&personName=' + $('#personId  option:selected').text() + '&personLat=' + data[report]['latitude'] + '&personLong=' + data[report]['longitude'] + '" target="_blank"';
                        locationButton = '<a href="' + locationReport + '" >' + data[report]['retailerName'] + ' (' + GetDistance(data[report]['retLat'], data[report]['retLong'], data[report]['latitude'], data[report]['longitude']) +' M)</a>';
                    }
                    var content = '<tr>' +
                        '<td>' + counter++ + '</td>' + '<td>' + sNo + '</td>' +
                        '<td>' + locationButton + '</td>' +
                        '<td>' + data[report]['battery'] + '</td>' +
                        '<td>' + data[report]['timeStamp'] + '</td>';

                    var salesId = data[report]['salesId'];
                    if (salesId !== -1) {
                        content += '<td><a href="#stay" onclick="GetDetails(' + data[report]['salesId'] + ', \'/SalesReport/GetIndividualReport\')">DETAILS</a>|' +
                        '<a href="#stay" onclick="Delete(this, ' + data[report]['salesId'] + ')">DELETE</a></td>';
                    } else {
                        content += '<td>NONE</td>';
                    }

                    content += '</tr>';
                    if (data[report]['latitude'] !== 0) {
                        locArray.push(new google.maps.LatLng(data[report]['latitude'], data[report]['longitude']));
                    }
                    reportTable.append(content);
                }
                reportTable.tablesorter();
                SplitRoute();
                $('#loader').fadeOut();
            },
            error: function (data) {
            debugger;
                ShowMessage('Oops.. Something wrong happened!', 'error');
                $('#loader').fadeOut();
            }
        });
    }
}

function GetDistance(firstLat, firstLong, lastLat, lastLong) {
    
    var shopLatLang = new google.maps.LatLng(firstLat, firstLong);
    var personLatLang = new google.maps.LatLng(lastLat, lastLong);
    return Math.round(google.maps.geometry.spherical.computeDistanceBetween(shopLatLang, personLatLang));
}

function getSaleAndLocation() {
    var personId = $('#personId').val();
    var date = $('#date').val();
    $('#loader').fadeIn();
    if (personId !== '' && date !== '') {
        initialize();
        $('#directions_panel').html('');
        $('#totalDistance').html('');
        $.ajax({
            url: '/GeoLocation/getSaleAndLocation',
            data: { personId: personId, date: date },
            type: "POST",
            success: function (data) {
                debugger;
                $('#loader').fadeOut();
                var reportTable = $('#geoReportsTable');
                reportTable.empty();
                var header = '<thead><tr class="info" style="cursor:pointer;">' +
                    '<th>S.No</th>' +
                    '<th>SalesId</th>' +
                    '<th>Retailer</th>' +
                    '<th>Status</th>' +
                    '<th>Quantity(Ltrs.)</th>' +
                    '<th>Status</th>' +
                    '<th>Last Sale On</th>' +
                    '<th>Last Sale Ltrs</th>' +
                    '<th>Avg Sale </th>' +
                    '<th>Duration</th>' +
                    '<th>Time</th>' +
                    '<th>Image</th>' +
                    '<th>Actions</th>' +
                    '</tr></thead><tbody>';
                reportTable.append(header);
                var charCode = 65;

                var totalQuantity = 0;
                for (var report in data) {

                    var countryArray = data[report]['timeDuration'].split(':');
                    for (var i = 0; i < countryArray.length; i++) {
                        countryArray[i] = parseInt(countryArray[i]);
                    }
                    var sNo;
                    if ((data[report]['latitude'] === '0.0' || data[report]['latitude'] === '0')) {
                        sNo = "*";
                    } else {
                        sNo = String.fromCharCode(charCode++);
                    }

                    if (data[report]['retLat'] === '' || data[report]['retLat'] === '0.0') {
                        data[report]['retLat'] = '0';
                    }
                    if (data[report]['retLong'] === '' || data[report]['retLong'] === '0.0') {
                        data[report]['retLong'] = '0';
                    }
                    if (data[report]['latitude'] === '' || data[report]['latitude'] === '0.0') {
                        data[report]['latitude'] = '0';
                    }
                    if (data[report]['longitude'] === '' || data[report]['longitude'] === '0.0') {
                        data[report]['longitude'] = '0';
                    }
                    var checkLatitutde = 0;
                    var checkLongitude = 0;
                    var style = '';
                    var timeStyle = 'Style="color:ForestGreen ;"';
                    if (data[report]['longitude'] < 0) {
                        checkLongitude = 1;
                        style = 'Style= "color:red;"';
                    }
                    if (data[report]['latitude'] < 0) {
                        checkLatitutde = 1;

                        style = 'Style= "color:red;"';
                    }
                    if (countryArray[0] < 5) {
                        timeStyle = 'Style="color:Tomato ;"';
                    }
                    locationReport = '/geolocation/geodetailreportV2?retailerId=' + data[report]['retailerId'] + '&retailerName=' + data[report]['retailerName'] + '&personName=' + $('#personId  option:selected').text() + '&personLat=' + data[report]['latitude'] + '&personLong=' + data[report]['longitude'] + '" target="_blank"';
                    //locationReport = "https://maps.google.com/maps?q=" + data[report]['latitude'] + ',' + data[report]['longitude'] + '" target="_blank"';
                    totalQuantity += data[report]['totalQuantity'];
                    var content = '<tr>' +
                        '<td>' + sNo + '</td>' +
                        '<td>' + data[report]['salesId'] + '</td>' +
                        '<td><a ' + style + ' href="' + locationReport + '" >' + data[report]['retailerName'] + '</a></td>' +
                        '<td>' + data[report]['area'] + ', ' + data[report]['zone'] + '</td>' +
                        '<td>' + data[report]['totalQuantity'] + '</td>' +
                        '<td>' + data[report]['status'] + '</td>' +
                        '<td>' + data[report]['lastSaleDate'] + '</td>' +
                        '<td>' + data[report]['lastSale'] + '</td>' +
                        '<td>' + data[report]['avgSale'] + '</td>' +
                        '<td><font ' + timeStyle + '>' + data[report]['timeDuration'] + '</font></td>' +
                        '<td>' + data[report]['time'] + '</td>' +
                        '<td><a href="/Uploads/Photo/' + data[report]['imagePath'] + '" target="_blank"><img src="/Uploads/Photo/' + data[report]['imagePath'] + '"  style="width:25px" /></a></td>' +
                        '<td>' +
                        '<a href="#" onclick="GetDetails(' + data[report]['salesId'] + ', \'/SalesReport/GetIndividualReport\')">' +
                        '<i class="fa fa-external-link" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
                        '</a>|  <a href="#" onclick="Delete(this, ' + data[report]['salesId'] + ' , \'/salesreport/Delete\')">' +
                        '<i class="fa fa-trash" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
                        '</a>|<a href="#stay" onclick="AddLocationToRetailer(' + data[report]['retailerId'] + ',' + data[report]['latitude'] + ',' + data[report]['longitude'] + ')">' +
                        '<i class="fa fa-upload" style="color: rgb(22, 147, 234);font-size:130%"></i>' +
                        '</a></a></td>' +
                        '</tr>';
                    if (data[report]['latitude'] !== 0 && checkLatitutde === 0 && checkLongitude === 0) {
                        locArray.push(new google.maps.LatLng(data[report]['latitude'], data[report]['longitude']));
                    }
                    reportTable.append(content);
                }
                SplitRoute();
                $('#totalQuantity').html(totalQuantity + ' Ltrs');
                reportTable.tablesorter();
            },
            error: function () {
                ShowMessage('Oops.. Something wrong happened!', 'error');
                $('#loader').fadeOut();
            }
        });
    }
}