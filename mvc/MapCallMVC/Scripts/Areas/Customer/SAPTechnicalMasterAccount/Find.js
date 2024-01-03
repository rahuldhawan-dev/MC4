var SapTechnicalMasterAccountFind = (function($) {
  var ELEMENTS = {};
  var URLS = {};
  var SapTechnicalMasterAccountFindFormId = '#SAPTechnicalMasterAccountForm';

  var stmaf = {
    intialize: function () {
      URLS = { installationSearch: $('#InstallationSearchUrl').val() };
      ELEMENTS = {
        deviceLocation: $('#DeviceLocation'),
        doSAPTechnicalMasterAccountSearch: $(SapTechnicalMasterAccountFindFormId + ' button[id=DoSAPTechnicalMasterAccountSearch]'),
        equipment: $('#SAPEquipmentNumber'),
        equipmentSearch: $(SapTechnicalMasterAccountFindFormId + ' input[id=Equipment]'),
        installationTypeSearch: $(SapTechnicalMasterAccountFindFormId + ' select[id=InstallationType]'),
        installationResults: $(SapTechnicalMasterAccountFindFormId + ' table[id=installationResults]'),
        meterSerialNumber: $('#MeterSerialNumber'),
        premiseNumber: $('#PremiseNumber'),
        premiseNumberSearch: $(SapTechnicalMasterAccountFindFormId + ' input[id=PremiseNumber]'),
        serviceUtilityType: $('#ServiceUtilityType'),
        selectInstallation: $(SapTechnicalMasterAccountFindFormId + ' button[id=selectInstallation]'),
        selectedInstallation: $(SapTechnicalMasterAccountFindFormId + ' input[id=selectedInstallation]'),
        selectedEquipment: $(SapTechnicalMasterAccountFindFormId + ' input[id=selectedEquipment]'),
        selectedDeviceLocation: $(SapTechnicalMasterAccountFindFormId + ' input[id=selectedDeviceLocation]'),
        selectedMeterSerialNumber: $(SapTechnicalMasterAccountFindFormId + ' input[id=selectedMeterSerialNumber]'),
        installation: $('#Installation'),
        selectedPremiseNumber: $(SapTechnicalMasterAccountFindFormId + ' input[id=SelectedPremiseNumber]')
      };
      ELEMENTS.doSAPTechnicalMasterAccountSearch.on('click', stmaf.doSearch_click);
      ELEMENTS.selectInstallation.on('click', stmaf.selectInstallation_click);

      stmaf.setInitialValues();
    },

    doSearch_click: function () {
      $.getJSON(URLS.installationSearch,
        $.param({
          'PremiseNumber': ELEMENTS.premiseNumberSearch.val(),
          'Equipment': ELEMENTS.equipmentSearch.val(),
          'InstallationType': ELEMENTS.installationTypeSearch.val()
        }),
        SapTechnicalMasterAccountFind.search_callBack);
      return false;
    },

    resultsRow_click: function (e) {
      $(e.target.parentElement.parentElement).find('td').css('background-color', '');
      $(e.target.parentElement).find('td').css('background-color', 'orange');
      ELEMENTS.selectedDeviceLocation.val(e.target.parentElement.cells[1].innerHTML);
      ELEMENTS.selectedEquipment.val(e.target.parentElement.cells[2].innerHTML);
      ELEMENTS.selectedInstallation.val(e.target.parentElement.cells[3].innerHTML);
      ELEMENTS.selectedMeterSerialNumber.val(e.target.parentElement.cells[5].innerHTML);
    },

    resultsRow_doubleClick: function (e) {
      stmaf.resultsRow_click(e);
      stmaf.selectInstallation_click();
    },

    search_callBack: function (d) {
      ELEMENTS.installationResults.find('tr:gt(0)').remove();
      for (var x = 0; x < d.Data.length; x++) {
          ELEMENTS.installationResults.find('tbody').append(
          '<tr>' +
          '<td>' + d.Data[x].InstallationType + '</td>' +
          '<td>' + d.Data[x].DeviceLocation + '</td>' +
          '<td>' + d.Data[x].Equipment + '</td>' +
          '<td>' + d.Data[x].Installation + '</td>' +
          '<td>' + d.Data[x].DeviceSerialNumber + '</td>' +
          '<td>' + d.Data[x].MeterSerialNumber + '</td>' +
          '<td>' + d.Data[x].Customer + '</td>' +
          '<td>' + d.Data[x].Phone + '</td>' +
          '<td>' + d.Data[x].Status + '</td>' +
          '<td>' + d.Data[x].Owner + '</td>' +
          '<td>' + d.Data[x].MeterSize + '</td>' +
          '<td>' + ((d.Data[x].BillingClassification === '') ? '' : d.Data[x].BillingClassification) + '</td>' +
          '</tr>');
      }

      const rows = ELEMENTS.installationResults.find(' > tbody > tr');
      rows.on('click', stmaf.resultsRow_click);
      rows.on('dblclick', stmaf.resultsRow_doubleClick);
    },

    selectInstallation_click: function () {
      if (ELEMENTS.selectedInstallation.val() !== '') {
        stmaf.setParentValue(ELEMENTS.installation, ELEMENTS.selectedInstallation.val());
        stmaf.setParentValue(ELEMENTS.deviceLocation, ELEMENTS.selectedDeviceLocation.val());
        stmaf.setParentValue(ELEMENTS.equipment, ELEMENTS.selectedEquipment.val());
        stmaf.setParentValue(ELEMENTS.meterSerialNumber, ELEMENTS.selectedMeterSerialNumber.val());
        //stmaf.setParentValue(ELEMENTS.streetNumber, ELEMENTS.selectedStreetNumber.val());
        //stmaf.setParentValue(ELEMENTS.street, ELEMENTS.selectedStreet.val());
      }
      $(SapTechnicalMasterAccountFindFormId + ' button[class=cancel]').click();
    },

    setParentValue: function (elem, val) {
      elem.val(val);
      elem.blur();
    },

    setInitialValues: function () {
      //grab values from the parent form
      ELEMENTS.premiseNumberSearch.val(ELEMENTS.premiseNumber.val());
      //ELEMENTS.equipmentSearch.val(ELEMENTS.equipment.val());

      if (ELEMENTS.premiseNumber.val() !== '') {
        ELEMENTS.selectedPremiseNumber.val(ELEMENTS.premiseNumber.val());
      }
      // serviceUtilityType.val() is undefined on service page, but is empty string on workorders.
      if (ELEMENTS.serviceUtilityType.val()) {
        ELEMENTS.installationTypeSearch.find('option')
          .filter(function () { return (new RegExp(ELEMENTS.serviceUtilityType.val(), 'i')).test($(this).text()); })
          .prop('selected', true);
      }
    }
  };
  $(document).ready(stmaf.intialize);
  return stmaf;
})(jQuery);