var WorkOrderAdditional = (function ($) {
    let ELEMENTS = {};
    const SERVICE_LINE_RENEWAL = [59, 193, 295];
    const OTHER_PITCHER_FILTER_DELIVERY_METHOD = 3;
    const MAIN_BREAK = [74, 80];    
    const SEWER_OVER_FLOWS = [95, 128];
    var m = {
        initialize: function () {
            ELEMENTS = {
                appendNotes: $('#AppendNotes'),
                appendNotesHiddenField: $('#AppendNotesField'),
                finalWorkDescription: $('#FinalWorkDescription'),
                initialServiceLineFlushTime: $('#InitialServiceLineFlushTime'),
                hasPitcherFilterBeenProvidedToCustomer: $('#HasPitcherFilterBeenProvidedToCustomer'),
                pitcherFilterCustomerDeliveryMethod: $('#PitcherFilterCustomerDeliveryMethod'),
                pitcherFilterCustomerDeliveryOtherMethod: $('#PitcherFilterCustomerDeliveryOtherMethod'),
                multiTenantFacility: $('#IsThisAMultiTenantFacility')
            },
            m.initWorkOrderDescription();
            m.initAppendNotes();
            // Compliance Data Validations
            m.initInitialServiceLineFlushTime();
            m.initHasPitcherFilterBeenProvidedToCustomer();
            m.initPitcherFilterCustomerDeliveryMethod();
            m.initIsThisAMultiTenantFacility();
        },
        initWorkOrderDescription: function () {
            ELEMENTS.finalWorkDescription.on('change', m.onWorkDescriptionChanged);
            m.onWorkDescriptionChanged();
        },
        onWorkDescriptionChanged: function () {
            ELEMENTS.initialServiceLineFlushTime.prop('required', false);
            ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.prop('required', false);

            const selectedWorkDescriptionId = parseInt(ELEMENTS.finalWorkDescription.val(), 10);
            if (SERVICE_LINE_RENEWAL.includes(selectedWorkDescriptionId)) {
                ELEMENTS.initialServiceLineFlushTime.prop('required', true);
                ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.prop('required', true);
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
                $('#multiTenantFacility').show();
            } else {
                $('#deliveryDetails').hide();
                $('#multiTenantFacility').hide();
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
        },

        initAppendNotes: function () {
            ELEMENTS.appendNotes.on('change', m.onAppendNotesChanged);
            m.onAppendNotesChanged();
        },
        onAppendNotesChanged: function () {
            if (ELEMENTS.appendNotes.val().length > 0) {
                ELEMENTS.appendNotesHiddenField.val(ELEMENTS.appendNotes.val());
            }
        },

        initIsThisAMultiTenantFacility: function () {
            ELEMENTS.multiTenantFacility.on('change', m.onIsThisAMultiTenantFacilityChanged);
            m.onIsThisAMultiTenantFacilityChanged();
        },

        onIsThisAMultiTenantFacilityChanged: function () {
            if (ELEMENTS.multiTenantFacility.val() === 'True') {
                $('#multiTenantFacilityRequiredFields').show();
            } else {
                $('#multiTenantFacilityRequiredFields').hide();
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);