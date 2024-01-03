var ApcInspectionItem = (function ($) {

    var apcInspectionItem = {
        init: function () {
            apcInspectionItem.score = 0;
            apcInspectionItem.totalmaxscore = 0;
            ELEMENTS = {
                isSafeCheckboxes: $('table tr td:nth-child(3) select'),
                score: $('input#Score'),
                percentage: $('input#Percentage'),
                facilityInspectionRatingType: $('select#FacilityInspectionRatingType'),
                generalWorkAreaConditionSectionTable: $("table#GeneralWorkAreaConditions"),
                isGeneralWorkAreaConditionsSafeCheckboxes: $('table#GeneralWorkAreaConditions tr td:nth-child(3) select'),
                emergencyResponseFirstAidSectionTable: $("table#EmergencyResponseFirstAid"),
                isEmergencyResponseFirstAidSafeCheckboxes: $('table#EmergencyResponseFirstAid tr td:nth-child(3) select'),
                securitySectionTable: $("table#Security"),
                isSecuritySafeCheckboxes: $('table#Security tr td:nth-child(3) select'),
                fireSafetySectionTable: $("table#FireSafety"),
                isFireSafetySafeCheckboxes: $('table#FireSafety tr td:nth-child(3) select'),
                personalProtectiveEquipmentSectionTable: $("table#PersonalProtectiveEquipment"),
                isPersonalProtectiveEquipmentSafeCheckboxes: $('table#PersonalProtectiveEquipment tr td:nth-child(3) select'),
                chemicalStorageHazComSectionTable: $("table#ChemicalStorageHazCom"),
                isChemicalStorageHazComSafeCheckboxes: $('table#ChemicalStorageHazCom tr td:nth-child(3) select'),
                equipmentToolsSectionTable: $("table#EquipmentTools"),
                isEquipmentToolsSafeCheckboxes: $('table#EquipmentTools tr td:nth-child(3) select'),
                confinedSpaceSectionTable: $("table#ConfinedSpace"),
                isConfinedSpaceSafeCheckboxes: $('table#ConfinedSpace tr td:nth-child(3) select'),
                vehicleMotorizedEquipmentSectionTable: $("table#VehicleMotorizedEquipment"),
                isVehicleMotorizedEquipmentSafeCheckboxes: $('table#VehicleMotorizedEquipment tr td:nth-child(3) select'),
                oshaTrainingSectionTable: $("table#OshaTraining"),
                isOshaTrainingSafeCheckboxes: $('table#OshaTraining tr td:nth-child(3) select')
            };
            apcInspectionItem.calculateTotalPossibleScore();
            apcInspectionItem.initIsSafeCheckboxes();
            apcInspectionItem.calculateScore();
            apcInspectionItem.initNotApplicableSection();
            apcInspectionItem.initNonEditableSection();
        },

        initNotApplicableSection: function() {
            $('.checkNotApplicableSection').on('change', apcInspectionItem.onNotApplicableSectionChange);
        },

        onNotApplicableSectionChange() {
            if ($(this).is(':checked')) {
                $(this).nextAll('table').find('select option[value=""]').prop('selected', true);
                apcInspectionItem.calculateTotalPossibleScore();
                apcInspectionItem.calculateScore();
                $(this).nextAll('table').hide();
            }
            else {
                $(this).nextAll('table').show();
            }
        },

        initNonEditableSection: function () {
            ELEMENTS.score.attr("onfocus","blur()");
            ELEMENTS.percentage.attr("onfocus","blur()");
        },

        initIsSafeCheckboxes() {
            ELEMENTS.isSafeCheckboxes.each((i, isSafe) => {
                $('#' + isSafe.id).on('change',
                    function () {
                        apcInspectionItem.calculateTotalPossibleScore();
                        apcInspectionItem.calculateScore();
                    });
            });
        },

        calculateTotalPossibleScore() {
            apcInspectionItem.totalmaxscore = 0;
            ELEMENTS.isSafeCheckboxes.each((i, isSafe) => {
                const isSafeElement = $('#' + isSafe.id);
                if (isSafeElement.val() === 'True' || isSafeElement.val() === 'False')
                this.totalmaxscore = this.totalmaxscore + (+isSafeElement.attr('data-weightage'));
               });
            },
        calculateScore() {
            apcInspectionItem.score = 0;
            let percentageScore;
            ELEMENTS.isSafeCheckboxes.each((i, isSafe) => {
                const isSafeElement = $('#' + isSafe.id);
                if (isSafeElement.val() === 'True') {
                    this.score = this.score + (+isSafeElement.attr('data-weightage'));
                }
            });
            ELEMENTS.score.val(this.score);
            if (this.totalmaxscore !== 0) {
                percentageScore = (100 * this.score / this.totalmaxscore).toFixed(0);
                ELEMENTS.percentage.val((100 * this.score / this.totalmaxscore).toFixed(0) + '%');
            }
            else
            {
                percentageScore = 0;
                ELEMENTS.percentage.val('0%');
            }
            if (percentageScore >= 80) {
                ELEMENTS.facilityInspectionRatingType.val('1');
            } else {
                ELEMENTS.facilityInspectionRatingType.val('2');
            }
        }
    }

    $(document).ready(apcInspectionItem.init);
    return apcInspectionItem;
})(jQuery);