/**
 * This module contains functionality that host pages can leverage a Find Premise dialog to allow users
 * to search for and assign Premise related information. To use this properly:
 *
 * 1. Add a link on your host page that when clicked pops up the areas/customer/premise/find dialog - potentially like this:
 *    - @Ajax.ActionLink("Click here to lookup Premise Information", "Find", "Premise", new { area = "Customer", id = Model.PremiseNumber }, new AjaxOptions { HttpMethod = "GET" }, new { data_ajax_table = "#premiseNumberTable", id = "PremiseFindLink" })
 *
 * 2. Make sure the host of that link includes this script file (inside the Razor "section scripts") after the host page's Form.js (or New.js/Edit.js/etc):
 *    - @Html.ScriptFor("~/Scripts/Areas/Customer/Premise/Find.js")
 *
 * 3. Make sure the host of that link, on page load, does this:
 *    - AjaxTable.initialize('#premiseNumberTable'); (this id selector should match whatever you use in step 1 for the data_ajax_table property )
 *
 * This module makes some assumptions about it's host, specifically - the ids of it's host elements. As such
 * it will only pre select the values of the host's controls with the values of a searched premise if it can find a match based on
 * the control's id attribute. For example, suppose there are 3 host views in all of the land - and each host's "city" control has an id of "city" -
 * great. Now suppose someone creates a new host view and uses a control for "city" but with an id of "city_region", then add "city_region" to the code
 * below (using comma separated id selector) where applicable to ensure all host's control values will be set when a premise is selected.
 */
