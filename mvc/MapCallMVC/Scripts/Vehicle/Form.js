var Vehicle = (function($) {

  var methods = {
    init: function() {
      $('#OwnershipType').change(methods.toggleLeaseVisibility);
      methods.toggleLeaseVisibility(); // Call to set initial visibility.
    },

    toggleLeaseVisibility: function() {
      var leaseFields = $('.lease-field').closest('.field-pair');
      var ownerType = $('#OwnershipType option:selected').text().toUpperCase();
      if (ownerType === 'LEASED') {
        leaseFields.show();
      } else {
        leaseFields.hide();
      }
    }
  };

  $(document).ready(methods.init);

  return methods;
})(jQuery);