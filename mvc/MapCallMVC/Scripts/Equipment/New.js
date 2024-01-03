var NewEquipmentForm = (function ($) {
  var ELEMENTS = {};
  var ef = {
    initialize: function () {
      ELEMENTS = {
        coordinate: '#Coordinate',
        coordinateIcon: 'img.coordinate-picker-icon',
        department: $('#Department'),
        facility: $('#Facility'),
        facilityShowUrl: $('#FacilityShowUrl'),
        functionalLocation: '#FunctionalLocation',
        manufacturerOther: $('#ManufacturerOther'),
        operatingCenter: '#OperatingCenter',
        otherCompliance: $('#OtherCompliance'),
        otherComplianceReason: '#OtherComplianceReason',
        planningPlant: '#PlanningPlant',
        sapEquipmentIdOverride: $('#SAPEquipmentIdOverride'),
        sapEquipmentId: $('#SAPEquipmentId'),
        equipmentManufacturer: $('#EquipmentManufacturer'),
        equipmentType: $('#EquipmentType'),
        saveButton: 'button[type="submit"]',
        typeID: $('#TypeID'),
        typesWithLockoutRequirements: $('#EquipmentTypesWithLockoutRequirements')
      };

      //call events to set initial status
      ef.sapEquipmentIdOverride_click();
      ef.equipmentManufacturer_change();
      ef.facility_change();
      ef.onEquipmentPurposeChange();
      ef.initOtherComplianceChange();
      //wire up events
      ELEMENTS.sapEquipmentIdOverride.on('click', ef.sapEquipmentIdOverride_click);
      ELEMENTS.equipmentManufacturer.on('change', ef.equipmentManufacturer_change);
      ELEMENTS.facility.on('change', ef.facility_change);
      $(ELEMENTS.operatingCenter).on('change', ef.clearFunctionalLocation);
      $(ELEMENTS.planningPlant).on('change', ef.clearFunctionalLocation);
      $(ELEMENTS.equipmentType).on('change', ef.onEquipmentPurposeChange);
      $(ELEMENTS.saveButton).on('click', ef.onSaveButtonClicked);
    },

    clearFunctionalLocation: function () {
      $(ELEMENTS.functionalLocation).val('');
    },

    facility_change: function() {
      if (ELEMENTS.facility.val() !== null && ELEMENTS.facility.val() !== '') {
        $.getJSON(
          ELEMENTS.facilityShowUrl.val() + '/' + ELEMENTS.facility.val() + '.json',
          null,
          ef.setValues
        );
      }
      ef.clearFunctionalLocation();
    },

    setCoordinateId: function (entity) {
      var $icon = $(ELEMENTS.coordinateIcon);
      if (entity.CoordinateId !== null && entity.CoordinateId !== '') {
        $(ELEMENTS.coordinate).val(entity.CoordinateId);
        if ($icon.attr('coordinateurl').indexOf('&id=') > 0) {
          $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, '&id=' + entity.CoordinateId));
        } else {
          $icon.attr('coordinateurl', $icon.attr('coordinateurl') + '&id=' + entity.CoordinateId);
        }
        $icon.attr('src', $icon.attr('src').replace('red', 'blue'));
      } else {
        $icon.attr('coordinateurl', $icon.attr('coordinateurl').replace(/&id=\d+/, ''));
        $icon.attr('src', $icon.attr('src').replace('blue', 'red'));
      }
    }, 

    setDepartmentId: function(entity) {
      ELEMENTS.department.val(entity.DepartmentId);
    },

    setValues: function (entity) {
      ef.setDepartmentId(entity);
      ef.setCoordinateId(entity);
    },

    manufacturerOtherKnown: function() {
      if ($('#EquipmentManufacturer :selected').text() === 'OTHER') {
        if (ELEMENTS.manufacturerOther.val() === '')
          return false;
        return true;
      }
      return true;
    },

    sapEquipmentIdOverride_click: function () {
      if (!ELEMENTS.sapEquipmentIdOverride.prop('checked'))
        ELEMENTS.sapEquipmentId.val('');
      ELEMENTS.sapEquipmentId.closest('.field-pair').toggle(ELEMENTS.sapEquipmentIdOverride.prop('checked'));
    },
    equipmentManufacturer_change: function () {
      if (ELEMENTS.equipmentManufacturer.length > 0) {
        ELEMENTS.manufacturerOther.closest('.field-pair')
          .toggle(($('#EquipmentManufacturer :selected').text() === 'OTHER'));
      }
    },

    onEquipmentPurposeChange: function() {
        const arrayTypesWithLockoutRequirements = ELEMENTS.typesWithLockoutRequirements.val().split(',');
        const selectedEquipmentPurposeId = ELEMENTS.equipmentType.find('option:selected').val();
        const flagForDisablingCheckbox = arrayTypesWithLockoutRequirements.some(e => e === selectedEquipmentPurposeId);
        if (flagForDisablingCheckbox ) {
             // if SAPEquipment is a certain type, Lockout should be checked and locked. Else, the checkbox will be enabled.
              $('#Prerequisites mc-checkboxlistitem[value=1]').prop('checked', true).prop('enabled', false);
          } else {
            $('#Prerequisites mc-checkboxlistitem[value=1]').prop('enabled', true);
        }
    },

    initOtherComplianceChange: function() {
        ELEMENTS.otherCompliance.on('change', ef.onOtherComplianceChecked);
        ef.onOtherComplianceChecked();
    },

    onOtherComplianceChecked: function() {
        var otherComplianceIsChecked = ELEMENTS.otherCompliance.is(':checked');
        Application.toggleField(ELEMENTS.otherComplianceReason, otherComplianceIsChecked);
    },
    
    onSaveButtonClicked: function() {
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