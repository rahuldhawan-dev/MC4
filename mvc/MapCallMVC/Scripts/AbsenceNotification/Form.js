var AbsenceNotificationForm = (function($) {

  var currentAjaxRequest;
  var EMPLOYEE = $('#Employee');  
  var RESTRICTION_NOTIFICATION = $('#RestrictionNotification');
  var serviceUrl = $('#DoctorsNoteRestrictionUrl').val();
  return {

    init: function() {
      EMPLOYEE.change(AbsenceNotificationForm.onEmployeeChange);
      // Fire this for the edit page since an employee will be selected.
      AbsenceNotificationForm.onEmployeeChange();
    },

    onEmployeeChange: function() {
      var employeeId = EMPLOYEE.val();
      if (!employeeId) {
        RESTRICTION_NOTIFICATION.hide();
        return;
      }      

      if (currentAjaxRequest) {
        currentAjaxRequest.abort();
      }

      currentAjaxRequest = $.ajax({
        url: serviceUrl,
        data: {
          employeeId: employeeId
        },
        async: false,
        type: 'GET',
        success: function(result) {
          if (result.success && result.hasRestriction) {
            RESTRICTION_NOTIFICATION.html("The selected employee has a one day doctor's note restriction until " + result.restrictionEndDate + '.');
            RESTRICTION_NOTIFICATION.show();
          } else {
            RESTRICTION_NOTIFICATION.hide();
          }
        },
        error: function() {
          RESTRICTION_NOTIFICATION.html('No wrong bad');
          RESTRICTION_NOTIFICATION.show();
        }
      });
    }
  }
})(jQuery);

$(document).ready(AbsenceNotificationForm.init);