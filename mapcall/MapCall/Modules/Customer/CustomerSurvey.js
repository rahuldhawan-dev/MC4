jQuery(document).ready(function() {
    CustomerSurvey.initialize();
});

var CustomerSurvey = {
  initialize: function() {
    if (jQuery('#divContent').length > 0)
      jQuery('#divContent').tabs();
  }
};