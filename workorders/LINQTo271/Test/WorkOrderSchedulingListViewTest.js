/// <reference path="../Views/WorkOrders/Scheduling/WorkOrderSchedulingListView.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />

WorkOrderSchedulingTests.WorkOrderSchedulingListViewTest = {
  testBtnAssignClickCallsOnAssignClicked: function() {
    var called = false;
    var oldFn = onAssignClicked;
    onAssignClicked = function() {
      called = true;
    };

    btnAssign_Click();

    onAssignClicked = oldFn;
  },

  testBtnAssignClickReturnsTheReturnValueOfOnAssignClicked: function() {
    var expected;
    var oldFn = onAssignClicked;
    var setReturnValueForFn = function(val) {
      onAssignClicked = function() { return val; };
    };

    expected = true;
    setReturnValueForFn(expected);
    Assert.areEqual(expected, btnAssign_Click());

    expected = false;
    setReturnValueForFn(expected);
    Assert.areEqual(expected, btnAssign_Click());

    onAssignClicked = oldFn;
  },

  testOnAssignClickPromptsFocusesAndReturnsFalseWhenNoCrewChosen: function() {
    var ddlCrewID = new MockSelect({
      selectedIndex: 0
    });
    var ccAssignmentDate = new MockTextInput({ value: '1/1/2008' });
    var oldVerifyWorkOrdersChosen = verifyWorkOrdersChosen;
    verifyWorkOrdersChosen = function() { return true; };
    var oldAlert = alert;
    alert = function(str) {
      Assert.areEqual('Please choose a crew.', str);
    };
    setup$('ddlCrewID', ddlCrewID);
    setup$('ccAssignmentDate', ccAssignmentDate);

    Assert.isFalse(onAssignClicked());
    Assert.isTrue(ddlCrewID.focused);

    alert = oldAlert;
    verifyWorkOrdersChosen = oldVerifyWorkOrdersChosen;
  },

  testOnAssignClickPromptsFocusesAndReturnsFalseWhenNoDateEntered: function() {
    var ddlCrewID = new MockSelect({
      selectedIndex: 1
    });
    var ccAssignmentDate = new MockTextInput();
    var oldVerifyWorkOrdersChosen = verifyWorkOrdersChosen;
    verifyWorkOrdersChosen = function() { return true; };
    var oldAlert = alert;
    alert = function(str) {
      Assert.areEqual('Please enter the date for the assignment.', str);
    };
    setup$('ddlCrewID', ddlCrewID);
    setup$('ccAssignmentDate', ccAssignmentDate);

    Assert.isFalse(onAssignClicked());
    Assert.isTrue(ccAssignmentDate.focused);

    alert = oldAlert;
    verifyWorkOrdersChosen = oldVerifyWorkOrdersChosen;
  },

  testOnAssignClickPromptsAndReturnsFalseWhenNoWorkOrderChosen: function() {
    var ddlCrewID = new MockSelect({
      selectedIndex: 1
    });
    var ccAssignmentDate = new MockTextInput({ value: '1/1/2008' });
    var oldVerifyWorkOrdersChosen = verifyWorkOrdersChosen;
    verifyWorkOrdersChosen = function() { return false; };
    var oldAlert = alert;
    alert = function(str) {
      Assert.areEqual('Please select at least one Work Order.', str);
    };
    setup$('ddlCrewID', ddlCrewID);
    setup$('ccAssignmentDate', ccAssignmentDate);

    Assert.isFalse(onAssignClicked());

    alert = oldAlert;
    verifyWorkOrdersChosen = oldVerifyWorkOrdersChosen;
  },

  testVerifyWorkOrderChosenReturnsTrueWhenAtLeastOneCheckBoxIsChecked: function() {
    var gvWorkOrders = new MockTable();
    // this relies on the function looking for the result
    // of the call to find being an array of at least 1
    var oldGetCheckedRows = getCheckedRows;
    getCheckedRows = function() { return [null] };
    setup$('gvWorkOrders', jQueryMock(gvWorkOrders));

    Assert.isTrue(verifyWorkOrdersChosen());

    getCheckedRows = oldGetCheckedRows;
  },

  testVerifyWorkOrderChosenReturnsFalseWhenNoCheckBoxesAreChecked: function() {
    var gvWorkOrders = new MockTable();
    var oldGetCheckedRows = getCheckedRows;
    getCheckedRows = function() { return []; };
    setup$('gvWorkOrders', jQueryMock(gvWorkOrders));

    Assert.isFalse(verifyWorkOrdersChosen());

    getCheckedRows = oldGetCheckedRows;
  }
};
