var SewerOpenings = (function() {
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

  var sm = {
    init: function() {
      ELEMENTS = {
        coordinate: $('#Coordinate'),
        criticalNotes: $('#CriticalNotes'),
        dateRetired: $('#DateRetired'),
        isCritical: $('#Critical'),
        operatingCenter: $('#OperatingCenter'),
        sapEquipmentId: $('#SAPEquipmentId'),
        street: $('#Street'),
        status: $('#Status'),
        town: $('#Town'),
        townName: $('#TownName'),
        state: $('#State'),
        taskNumber: $('#TaskNumber')
      };
      townStateServiceUrl = $('#TownStateServiceUrl').val();
      sm.initCriticalNotesHiding();
      sm.initSewerOpeningStatus();
    },

    getAddress: function() {
      // if we already have a coordinate we don't need to look it up.
      if (ELEMENTS.coordinate.val()) {
        return null;
      }

      // townName element only exists on edit view.
      var selectedTown = ELEMENTS.town.find('option:selected');
      var town = ELEMENTS.townName.val() || selectedTown.text();
      var street = ELEMENTS.street.find('option:selected').text();

      var state = ELEMENTS.state.val();

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

      return street + ', ' + town + ' ' + state;
    },

    initCriticalNotesHiding: function() {
      ELEMENTS.isCritical.on('change', sm.onIsCriticalChecked);
      sm.onIsCriticalChecked();
    },

    initSewerOpeningStatus: function() {
      ELEMENTS.status.on('change', sm.onStatusChange);
      ELEMENTS.status.on('change', function () {
          hasStatusChanged = true;  // storing the state only if the status is changed, cannot add this to existing status onStatusChange function as it is initiated when the page loads 
      });
      sm.onStatusChange();
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

    validateTaskNumber: function (value, element) {
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
      // validateFunctionalLocationId is required when OperatingCenter.IsContractedOperations == FALSE AND OperatingCenteRSAPEnabled == TRUE

      // Cut out early beacuse OperatingCenter is a required field and we don't wanna throw errors
      // when they haven't selected an operating center yet.
      var opc = ELEMENTS.operatingCenter.val();
      if (!opc) {
        return true;
      }

      // Always valid if it has a value.
      if (sapFunctionalLocationIdValue) {
        return true;
      }

      var isValid;
      var isSAPEnabledServiceUrl = $('#IsSAPEnabledServiceUrl').val();

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
    }
  }

  $(document).ready(sm.init);

  return sm;
})();
