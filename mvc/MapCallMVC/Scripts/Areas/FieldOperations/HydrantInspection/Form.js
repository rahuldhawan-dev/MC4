(($) => {
  const createHidInput = (name, value, parent) => {
    return $('<input>').attr({
      type: 'hidden',
      name: name,
      id: `Hid${name}`,
      value: value
    }).appendTo(parent);
  };

  const inspectionTypeChange = () => {
    const type = $('#HydrantInspectionType').find(':selected').text();
    const reasons =
      $('#FreeNoReadReason, #TotalNoReadReason');

    if (type.toLowerCase() === 'inspect') {
      const parent = $(reasons.parent());

      reasons.append($('<option>').attr({ label: 'Inspect Only', value: 3 }));
      reasons.val(3);
      reasons.prop('disabled', true);

      createHidInput('FreeNoReadReason', 3, parent);
      createHidInput('TotalNoReadReason', 3, parent);
    } else {
      reasons.each((_, r) => {
        const $r = $(r);
        if ($r.val() === 3) {
          $r.val(null);
        }
      });

      reasons.find('option[value=3]').remove();
      reasons.prop('disabled', false);
      $('#HidFreeNoReadReason, #HidTotalNoReadReason').remove();
      $('#ResidualChlorine, #TotalChlorine').change();
    }
  };

  const chlorineValueChange = (toCheck, toToggle) =>
    () => {
      if ($(toCheck).val() !== '') {
        $(toToggle)
          .val(null)
          .prop('disabled', true);
      } else {
        $(toToggle)
          .prop('disabled', false);
      }
    };

    var onSaveButtonClicked = function() {
        var maxChlorineLevel = parseFloat($('#MaxChlorineLevel').val());
        var minChlorineLevel = parseFloat($('#MinChlorineLevel').val());
        var totChlor = parseFloat($('#TotalChlorine').val());
        var resChlor = parseFloat($('#ResidualChlorine').val());
      
        if (totChlor > maxChlorineLevel || resChlor > maxChlorineLevel || (resChlor === 0 && !isNaN(resChlor)) || (totChlor === 0 && !isNaN(totChlor)) || totChlor < minChlorineLevel || resChlor < minChlorineLevel) {
            return confirm('This Entry is outside of the normal range of <0.2 and >3.2. Please select OK to Confirm, or Cancel to Re-Enter');
        }
    }

    $(() => {
    $('#HydrantInspectionType').change(inspectionTypeChange);
    inspectionTypeChange();
    $('#ResidualChlorine').change(chlorineValueChange('#ResidualChlorine', '#FreeNoReadReason'));
    chlorineValueChange('#ResidualChlorine', '#FreeNoReadReason')();
    $('#TotalChlorine').change(chlorineValueChange('#TotalChlorine', '#TotalNoReadReason'));
    chlorineValueChange('#TotalChlorine', '#TotalNoReadReason')();
    $('button[type="submit"]').on('click', onSaveButtonClicked);
  });
})(jQuery);