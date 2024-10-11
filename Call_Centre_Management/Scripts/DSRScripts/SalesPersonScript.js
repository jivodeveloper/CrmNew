function DdlChangeArea(val, actionPath, dropDown) {
    var x = $('#salesPerson_state').val();
    $('#loader').show();
    $.getJSON(actionPath, { zone: val, state: x },
        function (data) {
            //#State is the id of the second dropdownlist
            var x = $(dropDown);
            x.empty();
            $.each(data, function (i, item) {
                x.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            $('#loader').fadeOut();
        });
}

function addRow(data) {
    debugger;
    if (data == "education") {
        $('#education').append(
            "<tr><td><input class='form-control' type='text' id='school'/></td>" +
            "<td><input class='form-control' type='text' id='degree'/></td>" +
            "<td><input class='form-control' type='text' id='studyField'/></td>" +
            "<td><input class='form-control' type='date' id='completionDate'/></td>" +
            "<td><input class='form-control' type='text' id='notes'/></td>" +
            "<td><input class='form-control' type='text' id='interest'/></td>" +
            "<td><i id='education' class='fa fa-trash' style='color:#3C8DBC;font-size:18px' onClick='deleteRow(this)'></i></td></tr>"
            );
    } else if (data == "oldJob") {
        $('#oldJob').append(
            "<tr><td><input class='form-control' type='text'/></td>" +
            "<td><input class='form-control' type='text'/></td>" +
            "<td><input class='form-control' type='date'/></td>" +
            "<td><input class='form-control' type='date' /></td>" +
            "<td><input class='form-control' type='text'/></td>" +
            "<td><i id='oldJob' class='fa fa-trash' style='color:#3C8DBC;font-size:18px' onClick='deleteRow(this)'></i></td></tr>"
            );
    }
}
function deleteRow(r) {
    debugger;
    var i = r.parentNode.parentNode.rowIndex;
    document.getElementById(r.id).deleteRow(i);
}

function CheckStatus(obj) {
    debugger;
    if ($(obj).val() == "Married") {
        $("#married").show();
    }
    else {
        $("#married").hide();
    }
}
