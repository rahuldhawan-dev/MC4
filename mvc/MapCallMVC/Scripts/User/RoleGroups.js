(($) => {
    const init = () => {
        initCheckBoxToggleButton();
        initSelectAllRoleGroupsCheckboxes();
        initViewSelectedGroupLink();
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

        $('input.role-group-remove-checkbox').on('input', function () {
            const removeAllCheckboxes = $('.select-all-role-groups-checkbox');
            removeAllCheckboxes.prop('checked', false)
        });
    }

    const initSelectAllRoleGroupsCheckboxes = () => {
        $('.select-all-role-groups-checkbox').on('input', function () {
            const removeAllCheckbox = $(this);
            const removableCheckboxes = removeAllCheckbox.closest('table').find('input[name=RoleGroupsToRemove]');
            removableCheckboxes.prop('checked', removeAllCheckbox.prop('checked'));
        });
    }

    const initViewSelectedGroupLink = () => {
        const link = $('#view-selected-group-link');
        const roleGroupEl = $('#RoleGroup');
        const baseUrl = link.attr('href');
        link.on('click', () => {
            const roleGroupId = roleGroupEl.val();
            if (!roleGroupId) {
                return false; // don't let the click work.
            }

            const href = `${baseUrl}\\${roleGroupId}`;
            link.attr('href', href);
        })
    }

    $(document).ready(init);
})(jQuery);