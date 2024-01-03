(($) => {
    var selectors = {
        SubmitButton: '#Save',
        FrequencyDropdown: '#ProductionWorkOrderFrequency',
    };

    var initialFrequencyValue = $(selectors.FrequencyDropdown).val();

    $(() => {
        $(selectors.SubmitButton).click(() => {
            var frequency = $(selectors.FrequencyDropdown).val();
            if (frequency !== initialFrequencyValue) {
                return confirm("Changing the frequency will cause this maintenance plan's schedule to change. If the schedule is changed, any currently scheduled assignments on this plan will be deleted. Continue?")
            }
        });
    });
})(jQuery);