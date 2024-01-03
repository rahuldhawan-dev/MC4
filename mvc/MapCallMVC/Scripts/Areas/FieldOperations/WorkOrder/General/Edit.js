var WorkOrders = (function ($) {
    var ELEMENTS = {};
    const MAIN_BREAK = [74, 80];
    const REQUESTED_BY_CUSTOMER_ID = 1;
    const REQUESTED_BY_EMPLOYEE_ID = 2;
    const REQUESTED_BY_ACOUSTIC_MONITORING_ID = 6;
    const ASSET_TYPE_VALVE_ID = 1;
    const ASSET_TYPE_HYDRANT_ID = 2;
    const ASSET_TYPE_SERVICE_ID = 4;
    const ASSET_TYPE_SEWER_OPENING_ID = 5;
    const ASSET_TYPE_SEWER_LATERAL_ID = 6;
    const ASSET_TYPE_EQUIPMENT_ID = 9;
    const ASSET_TYPE_MAIN_CROSSING_ID = 12;
    const BRB_PMAT_DESCRIPTIONS = [54, 62, 75, 76, 85, 124, 225, 226];
    const PBC_PMAT_TYPE = 18;
    const NO_PREMISE_WORK_DESCRIPIONS = [60, 315]
    const getSelectedText = function (selector) {
        return $(':selected', selector).text();
    };
    var m = {
        initialize: function () {
            ELEMENTS = {
                accountCharged: $('#AccountCharged'),
                assetType: $('#AssetType'),
                digitalAsBuiltRequired: $('#DigitalAsBuiltRequired'),
                getTownCriticalMainBreakNotesUrl: $('#GetTownCriticalMainBreakNotesUrl'),
                getSampleSitesByPremiseNumberUrl: $('#GetSampleSitesByPremiseNumberUrl'),
                getCoordinateIdForAssetUrl: $('#GetCoordinateIdForAssetUrl'),
                hydrant: $('#Hydrant'),
                mainCrossing: $('#MainCrossing'),
                operatingCenter: $('#OperatingCenter'),
                pmatOverride: $('#PlantMaintenanceActivityTypeOverride'),
                premiseNumber: $('#PremiseNumber'),
                requestedBy: $('#RequestedBy'),
                sendToSAP: $('#SendToSAP'),
                sewerOpening: $('#SewerOpening'),
                state: '#State',
                street: '#Street',
                streetAutocomplete: '#Street_AutoComplete',
                streetNumber: '#StreetNumber',
                town: '#Town',
                valve: $('#Valve'),
                workDescription: $('#WorkDescription'),
                zipCode: '#ZipCode'
            };
            townStateServiceUrl = $('#TownStateServiceUrl').val();
            m.initAssetType();
            m.initValveId();
            m.initHydrantId();
            m.initSewerOpeningId();
            m.initMainCrossingId();
            m.initRequestedBy();
            m.initWorkOrderDescription();
            m.initPremiseNumber();            
            AjaxTable.initialize('#EditCrewAssignments');
        },
        initAssetType: function () {
            ELEMENTS.assetType.on('change', m.onAssetTypeChanged);
            m.onAssetTypeChanged();
        },
        onAssetTypeChanged: function () {
            $('#Hydrant').hide();
            $('#Valve').hide();
            $('#MainCrossing').hide();
            $('#SewerOpening').hide();
            $('#Equipment').hide();
            $('#PremiseNumber').hide();
            $('#ServiceNumber').hide();
            $('#serviceInfo').hide();
            $('#AssetChangeLink').hide();
            $('#MeterLocationInfo').hide();
            const selectedAssetTypeId = parseInt(ELEMENTS.assetType.val(), 10);
            if (selectedAssetTypeId == ASSET_TYPE_VALVE_ID) {
                $('#Valve').show();
                $("#assetLabel").html("Valve Id:");                
            }
            else if (selectedAssetTypeId == ASSET_TYPE_HYDRANT_ID) {
                $('#Hydrant').show();
                $("#assetLabel").html("Hydrant Id:");
            }
            else if (selectedAssetTypeId == ASSET_TYPE_MAIN_CROSSING_ID) {
                $('#MainCrossing').show();
                $("#assetLabel").html("Main Crossing:");
            }
            else if (selectedAssetTypeId == ASSET_TYPE_SERVICE_ID) {
                $('#ServiceNumber').show();
                $('#PremiseNumber').show();
                $('#serviceInfo').show();
                $("#assetLabel").html("Premise #: <br> Service #:");
                $('#AssetChangeLink').show();
                $('#MeterLocationInfo').show();
            }
            else if (selectedAssetTypeId == ASSET_TYPE_SEWER_LATERAL_ID) {
                $('#AssetChangeLink').show();
            }
            else if (selectedAssetTypeId == ASSET_TYPE_SEWER_OPENING_ID) {
                $('#SewerOpening').show();
            }
            else if (selectedAssetTypeId == ASSET_TYPE_EQUIPMENT_ID) {
                $('#Equipment').show();
                $("#assetLabel").html("Equipment:");
            }
            else {
                $("#assetLabel").html("");
            }
        },

        initValveId: function () {
            ELEMENTS.valve.on('change', m.onValveIdChanged);
        },
        onValveIdChanged: function () {
            m.loadAssetCoordinates(ASSET_TYPE_VALVE_ID, parseInt($('#Valve').val(), 10))
        },
        initHydrantId: function () {
            ELEMENTS.hydrant.on('change', m.onHydrantIdChanged);
        },
        onHydrantIdChanged: function () {
            m.loadAssetCoordinates(ASSET_TYPE_HYDRANT_ID, parseInt($('#Hydrant').val(), 10))
        },
        initSewerOpeningId: function () {
            ELEMENTS.sewerOpening.on('change', m.onSewerOpeningIdChanged);
        },
        onSewerOpeningIdChanged: function () {
            m.loadAssetCoordinates(ASSET_TYPE_SEWER_OPENING_ID, parseInt($('#SewerOpening').val(), 10))
        },
        initMainCrossingId: function () {
            ELEMENTS.mainCrossing.on('change', m.onMainCrossingIdChanged);
        },
        onMainCrossingIdChanged: function () {
            m.loadAssetCoordinates(ASSET_TYPE_MAIN_CROSSING_ID, parseInt($('#MainCrossing').val(), 10))
        },
        loadAssetCoordinates: function (assetType, assetId) {
            if (!assetType || !assetId) {
                m.onAssetCoordinateLoaded(null);
                return;
            }
            $.getJSON(ELEMENTS.getCoordinateIdForAssetUrl.val() + '/',
                $.param({
                    assetType: assetType,
                    assetId: assetId
                }),
                m.onAssetCoordinateLoaded);
        },
        onAssetCoordinateLoaded: function (response) {
            const $icon = $('img.coordinate-picker-icon');
            if (response) {
                $('#CoordinateId').val(response);
                $('.cp-coordinate-values').text(response);
                $icon.attr('coordinateurl', $icon.attr('coordinateurl') + '&id=' + response);
                $icon.attr('src', $icon.attr('src').replace('red', 'blue'));
            }
            else {
                $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, ''));
                $icon.attr('src', $icon.attr('src').replace('blue', 'red'));
                $('#CoordinateId').val('');
                $('.cp-coordinate-values').text('');
            }
        },
        getAddress: function () {
            const sb = [];

            sb.push($(ELEMENTS.streetNumber).val());
            sb.push($(ELEMENTS.streetAutocomplete).val());
            sb.push(getSelectedText(ELEMENTS.town));
            var state;
            if (getSelectedText(ELEMENTS.town).length > 0) {
                $.ajax({
                    url: townStateServiceUrl,
                    data: {
                        id: $(ELEMENTS.town).val()
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

            sb.push(state);
            sb.push($(ELEMENTS.zipCode).val());

            return sb.join(' ');
        },
        initWorkOrderDescription: function () {
            ELEMENTS.workDescription.on('change', m.onWorkDescriptionChanged);
            m.onWorkDescriptionChanged();
        },
        onWorkDescriptionChanged: function () {            
            const selectedWorkDescriptionId = parseInt(ELEMENTS.workDescription.val(), 10);            
            if (MAIN_BREAK.includes(selectedWorkDescriptionId)) {
                $('#MainBreakInfo1').show();
                $('#MainBreakInfo2').show();
                m.trySetTownCriticalMainBreakNotes();
            }
            else {
                $('#MainBreakInfo1').hide();
                $('#MainBreakInfo2').hide();
            }

            var selectedWorkDescription = ELEMENTS.workDescription.val();
            if (selectedWorkDescription) {
                $.ajax({
                    url: $('#WorkDescriptionUrl').val(),
                    async: false,
                    type: 'GET',
                    data: {
                        id: selectedWorkDescriptionId
                    },
                    success: function (result) {
                        if (result != null && result.DigitalAsBuiltRequired != null && result.DigitalAsBuiltRequired == true) {
                            ELEMENTS.digitalAsBuiltRequired.prop("checked", true);
                            ELEMENTS.digitalAsBuiltRequired.prop("disabled", true);
                        } else {
                            ELEMENTS.digitalAsBuiltRequired.prop("checked", false);
                            ELEMENTS.digitalAsBuiltRequired.prop("disabled", false);
                        }
                    }
                });
            }            
        },
        trySetTownCriticalMainBreakNotes: function () {
            var town = $(ELEMENTS.town).val();
            var url = $('#GetTownCriticalMainBreakNotesUrl').val();
            if (town) {
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
        },

        initRequestedBy: function () {
            ELEMENTS.requestedBy.on('change', m.onRequestedByChanged);
            m.onRequestedByChanged();
        },
        onRequestedByChanged: function () {            
            $('#CustomerInfo1').hide();
            $('#CustomerInfo2').hide();
            $('#CustomerName').hide();
            $('#AcousticMonitoringType').hide();
            $('#AcousticMonitoring').hide();
            $('#RequestingEmployee').hide();
            $('#EmployeeName').hide();
            const selectedRequestedById = parseInt(ELEMENTS.requestedBy.val(), 10);
            if (selectedRequestedById === REQUESTED_BY_CUSTOMER_ID) {
                $('#CustomerInfo1').show();
                $('#CustomerInfo2').show();
                $('#CustomerName').show();
            }
            else if (selectedRequestedById === REQUESTED_BY_EMPLOYEE_ID) {
                $('#RequestingEmployee').show();
                $('#EmployeeName').show();
            }
            else if (selectedRequestedById === REQUESTED_BY_ACOUSTIC_MONITORING_ID) {
                $('#AcousticMonitoringType').show();
                $('#AcousticMonitoring').show();
            }
        },

        initPlantMaintenanceActivityTypeOverride: function () {
            ELEMENTS.pmatOverride.on('change', m.onPlantMaintenanceActivityTypeOverrideChanged);
            m.onPlantMaintenanceActivityTypeOverrideChanged();
        },
        onPlantMaintenanceActivityTypeOverrideChanged: function () {
            const selectedPlantMaintenanceActivityTypeOverrideId = parseInt(ELEMENTS.pmatOverride.val(), 10);
            if (selectedPlantMaintenanceActivityTypeOverrideId === PBC_PMAT_TYPE) {
                ELEMENTS.accountCharged.val('');
            }
        },

        initPremiseNumber: function () {
            ELEMENTS.premiseNumber.on('change', m.onPremiseNumberChanged);
            m.onPremiseNumberChanged();
        },
        onPremiseNumberChanged: function () {
            const premiseNumber = ELEMENTS.premiseNumber.val();
            if (premiseNumber.length < 9 || premiseNumber.length > 10) {
                return;
            }

            const url = ELEMENTS.getSampleSitesByPremiseNumberUrl.val();
            $.ajax({
                type: 'GET',
                async: false,
                url: url,
                data: { 'premiseNumber': premiseNumber },
                success: function (d) {
                    if (d.length >= 1) {
                        alert('This Premise is linked to a Sample Site. Contact WQ before making any changes.');
                        $('#HasSampleSite').val('True');
                    } else {
                        $('#HasSampleSite').val('False');
                    }
                }
            });
        },

        validatePremiseNumber: function (premiseNumber, element) {
            const selectedAssetTypeId = parseInt(ELEMENTS.assetType.val(), 10);
            const selectedWorkDescriptionId = parseInt(ELEMENTS.workDescription.val(), 10);
            if (selectedAssetTypeId == ASSET_TYPE_SEWER_LATERAL_ID || selectedAssetTypeId == ASSET_TYPE_SERVICE_ID) {
                if (/^(\d)\1{8,9}$/.test(premiseNumber) && !NO_PREMISE_WORK_DESCRIPIONS.includes(selectedWorkDescriptionId)) {
                    if (!/^.{5}/.test($(ELEMENTS.notes).val())) {
                        return false;
                    }
                }
            }
            return true;
        },

        validatePlantMaintenanceActivityCode: function (plantMaintenanceActivityCode, element) {
            //if we aren't sending to sap, then we are valid here.
            if (ELEMENTS.sendToSAP.val() === "false")
                return true;
            const selectedWorkDescriptionId = parseInt(ELEMENTS.workDescription.val(), 10);
            if (BRB_PMAT_DESCRIPTIONS.includes(selectedWorkDescriptionId) && plantMaintenanceActivityCode === '') {
                return false;
            }

            return true;
        },

        validateWBSNumber: function (val, element) {
            // whether or not to send to sap is determined on operating center change
            if (ELEMENTS.operatingCenter.val() === '' || ELEMENTS.sendToSAP.val() === 'false')
                return true;

            // not required to verify if it is not populated or assetType/operating center hasn't been selected
            if (element.value === '' || $(ELEMENTS.assetType).val() === '')
                return true;
            let valid = false;
            $.ajax({
                url: $('#WbsElementUrl').val(),
                type: 'GET',
                async: false,
                data: {
                    'WBSNumber': ELEMENTS.accountCharged.val(),
                    'OperatingCenter': ELEMENTS.operatingCenter.val(),
                    'AssetType': ELEMENTS.assetType.val()
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
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);