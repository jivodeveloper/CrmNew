var RetailerCount;
var retailer;
$(function () {
    RetailerCount = 1;
    $("#hiddenTable #salesRetailerId").val(RetailerCount);
    $("#tbl_retailer").append($("#hiddenTable").html());
})
function NewRetailer(obj) {
    RetailerCount += 1;
    if (obj.value != "--Select--" || RetailerCount == 1) {
        //var x = $(obj).parent().parent();
        $("#hiddenTable #salesRetailerId").val(RetailerCount);
        $("#tbl_retailer").append($("#hiddenTable").html());
    }
}

function NewRet() {
    RetailerCount += 1;
    $("#tbl_retailer").append($("#hiddenTable").html());
}

function DdlChangeRetailer(obj, actionPath) {
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: obj.value },
        function (data) {
            //#State is the id of the second dropdownlist
            var y = $(obj).parent().parent().find('#retailer');
            y.empty();
            $.each(data, function (i, item) {
                y.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            $('#loader').fadeOut();
        });
}

function DdlChangeSubArea(obj, actionPath) {
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: obj.value },
        function (data) {
            //#State is the id of the second dropdownlist
            var y = $(obj).parent().parent().find('#subArea');
            y.empty();
            $.each(data, function (i, item) {
                y.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            $('loader').fadeOut();
        });
}

function AddProd(obj) {
    //alert("obj value: "+ obj.value);
    
    retailer = obj;
    var state = $('#salesInfo_state').val();
    if (obj.value != '-1' && obj.value != '') {
        $('#loader').show();
        $.ajax({
            url: '/sales/CreateProduct',
            data: {retailerId: obj.value, state: state},
            dataType: 'html',
            success: function (data) {
                $('#div_productDetails').html(data);
                $('#div_productDetails').dialog('open');
                $('#loader').fadeOut();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}
function AddProd2(obj) {
    //alert("obj value: "+ obj.value);
    
    retailer = obj;
    var state = $('#salesInfo_state').val();
    if (obj.value != '-1' && obj.value != '') {
        $('#loader').show();
        $.ajax({
            url: '/sales/CreateProduct',
            data: { retailerId: obj.value, state: state },
            dataType: 'html',
            success: function (data) {
                debugger;
                $('#div_productDetails2').html(data);
                $('#modal-default').modal('show');
                $('#modal-default').on('shown.bs.modal', function () {
                    debugger;
                    $("#product").focus();
                });
                $('#loader').fadeOut();
            },
            error: function () {
                $('#loader').hide();
                ShowMessage("ERROR", "error");
            }
        });
    }
}
function SaveProducts() {
    var q = 0;
    var p = 0;
    var rowList = new Array();
    $("#tbl_product tr").each(function () {
        var colList = new Array();
        var pieces = $(this).find('#pieces').val();
        var quantity = $(this).find('#quantity').val();
        var product = $(this).find('#product').val();
        var stock = $(this).find('#stock').val();
        var cost = $(this).find('#cost').val();
        var parentId = $('#parentId').val();
        if (product != undefined && product != -1) {
            if (pieces != undefined && pieces != '') {
                
                colList.push(product);
                colList.push(pieces);
                colList.push(stock);
                colList.push(cost);
                colList.push(parentId);
                q += (pieces * parseFloat(quantity));
                p += parseInt(pieces);
                rowList.push(colList);
            }
        }
    });
    console.log(rowList);
    //rowList.shift();
    //rowList.pop();
    var prods = JSON.stringify(rowList);
    var ret = $("#div_productDetails #salesRetailerId").val();
    var parent = $(retailer).parent().parent();
    $(retailer).attr('disabled', 'disabled');
    parent.find('#subArea').attr('disabled', 'disabled');
    //parent.find('#distId').attr('disabled', 'disabled');
    parent.find("#productsInfo").val(prods);
    parent.find("#pieces").val(p);
    parent.find("#quantity").val(q);
    var x = parent.find("#retailer").val();
    console.log(x);
    NewRetailer(x);
    $('#modal-default').modal("hide");
    $("#tbl_retailer tbody:last").prev().find("td:eq(3)").find("select").focus();
}

function SubmitSalesReport() {
    
    var personId = $('#salesInfo_personName').val();
    if (personId != null && personId != -1) {
        var rowList = new Array();
        var totalQuantity = 0;
        var totalPieces = 0;
        $("#tbl_retailer tr").each(function () {
            
            var colList = new Array();
            $("td", this).each(function () {
                
                var x = $(this).find('input').val();
                var y = $(this).find('select').val();
                var z = $(this).find('#pieces').val();
                var q = $(this).find('#quantity').val();
                if (x != undefined) { colList.push(x); }
                else if (y != undefined) { colList.push(y); }
                //colList.push(remarks);
                if (q != undefined && q != '') { totalQuantity += parseFloat(q) }
                if (z != undefined && z != '') { totalPieces += parseInt(z) }
            });
            rowList.push(colList);
        });
        //rowList.pop();
        if (confirm("Total Quantity: " + totalQuantity + ", Total Pieces: " + totalPieces)) {
            var rets = JSON.stringify(rowList);
            $("#allRetailers").val(rets);
            $('#submit').click();
        } else { rowList.length = 0; }
    } else {
        alert("Please Select Sales Person");
    }
    //return true;
}

function SubmitUpdatedReport() {
    var rowList = new Array();
    var totalQuantity = 0;
    var totalPieces = 0;
    $("#products tr").each(function () {
        var colList = new Array();
        var x = $(this).find('#key').val();
        var y = $(this).find('#product').val();
        var z = $(this).find('#pieces').val();
        //
        if (x != undefined) {
            colList.push(x);
            colList.push(y);
            colList.push(z);
            rowList.push(colList);
        }
    });
    //rowList.pop();

    if (confirm("Confirm Submit")) {
        var prods = JSON.stringify(rowList);
        $("#productsToSave").val(prods);
        $('#submit').click();
    } else { rowList.length = 0; }
}


function DeleteRow(obj) {
    $(obj).parent().parent().remove();
    $('#VisibleSubmit').focus();
}