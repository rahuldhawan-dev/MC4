/* Base Web Services class */

var Service = function() { };
Service.initialize = function(servicePath) {
    Service.prototype.servicePath = servicePath;
};
Service.prototype = {
    servicePath: "",
    currentRequest: null,
    abort: function() {
        if (this.currentRequest) { this.currentRequest.abort(); }
    },
    sendAjaxRequest: function(serviceCommandName, jsonArgs) {
        this.abort();
        var url = (this.servicePath + "/" + serviceCommandName);
        this.currentRequest = $.ajax({
            type: 'POST',
            url: url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: jsonArgs.data,
            success: function(resp) {
                if (jsonArgs.success) { jsonArgs.success(resp.d); }
            },
            error: function(req, status, errorThrown) {
                alert("Error: " + req.responseText);
            }
        });
    }
};