const PremiseFind = (function ($) {
    let ELEMENTS;

    var dialogFormSelector = '#CustomerPremiseFind';
    const premiseSearchUrl = '../../Customer/Premise/Index.json';


    /**
     * A collection of callbacks to be invoked when a premise is selected.
     */
    const subscribers = [];

    /**
     * The currently selected premise
     */
    let selectedPremise = null;

    var pf = {
        cacheJqueryReferences: () => {
            ELEMENTS = {

                /* dialog owned elements */
                searchButton: $(dialogFormSelector + ' button[id=DoPremiseFindSearch]'),
                cancelButton: $(dialogFormSelector + ' button.cancel'),
                selectPremiseButton: $('#selectPremise'),
                findPremiseDialog: $(dialogFormSelector),
                resultsCount: $(dialogFormSelector + ' div[id=ResultsCount]'),
                premiseResults: $('table[id=premiseResults]'),
                maxResultsLabel: $(dialogFormSelector + ' div[id=maxResults]'),
                activeSearchResultCells: null,

                /* dialog owned search interactivity elements */
                meterLocationFreeTextSearch: $(dialogFormSelector + ' input[id=MeterLocationFreeText_Value]'),
                meterLocationFreeTextMatchType: $(dialogFormSelector + ' select[id=MeterLocationFreeText_MatchType]'),
                meterSerialNumberSearch: $(dialogFormSelector + ' input[id=MeterSerialNumber_Value]'),
                meterSerialNumberSearchMatchType: $(dialogFormSelector + ' select[id=MeterSerialNumber_MatchType]'),
                installationTypeSearch: $(dialogFormSelector + ' select[id=ServiceUtilityType]'),
                streetSearch: $(dialogFormSelector + ' input[id=ServiceAddressStreet_Value]'),
                streetSearchMatchType: $(dialogFormSelector + ' select[id=ServiceAddressStreet_MatchType]'),
                streetNumberSearch: $(dialogFormSelector + ' input[id=ServiceAddressHouseNumber_Value]'),
                streetNumberSearchMatchType: $(dialogFormSelector + ' select[id=ServiceAddressHouseNumber_MatchType]'),
                operatingCenterSearch: $(dialogFormSelector + ' select[id=OperatingCenter]'),
                premiseNumberSearch: $(dialogFormSelector + ' input[id=PremiseNumber_Value]'),
                premiseNumberSearchMatchType: $(dialogFormSelector + ' select[id=PremiseNumber_MatchType]'),
                regionCodeSearch: $(dialogFormSelector + ' select[id=RegionCode]'),
                
                /* dialog owned hidden elements to support transmission to host controls */
                selectedPremiseAddress: $(dialogFormSelector + ' input[id=SelectedPremiseAddress]'),
                selectedPremiseApartment: $(dialogFormSelector + ' input[id=SelectedPremiseApartment]'),
                selectedCoordinateId: $(dialogFormSelector + ' input[id=SelectedCoordinateId]'),
                selectedCity: $(dialogFormSelector + ' input[id=SelectedCity]'),
                selectedRegionCode: $(dialogFormSelector + ' input[id=SelectedRegionCode]'),
                selectedDeviceLocation: $(dialogFormSelector + ' input[id=SelectedDeviceLocation]'),
                selectedMeterSerialNumber: $(dialogFormSelector + ' input[id=SelectedMeterSerialNumber]'),
                selectedEquipment: $(dialogFormSelector + ' input[id=SelectedEquipment]'),
                selectedInstallation: $(dialogFormSelector + ' input[id=SelectedInstallation]'),
                selectedPremise: $(dialogFormSelector + ' input[id=SelectedPremise]'),
                selectedPremiseNumber: $(dialogFormSelector + ' input[id=SelectedPremiseNumber]'),
                selectedServiceUtilityType: $(dialogFormSelector + ' input[id=SelectedServiceUtilityType]'),
                selectedStreet: $(dialogFormSelector + ' input[id=SelectedStreet]'),
                selectedStreetNumber: $(dialogFormSelector + ' input[id=SelectedStreetNumber]'),
                selectedZipCode: $(dialogFormSelector + ' input[id=SelectedZipCode]'),

                /* host controls */
                hostAssetType: $('#AssetType'),
                hostCoordinateId: $('#CoordinateId, #Coordinate'),
                hostCoordinateIcon: $('img.coordinate-picker-icon'),
                hostDeviceLocation: $('#DeviceLocation'),
                hostEquipment: $('#SAPEquipmentNumber'),
                hostInstallation: $('#Installation'),
                hostMeterSerialNumber: $('#MeterSerialNumber'),
                hostOperatingCenter: $('#OperatingCenter'),
                hostPremiseAddress: $('#PremiseAddress'),
                hostPremiseNumber: $('#PremiseNumber'),
                hostServiceUtilityType: $('#ServiceUtilityType'),
                hostApartmentAdditional: $('#ApartmentAddtl'),
                hostStreet: $('#Street'),
                hostStreetNumber: $('#StreetNumber'),
                hostTown: $('#Town'),
                hostZipCode: $('#ZipCode')
            };
        },

        /**
         * attaches event handlers, including listening for events on elements which are not yet in the dom
         */
        attachEventHandlers: () => {
            ELEMENTS.searchButton.on('click', pf.handleSearchButtonClick);
            ELEMENTS.selectPremiseButton.on('click', pf.handleSelectButtonClick);
            ELEMENTS.findPremiseDialog.on('click', 'table[id=premiseResults] > tbody > tr', pf.handleResultRowClick);
            ELEMENTS.findPremiseDialog.on('dblclick', 'table[id=premiseResults] > tbody > tr', pf.handleResultRowDoubleClick);
        },

        /**
         * handles the event when the search button is clicked -> makes a request for searching premises
         */
        handleSearchButtonClick: () => {
            $.getJSON(
                (window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\/|\/NEWFROMWORKORDER\//)) ? `../${premiseSearchUrl}` : premiseSearchUrl,
                $.param({
                    'OperatingCenter': ELEMENTS.operatingCenterSearch.val(),
                    'PremiseNumber.Value': ELEMENTS.premiseNumberSearch.val(),
                    'PremiseNumber.MatchType': ELEMENTS.premiseNumberSearchMatchType.val(),
                    'ServiceAddressStreet.Value': ELEMENTS.streetSearch.val(),
                    'ServiceAddressStreet.MatchType': ELEMENTS.streetSearchMatchType.val(),
                    'ServiceAddressHouseNumber.Value': ELEMENTS.streetNumberSearch.val(),
                    'ServiceAddressHouseNumber.MatchType': ELEMENTS.streetNumberSearchMatchType.val(),
                    'MeterSerialNumber.Value': ELEMENTS.meterSerialNumberSearch.val(),
                    'MeterSerialNumber.MatchType': ELEMENTS.meterSerialNumberSearchMatchType.val(),
                    'MeterLocationFreeText.Value': ELEMENTS.meterLocationFreeTextSearch.val(),
                    'MeterLocationFreeText.MatchType': ELEMENTS.meterLocationFreeTextMatchType.val(),
                    'ServiceUtilityType': (ELEMENTS.installationTypeSearch.val() !== null) ? ELEMENTS.installationTypeSearch.val().join(',') : '',
                    'RegionCode': (ELEMENTS.regionCodeSearch.val() !== null) ? ELEMENTS.regionCodeSearch.val().join(',') : ''
                }),
                pf.processSearchResponse);
            return false;
        },

        /**
         * handles the event when a user clicks one of the search result table rows
         */
        handleResultRowClick: function () {
            const $source = $(this);

            selectedPremise = $source.data('premise');
            pf.setSelectedPremiseMetaFromSearchResultRow($source);
        },

        /**
         * handles the event when a user double clicks one of the search result table rows
         */
        handleResultRowDoubleClick: function () {
            const $source = $(this);

            selectedPremise = $source.data('premise');
            pf.setSelectedPremiseMetaFromSearchResultRow($source);
            pf.setHostStateAndTriggerCompletion();
        },

        /**
         * handles the event when a user clicks the select button
         */
        handleSelectButtonClick: function () {
            pf.setHostStateAndTriggerCompletion();

            subscribers.forEach(onPremiseSelected => onPremiseSelected(selectedPremise));
        },

        /**
         * sets premise information from the given jQuery reference to a search result table row
         * onto the various hidden elements. Also sets the UI state of the table rows for easier clarification
         * of what is currently selected.
         * @param {jQuery} $row a search result table row containing premise related information from a recent search
         */
        setSelectedPremiseMetaFromSearchResultRow: ($row) => {
            const row = $row[0];

            if (ELEMENTS.activeSearchResultCells !== null) {
                ELEMENTS.activeSearchResultCells.css('background-color', '');
            } 
            
            ELEMENTS.activeSearchResultCells = $row.find('td');
            ELEMENTS.activeSearchResultCells.css('background-color', 'orange');

            ELEMENTS.selectedPremise.val($row.attr('premise'));
            ELEMENTS.selectedPremiseNumber.val(row.cells[0].innerHTML);
            ELEMENTS.selectedPremiseAddress.val(row.cells[1].innerHTML);
            ELEMENTS.selectedPremiseApartment.val(row.cells[2].innerHTML);
            ELEMENTS.selectedDeviceLocation.val(row.cells[4].innerHTML);
            ELEMENTS.selectedEquipment.val(row.cells[5].innerHTML);
            ELEMENTS.selectedMeterSerialNumber.val(row.cells[7].innerHTML);
            ELEMENTS.selectedInstallation.val($row.attr('installation'));
            ELEMENTS.selectedZipCode.val($row.attr('zipCode'));
            ELEMENTS.selectedCoordinateId.val($row.attr('coordinateId'));
            ELEMENTS.selectedStreet.val($row.attr('street'));
            ELEMENTS.selectedStreetNumber.val($row.attr('streetNumber'));
            ELEMENTS.selectedServiceUtilityType.val(row.cells[8].innerHTML);
            ELEMENTS.selectedCity.val($row.attr('town'));
            ELEMENTS.selectedRegionCode.val($row.attr('regionCode'));
        },

        /**
         * processes the given response; Sets the UI state of the premise search result table; binding
         * the response data to a table, creating a table row per response premise
         * @param {any} response an ajax response from making a search for premises
         */
        processSearchResponse: (response) => {
            const $tableBody = ELEMENTS.premiseResults
                .find('tbody')
                .empty();

            const premiseCount = response.Data.length;

            response.Data.forEach(premise => {
                const $row = $(`<tr 
                        coordinateId="${premise.CoordinateId}" 
                        streetNumber="${premise.FullStreetNumber}" 
                        installation="${premise.Installation}" 
                        zipCode="${premise.ZipCode}" 
                        town="${premise.ServiceCity}" 
                        regionCode="${premise.RegionCode}" 
                        street="${premise.Street}"
                        premise="${premise.Id}"
                    >
                        <td>${premise.PremiseNumber}</td>
                        <td>${premise.FullStreetAddress}</td>
                        <td>${premise.ApartmentNumber}</td>
                        <td>${premise.ServiceCity}</td>
                        <td>${premise.DeviceLocation}</td>
                        <td>${premise.Equipment}</td>
                        <td>${premise.DeviceSerialNumber}</td>
                        <td>${premise.MeterSerialNumber}</td>
                        <td>${premise.InstallationType}</td>
                        <td>${premise.Installation}</td>
                        <td>${premise.MeterLocationFreeText}</td>
                    </tr>`);

                $row.data('premise', premise);
                $tableBody.append($row);
            });

            ELEMENTS.resultsCount.text(`${premiseCount} Result(s)`);

            if (premiseCount === 1) {
                selectedPremise = response.Data[0];

                ELEMENTS.selectedPremise.val(selectedPremise.Id);
                ELEMENTS.selectedPremiseNumber.val(selectedPremise.PremiseNumber);
                ELEMENTS.selectedPremiseAddress.val(selectedPremise.FullStreetAddress);
                ELEMENTS.selectedPremiseApartment.val(selectedPremise.ApartmentNumber);
                ELEMENTS.selectedDeviceLocation.val(selectedPremise.DeviceLocation);
                ELEMENTS.selectedEquipment.val(selectedPremise.Equipment);
                ELEMENTS.selectedMeterSerialNumber.val(selectedPremise.MeterSerialNumber);
                ELEMENTS.selectedInstallation.val(selectedPremise.Installation);
                ELEMENTS.selectedZipCode.val(selectedPremise.ZipCode);
                ELEMENTS.selectedCoordinateId.val(selectedPremise.CoordinateId);
                ELEMENTS.selectedCity.val(selectedPremise.ServiceCity);
                ELEMENTS.selectedRegionCode.val(selectedPremise.RegionCode);
                ELEMENTS.selectedStreet.val(selectedPremise.ServiceAddressStreet);
            }

            if (premiseCount >= 100) {
                ELEMENTS.maxResultsLabel.text('MAXIMUM RESULTS REACHED. You have returned the maximum number of results. Please refine your search.');
            } else {
                ELEMENTS.maxResultsLabel.text('');
            }
        },

        /**
         * if a premise has been selected by the user, sets the state of the host with the selected premise information and closes the dialog,
         * else closes dialog.
         */
        setHostStateAndTriggerCompletion: () => {
            if (ELEMENTS.selectedPremiseNumber.val() !== '') {
                pf.setHostUiControlValue(ELEMENTS.hostPremiseNumber, ELEMENTS.selectedPremiseNumber.val());
                pf.setHostUiControlValue(ELEMENTS.hostPremiseAddress, ELEMENTS.selectedPremiseAddress.val());
                pf.setHostUiControlValue(ELEMENTS.hostApartmentAdditional, ELEMENTS.selectedPremiseApartment.val());
                pf.setHostUiControlValue(ELEMENTS.hostMeterSerialNumber, ELEMENTS.selectedMeterSerialNumber.val());
                pf.setHostUiControlValue(ELEMENTS.hostZipCode, ELEMENTS.selectedZipCode.val());
                pf.setHostCoordinateUiControl(ELEMENTS.selectedCoordinateId.val());
                pf.setHostUiControlValue(ELEMENTS.hostStreetNumber, ELEMENTS.selectedStreetNumber.val().trim());
                
                const townChanged = pf.trySetHostUnselectedDropdown(
                    ELEMENTS.hostTown,
                    ELEMENTS.selectedCity.val(),
                    ELEMENTS.selectedRegionCode.val());

                if (townChanged) {
                    ELEMENTS.hostTown.change();
                }
                
                pf.trySetHostUnselectedDropdown(ELEMENTS.hostStreet, ELEMENTS.selectedStreet.val());
                pf.setHostUiControlValue(ELEMENTS.hostServiceUtilityType, ELEMENTS.selectedServiceUtilityType.val());
                pf.setHostUiControlValue(ELEMENTS.hostDeviceLocation, ELEMENTS.selectedDeviceLocation.val());
                pf.setHostUiControlValue(ELEMENTS.hostEquipment, ELEMENTS.selectedEquipment.val());
                pf.setHostUiControlValue(ELEMENTS.hostInstallation, ELEMENTS.selectedInstallation.val());
                ELEMENTS.hostInstallation.change();
            }

            ELEMENTS.cancelButton.click();
            ELEMENTS.hostPremiseNumber.change();
        },

        /**
         * Sets the host's target control with the given value then blurs the controls
         * @param {any} $target The host's target control which should be set
         * @param {any} value The value that should be set on the control
         */
        setHostUiControlValue: function ($target, value) {
            $target.val(value);
            $target.blur();
        },

        /**
         * Sets the host's target coordinate control with the given coordinate id and updates it's visibility state
         * to reflect if it's selected or not
         * @param {any} coordinateId The coordinate of the id to set
         */
        setHostCoordinateUiControl: function (coordinateId) {
            ELEMENTS.hostCoordinateId.val(coordinateId);
            const $icon = ELEMENTS.hostCoordinateIcon;

            if (!!coordinateId) {
                $icon.attr('coordinateurl', $icon.attr('coordinateurl') + '&id=' + coordinateId);
                $icon.attr('src', $icon.attr('src').replace('red', 'blue'));
            } else {
                $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, ''));
                $icon.attr('src', $icon.attr('src').replace('blue', 'red'));
            }
        },

        /**
         * Attempts to set the state of the FindPremise dialog from the host page.
         */
        setStateFromHost: function () {
            ELEMENTS.operatingCenterSearch.val(ELEMENTS.hostOperatingCenter.val());
            ELEMENTS.premiseNumberSearch.val(ELEMENTS.hostPremiseNumber.val());

            if (ELEMENTS.hostPremiseNumber.val() !== '') {
                ELEMENTS.selectedPremiseNumber.val(ELEMENTS.hostPremiseNumber.val());
            }

            if (ELEMENTS.hostDeviceLocation.val() !== '') {
                ELEMENTS.selectedDeviceLocation.val(ELEMENTS.hostDeviceLocation.val());
            }

            if (ELEMENTS.hostEquipment.val() !== '') {
                ELEMENTS.selectedEquipment.val(ELEMENTS.hostEquipment.val());
            }

            if (ELEMENTS.hostMeterSerialNumber.val() !== '') {
                ELEMENTS.meterSerialNumberSearch.val(ELEMENTS.hostMeterSerialNumber.val());
                ELEMENTS.selectedMeterSerialNumber.val(ELEMENTS.hostMeterSerialNumber.val());
            }

            if (ELEMENTS.hostStreetNumber.val() !== '') {
                ELEMENTS.streetNumberSearch.val(ELEMENTS.hostStreetNumber.val());
            }

            if (ELEMENTS.hostStreet.val() !== '') {
              ELEMENTS.streetSearch.val(ELEMENTS.hostStreet.children('option').filter(':selected').text());
            }

            // if it's a sewer service
            if (ELEMENTS.hostAssetType.val() === '6') {
                ELEMENTS.installationTypeSearch.find('option').prop('selected', false);
                ELEMENTS.installationTypeSearch
                    .find('option')
                    .filter(function () { return (new RegExp('Waste', 'i')).test($(this).text()); })
                    .prop('selected', true);
            } 
            else {
                ELEMENTS.installationTypeSearch.find('option').prop('selected', true);
                ELEMENTS.installationTypeSearch.find('option')
                    .filter(function () { return (new RegExp('Waste', 'i')).test($(this).text()); })
                    .prop('selected', false);
            }

            if (ELEMENTS.hostTown.val() !== '') {
                // this can't happen right away because ajax
                window.setTimeout(pf.trySetRegionCodeTown, 50);
            }
        },

        /**
         * If target does not have any selected options, attempts to find and select the first matching
         * dropdown option by text value given the arguments - else exits early
         * @param {jQuery} $target The target dropdown to attempt to set values for
         * @param {Array<string>} one or more text values to find in the target's options
         * @return {boolean} true if the dropdown selected option was changed, else false
         */
        trySetHostUnselectedDropdown($target, ...args) {
            
            if (args.length === 0) {
                console.warn('find premise> trySetHostUnselectedDropdown: no arguments were passed for matching, exiting early');
                return false;
            }            

            /**
             * If the underlying dropdown is already selected, exit early, we don't want to select it with a different value
             * and confuse the users (this is happening prior to MC-4752) 
             */
            if ($target.prop('selectedIndex') > 0) {
                console.debug('find premise > trySetHostUnselectedDropdown: underlying dropdown is already selected, lets bounce');
                return false;
            }

            let dropdownOptionWasChanged = false;

            args.some(textToMatch => {
                console.debug(`Finding option in element with id: ${$target.attr('id')} with matching text: ${textToMatch}`);

                // adding word boundary for value to match the exact word.  \b allows you to perform a “whole words only” search using a regular expression in the form of \bword\b
                const $matchedOption = $target
                    .find('option')
                    .filter(function() { return (new RegExp(`\\b${textToMatch}\\b`, 'i')).test($(this).text()); });

                if ($matchedOption.length !== 0) {
                    $matchedOption.prop('selected', true);
                    dropdownOptionWasChanged = true;
                    return true;
                }

                return false;
            });

            return dropdownOptionWasChanged;
        },

        /**
         * Attempts to set the region code / town of the dialog from the host.
         */
        trySetRegionCodeTown: function () {
            ELEMENTS.regionCodeSearch.find('option').prop('selected', false);
            var selectedTown = hostTown.find('option:selected').text().toLowerCase();
            var options = $('#CustomerPremiseFind select[id=RegionCode]')[0].options;
            // Failed to get this to work. scope issue i think // $('#RegionCode').find('option').filter(function () { return (new RegExp('CAMDEN',     'i')).test($(this).text()); }).prop('selected', true)
            for (var i = 0; i < options.length; i++) {
                if (options[i].text.toLowerCase() === selectedTown) {
                    options[i].selected = true;
                }
            }
        }

    };

    /**
     * Let's just reveal an initialize method, no need for anything else to be spelunking down into this object
     */
    return {
        /**
         * Performs the necessary operations to initialize the Find Premise dialog such that it
         * can allow users to search for and select Premises to populate UI elements on the host.
         */
        initialize: () => {
            selectedPremise = null;
            pf.cacheJqueryReferences();
            pf.attachEventHandlers();
            pf.setStateFromHost();
        },

        /**
         * Allows an interested consumer to provide a callback which will accept as it's only parameter the premise when it is selected. This
         * will allow us to let consumers control their state instead of this dialog making a lot of assumptions about who is calling it and how
         * to set it's caller's state.
         *
         * @param {Function} onPremiseSelected A function that will be called when a premise is selected. The premise will be passed as the only argument when invoked.
         */
        subscribe: (onPremiseSelected) => {
            subscribers.push(onPremiseSelected);
        }
    };

})(jQuery);