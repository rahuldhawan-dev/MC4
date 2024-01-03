var Hydrants = (function () {
    var ELEMENTS = {};
    var hydrantPrefixServiceUrl;
    var townStateServiceUrl;
    var currentHydrantPrefixAjaxCall = null;

    /*
     * MC-2607, we want to show "Date Retired" label/input for an asset when it's status
     * is 'REMOVED', 'RETIRED' or 'REQUEST RETIREMENT' - so let's close over these and
     * reference them later during the asset status change event handler
     */
    const showDateRetiredForStatuses = [
        { id: 5, description: 'REMOVED' },
        { id: 6, description: 'RETIRED' },
        { id: 14, description: 'REQUEST RETIREMENT' }
    ];

    const workOrderStatuses = [
        { id: 5, description: 'REMOVED' },
        { id: 6, description: 'RETIRED' },
        { id: 1, description: 'ACTIVE' }
    ];

    const showWarningForRetiredRemovedStatuses = [
        { id: 5, description: 'REMOVED' },
        { id: 6, description: 'RETIRED' }
    ];

    let hasStatusChanged = false; 

    var h = {
        init: function () {
            ELEMENTS = {
                coordinate: $('#Coordinate'),
                criticalNotes: $('#CriticalNotes'),
                dateRetired: $('#DateRetired'),
                fireDistrict: $('#FireDistrict'),
                form: $('#hydrant-form'),
                isCritical: $('#Critical'),
                isFoundHydrant: $('#IsFoundHydrant'),
                hydrantBilling: $('#HydrantBilling'),
                hydrantPrefix: $('#HydrantPrefix'),
                hydrantSuffix: $('#HydrantSuffix'),
                operatingCenter: $('#OperatingCenter'),
                sapEquipmentId: $('#SAPEquipmentId'),
                sendNotification: $('#SendNotificationsOnSave'),
                state: $('#State'),
                status: $('#Status'),
                street: $('#Street'),
                streetNumber: $('#StreetNumber'),
                town: $('#Town'),
                townName: $('#TownName'),
                townSection: $('#TownSection'),
                workOrderNumber: $('#WorkOrderNumber'),
                hydrantStatusAlert: $('#HydrantStatusAlert')
            };
            hydrantPrefixServiceUrl = $('#HydrantPrefixServiceUrl').val();
            townStateServiceUrl = $('#TownStateServiceUrl').val();
            h.initCriticalNotesHiding();
            h.initHydrantPrefixCascade();
            h.initFoundHydrantFunctionality();
            h.initHydrantBilling();
            h.initHydrantStatus();
        },

        getAddress: function () {
            // Okay so if there's a coordinate already selected, auto-populating the address
            // and searching will cause the existing coordinate to not be displayed. 
            if (ELEMENTS.coordinate.val()) {
                return null;
            }
            // townName element only exists on edit view.
            var selectedTown = ELEMENTS.town.find('option:selected');
            var town = ELEMENTS.townName.val() || selectedTown.text();
            var streetNumber = ELEMENTS.streetNumber.val();
            var street = ELEMENTS.street.find('option:selected').text();

            // We need state from somewhere or else this doesn't work out very well.
            // The State element only exists on the edit view and is pre-populated.
            var state = ELEMENTS.state.val();
            if (/^\d+$/.test(state)) {
                // if ELEMENTS.state is a dropdown we have a number, which is no good.
                // get the option text instead.
                state = $('option:selected', ELEMENTS.state).text();
            }

            if (!state && selectedTown.val()) {
                $.ajax({
                    url: townStateServiceUrl,
                    data: {
                        id: ELEMENTS.town.val()
                    },
                    async: false, // So this just goes.
                    type: 'GET',
                    success: function (result) {
                        state = result.state;
                    },
                    error: function () {
                        alert("Something went wrong finding the state for the selected town.");
                    }
                });
            }

            return streetNumber + ' ' + street + ', ' + town + ' ' + state;
        },

        initCriticalNotesHiding: function () {
            ELEMENTS.isCritical.on('change', h.onIsCriticalChecked);
            h.onIsCriticalChecked();
        },

        onIsCriticalChecked: function () {
            // NOTE: The criticalNotes textbox is disabled so its value doesn't get posted
            //       back to the server, but allows for the value to stay there on the client.
            var parent = ELEMENTS.criticalNotes.closest('.field-pair');
            if (ELEMENTS.isCritical.is(':checked')) {
                parent.show();
                ELEMENTS.criticalNotes.prop('disabled', false);
            } else {
                parent.hide();
                ELEMENTS.criticalNotes.prop('disabled', true);
            }
        },

        initHydrantBilling: function () {
            ELEMENTS.hydrantBilling.on('change', h.onHydrantBillingChanged);
            h.onHydrantBillingChanged();
        },

        initHydrantStatus: function () {
            ELEMENTS.status.on('change', h.onStatusChange);
            ELEMENTS.status.on('change', function () {
                hasStatusChanged = true;  // storing the state only if the status is changed, cannot add this to existing status onStatusChange function as it is initiated when the page loads
            });
            h.onStatusChange();
        },

        onHydrantBillingChanged: function () {
            if (ELEMENTS.hydrantBilling.val() == "2") {
                ELEMENTS.fireDistrict.prop('disabled', false);
            } else {
                ELEMENTS.fireDistrict.prop('disabled', true);
                ELEMENTS.fireDistrict.val('');
            }
        },

        initHydrantPrefixCascade: function () {
            // This isn't available during edits.
            if (hydrantPrefixServiceUrl) {
                ELEMENTS.town.on('change', h.onHydrantPrefixChangeTriggered);
                ELEMENTS.townSection.on('change', h.onHydrantPrefixChangeTriggered);
            }
        },

        onHydrantPrefixChangeTriggered: function () {
            // This fires frequently due to all the cascading dropdowns changing values.
            if (currentHydrantPrefixAjaxCall) {
                currentHydrantPrefixAjaxCall.abort();
            }

            currentHydrantPrefixAjaxCall = $.ajax({
                url: hydrantPrefixServiceUrl,
                data: {
                    operatingCenterId: ELEMENTS.operatingCenter.val(),
                    townId: ELEMENTS.town.val(),
                    townSectionId: ELEMENTS.townSection.val()
                },
                async: false,
                type: 'GET',
                success: function (result) {
                    ELEMENTS.hydrantPrefix.text(result.prefix);
                },
                error: function () {
                    ELEMENTS.hydrantPrefix.text('');
                }
            });
        },

        initFoundHydrantFunctionality: function () {
            // This script is used for both new and edit but this function is only usable for new hydrants.
            if (ELEMENTS.isFoundHydrant.length === 0) {
                return;
            }
            ELEMENTS.isFoundHydrant.on('change', h.onIsFoundHydrantChanged);
            h.onIsFoundHydrantChanged();
        },

        onIsFoundHydrantChanged: function () {
            var suffixParent = ELEMENTS.hydrantSuffix.closest('.field-pair');
            if (ELEMENTS.isFoundHydrant.is(':checked')) {
                ELEMENTS.hydrantSuffix.prop('disable', false);
                suffixParent.show();
            } else {
                ELEMENTS.hydrantSuffix.prop('disable', true);
                suffixParent.hide();
            }
        },

        onStatusChange: function () {
            const selectedStatusId = parseInt(ELEMENTS.status.val(), 10);
            const shouldShowDateRetiredField = showDateRetiredForStatuses.some(status => status.id === selectedStatusId);
            const shouldWarnUsersforRetiredRemoved =
                showWarningForRetiredRemovedStatuses.some(status => status.id === selectedStatusId);
            Application.toggleField(ELEMENTS.dateRetired, shouldShowDateRetiredField);
            h.toggleNotification(ELEMENTS.hydrantStatusAlert, shouldWarnUsersforRetiredRemoved);
        },

        validateWorkOrderNumber: function (value, element) {
            if (!hasStatusChanged) return true;
            const selectedStatusId = parseInt(ELEMENTS.status.val(), 10);
            const showWorkOrderError = workOrderStatuses.some(status => status.id === selectedStatusId);
            if (!showWorkOrderError) return true;
            if (!value) return false;
            return true;
        },

        validateCriticalNotes: function (criticalNotesValue, element) {
            // NOTE: This validation will not run when the CriticalNotes field
            // is disabled.
            if (criticalNotesValue && !ELEMENTS.isCritical.is(':checked')) {
                return false;
            }
            return true;
        },

        validateYearManufactured: function (val, el) {
            if (val && (1850 > val || val > new Date().getFullYear())) {
                return false;
            }
            return true;
        },

        validateFunctionalLocation: function (sapFunctionalLocationIdValue, element) {
            // validateFunctionalLocationId is required when OperatingCenter.IsContractedOperations == FALSE AND OperatingCenteRSAPEnabled == TRUE

            // Cut out early beacuse OperatingCenter is a required field and we don't wanna throw errors
            // when they haven't selected an operating center yet.
            var opc = ELEMENTS.operatingCenter.val();
            if (!opc) {
                return true;
            }

            // Always valid if it has a value.
            if (sapFunctionalLocationIdValue) {
                return true;
            }

            var isSAPEnabledServiceUrl = $('#IsSAPEnabledServiceUrl').val();
            var isValid;

            $.ajax({
                url: isSAPEnabledServiceUrl,
                data: {
                    id: opc
                },
                async: false, // So this just goes.
                type: 'GET',
                success: function (result) {
                    isValid = !result.IsSAPEnabled;
                },
                error: function () {
                    alert("Something went wrong when validating the Functional Location.");
                }
            });
            return isValid;
        },

        toggleNotification: (sel, show = true) => $(sel).toggle(show)
    }

    $(document).ready(h.init);

    return h;
})();

