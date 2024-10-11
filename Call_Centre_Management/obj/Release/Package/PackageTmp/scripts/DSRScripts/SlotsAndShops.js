function GetSlots() {
    var state = $('#locations_stateId').val();
    var zone = $('#locations_zoneId').val();
    if (state != -1 && zone != -1) {
        $('#loader').show();
        $.ajax({
            url: '/Forecast/GetSlots',
            type: 'POST',
            data: { state: state, zone: zone },
            success: function (data) {
                $('#slotsContainer').fadeIn();
                $('#currentSlots').empty();
                var head = '<tr class="info">' +
                            '<th>SLOT ID</th>' +
                            '<th>FROM</th>' +
                            '<th>TO</th>' +
                            '<th>STATE</th>' +
                            '<th>ZONE</th>' +
                            '<th>FROM (LTRS)</th>' +
                            '<th>TO(LTRS)</th>' +
                            '<th>(%)</th>' +
                            '</tr>';
                $('#currentSlots').append(head);
                for (var i in data) {
                    var row = '<tr>' +
                        '<td>' + data[i]["slotId"] + '</td>' +
                        '<td>' + data[i]["fromDate"] + '</td>' +
                        '<td>' + data[i]["toDate"] + '</td>' +
                        '<td>' + data[i]["state"] + '</td>' +
                        '<td>' + data[i]["zone"] + '</td>' +
                        '<td>' + data[i]["quantityFrom"] + '</td>' +
                        '<td>' + data[i]["quantityTo"] + '</td>' +
                        '<td>' + data[i]["percentage"] + '</td>' +
                        '</tr>'
                    $('#currentSlots').append(row);
                }
                $('#fromDateToSubmit').val('');
                $('#toDateToSubmit').val('');
                if (data.length > 0) {
                    $('#fromDateToSubmit').val(data[0]["fromDate"]);
                    $('#toDateToSubmit').val(data[0]["toDate"]);
                }
                
                $('#loader').fadeOut();
            },
            error: function () {
                $('#loader').fadeOut();
                ShowMessage('Ooh.. There was an issue, please try again or report to the Admin!', 'warning');
            }
        });
    }
}

function GetRetailersWithAverage()
{
    var state = $('#locations_stateId').val();
    var zone = $('#locations_zoneId').val();
    var area = $('#locations_areaId').val();
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var fromDateToSubmit = $('#fromDateToSubmit').val();
    var toDateToSubmit = $('#toDateToSubmit').val();
    var key;
    var value;
    if (area != -1) {
        key = "area";
        value = area;
    } else if (zone != -1) {
        key = "zone";
        value = zone;
    }
    else if (state != -1) {
        key = "state";
        value = state;
    }
    
    if (fromDate != "" && toDate != "" && fromDate < toDate && key != undefined && fromDateToSubmit != "") {
        $('#loader').show();
        $('#retailersContainer').fadeOut();
        $.ajax({
            url: "/Forecast/GetRetailersWithAverage",
            data: { fromDate: fromDate, toDate: toDate, fromDateToSubmit: fromDateToSubmit, toDateToSubmit: toDateToSubmit, key: key, value: value },
            success: function (data) {
                FillRetailer(data);
                $('#loader').fadeOut();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    } else {
        ShowMessage("No Slots Created for month", 'warning');
        $('#retailersContainer').fadeOut();
    }
    
}

function FillRetailer(data) {
    var box = $('#retailers');
    box.empty();
    var head = '<tr class="info">' +
                            '<th>RETAILER</th>' +
                            '<th>AVERAGE (LTRS)</th>' +
                            '<th>(%)</th>' +
                            '</tr>';
    box.append(head);
    for (var i = 0; i < data.length; i++) {
        var row = '<tr>' +
                        '<td><input type="button" class="btn btn-info retailerId" id="' + data[i]["retailerId"] + '" value="' + data[i]["retailerName"] + '" disabled /></td>' +
                        '<td><input type="text" class="form-control average" value="' + data[i]["average"] + '" disabled /></td>' +
                        '<td><input type="text" class="form-control percentage" value="' + data[i]["percentage"] + '" /></td>' +
                        '</tr>';
        box.append(row);
    }
    $('#box').fadeIn();
    $('#retailersContainer').fadeIn();
}

function SaveShopsPercentage() {
    var fromDate = $('#fromDateToSubmit').val();
    var toDate = $('#toDateToSubmit').val();
    console.log(fromDate);
    console.log(toDate);
    var retailers = new Array();

    $('#retailers tr').each(function (index, element) {
        var percentage = $(element).find('.percentage').val();
        if (percentage != undefined && percentage != 0 && percentage != "") {
            var rets = new Array();
            var retailerId = $(element).find('.retailerId').attr('id');
            
            rets.push(fromDate);
            rets.push(toDate);
            rets.push(retailerId);
            rets.push(percentage);
            retailers.push(rets);
        }
    });
    if (retailers.length != 0 && fromDate != '' && toDate != '') {
        $('#loader').show();
        $.ajax({
            url: '/Forecast/SaveShopPercentage',
            data: { retailers: JSON.stringify(retailers) },
            success: function (data) {
                ShowMessage(data, 'info');
                //$("#retailers :input").attr("disabled", true);
                $('#loader').fadeOut();
            },
            error: function () {
                ShowMessage('Something went wrong, please contact the Admin', 'warning');
                $('#loader').fadeOut();
            }
        });
    }
}