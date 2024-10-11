var list;
var selectedOrderIndex;
var giftItems;
$(function () {
    $('#fromDate').val(moment().startOf('month').format('YYYY-MM-DD'));
    $('#toDate').val(moment().format('YYYY-MM-DD'));
    $('#ordersTable').hide();
    getOrdersByRetailer();
});

function getOrdersByRetailer() {
    var retailerId = 0;
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    $('#ordersTable').hide();
    $('#loader').show();
    $.ajax({
        url: '/orders/getOrdersByRetailer',
        type: 'POST',
        data: { retailerId: retailerId, fromDate: fromDate, toDate: toDate, },
        success: function (data) {
            $('#loader').hide();
            list = data;
            fillTable(data);
            $('#ordersTable').show();
        },
        error: function (err) {
            $('#loader').hide();
            ShowMessage('Error', 'error');
            console.log(err);
        }
    });
}

function fillTable(data) {
    $('#ordersTable tbody').children().remove();
    var html = '';
    for (var i = 0; i < data.length; i++) {
        var status = data[i].completionStatus === 1 ? 'Done' : 'Pending';
        html += '<tr>';
        html += '<td>' + data[i].retailerName + ' (' + data[i].retailerId + ')';
        html += '<td>' + data[i].area + ', ' + data[i].zone + ', ' + data[i].state;
        html += '<td>' + moment(data[i].timestamp).format('DD-MM-YYYY hh:mm a') + ' (' + moment(data[i].timestamp).fromNow() + ')';
        html += '<td>' + data[i].totalPieces;
        html += '<td>' + data[i].totalQuantity;
        html += '<td>' + data[i].giftPieces;
        html += '<td>' + status;
        html += '<td><button class="btn btn-primary btn-xs" onclick="getOrderDetails(' + i + ')">View';
    }
    $('#ordersTable tbody').append(html);
    $('#ordersTable').dataTable();
}

function getOrderDetails(i) {
    $('#availedGiftItemsDiv').hide();
    $('#loader').show();
    $.ajax({
        url: '/orders/getOrderItemsByRetailer',
        type: 'POST',
        data: { orderId: list[i].salesId },
        success: function (data) {
            $('#loader').hide();
            console.log(data);
            showModal(data[0], i);
            showAvailedGiftItems(data[1]);
        },
        error: function (err) {
            $('#loader').hide();
            ShowMessage('Error', 'error');
            console.log(err);
        }
    });
}

function showModal(data, index) {
    selectedOrderIndex = index;
    $('#orderItemsTable tbody').children().remove();

    var status = list[index].completionStatus === 1 ? 'Done' : 'Pending';

    $('#retailerNameHeading').html(list[index].retailerName);
    $('#salesId').val(list[index].salesId);
    $('#status').val(status);
    $('#orderDate').val(moment(list[index].timestamp).format('DD-MM-YYYY hh:mm a'));

    var html = '';
    for (var i = 0; i < data.length; i++) {
        html += '<tr>';
        html += '<td>' + data[i].productName + ' (' + data[i].productId + ')';
        html += '<td>' + data[i].pieces;
        html += '<td>' + data[i].totalQuantity;
        html += '<td>' + data[i].isScheme;
    }
    $('#orderItemsTable tbody').append(html);
    $('#orderItemsTable').dataTable();

    $('#orderDetailsModal').modal('show');
}

function showAvailedGiftItems(data) {
    
    $('#availedGiftItemsTable tbody').children().remove();

    var html = '';
    for (var i = 0; i < data.length; i++) {
        html += '<tr>';
        html += '<td>' + data[i].id;
        html += '<td>' + data[i].itemName + ' (' + data[i].itemId + ')';
        html += '<td>' + moment(data[i].createdOn).format('DD-MM-YYYY hh:mm a');
        html += '<td>' + data[i].pieces;
        html += '<td>' + data[i].price;
        html += '<td>' + data[i].totalPrice;
    }
    $('#availedGiftItemsTable tbody').append(html);
    $('#availedGiftItemsDiv').show();
    //$('#availedGiftItemsTable').dataTable();
}

function showGiftModal() {
    console.log(giftItems);
    if (giftItems === undefined) {
        getGiftItems();
    }
    $('#giftItemsTable tbody').children().remove();
    $('#giftModal').modal('show');
}

function addGiftItem() {
    var html = '';
    html += '<tr>';
    html += '<td>' + $('#hiddenGiftSelect').html();
    html += '<td><input type="number" class="form-control" id="pieces" placeholder="Enter Pieces">';
    html += '<td><button class="btn-link" onclick="deleteGift(this)">X';

    $('#giftItemsTable tbody').append(html);
}

function deleteGift(obj) {
    $(obj).parent().parent().remove();
}

function getGiftItems() {
    $('#loader').show();
    $.ajax({
        url: '/orders/getGiftItems',
        type: 'GET',
        success: function (data) {
            $('#loader').hide();
            giftItems = data;
            fillGiftItems(data);
        },
        error: function (err) {
            $('#loader').hide();
            ShowMessage('Error', 'error');
            console.log(err);
        }
    });
}

function fillGiftItems(data) {
    $('#giftItemsSelect').children().remove();
    var html = '';
    for (var i = 0; i < data.length; i++) {
        html += '<option value="'+ data[i].id +'">' + data[i].itemName;
    }
    $('#giftItemsSelect').append(html);
}

function readGiftsFromTable() {
    var giftsList = [];
    $('#giftItemsTable tbody tr').each(function (i, e) {
        var itemId = $(e).find('#giftItemsSelect option:selected').val();
        var pieces = $(e).find('#pieces').val();
        if (pieces === undefined || pieces === '' || pieces === 0) {
            alert('Please fill the form correctly');
            return;
        }
        giftsList.push({ itemId: itemId, pieces: pieces, salesId: list[selectedOrderIndex].salesId, retailerId: list[selectedOrderIndex].retailerId });
    });

    sendGift(giftsList);
}

function sendGift(giftsList) {
    console.log(giftsList);
    $('#loader').show();
    $.ajax({
        url: '/orders/sendGift',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({ param: giftsList }),
        success: function (data) {
            $('#loader').hide();
            getOrderDetails(selectedOrderIndex);
            alert('Gifts Saved');
            $('#giftModal').modal('hide');
        },
        error: function (err) {
            $('#loader').hide();
            ShowMessage('Error', 'error');
            console.log(err);
        }
    });
}