var Services = (function ($) {
  var ELEMENTS = {};
  var RENEWAL_SERVICE_CATEGORIES;
  var MEASUREMENT_ONLY;
  var townStateServiceUrl;
  var s = {
    init: function () {
      ELEMENTS = {
        dateInstalled: $('#DateInstalled'),
        serviceCategory: $('#ServiceCategory'),
      };
      RENEWAL_SERVICE_CATEGORIES = ['3', '6', '13', '23'];
  	},
    validateInstalledAndRenewal: function (val, el) {
  	  var isRequired = (RENEWAL_SERVICE_CATEGORIES.indexOf(ELEMENTS.serviceCategory.val()) > -1 && ELEMENTS.dateInstalled.val() !== '');
  	  if (isRequired && !val) {
  	    return false;
      }
  	  return true;
  	}
  };
  $(document).ready(s.init);
  return s;
})(jQuery);