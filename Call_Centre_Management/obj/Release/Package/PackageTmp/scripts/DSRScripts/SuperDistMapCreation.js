var selectedDistributorCount = 0;
var allDistributorsCount = 0;

function GetDistributorsByState(stateId) {
    if (stateId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetDistributorsByState",
            data: { state: stateId },
            type: 'GET',
            dataType: 'json',
            cache: false,
            success: function (data) {
                debugger;
                FillDistributor(data);
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
function GetDistributorsByZone(zoneId) {
    if (zoneId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetDistributorsByZone",
            data: { zone: zoneId },
            success: function (data) {
                FillDistributor(data);
                $('#loader').hide();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}
function GetDistributorsByArea(areaId) {
    if (areaId != -1) {
        $('#box').fadeOut();
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetDistributorsByArea",
            data: { area: areaId },
            success: function (data) {
                FillDistributor(data);
                $('#loader').hide();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}
function FillDistributor(data) {
    var box = $('#allDistributors');
    allDistributorsCount = data.length;
    $('#title').html(allDistributorsCount);
    box.empty();
    for (var i = 0; i < data.length; i++) {
        var button = "<p><input id=" + data[i]["retailerID"] + " class=\"btn btn-default draggable \" type=\"button\" onclick=\"AddDistributor(this," + data[i]["retailerID"] + ",'" + data[i]["retailerName"] + "','" + data[i]["state"] + "','" + data[i]["zone"] + "','" + data[i]["area"] + "');\" value= \"" + data[i]["retailerName"] + "\" /></p>";
        box.append(button);
    }
    $('#box').fadeIn();
}
function GetSuperDist(value) {
    debugger;
    if (value != -1) {
        $('#loader').show();
        $.ajax({
            url: "/Distributor/GetSuperDist",
            data: { superId: value },
            success: function (data) {
                debugger;
                $('#selectedDistributors').empty();
                $('#loader').fadeOut();
                var header = '<thead><tr style="background-color:#1693EA;color:white;">' +
                '<th>Id </th>' +
                '<th>Distributor Name </th>' +
                 '<th>State</th>' +
                '<th>Zone </th>' +
                '<th>Area</th>' +
                '<th>Remove</th>' +
               '</tr></thead>';
                $('#selectedDistributors').append(header);

                for (var i = 0; i < data.length; i++) {
                    AddDistributor("", data[i]["distId"], data[i]["distName"], data[i]["state"], data[i]["zone"], data[i]["area"]);
                    
                }
                $('#selectedDistributorsCount').html(data.length);
                var table = $('#selectedDistributors').DataTable();
                table.destroy();
                $('#selectedDistributors').dataTable({
                    "lengthMenu": [[-1], ["All"]]
                });

            },
            error: function (data) {
                debugger;
                $('#loader').fadeOut();
                ShowMessage('ERROR', 'error');
            }
        });
    }
}
function AddDistributor(obj, distId, distName, state, zone, area) {
    debugger;
    $('#box1').show();
    if (selectedDistributorCount == '') selectedDistributorCount = 0;
    if ($("#beatSelectedshop").find('#' + distId).length == 0) {
        debugger;
        $(obj).fadeOut();
        var t = $('#selectedDistributors').DataTable();
        var td = '<td onclick="DeleteR(this)" style="font-weight:bold;font-Size:18px;color:red;cursor:pointer">X</td>';
        var rowNode = t.row.add([distId, distName, state, zone, area, '']).draw().node();
        $(rowNode).find('td').eq(5).append('<input id=' + distId + ' style="margin-left:24px;background-color:#3C8DBC;font-weight:bold;" type="button" value="X" class="btn btn-info btn-xs" onclick="DeleteR(this);">');
        $('#selectedDistributorsCount').html(++selectedDistributorCount);
        if (obj != '') $('#title').html(--allDistributorsCount);

    } else {
        alert("Distributor Already Selected");
    }
}
function DeleteR(obj) {
    debugger;
    $(obj).fadeOut();
    $('#selectedDistributorsCount').html(--selectedDistributorCount);

    $('#allDistributors input[id=' + obj.id + ']').fadeIn();
    $(obj).parent().parent().remove();
    var table = $('#selectedDistributors').DataTable();
    table.row($(obj).parents('tr')).remove().draw();
}
function AddAllDistributors() {
    $('#allDistributors p').each(function () { $(this).find('input').click() });
}

function CheckSaveDist() {
    debugger;
    if ($('input[type=search]').val() == "")
    {
        debugger;
        var distId = $('#super').val();
        if (distId != -1 && distId != undefined) {
            var List = new Array();
            List.length = 0;
            $('#selectedDistributors tr').each(function (i) {
                var x = $(this);
                var id = x.find('input').attr('id');
                if (id != "" && id != undefined) {
                    List.push(id);
                }
            });
            if (List.length > 0) {
                $('#superDist').val(JSON.stringify(List));
                $('#checkSubmitBtn').attr("disabled", "disabled");
                $('#submit').click();
                location.reload();
            } else {
                alert("Please Select Distributor!");
            }
        } else {
            alert("Please Select a Super!");
        }
    } else {
        alert("Please Clear Search Box in selected Distributor Table!");
    }
}