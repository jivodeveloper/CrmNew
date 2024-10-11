$(function () {
    $("#tbl_product").append($("#tbl_hiddenProduct").html());
    $("#tbl_hiddenParent").dialog({
        autoOpen: false,
        title: "Approved By",
        width: '30%',
    });
});
function NewItems() {
    $("#div_productDetails").dialog("open");
}
function NewProd(val) {
    
    console.log(val);
    if (val != '') {
        $("#tbl_product").append($("#tbl_hiddenProduct").html());
        $("#tbl_product tr:last").find('#product').focus();
        NewItems();
    }
}
function DdlChangeItem(val, actionPath, dropDown) {
    $('#loader').show();
    $.getJSON(actionPath, { selectedValue: val},
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
function GetProductQuantity(obj) {
    $('#loader').show();
    $.getJSON("GetProductQuantity", { prodId: obj.value },
        function (data) {
            
            var parent = $(obj).parent().parent();
            parent.find('#quantity').val(data["quantity"]);
            parent.find('#cost').val(data["mrp"]);
            if ($('#salesInfo_personType').val() != "PROMOTER(MT)" && $('#salesInfo_personType').val() != "PROMOTER(GT)") {
                if (data["isScheme"] == true) {
                    var personId = $('#salesInfo_personName').val();
                    DdlChangeItem(personId, '/Sales/getAllParents', '#parentId');
                    $("#tbl_hiddenParent2").show();
                }
            }
            $('#loader').fadeOut();
        });
}
