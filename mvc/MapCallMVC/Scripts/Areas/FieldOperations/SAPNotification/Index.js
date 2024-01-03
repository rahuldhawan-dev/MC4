// NOTE: Ross wants to know why this is using the video picker html/css/classes. -Ross 1/10/2018
var SAPNotificationIndex = (function ($) {
  var currentDialog = $('<div class="video-picker" style="height:300px;">' +
    '<form class="no-double-submit"><div class="vp-title">' +
    '<div><h3 class="vp-title-text">SAP Notification</h3></div>' +
    '<div><button class="vp-close">X</button></div>' +
    '</div>' +
    '<div><span class="vp-message">Remarks:</span></div>' +
    '<div><textarea class="remarks-text" style="width:100%;height:200px;" rows="10"></textarea></div>' +
    '<div style="text-align:right;"><button class="vp-confirm-button" style="margin:3px;"></span><button style="margin:3px;" class="vp-close" id="sap-dialog-cancel-button">Cancel</button></div>' +
    "</form></div>");
  currentDialog.find('.vp-close')
    .click(function () {
      currentDialog.jqmHide();
      return false;
    });
  currentDialog.hide();

  var sapNotifications = {
    init: function() {
      $(document.body).append(currentDialog);
      $('button[class=cancel-notification]')
        .click(function() {
            sapNotifications.showConfirmDialog('Cancel Notification', 'Cancel SAP Notification?', this.parentElement);
          return false;
        });
      $('button[class=complete-notification]')
        .click(function () {
          sapNotifications.showConfirmDialog('Complete Notification', 'Complete SAP Notification?', this.parentElement);
          return false;
        });
      $('button[class=vp-confirm-button]')
        .click(function () {
          // no-double-submit
          $('button[class=vp-confirm-button]').attr('disabled', 'disabled');
        });
    },
    showConfirmDialog: function(confirmButtonText, msg, form) {
      currentDialog.jqm({ modal: true }).jqmShow();
      currentDialog.find('.vp-title-text').html(msg);
      currentDialog.find('.remarks-text').val('');
      currentDialog.find('.vp-confirm-button').text(confirmButtonText).click(function () {
        var remarks = currentDialog.find('.remarks-text').val();
        if (remarks === "" || remarks.length < 5) {
          alert('Please enter remarks.');
          $('button[class=vp-confirm-button]').prop('disabled', false);
          return false;
		}
		// Why is this searching for the Remarks element by index? -Ross 12/27/2017
        form.elements[0].value = remarks;
        form.submit();
        return true;
      });
    }
  };

  sapNotifications.init();
  return sapNotifications;
})(jQuery);