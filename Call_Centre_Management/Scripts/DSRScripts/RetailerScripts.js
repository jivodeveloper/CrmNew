var mainContent;

$(function () {
	debugger;

   $('#retailer_beatId').append('<option value="-1">--</option>');
    $('#retailer_userName, #retailer_password').unbind('focusout');
});


function GetSingleRetailerReport(url) {
    var retailerId = $('#retId').val();
    var retailerName = $('#retName').val();
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
   
    FetchRetailerReport(retailerId, retailerName, fromDate, toDate, url);
}

function GetSingleRetailerReportFromList(retailerId, retailerName, fromDate, toDate, url) {
    mainContent = $('#reportPage').html();
  
    FetchRetailerReport(retailerId, retailerName, null, null, url);
    
}

function FetchRetailerReport(retailerId, retailerName, fromDate, toDate, url) {
    if (retailerId != undefined && retailerId != -1 && fromDate != "") {
        if (toDate == '') toDate = fromDate;
        if (fromDate != "" && new Date(fromDate) <=new Date(toDate)) {
            $('#loader').show();
            $.ajax({
                url: url,
                data: { retailerId: retailerId, retailerName: retailerName, fromDate: fromDate, toDate: toDate },
                success: function (data) {
                    $('#loader').hide();
                    $('#reportPage').html(data);
                   
                },
                error: function () {
                    $('#loader').hide();
                    ShowMessage("ERROR", "error");
                }
            });
        } else {
            alert("Please Enter Correct Dates!!");
        }
    } else {
        alert("Please Enter Retailer And Dates!!");
    }
}

function GoBack() {
    if (mainContent != "" && mainContent != undefined) {
        $('#reportPage').html(mainContent);
    } else {
        location.reload();
    }
}

function CheckUpdateRetailer() {
    BackToPrevPage('mainRetailer');
}

function getBeatsByPerson(personId) {
    if (personId != undefined && personId != -1) {
        $('#loader').show();
        $.ajax({
            url: "/Beats/GetBeatsByPerson",
            data: { id: personId },
            type: "POST",
            success: function (data) {
                
                addBeatsTodropDown(data);
                $('#loader').fadeOut();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}

function addBeatsTodropDown(data) {
    var x = $('.beats');
    x.empty();
    $.each(data, function (i, item) {
        x.append($('<option/>', {
            value: data[i]["beatId"],
            text: data[i]["beatName"]
        }));
    });
}