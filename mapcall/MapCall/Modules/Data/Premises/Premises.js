// Service does servicey things
// Not tied to any controls so it's easy to attach to any weird controls
// Custom controls can use it however they want.

var PremiseService = function() { }
PremiseService.prototype = new Service();
PremiseService.initialize = function(servicePath) {
    PremiseService.prototype.servicePath = servicePath;
};
PremiseService.Response = {
    Success: 1,
    NoPremise: 2
};
PremiseService.getFormattedAddress = function(args) {
    var addy = "";
    addy += args.HouseNumber + " " + args.Street + "<br />";
    if (args.AptNumber) {
        addy += "APT: " + args.AptNumber + "<br />";
    }
    addy += args.City + ", " + args.State + " " + args.Zip + "<br />";

    return addy;
};

var proto = PremiseService.prototype;

proto.getCurrentPremiseOnMeterRecorderById = function(jsonArgs) {
    // args should have parameters: meterRecorderId, success method.
    var reqArgs = {
        data: JSON.stringify(jsonArgs),
        success: function(result) {
            if (result.Response === PremiseService.Response.Success) {
                result.FormattedAddress = PremiseService.getFormattedAddress(result);
            }
            jsonArgs.success(result);
        }
    };
    this.sendAjaxRequest("GetCurrentPremiseOnMeterRecorderById", reqArgs);
};

// Returned object from service should have the following properties:
//    -Response (values: Success, NoPremise)
//
//  if Successful:
//    -PremiseID 
//    -PremiseNumber
//    -HouseNumber
//    -AptNumber
//    -Street
//    -City
//    -Zip
//    -State

proto.getPremiseByMeterId = function(jsonArgs) {
    var reqArgs = {
        data: JSON.stringify(jsonArgs),
        success: function(result) {
            if (result.Response === PremiseService.Response.Success) {
                result.FormattedAddress = PremiseService.getFormattedAddress(result);
            }
            jsonArgs.success(result);
        }
    };
    this.sendAjaxRequest("GetPremiseByMeterId", reqArgs);
};


