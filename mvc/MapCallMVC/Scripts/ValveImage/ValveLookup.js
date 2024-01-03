var ValveLookup = (function($) {

  var setMessage = function(message) {
    $('#look-up-error').text(message);
  };

  var setData = function(data) {
    // Set the easy stuff first:
    $('#TownSection').val(data.townSection);
    $('#StreetNumber').val(data.streetNumber);
    $('#NumberOfTurns').val(data.turns);
    $('#ValveSize').val(data.valveSize);
    $('#CrossStreet').val(data.crossStreet);
    $('#Location').val(data.location);
    $('#NormalPosition').val(data.normalPosition);
    $('#OpenDirection').val(data.openDirection);
    $('#DateCompleted').val(data.dateCompleted);
    $('#ValveNumber').val(data.valveNumber);
    $('#IsDefaultImageForValve').val(data.isDefaultImageForValve);

    // NOTE: Since this is rarely used, 'refresh' is the event
    //       fired by cascading drop downs when the items refresh.
    $('#OperatingCenter').val(data.operatingCenterId);
    $('#Town').one('refresh', function() {
      // streetId can be 0 sadly.
      if (data.streetId) {
        $('#StreetIdentifyingInteger').one('refresh', function() {

          $('#Valve').one('refresh', function() {
            $('#Valve').val(data.valveId);
          });

          $('#StreetIdentifyingInteger').val(data.streetId);
          $('#StreetIdentifyingInteger').change();
        });
      }

      $('#Town').val(data.townId);
      $('#Town').change();
    });
    $('#OperatingCenter').change();
  };

  return {
    onBegin: function() {
      // Clear out the message.
      setMessage('Searching...');
    },
    onSuccess: function(data) {
      if (!data.success) {
        setMessage(data.message);
      } else {
        setMessage('');
        setData(data);
      }
    },
    onError: function() {
      setMessage("An unexpected error has occurred. Please try again.");
    }
  };

})(jQuery);