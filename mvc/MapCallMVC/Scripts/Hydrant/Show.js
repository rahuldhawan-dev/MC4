var Hydrants = (function($) {
  return {
    initialize: function() {
      $('#out-of-service-form').on('submit', function() {
        return confirm('Are you sure you want to change the out of service status of this hydrant? Doing so will send out an email notification.');
      });
    }
  };
})(jQuery);

$(document).ready(Hydrants.initialize);