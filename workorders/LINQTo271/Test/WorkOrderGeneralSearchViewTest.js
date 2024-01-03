/// <reference path="../Views/WorkOrders/General/WorkOrderGeneralSearchView.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />

var WorkOrderGeneralTests = {};

WorkOrderGeneralTests.WorkOrderGeneralSearchViewTest = {
  setup: function() {
    // asp.net elements
    var ddlAssetType = new MockSelect();
    // html elements
    var tdAssetIDLabel = new MockTableCell();
    var trAssetID = new MockTableRow();

    setup$('ddlAssetType', ddlAssetType);
    setup$('#tdAssetIDLabel', tdAssetIDLabel);
    setup$('#trAssetID', trAssetID);

    return {
      ddlAssetType: ddlAssetType,
      tdAssetIDLabel: tdAssetIDLabel,
      trAssetID: trAssetID
    };
  },

  teardown: function() {
    setup$('ddlAssetType', null);
    setup$('#tdAssetIDLabel', null);
    setup$('#trAssetID', null);
  },

  ///////////////////////////////////////////////////////////////////////////
  //////////////////////////////EVENT HANDLERS///////////////////////////////
  ///////////////////////////////////////////////////////////////////////////
  testDdlAssetTypeChangeCallsOnAssetTypeChanged: function(params) {
    var elem = params.ddlAssetType;
    var expected = 1;
    elem.selectedIndex = expected;
    var called = false;
    var oldFn = onAssetTypeChanged;
    onAssetTypeChanged = function(id) {
      called = true;
      Assert.areEqual(expected, id);
    };

    ddlAssetType_Change(elem);

    Assert.isTrue(called);

    onAssetTypeChanged = oldFn;
  },

  ///////////////////////////////////////////////////////////////////////////
  ////////////////////////////EVENT PASSTHROUGHS/////////////////////////////
  ///////////////////////////////////////////////////////////////////////////
  testOnAssetTypeChangedSetsAssetIDLabelAndShowsAssetIDRowWhenChosenTypeIsValve: function(params) {
    var expectedLabel = 'Valve ID:';
    var expectedVisible = true;
    var toggleAssetIDRowCalled = false,
      changeAssetIDLabelCalled = false;
    var oldToggleAssetIDRow = toggleAssetIDRow;
    var oldChangeAssetIDLabel = changeAssetIDLabel;
    toggleAssetIDRow = function(visible) {
      toggleAssetIDRowCalled = true;
      Assert.areEqual(expectedVisible, visible);
    };
    changeAssetIDLabel = function(str) {
      changeAssetIDLabelCalled = true;
      Assert.areEqual(expectedLabel, str);
    };

    onAssetTypeChanged(ASSET_TYPE_IDS.VALVE);

    Assert.isTrue(toggleAssetIDRowCalled && changeAssetIDLabelCalled);

    toggleAssetIDRow = oldToggleAssetIDRow;
    changeAssetIDLabel = oldChangeAssetIDLabel;
  },

  testOnAssetTypeChangedSetsAssetIDLabelAndShowsAssetIDRowWhenChosenTypeIsHydrant: function(params) {
    var expectedLabel = 'Hydrant ID:';
    var expectedVisible = true;
    var toggleAssetIDRowCalled = false,
      changeAssetIDLabelCalled = false;
    var oldToggleAssetIDRow = toggleAssetIDRow;
    var oldChangeAssetIDLabel = changeAssetIDLabel;
    toggleAssetIDRow = function(visible) {
      toggleAssetIDRowCalled = true;
      Assert.areEqual(expectedVisible, visible);
    };
    changeAssetIDLabel = function(str) {
      changeAssetIDLabelCalled = true;
      Assert.areEqual(expectedLabel, str);
    };

    onAssetTypeChanged(ASSET_TYPE_IDS.HYDRANT);

    Assert.isTrue(toggleAssetIDRowCalled && changeAssetIDLabelCalled);

    toggleAssetIDRow = oldToggleAssetIDRow;
    changeAssetIDLabel = oldChangeAssetIDLabel;
  },

  testOnAssetTypeChangedSetsAssetIDLabelAndShowsAssetIDRowWhenChosenTypeIsService: function(params) {
    var expectedLabel = 'Premise Number:';
    var expectedVisible = true;
    var toggleAssetIDRowCalled = false,
      changeAssetIDLabelCalled = false;
    var oldToggleAssetIDRow = toggleAssetIDRow;
    var oldChangeAssetIDLabel = changeAssetIDLabel;
    toggleAssetIDRow = function(visible) {
      toggleAssetIDRowCalled = true;
      Assert.areEqual(expectedVisible, visible);
    };
    changeAssetIDLabel = function(str) {
      changeAssetIDLabelCalled = true;
      Assert.areEqual(expectedLabel, str);
    };

    onAssetTypeChanged(ASSET_TYPE_IDS.SERVICE);

    Assert.isTrue(toggleAssetIDRowCalled && changeAssetIDLabelCalled);

    toggleAssetIDRow = oldToggleAssetIDRow;
    changeAssetIDLabel = oldChangeAssetIDLabel;
  },

  testOnAssetTypeChangedHidesAssetIDRowWhenChosenTypeIsNotValveHydrantOrService: function() {
    var called;
    var oldFn = toggleAssetIDRow;
    toggleAssetIDRow = function(show) {
      called = true;
      Assert.isFalse(show);
    };

    for (var x in ASSET_TYPE_IDS) {
      if (x == 'VALVE' || x == 'HYDRANT' || x == 'SERVICE') continue;
      called = false;

      onAssetTypeChanged(ASSET_TYPE_IDS[x]);

      Assert.isTrue(called);
    }

    toggleAssetIDRow = oldFn;
  },

  ///////////////////////////////////////////////////////////////////////////
  ////////////////////////////////UI HELPERS/////////////////////////////////
  ///////////////////////////////////////////////////////////////////////////
  testToggleAssetIDRowShowsRowWhenArgumentIsTrue: function(params) {
    var trAssetID = params.trAssetID;
    trAssetID.style.display = 'none';

    toggleAssetIDRow(true);

    Assert.areEqual('', trAssetID.style.display);
  },

  testToggleAssetIDRowHidesRowWhenArgumentIsFalse: function(params) {
    var trAssetID = params.trAssetID;
    trAssetID.style.display = '';

    toggleAssetIDRow(false);

    Assert.areEqual('none', trAssetID.style.display);
  },

  testChangeAssetIDLabelChangesTextOfAssetIDLabelCell: function(params) {
    var tdAssetIDLabel = params.tdAssetIDLabel;
    var expected = 'test string';

    changeAssetIDLabel(expected);

    Assert.areEqual(expected, $(tdAssetIDLabel).text());
  }
};
