var EquipmentForm = (function ($) {

    let redTagPermitPrerequisiteCheckbox = null;
    let redTagPermitEligibleEquipmentPurposeIds = null;

    const setRedTagPermitUiCheckedState = (equipmentPurposeId) => {
        redTagPermitPrerequisiteCheckbox.checked = redTagPermitEligibleEquipmentPurposeIds.some(id => id === equipmentPurposeId);
    };

    const setRedTagPermitUiReadOnlyState = (isFormValid) => {
        redTagPermitPrerequisiteCheckbox.enabled = isFormValid;
    };

    const init = () => {
        const $form = $('form');
        const redTagPermitProductionPrerequisiteId = 6;

        redTagPermitPrerequisiteCheckbox = document.querySelector(`#Prerequisites mc-checkboxlistitem[value='${redTagPermitProductionPrerequisiteId}']`);
        redTagPermitPrerequisiteCheckbox.enabled = false;

        redTagPermitEligibleEquipmentPurposeIds = document.getElementById('EquipmentTypeIdsWithRedTagPermitEligibility').value.split(',');

        document
            .getElementById('EquipmentType')
            .addEventListener('change', (event) => {
                setRedTagPermitUiCheckedState(event.target.value);
            });

        document
            .querySelector('button[type="submit"]')
            .addEventListener('click', () => {
                setRedTagPermitUiReadOnlyState($form.valid());
            });
    };

    const validateFunctionalLocation = (sapFunctionalLocationIdValue, element) => {
        // Cut out early because OperatingCenter is a required field and we don't wanna throw errors
        // when they haven't selected an operating center yet.
        var opc = $('#OperatingCenter').val();
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
            success: function(result) {
                isValid = !result.IsSAPEnabled;
            },
            error: function() {
                alert("Something went wrong when validating the Functional Location.");
            }
        });
        return isValid; 
    };

    window.addEventListener('DOMContentLoaded', init);

    return { validateFunctionalLocation };

})(jQuery);