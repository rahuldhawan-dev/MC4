(($) => {
    const DischargeStatus = {
        initialize: () => {
            $('#IsDischargePresent').change(DischargeStatus.onChange);
        },

        onChange: () => {
            const DischargeStatusValue = $('#IsDischargePresent').val();
            var elem = $('div.label:contains("Body Of Water")');
            var ctl = elem.closest('.field-pair');
            if (DischargeStatusValue == 'True') {
                ctl.show();
            }
            else {
                ctl.hide();
            }
        }
    };

    const FlowMeter = {
        initialize: () => {
            $('#HasFlowMeterMaintenanceBeenPerformed').change(FlowMeter.onChange);
        },

        onChange: () => {
            const FlowMeterValue = $('#HasFlowMeterMaintenanceBeenPerformed').val();
            if (FlowMeterValue == 'True') {
                Application.toggleField('#HasDownloadedFlowMeterData', true);
                Application.toggleField('#HasCalibratedFlowMeter', true);
                Application.toggleField('#HasInstalledFlowMeter', true);
                Application.toggleField('#HasRemovedFlowMeter', true);
                Application.toggleField('#HasFlowMeterBeenMaintainedOther', true);
            }
            else {
                Application.toggleField('#HasDownloadedFlowMeterData', false);
                Application.toggleField('#HasCalibratedFlowMeter', false);
                Application.toggleField('#HasInstalledFlowMeter', false);
                Application.toggleField('#HasRemovedFlowMeter', false);
                Application.toggleField('#HasFlowMeterBeenMaintainedOther', false);
            }
        }
    };

    $(DischargeStatus.initialize);
    $(DischargeStatus.onChange);
    $(FlowMeter.initialize);
    $(FlowMeter.onChange);
})(jQuery);
