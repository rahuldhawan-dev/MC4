var WorkOrderFinalization = (function ($) {
    const MAIN_BREAK = [74, 80];    
    const meterLocationUnknownId = 3;
    var m = {
        initialize: function () {
            ELEMENTS = {
                btnFinalize: $('#btnFinalize'),

                /* Additional Details Fields */
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

                /* Service Line Info Fields */
                previousServiceLineMaterial: $('#PreviousServiceLineMaterial'),
                previousServiceLineSize: $('#PreviousServiceLineSize'),
                companyServiceLineMaterial: $('#CompanyServiceLineMaterial'),
                companyServiceLineSize: $('#CompanyServiceLineSize'),
                customerServiceLineMaterial: $('#CustomerServiceLineMaterial'),
                customerServiceLineSize: $('#CustomerServiceLineSize'),
                doorNoticeLeftDate: $('#DoorNoticeLeftDate'),

                previousServiceLineMaterialInit: $('#PreviousServiceLineMaterialInit'),
                previousServiceLineSizeInit: $('#PreviousServiceLineSizeInit'),
                companyServiceLineMaterialInit: $('#CompanyServiceLineMaterialInit'),
                companyServiceLineSizeInit: $('#CompanyServiceLineSizeInit'),
                customerServiceLineMaterialInit: $('#CustomerServiceLineMaterialInit'),
                customerServiceLineSizeInit: $('#CustomerServiceLineSizeInit'),
                doorNoticeLeftDateInit: $('#DoorNoticeLeftDateInit'),

                /* Compliance Data Fields */
                initialServiceLineFlushTime: $('#InitialServiceLineFlushTime'),
                hasPitcherFilterBeenProvidedToCustomer: $('#HasPitcherFilterBeenProvidedToCustomer'),
                datePitcherFilterDeliveredToCustomer: $('#DatePitcherFilterDeliveredToCustomer'),
                pitcherFilterCustomerDeliveryMethod: $('#PitcherFilterCustomerDeliveryMethod'),
                pitcherFilterCustomerDeliveryOtherMethod: $('#PitcherFilterCustomerDeliveryOtherMethod'),
                dateCustomerProvidedAWStateLeadInformation: $('#DateCustomerProvidedAWStateLeadInformation'),                
                initialServiceLineFlushTimeInit: $('#InitialServiceLineFlushTimeInit'),
                hasPitcherFilterBeenProvidedToCustomerInit: $('#HasPitcherFilterBeenProvidedToCustomerInit'),
                datePitcherFilterDeliveredToCustomerInit: $('#DatePitcherFilterDeliveredToCustomerInit'),
                pitcherFilterCustomerDeliveryMethodInit: $('#PitcherFilterCustomerDeliveryMethodInit'),
                pitcherFilterCustomerDeliveryOtherMethodInit: $('#PitcherFilterCustomerDeliveryOtherMethodInit'),
                dateCustomerProvidedAWStateLeadInformationInit: $('#DateCustomerProvidedAWStateLeadInformationInit'),
                isThisAMultiTenantFacility: $('#IsThisAMultiTenantFacility'),
                isThisAMultiTenantFacilityInit: $('#IsThisAMultiTenantFacilityInit'),
                numberOfPitcherFiltersDelivered: $('#NumberOfPitcherFiltersDelivered'),
                numberOfPitcherFiltersDeliveredInit: $('#NumberOfPitcherFiltersDeliveredInit'),
                describeWhichUnits: $('#DescribeWhichUnits'),
                describeWhichUnitsInit: $('#DescribeWhichUnitsInit'),

                meterLocation: $('#MeterLocation'),
                valveFrame: $('#valveFrame'),
                hydrantFrame: $('#hydrantFrame'),
                sewerOpeningFrame: $('#sewerOpeningFrame'),
                serviceFrame: $('#serviceFrame'),
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
            // Complaince Data
            m.initInitialServiceLineFlushTime();
            m.initHasPitcherFilterBeenProvidedToCustomer();
            m.initDatePitcherFilterDeliveredToCustomer();
            m.initPitcherFilterCustomerDeliveryMethod();
            m.initPitcherFilterCustomerDeliveryOtherMethod();
            m.initDateCustomerProvidedAWStateLeadInformation();
            m.initIsThisAMultiTenantFacility();
            m.initNumberOfPitcherFiltersDelivered();
            m.initDescribeWhichUnits();
            // Service Line Info
            m.initPreviousServiceLineMaterial();
            m.initPreviousServiceLineSize();
            m.initCustomerServiceLineMaterial();
            m.initCustomerServiceLineSize();
            m.initCompanyServiceLineMaterial();
            m.initCompanyServiceLineSize();
            m.initDoorNoticeLeftDate();
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
            var additionalDetailsForm = $('#AdditionalDetailsForm');
            if (!additionalDetailsForm.valid()) {
                $('a[href="#AdditionalTab"]').click();
                $('#btnUpdateDetails').click();
            }
            var complianceDataForm = $('#ComplianceDataForm');
            if (!complianceDataForm.valid()) {
                $('a[href="#AdditionalTab"]').click();
                $('#btnUpdateCompliance').click();
            }
            var serviceLineInfoForm = $('#ServiceLineInfoForm');
            if (!serviceLineInfoForm.valid()) {
                $('a[href="#ServiceTab"]').click();
                $('#btnUpdateServiceLineInfo').click();
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

        validateMeterLocation: function (value, element) {
            const selectedMeterLocationId = parseInt(ELEMENTS.meterLocation.val(), 10);
            const showMeterLocationError = meterLocationUnknownId === selectedMeterLocationId;
            return !showMeterLocationError;
        },

        // Complaince Data fields
        initInitialServiceLineFlushTime: function () {
            ELEMENTS.initialServiceLineFlushTime.on('change', m.onInitialServiceLineFlushTimeChanged);
            m.onInitialServiceLineFlushTimeChanged();
        },
        onInitialServiceLineFlushTimeChanged: function () {
            ELEMENTS.initialServiceLineFlushTimeInit.val(ELEMENTS.initialServiceLineFlushTime.val());
        },

        initHasPitcherFilterBeenProvidedToCustomer: function () {
            ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.on('change', m.onHasPitcherFilterBeenProvidedToCustomerChanged);
            m.onHasPitcherFilterBeenProvidedToCustomerChanged();
        },
        onHasPitcherFilterBeenProvidedToCustomerChanged: function () {
            ELEMENTS.hasPitcherFilterBeenProvidedToCustomerInit.val(ELEMENTS.hasPitcherFilterBeenProvidedToCustomer.val());
        },

        initDateCustomerProvidedAWStateLeadInformation: function () {
            ELEMENTS.dateCustomerProvidedAWStateLeadInformation.on('change', m.onDateCustomerProvidedAWStateLeadInformationChanged);
            m.onDateCustomerProvidedAWStateLeadInformationChanged();
        },
        onDateCustomerProvidedAWStateLeadInformationChanged: function () {
            ELEMENTS.dateCustomerProvidedAWStateLeadInformationInit.val(ELEMENTS.dateCustomerProvidedAWStateLeadInformation.val());
        },

        initPitcherFilterCustomerDeliveryMethod: function () {
            ELEMENTS.pitcherFilterCustomerDeliveryMethod.on('change', m.onPitcherFilterCustomerDeliveryMethodChanged);
            m.onPitcherFilterCustomerDeliveryMethodChanged();
        },
        onPitcherFilterCustomerDeliveryMethodChanged: function () {
            ELEMENTS.pitcherFilterCustomerDeliveryMethodInit.val(ELEMENTS.pitcherFilterCustomerDeliveryMethod.val());
        },

        initPitcherFilterCustomerDeliveryOtherMethod: function () {
            ELEMENTS.pitcherFilterCustomerDeliveryOtherMethod.on('change', m.onPitcherFilterCustomerDeliveryOtherMethodChanged);
            m.onPitcherFilterCustomerDeliveryOtherMethodChanged();
        },
        onPitcherFilterCustomerDeliveryOtherMethodChanged: function () {
            ELEMENTS.pitcherFilterCustomerDeliveryOtherMethodInit.val(ELEMENTS.pitcherFilterCustomerDeliveryOtherMethod.val());
        },

        initDatePitcherFilterDeliveredToCustomer: function () {
            ELEMENTS.datePitcherFilterDeliveredToCustomer.on('change', m.onDatePitcherFilterDeliveredToCustomerChanged);
            m.onDatePitcherFilterDeliveredToCustomerChanged();
        },
        onDatePitcherFilterDeliveredToCustomerChanged: function () {
            ELEMENTS.datePitcherFilterDeliveredToCustomerInit.val(ELEMENTS.datePitcherFilterDeliveredToCustomer.val());
        },

        initIsThisAMultiTenantFacility: function () {
            ELEMENTS.isThisAMultiTenantFacility.on('change', m.onIsThisAMultiTenantFacilityChanged);
            m.onIsThisAMultiTenantFacilityChanged();
        },
        onIsThisAMultiTenantFacilityChanged: function () {
            ELEMENTS.isThisAMultiTenantFacilityInit.val(ELEMENTS.isThisAMultiTenantFacility.val());
        },

        initNumberOfPitcherFiltersDelivered: function () {
            ELEMENTS.numberOfPitcherFiltersDelivered.on('change', m.onNumberOfPitcherFiltersDeliveredChanged);
            m.onNumberOfPitcherFiltersDeliveredChanged();
        },
        onNumberOfPitcherFiltersDeliveredChanged: function () {
            ELEMENTS.numberOfPitcherFiltersDeliveredInit.val(ELEMENTS.numberOfPitcherFiltersDelivered.val());
        },

        initDescribeWhichUnits: function () {
            ELEMENTS.describeWhichUnits.on('change', m.onDescribeWhichUnitsChanged);
            m.onDescribeWhichUnitsChanged();
        },
        onDescribeWhichUnitsChanged: function () {
            ELEMENTS.describeWhichUnitsInit.val(ELEMENTS.describeWhichUnits.val());
        },

        /* Service Line Info */

        initPreviousServiceLineMaterial: function () {
            ELEMENTS.previousServiceLineMaterial.on('change', m.onPreviousServiceLineMaterialChanged);
            m.onPreviousServiceLineMaterialChanged();
        },
        onPreviousServiceLineMaterialChanged: function () {
            ELEMENTS.previousServiceLineMaterialInit.val(ELEMENTS.previousServiceLineMaterial.val());
        },

        initPreviousServiceLineSize: function () {
            ELEMENTS.previousServiceLineSize.on('change', m.onPreviousServiceLineSizeChanged);
            m.onPreviousServiceLineSizeChanged();
        },
        onPreviousServiceLineSizeChanged: function () {
            ELEMENTS.previousServiceLineSizeInit.val(ELEMENTS.previousServiceLineSize.val());
        },

        initCompanyServiceLineMaterial: function () {
            ELEMENTS.companyServiceLineMaterial.on('change', m.onCompanyServiceLineMaterialChanged);
            m.onCompanyServiceLineMaterialChanged();
        },
        onCompanyServiceLineMaterialChanged: function () {
            ELEMENTS.companyServiceLineMaterialInit.val(ELEMENTS.companyServiceLineMaterial.val());
        },

        initCompanyServiceLineSize: function () {
            ELEMENTS.companyServiceLineSize.on('change', m.onCompanyServiceLineSizeChanged);
            m.onCompanyServiceLineSizeChanged();
        },
        onCompanyServiceLineSizeChanged: function () {
            ELEMENTS.companyServiceLineSizeInit.val(ELEMENTS.companyServiceLineSize.val());
        },

        initCustomerServiceLineMaterial: function () {
            ELEMENTS.customerServiceLineMaterial.on('change', m.onCustomerServiceLineMaterialChanged);
            m.onCustomerServiceLineMaterialChanged();
        },
        onCustomerServiceLineMaterialChanged: function () {
            ELEMENTS.customerServiceLineMaterialInit.val(ELEMENTS.customerServiceLineMaterial.val());
        },

        initCustomerServiceLineSize: function () {
            ELEMENTS.customerServiceLineSize.on('change', m.onCustomerServiceLineSizeChanged);
            m.onCustomerServiceLineSizeChanged();
        },
        onCustomerServiceLineSizeChanged: function () {
            ELEMENTS.customerServiceLineSizeInit.val(ELEMENTS.customerServiceLineSize.val());
        },

        initDoorNoticeLeftDate: function () {
            ELEMENTS.doorNoticeLeftDate.on('change', m.onDoorNoticeLeftDateChanged);
            m.onDoorNoticeLeftDateChanged();
        },
        onDoorNoticeLeftDateChanged: function () {
            ELEMENTS.doorNoticeLeftDateInit.val(ELEMENTS.doorNoticeLeftDate.val());
        },
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);