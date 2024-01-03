(function ($) {

    function initialize(){
        $('#SystemDeliveryType').change(onSystemDeliveryTypeChange);
        // Fire once to init the field visibility.
        onSystemDeliveryTypeChange();
    }

    function onSystemDeliveryTypeChange(){
        var selectedSystemDeliveryType = $('#SystemDeliveryType').val();
        Application.toggleField($('#PublicWaterSupplies'), selectedSystemDeliveryType === '1'); //SystemDeliveryType (1) == WATER
        Application.toggleField($('#WasteWaterSystems'), selectedSystemDeliveryType === '2'); //SystemDeliveryType (2) == WASTE_WATER
    }
    
    $(document).ready(initialize);

})(jQuery);