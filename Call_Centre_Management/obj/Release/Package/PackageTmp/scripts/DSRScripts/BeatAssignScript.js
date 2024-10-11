//$(function () {
//    $("#popup").dialog({
//        autoOpen: false,
//        title: 'SHOPS',
//        height: 500,
//        width: 600,
//    });
//});
$(document).ready(function () {

    /* initialize the calendar
    -----------------------------------------------------------------*/
    
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        editable: true,
        disableDragging: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (date, allDay, jsEvent) { // this function is called when something is dropped
            
            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);
            //if (date >= currentDate) {
                // retrieve the dropped element's stored Event Object
                var originalEventObject = $(this).data('eventObject');

                // we need to copy it, so that multiple events don't have a reference to the same object
                var copiedEventObject = $.extend({}, originalEventObject);

                // assign it the date that was reported
                copiedEventObject.start = date;
                copiedEventObject.allDay = allDay;

                var overlap = CheckEventExists(date);

                // render the event on the calendar
                // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                if (!overlap) {
                    $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);
                }
            //}

        },
        eventClick: function (event) {
            
            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);
            //if (event.start >= currentDate) {
                $('#calendar').fullCalendar('removeEvents', event._id);
            //}
        }
    });


});

function CheckEventExists(date) {
    var start = new Date(date);
    var end = new Date(date);
    var overlap = $('#calendar').fullCalendar('clientEvents');
    for (var i in overlap) {
        var edate = new Date(overlap[i]["_start"])
        var actualDate = new Date(date);
        if (edate.toLocaleDateString() == actualDate.toLocaleDateString()) {
            return true;
        }
    }
    return false;
}

function CheckBeats() {
    var personId = $('#personId').val();
    if (personId != undefined && personId != -1) {
        $('#loader').show();
        $.ajax({
            url: "/Beats/GetBeatsByPerson",
            data: { id: personId },
            type: "POST",
            success: function (data) {
                FillBeats(data);
                $('#loader').fadeOut();
            }
        });
    }
}

function FillBeats(data) {
    var table = $('#external-events');
    table.empty();
    for (var i = 0; i < data.length; i++) {
        var val = '<tr>' +
        '<td>' +
        '<div id="' + data[i]["beatId"] + '" class="btn btn-danger btn-sm external-event">' + data[i]["beatName"] + ' ' + data[i]["beatId"] + '</div> | ' +
        '<input type="button" value="VIEW" onclick="GetViewBeatPage(' + data[i]["beatId"] + ')" class="btn btn-primary btn-xs" /> ' +
        '</td>' +
        '</tr>';
        table.append(val);
    }
    table.find('div').draggable({ revert: true, helper: "clone" });
    $('#external-events div.external-event').each(function () {

        // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
        // it doesn't need to have a start or end
        var eventObject = {
            title: $.trim($(this).text()),
            //id: $.trim($(this).val())// use the element's text as the event title
        };

        // store the Event Object in the DOM element so we can get to it later
        $(this).data('eventObject', eventObject);

        // make the event draggable using jQuery UI
        $(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });
}

function SubmitBeats()
{
    var personId = $('#personId').val();
    var beats = new Array();
    
    var overlap = $('#calendar').fullCalendar('clientEvents');
    for (var i in overlap) {
        var beat = new Array();
        var matches = overlap[i]["title"].match(/\d+$/);
        var beatId = matches[0];
        var date = new Date(overlap[i]["start"]);
        var day = date.getDate() + 1;
        var month = date.getMonth();
        var year = date.getFullYear();
        date = new Date(year, month, day);
        beat.push(beatId);
        beat.push(date);
        beats.push(beat);
    }
    if (beats.length > 0) {
        
        alert(beats.length);
        var str = JSON.stringify(beats);
        $('#loader').show();
        $.ajax({
            url: "/beats/SaveAssignedBeats",
            type: "POST",
            data: { data: str, personId: parseInt(personId) },
            success: function (data) {
                //alert("SAVED");
                ShowMessage('SAVED', 'info');
                $('#loader').fadeOut();
            },
            error: function () {
                ShowMessage('ERROR', 'error');
                $('#loader').fadeOut();
            }
        });
    } else {
        alert('PLEASE ASSIGN BEATS FIRST');
    }
}

function GetViewBeatPage(id) {
    $('#loader').show();
    $.ajax({
        url: "/Beats/GetSingleBeat",
        data: { id: id },
        success: function (data) {
            FillShops(data);
            //$("#popup").dialog('open');
            $("#popup").modal('show');
            $('#loader').fadeOut();
        }
    });
}

function FillShops(data) {
    var header = '<tr class="info">' +
        '<th>SHOP ID</th>' +
        '<th>SHOP NAME</th>' +
        //'<th>STATE</th>' +
        //'<th>ZONE</th>' +
        '<th>AREA</th>' +
        '</tr>';
    $("#Beatshops").empty();
    $("#Beatshops").append(header);
    for (var i = 0; i < data.length; i++) {
        var val = '<tr>' +
        '<td>' + data[i]["shopId"] + '</td>' +
        '<td>' + data[i]["shopName"] + '</td>' +
        //'<td>' + data[i]["state"] + '</td>' +
        //'<td>' + data[i]["zone"] + '</td>' +
        '<td>' + data[i]["area"] + '</td>' +
        '</tr>';
        $("#Beatshops").append(val);
    }
}

function GetAssignedBeats(personId) {
    $.ajax({
        url: "/beats/GetAssignedBeats",
        data: { personId: personId },
        dataType: "JSON",
        success: function (data) {

            $('#calendar').fullCalendar('removeEvents');
            var events = new Array();
            for (var i in data) {
         
                var event = new Object();
                event.title = data[i]["title"];
                var datestring = data[i]["start"];
                var dt = new Date(datestring);
                event.start = dt;
                //event.start =new Date(data[i]["start"]);
                event.allDay = true;
                $('#calendar').fullCalendar('renderEvent', event);
                events.push(event);
            }
            //$('#calendar').fullCalendar('addEventSources', data);
             //$('#calendar').fullCalendar('renderEvents', events);
        }
    });
}

