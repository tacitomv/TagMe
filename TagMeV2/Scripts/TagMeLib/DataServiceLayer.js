/// <reference path="../_references.js" />

function DataService() {
    this.MobileDevices = {
        register: function (device, xhrOptions) {
            data = {
                dataType: 'json',
                type: 'POST',
                url: 'api/MD/register',
                data: device
            }

            $.extend(data, xhrOptions);

            $.ajax(data);
        }
    }
    this.AccessPoints = {
        register: function (device, xhrOptions) {
            data = {
                dataType: 'json',
                type: 'POST',
                url: 'api/AP/register',
                data: device
            }

            $.extend(data, xhrOptions);

            $.ajax(data);
        }
    }
}