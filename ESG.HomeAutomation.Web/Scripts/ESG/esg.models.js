/// <reference path="~/Scripts/ESG/_references.js" />

ESG.Models = ESG.Models || {};

ESG.Models.ViewModelBase = function () {
    var self = this;

    // data
    self.isLoading = ko.observable(true);
};

ESG.Models.Netduino = (function($) {

    var netduino = function(hub) {
        var self = this;
        ESG.Models.ViewModelBase.call(self);

        // data
        self.hub = hub;

        // observables
        self.deviceId = ko.observable("Device not ready");

        self.led = ko.observable(false);

        // computed
        self.connected = ko.computed(function() {
            return self.deviceId() != undefined && self.deviceId().length > 0;
        });

        // behavior
        self.init = function () {

            // register collection of device registration event
            $("#scripts").on("registerDevice", function (event, data) {

                if (data) {
                    var device = $.parseJSON(data);
                    if (device) {
                        self.deviceId(device.DeviceId);
                        self.led(device.Led.Status === true);
                    }
                    
                }

            });

            // poll for specific device
            self.pollForDevice("netduino1");

            self.isLoading(false);

        };

        self.pollForDevice = function(deviceId) {
            self.hub.server.pollForDevice(deviceId);
        };

        self.toggleLed = function() {
            self.hub.server.toggleLed();
        };

        // exec
        self.init();
    };

    return netduino;

})(jQuery);