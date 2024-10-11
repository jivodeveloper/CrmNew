var selectedRetailerCount = 0;
var allRetailersCount = 0;

function GetRetailersByState(stateId) {
    if (stateId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetRetailersByState",
            data: { state: stateId },
            type: 'GET',
            dataType: 'json',
            cache: false,
            success: function (data) {
                debugger;
                FillRetailer(data);
                $('#loader').hide();
            },
            error: function (data) {
                debugger;
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}

function GetRetailersByZone(zoneId) {
    if (zoneId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetRetailersByZone",
            data: { zone: zoneId },
            success: function (data) {
                FillRetailer(data);
                $('#loader').hide();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}

function GetRetailersByArea(areaId) {
    if (areaId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetRetailersByArea",
            data: { area: areaId },
            success: function (data) {
                FillRetailer(data);
                $('#loader').hide();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}

function FillRetailer(data) {
    debugger;
    var box = $('#allRetailers');
    allRetailersCount = data.length;
    $('#title').html(allRetailersCount);
    box.empty();
    for (var i = 0; i < data.length; i++) {
      //  var button = "<p><input id=" + data[i]["retailerID"] + " class=\"btn btn-default draggable \" type=\"button\" onclick=\"AddRetailer(this," + data[i]["retailerID"] + ",'" + data[i]["retailerName"] + "');\" value= \"" + data[i]["retailerName"] + "\" /></p>";
        var button = "<p><input id=" + data[i]["retailerID"] + " class=\"btn btn-default draggable \" type=\"button\" onclick=\"AddRetailer(this," + data[i]["retailerID"] + ",'" + data[i]["retailerName"] + "','" + data[i]["state"] + "','" + data[i]["zone"] + "','" + data[i]["area"] + "');\" value= \"" + data[i]["retailerName"] + "\" /></p>";
        box.append(button);
    }
    $('#box').fadeIn();
}

function AddRetailer(obj, retailerId, retailerName, state, zone, area) {
    debugger;
    $('#box1').show();
    if (selectedRetailerCount == '') selectedRetailerCount = 0;
    if ($("#beatSelectedshop").find('#' + retailerId).length == 0) {
        debugger;
        $(obj).fadeOut();
        var t = $('#selectedRetailers').DataTable();
        var td = '<td onclick="DeleteR(this)" style="font-weight:bold;font-Size:18px;color:red;cursor:pointer">X</td>';
        var rowNode = t.row.add([retailerId, retailerName, state, zone, area, '']).draw().node();
        $(rowNode).find('td').eq(5).append('<input id=' + retailerId + ' style="margin-left:24px;background-color:#3C8DBC;font-weight:bold;" type="button" value="X" class="btn btn-info btn-xs" onclick="DeleteR(this);">');
        $('#selectedRetailersCount').html(++selectedRetailerCount);
        if (obj != '') $('#title').html( --allRetailersCount);


    } else {
        alert("Retailer Already Selected");
    }
}

function DeleteR(obj) {
    
    $(obj).fadeOut();
    $('#selectedRetailersCount').html(--selectedRetailerCount);
   
    //$('#title').html("All Retailers : " + ++allRetailersCount);

    $('#allRetailers input[id=' + obj.id + ']').fadeIn();
    $(obj).parent().parent().remove();
    var table = $('#selectedRetailers').DataTable();
    table.row($(obj).parents('tr')).remove().draw();
}
//function AddRetailer(obj, retailerId, retailerName, area) {
//    
//    if ($("#selectedRetailers").find('#' + retailerId).length == 0) {
//        $(obj).fadeOut();
//        var box = $('#selectedRetailers').prepend("<tr><td><input id=" + retailerId + " style='margin: 5px 0;' class='btn btn-default' type=\"button\" value=\"" + retailerName + " - " + area + "\" onclick=\"DeleteRet(this);\" /></td></tr>");
//        $('#selectedRetailersCount').html(++selectedRetailerCount);
//        $('#title').html(--allRetailersCount);
//        $('#box1').fadeIn();
//    } else {
//        alert("Retailer Already Selected");
//    }
//}

function GetDistShops(value) {
    if (value != -1) {
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetDistShops",
            data: { distId: value },
            success: function (data) {
                $('#selectedRetailers').empty();
                $('#loader').fadeOut();
                var header = '<thead><tr style="background-color:#1693EA;color:white;">' +
                '<th>Id </th>' +
                '<th>Shop Name </th>' +
                 '<th>State</th>' +
                '<th>Zone </th>' +
                '<th>Area</th>' +
                '<th>Remove</th>' +
               '</tr></thead>';
                $('#selectedRetailers').append(header);
               
                for (var i = 0; i < data.length; i++) {
                    //AddRetailer(null, data[i]["shopId"], data[i]["retailerName"], data[i]["area"]);
                    AddRetailer("", data[i]["shopId"], data[i]["retailerName"], data[i]["state"], data[i]["zone"], data[i]["area"]);
                    //var button = "<p><input id=" + data[i]["retailerID"] + " class=\"btn btn-default draggable \" type=\"button\" onclick=\"AddRetailer(this," + data[i]["retailerID"] + ",'" + data[i]["retailerName"] + "');\" value= \"" + data[i]["retailerName"] + "\" /></p>";
                    //box.append(button);
                }
                $('#selectedRetailersCount').html(data.length);
                var table = $('#selectedRetailers').DataTable();
                table.destroy();
                $('#selectedRetailers').dataTable({
                    "lengthMenu": [[-1], ["All"]]
                });
                
            },
            error: function () {
                $('#loader').fadeOut();
                ShowMessage('ERROR', 'error');
            }
        });
    }
}

function AddAllRetailers() {
    $('#allRetailers p').each(function () { $(this).find('input').click() });
}

function DeleteRet(obj) {
    $(obj).fadeOut();
    $('#selectedRetailersCount').html(--selectedRetailerCount);
    $('#title').html(++allRetailersCount);
    $('#allRetailers input[id=' + obj.id + ']').fadeIn();
    $(obj).remove();
}

function CheckSaveShops() {
    debugger;
    if ($('input[type=search]').val() == "")
    {
        var distId = $('#distributor').val();
        if (distId != -1 && distId != undefined) {
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
                $('#distShops').val(JSON.stringify(List));
                $('#checkSubmitBtn').attr("disabled", "disabled");
                $('#submit').click();
                location.reload();
            } else {
                alert("Please Select Shops!");
            }
        } else {
            alert("Please Select a Sales Person!");
        }
    }
    else {
        alert("Please Clear Search Box in selected Retailers Table!");
    }
}