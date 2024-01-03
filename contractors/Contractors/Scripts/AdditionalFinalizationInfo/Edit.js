var AdditionalFinalizationInfoEdit = {
  OPTION_VALUES: {
      otherDeliveryMethod: '3'
    },
  MINIMUM_FLUSH_TIME: 30,
  ELEMENTS: {
      pitcherFilterDeliveredToCustomerOptions: '#pitcher-filter-delivered',
      pitcherFilterDelivered: '#HasPitcherFilterBeenProvidedToCustomer',
      pitcherFilterCustomerDeliveryMethod: '#PitcherFilterCustomerDeliveryMethod',
      pitcherFilterDeliveryOther: '#pitcher-filter-delivery-other',
      flushTimeBelowMinimumMessage: '#flush-time-below-minimum-message',
      InitialServiceLineFlushTime: '#InitialServiceLineFlushTime'
    },
  ajaxSuccess: function () {
    window.location.reload(false);
  },
  ajaxFailure: function (msg) {
    Main.displayUserError('Error: ' + msg.statusText);
    return false;
    },
    onPitcherFilterProvidedToCustomer: function () {
        if ($(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDelivered).val() === 'True') {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDeliveredToCustomerOptions).show();
        } else {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDeliveredToCustomerOptions).hide();
        }
    },
    onPitcherFilterCustomerDeliveryMethod: function () {

        if ($(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterCustomerDeliveryMethod).val() === AdditionalFinalizationInfoEdit.OPTION_VALUES.otherDeliveryMethod) {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDeliveryOther).show();
        } else {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDeliveryOther).hide();
        }
    },
    onInitialServiceLineFlushTimeBlur: function () {
        if ((parseInt($(AdditionalFinalizationInfoEdit.ELEMENTS.InitialServiceLineFlushTime).val()) || 0 ) < AdditionalFinalizationInfoEdit.MINIMUM_FLUSH_TIME) {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.flushTimeBelowMinimumMessage).show();
        } else {
            $(AdditionalFinalizationInfoEdit.ELEMENTS.flushTimeBelowMinimumMessage).hide();
        }
    },
    init: function () {
        $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterDelivered).change(AdditionalFinalizationInfoEdit.onPitcherFilterProvidedToCustomer);
        AdditionalFinalizationInfoEdit.onPitcherFilterProvidedToCustomer();
        $(AdditionalFinalizationInfoEdit.ELEMENTS.pitcherFilterCustomerDeliveryMethod).change(AdditionalFinalizationInfoEdit.onPitcherFilterCustomerDeliveryMethod);
        AdditionalFinalizationInfoEdit.onPitcherFilterCustomerDeliveryMethod();
        $(AdditionalFinalizationInfoEdit.ELEMENTS.InitialServiceLineFlushTime).blur(AdditionalFinalizationInfoEdit.onInitialServiceLineFlushTimeBlur);
        AdditionalFinalizationInfoEdit.onInitialServiceLineFlushTimeBlur();
    }
};
$(document).ready(AdditionalFinalizationInfoEdit.init);