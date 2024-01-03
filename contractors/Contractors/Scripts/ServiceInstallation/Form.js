var ServiceInstallation = (function ($) {
  var ELEMENTS = {};
  var URLS = {};
  var btnVerifyMiu1CLicked;
  var miuNumber;
  var SAPErrorCode;

  var si = {
    init: function () {
      URLS = { sapDeviceDetail: '../../SAPDeviceDetail/Index.json' };
      ELEMENTS = {
        ajaxError: $('#ajaxError'),
        ajaxMiu1Error: $('#ajaxMiu1Error'),
        ajaxMiu2Error: $('#ajaxMiu2Error'),
        btnVerify: $('#btnVerify'),
        btnVerifyMiu1: $('#btnVerifyMiu1'),
        btnVerifyMiu2: $('#btnVerifyMiu2'),
        fullForm: $('#fullForm'),
        materialNumber: $('#MaterialNumber'),
        manufacturer: $('#Manufacturer'),
        meterSerialNumber: $('#MeterSerialNumber'),
        meterSize: $('#MeterSize'),
        meterManufacturerSerialNumber: $('#MeterManufacturerSerialNumber'),
        meterDeviceCategory: $('#MeterDeviceCategory'),
        register1RFMIU: $('#Register1RFMIU'),
        register1Size: $('#Register1Size'),
        register1Dials: $('#Register1Dials'),
        register1UnitOfMeasure: $('#Register1UnitOfMeasure'),
        register1ReadType: $('#Register1ReadType'),
        register1CurrentRead: $('#Register1CurrentRead'),
        register1EncoderId: $('#Register1TPEncoderID'),
        register1DeviceCategory: $('#Register1DeviceCategory'),
        register2RFMIU: $('#Register2RFMIU'),
        register2Size: $('#Register2Size'),
        register2Dials: $('#RegisterTwoDials'),
        register2UnitOfMeasure: $('#Register2UnitOfMeasure'),
        register2ReadType: $('#Register2ReadType'),
        register2CurrentRead: $('#Register2CurrentRead'),
        register2EncoderId: $('#Register2TPEncoderID'),
        register2DeviceCategory: $('#Register2DeviceCategory'),
        save: $('#NSISave'),
        serviceType: $('#ServiceType'),
        workOrder: $('#WorkOrder')
      };
      ELEMENTS.meterSerialNumber.on('blur', si.onMeterSerialNumberBlur);
      ELEMENTS.register1RFMIU.on('blur', si.onRegister1RFMIUBlur);
      ELEMENTS.register2RFMIU.on('blur', si.onRegister2RFMIUBlur);
      ELEMENTS.btnVerify.on('click', si.onBtnVerifyClick);
      ELEMENTS.btnVerifyMiu1.on('click', si.onBtnVerifyMiu1Click);
      ELEMENTS.btnVerifyMiu2.on('click', si.onBtnVerifyMiu2Click);
      ELEMENTS.save.prop('disabled', true);
      if (ELEMENTS.meterSerialNumber.val() === '')
        ELEMENTS.fullForm.toggle(false);
    },
    onBtnVerifyClick: function () {
      ELEMENTS.ajaxError.text('');
      if (ELEMENTS.workOrder.val() !== '' && ELEMENTS.meterSerialNumber.val() !== '') {
        $.getJSON(
          (window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\//))
          ? '../' + URLS.sapDeviceDetail
          : URLS.sapDeviceDetail,
          $.param({
            meterSerialNumber: ELEMENTS.meterSerialNumber.val(),
            workOrderID: ELEMENTS.workOrder.val(),
            DeviceType: 'Z',
            IsMeter: true
          }),
          ServiceInstallation.search_callBack
        );
      } else {
        ELEMENTS.ajaxError.text('Please enter a work orderID and a Meter Manufacturer Serial Number.');
      }
    },
    onBtnVerifyMiu1Click: function () {
        btnVerifyMiu1CLicked = true;
        ELEMENTS.save.prop('disabled', false);
        ELEMENTS.ajaxMiu1Error.text('');
        miuNumber = ELEMENTS.register1RFMIU.val();
        si.onBtnVerifyMiuClick();
    },
    onBtnVerifyMiu2Click: function () {
        btnVerifyMiu1CLicked = false;
        ELEMENTS.save.prop('disabled', false);
        ELEMENTS.ajaxMiu2Error.text('');
        miuNumber = ELEMENTS.register2RFMIU.val();
        si.onBtnVerifyMiuClick();
    },
    onBtnVerifyMiuClick: function () {
        if (miuNumber !== '') {
            $.getJSON(
                (window.location.pathname.toUpperCase().match(/\/NEW\/|\/EDIT\//))
                ? '../' + URLS.sapDeviceDetail
                : URLS.sapDeviceDetail,
                $.param({
                    meterSerialNumber: miuNumber,
                    workOrderID: ELEMENTS.workOrder.val(),
                    DeviceType: 'G'
                }),
                ServiceInstallation.miu_callBack
            );
        } else {
            if (btnVerifyMiu1CLicked) {
                ELEMENTS.ajaxMiu1Error.text('Please enter Register 1 MIU Number.');
                ELEMENTS.ajaxMiu1Error.css('color', 'red');
            } else {
                ELEMENTS.ajaxMiu2Error.text('Please enter Register 2 MIU Number.');
                ELEMENTS.ajaxMiu2Error.css('color', 'red');
            }
        }
    },
    onMeterSerialNumberBlur: function () {
      if (ELEMENTS.meterSerialNumber.val() === '') {
        ELEMENTS.save.prop('disabled', true);
      }
      ELEMENTS.fullForm.toggle(false);
    },
    search_callBack: function (d) {
      var result = d.EquipmentData[0];
      if (result.ReturnStatuses.length > 0) {
          SAPErrorCode = result.ReturnStatuses[0].ReturnStatusDescription;
      }
      ELEMENTS.ajaxError.text(SAPErrorCode);
      ELEMENTS.save.prop('disabled', false);
      ELEMENTS.fullForm.toggle(true);
      ELEMENTS.meterManufacturerSerialNumber.val(result.ManufacturerSerial);
      ELEMENTS.manufacturer.val(result.Manufacturer);
      ELEMENTS.serviceType.val(result.MapCallServiceType);
      ELEMENTS.meterSize.val(result.MapCallMeterSize);
      ELEMENTS.meterDeviceCategory.val(result.DeviceCategory);
      ELEMENTS.materialNumber.val(result.MaterialNumber);
      // Register 1 details
      ELEMENTS.register1RFMIU.val(result.MUINumber1);
      ELEMENTS.register1EncoderId.val(result.EncoderId1);
      ELEMENTS.register1Size.val(result.MapCallMeterSize);
      ELEMENTS.register1DeviceCategory.val(result.MIU1DeviceCategory);
      if (result.RegisterDetails.length > 0) {
          ELEMENTS.register1Dials.val(result.RegisterDetails[0].Dials);
          ELEMENTS.register1UnitOfMeasure.val(result.RegisterDetails[0].UnitOfMeasure);
          ELEMENTS.register1CurrentRead.val(result.RegisterDetails[0].Read);
          if (result.RegisterDetails[0].ReadType !== null && result.RegisterDetails[0].ReadType !== '') {
              ELEMENTS.register1ReadType.val(result.RegisterDetails[0].ReadType);
          }
      }

      // Register 2 details
      ELEMENTS.register2RFMIU.val(result.MUINumber2);
      ELEMENTS.register2EncoderId.val(result.EncoderId2);
      ELEMENTS.register2DeviceCategory.val(result.MIU2DeviceCategory);
      if (result.RegisterDetails.length > 1) {
          ELEMENTS.register2Dials.val(result.RegisterDetails[1].Dials);
          ELEMENTS.register2UnitOfMeasure.val(result.RegisterDetails[1].UnitOfMeasure);
          ELEMENTS.register2CurrentRead.val(result.RegisterDetails[1].Read);
          if (result.RegisterDetails[1].ReadType !== null && result.RegisterDetails[1].ReadType !== '') {
              ELEMENTS.register2ReadType.val(result.RegisterDetails[1].ReadType);
          }
      }
    },
    miu_callBack: function (d) {
        var result = d.EquipmentData[0];
        if (result.ReturnStatuses.length > 0) {
            SAPErrorCode = result.ReturnStatuses[0].ReturnStatusDescription;
        }
        if (btnVerifyMiu1CLicked) {
            ELEMENTS.ajaxMiu1Error.text(SAPErrorCode);
            ELEMENTS.ajaxMiu1Error.css('color', 'red');
            ELEMENTS.register1DeviceCategory.val(result.DeviceCategory);
            if (result.ReadType !== null && result.ReadType !== '') {
                ELEMENTS.register1ReadType.val(result.ReadType);
            }
            return;
        }
        ELEMENTS.ajaxMiu2Error.text(SAPErrorCode);
        ELEMENTS.ajaxMiu2Error.css('color', 'red');
        ELEMENTS.register2DeviceCategory.val(result.DeviceCategory);
        if (result.ReadType !== null && result.ReadType !== '') {
            ELEMENTS.register2ReadType.val(result.ReadType);
        }
        return;
    },
    validateReading1: function (read, element) {
      if (read.length !== parseInt($('#Register1Dials').val()))
        return false;
      return $.isNumeric(read);
    },
    validateReading2: function (read, element) {
      if ($('#RegisterTwoDials').val() === '')
        return true;
      if (read.length !== parseInt($('#RegisterTwoDials').val()))
        return false;
      return $.isNumeric(read);
    },
    onRegister1RFMIUBlur: function () {
        ELEMENTS.save.prop('disabled', true);
        ELEMENTS.ajaxMiu1Error.text('Please Verify the MUI');
        ELEMENTS.ajaxMiu1Error.css('color', 'red');
    },
    onRegister2RFMIUBlur: function () {
        ELEMENTS.save.prop('disabled', true);
        ELEMENTS.ajaxMiu2Error.text('Please Verify the MUI');
        ELEMENTS.ajaxMiu2Error.css('color', 'red');
    }
  };

  $(document).ready(si.init);
  return si;
})(jQuery);