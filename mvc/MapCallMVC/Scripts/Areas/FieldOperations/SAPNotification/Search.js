var SAPNotificationSearch = (function ($) {
    var sapNotifications = {
        init: function () {
            if (sapNotifications.getParamByName('invokeSearch') === '1')
                $('button[id=Search]').click();
        },
        getParamByName: function (name) {
            var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }
    };
    sapNotifications.init();
    return sapNotifications;
})(jQuery);