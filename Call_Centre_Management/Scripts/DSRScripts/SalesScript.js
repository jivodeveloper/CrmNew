$(function () {
    $('#products').hide();
    $("#div_productDetails").dialog({
        autoOpen: false,
        title: 'Add Products',
        height: 400,
        width: 800,
    });
    Date.prototype.toDateInputValue = (function () {
        var local = new Date(this);
        local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
        return local.toJSON().slice(0, 10);
    });
    BindCurrentDate();
    
});
function BindCurrentDate() {
    var monthNames = ["January", "February", "March", "April", "May", "June",
"July", "August", "September", "October", "November", "December"
    ];
    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();

    $('#salesInfo_SalesDate1').val(curr_date + "-" + monthNames[curr_month]
+ "-" + curr_year);
}

function CheckPerson(typeId) {
    var x = $(typeId).val();
    if (x !== '--Select--') {
        DdlChangePerson(x, '/Sales/GetPersons', '.salesPerson');
    }
}

function DdlChangePerson(val, actionPath, dropDown) {
    var x = $('.stateId').val();
    if (typeof(x) === "object")
    { x = x[0];}
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: val, state: x },
        function (data) {
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

function DdlChangePersonAll(val, actionPath, dropDown) {
    var x = $('.stateId').val();
    if (typeof (x) === "object")
    { x = x[0]; }
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: val, state: x },
        function (data) {
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
function DdlChangePersonWithGroup(val, actionPath, dropDown) {
    var x = $('.stateId').val();
    var y = $('#personType').val();
    if (typeof (x) === "object")
    { x = x.join(", "); }
    if (typeof (y) === "object")
    { y = y.join(", "); }
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: val, state: x, selectedType: y },
        function (data) {
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
function ShowSKUs(val) {
    $('#loader').show();
    $.getJSON("/common/GetProductsByState", {state: val},
        function (data) {
            var list;
            var ul = $('#activeProds');
            ul.empty();
            for (var i in data ) {
                ul.append("<li>" + data[i] + "</li>");
            }
            $('#products').fadeIn();
            $('#loader').fadeOut();
        });
}

function CheckAllowedDate() {
    
    var saleDate = $('#salesInfo_SalesDate1').val();
    if (saleDate !== '') {
        $('#loader').show();
        if ($('#distId').val() !== '') {
            $.ajax({
                url: '/Login/CheckBackDate',
                data: { date: saleDate },
                success: function (data) {
                    
                    if (data === 'OK') {
                        $('#salesInfo_SalesDate').val($('#salesInfo_SalesDate1').val());
                        SubmitSalesReport();
                    } else if (data === "future") {
                        ShowMessage("future entries not allowed", "warning");
                    }
                    else {
                        ShowMessage("Only " + data + " days back entry allowed", "warning");
                    }
                    $('#loader').fadeOut();
                },
                error: function () {
                    ShowMessage("Error", "error");
                    $('#loader').fadeOut();
                }
            });
        }
        else {
            ShowMessage("Select Distributer", "error");
            $('#loader').fadeOut();
        }
    }
}