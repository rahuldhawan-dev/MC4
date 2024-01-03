var EditEquipmentForm = (function ($) {
  var ELEMENTS = {};
  var ef = {
    initialize: function () {
      ELEMENTS = {
        coordinate: '#Coordinate',
        coordinateDisplay: 'span.cp-coordinate-values',
        coordinateIcon: 'img.coordinate-picker-icon',
        facility: '#Facility',
        facilityShowUrl: '#FacilityShowUrl',
        functionalLocation: '#FunctionalLocation',
        manufacturerOther: '#ManufacturerOther',
        operatingCenter: '#OperatingCenter',
        otherCompliance: $('#OtherCompliance'),
        otherComplianceReason: '#OtherComplianceReason',
        planningPlant: '#PlanningPlant',
        sapEquipmentIdOverride: '#SAPEquipmentIdOverride',
        sapEquipmentId: $('#SAPEquipmentId'),
        equipmentManufacturer: '#EquipmentManufacturer',
        hasLockoutRequired: $('#HasLockoutRequired'),
        equipmentType: $('#EquipmentType'),
        saveButton: 'button[type="submit"]',
        typesWithLockoutRequirements: $('#EquipmentTypesWithLockoutRequirements')
      };

      ef.sapEquipmentIdOverride_click();
      ef.shouldLockoutGetLocked_onLoad();
      $(ELEMENTS.sapEquipmentIdOverride).on('click', ef.sapEquipmentIdOverride_click);
      ef.equipmentManufacturer_change();
      ef.initOtherComplianceChange();
      $(ELEMENTS.equipmentManufacturer).on('change', ef.equipmentManufacturer_change);
      $(ELEMENTS.facility).on('change', ef.facility_change); // does not need to fire initially
      $(ELEMENTS.operatingCenter).on('change', ef.clearFunctionalLocation);
      $(ELEMENTS.planningPlant).on('change', ef.clearFunctionalLocation);
      $(ELEMENTS.equipmentType).on('change', ef.onEquipmentPurposeChange);
      $(ELEMENTS.saveButton).on('click', ef.onSaveButtonClicked);
    },

      shouldLockoutGetLocked_onLoad: function () {
          const arrayTypesWithLockoutRequirements = ELEMENTS.typesWithLockoutRequirements.val().split(',');
          const EquipmentPurposeId = ELEMENTS.equipmentType.val();
          const flagForDisablingCheckbox = arrayTypesWithLockoutRequirements.some(e => e === EquipmentPurposeId);
          if (flagForDisablingCheckbox) {
            //the On_Change implementation is different because the dropdown might be disabled on load.
            $('#Prerequisites mc-checkboxlistitem[value=1]').prop('checked', true).prop('enabled', false);
          } else {
            $('#Prerequisites mc-checkboxlistitem[value=1]').prop('enabled', true);
          }
      },

    clearFunctionalLocation: function() {
      $(ELEMENTS.functionalLocation).val('');
    },

    facility_change: function () {
      var facility = $(ELEMENTS.facility);
      if (facility.val() !== null && facility.val() !== '') {
        $.getJSON(
          $(ELEMENTS.facilityShowUrl).val() + '/' + facility.val() + '.json',
          null,
          ef.setCoordinateId
        );
      }
      ef.clearFunctionalLocation();
    },

    sapEquipmentIdOverride_click: function () {
        var sapEquipmentIdOverride = $(ELEMENTS.sapEquipmentIdOverride);
        var sapEquipmentId = $(ELEMENTS.sapEquipmentId);
        if (sapEquipmentIdOverride.val() !== "True") {
          sapEquipmentId.val('');
          sapEquipmentId.closest('.field-pair').toggle(sapEquipmentIdOverride.prop('checked'));
        }
    },

    equipmentManufacturer_change: function () {
      if ($(ELEMENTS.equipmentManufacturer).length > 0) {
        $(ELEMENTS.manufacturerOther).closest('.field-pair')
          .toggle(($('#EquipmentManufacturer :selected').text() === 'OTHER'));
      }
    },

    setCoordinateId: function(entity) {
      var $icon = $(ELEMENTS.coordinateIcon);
      $(ELEMENTS.coordinate).val(entity.CoordinateId);
      if ($icon.attr('coordinateurl').indexOf('&id=') > 0) {
        $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, '&id=' + entity.CoordinateId));
      } else {
        $icon.attr('coordinateurl', $icon.attr('coordinateurl') + '&id=' + entity.CoordinateId);
      }
      $(ELEMENTS.coordinateDisplay).text(entity.Latitude + ', ' + entity.Longitude);
      $icon.attr('src', $icon.attr('src').replace('red', 'blue'));
      },

    onEquipmentPurposeChange: function () {
        const arrayTypesWithLockoutRequirements = ELEMENTS.typesWithLockoutRequirements.val().split(',');
        const selectedEquipmentPurposeId = ELEMENTS.equipmentType.find('option:selected').val();
        const flagForDisablingCheckbox = arrayTypesWithLockoutRequirements.some(e => e === selectedEquipmentPurposeId);
        if (flagForDisablingCheckbox) {
          // if SAPEquipment is a certain type, Lockout should be checked and locked. Else, the checkbox will be enabled.
          $('#Prerequisites mc-checkboxlistitem[value=1]').prop('checked', true).prop('enabled', false);
        } else {
          $('#Prerequisites mc-checkboxlistitem[value=1]').prop('enabled', true);
        }
    },

    initOtherComplianceChange: function () {
        ELEMENTS.otherCompliance.on('change', ef.onOtherComplianceChecked);
        ef.onOtherComplianceChecked();
    },

    onOtherComplianceChecked: function () {
        var otherComplianceIsChecked = ELEMENTS.otherCompliance.is(':checked');
        Application.toggleField(ELEMENTS.otherComplianceReason, otherComplianceIsChecked);
    },

    onSaveButtonClicked: function () {
        // Disabling the checkbox means the value will not be posted back. 
        // There is no readonly checkbox feature in html
        if ($('form').valid()) {
          $('#Prerequisites mc-checkboxlistitem[value=1]').prop('enabled', true);
        }
    }
  };
  $(document).ready(ef.initialize);
  return ef;
})(jQuery);