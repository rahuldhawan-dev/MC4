const SafetyBrief = (function ($) {
    return {
        validatePpeChecked: () => {
            // We only care if at least one checkbox is checked.
            return $('#ppe-fieldset input[type=checkbox]').is(':checked');
        },

        validateHaveAllHazardsAndPrecautionsBeenReviewedIsTrue: () => {
            return $('#HaveAllHazardsAndPrecautionsBeenReviewed').val() === 'True';
        }
    };
})(jQuery);