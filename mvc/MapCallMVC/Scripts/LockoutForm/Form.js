var LockoutForm = (function ($) {
  var ELEMENTS = {
    ADDRESS: '#Address',
    ANSWERS: "select[name$='.Answer']",
    COMMENTS: "input[name$='.Comments']",
    CONTRACTOR: '#Contractor',
    CONTRACTOR_LOCKOUT: '#ContractorLockOutTagOut',
    CONTRACTOR_NAME_TOGGLE: '#ContractorNameToggle',
    CONTRACTOR_NAME: '#ContractorName',
    EQUIPMENT: '#Equipment',
    EQUIPMENT_LINK: '#equipmentShowDocuments',
    EQUIPMENT_SHOW_URL: '#EquipmentShowURL',
    EMPLOYEE_ACKNOWLEDGED_TRAINING: '#EmployeeAcknowledgedTraining',
    FACILITY: '#Facility',
    MANAGEMENT: '#management',
    RETURN_TO_SERVICE_DATE: '#ReturnedToServiceDateTime',
    SAME_AS_INSTALLER: '#SameAsInstaller'
  };

  // EVENT HANDLERS

  var answerChange = function (answer) {
    var comment = $('#' + answer.id.replace('__Answer', '__Comments'));
    if ($(answer).val() === 'False') {
      comment.closest('.field-pair').show();
    } else {
      comment.closest('.field-pair').hide();
    }
  };

  var equipmentChange = function(el) {
    //call up the equipment controller to find out if the equipment has an SOP document
    //if so, get and display the link, require them to agree to having read it
    var equipmentId = $(ELEMENTS.EQUIPMENT).val();
    var link = $(ELEMENTS.EQUIPMENT_LINK);

    if (equipmentId === '' || equipmentId === null) {
      link.hide();
      return;
    }
    var docId = 0;
    var equipmentShowURL = $(ELEMENTS.EQUIPMENT_SHOW_URL).val() + '/' + equipmentId;
    $.ajax({
      url: equipmentShowURL + '.json',
      async: false, // So this just goes.
      type: 'GET',
      success: function (result) {
        docId = result.StandardOperatingProcedureDocumentId;
        if (docId > 0) {
          link.attr('href', equipmentShowURL + '#DocumentsTab');
          link.show();
        } else {
          link.hide();
        }
      },
      error: function () {
        alert("Something went wrong finding the SOP for the selected equipment.");
      }
    });
  };

  var contractorLockoutChange = function() {
    $('#contractor').toggle($(ELEMENTS.CONTRACTOR_LOCKOUT).val() === 'True');
  };

  var contractorNameClick = function () {
    var ddl = $(ELEMENTS.CONTRACTOR);
    var txt = $(ELEMENTS.CONTRACTOR_NAME);
    if (ddl.is(':visible')) {
      ddl.val('');
      txt.toggle(true);
      ddl.toggle(false);
      txt.focus();
    } else {
      txt.val('');
      ddl.toggle(true);
      ddl.focus();
      txt.toggle(false);
    }
  };

  var returnToDateClick = function() {
    var returnToDate = $(ELEMENTS.RETURN_TO_SERVICE_DATE);
    if (returnToDate.val() === '') {
      returnToDate.datepicker('setDate', new Date());
    }
  };

  var sameAsInstallerChange = function() {
    var show = $(ELEMENTS.SAME_AS_INSTALLER + ' option:selected').html() === 'No';
    $(ELEMENTS.MANAGEMENT).css({ display: show ? '' : 'none' });
  };

  // END EVENT HANDLERS

  var wireCommentToggles = function() {
    // get all the CreateLockoutFormAnswers and corresponding comments
    var answers = $(ELEMENTS.ANSWERS);

    // add an onchange to them
    for (var i = 0; i < answers.length; i++) {
      var answer = '#' + answers[i].id;
      $(answer).on('change',
        function() {
          answerChange(this);
        });
    }
  };

  // this is your init function.
  $(function() {
    $(ELEMENTS.CONTRACTOR_NAME_TOGGLE).click(contractorNameClick);
    $(ELEMENTS.CONTRACTOR_NAME).toggle(false);
    $(ELEMENTS.RETURN_TO_SERVICE_DATE).click(returnToDateClick);
		$(ELEMENTS.SAME_AS_INSTALLER).change(sameAsInstallerChange);
    sameAsInstallerChange();
    wireCommentToggles();
    $(ELEMENTS.EQUIPMENT).change(equipmentChange);
    equipmentChange();
    $(ELEMENTS.ANSWERS).change();
    $(ELEMENTS.CONTRACTOR_LOCKOUT).change(contractorLockoutChange);
    contractorLockoutChange();
  });

  return {
    validateEmployeeAcknowledgedTraining: function (currentValue, other) {
      if (!$(ELEMENTS.EQUIPMENT_LINK).is(':visible')) {
          return true;
      }
      return $(ELEMENTS.EMPLOYEE_ACKNOWLEDGED_TRAINING).is(':checked');
    },
    validateAnswer: function(answerVal, element) {
      var category = $('#' + element.id.replace('__Answer', '__Category')).val();
      var sameAsInstaller = $('#SameAsInstaller').val();

      // category 2 - return to service required when return to service date entered.
      if (category === '2' && answerVal === '' && $(ELEMENTS.RETURN_TO_SERVICE_DATE).val() !== '') {
        return false;
      } else {
        return true;
      }

      // if same as installer is false and the value hasn't been entered for category 3 (management), then it's required
      if (sameAsInstaller === 'False' && category === '3' && answerVal === '') {
        return false;
      } else {
        return true;
      }
    },
    validateContractorEntered: function () {
      if ($(ELEMENTS.CONTRACTOR_LOCKOUT).val() !== 'True')
        return true;
      return ($(ELEMENTS.CONTRACTOR).val() !== '' || $(ELEMENTS.CONTRACTOR_NAME).val() !== '');
    }
  };
})(jQuery);