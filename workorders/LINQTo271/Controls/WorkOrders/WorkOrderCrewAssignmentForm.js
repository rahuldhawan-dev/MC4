var WorkOrderCrewAssignmentForm = {
  initialize: function() {
    // need the "fn.call()" dance here to ensure that 'this' means what we need
    // it to
    var that = this;
    getServerElementById('txtEmployeesOnJob').keydown(function(e) {
      that.txtEmployeesOnJob_keydown.call(that, e);
    });
  },

  ///////////////////////////////////////////////////////////////////////////
  //////////////////////////////EVENT HANDLERS///////////////////////////////
  ///////////////////////////////////////////////////////////////////////////
  txtEmployeesOnJob_keydown: function(e) {
    this.restrictToNumbers(e);
    if (!e.isDefaultPrevented()) {
      var text = $(e.target).val();
      var keyCode = e.keyCode < 58 ? e.keyCode : e.keyCode - 48;
      var chr = String.fromCharCode(keyCode);

      if ((parseFloat(text + chr, 10) % 0.5) != 0) {
        e.preventDefault();
      }
    }
  },

  lbEndDate_click: function(lb) {
    return this.validateForEnd(lb);
  },

  lbUpdate_click: function(lb) {
    return this.validateForUpdate(lb);
  },

  ///////////////////////////////////////////////////////////////////////////
  ////////////////////////////////UI HELPERS/////////////////////////////////
  ///////////////////////////////////////////////////////////////////////////
  hasOpenAssignments: function() {
    var table = getServerElementById('gvCrewAssignments')[0];
    for (var i = table.rows.length - 1; i > 0; --i) {
      var row = table.rows[i];

      if (isNaN(Date.parse($(row.cells[5]).text())) && !isNaN(Date.parse($(row.cells[4]).text()))) {
        return true;
      }
    }
    return false;
  },

  restrictToNumbers: function(e) {
    if ((e.keyCode < 48 || e.keyCode > 57) &&    // number keys
      (e.keyCode < 96 || e.keyCode > 105) &&   // number keys (keypad)
        e.keyCode != 190 && e.keyCode != 110 && // '.' keys
        e.keyCode != 8 && e.keyCode != 46) {    // backspace and delete
      e.preventDefault();
      // if '.', make sure it's only entered into the field once
    } else if ((e.keyCode == 110 || e.keyCode == 190) && $(e.target).val().indexOf('.') > -1) {
      e.preventDefault();
    }
  },

  validateEmployeesOnJob: function(btn, query) {
    var row = getParentRow(btn);
    var txtEmployeesOnJob = $(query, row);
    var employees = txtEmployeesOnJob.val();
    if (employees == '') {
      $(btn).toggle(true);
      alert('Please enter the number of employees on the crew.  Half (.5) values may be used to denote someone who left before the crew was finished working the job, though there is a minimum value of 1.');
      txtEmployeesOnJob.focus();
      return false;
    }
    return true;
  },

  validateStartVsEndTime: function(btn) {
    var row = getParentRow(btn);
    var txtStartTime = $('.dateTimePicker', row).first();
    var txtEndTime = $('.dateTimePicker', row).last();

    if (Date.parse(txtStartTime.val()) > Date.parse(txtEndTime.val())) {
      $(btn).toggle(true);
      alert('End time must fall after start time.')
      txtEndTime.focus();
      return false;
    }
    return true;
  },

  validateForEnd: function (btn) {
    $(btn).toggle(false);
    if (!WorkOrderCrewAssignmentForm.validateEmployeesOnJob(btn, 'input[type="text"]')) {
      return false;
    }
    $(btn).after('submitting...');
    return true;
  },

  validateForUpdate: function (btn) {
    $(btn).toggle(false);
    if (!WorkOrderCrewAssignmentForm.validateEmployeesOnJob(btn, 'txtEmployeesOnJob')) {
      return false;
    }
    if (!WorkOrderCrewAssignmentForm.validateStartVsEndTime(btn)) {
      return false;
    }
    $(btn).after('submitting...');
    return true;
  }
};
