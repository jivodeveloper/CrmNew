var type = '';
var unReadCount = 0;
$(function () {
    $('.modal').draggable();
    $('#messageDialog').on('hidden.bs.modal', function () {
        $("#loader").hide();
    });
});

function sendMessage(message, state, personId, receiverName) {
    $('#loader').show();
    $('.modal').hide();
    $.ajax({
        url: "/Home/AndroidConsole",
        data: { message: message, stateId: state, personId: personId, receiverName: receiverName },
        type: "POST",
        success: function (data) {
            if (data === "success") {
                ShowMessage("Successfully Sent Message", "info");
            } else {
                ShowMessage("Failed To send Message", "error");
            }
            $('#loader').hide();
        },
        error: function () {
            ShowMessage("Failed To send Message", "error");
            $('.modal').fadeIn();
            $('#loader').hide();
        }
    });
} 
//function sendNotification(data, title, message) {
//    debugger;
//    $('#loader').show();
//    $('.modal').hide();
//    $.ajax({
//        url: "/Home/sendNotification",
//        data: { data: data, title: title, message: message },
//        type: "POST",
//        success: function (data) {
//            if (data == "success") {
//                ShowMessage("Successfully Sent Notification", "info");
//            } else {
//                ShowMessage("Failed To send Notification", "error");
//            }
//            $('#loader').hide();
//        },
//        error: function () {
//            ShowMessage("Failed To send Notification", "error");
//            $('.modal').fadeIn();
//            $('#loader').hide();
//        }
//    });
//}
function CheckMessage() {
    
    var receiverName = '';
    var message = $('textarea#message').val();
    var state = $('#states').val();
    var person = $('#personId').val();
    if (message != '' && type != '') {
        if (type == 'state' && state == -1) {
            ShowMessage('Please select a state', 'warning');
        } else if (type == 'person' && (person == -1 || person == null)) {
            ShowMessage('Please select a person', 'warning')
        } else {
            if (type == 'state') receiverName = $('#states option:selected').text();
            else if (type == 'person') receiverName = $('#personId option:selected').text();
            else receiverName = 'All';
            sendMessage(message, state, person, receiverName)
        }
    } else {
        ShowMessage('Please select message type and type the message correctly', 'warning')
    }
}

function messageForAll() {
    $('#sendToState, #sendToPerson').fadeOut();
    type = 'all';
}
function messageForState() {
    $('#sendToAll, #sendToPerson').fadeOut();
    $('#statesRow').fadeIn();
    type = 'state';
}
function messageForPerson() {
    $('#sendToAll, #sendToState').fadeOut();
    $('#statesRow, #peopleRow').fadeIn();
    type = 'person'
}





function closeNewMessageBox() {
    type = '';
    $('#sendToAll, #sendToState, #sendToPerson').fadeIn();
    $('#statesRow, #peopleRow').fadeOut();
    $("#states").val(-1);
    $("#personId").val(-1);

    //$('textarea#message').val('');
    $('.modal').fadeOut()
}

function searchMessages() {
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();
    var search = $('#searchBox').val();
    
    $('#loader').show();

    $.ajax({
        url: '/home/searchAndroidMessages',
        data: {search: search, fromDate: fromDate, toDate: toDate},
        success: function (data) {
            fillMessagesTable(data);
            $('#loader').fadeOut();
        },
        error: function () {
            ShowMessage("ERROR", "error");
            $('#loader').fadeOut();
        }
    });
}

function fillMessagesTable(data) {
    var messagesTable = $('#messagesTable');
    var header = '<tr class="info">' +
        '<th>S.No.</th>' +
        '<th>SENDER NAME</th>' +
        '<th>RECEIVER NAME</th>' +
        '<th>MESSAGE</th>' +
        '<th>TIMESTAMP</th>' +
        '<th>RECEIVED</th>' +
        '<th>ACTION</th>'+
        '</tr>';

    messagesTable.empty();
    messagesTable.append(header);
    for (var message in data) {
        
        var received;
        if (data[message]['received'] == true) {
            received = '<input type="checkbox" checked disabled />';
        } else {
            received = '<input type="checkbox" disabled />';
        }
        var content = '<tr>' +
            '<td>' + data[message]['id'] + '</td>' +
            '<td>' + data[message]['senderName'] + '</td>' +
            '<td>' + data[message]['receiverName'] + '</td>' +
            '<td>' + data[message]['message'] + '</td>' +
            '<td>' + data[message]['timeStamp'] + '</td>' +
            '<td>' + received + '</td>' +
            '<td><input type="button" value="RESEND" class="btn-info" onclick="resendMessage(this)" /><td>' +
            '</tr>';
        messagesTable.append(content);
    }
}

