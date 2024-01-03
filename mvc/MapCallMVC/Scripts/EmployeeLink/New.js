var EmployeeLinkNew = {  
  selectors: {
    pnlLinkEmployee: 'div#pnlLinkEmployee',
    linkEmployees: 'button#linkEmployees',
    linkEmployeesForm: 'form#linkEmployees',
    ddlOpCenter: 'select#OperatingCenter',
    listEmployeeIds: '#EmployeeIds'
  },

  initialize: function() {
    $(EmployeeLinkNew.selectors.linkEmployeesForm).submit(function() {
      EmployeeLinkNew.toggleLinkEmployeesButton(false);
      return true;
    });

    $(EmployeeLinkNew.selectors.ddlOpCenter).change(function() {
      EmployeeLinkNew.filterEmployees();
    });

    // Call this once in case of back button usage and pre-selected values.
    EmployeeLinkNew.filterEmployees();
  },

  toggleLinkEmployeesButton: function(show) {
    $(EmployeeLinkNew.selectors.linkEmployees).prop("disabled", !show);
  },
  filterEmployees: function() {
      // This is a temporary hack that should be replaced by proper cascading 
      // functionality.

      var selectedOp = $(EmployeeLinkNew.selectors.ddlOpCenter).find(':selected');
      var opCode;
      if (selectedOp.val()) {
          opCode = selectedOp.text().split('-')[0];
          opCode = $.trim(opCode);
      }

      $(EmployeeLinkNew.selectors.listEmployeeIds).find('label').each(function () {
          var $this = $(this);
          var $parent = $this.parent();
          if (opCode) {
              if ($this.text().indexOf(opCode) > -1) {
                  $parent.show();
              } else {
                  $parent.hide();
              }
          } else {
              $parent.show();
          }
      });
  }
};

$(document).ready(EmployeeLinkNew.initialize);