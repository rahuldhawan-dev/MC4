/// <reference path="GooglePickerTest.aspx" />
/// <reference path="../Views/Assets/GooglePicker.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />

var GooglePickerTests = {
  GooglePickerTest: {
    /* btnSave_Click */
    testBtnSave_ClickReturnsResultOfCallToConfirm: function() {
      var expected = true;
      var oldFn = confirm;
      confirm = function() { return expected; };

      Assert.isTrue(btnSave_Click());

      expected = false;

      Assert.isFalse(btnSave_Click());

      confirm = oldFn;
    },

    /* isNaNOrEmptyString */
    testIsNaNOrEmptyStringReturnFalseWhenStringContainsANumber: function() {
      var num = '1';

      Assert.isFalse(isNaNOrEmptyString(num));

      num = '2.1';

      Assert.isFalse(isNaNOrEmptyString(num));
    },

    testIsNaNOrEmptyStringReturnsTrueWhenStringContainsNonNumber: function() {
      var nonNum = '1a';

      Assert.isTrue(isNaNOrEmptyString(nonNum));

      nonNum = 'this is not a number either';

      Assert.isTrue(isNaNOrEmptyString(nonNum));
    },

    testIsNaNOrEmptyStringReturnsTrueWhenStringIsEmpty: function() {
      var empty = '';

      Assert.isTrue(isNaNOrEmptyString(empty));
    },

    /* getMap */
    testGetMapCallsGMapContstructorWhenGlobalMapVariableIsNullOrUndefined: function() {
      var called = false;
      var mapDiv = new MockDiv();
      var oldFn = GMap2;
      GMap2 = function(obj) {
        this.map = obj;
        called = true;
        return true;
      };
      var oldMap = map;
      map = undefined;
      setup$('#map', mapDiv);

      var gmap = getMap();

      Assert.isTrue(called);
      Assert.areEqual(mapDiv, gmap.map);
      Assert.areEqual(gmap, map);

      called = false;
      map = null;
      gmap = getMap();

      Assert.isTrue(called);
      Assert.areEqual(mapDiv, gmap.map);
      Assert.areEqual(gmap, map);

      GMap2 = oldFn;
      map = oldMap;
    },

    testGetMapSimplyReturnsGlobalMapObjectWhenDefinedAndNotNull: function() {
      var expected = 'type matters not in this land';
      var called = false;
      var oldFn = GMap2;
      GMap2 = function(obj) {
        this.map = obj;
        called = true;
        return true;
      };
      var oldMap = map;
      map = expected;

      var gmap = getMap();

      Assert.isFalse(called);
      Assert.areEqual(expected, gmap);

      GMap2 = oldFn;
      map = oldMap;
    }
  }
};