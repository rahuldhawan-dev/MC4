var Restoration = (function ($) {

  var validateFootageForNote = function (noteValue, $actualFootageEl) {
    if (noteValue) {
      return true;
    }

    var estimated = parseFloat($('#EstimatedRestorationFootage').val());
    var actual = parseFloat($actualFootageEl.val());

    // If either field is not entered then we can't determine anything, so return as valid.
    if (isNaN(estimated) || isNaN(actual)) {
      return true;
    }

    // The notes field is only required when the actual value is greater than the estimated.
    return (actual <= estimated);
  }

  var rest = {
    init: function () {
      if (Application.routeData.action === "New") {
        var updateWorkOrderButton = $('<button id="update-workorder" type="button">Update</button>');
        updateWorkOrderButton.click(rest.onUpdateWorkOrderClicked);
        $('#WorkOrder').after(updateWorkOrderButton);
      }

      $('#CompletedByOthers').change(rest.onCompletedByOthersChecked);
      rest.onCompletedByOthersChecked(); // Set initial view
    },

    onCompletedByOthersChecked: function() {
      var toShowHide = $('.hide-if-completed-by-others');
      if ($('#CompletedByOthers').is(':checked')) {
        toShowHide.hide();
      } else {
        toShowHide.show();
      }
    },

    onUpdateWorkOrderClicked: function () {
      var url = $('#redirect-url').val();
      var workorder = $('#WorkOrder').val();
      window.location = url + '/' + workorder;
    },

    validatePartialRestorationNotes: function (value, element) {
      return validateFootageForNote(value, $('#PartialPavingSquareFootage'));
    },

    validateFinalRestorationNotes: function (value, element) {
      return validateFootageForNote(value, $('#FinalPavingSquareFootage'));
    }
  };

  $(document).ready(rest.init);

  return rest;
})(jQuery);
