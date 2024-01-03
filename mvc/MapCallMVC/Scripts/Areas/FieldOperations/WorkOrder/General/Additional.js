var WorkOrderAdditional = (function ($) {
    const SERVICE_LINE_RENEWAL = [59, 193, 295];
    const PITCHER_FILTER_REQUIREMENT = [59, 295, 222, 307];
    const MAIN_BREAK = [74, 80];
    const SEWER_OVER_FLOWS = [95, 128];
    const OTHER_PITCHER_FILTER_DELIVERY_METHOD = 3;
    var m = {
        initialize: function () {
            ELEMENTS = {
                finalWorkDescription: $('#FinalWorkDescription'),
                initialServiceLineFlushTime: $('#InitialServiceLineFlushTime'),
                hasPitcherFilterBeenProvidedToCustomer: $('#HasPitcherFilterBeenProvidedToCustomer'),
                pitcherFilterCustomerDeliveryMethod: $('#PitcherFilterCustomerDeliveryMethod'),
                pitcherFilterCustomerDeliveryOtherMethod: $('#PitcherFilterCustomerDeliveryOtherMethod')
            },
            m.initWorkOrderDescription();
            m.initInitialServiceLineFlushTime();
            m.initHasPitcherFilterBeenProvidedToCustomer();
            m.initPitcherFilterCustomerDeliveryMethod();
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
        },

        initInitialServiceLineFlushTime: function () {
            ELEMENTS.initialServiceLineFlushTime.on('change', m.onInitialServiceLineFlushTimeChanged);
            m.onInitialServiceLineFlushTimeChanged();
        },
        onInitialServiceLineFlushTimeChanged: function () {
            const enteredInitialServiceLineFlushTime = parseInt(ELEMENTS.initialServiceLineFlushTime.val(), 10);
            if (enteredInitialServiceLineFlushTime < 30) {
                $('#flush-time-below-minimum-message').show();
            } else {
                $('#flush-time-below-minimum-message').hide();
            }
        },

        initHasPitcherFilterBeenProvidedToCustomer: function () {
            ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.on('change', m.onHasPitcherFilterBeenProvidedToCustomerChanged);
            m.onHasPitcherFilterBeenProvidedToCustomerChanged();
        },
        onHasPitcherFilterBeenProvidedToCustomerChanged: function () {
            if (ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.val() === 'True') {
                $('#deliveryDetails').show();
            } else {
                $('#deliveryDetails').hide();
            }
        },

        initPitcherFilterCustomerDeliveryMethod: function () {
            ELEMENTS.pitcherFilterCustomerDeliveryMethod.on('change', m.onPitcherFilterCustomerDeliveryMethodChanged);
            m.onPitcherFilterCustomerDeliveryMethodChanged();
        },
        onPitcherFilterCustomerDeliveryMethodChanged: function () {
            const selectedDeliveryMethodId = parseInt(ELEMENTS.pitcherFilterCustomerDeliveryMethod.val(), 10);
            if (selectedDeliveryMethodId === OTHER_PITCHER_FILTER_DELIVERY_METHOD) {
                $('#otherDeliveryDescription').show();
            } else {
                $('#otherDeliveryDescription').hide();
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);