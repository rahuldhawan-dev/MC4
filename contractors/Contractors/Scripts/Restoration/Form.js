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
    validatePartialRestorationNotes: function (value, element) {
      return validateFootageForNote(value, $('#PartialPavingSquareFootage'));
    },

    validateFinalRestorationNotes: function (value, element) {
      return validateFootageForNote(value, $('#FinalPavingSquareFootage'));
    }
  };

  return rest;
})(jQuery);
