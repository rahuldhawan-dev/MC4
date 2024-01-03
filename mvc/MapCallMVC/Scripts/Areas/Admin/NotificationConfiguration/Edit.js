(($) => {
    const ELEMENTS = {
        appliesToAllOperatingCenters: $('#AppliesToAllOperatingCenters'),
        operatingCenter: $('#OperatingCenter')
    };

    const setEnabledStateOfOperatingCentersControl = () => {
        const appliesToAll = ELEMENTS.appliesToAllOperatingCenters.is(':checked');
        // Need to force validation to run on the field. If a validation message is
        // visible before they check the checkbox, then the validation message will
        // get stuck there since jQuery won't run validation against disabled controls.
        ELEMENTS.operatingCenter.valid();
        ELEMENTS.operatingCenter.prop('disabled', appliesToAll);
    };

    const init = () => {
        ELEMENTS.appliesToAllOperatingCenters.on('change', setEnabledStateOfOperatingCentersControl);
        setEnabledStateOfOperatingCentersControl();
    };

    $(document).ready(init);
})(jQuery);