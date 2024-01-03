const WorkOrderAdditionalFinalizationInfoForm = (function ($, url, storage) {
  const RGX_MAIN_BREAK = /^WATER MAIN BREAK REP(AIR|LACE)$/;
  // list of elements which should have their values persisted in sessionStorage onchange in case the user
  // is logged out or redirected before they're able to finish and hit 'save'
  const elementsToStore = [
    'ddlFinalWorkDescription',
    'ddlFinalCustomerImpactRange',
    'ddlFinalRepairTimeRange',
    'ddlFinalAlertIssued',
    'ddlFinalSignificantTrafficImpact',
    'txtLostWater',
    'txtDistanceFromCrossStreet',
    'txtAppendNotes',
    'ddlPreviousServiceLineMaterial',
    'ddlPreviousServiceLineSize',
    'ddlCompanyServiceLineMaterial',
    'ddlCompanyServiceLineSize',
    'ddlCustomerServiceLineMaterial',
    'ddlCustomerServiceLineSize',
    'txtDoorNoticeLeftDate',
    'txtInitialServiceLineFlushTime',
    'chkHasPitcherFilterBeenProvidedToCustomer',
    'txtDatePitcherFilterDeliveredToCustomer',
    'ddlPitcherFilterCustomerDeliveryMethodId',
    'txtDateCustomerProvidedAWStateLeadInformation'
  ];

  const finalWorkDescriptionIsMainBreak = function (desc) {
    return RGX_MAIN_BREAK.test(
      desc || $('option:selected',
        getServerElementById('ddlFinalWorkDescription')).text());
  };

  const toggleMainBreakInfo = function (show) {
    $('.trFinalMainBreakInfo').toggle(show);
    //$('#mainbreak').toggle(show);
    $('#mainBreakLi').toggle(show);
    if ($('option:selected', getServerElementById('ddlFinalWorkDescription')).text() == 'WATER MAIN BREAK REPLACE') {
      getServerElementById('txtFootageReplaced').removeAttr('disabled');
      getServerElementById('ddlReplacedWith').removeAttr('disabled');
    } else {
      getServerElementById('txtFootageReplaced').attr('disabled', 'disabled');
      getServerElementById('ddlReplacedWith').attr('disabled', 'disabled');
    }
  };

  const onFinalWorkDescriptionChanged = function (ddl) {
    toggleMainBreakInfo(finalWorkDescriptionIsMainBreak($('option:selected', ddl).text()));
  };

  const onHasPitcherFilterBeenProvidedToCustomerClick = (chk) => {
    const delivered = $(chk).is(':checked');
    const rfvDatePitcherFilterDeliveredToCustomer =
      getServerElementById('rfvDatePitcherFilterDeliveredToCustomer');
    const cvPitcherFilterCustomerDeliveryMethod =
      getServerElementById('cvPitcherFilterCustomerDeliveryMethod');

    $('tr.pitcher-delivery-details').toggle(delivered);
    if (rfvDatePitcherFilterDeliveredToCustomer.length) {
      ValidatorEnable(rfvDatePitcherFilterDeliveredToCustomer[0], delivered);
    }
    if (cvPitcherFilterCustomerDeliveryMethod.length) {
      ValidatorEnable(cvPitcherFilterCustomerDeliveryMethod[0], delivered);
    }

    if (!delivered) {
      getServerElementById('txtDatePitcherFilterDeliveredToCustomer').val('');
      getServerElementById('ddlPitcherFilterCustomerDeliveryMethod').val('');
      getServerElementById('txtPitcherFilterCustomerDeliveryOtherMethod').val('');
    }
  };

  const onPitcherFilterCustomerDeliveryMethodChange = (ddl) => {
    const otherSelected = $('option:selected', ddl).text() === 'Other';
    const rfvPitcherFilterCustomerDeliveryOtherMethod =
      getServerElementById('rfvPitcherFilterCustomerDeliveryOtherMethod');

    $('tr.pitcher-delivery-details-other').toggle(otherSelected);
    if (rfvPitcherFilterCustomerDeliveryOtherMethod.length) {
      ValidatorEnable(
        rfvPitcherFilterCustomerDeliveryOtherMethod[0],
        otherSelected);
    }

    if (!otherSelected) {
      getServerElementById('txtPitcherFilterCustomerDeliveryOtherMethod').val('');
    }
  };

  const onInitialServiceLineFlushTime_Change = function(txtInitialServiceLineFlushTime) {
    const flushTime = $(txtInitialServiceLineFlushTime).val();
    if (parseInt(flushTime, 10) < 30) {
      getServerElementById('lblInitialServiceLineFlushTimeBelowMinimum').show();
    } else {
      getServerElementById('lblInitialServiceLineFlushTimeBelowMinimum').hide();
    }
  };

  // sessionStorage keys are
  // "<full url of the current page including query string>_<shortened element id>"
  const getStorageKey = function (elem) {
    return url + '_' + elem;
  };

  const maybeResetStoredValues = function () {
    elementsToStore.forEach(elem => {
      const value = storage.getItem(getStorageKey(elem));

      if (value) {
        getServerElementById(elem).val(value);
      }
    });
  };

  const clearStoredValues = function () {
    elementsToStore.forEach(elem => {
      storage.removeItem(getStorageKey(elem));
    });
  };

  const initializeElementStorage = function () {
    elementsToStore.forEach(elem =>
      getServerElementById(elem).change(evt =>
        storage.setItem(
          getStorageKey(elem),
          $(evt.target).val())));
  };

  return {
    initialize: function () {
      if (finalWorkDescriptionIsMainBreak()) {
        toggleMainBreakInfo(true);
      }
      onHasPitcherFilterBeenProvidedToCustomerClick(
        getServerElementById('chkHasPitcherFilterBeenProvidedToCustomer'));
      onPitcherFilterCustomerDeliveryMethodChange(
        getServerElementById('ddlPitcherFilterCustomerDeliveryMethod'));
      initializeElementStorage();
      maybeResetStoredValues();
      onInitialServiceLineFlushTime_Change(
        getServerElementById('txtInitialServiceLineFlushTime'));
    },

    ddlFinalWorkDescription_Change: function (ddl) {
      onFinalWorkDescriptionChanged(ddl);
    },

    chkHasPitcherFilterBeenProvidedToCustomer_Click: (chk) =>
      onHasPitcherFilterBeenProvidedToCustomerClick(chk),

    ddlPitcherFilterCustomerDeliveryMethod_Change: (ddl) =>
      onPitcherFilterCustomerDeliveryMethodChange(ddl),

    txtInitialServiceLineFlushTime_Change: (txt) =>
      onInitialServiceLineFlushTime_Change(txt),

    lbUpdate_Click: function () {
      // if everything on the current page is valid, we're about to submit the form.  if we're about to
      // submit the form there's no need to continue to persist values to sessionStorage
      if (window.Page_IsValid) {
        clearStoredValues();
      }

      return true;
    },

    validateDistanceFromCrossStreet: function (source, args) {
      const streetOpeningPermitRequired = (getServerElementById('hidStreetOpeningPermitRequired').val().toLowerCase() == 'true');
      const priorityIsEmergency = (getServerElementById('hidPriority').val() == 'Emergency');
      const distanceFromCrossStreet = getServerElementById('txtDistanceFromCrossStreet').val();

      args.IsValid = ((!streetOpeningPermitRequired || !priorityIsEmergency) || distanceFromCrossStreet != '');
    },

    validateRepairTimeRange: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('ddlFinalRepairTimeRange').val() != '');
    },

    validateCustomerImpactRange: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('ddlFinalCustomerImpactRange').val() != '');
    },

    validateCustomerAlert: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('ddlFinalCustomerAlert').val() != '');
    },

    validateSignificantTrafficImpact: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('ddlFinalSignificantTrafficImpact').val() != '');
    },

    validateLostWater: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('txtLostWater').val() != '');
    },

    validateMainBreakRecord: function (source, args) {
      args.IsValid = !finalWorkDescriptionIsMainBreak() ||
        (getServerElementById('gvMainBreak')[0].rows[1].cells.length > 1);
    },

    validatePitcherFilterCustomerDeliveryMethod: (_, args) =>
      args.IsValid = getServerElementById('ddlPitcherFilterCustomerDeliveryMethod').val() !== ''
  };
})(jQuery, window.location.href, window.sessionStorage);
