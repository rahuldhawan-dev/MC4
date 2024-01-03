var WorkOrderMarkoutForm = {
    initialize: function () {
        Sys.Application.add_load(WorkOrderMarkoutForm.getCalendarDateFields);
        Sys.Application.add_load(WorkOrderMarkoutForm.onUpdatePanelPostBack);
  },

  ddlMarkoutType_Changed: function(elem) {
    this.onMarkoutTypeChanged(elem);
    },

    getCalendarDateFields: function () {
        var readyDateEdit = $('input[id*=ccMarkoutReadyDateEdit]');
        var expirationDateEdit = $('input[id*=ccMarkoutExpirationDateEdit]');
        var readyDateNew = $('input[id*=ccMarkoutReadyDate]');
        var expirationDateNew = $('input[id*=ccMarkoutExpirationDate]');


        $(readyDateEdit).datetimepicker({ controlType: 'select'});
        $(expirationDateEdit).datetimepicker({ controlType: 'select'});
        $(readyDateNew).datetimepicker({ controlType: 'select'});
        $(expirationDateNew).datetimepicker({ controlType: 'select'});
    },

  lbInsert_Click: function() {
    return this.onLbInsertClick();
  },

  lbUpdate_Click: function() {
    return this.onLbUpdateClick();
  },

  onMarkoutTypeChanged: function(elem) {
    $(elem).siblings()[($(elem).val() == 38) ? 'show' : 'hide']();
  },

  onUpdatePanelPostBack: function() {
    var elem = getServerElementById('ddlMarkoutTypeEdit');
    WorkOrderMarkoutForm.onMarkoutTypeChanged(elem);
  },

  onLbInsertClick: function() {
    var valid = true;
    valid = this.validateMarkoutNumber(
              getServerElementById('txtMarkoutNumber').val()) &&
            this.validateMarkoutType(
              getServerElementById('ddlMarkoutType').val(),
              getServerElementById('txtNote').val()) &&
            this.validateDateEntered(
              getServerElementById('ccMarkoutReadyDate'), 'Ready Date') &&
            this.validateDateEntered(
              getServerElementById('ccMarkoutExpirationDate'), 'Expiration Date') &&
            this.validateDatesAreInOrder('');
    return valid;
  },

  onLbUpdateClick: function() {
    var valid = true;
    valid = this.validateMarkoutNumber(
              $('input[id*=txtMarkoutNumberEdit]').val()) && 
            this.validateMarkoutType(
              $('select[id*=ddlMarkoutTypeEdit]').val(), 
              $('input[id*=txtNoteEdit]').val()) &&
            this.validateDateEntered(
              $('input[id*=ccMarkoutReadyDate]'), 'Ready Date') &&
            this.validateDateEntered(
              $('input[id*=ccMarkoutExpirationDate]'), 'Expiration Date') &&
            this.validateDatesAreInOrder('Edit');
    return valid;
  },

  validateMarkoutNumber: function(markoutNumber) {
    if (markoutNumber.length == 0) {
      alert('Please enter a Markout Number');
      return false;
    }
    return true;
  },

  validateMarkoutType: function(markoutTypeID, note) {
    if (markoutTypeID == 38 && note.length < 10) {
      alert('Please enter valid notes for the Markout Type');
      return false;
    }
    if (markoutTypeID == "") {
      alert('Please select the Markout Type');
      return false;
    }
    return true;
  },

  validateDateEntered: function (date, dateField)
  {
    if (date.val() === '') {
      alert('Please enter a valid ' + dateField);
      date.focus();
      return false;
    }
    return true;
  },

  /// controlMode - appends this value to the control id. E.g. Edit => ccMarkoutReadyDateEdit
  validateDatesAreInOrder: function(controlMode) {
    var requestDate = $('input[id*=ccDateOfRequest' + controlMode + ']');
    var readyDate = $('input[id*=ccMarkoutReadyDate' + controlMode + ']');
    var expirationDate = $('input[id*=ccMarkoutExpirationDate' + controlMode + ']');
    if (readyDate.length === 0) {
      return true;
    }
    if (new Date(readyDate.val()) > new Date(expirationDate.val())) {
      alert('Ready Date cannot be after the Expiration Date.');
      expirationDate.focus();
      return false;
    }
    if (new Date(requestDate.val()) > new Date(readyDate.val())) {
      alert('Request Date cannot be after the Ready Date.');
      readyDate.focus();
      return false;
    }
    }
};