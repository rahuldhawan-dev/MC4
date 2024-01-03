var WorkOrderFinalization = (function ($) {
    var m = {
        UNKNOWN_METER_LOCATION_ID: 3,
        validateMeterLocation: function (val) {
            const selectedMeterLocationId = parseInt(($('#MeterLocation')).val(), 10);
            const showMeterLocationError = WorkOrderFinalization.UNKNOWN_METER_LOCATION_ID === selectedMeterLocationId;
            return !showMeterLocationError;
        },
        init: function () {
            m.initBtnFinalize();
        },
        initBtnFinalize: function () {
            const submitButton = $('#Submit');
            submitButton.on('click', m.onBtnFinalizeClick);
        },
        onBtnFinalizeClick: function () {
            const serviceFrame = $('#serviceFrame');
            if (serviceFrame.length && serviceFrame.get(0).contentWindow.Application.routeData.action === 'Edit') {
                alert("Please save the service first!");
                $('a[href="#serviceTab"]').click();
                return false;
            }
        }
    };

    $(document).ready(m.init);
    return m;
})(jQuery);