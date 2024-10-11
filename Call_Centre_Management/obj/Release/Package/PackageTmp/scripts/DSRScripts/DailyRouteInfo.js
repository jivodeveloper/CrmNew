var locArray = new Array();
var directionsDisplay;
var directionsService;
var map;
var MINIMUM_DISTANCE = 200;
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
    };
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    directionsDisplay.setMap(map);

}

function GetLocationReport() {
    var personId = $('#personId').val();
    var date = $('#date').val();
    var counter = 1;
    if (personId != '' && date != '') {
        $('#loader').fadeIn();
        $.ajax({
            url: '/GeoLocation/GetLocationReport',
            data: { personId: personId, date: date },
            success: function (data) {
                data = filterLocations(data);
                var reportTable = $('#locationReportsTable');
                reportTable.empty();
                var header = '<thead><tr class="info" style="cursor:pointer;">' +
                    '<th>S.No.</th>' + '<th>LCTN</th>' + '<th>LOCATION</th>' + '<th>BATTERY</th>' + '<th>FROM</th><th>TO</th><th>Diff</th>' + '<th>ACTIONS</th>' +
                    '</tr></thead><tbody>';
                reportTable.append(header);

                var sno = 0;
                var charCode = 65;
                for (var report in data) {

                    var sNo;
                    var locationButton = "";
                    if ((data[report]['latitude'] == '0.0' || data[report]['latitude'] == '0') && (data[report]['longitude'] == '0' || data[report]['longitude'] == '0.0')) {
                        sNo = "*";
                    } else {
                        sNo = String.fromCharCode(charCode++);
                    }

                    var retailerId = data[report]['retailerId'];
                    if (+retailerId === 0) {
                        var address = getAddress(data[report]['latitude'], data[report]['longitude']);
                        locationButton = '<a style="color: yellow;" href="https://maps.google.com/maps?q=' + data[report]['latitude'] + ',' + data[report]['longitude'] + '" target="_blank" title="Accuracy: ' + data[report]['accuracy'] + ' M">'+ address +'</a>';
                    } else {
                        var personBeat = data[report]['personBeat'];
                        var retailerBeat = data[report]['retailerBeatId'];
                        var style = "";
                        if(personBeat != retailerBeat) {
                            style = 'style="color:red;"';
                        }
                        
                        var locationReport = '/geolocation/geodetailreportV2?retailerId=' + data[report]['retailerId'] + '&retailerName=' + data[report]['retailerName'] + '&personName=' + $('#personId  option:selected').text() + '&personLat=' + data[report]['latitude'] + '&personLong=' + data[report]['longitude'] + '" target="_blank"';
                        locationButton = '<a href="' + locationReport + '" '+ style +' >' + data[report]['retailerName'] + ' (' + GetDistance(data[report]['retLat'], data[report]['retLong'], data[report]['latitude'], data[report]['longitude']) + ' M)</a>';
                    }
                    var content = '<tr>' +
                        '<td>' + counter++ + '</td>' + '<td>' + sNo + '</td>' +
                        '<td>' + locationButton + '</td>' +
                        '<td>' + data[report]['battery'] + '</td>' +
                        '<td>' + data[report]['timeStamp'] + '</td>' +
                        '<td>' + data[report]['lastTime'] + '</td>' +
                        '<td>' + data[report]['timeDiff'] + '</td>';

                    var salesId = data[report]['salesId'];
                    if (salesId != -1) {
                        content += '<td><a href="#stay" onclick="GetDetails(' + data[report]['salesId'] + ', \'/SalesReport/GetIndividualReport\')">DETAILS</a>|' +
                            '<a href="#stay" onclick="Delete(this, ' + data[report]['salesId'] + ')">DELETE</a></td>';
                    } else {
                        content += '<td>NONE</td>';
                    }

                    content += '</tr>';
                    if (data[report]['latitude'] != 0) {
                        locArray.push({
                            latLong: new google.maps.LatLng(data[report]['latitude'], data[report]['longitude'])
                        });
                    }
                    reportTable.append(content);
                }
                reportTable.tablesorter();
                SplitRoute();
                $('#loader').fadeOut();
            },
            error: function (data) {
                ShowMessage('Oops.. Something wrong happened!', 'error');
                $('#loader').fadeOut();
            }
        });
    }
}

function filterLocations(data) {
    var locations = new Array();
    var prevLocation;
    for (var i in data) {
        currentLocation = data[i];
        currentLocation['lastTime'] = '';
        currentLocation['timeDiff'] = '';
        if (+i === 0) {
            locations.push(currentLocation);
            prevLocation = currentLocation;
        } else {
            var distance = GetDistance(prevLocation["latitude"], prevLocation["longitude"], currentLocation['latitude'], currentLocation['longitude']);
            if (distance > MINIMUM_DISTANCE || +currentLocation["salesId"] !== -1) {
                locations.push(currentLocation);
                prevLocation = currentLocation;
            } else {
                prevLocation['lastTime'] = currentLocation["timeStamp"];
                prevLocation['battery'] = currentLocation["battery"];

                var now = moment(prevLocation['dateTime']); //todays date
                var end = moment(currentLocation['dateTime']); // another date
                var duration = moment.duration(end.diff(now));
                var hours = duration.asHours();

                if (hours > 1) {
                    prevLocation['timeDiff'] = hours.toFixed(2) + ' Hours';
                } else {
                    prevLocation['timeDiff'] = duration.asMinutes().toFixed(2) + ' Minutes';
                }
            }
        }
    }

    return locations;
}

function GetDistance(firstLat, firstLong, lastLat, lastLong) {
    var shopLatLang = new google.maps.LatLng(firstLat, firstLong);
    var personLatLang = new google.maps.LatLng(lastLat, lastLong);
    return Math.round(google.maps.geometry.spherical.computeDistanceBetween(shopLatLang, personLatLang));
}

function getAddress(latitude, longitude) {
    var address = "";
    var lat = parseFloat(latitude);
    var lng = parseFloat(longitude);
    var latlng = new google.maps.LatLng(lat, lng);
    var geocoder = geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                address = results[1].formatted_address;
            }
        }
    });
    return address;
}

function SplitRoute() {
    initialize();
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
                waypoint: {
                    location: stops[j].latLong,
                    stopover: true,
                },
                item: stops[j]
            });
            if (subitemsCounter == itemsPerBatch)
                break;
        }

        itemsCounter += subitemsCounter;
        batches.push(subBatch);
        wayptsExist = itemsCounter < stops.length;
        // If it runs again there are still points. Minus 1 before continuing to
        // start up with end of previous tour leg
        itemsCounter--;
    }
    console.log(batches);
    calcRoute(batches);
}

function calcRoute(batches) {
    var totalDistance = 0;
    var combinedResults;
    var unsortedResults = [{}]; // to hold the counter and the results themselves as they come back, to later sort
    var directionsResultsReturned = 0;

    for (var k = 0; k < batches.length; k++) {
        var lastIndex = batches[k].length - 1;
        var start = batches[k][0].waypoint.location;
        var end = batches[k][lastIndex].waypoint.location;

        // trim first and last entry from array
        var waypts = [];
        waypts = batches[k];
        waypts.splice(0, 1);
        waypts.splice(waypts.length - 1, 1);


        var request = {
            origin: start,
            destination: end,
            waypoints: waypts.map(x => x.waypoint),
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
                        // addMarker(route.legs[i].end_location, i, map);
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