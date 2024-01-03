(($) => {
    var selectors = {
        add: '#AddEquipment',
        addAll: '#AddAllEquipment',
        equipment: '#Equipment',
        equipmentForm: '#RemoveEquipmentMaintenancePlanForm',
        remove: '#RemoveButton',
        removeAll: '#RemoveAllButton',
        removeEquipmentCheckbox: '.removeEquipmentCheckbox'
    };

    var confirmRemove = (onConfirm) => {
        var answer = confirm('Are you sure you want to remove the selected equipment linked to this Maintenance Plan?');
        if (answer) {
            onConfirm();
        }
    };

    var removeCheckedEquipment = () => {
        $(selectors.equipmentForm).submit();
    };

    var addAllClick = () => {
        $(selectors.equipment + ' option').prop('selected', true);
        $(selectors.add).click();
    };
    
    var removeClick = () => {
        confirmRemove(() => {
            removeCheckedEquipment();
        });
    };

    var removeAllClick = () => {
        confirmRemove(() => {
            $(selectors.removeEquipmentCheckbox).attr('checked', true);
            removeCheckedEquipment();
        });
    };

    $(() => {
        $(selectors.addAll).click(addAllClick);
        $(selectors.remove).click(removeClick);
        $(selectors.removeAll).click(removeAllClick);
    });
})(jQuery);