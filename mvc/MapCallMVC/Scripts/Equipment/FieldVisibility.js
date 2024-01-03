(function ($) {

    function initialize() {
        $('#EquipmentStatus').change(onEquipmentStatusChange);
        // Fire once to init the field visibility.
        onEquipmentStatusChange();
    }

    function onEquipmentStatusChange() {
        var selectedEquipmentStatus = $('#EquipmentStatus').val();
        Application.toggleField($('#DateRetired'), (selectedEquipmentStatus === '4' || selectedEquipmentStatus === '5')); //RETIRED = 4,PENDING_RETIREMENT = 5,
    }

    $(document).ready(initialize);

})(jQuery);