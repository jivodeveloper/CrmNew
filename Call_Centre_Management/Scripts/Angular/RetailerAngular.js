var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);

myApp.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
});

myApp.controller('RetailerController', ['$scope', '$http','Excel', function ($scope, $http, Excel) {
    debugger;

    var obj = this;
    obj.allRetailers = [];
    var del = 0;
    obj.deactivatedRetailers = false;
   
    //$('#loader').show();
    //$http({
    //    method: 'POST',
    //    url: '/Retailer/getAllRetailers',
    //    data: { deleted: del }
    //}).then(function (data) {
    //    //debugger;
    //    obj.allRetailers = data.data;
    //    $('#loader').hide();
    //});

    obj.isChecked = function () {
        obj.allRetailers = [];
        del = 1;
        $('#loader1').show();
        $http({
            method: 'POST',
            url: '/Retailer/getAllRetailers',
            data: { deleted: obj.deactivatedRetailers }
        }).then(function (data) {
            //debugger;
            obj.allRetailers = {};
            obj.allRetailers = data.data;
            $('#loader1').hide();
        });
    };

    obj.isChecked();

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };

    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first paghe
    }
    

    obj.exportData = function () {
        debugger;
        var exportData = obj.allRetailers;
        obj.JSONToCSVConvertor(exportData, "Report", true);
        
    };

    obj.JSONToCSVConvertor = function(JSONData, ReportTitle, ShowLabel) {
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
        var CSV = '';    
        
        CSV += ReportTitle + '\r\n';
        
        if (ShowLabel) {
            var row = "";
            
            for (var index in arrData[0]) {

                
                row += index + ',';
            }
            row = row.slice(0, -1);
            
            CSV += row + '\r\n';
        }

        
        for (var i = 0; i < arrData.length; i++) {
            var row = "";
            
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }
            row.slice(0, row.length - 1);
            
            CSV += row + '\r\n';
        }
        if (CSV == '') {        
            alert("Invalid data");
            return;
        }   
        
        var fileName = "MyReport_";
        
        fileName += ReportTitle.replace(/ /g,"_");   

        
        var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
        
        var link = document.createElement("a");    
        link.href = uri;
        
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";
        
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    obj.DeleteRetailer = function (id, objs) {
        if (confirm("ARE YOU SURE?")) {
            $('#loader').show();
            $.ajax({
                url: "/Retailer/Delete",
                data: { id: id },
                success: function (data) {
                    if (data == "Done") {
                        $('#loader').hide();
                        ShowMessage('Entry Deleted', 'info');
                        $(objs).parent().parent().fadeOut();
                    } else {
                        ShowMessage('You Don\'t have permission to delete stuff', 'warning');
                    }
                }, error: function () {
                    $('#loader').hide();
                    ShowMessage('Oops.. Something went wrong', 'error');
                }
            });
        }
    };

}]);
