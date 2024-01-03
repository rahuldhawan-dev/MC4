// requires Form.js

(function ($) {
  ProductionWorkOrderForm.afterInit = function() {
    ProductionWorkOrderForm.equipment_change();
    ProductionWorkOrderForm.facility_change();
    ProductionWorkOrderForm.pmatOverride_change();
    // this was causing a console error
    //ProductionWorkOrderForm.ELEMENTS.operatingCenter.focus();
  };
})(jQuery);
