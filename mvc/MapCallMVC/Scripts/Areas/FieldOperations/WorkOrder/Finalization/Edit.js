var WorkOrderFinalization = (function ($) {
    const MAIN_BREAK = [74, 80];
    const OTHER_PITCHER_FILTER_DELIVERY_METHOD = 3;
    const meterLocationUnknownId = 3;
    var m = {
        initialize: function () {
            ELEMENTS = {
                btnFinalize: $('#btnFinalize'),
                finalWorkDescription: $('#FinalWorkDescription'),
                workDescriptionInit: $('#WorkDescriptionInit'),
                lostWater: $('#LostWater'),
                lostWaterInit: $('#LostWaterInit'),
                distanceFromCrossStreet: $('#DistanceFromCrossStreet'),
                distanceFromCrossStreetInit: $('#DistanceFromCrossStreetInit'),
                
                estimatedCustomerImpact: $('#CustomerImpact'),
                estimatedCustomerImpactInit: $('#EstimatedCustomerImpactInit'),
                anticipatedRepairTime: $('#RepairTime'),
                anticipatedRepairTimeInit: $('#AnticipatedRepairTimeInit'),
                alertIssued: $('#AlertIssued'),
                alertIssuedInit: $('#AlertIssuedInit'),
                significantTrafficImpact: $('#TrafficImpact'),
                significantTrafficImpactInit: $('#SignificantTrafficImpactInit'),
                appendNotes: $('#AppendNotes'),
                appendNotesInit: $('#AppendNotesInit'),
                // Compliance Data Fields
                initialServiceLineFlushTime: $('#InitialServiceLineFlushTime'),
                hasPitcherFilterBeenProvidedToCustomer: $('#HasPitcherFilterBeenProvidedToCustomer'),
                pitcherFilterCustomerDeliveryMethod: $('#PitcherFilterCustomerDeliveryMethod'),
                pitcherFilterCustomerDeliveryOtherMethod: $('#PitcherFilterCustomerDeliveryOtherMethod'),
                meterLocation: $('#MeterLocation'),
                valveFrame: $('#valveFrame'),
                hydrantFrame: $('#hydrantFrame'),
                sewerOpeningFrame: $('#sewerOpeningFrame'),
                serviceFrame: $('#serviceFrame')
            },
            m.initBtnFinalize();

            m.initWorkOrderDescription();
            m.initLostWater();
            m.initDistanceFromCrossStreet();
            m.initAppendNotes();
            // Main Break Info
            m.initEstimatedCustomerImpact();
            m.initAnticipatedRepairTime();
            m.initAlertIssued();
            m.initSignificantTrafficImpact();
            // Validation Messages
            m.initValidationMessages();
            // Compliance Data Validations
            m.initInitialServiceLineFlushTime();
            m.initHasPitcherFilterBeenProvidedToCustomer();
            m.initPitcherFilterCustomerDeliveryMethod();
        },
        initValidationMessages: function () {
            const selectedWorkDescriptionId = parseInt(ELEMENTS.workDescriptionInit.val(), 10);
            if (MAIN_BREAK.includes(selectedWorkDescriptionId) &&
                $('#mainBreaksTable tr').length < 2) {
                $('#main-break-error-message').show();
                ELEMENTS.btnFinalize.prop('disabled', true);
            }
            else {
                $('#main-break-error-message').hide();
                ELEMENTS.btnFinalize.prop('disabled', false);
            }
        },

        initBtnFinalize: function () {
            ELEMENTS.btnFinalize.on('click', m.onBtnFinalizeClick);
        },
        onBtnFinalizeClick: function () {
            var additionalForm = $('#AdditionalForm');
            if (!additionalForm.valid()) {
                $('a[href="#AdditionalTab"]').click();
                $('#btnUpdate').click();
            }
            // jQuery will typically return some object when you do $("element"). So checking that its not null via if($("something") != null), 
            // will not work to verify that we found an element! In order to check this we need to use if($("something").length).   (remember that 0 = false in js)
            else if (ELEMENTS.valveFrame.length && ELEMENTS.valveFrame.get(0).contentWindow.Application.routeData.action === 'Edit') {
                alert("Please save the valve first!");
                $('a[href="#ValveTab"]').click();
                return false;
            }
            else if (ELEMENTS.hydrantFrame.length && ELEMENTS.hydrantFrame.get(0).contentWindow.Application.routeData.action === 'Edit') {
                alert("Please save the hydrant first!");
                $('a[href="#HydrantTab"]').click();
                return false;
            } 
            else if (ELEMENTS.sewerOpeningFrame.length && ELEMENTS.sewerOpeningFrame.get(0).contentWindow.Application.routeData.action === 'Edit') {
                alert("Please save the sewer opening first!");
                $('a[href="#SewerOpeningTab"]').click();
                return false;
            }
            else if (ELEMENTS.serviceFrame.length && ELEMENTS.serviceFrame.get(0).contentWindow.Application.routeData.action === 'Edit') {
                alert("Please save the service first!");
                $('a[href="#ServiceTab"]').click();
                return false;
            } 
            else {
                Application.clearNotification();
            }
        },
        initWorkOrderDescription: function () {
            ELEMENTS.finalWorkDescription.on('change', m.onWorkDescriptionChanged);
            m.onWorkDescriptionChanged();
        },
        onWorkDescriptionChanged: function () {
            ELEMENTS.workDescriptionInit.val(ELEMENTS.finalWorkDescription.val());
            m.initValidationMessages();
        },

        initLostWater: function () {
            ELEMENTS.lostWater.on('change', m.onLostWaterChanged);
            m.onLostWaterChanged();
        },
        onLostWaterChanged: function () {
            ELEMENTS.lostWaterInit.val(ELEMENTS.lostWater.val());
        },

        initDistanceFromCrossStreet: function () {
            ELEMENTS.distanceFromCrossStreet.on('change', m.onDistanceFromCrossStreetChanged);
            m.onDistanceFromCrossStreetChanged();
        },
        onDistanceFromCrossStreetChanged: function () {
            ELEMENTS.distanceFromCrossStreetInit.val(ELEMENTS.distanceFromCrossStreet.val());
        },

        initEstimatedCustomerImpact: function () {
            ELEMENTS.estimatedCustomerImpact.on('change', m.onEstimatedCustomerImpactChanged);
            m.onEstimatedCustomerImpactChanged();
        },
        onEstimatedCustomerImpactChanged: function () {
            ELEMENTS.estimatedCustomerImpactInit.val(ELEMENTS.estimatedCustomerImpact.val());
        },
        initAnticipatedRepairTime: function () {
            ELEMENTS.anticipatedRepairTime.on('change', m.onAnticipatedRepairTimeChanged);
            m.onAnticipatedRepairTimeChanged();
        },
        onAnticipatedRepairTimeChanged: function () {
            ELEMENTS.anticipatedRepairTimeInit.val(ELEMENTS.anticipatedRepairTime.val());
        },
        initAlertIssued: function () {
            ELEMENTS.alertIssued.on('change', m.onAlertIssuedChanged);
            m.onAlertIssuedChanged();
        },
        onAlertIssuedChanged: function () {
            ELEMENTS.alertIssuedInit.val(ELEMENTS.alertIssued.val());
        },
        initSignificantTrafficImpact: function () {
            ELEMENTS.significantTrafficImpact.on('change', m.onSignificantTrafficImpactChanged);
            m.onSignificantTrafficImpactChanged();
        },
        onSignificantTrafficImpactChanged: function () {
            ELEMENTS.significantTrafficImpactInit.val(ELEMENTS.significantTrafficImpact.val());
        },

        initAppendNotes: function () {
            ELEMENTS.appendNotes.on('change', m.onAppendNotesChanged);
            m.onAppendNotesChanged();
        },
        onAppendNotesChanged: function () {
            ELEMENTS.appendNotesInit.val(ELEMENTS.appendNotes.val());
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
        },

        validateMeterLocation: function (value, element) {
            const selectedMeterLocationId = parseInt(ELEMENTS.meterLocation.val(), 10);
            const showMeterLocationError = meterLocationUnknownId === selectedMeterLocationId;
            return !showMeterLocationError;
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);