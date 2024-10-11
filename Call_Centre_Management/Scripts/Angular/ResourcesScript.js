var app = angular.module('resourceApp', []);

app.controller('resourceController', function ($http) {
    var obj = this;
    obj.message = "Hello world";
    obj.data = {};
    obj.model;

    var getResources = function () {
        $('#loader').show();
        $http.get('/Resources/getResourcesByUser').then(function (data) {
            obj.data = data.data;
            $('#loader').hide();
        });
    };

    obj.newResource = function () {
        $("#stateId option:selected").removeAttr("selected");
        obj.model = null;
        obj.modalhead = "Create Resource";
        $('#modal-info').modal('show');

    }

    obj.editResource = function (resource) {
        $("#stateId option:selected").removeAttr("selected");
        obj.model = resource;
        obj.modalhead = "Update Resource";
        $('#modal-info').modal('show');
        
        for(res in resource.stateIds)
        {
            var stateId = stateId = resource.stateIds[parseFloat(res)];
            var a = '#stateId>option:eq(' + stateId+')';
            $('#modal-info').find(a).attr('selected', true);
        }
    }

    obj.deleteResource = function (resource) {
        
        if (confirm("Are you sure?"))
        {
            $('#loader').show();

            $http.post('/Resources/deleteResource', { id: resource.id }).then(function (data) {
                ShowMessage(data.data, 'info');
                $('#loader').hide();
                $('#modal-info').modal('hide');
                obj.data.splice(obj.data.indexOf(resource, 1));
            });
        }
    }

    obj.submitResource = function (resource) {
        
        obj.model.stateId = JSON.stringify($('#stateId').val());
        $('#loader').show();
        $http.post('/Resources/submitResource', { model: obj.model}).then(function (data) {
            ShowMessage(data.data, 'info');
            $('#loader').hide();
            $('#modal-info').modal('hide');
            getResources();
        });
    }

    getResources();
});