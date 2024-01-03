/// <reference path="../Views/WorkOrders/Scheduling/WorkOrderSchedulingSearchView.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />

var WorkOrderSchedulingTests = {};

WorkOrderSchedulingTests.WorkOrderSchedulingSearchViewTest = {
  testDdlMarkoutRequiredChangeFiresOnDdlMarkoutRequiredChanged: function() {
    var expectedValue = 'Foo';
    var ddlMarkoutRequired = new MockSelect({
      options: [new MockOption(expectedValue, expectedValue)],
      selectedIndex: 0
    });
    var called = false;
    var oldFn = onDdlMarkoutRequiredChanged;
    onDdlMarkoutRequiredChanged = function(value) {
      called = true;
      Assert.areEqual(expectedValue, value);
    };

    ddlMarkoutRequired_Change(ddlMarkoutRequired);

    Assert.isTrue(called);

    onDdlMarkoutRequiredChanged = oldFn;
  },

  testOnDdlMarkoutRequiredChangedHidesReadyDateRowWhenValueIsEmpty: function() {
    var trMarkoutReadyDate = new MockTableRow();
    setup$('#trMarkoutReadyDate', trMarkoutReadyDate);

    onDdlMarkoutRequiredChanged('');

    Assert.areEqual('none', trMarkoutReadyDate.style.display);
  },

  testOnDdlMarkoutRequiredChangedHidesReadyDateRowWhenValueIsFalse: function() {
    var trMarkoutReadyDate = new MockTableRow();
    setup$('#trMarkoutReadyDate', trMarkoutReadyDate);

    onDdlMarkoutRequiredChanged('false');

    Assert.areEqual('none', trMarkoutReadyDate.style.display);
  },

  testOnDdlMarkoutRequiredChangedShowsReadyDateRowWhenValueIsTrue: function() {
    var trMarkoutReadyDate = new MockTableRow({style: {display: 'none'}});
    setup$('#trMarkoutReadyDate', trMarkoutReadyDate);

    onDdlMarkoutRequiredChanged('true');

    Assert.areEqual('', trMarkoutReadyDate.style.display);
  }
};
