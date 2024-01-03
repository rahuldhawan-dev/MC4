// requires Form.js

(function($) {
  ProductionWorkOrderForm.afterInit = function() {
    ProductionWorkOrderForm.pmatOverride_change();
    // don't reset the functional location on init
    var fl = $('#FunctionalLocation').val();
    ProductionWorkOrderForm.operatingCenter_change();
    ProductionWorkOrderForm.equipment_change();
    $('#FunctionalLocation').val(fl);
  };
})(jQuery);
