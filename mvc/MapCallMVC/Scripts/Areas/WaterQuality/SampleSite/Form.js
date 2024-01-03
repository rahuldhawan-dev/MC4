const SampleSites = (function ($) {
    const ELEMENTS = {
        id: $('#Id'),
        agencyId: $('#AgencyId'),
        county: $('#County'),
        coordinate: $('#Coordinate'),
        facility: $('#Facility'),
        facilityShowUrl: $('#FacilityShowUrl'),
        serviceShowUrl: $('#ServiceShowUrl'),
        outOfServiceArea: $('#OutOfServiceArea'),
        status: $('#Status'),
        state: $('#State'),
        street: $('#Street'),
        streetNumber: $('#StreetNumber'),
        town: $('#Town'),
        townSection: $('#TownSection'),
        townText: $('#TownText'),
        zipCode: $('#ZipCode'),
        leadCopperSite: $('#LeadCopperSite'),
        leadAndCopperFields: $('#lead-and-copper-fields'),
        previousMonitoringPeriod: $('#PreviousMonitoringPeriod'),
        isAlternateSite: $('#IsAlternateSite'),
        isBeingValidated: $('#IsBeingValidated'),
        isComplianceSampleSite: $('#IsComplianceSampleSite'),
        resourceDistributionMaps: $('#ResourceDistributionMaps'),
        resourceCapitalImprovement: $('#ResourceCapitalImprovement'),
        resourceUtilityRecords: $('#ResourceUtilityRecords'),
        resourceSamplingResults: $('#ResourceSamplingResults'),
        resourceInterviewsPersonnel: $('#ResourceInterviewsPersonnel'),
        resourceCommunitySurvey: $('#ResourceCommunitySurvey'),
        resourceCountyAppraisal: $('#ResourceCountyAppraisal'),
        resourceContacts: $('#ResourceContacts'),
        resourceSurveyResults: $('#ResourceSurveyResults'),
        resourceInterviewsResidents: $('#ResourceInterviewsResidents'),
        resourceInterviewsContractors: $('#ResourceInterviewsContractors'),
        resourceLeadCheckSwabs: $('#ResourceLeadCheckSwabs'),
        otherSources: $('#OtherSources'),
        leadCopperTierSampleCategory: $('#LeadCopperTierSampleCategory'),
        addressLocationType: $('#SampleSiteAddressLocationType'),
        premise: $('#Premise'),
        premiseNumberDisplay: $('#premise-number-display'),
        getSampleSitesByPremiseNumberUrl: $('#GetSampleSitesByPremiseNumberUrl'),
        premiseSampleSitesGrid: $('#premise-sample-sites-grid'),
        saveButton: $('button[type=submit]'),
        isLimsLocation: $('#IsLimsLocation'),
        limsFields: $('#horizon-lims-fields'),
        limsPrimaryStationCode: $('#LimsPrimaryStationCode'),
        limsSiteId: $('#LimsSiteId'),
        limsFacilityId: $('#LimsFacilityId')
    };

    /**
     * A map of integers that represent the ids of various address location types as defined in SampleSiteAddressLocationType.cs
     */ 
    const addressLocationTypes = {
        facility: 1,
        premise: 2,
        custom: 3
    };

    /**
     * A map of integers that represent the ids of various states as defined in State.cs
     */ 
    const stateIds = {
        nj: 1,
        pa: 3,
        ca: 5
    };

    /**
     * A map of integers that represent the ids of various sample site statuses as defined in SampleSiteStatus.cs 
     */
    const sampleSiteStatuses = {
        active: 1, 
        inactive: 2, 
        pending: 5
    };

    const ss = {
        init: function () {
            ELEMENTS.outOfServiceArea.change(ss.onOutOfServiceAreaChange);
            ELEMENTS.leadCopperSite.change(ss.onLeadCopperSiteChange);
            ELEMENTS.isLimsLocation.change(ss.onIsLimsLocationChange);
            ELEMENTS.isBeingValidated.change(ss.onIsBeingValidatedChange);
            ELEMENTS.facility.change(ss.onFacilityChange);
            ELEMENTS.addressLocationType.change(ss.onAddressLocationChange);
            ELEMENTS.state.change(ss.onStateChange);
            ELEMENTS.isComplianceSampleSite.change(ss.onIsComplianceSampleSiteChange);

            ss.onOutOfServiceAreaChange();
            ss.onLeadCopperSiteChange();
            ss.onIsLimsLocationChange();
            ss.onPremiseChange();
            ss.onAddressLocationChange();
            ss.onStateChange();

            AjaxTable.initialize('#premiseNumberTable');

            if (PremiseFind) {
                PremiseFind.subscribe((selectedPremise) => {
                    /**
                     * todo - implement this further and remove assumptions from that find premise dialog about the DOM of this sample site page
                     */ 
                    ELEMENTS.premiseNumberDisplay.text(selectedPremise.PremiseNumber);
                    ELEMENTS.premise.val(selectedPremise.Id);
                    ss.onPremiseChange();
                });
            }

        },

        onOutOfServiceAreaChange: function () {
            const isOutOfServiceArea = ELEMENTS.outOfServiceArea.is(':checked');

            ELEMENTS.townText.closest('.field-pair').toggle(isOutOfServiceArea);
            ELEMENTS.town.closest('.field-pair').toggle(!isOutOfServiceArea);
            ELEMENTS.street.closest('.field-pair').toggle(!isOutOfServiceArea);
        },

        onAddressLocationChange: function () {
            if (ELEMENTS.addressLocationType.val() != addressLocationTypes.premise) {
                ELEMENTS.premiseSampleSitesGrid.toggle(false);
                ELEMENTS.premise.val(null);
            }
        },

        onPremiseChange: function () {
            const premiseNumber = ELEMENTS.premiseNumberDisplay.text();

            if (!premiseNumber) {
                ELEMENTS.premiseSampleSitesGrid.toggle(false);
                return;
            }

            $.ajax({
                async: false,
                url: ELEMENTS.getSampleSitesByPremiseNumberUrl.val(),
                data: { premiseNumber: premiseNumber },
                success: function (sampleSites) {
                    /* 
                     * If we found sample sites for the given premise number
                     * And if the we ignore the current sample site (cuz you *can* edit a
                     *   sample site with a premise that exists on the *same* sample site...)
                     * And the count of linked sample sites > 0, show the samples sites this 
                     * premise is already assigned to.
                     */
                    if (sampleSites &&
                        sampleSites.filter(({ Id }) => Id != ELEMENTS.id.val()).length > 0) {
                        ELEMENTS.premiseSampleSitesGrid.toggle(true);

                        const itemTemplate = document.getElementById(
                            'premise-sample-sites-grid-item-template'
                        );
                        const tableBody = document.querySelector(
                            '#premise-sample-sites-grid tbody'
                        );

                        while (tableBody.firstChild) {
                            tableBody.removeChild(tableBody.firstChild);
                        }

                        sampleSites.forEach((sampleSite) => {
                            const sampleSiteRow = itemTemplate.content.cloneNode(true);
                            let showLink = sampleSiteRow.querySelector('a');
                            showLink.href = `${showLink.href}/${sampleSite.Id}`;

                            const columns = sampleSiteRow.querySelectorAll('td');
                            columns[1].textContent = sampleSite.CommonSiteName;
                            columns[2].textContent = sampleSite.OperatingCenter;
                            columns[3].textContent = sampleSite.PublicWaterSupply;

                            tableBody.appendChild(sampleSiteRow);
                        });
                    }
                    else {
                        ELEMENTS.premiseSampleSitesGrid.toggle(false);
                    }
                },
            });
        },

        onFacilityChange: function () {
            const selectedFacilityId = ELEMENTS.facility.val();

            if (!selectedFacilityId) {
                return;
            }

            $.ajax({
                url: `${ELEMENTS.facilityShowUrl.val()}/${selectedFacilityId}.json`,
                async: false,
                success: function (facility) {
                    if (facility != null) {
                        ELEMENTS.county.val(facility.County);
                        ELEMENTS.town.val(facility.Town);
                        ELEMENTS.townSection.val(facility.TownSection);
                        ELEMENTS.town.change();
                        ELEMENTS.townSection.change();
                        ELEMENTS.streetNumber.val(facility.StreetNumber);
                        ELEMENTS.coordinate.val(facility.CoordinateId);
                        ELEMENTS.zipCode.val(facility.ZipCode);
                        ELEMENTS.street.val(facility.Street);
                        ELEMENTS.streetNumber.change();
                        ELEMENTS.coordinate.change();
                        ELEMENTS.zipCode.change();
                        ELEMENTS.street.change();
                    }
                },
            });
        },

        onLeadCopperSiteChange: function () {
            const visibilityUiStateToTransitionTo = ELEMENTS.leadCopperSite.val() === 'True';

            ELEMENTS.previousMonitoringPeriod.val('').closest('.field-pair').toggle(visibilityUiStateToTransitionTo)
            ELEMENTS.leadAndCopperFields.toggle(visibilityUiStateToTransitionTo);
        },

        onStateChange: function () {
            ss.toggleLimsFieldsVisibilityAndRequiredUiState();
        },

        onIsComplianceSampleSiteChange: function() {
            ss.toggleLimsFieldsVisibilityAndRequiredUiState();
        },

        onIsLimsLocationChange: function () {
            const visibilityUiStateToTransitionTo = ELEMENTS.isLimsLocation.val() === 'True';
            
            ELEMENTS.limsFields.toggle(visibilityUiStateToTransitionTo);
        },

        onSaveButtonClick: function () {
            if (ELEMENTS.leadCopperSite.val() === 'True' &&
                !SampleSites.isSupplementalValidationValid()) {
                alert('Please select at least one option on the supplemental validation tab.');
                return false;
            }

            return true;
        },

        onIsBeingValidatedChange: function () {
            if (ELEMENTS.isBeingValidated.prop('checked')) {
                ELEMENTS.isBeingValidated.prop('checked', confirm('Are you sure you want to validate this Sample Site?'));
            }
        },

        isSupplementalValidationValid: function () {
            return (
                ELEMENTS.resourceDistributionMaps.val() === 'True' ||
                ELEMENTS.resourceCapitalImprovement.val() === 'True' ||
                ELEMENTS.resourceUtilityRecords.val() === 'True' ||
                ELEMENTS.resourceSamplingResults.val() === 'True' ||
                ELEMENTS.resourceInterviewsPersonnel.val() === 'True' ||
                ELEMENTS.resourceCommunitySurvey.val() === 'True' ||
                ELEMENTS.resourceCountyAppraisal.val() === 'True' ||
                ELEMENTS.resourceContacts.val() === 'True' ||
                ELEMENTS.resourceSurveyResults.val() === 'True' ||
                ELEMENTS.resourceInterviewsResidents.val() === 'True' ||
                ELEMENTS.resourceInterviewsContractors.val() === 'True' ||
                ELEMENTS.resourceLeadCheckSwabs.val() === 'True' ||
                ELEMENTS.otherSources.val() === 'True'
            );
        },

        validateLeadCopperTierSampleCategory: function () {
            if (ELEMENTS.leadCopperSite.val() === 'True' &&
                ELEMENTS.leadCopperTierSampleCategory.val() === '' &&
                ELEMENTS.state.val() == stateIds.nj) {
                return false;
            }
            return true;
        },

        validateAgencyId: function () {
            return (
                ELEMENTS.status.val() == sampleSiteStatuses.pending ||
                ELEMENTS.isComplianceSampleSite.val() !== 'True' ||
                ELEMENTS.agencyId.val() !== ''
            );
        },

        /**
         * Required when is compliance sample site yes, is lims location yes and state is not equal to CA, PA
         */
        validateLimsFacilityId: () => {
            return ELEMENTS.isComplianceSampleSite.val() === 'True' &&
                ELEMENTS.isLimsLocation.val() === 'True' &&
                ![
                    stateIds.ca.toString(),
                    stateIds.pa.toString()
                ].includes(ELEMENTS.state.val())
                ? ELEMENTS.limsFacilityId.val() !== ''
                : true;
        },

        /**
         * Required when is compliance sample site yes, is lims location yes and state is not equal to CA
         */
        validateLimsSiteId: () => {
            return ELEMENTS.isComplianceSampleSite.val() === 'True' &&
                ELEMENTS.isLimsLocation.val() === 'True' && 
                ELEMENTS.state.val() != stateIds.ca
                ? ELEMENTS.limsSiteId.val() !== ''
                : true;
        },

        /**
         * Required when is compliance sample site yes, is lims location yes and state is CA
         */
        validateLimsPrimaryStationCode: () => {
            return ELEMENTS.isComplianceSampleSite.val() === 'True' &&
                ELEMENTS.isLimsLocation.val() === 'True' &&
                ELEMENTS.state.val() == stateIds.ca
                ? ELEMENTS.limsPrimaryStationCode.val() !== ''
                : true;
        },

        toggleLimsFieldsVisibilityAndRequiredUiState: function () {
            const stateId = ELEMENTS.state.val();
            var isComplaint = ELEMENTS.isComplianceSampleSite.val() === 'True';

            /**
             * Required and visible when:
             * 
             * LimsPrimaryCode      - state is CA & is complaint sample site
             * LimsSiteId           - state is not equal to CA & is complaint sample site
             * LimsFacilityId       - state is not equal to CA, PA & is complaint sample site
             */

            switch (parseInt(stateId, 10)) {
            case stateIds.ca:
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsPrimaryStationCode, true, isComplaint);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsFacilityId, false, false);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsSiteId, false, false);
                break;

            case stateIds.pa:
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsPrimaryStationCode, false, false);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsFacilityId, false, false);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsSiteId, true, isComplaint);
                break;

            default:
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsPrimaryStationCode, false, false);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsFacilityId, true, isComplaint);
                ss.toggleFieldAndRequiredUiState(ELEMENTS.limsSiteId, true, isComplaint);
                break;
            }
        },

        /**
         * Toggles the visibility *and* required ui state of the target's field and label
         * 
         * @param {jQuery} $target - The target whose visual state will be toggled
         * @param {boolean} [isVisible=true] - True if the target's field and label should be visible, else false. Default: true
         * @param {boolean} [isRequired=true] - True if the target's field and label's ui state should be required, else false. Default: true
         * @returns {jQuery} - The original target to support chaining
         * 
         */
        toggleFieldAndRequiredUiState: ($target, isVisible = true, isRequired = true) => {
            Application.toggleField($target, isVisible);
            Application.toggleFieldRequiredUiState($target, isRequired);
            return $target;
        }
    };

    $(document).ready(ss.init);

    return ss;
})(jQuery);
