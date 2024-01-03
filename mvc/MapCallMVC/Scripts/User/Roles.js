(($) => {
    const init = () => {
        initCheckBoxToggleButton();
        initSelectAllRolesPerAppCheckboxes();
        initRemoveRolesFormButton();
    };

    const initCheckBoxToggleButton = () => {
        $('.checkbox-button-toggle').on('click', function (e) {
            const button = $(this);
            const cb = button.find('input');
            // We need to mimic checking a checkbox if the button itself is clicked
            // rather than the checkbox.
            if (e.originalEvent.target !== cb[0]) {
                cb.prop('checked', !cb.prop('checked'));
                cb.trigger('input');
            }
        });

        $('.checkbox-button-toggle input').on('input', function () {
            const removePerAppCheckboxes = $('.select-all-roles-per-app-checkbox');
            removePerAppCheckboxes.prop('checked', $(this).prop('checked'))
            removePerAppCheckboxes.trigger('input');
        });
    }

    const initSelectAllRolesPerAppCheckboxes = () => {
        $('.select-all-roles-per-app-checkbox').on('input', function () {
            const removePerAppCheckbox = $(this);
            const removableCheckboxes = removePerAppCheckbox.closest('table').find('input[name=RolesToRemove]');
            removableCheckboxes.prop('checked', removePerAppCheckbox.prop('checked'));
        });
    }

    const initRemoveRolesFormButton = () => {
        $('#remove-selected-roles-button').on('click', function () {
            $('#remove-roles-form').submit();
        })
    }

    $(document).ready(init);
})(jQuery);