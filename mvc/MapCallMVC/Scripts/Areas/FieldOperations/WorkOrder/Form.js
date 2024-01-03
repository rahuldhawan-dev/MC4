const WorkOrders = (($, workDescriptions) => {
    const ELEMENTS = {
        accountCharged: '#AccountCharged',
        acousticMonitoringType: '#AcousticMonitoringType',
        alertIssued: '#AlertIssued',
        anticipatedRepairTime: '#AnticipatedRepairTime',
        assetIdLabel: '.asset-id > .label',
        assetRow: '.asset-id',
        assetType: '#AssetType',
        coordinateId: '#CoordinateId',
        coordinateLabel: '.cp-coordinate-values',
        coordinateIcon: 'img.coordinate-picker-icon',
        customer: '#CustomerName,#PhoneNumber,#SecondaryPhoneNumber',
        dummyAsset: '#DummyAsset',
        digitalAsBuiltRequired: '#DigitalAsBuiltRequired',
        equipment: '#Equipment',
        estimatedCustomerImpact: '#EstimatedCustomerImpact',
        getAssetHistoryUrl: '#GetAssetHistoryUrl',
        getCoordinateIdForAssetUrl: '#GetCoordinateIdForAssetUrl',
        getHasSampleSite: '#HasSampleSite',
        getOperatingCenterIsSAPWorkOrdersEnabled: '#GetOperatingCenterIsSAPWorkOrdersEnabled',
        getSampleSitesByPremiseNumberUrl: '#GetSampleSitesByPremiseNumberUrl',
        getStateUrl: '#GetStateUrl',
        getTownCriticalMainBreakNotesUrl: '#GetTownCriticalMainBreakNotesUrl',
        history: '#History',
        hydrant: '#Hydrant',
        isRevisit: '#IsRevisit',
        latitude: '#Latitude',
        longitude: '#Longitude',
        mainBreak: '#EstimatedCustomerImpact,#AnticipatedRepairTime,#AlertIssued,#SignificantTrafficImpact',
        meterSerialNumber: '#MeterSerialNumber',
        mainCrossing: '#MainCrossing',
        nearestCrossStreet: '#NearestCrossStreet',
        notes: '#Notes',
        operatingCenter: '#OperatingCenter',
        originalOrderNumber: '#OriginalOrderNumber',
        phoneNumber: '#PhoneNumber',
        pmatOverride: '#PlantMaintenanceActivityTypeOverride',
        premiseNumber: '#PremiseNumber',
        priority: '#Priority',
        requestedBy: '#RequestedBy',
        requestingEmployee: '#RequestingEmployee',
        sapWorkOrderNumber: '#SAPWorkOrderNumber',
        saveButton: 'button[type="submit"]',
        service: '#PremiseNumber,#ServiceNumber,#Device,#DeviceLocation,#SAPEquipmentNumber,#PremiseAddress,#premiseLookupLink,#Installation,#installationLookupLink,#premiseLookup', // actually service number and premise number
        premiseLookupLink: '#premiseLookupLink,#installationLookupLink',
        sendToSAP: '#SendToSAP',
        serviceNumber: '#ServiceNumber',
        sewerOpening: '#SewerOpening',
        significantTrafficImpact: '#SignificantTrafficImpact',
        state: '#State',
        street: '#Street',
        streetAutocomplete: '#Street_AutoComplete',
        streetNumber: '#StreetNumber',
        town: '#Town',
        townSection: '#TownSection',
        valve: '#Valve',
        plannedCompletionDate: '#PlannedCompletionDate',
        workDescription: '#WorkDescription',
        zipCode: '#ZipCode'
    };

    const ASSET_IDS = {
        'Valve': {
            element: ELEMENTS.valve
        },
        'Hydrant': {
            element: ELEMENTS.hydrant
        },
        'Main': {
            element: null
        },
        'Main Crossing': {
            element: ELEMENTS.mainCrossing
        },
        'Service': {
            element: ELEMENTS.service,
            label: 'Service Information'
        },
        'Sewer Opening': {
            element: ELEMENTS.sewerOpening
        },
        'Sewer Lateral': {
            element: ELEMENTS.service,
            label: 'Service Information'
        },
        'Sewer Main': {
            element: null
        },
        'Storm/Catch': {
            element: ELEMENTS.stormCatch
        },
        'Equipment': {
            element: ELEMENTS.equipment
        }
    };

    // asset types for which the loading of coordinates is no
    const NO_COORDINATE_ASSETS = [
        'MAIN',
        'SERVICE',
        'SEWER LATERAL',
        'SEWER MAIN'
    ];

    // BRB  Work Descriptions
    const BRB_PMAT_DESCRIPTIONS = [
        '54', '62', '75', '76', '85', '124', '225', '226', '327', '328'
    ];

    // helpers

    const getSelectedText = function (selector) {
        return $(':selected', selector).text();
    };

    const setField = function (elem, val) {
        return $(elem).val(val).change();
    };

    const setAndDisableField = function (elem, val) {
        setField(elem, val).attr('disabled', 'disabled');
    };

    const waitThenSetField = function (elem, val) {
        $(elem).ready(function () {
            setField(elem, val);
        });
    };

    const waitThenSetAndDisableField = function (elem, val) {
        $(elem).ready(function () {
            setAndDisableField(elem, val);
        });
    };

    const toggleRow = function ($elem, show) {
        $elem.each(function () {
            $(this).parent().parent().parent()[show ? 'show' : 'hide']();
        });
    };

    const showRow = function ($elem) {
        toggleRow($elem, true);
    };

    const hideRow = function ($elem) {
        toggleRow($elem, false);
    };

    const clearDropDown = function ($dropDown) {
        $dropDown.each(function () {
            const $this = $(this);
            $this.val($('option:first', $this).val());
        });
    };

    const hideAssetInputFields = function () {
        const dropDowns = $(ELEMENTS.valve)
            .add(ELEMENTS.hydrant)
            .add(ELEMENTS.sewerOpening)
            .add(ELEMENTS.stormCatch)
            .add(ELEMENTS.equipment)
            .add(ELEMENTS.mainCrossing);

        clearDropDown(dropDowns);

        dropDowns.add(ELEMENTS.dummyAsset).hide();

        $(ELEMENTS.service).val('').hide();
        $(ELEMENTS.premiseLookupLink).hide();
    };

    const isMainBreak =
        () => /^WATER MAIN BREAK REP(AIR|LACE)$/.test(getSelectedText(ELEMENTS.workDescription));

    const isNoPremiseDescription =
        () => /^SERVICE LINE RETIRE(?:-LEAD)? NO PREMISE$/.test(getSelectedText(ELEMENTS.workDescription));

    const determineIfDescriptionRequiresDigitalAsBuilt = () => {
        const workDescriptionId = $(ELEMENTS.workDescription).val();
        
        if (!workDescriptionId) {
            return;
        }
        
        const workDescription = workDescriptions.find(x => x.Id.toString() === workDescriptionId);
        
        if (workDescription.DigitalAsBuiltRequired) {
            $(ELEMENTS.digitalAsBuiltRequired)
                .prop('checked', true)
                .prop('disabled', true);
        } else {
            $(ELEMENTS.digitalAsBuiltRequired).removeProp('disabled');
        }
    };

    const trySetState = function (opCntr) {
        $.ajax({
            type: 'GET',
            url: $(ELEMENTS.getStateUrl).val() + '/' + opCntr,
            success: function (msg) {
                $(ELEMENTS.state).val(msg);
            }
        });
    };

    const trySetTownCriticalMainBreakNotes = function () {
        const town = $(ELEMENTS.town).val();
        const url = $(ELEMENTS.getTownCriticalMainBreakNotesUrl).val();
        if (town && isMainBreak()) {
            $.ajax({
                type: 'GET',
                url: url + '/' + town,
                success: function (msg) {
                    if (msg && msg.length) {
                        Application.displayNotification(msg);
                    } else {
                        Application.clearNotification();
                    }
                },
                error: function (msg) {
                    Application.displayNotification('Error loading town critical main break notes.');
                }
            });
        } else {
            Application.clearNotification();
        }
    };

    // event handlers

    const onSaveButtonClicked = function () {
        if ($('form').valid()) {
            $('[disabled="disabled"]').removeAttr('disabled');
        }
    };

    const onAssetTypeChanged = function () {
        const $assetType = $(ELEMENTS.assetType);
        let label = 'Asset';
        let elem = ELEMENTS.dummyAsset;
        const type = getSelectedText($assetType);
        const value = $assetType.val();
        const lookup = ASSET_IDS[type];
        const lastVal = $assetType.data('last-val');

        if (value !== lastVal && !!lastVal) {
            hideAssetInputFields();
        } else if (!lastVal) {
            $(ELEMENTS.dummyAsset).hide();
        }

        trySetTownCriticalMainBreakNotes();

        if (lookup && lookup.element) {
            label = lookup.label || type;
            elem = lookup.element;
            $(ELEMENTS.assetRow).show();
            if ($(elem).val() !== '') // it has a value, lets make sure we fire change to load the history
                $(elem).change();
        } else if (lookup) { // lookup is valid, but there's no associated elem (main)
            label = '';
            elem = null;
            $(ELEMENTS.assetRow).hide();
        }

        $assetType.data('last-val', value);
        $(ELEMENTS.assetIdLabel).html(label);
        $(elem).show();

        if (value !== lastVal && !!lastVal) {
            loadAssetCoordinates(null);
            loadAssetHistory(null);
        }
    };

    const onOperatingCenterChanged = function () {
        const opCntr = $(ELEMENTS.operatingCenter).val();
        if (opCntr) {
            trySetState(opCntr);
            setFindLinkParameters(opCntr);
            $.ajax({
                type: 'GET',
                url: $(ELEMENTS.getOperatingCenterIsSAPWorkOrdersEnabled).val() + '/' + opCntr,
                success: function (result) {
                    $(ELEMENTS.sapWorkOrderNumber).val('');
                    $(ELEMENTS.sapWorkOrderNumber).closest('.field-pair').toggle((!result.IsSAPWorkOrdersEnabled));
                    $(ELEMENTS.pmatOverride).closest('.field-pair').toggle((result.IsSAPWorkOrdersEnabled));
                    $(ELEMENTS.sendToSAP).val(result.IsSAPWorkOrdersEnabled);
                }
            });
        }
        $(ELEMENTS.streetNumber).val('');
        $(ELEMENTS.zipCode).val('');
        $(ELEMENTS.meterSerialNumber).val('');
    };

    const onHistoryTableLoaded = function () {
        $(ELEMENTS.history).ajaxifyTable();
    };

    const onPremiseChange = function () {
        // premiseNumber values are only 9 or 10 chars
        const premiseNumber = $(ELEMENTS.premiseNumber).val();
        if (premiseNumber.length < 9 || premiseNumber.length > 10) {
            return;
        }

        const url = $(ELEMENTS.getSampleSitesByPremiseNumberUrl).val();
        $.ajax({
            type: 'GET',
            async: false,
            url: url,
            data: {'premiseNumber': premiseNumber},
            success: function (d) {
                if (d.length >= 1) {
                    alert('This Premise is linked to a Sample Site. Contact WQ before making any changes.');
                    $('#HasSampleSite').val('True');
                } else {
                    $('#HasSampleSite').val('False');
                }
            }
        });
    };

    const onPMATOverrideChanged = function () {
        $(ELEMENTS.accountCharged).closest('.field-pair').toggle($(ELEMENTS.pmatOverride).val() !== '');

        if ($(ELEMENTS.pmatOverride).val() === '18') {
            $(ELEMENTS.accountCharged).closest('.field-pair').toggle(false);
            $(ELEMENTS.accountCharged).val('');
        }
    };

    const loadAssetHistory = function (assetId) {
        if (!!assetId) {
            $(ELEMENTS.history)
                .load($(ELEMENTS.getAssetHistoryUrl).val() + '/',
                    $.param({
                        operatingCenter: $(ELEMENTS.operatingCenter).val(),
                        assetType: $(ELEMENTS.assetType).val(),
                        assetId: assetId
                    }),
                    onHistoryTableLoaded);
        } else {
            $(ELEMENTS.history).html('');
        }
    };

    const onAssetCoordinateLoaded = function (response) {
        $(ELEMENTS.coordinateId).val(response);
        const $icon = $(ELEMENTS.coordinateIcon);

        if (!!response) {
            $icon.attr('coordinateurl', $icon.attr('coordinateurl') + '&id=' + response);
            $icon.attr('src', $icon.attr('src').replace('red', 'blue'));
        } else {
            $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, ''));
            $icon.attr('src', $icon.attr('src').replace('blue', 'red'));
        }
    };

    const loadAssetCoordinates = function (assetId) {
        if (!!assetId && NO_COORDINATE_ASSETS.indexOf(getSelectedText(ELEMENTS.assetType).toUpperCase()) < 0) {
            $.getJSON($(ELEMENTS.getCoordinateIdForAssetUrl).val() + '/',
                $.param({
                    assetType: $(ELEMENTS.assetType).val(),
                    assetId: assetId
                }),
                onAssetCoordinateLoaded);
        } else {
            onAssetCoordinateLoaded(null);
        }
    };

    const onAssetIdChanged = function () {
        const type = getSelectedText(ELEMENTS.assetType);
        let lookup;

        $(ELEMENTS.coordinateLabel).text('');

        if ((lookup = ASSET_IDS[type]) && lookup.element) {
            const split = lookup.element.split(',');
            let value = $(split[0]).val() || '';

            for (let i = 1; i < split.length; ++i) {
                value += ',' + ($(split[i]).val() || '');
            }

            if (value !== '') {
                loadAssetHistory(value);
            }
            if ($(ELEMENTS.coordinateId).val() === "") {
                loadAssetCoordinates(value);
            }
        }
    };

    const onRequestedByChanged = function () {
        const $customer = $(ELEMENTS.customer);
        const $employee = $(ELEMENTS.requestingEmployee);
        const $acoustic = $(ELEMENTS.acousticMonitoringType);
        const requestedBy = getSelectedText(ELEMENTS.requestedBy);

        hideRow($customer);
        if (requestedBy !== 'Customer' && requestedBy !== '-- Select --')
            $customer.val('');

        hideRow($employee);
        hideRow($acoustic);

        switch (requestedBy) {
            case 'Employee':
                showRow($employee);
                break;
            case 'Customer':
                showRow($customer);
                break;
            case 'Acoustic Monitoring':
                showRow($acoustic);
                break;
        }
    };

    const onWorkDescriptionChanged = function () {
        const $mainBreak = $(ELEMENTS.mainBreak);

        if (isMainBreak()) {
            showRow($mainBreak);
        } else {
            clearDropDown($mainBreak);
            hideRow($mainBreak);
        }
        
        trySetTownCriticalMainBreakNotes();
        determineIfDescriptionRequiresDigitalAsBuilt();
    };

    const onTownChanged = function () {
        trySetTownCriticalMainBreakNotes();
    };

    const onInitialRevisitChanged = function () {
        ($(ELEMENTS.isRevisit).val() === 'True' ? showRow : hideRow)($(ELEMENTS.originalOrderNumber));
    };

    const loadRevisitOrder = function (number) {
        $.getJSON('Show.json?id=' + number.toString(), function (result) {
            const data = result.Data;
            setAndDisableField(ELEMENTS.operatingCenter, data.OperatingCenter);
            waitThenSetAndDisableField(ELEMENTS.town, data.Town);
            waitThenSetAndDisableField(ELEMENTS.townSection, data.TownSection);
            setAndDisableField(ELEMENTS.streetNumber, data.StreetNumber);
            waitThenSetAndDisableField(ELEMENTS.street, data.Street);
            waitThenSetAndDisableField(ELEMENTS.nearestCrossStreet, data.NearestCrossStreet);
            setAndDisableField(ELEMENTS.zipCode, data.ZipCode);
            waitThenSetField(ELEMENTS.assetType, data.AssetType);

            switch (true) {
                case !!data.Hydrant:
                    waitThenSetAndDisableField(ELEMENTS.hydrant, data.Hydrant);
                    break;
                case !!data.SewerOpening:
                    waitThenSetAndDisableField(ELEMENTS.sewerOpening, data.SewerOpening);
                    break;
                case !!data.StormCatch:
                    waitThenSetAndDisableField(ELEMENTS.stormCatch, data.StormCatch);
                    break;
                case !!data.Valve:
                    waitThenSetAndDisableField(ELEMENTS.valve, data.Valve);
                    break;
                case !!data.PremiseNumber || !!data.ServiceNumber:
                    setAndDisableField(ELEMENTS.premiseNumber, data.PremiseNumber);
                    setAndDisableField(ELEMENTS.serviceNumber, data.ServiceNumber);
                    break;
            }
        });
    };

    const onOriginalOrderNumberChanged = function () {
        const $originalOrderNumber = $(ELEMENTS.originalOrderNumber);
        const number = $originalOrderNumber.val();

        if (!$.isNumeric(number)) {
            $originalOrderNumber.select();
        } else {
            loadRevisitOrder(number);
        }
    };

    // initialization

    const initEventHandlers = function () {
        $(ELEMENTS.saveButton).on('click', onSaveButtonClicked);
        $(ELEMENTS.assetType).on('change', onAssetTypeChanged);
        $(ELEMENTS.operatingCenter).on('change', onOperatingCenterChanged);
        if ($(ELEMENTS.operatingCenter).val() !== '')
            trySetState($(ELEMENTS.operatingCenter).val());
        for (let x in ASSET_IDS) {
            if (ASSET_IDS.hasOwnProperty(x)) {
                $(ASSET_IDS[x].element).on('change', onAssetIdChanged);
            }
        }
        $(ELEMENTS.requestedBy).on('change', onRequestedByChanged);
        $(ELEMENTS.workDescription).on('change', onWorkDescriptionChanged);
        $(ELEMENTS.town).on('change', onTownChanged);
        $(ELEMENTS.isRevisit).on('change', onInitialRevisitChanged);
        $(ELEMENTS.originalOrderNumber).on('change', onOriginalOrderNumberChanged);
        $(ELEMENTS.pmatOverride).on('change', onPMATOverrideChanged);
        $(ELEMENTS.pmatOverride).change();
        $(ELEMENTS.premiseNumber).on('change', onPremiseChange);
    };

    const setFindLinkParameters = function (opCtnr) {
        $('#PremiseFindLink').attr('href', '../../Customer/Premise/Find?operatingCenterId=' + opCtnr);
        $('#WBSElementFindLink').attr('href', '../../SAP/WBSElement/Find?operatingCenterId=' + opCtnr);
    };

    const setRevisit = function () {
        const revisit = $(ELEMENTS.isRevisit);
        if (revisit.val() === '')
            revisit.val('False');
    };

    const tryInitializeAssetType = function () {
        const $assetType = $(ELEMENTS.assetType);
        const $workDescription = $(ELEMENTS.workDescription);
        const workDescriptionValue = $workDescription.val();
        if (!$assetType.is(':disabled') && (!$workDescription.is(':disabled') || workDescriptionValue === '')) {
            $assetType.change();
            $workDescription.change();
            if (workDescriptionValue !== '')
                $workDescription.val(workDescriptionValue);
        } else {
            window.setTimeout(tryInitializeAssetType, 50);
        }
    };

    const wo = {
        init: function () {
            initEventHandlers();
            window.setTimeout(tryInitializeAssetType, 50);
            onRequestedByChanged();
            onWorkDescriptionChanged();
            setRevisit();
            onInitialRevisitChanged();
            AjaxTable.initialize('#premiseNumberTable');
            AjaxTable.initialize('#installationTable');
            AjaxTable.initialize('#wbsElementTable');
            if ($(ELEMENTS.assetType).val() !== '') {
                onAssetTypeChanged();
                onAssetIdChanged();
            }
        },

        getAddress: function () {
            const sb = [];

            sb.push($(ELEMENTS.streetNumber).val());
            sb.push($(ELEMENTS.streetAutocomplete).val());
            sb.push(getSelectedText(ELEMENTS.town));
            sb.push($(ELEMENTS.state).val());
            sb.push($(ELEMENTS.zipCode).val());

            return sb.join(' ');
        },

        validatePremiseNumber: function (premiseNumber, element) {
            if ($(ELEMENTS.assetType).val() === "4" || $(ELEMENTS.assetType).val() === "6") {
                if (/^(\d)\1{8,9}$/.test(premiseNumber) && !isNoPremiseDescription()) {
                    if (!/^.{5}/.test($(ELEMENTS.notes).val())) {
                        return false;
                    }
                }
            }
            return true;
        },

        validatePlantMaintenanceActivityCode: function (plantMaintenanceActivityCode, element) {
            //if we aren't sending to sap, then we are valid here.
            if ($(ELEMENTS.sendToSAP).val() === "false")
                return true;
            const workDescription = $(ELEMENTS.workDescription);
            if (workDescription == null)
                return true;

            const override = $(ELEMENTS.pmatOverride);
            if (BRB_PMAT_DESCRIPTIONS.indexOf(workDescription.val()) > -1 && override.val() === '') {
                if ($(ELEMENTS.pmatOverride).val() === '')
                    return false;
            }

            return true;
        },

        validateServiceForSap: function (val, element) {
            if ($(ELEMENTS.assetType).val() === "4" || $(ELEMENTS.assetType).val() === "6") {
                if (/^(\d)\1{8,9}$/.test($(ELEMENTS.premiseNumber).val())) {
                    return true;
                }
                // Cut out early beacuse OperatingCenter is a required field and we don't wanna throw errors
                // when they haven't selected an operating center yet.
                var opc = $(ELEMENTS.operatingCenter).val();
                if (!opc) {
                    return true;
                }
                // Always valid if it has a value.
                if (val) {
                    return true;
                }
                var isValid;
                var isContractedOperationsServiceUrl = $('#IsContractedOperationsServiceUrl').val();

                $.ajax({
                    url: isContractedOperationsServiceUrl,
                    data: {
                        id: opc
                    },
                    async: false, // So this just goes.
                    type: 'GET',
                    success: function (result) {
                        isValid = result.IsContractedOperations;
                    },
                    error: function () {
                        alert("Something went wrong when validating the Device Location and Installation.");
                    }
                });
                return isValid;
            }
            return true;
        },

        validateWBSNumber: function (val, element) {
            // whether or not to send to sap is determined on operating center change
            if ($(ELEMENTS.operatingCenter).val() === '' || $(ELEMENTS.sendToSAP).val() === 'false')
                return true;

            const url = '../../SAP/WBSElement/index.json';
            // not required to verify if it is not populated or assetType/operating center hasn't been selected
            if (element.value === '' || $(ELEMENTS.assetType).val() === '')
                return true;
            let valid = false;
            $.ajax({
                type: 'GET',
                async: false,
                url: (window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\/|\/NEWFROMWORKORDER\//))
                    ? '../' + url
                    : url,
                data: {
                    'WBSNumber': $(ELEMENTS.accountCharged).val(),
                    'OperatingCenter': $(ELEMENTS.operatingCenter).val(),
                    'AssetType': $(ELEMENTS.assetType).val()
                },
                success: function (d) {
                    if (d.Data[0].SAPErrorCode.indexOf('Successful') > -1)
                        valid = true;
                    else {
                        window.setTimeout(function () {
                            $('span [for=AccountCharged]').text(d.Data[0].SAPErrorCode);
                        }, 100);
                    }
                }
            });

            return valid;
        },

        validatePlannedCompletionDate: function (val, element) {
            let plannedCompletionDate = new Date($(ELEMENTS.plannedCompletionDate).val()),
                now = new Date();
            plannedCompletionDate.setHours(0, 0, 0, 0);
            now.setHours(0, 0, 0, 0);

            const selectedPriorityId = parseInt($(ELEMENTS.priority).val(), 10);
            if (!val || (selectedPriorityId === 1 && plannedCompletionDate >= now)) return true;

            if (plannedCompletionDate < now) {
                return false;
            }
            now.setDate(now.getDate() + 2)            

            if (plannedCompletionDate >= now) {
                return true;
            }

            return false;
        }
    };

    //TOGGLES
    //Initial/Revisit - OriginalOrderNumber
    //	Disables - OperatingCenter, Town, TownSection, StreetNumber, Street, NearestCrossStreet, ZipCode
    //AssetType - Hydrant/Valve/SewerOpening/Equipment/MainCrossing
    //RequestedBy - CustomerName, PhoneNumber, SecondaryPhoneNumber, RequestingEmployee
    //WorkDescription - MainBreak - EstimatedCustomerImpact, AnticipatedRepairTime, AlertIssued, SignificantTrafficImpact
    //Show List of Open and Completed WorkOrders for Asset- Toggles when Hydrant/Valve/SewerOpening/Equipment selected

    $(document).ready(wo.init);

    return wo;
})(jQuery, window.workDescriptions);
