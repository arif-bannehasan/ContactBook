(function () {
    'use strict';

    angular.module('contactbook.services', [])
    .run(function ($rootScope, cbPubSubScopeFactory) {
        cbPubSubScopeFactory($rootScope);
    });
})();
