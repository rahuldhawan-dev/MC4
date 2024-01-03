var Valve = (function($) {
  var ELEMENTS = {};
  var townStateServiceUrl;

  /*
   * MC-2607, we want to show "Date Retired" label/input for an asset when it's status
   * is 'REMOVED', 'RETIRED' or 'REQUEST RETIREMENT' - so let's close over these and
   * reference them later during the asset status change event handler
   */
  const showDateRetiredForStatuses = [
    { id: 5, description: 'REMOVED' },
    { id: 6, description: 'RETIRED' },
    { id: 14, description: 'REQUEST RETIREMENT' }
    ];

  const workOrderStatuses = [
        { id: 5, description: 'REMOVED' },
        { id: 6, description: 'RETIRED' },
        { id: 1, description: 'ACTIVE' }
  ];

  let hasStatusChanged = false; 

  var val = {
    init: function() {
      ELEMENTS = {
        controlsCrossing: $('#ControlsCrossing'),
        coordinate: $('#Coordinate'),
        criticalNotes: $('#CriticalNotes'),
        dateRetired: $('#DateRetired'),
        mainCrossingOperatingCenter: $('#MainCrossingOperatingCenter'),
        mainCrossings: $('#MainCrossings'),
        isCritical: $('#Critical'),
        operatingCenter: $('#OperatingCenter'),
        sapEquipmentId: $('#SAPEquipmentId'),
        state: $('#State'),
        status: $('#Status'),
        stateAbbreviation: $('#StateAbbreviation'),
        street: $('#Street'),
        streetNumber: $('#StreetNumber'),
        town: $('#Town'),
        townName: $('#TownName'),
        townSection: $('#TownSection'),
        turns: $('#Turns'),
        valveStatus: $('#ValveStatus'),
        valveType: $('#ValveType'),
        workOrderNumber: $('#WorkOrderNumber')
      };
      townStateServiceUrl = $('#TownStateServiceUrl').val();
      val.initCriticalNotesHiding();
      val.initControlsCrossing();
      val.initValveStatus();
    },

    getAddress: function() {
      // Okay so if there's a coordinate already selected, auto-populating the address
      // and searching will cause the existing coordinate to not be displayed. 
      if (ELEMENTS.coordinate.val()) {
        return null;
      }
      var streetNumber = ELEMENTS.streetNumber.val();
      var street = ELEMENTS.street.find('option:selected').text();
      // townName element only exists on edit view.
      var selectedTown = ELEMENTS.town.find('option:selected');
      var town = ELEMENTS.townName.val() || selectedTown.text();
      // We need state from somewhere or else this doesn't work out very well.
      // The State element only exists on the NEW view and is a dropdown,
      // The StateAbbreviation element only exists on the EDIT view and is a hidden field.
      var state = $('option:selected', ELEMENTS.state).text() || ELEMENTS.stateAbbreviation.val();

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

      return streetNumber + ' ' + street + ', ' + town + ' ' + state;
    },

    initCriticalNotesHiding: function() {
      ELEMENTS.isCritical.on('change', val.onIsCriticalChecked);
      val.onIsCriticalChecked();
    },

    initControlsCrossing: function() {
      ELEMENTS.controlsCrossing.on('change', val.onControlsCrossingChecked);
      val.onControlsCrossingChecked();
    },

    initValveStatus: function() {
      ELEMENTS.status.on('change', val.onStatusChange);
      ELEMENTS.status.on('change', function () {
          hasStatusChanged = true;  // storing the state only if the status is changed, cannot add this to existing status onStatusChange function as it is initiated when the page loads 
      });
      val.onStatusChange();
    },

    onControlsCrossingChecked: function() {
      var controlsCrossingIsChecked = ELEMENTS.controlsCrossing.is(':checked');
      var mainCrossingsParent = ELEMENTS.mainCrossings.closest('.field-pair');
      mainCrossingsParent.toggle(controlsCrossingIsChecked);
      var mainCrossingOperatingCenterParent = ELEMENTS.mainCrossingOperatingCenter.closest('.field-pair');
      mainCrossingOperatingCenterParent.toggle(controlsCrossingIsChecked);
      // display the main crossings

      // valves that control crossings are required to enter critical notes about it.
      // critical notes can only be entered if the critical checkbox is checked. So we
      // check the box for them if it's not already checked.
      if (controlsCrossingIsChecked) {
        ELEMENTS.isCritical.prop('checked', true);
        ELEMENTS.isCritical
          .change(); // Need to fire the event since changing the prop doesn't automatically do it.
      }
    },

    onIsCriticalChecked: function() {
      // NOTE: The criticalNotes textbox is disabled so its value doesn't get posted
      //       back to the server, but allows for the value to stay there on the client.
      var parent = ELEMENTS.criticalNotes.closest('.field-pair');
      if (ELEMENTS.isCritical.is(':checked')) {
        parent.show();
        ELEMENTS.criticalNotes.prop('disabled', false);
      } else {
        parent.hide();
        ELEMENTS.criticalNotes.prop('disabled', true);
      }
    },

    onStatusChange: function () {
      const selectedStatusId = parseInt(ELEMENTS.status.val(), 10);
      const shouldShowDateRetiredField = showDateRetiredForStatuses.some(status => status.id === selectedStatusId);

      Application.toggleField(ELEMENTS.dateRetired, shouldShowDateRetiredField);
    },

    validateWorkOrderNumber: function (value, element) {
        if (!hasStatusChanged) return true;
        const selectedStatusId = parseInt(ELEMENTS.status.val(), 10);
        const showWorkOrderError = workOrderStatuses.some(status => status.id === selectedStatusId);
        if (!showWorkOrderError) return true;
        if (!value) return false;
        return true;
    },

    validateCriticalNotes: function(criticalNotesValue, element) {
      // NOTE: This validation will not run when the CriticalNotes field
      // is disabled.
      if (criticalNotesValue && !ELEMENTS.isCritical.is(':checked')) {
        return false;
      }
      return true;
    },

    validateFunctionalLocation: function(sapFunctionalLocationIdValue, element) {
      // validateFunctionalLocationId is required when OperatingCenter.IsContractedOperations == FALSE AND OperatingCenterSAPEnabled == TRUE

      // Cut out early because OperatingCenter is a required field and we don't wanna throw errors
      // when they haven't selected an operating center yet.
      var opc = ELEMENTS.operatingCenter.val();
      if (!opc) {
        return true;
      }

      // Always valid if it has a value.
      if (sapFunctionalLocationIdValue) {
        return true;
      }

      var isSAPEnabledServiceUrl = $('#IsSAPEnabledServiceUrl').val();
      var isValid;

      $.ajax({
        url: isSAPEnabledServiceUrl,
        data: {
          id: opc
        },
        async: false, // So this just goes.
        type: 'GET',
        success: function(result) {
          isValid = !result.IsSAPEnabled;
        },
        error: function() {
          alert("Something went wrong when validating the Functional Location.");
        }
      });
      return isValid;
    },
    validateTurns: function() {
      var valveTypesRequired = ['1', '2', '5', '11']; // BALL = 1, BUTTERFLY = 2, GATE = 5, TAPPING = 11;
      if ((ELEMENTS.valveStatus.val() === '1' || ELEMENTS.valveStatus.val() === '8') &&
        valveTypesRequired.indexOf(ELEMENTS.valveType.val()) > -1 &&
        ELEMENTS.turns.val() === '')
        return false;
      return true;
    }
  }

  $(document).ready(val.init);

  return val;
})(jQuery);
