/// <reference path="../Views/WorkOrders/Input/WorkOrderInputDetailView.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />
/// <reference path="../Common/LatLonPicker.js" />

var WorkOrderInputTests = {
  WorkOrderInputDetailViewTest: {
    ////////////////////////////////INPUT/EDIT////////////////////////////////

    /* EVENT HANDLERS */
    testBtnSave_ClickCallsValidateForm: function() {
      var called = false;
      var oldFn = validateForm;
      validateForm = function() { called = true; };

      btnSave_Click();
      Assert.isTrue(called);

      validateForm = oldFn;
    },

    testDDLTown_ChangeCallsOnTownChanged: function() {
      var called = false;
      var oldFn = onTownChanged;
      onTownChanged = function() { called = true; };

      ddlTown_Change();

      Assert.isTrue(called);

      onTownChanged = oldFn;
    },

    testDDLTownSection_ChangeCallsOnTownChanged: function() {
      var called = false;
      var oldFn = onTownChanged;
      onTownSectionChanged = function() { called = true; };

      ddlTownSection_Change();

      Assert.isTrue(called);

      onTownSectionChanged = oldFn;
    },

    testDDLStreet_ChangeCallsOnStreetChanged: function() {
      var called = false;
      var oldFn = onStreetChanged;
      onStreetChanged = function() { called = true; };

      ddlStreet_Change();

      Assert.isTrue(called);

      onStreetChanged = oldFn;
    },

    testDDLNearestCrossStreet_ChangeCallsOnNearestCrossStreetChanged: function() {
      var called = false;
      var oldFn = onNearestCrossStreetChanged;
      onNearestCrossStreetChanged = function() { called = true; };

      ddlNearestCrossStreet_Change();

      Assert.isTrue(called);

      onNearestCrossStreetChanged = oldFn;
    },

    testTXTStreetNumber_ChangeCallsOnStreetNumberChanged: function() {
      var called = false;
      var oldFn = onStreetNumberChanged;
      onStreetNumberChanged = function() { called = true; };

      txtStreetNumber_Change();

      Assert.isTrue(called);

      onStreetNumberChanged = oldFn;
    },

    testDDLRequestedBy_ChangeCallsOnRequesterChanged: function() {
      var called = false, calledValue = null;
      var expectedValue = 1;
      var oldFn = onRequesterChanged;
      onRequesterChanged = function(val) { called = true; calledValue = val; };

      var select = new MockSelect({ selectedIndex: expectedValue });

      ddlRequestedBy_Change(select);
      Assert.isTrue(called);
      Assert.areEqual(expectedValue, calledValue);

      onRequesterChanged = oldFn;
    },

    testDDLAssetType_ChangeCallsOnAssetTypeChanged: function() {
      var called = false, calledValue = null;
      var expectedValue = 1;
      var oldFn = onAssetTypeChanged;
      onAssetTypeChanged = function(val) { called = true; calledValue = val; };

      var select = new MockSelect({ selectedIndex: expectedValue });

      ddlAssetType_Change(select);
      Assert.isTrue(called);
      Assert.areEqual(expectedValue, calledValue);

      onAssetTypeChanged = oldFn;
    },

    testDDLAssetID_ChageCallsOnAssetIDChangedWithProperArgument: function() {
      var expected = 1;
      var elem = new MockTextInput({ value: expected });
      var correct = false;
      var oldFn = onAssetIDChanged;
      onAssetIDChanged = function(val) { correct = (val == expected); };

      ddlAssetID_Change(elem);

      Assert.isTrue(correct);

      onAssetIDChanged = oldFn;
    },

    /* EVENT PASSTHROUGHS */
    testOnTownChangedCallsSetAssetAddressField: function() {
      var called = false;
      var oldFn = setAssetAddressField;
      setAssetAddressField = function() { called = true; };

      onTownChanged();

      Assert.isTrue(called);

      setAssetAddressField = oldFn;
    },

    testOnTownSectionChangedCallsSetAssetAddressField: function() {
      var called = false;
      var oldFn = setAssetAddressField;
      setAssetAddressField = function() { called = true; };

      onTownSectionChanged();

      Assert.isTrue(called);

      setAssetAddressField = oldFn;
    },

    testOnStreetChangedCallsSetAssetAddressField: function() {
      var called = false;
      var oldFn = setAssetAddressField;
      setAssetAddressField = function() { called = true; };

      onStreetChanged();

      Assert.isTrue(called);

      setAssetAddressField = oldFn;
    },

    testOnNearestCrossStreetChangedCallsGetAssetAddress: function() {
      var called = false;
      var oldFn = setAssetAddressField;
      setAssetAddressField = function() { called = true; };

      onNearestCrossStreetChanged();

      Assert.isTrue(called);

      setAssetAddressField = oldFn;
    },

    testOnStreetNumberChangedCallsGetAssetAddress: function() {
      var called = false;
      var oldFn = setAssetAddressField;
      setAssetAddressField = function() { called = true; };

      onStreetNumberChanged();

      Assert.isTrue(called);

      setAssetAddressField = oldFn;
    },

    testOnRequesterChangedTogglesCustomerAndEmployeeInfoOff: function() {
      var customerInfoTurnedOff = false, employeeInfoTurnedOff = false;
      var oldToggleCustomerInfo = toggleCustomerInfo;
      var oldToggleEmployeeInfo = toggleEmployeeInfo;
      toggleCustomerInfo = function(val) { customerInfoTurnedOff = (val == false); };
      toggleEmployeeInfo = function(val) { employeeInfoTurnedOff = (val == false); };
      setup$('#lblRequester', new MockDiv());

      onRequesterChanged(null);
      Assert.isTrue(customerInfoTurnedOff);
      Assert.isTrue(employeeInfoTurnedOff);

      toggleCustomerInfo = oldToggleCustomerInfo;
      toggleEmployeeInfo = oldToggleEmployeeInfo;
    },

    testOnRequesterChangedSetsRequesterLabelToSpaceByDefault: function() {
      var oldToggleCustomerInfo = toggleCustomerInfo;
      var oldToggleEmployeeInfo = toggleEmployeeInfo;
      toggleCustomerInfo = function(val) { /* noop */ };
      toggleEmployeeInfo = function(val) { /* noop */ };
      var div = new MockDiv();
      setup$('#lblRequester', div);

      onRequesterChanged(null);
      Assert.areEqual('&nbsp;', div.innerHTML);

      toggleCustomerInfo = oldToggleCustomerInfo;
      toggleEmployeeInfo = oldToggleEmployeeInfo;
    },

    testOnRequesterChangedTogglesCustomerInfoAndSetsLabelWhenRequesterIsCustomer: function() {
      var customerInfoTurnedOn = false;
      var oldToggleCustomerInfo = toggleCustomerInfo;
      var oldToggleEmployeeInfo = toggleEmployeeInfo;
      toggleCustomerInfo = function(val) { customerInfoTurnedOn = (val == true); };
      toggleEmployeeInfo = function(val) { /* noop */ };
      var div = new MockDiv();
      setup$('#lblRequester', div);

      onRequesterChanged(REQUESTER_IDS.CUSTOMER);
      Assert.isTrue(customerInfoTurnedOn);
      Assert.areEqual('Customer Name: ', div.innerHTML);

      toggleCustomerInfo = oldToggleCustomerInfo;
      toggleEmployeeInfo = oldToggleEmployeeInfo;
    },

    testOnRequesterChangedTogglesEmployeeInfoAndSetsLabelWhenRequesterIsEmployee: function() {
      var employeeInfoTurnedOn = false;
      var oldToggleCustomerInfo = toggleCustomerInfo;
      var oldToggleEmployeeInfo = toggleEmployeeInfo;
      toggleCustomerInfo = function(val) { /* noop */ };
      toggleEmployeeInfo = function(val) { employeeInfoTurnedOn = (val == true); };
      var div = new MockDiv();
      setup$('#lblRequester', div);

      onRequesterChanged(REQUESTER_IDS.EMPLOYEE);
      Assert.isTrue(employeeInfoTurnedOn);
      Assert.areEqual('Employee Name: ', div.innerHTML);

      toggleCustomerInfo = oldToggleCustomerInfo;
      toggleEmployeeInfo = oldToggleEmployeeInfo;
    },

    testOnAssetTypeChangedHidesAssetInputFields: function() {
      var called = false;
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { called = true; };
      setAssetCoordinates = function() { /* noop */ };
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', new MockDiv());

      onAssetTypeChanged(null);
      Assert.isTrue(called);

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedLeavesLabelAndControlAloneByDefault: function() {
      var ddlDummyAssetID = new MockSelect();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('ddlDummyAssetID', ddlDummyAssetID);
      setup$('#lblAssetID', lblAssetID);

      onAssetTypeChanged(null);
      Assert.areEqual('', ddlDummyAssetID.style.display);
      Assert.areEqual('Asset ID: ', lblAssetID.innerHTML);

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedHidesDummyDDLWhenArgumentIsAssetTypeID: function() {
      var ddlDummyAssetID = new MockSelect();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('ddlDummyAssetID', ddlDummyAssetID);
      setup$('#lblAssetID', new MockDiv());
      setup$('ddlValve', new MockSelect());
      setup$('ddlHydrant', new MockSelect());
      setup$('ddlMain', new MockSelect());
      setup$('pnlService', new MockDiv());
      setup$('hidAssetTypeID', new MockTextInput());

      onAssetTypeChanged(ASSET_TYPE_IDS.VALVE);
      Assert.areEqual('none', ddlDummyAssetID.style.display);

      onAssetTypeChanged(ASSET_TYPE_IDS.HYDRANT);
      Assert.areEqual('none', ddlDummyAssetID.style.display);

      onAssetTypeChanged(ASSET_TYPE_IDS.MAIN);
      Assert.areEqual('none', ddlDummyAssetID.style.display);

      onAssetTypeChanged(ASSET_TYPE_IDS.SERVICE);
      Assert.areEqual('none', ddlDummyAssetID.style.display);

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedShowsCorrectControlAndLabelWhenAssetTypeIDIsValve: function() {
      var hidAssetTypeID = new MockTextInput();
      var ddlValve = new MockSelect();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', lblAssetID);
      setup$('ddlValve', ddlValve);

      onAssetTypeChanged(ASSET_TYPE_IDS.VALVE);
      Assert.areEqual(ddlValve.style.display, '');
      Assert.areEqual(lblAssetID.innerHTML, 'Valve ID: ');

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedShowsCorrectControlAndLabelWhenAssetTypeIDIsHydrant: function() {
      var hidAssetTypeID = new MockTextInput();
      var ddlHydrant = new MockSelect();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', lblAssetID);
      setup$('ddlHydrant', ddlHydrant);

      onAssetTypeChanged(ASSET_TYPE_IDS.HYDRANT);
      Assert.areEqual(ddlHydrant.style.display, '');
      Assert.areEqual(lblAssetID.innerHTML, 'Hydrant ID: ');

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedShowsCorrectLabelWhenAssetTypeIDIsMain: function() {
      var hidAssetTypeID = new MockTextInput();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', lblAssetID);

      onAssetTypeChanged(ASSET_TYPE_IDS.MAIN);
      Assert.areEqual(lblAssetID.innerHTML, '');

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedShowsCorrectControlAndLabelWhenAssetTypeIDIsService: function() {
      var hidAssetTypeID = new MockTextInput();
      var pnlService = new MockDiv();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', lblAssetID);
      setup$('pnlService', pnlService);

      onAssetTypeChanged(ASSET_TYPE_IDS.SERVICE);
      Assert.areEqual(pnlService.style.display, '');
      Assert.areEqual(lblAssetID.innerHTML, 'Premise #:<br/>Service #:');

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetTypeChangedSetsValueOfHiddenAssetTypeInput: function() {
      var hidAssetTypeID = new MockTextInput();
      var ddlValve = new MockSelect();
      var lblAssetID = new MockDiv();
      var oldFn = hideAssetInputFields;
      var oldSetAssetCoordinates = setAssetCoordinates;
      hideAssetInputFields = function() { /* noop */ };
      setAssetCoordinates = function() { /* noop */ };
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('ddlDummyAssetID', new MockSelect());
      setup$('#lblAssetID', lblAssetID);

      var expected = 1;

      onAssetTypeChanged(expected);
      Assert.areEqual(expected.toString(), hidAssetTypeID.value);

      hideAssetInputFields = oldFn;
      setAssetCoordinates = oldSetAssetCoordinates;
    },

    testOnAssetIDChangedSetsValueOfHiddenAssetIDInput: function() {
      var expected = 1;
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Valve', expected)]
      });
      var hidAssetID = new MockTextInput();
      var hidAssetTypeID = new MockTextInput();
      var oldGetAssetCoordinates = getAssetCoordinates;
      getAssetCoordinates = function() { /* noop */ };
      setup$('ddlAssetType', ddlAssetType);
      setup$('hidAssetID', hidAssetID);
      setup$('hidHistoryAssetID', hidAssetID);
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('hidHistoryAssetTypeID', hidAssetTypeID);

      onAssetIDChanged(expected);

      Assert.areEqual(expected, hidAssetID.value);
      getAssetCoordinates = oldGetAssetCoordinates;
    },

    testOnAssetIDChangedSetsValueOfHiddenAssetTypeInput: function() {
      var expected = 1;
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Valve', expected)]
      });
      var hidAssetID = new MockTextInput();
      var hidAssetTypeID = new MockTextInput();
      var oldGetAssetCoordinates = getAssetCoordinates;
      getAssetCoordinates = function() { /* noop */ };
      setup$('ddlAssetType', ddlAssetType);
      setup$('hidAssetID', hidAssetID);
      setup$('hidHistoryAssetID', hidAssetID);
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('hidHistoryAssetTypeID', hidAssetTypeID);

      onAssetIDChanged(null);

      Assert.areEqual(expected, hidAssetTypeID.value);
      getAssetCoordinates = oldGetAssetCoordinates;
    },

    testOnAssetIDChangedFiresChangeEventOnHiddenAssetIDField: function() {
      var called = false;
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Valve', 1)]
      });
      var hidAssetID = new MockTextInput({ onchange: function() { called = true; } });
      var hidAssetTypeID = new MockTextInput();
      var oldGetAssetCoordinates = getAssetCoordinates;
      getAssetCoordinates = function() { /* noop */ };
      setup$('ddlAssetType', ddlAssetType);
      setup$('hidAssetID', hidAssetID);
      setup$('hidHistoryAssetID', hidAssetID);
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('hidHistoryAssetTypeID', hidAssetTypeID);

      onAssetIDChanged(null);

      Assert.isTrue(called);
      getAssetCoordinates = oldGetAssetCoordinates;
    },

    testOnAssetIDChangedCallsGetAssetCoordinates: function() {
      var called = false;
      var oldFn = getAssetCoordinates;
      getAssetCoordinates = function() { called = true; };
      var ddlAssetType = new MockSelect({
        // this all needs to be here so onAssetIDChanged
        // doesn't freak out
        selectedIndex: 0,
        options: [new MockOption('Valve', 1)]
      });
      var hidAssetID = new MockTextInput();
      var hidAssetTypeID = new MockTextInput();
      setup$('ddlAssetType', ddlAssetType);
      setup$('hidAssetID', hidAssetID);
      setup$('hidHistoryAssetID', hidAssetID);
      setup$('hidAssetTypeID', hidAssetTypeID);
      setup$('hidHistoryAssetTypeID', hidAssetTypeID);

      onAssetIDChanged(null);

      Assert.isTrue(called);

      getAssetCoordinates = oldFn;
    },

    /* UI FUNCTIONALITY */
    testIsInitialInputReturnsTrueWhenNoValueIsSelectedForTown: function() {
      var ddlTown = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('--Select Here--', '')]
      });
      setup$('ddlTown', ddlTown);

      Assert.isTrue(isInitialInput());
    },

    testIsInitialInputReturnsFalseWhenValueIsSelectedForTown: function() {
      var ddlTown = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('BumsVille', 1)]
      });
      setup$('ddlTown', ddlTown);

      Assert.isFalse(isInitialInput());
    },

    testInitializeInputEditCallsInitializeDate: function() {
      var called = false;
      var oldFn = initializeDate;
      var oldIsInitialInput = isInitialInput;
      initializeDate = function() { called = true; };
      isInitialInput = function() { return true; };

      initializeInputEdit();

      initializeDate = oldFn;
      isInitialInput = oldIsInitialInput;
    },

    testInitializeInputEditCallsInitializeEditWhenIsInitialInputReturnsFalse: function() {
      var called = false;
      var oldFn = initializeEdit;
      var oldIsInitialInput = isInitialInput;
      var oldInitializeDate = initializeDate;
      initializeEdit = function() { called = true; };
      isInitialInput = function() { return false; };
      initializeDate = function() { /* noop */ };

      initializeInputEdit();
      Assert.isTrue(called);

      initializeEdit = oldFn;
      isInitialInput = oldIsInitialInput;
      initializeDate = oldInitializeDate;
    },

    testInitializeInputEditDoesNotCallInitializeEditWhenIsInitialInputReturnsTrue: function() {
      var called = false;
      var oldFn = initializeEdit;
      var oldIsInitialInput = isInitialInput;
      var oldInitializeDate = initializeDate;
      initializeEdit = function() { called = true; };
      isInitialInput = function() { return true; };
      initializeDate = function() { /* noop */ };

      initializeInputEdit();
      Assert.isFalse(called);

      initializeEdit = oldFn;
      isInitialInput = oldIsInitialInput;
      initializeDate = oldInitializeDate;
    },

    testInitializeEditCallsOnChangeOnAssetTypeDropDown: function() {
      var called = false;
      var ddlAssetType = new MockSelect({
        onchange: function() { called = true; }
      });
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlRequestedBy', new MockSelect());

      initializeEdit();
      Assert.isTrue(called);
    },

    testInitializeEditCallsOnChangeOnRequestedByDropDown: function() {
      var called = false;
      var ddlRequestedBy = new MockSelect({
        onchange: function() { called = true; }
      });
      setup$('ddlRequestedBy', ddlRequestedBy);
      setup$('ddlAssetType', new MockSelect());

      initializeEdit();
      Assert.isTrue(called);
    },

    testInitializeDateLeavesDateReceivedAloneWhenValueSet: function() {
      var expectedValue = 'This should not change';
      var ccDateReceived = new MockTextInput({ value: expectedValue });
      setup$('ccDateReceived', ccDateReceived);

      initializeDate();
      Assert.areEqual(expectedValue, ccDateReceived.value);
    },

    testInitializeDateSetsDateReceivedWhenNoValueSet: function() {
      var unexpectedValue = '';
      var ccDateReceived = new MockTextInput({ value: unexpectedValue });
      setup$('ccDateReceived', ccDateReceived);

      initializeDate();
      Assert.areNotEqual(unexpectedValue, ccDateReceived.value);
    },

    testDisplayValidationMessageShowsValidationArea: function() {
      var shown = false;
      var oldFn = toggleValidationArea;
      toggleValidationArea = function(val) { shown = (val == true); };
      setup$('#tdNotificationArea', new MockDiv());

      displayValidationMessage(null);
      Assert.isTrue(shown);

      toggleValidationArea = oldFn;
    },

    testDisplayValidationMessageSetsValidationAreaText: function() {
      var tdNotificationArea = new MockDiv();
      var expected = 'Test String';
      var oldFn = toggleValidationArea;
      toggleValidationArea = function(val) { /* noop */ };
      setup$('#tdNotificationArea', tdNotificationArea);

      displayValidationMessage(expected);
      Assert.areEqual(expected, tdNotificationArea.innerHTML);

      toggleValidationArea = oldFn;
    },

    testToggleValidationAreaCallsToggleElementArrayWithCorrectArguments: function() {
      var trNotificationArea = new MockDiv();
      var correct = false;
      var expectedVisible;
      var oldFn = toggleElementArray;
      toggleElementArray = function(visible, arr) {
        correct = (visible == expectedVisible && arr[0][0] == trNotificationArea);
      };
      setup$('#trNotificationArea', trNotificationArea);

      expectedVisible = true;
      toggleValidationArea(expectedVisible);
      Assert.isTrue(correct);

      correct = false;
      expectedVisible = false;
      toggleValidationArea(expectedVisible);
      Assert.isTrue(correct);

      toggleElementArray = oldFn;
    },

    testToggleCustomerInfoCallsToggleElementArrayWithCorrectArguments: function() {
      var trCustomerInfo = new MockDiv();
      var txtCustomerName = new MockDiv();
      var correct = false;
      var expectedVisible;
      var oldFn = toggleElementArray;
      toggleElementArray = function(visible, arr) {
        correct = (visible == expectedVisible &&
                   arr[0][0] == trCustomerInfo &&
                   arr[1][0] == txtCustomerName);
      };
      setup$('.trCustomerInfo', trCustomerInfo);
      setup$('txtCustomerName', txtCustomerName);

      expectedVisible = true;
      toggleCustomerInfo(expectedVisible);
      Assert.isTrue(correct);

      correct = false;
      expectedVisible = false;
      toggleCustomerInfo(expectedVisible);
      Assert.isTrue(correct);

      toggleElementArray = oldFn;
    },

    testToggleEmployeeInfoCallsToggleElementArrayWithCorrectArguments: function() {
      var ddlRequestingEmployee = new MockSelect();
      var trSafetyRequirements = new MockTableRow();
      var correct = false;
      var expectedVisible;
      var oldFn = toggleElementArray;
      toggleElementArray = function(visible, arr) {
        correct = (visible == expectedVisible &&
                   arr[0][0] == ddlRequestingEmployee);
      };
      setup$('ddlRequestingEmployee', ddlRequestingEmployee);
      setup$('#trSafetyRequirements', trSafetyRequirements);

      expectedVisible = true;
      toggleEmployeeInfo(expectedVisible);
      Assert.isTrue(correct,
        'Arguments were not correct when turning on desired fields.');

      correct = false;
      expectedVisible = false;
      toggleEmployeeInfo(expectedVisible);
      Assert.isTrue(correct,
        'Arguments were not correct when turning off desired fields.');

      toggleElementArray = oldFn;
    },

    testHideAssetInputFieldsHidesAssetFieldsAndShowsDummyField: function() {
      var ddlValve = new MockSelect();
      var ddlHydrant = new MockSelect();
      var ddlMain = new MockSelect();
      var pnlService = new MockDiv();
      var ddlDummyAssetID = new MockDiv();
      var oldFn = toggleElementArray;
      var correct = false;
      toggleElementArray = function(visible, arr) {
        correct = (visible == false &&
                  arr[0][0] == ddlValve &&
                  arr[1][0] == ddlHydrant &&
                  arr[2][0] == ddlMain &&
                  arr[3][0] == pnlService);
        toggleElementArray = function(visible, arr) {
          correct = (correct && visible == true &&
                      arr[0][0] == ddlDummyAssetID);
        };
      };
      setup$('ddlValve', ddlValve);
      setup$('ddlHydrant', ddlHydrant);
      setup$('ddlMain', ddlMain);
      setup$('pnlService', pnlService);
      setup$('ddlDummyAssetID', ddlDummyAssetID);

      hideAssetInputFields();
      Assert.isTrue(correct);

      toggleElementArray = oldFn;
    },

    testResetFormHidesCustomerAndEmployeeInfo: function() {
      var customerInfoHidden = false, employeeInfoHidden = false;
      var oldToggleCusomerInfo = toggleCustomerInfo;
      var oldToggleEmployeeInfo = toggleEmployeeInfo;
      toggleCustomerInfo = function(val) { customerInfoHidden = (val == false); };
      toggleEmployeeInfo = function(val) { employeeInfoHidden = (val == false); };

      resetForm();
      Assert.isTrue(customerInfoHidden);
      Assert.isTrue(employeeInfoHidden);

      toggleCustomerInfo = oldToggleCusomerInfo;
      toggleEmployeeInfo = oldToggleEmployeeInfo;
    },

    /* VALIDATION */
    testValidateFormHidesValidationArea: function() {
      var correct = false;
      var oldFn = toggleValidationArea;
      var oldValidateLocationInfo = validateLocationInfo;
      var oldValidateJobInfo = validateJobInfo;
      var oldValidateRequesterInfo = validateRequesterInfo;
      toggleValidationArea = function(val) { correct = (val == false); };
      validateLocationInfo = validateJobInfo = validateRequesterInfo = function() { return false; };

      validateForm();
      Assert.isTrue(correct);

      toggleValidationArea = oldFn;
      validateLocationInfo = oldValidateLocationInfo;
      validateJobInfo = oldValidateJobInfo;
      validateRequesterInfo = oldValidateRequesterInfo;
    },

    testValidateFormReturnsFalseIfAnyValidationMethodReturnsFalse: function() {
      var returnTrue = function() { return true; };
      var returnFalse = function() { return false; };
      var oldValidateLocationInfo = validateLocationInfo;
      var oldValidateJobInfo = validateJobInfo;
      var oldValidateRequesterInfo = validateRequesterInfo;
      var oldValidateAssetInfo = validateAssetInfo;

      validateLocationInfo = returnFalse;
      validateJobInfo = validateRequesterInfo = validateAssetInfo = returnTrue;
      Assert.isFalse(validateForm());

      validateJobInfo = returnFalse;
      validateLocationInfo = validateRequesterInfo = validateAssetInfo = returnTrue;
      Assert.isFalse(validateForm());

      validateRequesterInfo = returnFalse;
      validateLocationInfo = validateJobInfo = validateAssetInfo = returnTrue;
      Assert.isFalse(validateForm());

      validateAssetInfo = returnFalse;
      validateLocationInfo = validateJobInfo = validateRequesterInfo = returnTrue;
      Assert.isFalse(validateForm());

      validateLocationInfo = oldValidateLocationInfo;
      validateJobInfo = oldValidateJobInfo;
      validateRequesterInfo = oldValidateRequesterInfo;
      validateAssetInfo = oldValidateAssetInfo;
    },

    testValidateFormReturnsTrueIfAllValidationMethodsReturnTrue: function() {
      var oldValidateLocationInfo = validateLocationInfo;
      var oldValidateJobInfo = validateJobInfo;
      var oldValidateRequesterInfo = validateRequesterInfo;
      var oldValidateAssetInfo = validateAssetInfo;
      validateLocationInfo = validateJobInfo = validateRequesterInfo =
        validateAssetInfo = function() { return true; };

      Assert.isTrue(validateForm());

      validateLocationInfo = oldValidateLocationInfo;
      validateJobInfo = oldValidateJobInfo;
      validateRequesterInfo = oldValidateRequesterInfo;
      validateAssetInfo = oldValidateAssetInfo;
    },

    testValidateRequesterInfoValdatesCustomerInfoWhenRequesterIsCustomer: function() {
      var ddlRequestedBy = new MockSelect({ selectedIndex: REQUESTER_IDS.CUSTOMER });
      var called = false;
      var oldFn = validateCustomerInfo;
      validateCustomerInfo = function() { called = true; return true; };
      setup$('ddlRequestedBy', ddlRequestedBy);

      Assert.isTrue(validateRequesterInfo());
      Assert.isTrue(called);

      validateCustomerInfo = oldFn;
    },

    testValidateRequesterInfoValdatesEmployeeInfoWhenRequesterIsEmployee: function() {
      var ddlRequestedBy = new MockSelect({ selectedIndex: REQUESTER_IDS.EMPLOYEE });
      var called = false;
      var oldFn = validateEmployeeInfo;
      validateEmployeeInfo = function() { called = true; return true; };
      setup$('ddlRequestedBy', ddlRequestedBy);

      Assert.isTrue(validateRequesterInfo());
      Assert.isTrue(called);

      validateEmployeeInfo = oldFn;
    },

    testValidateRequesterInfoValdatesCustomerGovernmentInfoWhenRequesterIsGovernment: function() {
      var ddlRequestedBy = new MockSelect({ selectedIndex: REQUESTER_IDS.LOCAL_GOVERNMENT });
      var called = false;
      var oldFn = validateGovernmentInfo;
      validateGovernmentInfo = function() { called = true; return true; };
      setup$('ddlRequestedBy', ddlRequestedBy);

      Assert.isTrue(validateRequesterInfo());
      Assert.isTrue(called);

      validateGovernmentInfo = oldFn;
    },

    testValidateRequesterInfoValdatesCallCenterInfoWhenRequesterIsCallCenter: function() {
      var ddlRequestedBy = new MockSelect({ selectedIndex: REQUESTER_IDS.CALL_CENTER });
      var called = false;
      var oldFn = validateCallCenterInfo;
      validateCallCenterInfo = function() { called = true; return true; };
      setup$('ddlRequestedBy', ddlRequestedBy);

      Assert.isTrue(validateRequesterInfo());
      Assert.isTrue(called);

      validateCallCenterInfo = oldFn;
    },

    testValidateLocationInfoReturnsTrueWhenSectionIsValid: function() {
      var ddl = new MockSelect({ selectedIndex: 1 });
      setup$('ddlTown', ddl);
      setup$('ddlTownSection', ddl);
      setup$('ddlStreet', ddl);
      setup$('ddlNearestCrossStreet', ddl);
      setup$('txtStreetNumber', new MockTextInput({ value: 'not empty' }));

      Assert.isTrue(validateLocationInfo());
    },

    testValidateLocationInfoReturnsTrueWhenNoTownSectionSelected: function() {
      var ddlTownSection = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddl);
      setup$('ddlStreet', ddl);
      setup$('ddlNearestCrossStreet', ddl);
      setup$('txtStreetNumber', new MockTextInput({ value: 'not empty' }));

      Assert.isTrue(validateLocationInfo());
    },

    testValidateLocationInfoPromptsFocusesAndReturnsFalseWhenNoStreetNumberSelected: function() {
      var txtStreetNumber = new MockTextInput();
      var ddl = new MockSelect({ selectedIndex: 1 });
      var expectedMessage = 'Please enter the nearest (or customer) house number.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlTown', ddl);
      setup$('ddlTownSection', ddl);
      setup$('ddlStreet', ddl);
      setup$('ddlNearestCrossStreet', ddl);
      setup$('txtStreetNumber', txtStreetNumber);

      Assert.isFalse(validateLocationInfo());
      Assert.isTrue(txtStreetNumber.focused);

      displayValidationMessage = oldFn;
    },

    testValiateLocationInfoPromptsFocusesAndReturnsFalseWhenNoTownSelected: function() {
      var ddlTown = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var expectedMessage = 'Please choose the town.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlTown', ddlTown);
      setup$('ddlTownSection', ddl);
      setup$('ddlStreet', ddl);
      setup$('ddlNearestCrossStreet', ddl);
      setup$('txtStreetNumber', new MockTextInput({ value: 'not empty' }));

      Assert.isFalse(validateLocationInfo());
      Assert.isTrue(ddlTown.focused);

      displayValidationMessage = oldFn;
    },

    testValiateLocationInfoPromptsFocusesAndReturnsFalseWhenNoStreetSelected: function() {
      var ddlStreet = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var expectedMessage = 'Please choose the street.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTown', ddl);
      setup$('ddlTownSection', ddl);
      setup$('ddlNearestCrossStreet', ddl);
      setup$('txtStreetNumber', new MockTextInput({ value: 'not empty' }));

      Assert.isFalse(validateLocationInfo());
      Assert.isTrue(ddlStreet.focused);

      displayValidationMessage = oldFn;
    },

    testValiateLocationInfoPromptsFocusesAndReturnsFalseWhenNoNearestCrossStreetSelected: function() {
      var ddlNearestCrossStreet = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var expectedMessage = 'Please choose the nearest cross street.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlNearestCrossStreet', ddlNearestCrossStreet);
      setup$('ddlStreet', ddl);
      setup$('ddlTown', ddl);
      setup$('ddlTownSection', ddl);
      setup$('txtStreetNumber', new MockTextInput({ value: 'not empty' }));

      Assert.isFalse(validateLocationInfo());
      Assert.isTrue(ddlNearestCrossStreet.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoReturnsTrueWhenSectionIsValid: function() {
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      setup$('ddlAssetType', ddl);
      setup$('ddlDrivenBy', ddl);
      setup$('ddlPriority', ddl);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isTrue(validateJobInfo());
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoAssetTypeSelected: function() {
      var ddlAssetType = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please choose the asset type.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlDrivenBy', ddl);
      setup$('ddlPriority', ddl);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ddlAssetType.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoPurposeSelected: function() {
      var ddlDrivenBy = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please choose the purpose of this order.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlDrivenBy', ddlDrivenBy);
      setup$('ddlAssetType', ddl);
      setup$('ddlPriority', ddl);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ddlDrivenBy.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoPrioritySelected: function() {
      var ddlPriority = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please select job priority.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlPriority', ddlPriority);
      setup$('ddlAssetType', ddl);
      setup$('ddlDrivenBy', ddl);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ddlPriority.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoDescriptionOfWorkSelected: function() {
      var ddlDescriptionOfWork = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please select the description of work.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlDescriptionOfWork', ddlDescriptionOfWork);
      setup$('ddlPriority', ddl);
      setup$('ddlAssetType', ddl);
      setup$('ddlDrivenBy', ddl);
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ddlDescriptionOfWork.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoMarkoutRequirementSelected: function() {
      var ddlMarkoutRequirement = new MockSelect({ selectedIndex: 0 });
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please select the markout requirement.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlMarkoutRequirement', ddlMarkoutRequirement);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlPriority', ddl);
      setup$('ddlAssetType', ddl);
      setup$('ddlDrivenBy', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ddlMarkoutRequirement.focused);

      displayValidationMessage = oldFn;
    },

    testValidateJobInfoPromptsFocusesAndReturnsFalseWhenNoRequestDateEntered: function() {
      var ddl = new MockSelect({ selectedIndex: 1 });
      var ccDateReceived = new MockTextInput({ value: '' });
      var expectedMessage = 'Please enter the date received.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlMarkoutRequirement', ddl);
      setup$('ddlDescriptionOfWork', ddl);
      setup$('ddlPriority', ddl);
      setup$('ddlAssetType', ddl);
      setup$('ddlDrivenBy', ddl);
      setup$('ccDateReceived', ccDateReceived);

      Assert.isFalse(validateJobInfo());
      Assert.isTrue(ccDateReceived.focused);

      displayValidationMessage = oldFn;
    },

    testValidateCustomerInfoReturnsTrueWhenSectionIsValid: function() {
      var txt = new MockTextInput({ value: 'not empty' });
      setup$('txtCustomerName', txt);
      setup$('txtPhoneNumber', txt);

      Assert.isTrue(validateCustomerInfo());
    },

    testValidateCustomerPromptsFocusesAndReturnsFalseWhenNoCustomerNameEntered: function() {
      var txtCustomerName = new MockTextInput({ value: '' });
      var txt = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please enter the customer\'s name.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('txtCustomerName', txtCustomerName);
      setup$('txtPhoneNumber', txt);

      Assert.isFalse(validateCustomerInfo());
      Assert.isTrue(txtCustomerName.focused);

      displayValidationMessage = oldFn;
    },

    testValidateCustomerPromptsFocusesAndReturnsFalseWhenNoPhoneNumberEntered: function() {
      // the MaskedEditExtender on txtPhoneNumber makes the value fit
      // this format
      var txtPhoneNumber = new MockTextInput({ value: '(___)___-____' });
      var txt = new MockTextInput({ value: 'not empty' });
      var expectedMessage = 'Please enter the customer\'s phone number.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('txtPhoneNumber', txtPhoneNumber);
      setup$('txtCustomerName', txt);

      Assert.isFalse(validateCustomerInfo());
      Assert.isTrue(txtPhoneNumber.focused);

      displayValidationMessage = oldFn;
    },

    testValidateEmployeeInfoReturnsTrueWhenSectionIsValid: function() {
      var ddlRequestingEmployee = new MockSelect({
        options: [new MockOption('foo', 'bar')],
        selectedIndex: 0
      });
      setup$('ddlRequestingEmployee', ddlRequestingEmployee);

      Assert.isTrue(validateEmployeeInfo());
    },

    testValidateEmployeeInfoPromptsFocusesAndReturnsFalseWhenNoEmployeeEntered: function() {
      var ddlRequestingEmployee = new MockSelect({
        options: [new MockOption('', '')],
        selectedIndex: 0
      });
      var expectedMessage = 'Please select an employee.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlRequestingEmployee', ddlRequestingEmployee);

      Assert.isFalse(validateEmployeeInfo());
      Assert.isTrue(ddlRequestingEmployee.focused);

      displayValidationMessage = oldFn;
    },

    testValidateGovernmentInfoReturnsTrue: function() {
      Assert.isTrue(validateGovernmentInfo());
    },

    testValidateCallCenterInfoReturnsTrueWhenORCOMServiceOrderNumberHasBeenEntered: function() {
      var txtORCOMServiceOrderNumber = new MockTextInput({ value: '123456789' });
      setup$('txtORCOMServiceOrderNumber', txtORCOMServiceOrderNumber);

      Assert.isTrue(validateCallCenterInfo());
    },

    testValidateAssetInfoReturnsTrueWhenAssetTypeIsValveAndValveHasBeenChosenAndCoordinatesSet: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Valve', ASSET_TYPE_IDS.VALVE)]
      });
      var ddlValve = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('1234', 1)]
      });
      var ddl = new MockSelect();
      var txt = new MockTextInput();
      var called = false;
      var oldAreCoordinatesSet = areCoordinatesSet;
      areCoordinatesSet = function() {
        called = true;
        return true;
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlValve', ddlValve);
      setup$('ddlHydrant', ddl);
      setup$('txtServiceNumber', txt);
      setup$('txtPremiseNumber', txt);

      Assert.isTrue(validateAssetInfo());
      Assert.isTrue(called);

      areCoordinatesSet = oldAreCoordinatesSet;
    },

    testValidateAssetInfoReturnsFalseWhenAssetTypeIsValveAndNoValveHasBeenChosen: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Valve', ASSET_TYPE_IDS.VALVE)]
      });
      var ddlValve = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption(), new MockOption('1234', 1)]
      });
      var ddl = new MockSelect();
      var txt = new MockTextInput();
      var expectedMessage = 'Please choose a valve.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlValve', ddlValve);
      setup$('ddlHydrant', ddl);
      setup$('txtServiceNumber', txt);
      setup$('txtPremiseNumber', txt);

      Assert.isFalse(validateAssetInfo());
      Assert.isTrue(ddlValve.focused);

      displayValidationMessage = oldFn;
    },

    testValidateAssetInfoReturnsTrueWhenAssetTypeIsHydrantAndHydrantHasBeenChosen: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Hydrant', ASSET_TYPE_IDS.HYDRANT)]
      });
      var ddlHydrant = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('1234', 1)]
      });
      var ddl = new MockSelect();
      var called = false;
      var oldAreCoordinatesSet = areCoordinatesSet;
      areCoordinatesSet = function() {
        called = true;
        return true;
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlHydrant', ddlHydrant);
      setup$('ddlValve', ddl);
      setup$('ddlMain', ddl);
      setup$('ddlService', ddl);

      Assert.isTrue(validateAssetInfo());
      Assert.isTrue(called);

      areCoordinatesSet = oldAreCoordinatesSet;
    },

    testValidateAssetInfoReturnsFalseWhenAssetTypeIsHydrantAndNoHydrantHasBeenChosen: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Hydrant', ASSET_TYPE_IDS.HYDRANT)]
      });
      var ddlHydrant = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption(), new MockOption('1234', 1)]
      });
      var ddl = new MockSelect();
      var expectedMessage = 'Please choose a hydrant.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlHydrant', ddlHydrant);
      setup$('ddlValve', ddl);
      setup$('ddlMain', ddl);
      setup$('ddlService', ddl);

      Assert.isFalse(validateAssetInfo());
      Assert.isTrue(ddlHydrant.focused);

      displayValidationMessage = oldFn;
    },

    testValidateAssetInfoReturnsTrueWhenAssetTypeIsMainAndCoordinatesSet: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Main', ASSET_TYPE_IDS.MAIN)]
      });
      var ddl = new MockSelect();
      var called = false;
      var oldAreCoordinatesSet = areCoordinatesSet;
      areCoordinatesSet = function() {
        called = true;
        return true;
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlValve', ddl);
      setup$('ddlHydrant', ddl);
      setup$('ddlService', ddl);

      Assert.isTrue(validateAssetInfo());
      Assert.isTrue(called);

      areCoordinatesSet = oldAreCoordinatesSet;
    },

    testValidateAssetInfoReturnsFalseWhenAssetTypeIsMainAndCoordinatesNotSet: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Main', ASSET_TYPE_IDS.MAIN)]
      });
      var txt = new MockTextInput();
      var ddl = new MockSelect();
      var expectedMessage = 'Please enter the location for this order using the globe icon.';
      var oldAreCoordinatesSet = areCoordinatesSet;
      areCoordinatesSet = function() {
        return false;
      };
      var oldDisplayValidationMessage = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('ddlValve', ddl);
      setup$('ddlHydrant', ddl);
      setup$('txtServiceNumber', txt);
      setup$('txtPremiseNumber', txt);

      Assert.isFalse(validateAssetInfo());

      displayValidationMessage = oldDisplayValidationMessage;
      areCoordinatesSet = oldAreCoordinatesSet;
    },

    testValidateAssetInfoReturnsTrueWhenAssetTypeIsServiceAndServiceHasBeenChosen: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Service', ASSET_TYPE_IDS.SERVICE)]
      });
      var txtPremiseNumber = new MockTextInput({ value: '123456789' });
      var ddl = new MockSelect();
      var called = false;
      var oldAreCoordinatesSet = areCoordinatesSet;
      areCoordinatesSet = function() {
        called = true;
        return true;
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('txtPremiseNumber', txtPremiseNumber);
      setup$('ddlValve', ddl);
      setup$('ddlHydrant', ddl);
      setup$('ddlMain', ddl);

      Assert.isTrue(validateAssetInfo());
      Assert.isTrue(called);

      areCoordinatesSet = oldAreCoordinatesSet;
    },

    testValidateAssetInfoReturnsFalseWhenAssetTypeIsServiceAndNoServiceHasBeenChosen: function() {
      var ddlAssetType = new MockSelect({
        selectedIndex: 0,
        options: [new MockOption('Service', ASSET_TYPE_IDS.SERVICE)]
      });
      var txtPremiseNumber = new MockTextInput({ value: '' });
      var ddl = new MockSelect();
      var expectedMessage = 'Please enter a premise #.';
      var oldFn = displayValidationMessage;
      displayValidationMessage = function(msg) {
        Assert.areEqual(expectedMessage, msg);
      };
      setup$('ddlAssetType', ddlAssetType);
      setup$('txtPremiseNumber', txtPremiseNumber);
      setup$('ddlValve', ddl);
      setup$('ddlHydrant', ddl);
      setup$('ddlMain', ddl);

      Assert.isFalse(validateAssetInfo());
      Assert.isTrue(txtPremiseNumber.focused);

      displayValidationMessage = oldFn;
    },

    testIsAssetChosenReturnsFalseWhenNoAssetTypeChosen: function() {
      setup$('hidAssetID', new MockTextInput({ value: 1 }));
      setup$('hidAssetTypeID', new MockTextInput());

      Assert.isFalse(isAssetChosen());
    },

    testIsAssetChosenReturnsFalseWhenNoAssetChosen: function() {
      setup$('hidAssetID', new MockTextInput());
      setup$('hidAssetTypeID', new MockTextInput({ value: 1 }));

      Assert.isFalse(isAssetChosen());
    },

    testIsAssetChosenReturnsTrueWhenAssetTypeAndAssetIDSet: function() {
      setup$('hidAssetID', new MockTextInput({ value: 1 }));
      setup$('hidAssetTypeID', new MockTextInput({ value: 1 }));

      Assert.isTrue(isAssetChosen());
    },

    /* HELPER METHODS */
    testGetAssetAddressBuildsAddressStringFromProperControls: function() {
      var expected = {
        streetNumber: '123',
        street: 'Maple St',
        townSection: 'AnySection',
        town: 'AnyTown',
        state: 'Disarray',
        fullString: '123 Maple St AnySection AnyTown Disarray'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 1
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 1
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 1
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAssetAddressSkipsStreetNumberWhenNoValuePresent: function() {
      var expected = {
        streetNumber: '',
        street: 'Maple St',
        townSection: 'AnySection',
        town: 'AnyTown',
        state: 'Disarray',
        fullString: 'Maple St AnySection AnyTown Disarray'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 1
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 1
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 1
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAssetAddressSkipsStreetNameWhenNoValuePresent: function() {
      var expected = {
        streetNumber: '123',
        street: '',
        townSection: 'AnySection',
        town: 'AnyTown',
        state: 'Disarray',
        fullString: '123 AnySection AnyTown Disarray'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 0
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 1
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 1
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAssetAddressSkipsTownSectionWhenNoValuePresent: function() {
      var expected = {
        streetNumber: '123',
        street: 'Maple St',
        townSection: '',
        town: 'AnyTown',
        state: 'Disarray',
        fullString: '123 Maple St AnyTown Disarray'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 1
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 0
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 1
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAssetAddressSkipsTownWhenNoValuePresent: function() {
      var expected = {
        streetNumber: '123',
        street: 'Maple St',
        townSection: 'AnySection',
        town: '',
        state: 'Disarray',
        fullString: '123 Maple St AnySection Disarray'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 1
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 1
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 0
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAssetAddressSkipsStateWhenNoValuePresent: function() {
      var expected = {
        streetNumber: '123',
        street: 'Maple St',
        townSection: 'AnySection',
        town: 'AnyTown',
        state: '',
        fullString: '123 Maple St AnySection AnyTown'
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [null, new MockOption(expected.street, 1)],
        selectedIndex: 1
      });
      var ddlTownSection = new MockSelect({
        options: [null, new MockOption(expected.townSection, 1)],
        selectedIndex: 1
      });
      var ddlTown = new MockSelect({
        options: [null, new MockOption(expected.town, 1)],
        selectedIndex: 1
      });
      var hidState = new MockTextInput({
        value: expected.state
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);
      setup$('hidState', hidState);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    testGetAddressReturnsEmptyStringWhenNoRelevantValuesSet: function() {
      var expected = {
        streetNumber: '',
        street: '',
        townSection: '',
        town: '',
        fullString: ''
      };
      var txtStreetNumber = new MockTextInput({ value: expected.streetNumber });
      var ddlStreet = new MockSelect({
        options: [new MockOption(expected.street, 1)],
        selectedIndex: 0
      });
      var ddlTownSection = new MockSelect({
        options: [new MockOption(expected.townSection, 1)],
        selectedIndex: 0
      });
      var ddlTown = new MockSelect({
        options: [new MockOption(expected.town, 1)],
        selectedIndex: 0
      });
      setup$('txtStreetNumber', txtStreetNumber);
      setup$('ddlStreet', ddlStreet);
      setup$('ddlTownSection', ddlTownSection);
      setup$('ddlTown', ddlTown);

      var actual = getAssetAddress();

      Assert.areEqual(expected.fullString, actual);
    },

    /* AJAX */
    testGetAssetCoordinatesDoesNotMakeAJAXCallWhenAssetTypeAndAssetIDAreNotSet: function() {
      var called = false;
      var oldIsAssetChosen = isAssetChosen;
      var oldSetAssetCoordinates = setAssetCoordinates;
      isAssetChosen = function() { return false; };
      setAssetCoordinates = function() { /* noop */ };
      var old$ajax = $.ajax;
      $.ajax = function() { called = true; };

      getAssetCoordinates();

      Assert.isFalse(called);

      isAssetChosen = oldIsAssetChosen;
      setAssetCoordinates = oldSetAssetCoordinates;
      $.ajax = old$ajax;
    },

    testGetAssetCoordinatesMakesTheProperAJAXCallWhenAssetTypeAndAssetIDAreSet: function() {
      var correct = false;
      var expectedAssetTypeID = 1234;
      var expectedAssetID = 1234;
      var expected = {
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: '../../Coordinates/CoordinateServiceView.asmx/GetCoordinatesForAsset',
        data: '{assetTypeID:' + expectedAssetTypeID + ',assetID:' + expectedAssetID + '}',
        dataType: 'json'
      };
      var oldIsAssetChosen = isAssetChosen;
      isAssetChosen = function() { return true; };
      var old$ajax = $.ajax;
      $.ajax = function(obj) {
        for (var x in expected) {
          if (obj[x] == 'undefined') {
            Assert.fail('obj[\'' + x + '\'] for call to $.ajax() was not defined.');
          }
          Assert.areEqual(expected[x], obj[x]);
        }
        correct = true;
      };
      setup$('hidAssetTypeID', new MockTextInput({ value: expectedAssetTypeID }));
      setup$('hidAssetID', new MockTextInput({ value: expectedAssetID }));

      getAssetCoordinates();

      Assert.isTrue(correct);

      isAssetChosen = oldIsAssetChosen;
      $.ajax = old$ajax;
    },

    testGetAssetCoordinatesCallsSetAssetCoordinatesUponSuccessfulResponseFromWebService: function() {
      Assert.notImplemented();
    },

    testSetAssetCoordinatesWritesCoordinateValuesToHiddenInputsWhenArgumentIsNotNull: function() {
      var expected = {
        latitude: 1,
        longitude: 2
      };
      var imgShowPicker = new MockImage();
      var hidLatitude = new MockTextInput();
      var hidLongitude = new MockTextInput();
      setup$('hidLatitude', hidLatitude);
      setup$('hidLongitude', hidLongitude);
      setup$('imgShowPicker', imgShowPicker);

      setAssetCoordinates(expected);

      Assert.areEqual(expected.latitude, hidLatitude.value);
      Assert.areEqual(expected.longitude, hidLongitude.value);
    },

    testSetAssetCoordinatesCallsPickerSetIconWhenArgumentIsNotNull: function() {
      var called = false;
      var coords = {
        latitude: 1,
        longitude: 2
      };
      var hidLatitude = new MockTextInput();
      var hidLongitude = new MockTextInput();
      var imgShowPicker = new MockImage();
      var oldFn = LatLonPicker.ImageButton.setIcon;
      LatLonPicker.ImageButton.setIcon = function(img, lat, lon) {
        called = true;
        Assert.areEqual(imgShowPicker, img[0]);
        Assert.areEqual(hidLatitude, lat[0]);
        Assert.areEqual(hidLongitude, lon[0]);
      };
      setup$('hidLatitude', hidLatitude);
      setup$('hidLongitude', hidLongitude);
      setup$('imgShowPicker', imgShowPicker);

      setAssetCoordinates(coords);

      Assert.isTrue(called,
        'LatLonPicker.ImageButton.setIcon was not called as expected.');

      LatLonPicker.ImageButton.setIcon = oldFn;
    },

    testSetAssetAddressFieldSetsHiddenAddressFieldToReturnValueFromGetAssetAddress: function() {
      var expected = 'foobar';
      var hidAddress = new MockTextInput();
      var oldFn = getAssetAddress;
      getAssetAddress = function() { return expected; };
      setup$('hidAddress', hidAddress);

      setAssetAddressField();

      Assert.areEqual(expected, hidAddress.value);

      getAssetAddress = oldFn;
    },

    /////////////////////////////////READ ONLY/////////////////////////////////

    /* UI FUNCTIONALITY */
    testInitializeReadOnlySetsAssetIDLabelFromAssetTypeValue: function() {
      var called = false;
      var expected = 'Valve';
      var lblAssetType = new MockDiv({ innerText: expected });
      var oldSetAssetIDLabelFromAssetTypeName = setAssetIDLabelFromAssetTypeName;
      var oldDisplayRequesterInfoByRequester = displayRequesterInfoByRequester;
      setAssetIDLabelFromAssetTypeName = function(str) {
        called = true;
        Assert.areEqual(expected, str);
      };
      displayRequesterInfoByRequester = function() { /* noop */ };
      setup$('lblAssetType', lblAssetType);
      setup$('lblRequestedBy', new MockDiv());

      initializeReadOnly();
      Assert.isTrue(called);

      setAssetIDLabelFromAssetTypeName = oldSetAssetIDLabelFromAssetTypeName;
      displayRequesterInfoByRequester = oldDisplayRequesterInfoByRequester;
    },

    testInitializeReadOnlyCallsDisplayRequesterInfoByRequester: function() {
      var called = false;
      var expected = 'Customer';
      var lblRequestedBy = new MockDiv({ innerText: expected });
      var oldDisplayRequesterInfoByRequester = displayRequesterInfoByRequester;
      var oldSetAssetIDLabelFromAssetTypeName = setAssetIDLabelFromAssetTypeName;
      displayRequesterInfoByRequester = function(str) {
        called = true;
        Assert.areEqual(expected, str);
      };
      setAssetIDLabelFromAssetTypeName = function() { /* noop */ };
      setup$('lblRequestedBy', lblRequestedBy);
      setup$('lblAssetType', new MockDiv());

      initializeReadOnly();
      Assert.isTrue(called);

      setAssetIDLabelFromAssetTypeName = oldSetAssetIDLabelFromAssetTypeName;
      displayRequesterInfoByRequester = oldDisplayRequesterInfoByRequester;
    },

    testSetAssetIDLabelFromAssetTypeNameDoesJustThat: function() {
      var assetTypeName = 'Valve';
      var expected = assetTypeName + ' ID:';
      var divAssetLabel = new MockDiv();
      setup$('#divAssetLabel', divAssetLabel);

      setAssetIDLabelFromAssetTypeName(assetTypeName);

      Assert.areEqual(expected, divAssetLabel.innerText);
    },

    testDisplayRequesterInfoByRequesterDisplaysCustomerInfoWhenRequesterIsCustomer: function() {
      var requester = 'Customer';
      var called = false;
      var oldFn = displayCustomerInfo;
      displayCustomerInfo = function() { called = true; };

      displayRequesterInfoByRequester(requester);
      Assert.isTrue(called);

      displayCustomerInfo = oldFn;
    },

    testDisplayCustomerInfoCallsToggleElementArrayWithCorrectArguments: function() {
      var lblCustomerName = new MockElement();
      var trCustomerInfo = new MockElement();
      var correct = false;
      var oldFn = toggleElementArray;
      toggleElementArray = function(visible, arr) {
        correct = (visible == true &&
                          arr[0][0] == lblCustomerName &&
                          arr[1][0] == trCustomerInfo);
      };
      setup$('lblCustomerName', lblCustomerName);
      setup$('.trCustomerInfo', trCustomerInfo);
      setup$('#divRequesterLabel', new MockElement());

      displayCustomerInfo(true);
      Assert.isTrue(correct);

      toggleElementArray = oldFn;
    },

    testDisplayCustomerInfoSetsRequesterLabelToCustomer: function() {
      var expected = 'Customer Name: ';
      var divRequesterLabel = new MockDiv();
      var elem = new MockElement();
      var oldFn = toggleElementArray;
      toggleElementArray = function() { /* noop */ };
      setup$('#divRequesterLabel', divRequesterLabel);
      setup$('lblCustomerName', elem);
      setup$('.trCustomerInfo', elem);

      displayCustomerInfo();
      Assert.areEqual(expected, divRequesterLabel.innerText);

      toggleElementArray = oldFn;
    },

    //////////////////////////RE-USABLE FUNCTIONALITY//////////////////////////

    testToCalendarControlDateStringFormatsDateProperly: function() {
      var date = new Date('Tue Jun 6 2006');
      var expected = '06/6/06';

      Assert.areEqual(expected, toCalendarControlDateString(date));
    },

    testGetSelectedTextReturnsTextPropertyFromSelectedOption: function() {
      var expected = 'foobar';
      var ddlTest = new MockSelect({
        options: [null, new MockOption(expected, 0)],
        selectedIndex: 1
      });

      Assert.areEqual(expected, getSelectedText(jQueryMock(ddlTest)));
    },

    testGetSelectedTextReturnsNullWhenSelectedIndexNotSet: function() {
      var expected = null;
      var ddlTest = new MockSelect({
        selectedIndex: -1
      });

      Assert.areEqual(expected, getSelectedText(jQueryMock(ddlTest)));
    },

    testGetSelectedTextReturnsNullWhenSelectedIndexGreaterThanOrEqualToNumberOfOptions: function() {
      var expected = null;
      var ddlTest = new MockSelect({
        options: [new MockOption(expected, 0)],
        selectedIndex: 2
      });

      // greater than
      Assert.areEqual(expected, getSelectedText(jQueryMock(ddlTest)));

      ddlTest.selectedIndex = 1;

      // equal to
      Assert.areEqual(expected, getSelectedText(jQueryMock(ddlTest)));
    }
  }
};
