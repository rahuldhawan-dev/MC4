var Services = (function ($, categories, installationPurposes) {
    var ELEMENTS = {};
    var RENEWAL_SERVICE_CATEGORIES;
    var SEWER_SERVICE_CATEGORIES;
    var MEASUREMENT_ONLY;
    var townStateServiceUrl;
    const waterServiceRenewalCustomerSideCategoryId = 27;

    var s = {
        init: function () {
            ELEMENTS = {
                apartmentNumber: $('#ApartmentNumber'),
                coordinate: $('#Coordinate'),
                customerSideSLReplacement: $('#CustomerSideSLReplacement'),
                dateInstalled: $('#DateInstalled'),
                deviceLocation: $('#DeviceLocation'),
                deviceLocationUnavailable: $('#DeviceLocationUnavailable'),
                installation: $('#Installation'),
                isExistingOrRenewal: $('#IsExistingOrRenewal'),
                isInstalledNonVerification: $('#IsInstalledNonVerification'),
                mailState: $('#MailState'),
                mailStreet: $('#MailStreetName'),
                mailStreetNumber: $('#MailStreetNumber'),
                mailTown: $('#MailTown'),
                mailZip: $('#MailZip'),
                offeredAgreement: $('#OfferedAgreement'),
                offeredAgreementDate: $('#OfferedAgreementDate'),
                operatingCenter: $('#OperatingCenter'),
                operatingCenterSAPEnabledUrl: $('#OperatingCenterSAPEnabledUrl'),
                premiseNumber: $('#PremiseNumber'),
                premiseNumberUnavailable: $('#PremiseNumberUnavailable'),
                premiseUnavailableReason: $('#PremiseUnavailableReason'),
                previousServiceCustomerMaterial: $('#PreviousServiceCustomerMaterial'),
                previousServiceCustomerSize: $('#PreviousServiceCustomerSize'),
                previousServiceMaterial: $('#PreviousServiceMaterial'),
                previousServiceSize: $('#PreviousServiceSize'),
                retiredDate: $('#RetiredDate'),
                sameAsAbove: $('#SameAsAbove'),
                selectedServiceCategory: $('#ServiceCategory option:selected'),
                serviceCategory: $('#ServiceCategory'),
                serviceInstallationPurpose: $('#ServiceInstallationPurpose'),
                serviceNumber: $('#ServiceNumber'),
                state: $('#State'),
                street: $('#Street'),
                streetNumber: $('#StreetNumber'),
                terminationPoint: $('#TerminationPoint'),
                town: $('#Town'),
                townName: $('#TownName'),
                zip: $('#Zip'),
            };
            RENEWAL_SERVICE_CATEGORIES = ['3', '6', '13', '19', '23'];
            SEWER_SERVICE_CATEGORIES = ['8', '9', '10', '11', '12', '13', '14', '25', '26', '34']
            MEASUREMENT_ONLY = ['19', '15', '8', '10'];
            townStateServiceUrl = $('#TownStateServiceUrl').val();
            ELEMENTS.sameAsAbove.click(s.onSameAsAboveClick);
            ELEMENTS.installation.change(s.onInstallationChange);
            s.initIsExistingOrRenewal();
            AjaxTable.initialize('#premiseNumberTable');
            AjaxTable.initialize('#wbsElementTable');
            ELEMENTS.operatingCenter.on('change', s.setFindLinkParameters);
            s.setFindLinkParameters();
            s.initPremiseNumberUnavailable();
            ELEMENTS.customerSideSLReplacement.change(s.setAgreementFieldsVisibility);
            s.setAgreementFieldsVisibility(); // call once so toggling is set on initial page load
            ELEMENTS.terminationPoint.change(s.setOtherPointVisibility);
            s.setOtherPointVisibility(); // call once so toggling is set on initial page load
            ELEMENTS.serviceCategory.change(s.onServiceCategoryChanged);
            s.onServiceCategoryChanged();

            ELEMENTS.dateInstalled.change(s.maybeSetIsInstalledNonVerification);
            ELEMENTS.serviceInstallationPurpose.change(s.maybeSetIsInstalledNonVerification);
        },

        setFindLinkParameters: function () {
            $('#WBSElementFindLink').attr('href', $('#WBSElementFindUrl').val() + '?operatingCenterId=' + ELEMENTS.operatingCenter.val());
            s.onServiceCategoryChanged();
        },

        initIsExistingOrRenewal: function () {
            if (ELEMENTS.isExistingOrRenewal.length === 0) {
                return;
            }
            ELEMENTS.isExistingOrRenewal.on('change', s.onIsExistingOrRenewalChanged);
            s.onIsExistingOrRenewalChanged();
        },

        onInstallationChange: function () {
            if (ELEMENTS.operatingCenter.val() === '')
                return;

            var url = $('#AnyWithInstallationNumberAndOperatingCenterUrl').val();
            $.ajax({
                type: 'GET',
                async: false,
                url: url,
                data: { 'installation': ELEMENTS.installation.val(), 'operatingCenterId': ELEMENTS.operatingCenter.val() },
                success: function (d) {
                    var warning = 'Another Service Record exists with this installation that is linked to a Sample Site. Contact WQ before making any changes.';
                    if (d.Data.length > 1) {
                        alert(warning);
                    } else if (d.Data.length > 0) {
                        for (var x = 0; x < d.Data.length; x++) {
                            var id = $('#ServiceId').val();
                            if (d.Data[x].Id.toString() === id)
                                return;
                        }
                        alert(warning);
                    }
                }
            });
        },

        onServiceCategoryChanged: function () {
            const selectedServiceCategoryId = parseInt(ELEMENTS.serviceCategory.val(), 10);
            if (selectedServiceCategoryId == waterServiceRenewalCustomerSideCategoryId) {
                $('option:selected', ELEMENTS.previousServiceMaterial).removeAttr('selected');
                ELEMENTS.previousServiceMaterial.attr('disabled', 'disabled');
                $('option:selected', ELEMENTS.previousServiceSize).removeAttr('selected');
                ELEMENTS.previousServiceSize.attr('disabled', 'disabled');
                ELEMENTS.retiredDate.val('');
                ELEMENTS.retiredDate.attr('disabled', 'disabled');
                Application.displayNotification('Previous Service Material, Previous Service Size and Retired Date are not required for customer renewals.');
                ELEMENTS.previousServiceCustomerMaterial.removeAttr('disabled');
                ELEMENTS.previousServiceCustomerSize.removeAttr('disabled');
                $('#customerSideSLSection').show();
            } else {
                ELEMENTS.previousServiceMaterial.removeAttr('disabled');
                ELEMENTS.previousServiceSize.removeAttr('disabled');
                ELEMENTS.retiredDate.removeAttr('disabled');
                Application.clearNotification();
                $('option:selected', ELEMENTS.previousServiceCustomerMaterial).removeAttr('selected');
                ELEMENTS.previousServiceCustomerMaterial.attr('disabled', 'disabled');
                $('option:selected', ELEMENTS.previousServiceCustomerSize).removeAttr('selected');
                ELEMENTS.previousServiceCustomerSize.attr('disabled', 'disabled');
                $('#customerSideSLSection').hide();
            }

            s.maybeSetIsInstalledNonVerification();
        },

        onIsExistingOrRenewalChanged: function () {
            var suffixParent = ELEMENTS.serviceNumber.closest('.field-pair');
            if (ELEMENTS.isExistingOrRenewal.is(':checked')) {
                ELEMENTS.serviceNumber.prop('disable', false);
                suffixParent.show();
            } else {
                ELEMENTS.serviceNumber.prop('disable', true);
                suffixParent.hide();
            }
        },

        onSameAsAboveClick: function () {
            var checked = $('#SameAsAbove')[0].checked;
            ELEMENTS.mailStreetNumber.val(checked ? ELEMENTS.streetNumber.val() : "");
            if (ELEMENTS.apartmentNumber.val() !== "") {
                ELEMENTS.mailStreetNumber.val(checked ? ELEMENTS.mailStreetNumber.val() + ' Apt ' + ELEMENTS.apartmentNumber.val() : "");
            }
            ELEMENTS.mailStreet.val(checked ? ELEMENTS.street.find('option:selected').text() : "");
            ELEMENTS.mailTown.val(checked ? ELEMENTS.town.find('option:selected').text() : "");
            ELEMENTS.mailState.val(checked ? ELEMENTS.state.find('option:selected').text() : "");
            ELEMENTS.mailZip.val(checked ? ELEMENTS.zip.val() : "");
        },

        getAddress: function () {
            // We've already selected a coordinate. Don't attempt to get the address again.
            if (ELEMENTS.coordinate.val()) {
                return null;
            }

            var selectedTown = ELEMENTS.town.find('option:selected');
            var town = ELEMENTS.townName.val() || selectedTown.text();
            var streetNumber = ELEMENTS.streetNumber.val();
            var street = ELEMENTS.street.find('option:selected').text();
            var state = ELEMENTS.state.find('option:selected').text();

            if ((!state || state === '-- Select --') && selectedTown.val()) {
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

        setAgreementFieldsVisibility: function () {
            var valuesThatShouldBeVisible = ['On List For Future LSLR',
                'Initiated Customer Contact',
                'Test Pit Completed',
                'Offered Agreement',
                'Offered-Accepted',
                'Offered-Rejected'];
            var selectedText = ELEMENTS.customerSideSLReplacement.find('option:selected').text();
            var shouldBeVisible = valuesThatShouldBeVisible.indexOf(selectedText) > -1;
            $('.fp-agreement-related').toggle(shouldBeVisible);
        },

        setOtherPointVisibility: function () {
            var selectedText = ELEMENTS.terminationPoint.find('option:selected').text();
            var shouldBeVisible = selectedText === 'Other';
            $('.fp-other-point').toggle(shouldBeVisible);
        },

        validateDeviceLocation: function (val, el) {
            //if we have a value, lets return
            if (val)
                return true;
            if (!s.isSAPEnabled()) // this isn't required unless we're in an SAP enabled OperatingCenter
                return true;
            if (ELEMENTS.dateInstalled.val() !== '')
                return true;

            //Category of Service  =  water retire service only, water measurement only, sewer measurement only,  sewer retire service only  - - need popup which says "no Device Location" 
            if (MEASUREMENT_ONLY.indexOf(ELEMENTS.serviceCategory.val()) > -1) {
                return true;//('By continuing you acknowledge that there is no device location');
            }

            // If Premise Number is Unavailable, this is not required
            if (ELEMENTS.premiseNumberUnavailable.is(':checked')) {
                return true;
            }
            if (ELEMENTS.deviceLocationUnavailable.is(':checked')) {
                return true;
            }

            if (val == null || val === '')
                return false;
            return true;
        },

        validateDeviceLocationUnavailable: function (val, el) {
            if (MEASUREMENT_ONLY.indexOf(ELEMENTS.serviceCategory.val()) === -1)
                return true;
            if (ELEMENTS.deviceLocation.val() === '' && !el.checked)
                return false;
            return true;
        },

        validatePremiseNumberUnavailable: function (val, el) {
            if (ELEMENTS.premiseNumber.val() === '' && !el.checked) {
                return false;
            }
            return true;
        },

        validatePremiseNumber: function (val, el) {
            if (val === '0000000000' && !ELEMENTS.premiseNumberUnavailable.is(':checked')) {
                return false;
            }
            return true;
        },

        initPremiseNumberUnavailable: function () {
            if (ELEMENTS.premiseNumberUnavailable.length === 0) {
                return;
            }
            ELEMENTS.premiseNumberUnavailable.on('change', s.onPremiseNumberUnavailableChanged);
            s.onPremiseNumberUnavailableChanged();
        },

        onPremiseNumberUnavailableChanged: function () {
            if (ELEMENTS.premiseNumberUnavailable.is(':checked')) {
                ELEMENTS.premiseUnavailableReason.closest('.field-pair').show();
                ELEMENTS.premiseNumber.closest('.field-pair').hide();
                ELEMENTS.installation.closest('.field-pair').hide();
                ELEMENTS.deviceLocation.closest('.field-pair').hide();
                ELEMENTS.deviceLocationUnavailable.prop('checked', true);
                ELEMENTS.premiseNumber.val('');
            } else {
                ELEMENTS.premiseUnavailableReason.val('');
                ELEMENTS.premiseUnavailableReason.closest('.field-pair').hide();
                ELEMENTS.premiseNumber.closest('.field-pair').show();
                ELEMENTS.installation.closest('.field-pair').show();
                ELEMENTS.deviceLocation.closest('.field-pair').show();
                if (ELEMENTS.deviceLocation.val().length > 0) {
                    ELEMENTS.deviceLocationUnavailable.prop('checked', false);
                }
            }
        },

        validateRetirementInformation: function (val, el) {
            var isRequired = (RENEWAL_SERVICE_CATEGORIES.indexOf(ELEMENTS.serviceCategory.val()) > -1 && ELEMENTS.dateInstalled.val() !== '');
            if (isRequired && !val) {
                return false;
            }
            return true;
        },

        validateMeterSettingRequirement: function (val, el) {
            if (SEWER_SERVICE_CATEGORIES.indexOf(ELEMENTS.serviceCategory.val()) > -1) {
                return true;
            }
            var isRequired = (ELEMENTS.isInstalledNonVerification.val() === 'true' ||
                ELEMENTS.retiredDate.val() !== '');
            if (isRequired && !val) {
                return false;
            }
            return true;
        },

        validateMeterSettingSize: function (val, el) {
            if (SEWER_SERVICE_CATEGORIES.indexOf(ELEMENTS.serviceCategory.val()) > -1) {
                return true;
            }

            const isRequired = (ELEMENTS.isInstalledNonVerification.val() === 'true');
            return !(isRequired && !val);
        },

        isSAPEnabled: function () {
            // Cut out early beacuse OperatingCenter is a required field and we don't wanna throw errors
            // when they haven't selected an operating center yet.
            var opc = ELEMENTS.operatingCenter.val();
            if (!opc) {
                return false;
            }

            var isValid = false;
            $.ajax({
                url: ELEMENTS.operatingCenterSAPEnabledUrl.val(),
                data: {
                    id: opc
                },
                async: false, // So this just goes.
                type: 'GET',
                success: function (result) {
                    isValid = result.IsSAPEnabled;
                },
                error: function () {
                    alert("Something went wrong looking up SAP Enabled.");
                }
            });
            return isValid;
        },

        validateWBSNumber: function (val, element) {
            var sapEnabled = s.isSAPEnabled(); //this is a negative method
            // whether or not to send to sap is determined on operating center change
            // not required to verify if it is not populated or assetType/operating center hasn't been selected
            if (ELEMENTS.operatingCenter.val() === ''
                || element.value === ''
                || !sapEnabled)
                return true;
            if (ELEMENTS.dateInstalled.val() !== '') {
                return true;
            }

            var url = '../../SAP/WBSElement/index.json';
            var valid = false;
            var assetTypeId =
                ($('#ServiceCategory option:selected').text().toUpperCase().indexOf('SEWER') > -1) ? 6 : 4;

            $.ajax({
                type: 'GET',
                async: false,
                url: (window.location.pathname.toUpperCase().indexOf('/EDIT/') > -1 ||
                    window.location.pathname.toUpperCase().indexOf('/NEW/') > -1)
                    ? '../' + url
                    : url,
                data: {
                    'WBSNumber': val,
                    'OperatingCenter': ELEMENTS.operatingCenter.val(),
                    'AssetType': assetTypeId
                },
                success: function (d) {
                    if (d.Data[0].SAPErrorCode.indexOf('Successful') > -1)
                        valid = true;
                    else {
                        window.setTimeout(function () { $('span [for=AccountCharged]').text(d.Data[0].SAPErrorCode); }, 40);
                    }
                }
            });

            return valid;
        },

        maybeSetIsInstalledNonVerification: () => {
            const boolValue = ELEMENTS.dateInstalled.val() !== '' &&
               (ELEMENTS.serviceCategory.val() != categories.WaterMeasurementOnly ||
                ELEMENTS.serviceInstallationPurpose.val() != installationPurposes.MaterialVerification);
            ELEMENTS.isInstalledNonVerification.val(boolValue ? 'true' : 'false');
        }
    };

    $(document).ready(s.init);

    return s;
})(jQuery, SERVICE_CATEGORIES, SERVICE_INSTALLATION_PURPOSES);