var EnvironmentalPermit = (function ($) {
  var ELEMENTS = {};
  var ef = {
    initialize: function () {
      ELEMENTS = {
        requirementCount: '#RequirementCount'
      };
    },

    validateRequiresRequirements: function (htmlElementValue, htmlElement) {
      if (htmlElementValue === "True") {
        return ($(ELEMENTS.requirementCount).val() > 0);
      }
      return true;
    }
  };
  $(document).ready(ef.initialize);
  return ef;
})(jQuery);