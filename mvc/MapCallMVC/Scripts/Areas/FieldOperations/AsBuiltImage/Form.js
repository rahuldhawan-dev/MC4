var AsBuiltImage = (function($) {
  var ELEMENTS = {};
  var townStateServiceUrl;

  var abi = {
    init: function() {
      ELEMENTS = {
        coordinate: $('#Coordinate'),
        state: $('#State'),
        town: $('#Town'),
        street: $('#Street'),
        streetPrefix: $('#StreetPrefix'),
        streetSuffix: $('#StreetSuffix')
      };
      townStateServiceUrl = $('#TownStateServiceUrl').val();
    },

    getAddress:function() {
      if (ELEMENTS.coordinate.val() || ELEMENTS.town.val() === '') {
        return null;
      }
      var street = ELEMENTS.streetPrefix.val() + ' ' + ELEMENTS.street.val() + ' ' + ELEMENTS.streetSuffix.val();
      var selectedTown = ELEMENTS.town.find('option:selected');
      var town = selectedTown.text();
      var state;

      if (!state && selectedTown.val()) {
        $.ajax({
          url: townStateServiceUrl,
          data: {
            id: ELEMENTS.town.val()
          },
          async: false, // So this just goes.
          type: 'GET',
          success: function(result) {
            state = result.state;
          },
          error: function() {
            alert("Something went wrong finding the state for the selected town.");
          }
        });
      }
      return street + ' ' + town + ',' + state;
    }
  };
  $(document).ready(abi.init);
  return abi;
})(jQuery);