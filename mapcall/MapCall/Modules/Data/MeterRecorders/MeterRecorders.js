
var MeterRecorderService = function() { }
MeterRecorderService.prototype = new Service();
MeterRecorderService.initialize = function(servicePath) {
    MeterRecorderService.prototype.servicePath = servicePath;
};
MeterRecorderService.Response = {
    NotFound: 0,
    Premise: 1,
    StorageLocation: 2
};
MeterRecorderService.getFormattedAddress = function(args) {
    var addy = "";
    addy += args.HouseNumber + " " + args.Street + "<br />";
    if (args.AptNumber) {
        addy += "APT: " + args.AptNumber + "<br />";
    }
    addy += args.City + ", " + args.State + " " + args.Zip + "<br />";

    return addy;
};

var proto = MeterRecorderService.prototype;

proto.getMeterRecorderCurrentLocation = function(jsonArgs) {
    // args should have parameters: meterRecorderId, success method.

    var reqArgs = {
        data: JSON.stringify(jsonArgs),
        success: function(result) {
            if (result.Response !== MeterRecorderService.Response.NotFound) {
                result.FormattedAddress = MeterRecorderService.getFormattedAddress(result);
            }
            jsonArgs.success(result);
        }
    };
    this.sendAjaxRequest("GetMeterRecorderCurrentLocation", reqArgs);
};