function searchMessagesFromAndroid() {
    var fromDate = $('#fromDate1').val();
    var toDate = $('#toDate1').val();
    var search = $('#searchBox1').val();
    var read = $('#read').val();
    $('#loader').show();

    $.ajax({
        url: '/home/searchMessagesFromAndroid',
        data: { search: search, fromDate: fromDate, toDate: toDate, read: read },
        success: function (data) {
            fillMessagesFromAndroid(data);
            $('#loader').fadeOut();
        },
        error: function () {
            ShowMessage("ERROR", "error");
            $('#loader').fadeOut();
        }
    });
}

function fillMessagesFromAndroid(data) {
  
    var messagesTable = $('#messagesFromAndroidTable');
    var header = '<thead><tr class="info" style="cursor:pointer;">' +
        '<th>S.No.</th>' +
        '<th>SENDER NAME</th>' +
        //'<th>RECEIVER NAME</th>' +
        '<th>SUBJECT</th>' +
        '<th>DATE</th>' +
        '<th>TIME</th>' +
        '<th>IMAGE</th>' +
        '<th>READ</th>' +
        '<th>ACTION</th>' +
         '</tr></thead><tbody>';

    messagesTable.empty();
    messagesTable.append(header);
    for (var message in data) {

        var received;
        if (data[message]['read'] == true) {
            read = '<input  id="check" type="checkbox" checked disabled />';
           
        } else {
            read = '<input id="check" type="checkbox" disabled />';
            unReadCount += 1;
        }
        var content = '<tr>' +
               
            '<td>' + data[message]['id'] + '</td>' +
            '<td>' + data[message]['senderName'] + '</td>' +
            //'<td>' + data[message]['receiverName'] + '</td>' +
            '<td>' + data[message]['subject'] + '</td>' +
            '<td>' + data[message]['date'] + '</td>' +
            '<td>' + data[message]['time'] + '</td>' +
            '<td><a href="/Uploads/Photo/' + data[message]['imagePath'] + '" target="_blank"><img src="/Uploads/Photo/' + data[message]['imagePath'] + '" style="width:25px" /></a></td>' +
            '<td>' + read + '</td>' +
           
            '<td><input type="button" value="READ" class="btn-info" onclick="readMessage(this)" /><td>' +
            '</tr>';
        messagesTable.append(content);
    }
    messagesTable.tablesorter();
   
}

function resendMessage(obj) {
    $('.modal').fadeIn();
    $('textarea#message').val($(obj).parent().parent().find('td:eq(3)').text());
}

function readMessage(obj) {
    var id = $(obj).parent().parent().find('td:eq(0)').text();
   
    //var countMsg = $(obj).parent().parent().find($('#check')).prop('checked');
  
    $('#loader').show();
    $.ajax({
        url: '/home/GetMessageFromAndroid',
        data: { id: id},
        success: function (data) {
            showMessageFromAndroid(data);
            var checkMsg = $(obj).parent().parent().find('td:eq(6)').children('input').is(':checked')

            if (checkMsg == false) {
                var countUnRead = $('#receviedSpan').text();
                var unReadMsg = countUnRead - 1;
                ShowPendingCount('#receviedSpan', '#receivedTab', unReadMsg);
            }
           
            $(obj).parent().parent().find('td:eq(6)').children('input').attr('checked', true);
            $('#loader').fadeOut();
            
           
        },
        error: function () {
            ShowMessage("ERROR", "error");
            $('#loader').fadeOut();
        }
    });
}
var latitude = '';
var longitude = '';
function showMessageFromAndroid(data) {
    
    
    //$("#messageDialog").dialog({
    //    autoOpen: false,
    //    title: 'MESSAGE FROM: ' + data["senderName"],
    //    height: 400,
    //    width: 600,
    //});
    $("#messageDialog").modal("show");
    $('#subjectForMessage').html(data["subject"]);
    $('#bodyForMessage').html(data['message']);
    $('#imageForMessage').html('<a href="/Uploads/Photo/' + data['imagePath'] + '" target="_blank"><img src="/Uploads/Photo/' + data['imagePath'] + '" alt="NO IMG" style="max-height:300px; max-width:300px;box-shadow: 5px 5px 5px grey;border-radius: 5px;" /></a>');
    
    latitude = data["latitude"];
    longitude = data["longitude"];
    if (latitude == "" && longitude == "") {
        $('#locationtd').hide();
    }
    else {
        $('#locationtd').show();
    }
    //LatLang = new google.maps.LatLng(data["latitude"], data["longitude"]);
    //marker = new google.maps.Marker({
    //    position: LatLang,
    //    map: map,
    //    title: "title",

    //});   

    //infowindow = new google.maps.InfoWindow({
    //    content: "content"
    //});

    //infowindow.open(map, marker);
    
    $('#messageDialog').dialog('open');
}

function GetLocation() {
    window.open('https://maps.google.com/maps?q='+latitude+','+longitude+'', '_newtab');
    //location.href = 'https://maps.google.com/maps?q='+latitude+','+longitude;
}