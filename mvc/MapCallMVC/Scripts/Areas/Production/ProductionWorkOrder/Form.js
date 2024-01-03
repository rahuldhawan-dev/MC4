var ProductionWorkOrderForm = (function($) {
  var ELEMENTS = {};

  var pwof = {
    initialize: function() {
      ELEMENTS = {
        breakdownIndicator: $('#BreakdownIndicator'),
        coordinate: $('#Coordinate'),
        coordinateIcon: 'img.coordinate-picker-icon',
        correctiveOrderProblemCode: '#CorrectiveOrderProblemCode',
        equipment: $('#Equipment'),
        equipmentCriticalNotesUrl: $('#EquipmentCriticalNotesUrl'),
        equipmentShowUrl: $('#EquipmentShowUrl'),
        facility: $('#Facility'),
        facilityShowUrl: $('#FacilityShowUrl'),
        functionalLocation: $('#FunctionalLocation'),
        getOperatingCenterIsSAPWorkOrdersEnabled: $('#GetOperatingCenterIsSAPWorkOrdersEnabled'),
        operatingCenter: $('#OperatingCenter'),
        planningPlant: $('#PlanningPlant'),
        otherProblemNotes: '#OtherProblemNotes',
        pmatOverride: $('#PlantMaintenanceActivityTypeOverride'),
        productionPrerequisites: document.querySelectorAll('#Prerequisites mc-checkboxlistitem'),
        saveButton: 'button[type="submit"]',
        sendToSAP: $('#SendToSAP'),
        workDescription: $('#ProductionWorkDescription'),
        workDescriptionShowUrl: $('#ProductionWorkDescriptionShowUrl'),
        wbsElement: $('#WBSElement'),
        history: $('#History'),
        getEquipmentHistoryUrl: $('#GetEquipmentHistoryUrl')

    };
      AjaxTable.initialize('#wbsElementTable');
      ELEMENTS.equipment.on('change', pwof.equipment_change);
      ELEMENTS.facility.on('change', pwof.facility_change);
      ELEMENTS.planningPlant.change(pwof.clearFunctionalLocation);
      ELEMENTS.workDescription.on('change', pwof.workDescription_change);
      ELEMENTS.pmatOverride.on('change', pwof.pmatOverride_change);
      ELEMENTS.operatingCenter.on('change', pwof.operatingCenter_change);
      $(ELEMENTS.correctiveOrderProblemCode).on('change', pwof.correctiveOrderProblemCode_change);
      $(ELEMENTS.saveButton).on('click', pwof.onSaveButtonClicked);
      pwof.correctiveOrderProblemCode_change();

      pwof.afterInit();
    },

    // to be set by new/edit scripts
    afterInit: function() {},

    clearFunctionalLocation: function() {
      $(ELEMENTS.functionalLocation).val('');
    },

    correctiveOrderProblemCode_change: function() {
        var correctiveOrderSelectedOption = $(ELEMENTS.correctiveOrderProblemCode + ' option:selected');
        var val = correctiveOrderSelectedOption.text();
        Application.toggleField(ELEMENTS.otherProblemNotes, val === 'OTHER');

        var preJobSafetyBriefPreRequisite = $('#Prerequisites mc-checkboxlistitem[value=6]');
        correctiveOrderSelectedOption.val() === '' ? // val() is empty when corrective order is not selected
            preJobSafetyBriefPreRequisite.show() :
            preJobSafetyBriefPreRequisite.hide();
    },

    equipment_change: function() {
      pwof.clearPrerequisitesUiState();

      if (ELEMENTS.equipment.prop('selectedIndex') !== 0) {
        const equipmentId = ELEMENTS.equipment.val();
        $.getJSON(`${ELEMENTS.equipmentShowUrl.val()}/${equipmentId}.json`, null, function(equipmentEntity) {
          pwof.setCoordinateId(equipmentEntity);
          pwof.setPrerequisitesUiState(equipmentEntity.ProductionPrerequisites);
        });

        $.ajax({
          type: 'GET',
          url: `${ELEMENTS.equipmentCriticalNotesUrl.val()}/${equipmentId}.json`,
          success: function(notes) {
            if (notes && notes.length) {
              Application.displayNotification(notes);
            }
          },
          error: function() {
            Application.displayNotification('Error loading equipment critical notes');
          }
        });
        ELEMENTS.history
            .load(ELEMENTS.getEquipmentHistoryUrl.val(),
                $.param({
                    operatingCenterId: ELEMENTS.operatingCenter.val(),
                    equipment: ELEMENTS.equipment.val()
                }),
                () => {
                    ELEMENTS.history.ajaxifyTable();
                });
      } else {
          ELEMENTS.history.html('');
      }
    },

    clearPrerequisitesUiState: () => {
      ELEMENTS.productionPrerequisites.forEach((prerequisiteCheckbox) => {
        prerequisiteCheckbox.checked = false;
        prerequisiteCheckbox.enabled = true;
      });
    },

    preparePrerequisitesUiStateForSaving: () => {
      ELEMENTS.productionPrerequisites.forEach((prerequisiteCheckbox) => (prerequisiteCheckbox.enabled = true));
    },

      setPrerequisitesUiState: (equipmentPrerequisites) => {
          console.log(equipmentPrerequisites)
      ELEMENTS.productionPrerequisites.forEach((prerequisiteCheckbox) => {
        const prerequisiteIsRequired = equipmentPrerequisites.some((x) => x.Id.toString() === prerequisiteCheckbox.value);
        prerequisiteCheckbox.checked = prerequisiteIsRequired;
        prerequisiteCheckbox.enabled = !prerequisiteIsRequired;
      });
    },

    facility_change: function() {
      // reset functional location
      ELEMENTS.functionalLocation.val('');

      // if we select a facility, lets get it's coordinate ID and use it if we don't have one already
      if (ELEMENTS.facility.val() !== null && ELEMENTS.facility.val() !== '') {
        $.getJSON(ELEMENTS.facilityShowUrl.val() + '/' + ELEMENTS.facility.val() + '.json', null, pwof.setCoordinateIdAndCheckConfined);
      }
      pwof.clearFunctionalLocation();
    },

    operatingCenter_change: function() {
      var opCntr = $(ELEMENTS.operatingCenter).val();
      if (opCntr) {
        $.ajax({
          type: 'GET',
          url: ELEMENTS.getOperatingCenterIsSAPWorkOrdersEnabled.val() + '/' + opCntr,
          success: function(result) {
            ELEMENTS.sendToSAP.val(result.IsSAPWorkOrdersEnabled);
            Application.toggleField(ELEMENTS.functionalLocation, result.IsSAPWorkOrdersEnabled === true);
          },
        });
        pwof.addOperatingCenterToWBSFindUrl();
      }
      pwof.clearFunctionalLocation();
    },
    addOperatingCenterToWBSFindUrl: function() {
      //  -- add or replace operatingCenterId at the end
      var opCntr = $(ELEMENTS.operatingCenter).val();
      var wbsLink = $('[data-ajax-table="#wbsElementTable"]').attr('href');
      var matches = /\d+$/;
      var foo = wbsLink.match(matches);
      if (foo !== null && foo !== undefined && foo.length > 0) {
        $('[data-ajax-table="#wbsElementTable"]').attr('href', wbsLink.replace(/\d+$/, opCntr));
      } else {
        $('[data-ajax-table="#wbsElementTable"]').attr('href', wbsLink + '?operatingCenterId=' + opCntr);
      }
    },

    pmatOverride_change: function() {
      if (ELEMENTS.pmatOverride.val() !== '') {
        ELEMENTS.wbsElement.closest('.field-pair').toggle(true);
      } else {
        ELEMENTS.wbsElement.closest('.field-pair').toggle(false);
        ELEMENTS.wbsElement.val('');
      }
      pwof.addOperatingCenterToWBSFindUrl();
    },

    setCoordinateId: function(entity) {
      var $icon = $(ELEMENTS.coordinateIcon);
      if (entity.CoordinateId !== null && entity.CoordinateId !== '') {
        ELEMENTS.coordinate.val(entity.CoordinateId);
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

    setCoordinateIdAndCheckConfined: function(entity) {
      if (entity.HasConfinedSpaceRequirement) {
        $('#Prerequisites input:checkbox[value=2]').prop('checked', true);
      }

      var $icon = $(ELEMENTS.coordinateIcon);

      if (entity.CoordinateId !== null && entity.CoordinateId !== '') {
        ELEMENTS.coordinate.val(entity.CoordinateId);
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

    validateWBSNumber: function(val, element) {
      // whether or not to send to sap is determined on operating center change
      if ($(ELEMENTS.operatingCenter).val() === '' || $(ELEMENTS.sendToSAP).val() === 'false') return true;

      var url = '../../SAP/WBSElement/index.json';
      // not required to verify if it is not populated or assetType/operating center hasn't been selected
      if (element.value === '') return true;
      var valid = false;
      $.ajax({
        type: 'GET',
        async: false,
        url: window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\//) ? '../' + url : url,
        data: {
          WBSNumber: $(ELEMENTS.wbsElement).val(),
          OperatingCenter: $(ELEMENTS.operatingCenter).val(),
        },
        success: function(d) {
          if (d.Data[0].SAPErrorCode.indexOf('Successful') > -1) valid = true;
          else {
            window.setTimeout(function() {
              $('span [for=WBSElement]').text(d.Data[0].SAPErrorCode);
            }, 100);
          }
        },
      });

      return valid;
    },

    onSaveButtonClicked: function() {
      if ($('form').valid()) {
        pwof.preparePrerequisitesUiStateForSaving();
      }
    },

    workDescription_change: function() {
      if (ELEMENTS.workDescription.val() !== null && ELEMENTS.workDescription.val() !== '') {
        $.getJSON(ELEMENTS.workDescriptionShowUrl.val() + '/' + ELEMENTS.workDescription.val() + '.json', null, function(entity) {
          ELEMENTS.breakdownIndicator.val(entity.BreakdownIndicator);
        });
      }
    },
  };

  pwof.ELEMENTS = ELEMENTS;
  $(document).ready(pwof.initialize);
  return pwof;
})(jQuery);
