const SafetyBrief = (function ($) {
    let ELEMENTS = {};
    const safetyBrief = {
        intialize: function() {
          ELEMENTS = {
                workingWithACPipe: $('#WorkingWithACPipe'),
                crewMembersTrainedInACPipe: $('#CrewMembersTrainedInACPipe'),
                involveConfinedSpace: $('#InvolveConfinedSpace'),
                isTheExcavationGuardedFromAccidentalEntry: $('#IsTheExcavationGuardedFromAccidentalEntry')
          };

            ELEMENTS.crewMembersTrainedInACPipe.on('change', safetyBrief.acPipeAlert);
            ELEMENTS.involveConfinedSpace.on('change', safetyBrief.involveConfinedSpaceChange);
            ELEMENTS.isTheExcavationGuardedFromAccidentalEntry.on('change', safetyBrief.isTheExcavationGuardedFromAccidentalEntryChange);
        },

        acPipeAlert: () => {
          if ($(ELEMENTS.workingWithACPipe).is(':checked') && !Application.getBooleanValue(ELEMENTS.crewMembersTrainedInACPipe)) {
                alert("Please contact supervisor");
            }
        },

        involveConfinedSpaceChange: () => {
          if (Application.getBooleanValue(ELEMENTS.involveConfinedSpace)) {
            alert("The MapCall Confined Space Form must be completed and all employees involved in the confined space entry must be trained and experienced in identifying and evaluating existing and potential hazards within the confined space.")
          }
        },

        isTheExcavationGuardedFromAccidentalEntryChange: () => {
            if ( !Application.getBooleanValue(ELEMENTS.isTheExcavationGuardedFromAccidentalEntry)) {         
                ELEMENTS.isTheExcavationGuardedFromAccidentalEntry.val('0');
                alert("Stop Work and barricade/guard the excavation")
            }
        },

        validatePpeChecked: () => {
        const headProtection = $('#HeadProtection').is(':checked');
        const handProtection = $('#HandProtection').is(':checked');
        const electricalProtection = $('#ElectricalProtection').is(':checked');
        const footProtection = $('#FootProtection').is(':checked');
        const eyeProtection = $('#EyeProtection').is(':checked');
        const faceShield = $('#FaceShield').is(':checked');
        const safetyGarment = $('#SafetyGarment').is(':checked');
        const hearingProtection = $('#HearingProtection').is(':checked');
        const respiratoryProtection = $('#RespiratoryProtection').is(':checked');
        const ppeOther = $('#PPEOther').is(':checked');

        return headProtection || handProtection || electricalProtection || footProtection || eyeProtection || faceShield || safetyGarment || hearingProtection || respiratoryProtection || ppeOther

      }


};

    safetyBrief.ELEMENTS = ELEMENTS;
    $(document).ready(safetyBrief.intialize);
    return safetyBrief;
})(jQuery);