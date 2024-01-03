var WorkOrderAdditional = (function ($) {
    const SERVICE_LINE_RENEWAL = [59, 193, 295];
    const PITCHER_FILTER_REQUIREMENT = [ 59, 295, 222, 307 ];
    const MAIN_BREAK = [74, 80];    
    const SEWER_OVER_FLOWS = [95, 128];
    var m = {
        initialize: function () {
            ELEMENTS = {
                finalWorkDescription: $('#FinalWorkDescription')
            },
            m.initWorkOrderDescription();
        },
        initWorkOrderDescription: function () {
            ELEMENTS.finalWorkDescription.on('change', m.onWorkDescriptionChanged);
            m.onWorkDescriptionChanged();
        },
        onWorkDescriptionChanged: function () {
            const selectedWorkDescriptionId = parseInt(ELEMENTS.finalWorkDescription.val(), 10);
            if (SERVICE_LINE_RENEWAL.includes(selectedWorkDescriptionId)) {
                $('#serviceLineInfo').show();
                if (SERVICE_LINE_RENEWAL.includes(selectedWorkDescriptionId)) {
                    $('.serviceLineRenewalInfo').show();                   
                }
            } else {
                $('#serviceLineInfo').hide();
                $('#complianceInfo').hide();
            }

            if (PITCHER_FILTER_REQUIREMENT.includes(selectedWorkDescriptionId)) {
                $('#complianceInfo').show();
            }
            else {
                $('#complianceInfo').hide();
            }

            if (MAIN_BREAK.includes(selectedWorkDescriptionId)) {
                $('#mainBreakInfo').show();
                $("#WorkOrderMainBreaksTab").show();
            }
            else {
                $('#mainBreakInfo').hide();
                $("#WorkOrderMainBreaksTab").hide();
            }

            if (SEWER_OVER_FLOWS.includes(selectedWorkDescriptionId)) {
                $("#WorkOrderSewerOverflowsTab").show();
            }
            else {
                $("#WorkOrderSewerOverflowsTab").hide();
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);