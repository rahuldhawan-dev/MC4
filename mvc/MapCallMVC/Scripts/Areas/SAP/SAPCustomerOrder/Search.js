var SAPCustomerOrderSearch = (function ($) {
  var ELEMENTS = {};

  var scos = {
    initialize: function () {
      ELEMENTS = {
        FSR_ID: $('#FSR_ID'),
        WorkOrder: $('#WorkOrder')
      }

      ELEMENTS.FSR_ID.on('click', function () { $('#WorkOrder').val(''); });
      ELEMENTS.WorkOrder.on('click', function () { $('#FSR_ID').val(''); });
    }
  };

  $(document).ready(scos.initialize);
  return scos;
})(jQuery);