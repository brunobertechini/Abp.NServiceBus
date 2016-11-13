var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('NServiceBus');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);