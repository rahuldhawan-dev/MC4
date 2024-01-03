(($, operatingCentersByState) => {
    // NOTE: The OperatingCenter dropdown doesn't cascade off of the State dropdown.
    // The State dropdown solely exists for this functionality of automatically selecting
    // the operating centers that match.

    const ELEMENTS = {
        appliesToAllOperatingCenters: $('#AppliesToAllOperatingCenters'),
        states: $('#States'),
        operatingCenters: $('#OperatingCenters')
    };

    const selectOperatingCentersByState = () => {
        // boy do I miss TypeScript
        const selectedStates = ELEMENTS.states.val() || [];
        let operatingCenters = [];

        selectedStates.forEach((stateId) => {
            const opcForState = operatingCentersByState[stateId];
            // Dropdown includes all 50 states. Not all states have operating centers.
            if (opcForState) {
                operatingCenters = operatingCenters.concat(opcForState);
            }
        });

        $('#OperatingCenters').val(operatingCenters);
    }

    const setEnabledStateOfOperatingCentersControl = () => {
        const appliesToAll = ELEMENTS.appliesToAllOperatingCenters.is(':checked');
        // Need to force validation to run on the field. If a validation message is
        // visible before they check the checkbox, then the validation message will
        // get stuck there since jQuery won't run validation against disabled controls.
        ELEMENTS.operatingCenters.valid();
        ELEMENTS.operatingCenters.prop('disabled', appliesToAll);
    };

    const init = () => {
        // init and call once in case the States dropdown has any preselected values.
        ELEMENTS.states.on('change', selectOperatingCentersByState);
        selectOperatingCentersByState();

        ELEMENTS.appliesToAllOperatingCenters.on('change', setEnabledStateOfOperatingCentersControl);
        setEnabledStateOfOperatingCentersControl();
    };

    $(document).ready(init);


})(jQuery, window.operatingCentersByState);