//$(function () {
//    $('input, textArea').bind("focusout",function () {
//        console.log('This piece of code just ran');
//        var x = $(this).val().toUpperCase();
//        $(this).val(x);
//    });
//});
var pervPage;
var SavedPages = new Array();
var temp;
function ShowMessage(message, type) {
    var msg = $('#message');
    msg.html('').removeClass();
    msg.append(message);
    switch (type) {
        case 'info':
            msg.addClass("alert alert-dismissable alert-info");
            break;
        case 'warning':
            msg.addClass("alert alert-dismissable alert-warning");
            break;
        case 'error':
            msg.addClass("alert alert-dismissable alert-danger");
            break;
    }
    msg.css("margin-top", 30).css("position", 'fixed').css('width', '80%').css('z-index', '1000');
    msg.fadeIn();
    setTimeout(function () { msg.fadeOut(); }, 3000);
}

function DdlChange(val, actionPath, dropDown) {
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: val },
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
            $('#loader').hide();
        });
}

function DdlChangeState(obj, actionPath, dropDown) {
    
    var value = $(obj).val();
    $('#loader').show();
    
    $.ajax({
        type: "POST",
        datatype: "json",
        traditional: true,
        url: actionPath,
        data: { selectedValue: value },
        success: function (data) {
            //#State is the id of the second dropdownlist
            
            var x = $(dropDown);
            x.empty();
            $.each(data, function (i, item) {
                x.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            $('#loader').hide();
        }
    });
}

function GetStates(actionPath, dropDown) {
    $('#loader').show();
    $.getJSON(actionPath,
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
            $('#loader').hide();
        });
}

function OpenLocation(url, div, title, data) {
    $('#loader').show();
    $.ajax({
        url: url,
        data: { loc: data },
        dataType: 'html',
        success: function (data) {
            $('#loader').hide();
            
            $(div).html(data);
            $(div).dialog({
                title: title
            });
            $(div).dialog('open');
        },
        error: function () {
            $('#loader').hide();
            ShowMessage("ERROR", "error");
        }
    });
}

//$(function () {
//    
//    $("#location").dialog({
//        autoOpen: false,
//        height: 400,
//        width: 450,
//    });
//});
function DdlChangeArea(val, actionPath, dropDown) {
    var x = $('#retailer_state').val();
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
            $('#loader').hide();
        });
}

function GoBackToPrevPage(page)
{
    $('#' + page).html(prevPage);
}

function SaveCurrentPage(page)
{
    prevPage = $('#' + page).html();
}

function SavePage(page) {
    
    SavedPages.push($('#' + page).html());
}

function BackToPrevPage(page) {
    
    var savedPage = SavedPages.pop();
    $('#' + page).html(savedPage);
}

function CheckPersonName(userName, type) {
    if (userName.value !== '') {
        $('#loader').show();
        $.ajax({
            url: '/Common/CheckPersonName',
            data: { userName: userName.value, type: type },
            success: function (data) {

                if (data === 'True') {
                    ShowMessage("Name not available, please choose a new one", "warning");
                    temp = 'true';
                    $(userName).focus();
                }
                else {
                    ShowMessage("Name available", "info");
                }
                $('#loader').fadeOut();
            },
            error: function () {
                ShowMessage("Error", "error");
                $('#loader').fadeOut();
            }
        });
    }
}

function CheckUserName(userName, type) {
    if (userName.value !== '') {
        $('#loader').show();
        $.ajax({
            url: '/Common/CheckUserName',
            data: { userName: userName.value, type: type },
            success: function (data) {
                
                if (data === 'True') {
                    ShowMessage("UserName not available, please choose a new one", "warning");
                    temp = 'true';
                    $(userName).focus();
                }
                else {
                    ShowMessage("UserName available", "info");
                }
                $('#loader').fadeOut();
            },
            error: function () {
                ShowMessage("Error", "error");
                $('#loader').fadeOut();
            }
        });
    }
}

function EmailPopup(fileName) {
    //$("#emailPopup").dialog({
    //    autoOpen: false,
    //    height: 350,
    //    width: 400,
    //});
    $('#fileName').val(fileName);
    //$("#emailPopup").dialog("open");
    $("#emailPopup").modal("show");
}

function sendEmail() {
    
    var fileName = $('#fileName').val();
    var emailId = $('#emailId').val();
    var subject = $('#subject').val();
    var body = $('#body').val();
    if (emailId !== '') {
        $('#loader').show();
        $.ajax({
            url: '/Common/SendEmail',
            type: 'POST',
            data: { emailId: emailId, emailHeader: subject, message: body, fileName: fileName },
            success: function (data) {
                $('#loader').fadeOut();
                ShowMessage("SUCCESS", "info");
                //$("#emailPopup").dialog("close");
                $("#emailPopup").modal("hide");
            },
            error: function () {
                $('#loader').fadeOut();
                ShowMessage("ERROR", "error");
            }
        });
    } else {
        ShowMessage('Enter E-Mail Address', 'warning');
    }
}

function sorting(tableName) {  
    $(tableName).DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
}

function PrintDiv(div, heading) {

    // var prtContent = $('#' + div).clone();
    // prtContent.prepend('<h3>'+heading+'</h3>');
    // prtContent.prepend('<link href="/Content/bootstrap.css" rel="stylesheet">');
    // var WinPrint = window.open('', '', 'left=100,top=100,width=650,height=600');
    // WinPrint.document.write(prtContent.html());
    // //WinPrint.document.close();
    //// WinPrint.focus();
    //// WinPrint.print();
    //// WinPrint.close();
    // setTimeout(function () {
    //     WinPrint.focus();
    //     WinPrint.print();
    //      }, 500);



    var contents = $('#' + div).html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<div style="border: none;">');
    frameDoc.document.write('<div><table style="width:100%"><tr><td align="center" style ="color: black;"><h1>Sales Detail-' + heading + '</h1></td></tr></table></div>');
    frameDoc.document.write('<hr>');
    frameDoc.document.write('<link href="/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Content/bootstrap.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="/Content/PrintDiv.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<div>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</div>');
    frameDoc.document.write('</div>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);

}