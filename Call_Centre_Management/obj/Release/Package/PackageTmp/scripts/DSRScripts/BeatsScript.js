//$(function () {
//    $("#popup").dialog({
//        autoOpen: false,
//        title: 'SHOPS',
//        height: 500,
//        width: 600,
//    });
//});
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
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}

function FillBeats(data) {
    var table = $('#beatsList');
    table.empty();
    var header = '<tr class="info">' +
        '<th>BEAT ID</th>' +
        '<th>BEAT NAME</th>' +
         '<th>SHOP COUNTS</th>' +
        '<th>ACTIONS</th>' +
        '</tr>';
    table.append(header);
    var shopCount = 0;
    for (var i = 0; i < data.length; i++) {
        shopCount += data[i]["shopCount"];
        var val = '<tr>' +
        '<td>' + data[i]["beatId"] + '</td>' +
        '<td>' + data[i]["beatName"] + '</td>' +
         '<td>' + data[i]["shopCount"] + ' (Shops)</td>' +
        '<td>'+
        '<input type="button" value="VIEW" onclick="GetViewBeatPage(' + data[i]["beatId"] + ')" class="btn btn-primary btn-xs" /> | ' +
        '<input type="button" value="MOVE" onclick="GetMoveBeatPage(' + data[i]["beatId"] + ')" class="btn btn-primary btn-xs" /> | ' +
        '<input type="button" value="EDIT" onclick="GetEditBeatPage(' + data[i]["beatId"] + ')" class="btn btn-primary btn-xs" /> | ' +
        '<input type="button" value="DELETE" onclick="DeleteBeat(this, ' + data[i]["beatId"] + ')" class="btn btn-primary btn-xs" />' +
        '</td>' +
        '</tr>';
        table.append(val);
    }
    $('#shopCount').text(shopCount);
     
}

function GetViewBeatPage(id) {
    $('#loader').show();
    //if ($(".modal-content #moveBtn").length == 1) {
    //    $(".modal-content #moveBtn").remove()
    //    $('.modal-content').removeAttr('style');
    //}
    $.ajax({
        url: "/Beats/GetSingleBeat",
        data: { id: id },
        success: function (data) {
            FillShops(data);
             //$("#popup").dialog('open');
             $("#popup").modal('show');
            //$("#popup").dialog({
            //    autoOpen: false,
            //    title: 'Beat Details',
            //    width: 1000,
            //})
            $('#loader').fadeOut();
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}

function GetMoveBeatPage(id)
{
    var body = $('#table').html();
    body = body.replace("'#personId'", "'#mpersonId'")
    body = body.replace('"personId"', '"mpersonId"')
    //$('#Beatshops').parent().append('')
    $("#Beatshops2").empty();
    $("#Beatshops2").append(body);
    if ($("#modal_content #moveBtn").length == 1) {
        $("#modal_content #moveBtn").remove()
    }
    $('#Beatshops2').parent().append('<div class="col-lg-12"><button id="moveBtn" class="pull-right" onclick="moveBeat(' + id + ')">Move</button></div>')
    $("#excelDownload2").hide();
    $("#popup2").modal('show');
}
function moveBeat(beatId)
{
    var personId = $('#modal_content').find('#mpersonId').val();
    if(personId==-1)
    {
        alert("Select a person to assign with this beat");
    }
    else {
        if(confirm("Are you sure"))
        {
            $("#popup2").modal('hide');
            $.ajax({
                url: "/Beats/MoveBeat",
                data: { beatid: beatId, personId: personId },
                success: function (data) {
                    location.reload();
                }
            });
        }
    }
}
function FillShops(data) {
   
    var header = '<thead><tr class="info">' +
        '<th>SHOP ID</th>' +
        '<th>SHOP NAME</th>' +
        //'<th>STATE</th>' +
        //'<th>ZONE</th>' +
        '<th>AREA</th>' +
        '</tr></thead>';
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
   
    var table = $('#Beatshops').DataTable();
    table.destroy();
    $('#Beatshops').DataTable();
   
}
  

function DeleteBeat(obj, id) {
    var confrm = confirm("ARE YOU SURE YOU WANT TO DELETE THIS BEAT ?");
    if (confrm == true) {
        $('#loader').show();
        $.ajax({
            url: "/Beats/DeleteBeat",
            data: { id: id },
            success: function () {
                $(obj).parent().parent().remove();
                $('#loader').fadeOut();
                ShowMessage("Entery deleted", 'info');
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
   
}

var prevPage;

function GetEditBeatPage(id) {
    prevPage = $('#mainContent').html();
    $('#loader').show();
    $.ajax({
        url: "/Beats/Edit",
        data: { id: id },
        success: function (data) {
            debugger;
            $('#mainContent').html(data);
        }
    });
    $('#message').hide();
    $('#loader').fadeOut();
}

function BackToViewBeat() {
    $('#loader').show();
    $('#mainContent').html(prevPage);
    $('#loader').fadeOut();
    $("#popup").modal('show');
    //$("#popup").dialog({
    //    autoOpen: false,
    //    title: 'SHOPS',
    //    height: 500,
    //    width: 600,
    //});
}

function SaveEditedBeat() {
    var personId = $('#beat_personId').val();
    if (personId != -1 && personId != undefined) {
        var List = new Array();
        List.length = 0;
        $('#selectedRetailers tr').each(function (i) {
            var x = $(this);
            var id = x.find('input').attr('id');
            if (id != "" && id != undefined) {
                List.push(id);
            }
        });
        if (List.length > 0) {
            $('#beatShops').val(JSON.stringify(List));
            $('#submit').click();
        } else {
            alert("Please Select Shops!");
        }
    } else {
        alert("Please Select a Sales Person!");
    }
}
