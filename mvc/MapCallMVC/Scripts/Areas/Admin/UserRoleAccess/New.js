(($, operatingCentersByState) => {
    const ELEMENTS = {
        isForAllOperatingCenters: $('#IsForAllOperatingCenters'),
        modules: $('#Modules'),
        saveRolesButton: $('#save-roles-button'),
        states: $('#States'),
        operatingCenters: $('#OperatingCenters'),
        operatingCentersValidatable: $("[name='OperatingCenters_CheckBoxList']"),
        selectAllModules: $('#select-all-modules')
    };

    const selectOperatingCentersByState = () => {
        const selectedStates = ELEMENTS.states.find('input:checked');
        let operatingCenters = [];
        selectedStates.each(function() {
            const stateId = $(this).val();
            const opcForState = operatingCentersByState[stateId];
            // Dropdown includes all 50 states. Not all states have operating centers.
            if (opcForState) {
                operatingCenters = operatingCenters.concat(opcForState);
            }
        });

        // I should probably write a less clunky way of setting the value of
        // checkboxlist but that's out of scope. There's no mc-checkboxlist.
        // When the company supports iOS 16.4, which is the hold out browser
        // for web components that support forms, then I can come back and 
        // clean up this mess with a proper mc-checkboxlist component.
        const operatingCenterCheckBoxes = ELEMENTS.operatingCenters.find('input');
        operatingCenterCheckBoxes.each(function () {
            const cb = $(this);
            const shouldCheck = operatingCenters.includes(parseInt(cb.val()));
            cb.prop('checked', shouldCheck);
        });
    }

    const onIsForAllOperatingCentersChanged = () => {
        const appliesToAll = ELEMENTS.isForAllOperatingCenters.is(':checked');

        // Need to force validation to run on the field. If a validation message is
        // visible before they check the checkbox, then the validation message will
        // get stuck there since jQuery won't run validation against disabled controls.
        ELEMENTS.operatingCentersValidatable.valid();

        // TODO: Need a less clunky way of dealing with this too. Out of scope.
        ELEMENTS.operatingCenters.attr('disabled', appliesToAll);
        ELEMENTS.states.attr('disabled', appliesToAll);
        ELEMENTS.operatingCenters.find('input').prop('disabled', appliesToAll);
        ELEMENTS.states.find('input').prop('disabled', appliesToAll);
    };

    onResetAddRolesButtonClicked = () => {

    }

    const onSelectAllModulesChanged = () => {
        const isChecked = ELEMENTS.selectAllModules.is(':checked');
        ELEMENTS.modules.find('input[type="checkbox"]').each(function () {
            $(this).prop('checked', isChecked);
        });
    }

    const initializeDialog = () => {
        const dialogEl = document.getElementById('add-roles-dialog');
        const openButton = document.getElementById('open-dialog-button');
        const closeDialogButton = dialogEl.querySelector('.close-dialog-button');
        const resetButton = document.getElementById('reset-add-roles-form-button');

        $(openButton).on('click', () => {
            // For the role groups page, they can re-open the dialog to add
            // more roles. We need to override the no-double-submit functionality here.
            ELEMENTS.saveRolesButton.prop('disabled', false);
            dialogEl.showModal();

            // NOTE: Any code after showModal will run immediately. showModal doesn't pause execution.
        });
        $(closeDialogButton).on('click', () => {
            dialogEl.close();
        });
        $(resetButton).on('click', () => {
            // We can't do a normal form reset because it makes all the
            // checkboxes lose their values, which is dumb. 
            $(dialogEl).find('input:checked').each(function () {
                $(this).prop('checked', false);
            })
        });
    }

    const initializeSelectAll = () => {
        // init and call once in case the States dropdown has any preselected values.
        ELEMENTS.states.on('change', selectOperatingCentersByState);
        selectOperatingCentersByState();
    }

    const init = () => {
        initializeDialog();
        initializeSelectAll();

        ELEMENTS.isForAllOperatingCenters.change(onIsForAllOperatingCentersChanged);
        onIsForAllOperatingCentersChanged();

        // no need to run this one immediately.
        ELEMENTS.selectAllModules.change(onSelectAllModulesChanged);
    };

    $(document).ready(init);
})(jQuery, window.operatingCentersByState);