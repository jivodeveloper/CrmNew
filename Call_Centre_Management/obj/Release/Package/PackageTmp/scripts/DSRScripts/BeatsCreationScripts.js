var selectedRetailerCount = 0;
var allRetailersCount = 0;

function GetRetailersByState(stateId) {
    if (stateId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Beats/GetRetailersByState",
            data: { state: stateId },
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

function GetRetailersByZone(zoneId) {
    if (zoneId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Beats/GetRetailersByZone",
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
            url: "/Beats/GetRetailersByArea",
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

function GetRetailersBySubArea(subAreaId) {
    if (subAreaId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Beats/GetRetailersBySubArea",
            data: { subArea: subAreaId },
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
    var box = $('#allRetailers');
    allRetailersCount = data.length;
    $('#title').html(allRetailersCount);
    box.empty();
    for (var i = 0; i < data.length; i++) {
        var button = "<p><input id=" + data[i]["retailerID"] + " class=\"btn btn-default draggable \" type=\"button\" onclick=\"AddRetailer(this," + data[i]["retailerID"] + ",'" + data[i]["retailerName"] + "');\" value= \"" + data[i]["retailerID"] + "-" + data[i]["retailerName"] + "\" /></p>";
        box.append(button);
    }
    $('#box').fadeIn();
}



function AddRetailer(obj, retailerId, retailerName) {
    
    if ($("#selectedRetailers").find('#'+retailerId).length == 0) {
        $(obj).fadeOut();
        var box = $('#selectedRetailers').prepend("<tr><td><input id=" + retailerId + " style='margin: 5px 0;' class='btn btn-default' type=\"button\" value=\"" + retailerName + "\" onclick=\"DeleteRet(this);\" /></td></tr>");
        $('#selectedRetailersCount').html(++selectedRetailerCount);
        $('#title').html(--allRetailersCount);
        $('#box1').fadeIn();
    } else {
        alert("Retailer Already Selected");
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

function CheckSaveBeat() {
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
            $('#checkSubmitBtn').attr("disabled", "disabled");
            $('#submit').click();
        } else {
            alert("Please Select Shops!");
        }
    } else {
        alert("Please Select a Sales Person!");
    }
}