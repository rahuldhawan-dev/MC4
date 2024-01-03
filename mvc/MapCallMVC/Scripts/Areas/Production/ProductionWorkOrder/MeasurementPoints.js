var MeasurementPoints = (function($) {
  var ELEMENTS = {};
  var mp = {
    initialize: function() {
      ELEMENTS = {
        checkAll: $('#CheckAll'),
        completeMeasurementPoints: $('#CompleteMeasurementPoints'),
        submitMeasurementPoints: $('#SubmitMeasurementPoints'),
        selectedEquipmentIds: $('#SelectedEquipmentIds'),
        selectedCompletionEquipmentIds: $('#SelectedCompletionEquipmentIds')
      };
      ELEMENTS.completeMeasurementPoints.on('click', mp.completeMeasurementPoints_click);
      ELEMENTS.submitMeasurementPoints.on('click', mp.submitMeasurementPoints_click);
      ELEMENTS.checkAll.on('click', mp.checkAll_click);
    },
    checkAll_click: function () {
      $('input:checkbox').prop('checked', $('input:checked[id=MeasurementPointEquipmentIds]').length === 0);
    },
    completeMeasurementPoints_click: function () {
      var checked = $('input:checked[id=MeasurementPointEquipmentIds]');
      if (checked.length > 0) {
        ELEMENTS.selectedCompletionEquipmentIds.val(checked.map(function () { return this.value; }).get().join(','));
      }
    },
    submitMeasurementPoints_click: function () {
      var checked = $('input:checked[id=MeasurementPointEquipmentIds]');
      if (checked.length > 0) {
        ELEMENTS.selectedEquipmentIds.val(checked.map(function () { return this.value; }).get().join(','));
      }
    }
  }

  $(document).ready(mp.initialize);
  return mp;
})(jQuery);