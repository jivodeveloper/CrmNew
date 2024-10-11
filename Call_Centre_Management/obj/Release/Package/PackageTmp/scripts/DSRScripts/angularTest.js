(function () {
    var app = angular.module('DailySalesReport', ['ngMaterial', 'ngMessages']);


    app.controller("AppController", ['$http', '$timeout', '$q', '$log', function ($http, $timeout, $q, $log) {
        var self = this;
        this.userId = 12;
        this.fromDate = new Date();
        this.isOpen = false;
        this.toDate = new Date();
        this.data = [];
        this.welcome;
        this.isLoading = false;
        this.message;

        this.submitClicked = function () {
            var params = { userId: this.userId, fromDate: this.fromDate, toDate: this.toDate };
            this.isLoading = true;
            $http.get("/androidserver/getDailyReport", { params: params })
                .then(function (response) {
                    self.isLoading = false;
                    self.data = response.data;
                });
        };

        self.simulateQuery = false;
        self.isDisabled = false;

        // list of `state` value/display objects
        self.states = loadAll();
        self.querySearch = querySearch;
        self.selectedItemChange = selectedItemChange;
        self.searchTextChange = searchTextChange;

        self.newState = newState;

        function newState(state) {
            alert("Sorry! You'll need to create a Constitution for " + state + " first!");
        }
        function querySearch(query) {
            var results = query ? self.states.filter(createFilterFor(query)) : self.states,
                deferred;
            if (self.simulateQuery) {
                deferred = $q.defer();
                $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
                return deferred.promise;
            } else {
                return results;
            }
        }

        function searchTextChange(text) {
            $log.info('Text changed to ' + text);
        }

        function selectedItemChange(item) {
            $log.info('Item changed to ' + JSON.stringify(item));
        }

        /**
         * Build `states` list of key/value pairs
         */
        function loadAll() {
            var allStates = 'Alabama, Alaska, Arizona, Arkansas, California, Colorado, Connecticut, Delaware,\
              Florida, Georgia, Hawaii, Idaho, Illinois, Indiana, Iowa, Kansas, Kentucky, Louisiana,\
              Maine, Maryland, Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana,\
              Nebraska, Nevada, New Hampshire, New Jersey, New Mexico, New York, North Carolina,\
              North Dakota, Ohio, Oklahoma, Oregon, Pennsylvania, Rhode Island, South Carolina,\
              South Dakota, Tennessee, Texas, Utah, Vermont, Virginia, Washington, West Virginia,\
              Wisconsin, Wyoming';

            return allStates.split(/, +/g).map(function (state) {
                return {
                    value: state.toLowerCase(),
                    display: state
                };
            });
        }

        /**
         * Create filter function for a query string
         */
        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }

    }]);
})();