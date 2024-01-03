(function ($) {

  var $newSerialNumber = $('#NewSerialNumber');
  var $newSNValidation = $('<div class="field-validation-error">A meter change out record already uses this serial number.</div>');
  var $assignedContractorMeterCrew = $('#AssignedContractorMeterCrew');
  var $calledInByContractorMeterCrew = $('#CalledInByContractorMeterCrew');
  var $clickAdvantexUpdated = $('#ClickAdvantexUpdated');
  var $dateScheduled = $('#DateScheduled');
  var $dateStatusChanged = $('#DateStatusChanged');
  var $isNeptuneRFOnly = $('#IsNeptuneRFOnly');
  var $isHotRodRFOnly = $('#IsHotRodRFOnly');
  var $meterChangeOutStatus = $('#MeterChangeOutStatus');
  var $meterLocation = $('#MeterLocation');
  var $meterSupplementalLocation = $('#MeterSupplementalLocation');
  var $meterDirection = $('#MeterDirection');
  var $outRead = $('#OutRead');
  var $operatedPointOfControlAtAnyValve = $('#OperatedPointOfControlAtAnyValve');
  var $removedSerialNumber = $('#Display_RemovedSerialNumber');
  var $rfLocation = $('#RFLocation');
  var $rfSupplementalLocation = $('#RFSupplementalLocation');
  var $rfDirection = $('#RFDirection');
  var $startRead = $('#StartRead');
  var $servicePhone = $('#ServicePhone');

  var MeterChangeOutStatuses = {
    Changed: 3,
    Scheduled: 10
  };

  var methods = {
    init: function () {
      $newSerialNumber.change(methods.validateNewSerialNumber);
      $newSNValidation.hide();
      $newSerialNumber.parent().append($newSNValidation);
      $assignedContractorMeterCrew.change(methods.assignedContractorMeterCrewChange);
      $calledInByContractorMeterCrew.change(methods.calledInByChange);
      $isNeptuneRFOnly.change(methods.isNeptuneRFOnlyChange);
      $isHotRodRFOnly.change(methods.isHotRodRFOnlyChange);
      $meterLocation.blur(methods.meterLocationBlur);
      $meterDirection.change(methods.meterDirectionChange);
      $operatedPointOfControlAtAnyValve.change(methods.operatedPointOfControlAtAnyValveChange);
      $startRead.blur(methods.startReadBlur);
      methods.focusOnFieldsUsersNeedToEdit();
    },
     
    assignedContractorMeterCrewChange: function () {
      $meterChangeOutStatus.val(MeterChangeOutStatuses.Scheduled);
    },
    calledInByChange: function () {
      $meterChangeOutStatus.val(MeterChangeOutStatuses.Changed);
      $outRead.focus();
      $dateStatusChanged.val($.datepicker.formatDate('mm/dd/yy', new Date()));
      $clickAdvantexUpdated.val('False');
    },
    isNeptuneRFOnlyChange: function () {
      if ($isNeptuneRFOnly.val() === 'True')
        $newSerialNumber.val($removedSerialNumber.val().substring(4, 12));
    },
    isHotRodRFOnlyChange: function () {
      if ($isHotRodRFOnly.val() === 'True')
        $newSerialNumber.val($removedSerialNumber.val().substring(4, 12));
    },
    focusOnFieldsUsersNeedToEdit: function () {
      // set the focus based on if this has been scheduled or not yet
      if ($dateScheduled.val() === '') {
        $calledInByContractorMeterCrew.focus();
        $servicePhone.focus();
      } else {
        $calledInByContractorMeterCrew.focus();
        if ($startRead.val() === '' || $startRead.val() === '0')
          $startRead.val('000000');
      }
    },
    meterLocationBlur: function () {
      var outsideLocations = [2, 7, 8, 9, 10, 11, 12, 13, 14, 15, 19];
      if ($meterLocation.val() !== '' && outsideLocations.indexOf(parseInt($meterLocation.val())) >= 0) {
        $meterSupplementalLocation.val(2);
        $rfSupplementalLocation.val(2);
      }
      else {
        $meterSupplementalLocation.val(1);
        $rfSupplementalLocation.val(1);
      }
      $rfLocation.val($meterLocation.val());
      $meterDirection.focus();
    },
    meterDirectionChange: function () {
      $rfDirection.val($meterDirection.val());
    },
    operatedPointOfControlAtAnyValveChange: function () {
      $startRead.focus();
    },
    startReadBlur: function () {
      $meterLocation.focus();
    },
    validateNewSerialNumber: function () {
      var serviceUrl = $('#ValidateNewSerialNumberService').val();
      var newSerialNumber = $('#NewSerialNumber').val();

      // Don't care if the serial number is empty, that's just always gonna return true 99% of the time.
      if (!newSerialNumber) {
        return;
      }

      $.ajax({
        url: serviceUrl,
        data: {
          newSerialNumber: $newSerialNumber.val()
        },
        async: false, // So this just goes.
        type: 'GET',
        success: function (result) {
          // Only display when the serial number is not unique.
          $newSNValidation.toggle(!result.isUnique);
        },
        error: function () {
          alert("An error occurred while attempting to match this serial number.");
        }
      });
    }
  }

  $(document).ready(methods.init);

})(jQuery);