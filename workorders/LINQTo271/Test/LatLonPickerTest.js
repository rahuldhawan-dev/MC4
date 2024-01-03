/// <reference path="../Common/LatLonPicker.js" />
/// <reference path="assertions.js" />
/// <reference path="mocking.js" />
/// <reference path="LatLonPickerTest.aspx" />

var LatLonPickerTests = {
  ImageButtonTest: {
    testSetIconChangesImageToRedWhenPickerHasCoordinatesReturnsFalse: function() {
      var expected = 'red';
      var img = new MockImage({ src: 'blue' });
      var oldFn = LatLonPicker.Picker.hasCoordinates;
      LatLonPicker.Picker.hasCoordinates = function() { return false; };

      LatLonPicker.ImageButton.setIcon($(img), null, null);

      Assert.areEqual(expected, img.src);

      LatLonPicker.Picker.hasCoordinates = oldFn;
    },

    testSetIconChangesImageToBlueWhenPickerHasCoordinatesReturnsTrue: function() {
      var expected = 'blue';
      var img = new MockImage({ src: 'red' });
      var oldFn = LatLonPicker.Picker.hasCoordinates;
      LatLonPicker.Picker.hasCoordinates = function() { return true; };

      LatLonPicker.ImageButton.setIcon($(img), null, null);

      Assert.areEqual(expected, img.src);

      LatLonPicker.Picker.hasCoordinates = oldFn;
    }
  },

  PickerTest: {
    testInitializeInitializesJQueryModalOnCorrectElementWithCorrectArguments: function() {
      var jqmCalled = false, showPickerCalled = false;
      var jqWindow = new MockDiv();
      var jqTrigger = new MockImage();
      var jqFrame = new MockIFrame();
      var jqAssetID = new MockTextInput({ value: 1 });
      var jqAssetTypeID = new MockTextInput({ value: 1 });
      var jqAddress = new MockTextInput();
      var expected = {
        modal: true,
        trigger: jqTrigger
      };
      var oldShowPicker = LatLonPicker.Picker.showPicker;
      LatLonPicker.Picker.showPicker = function() { showPickerCalled = true; };
      var testFn;
      jqWindow.jqm = function(obj) {
        jqmCalled = true;
        if (obj === undefined) {
          Assert.fail('jqm should not be called with no argument.');
        } else {
          for (var x in expected) {
            if (obj[x] === undefined) {
              Assert.fail('obj[' + x + '] was not passed to jqm as expected.');
            } else {
              Assert.areEqual(expected[x], obj[x]);
            }
            testFn = obj.onShow;
          }
        }
      }

      LatLonPicker.Picker.initialize(jqWindow, jqTrigger, jqFrame, jqAssetID,
        jqAssetTypeID, jqAddress);

      Assert.isTrue(jqmCalled,
        'LatLonPicker.Picker.initialize should call the jqm method on the jqWindow object.');

      testFn();

      Assert.isTrue(showPickerCalled,
        'LatLonPicker.Picker.initialize should pass a closure to call LatLonPicker.Picker.showPicker to the onShow property.');

      LatLonPicker.Picker.showPicker = oldShowPicker;
    },

    testGenerateUrlGeneratesQueryStringValuesBasedOnAssetAndTypeIDsAndAddress: function() {
      var expected = LatLonPicker.Picker.url + '?assetID=1&assetTypeID=1&location=foo';
      var hidAssetID = new MockTextInput({ value: 1 });
      var hidAssetTypeID = new MockTextInput({ value: 1 });
      var hidAddress = new MockTextInput({ value: 'foo' });

      Assert.areEqual(expected,
        LatLonPicker.Picker.generateUrl(hidAssetID, hidAssetTypeID, hidAddress));
    },

    testShowPickerSetsUrlOfPickerIFrameAndCallsShowOnWindow: function() {
      var showCalled = false;
      var expected = LatLonPicker.Picker.url + '?assetID=1&assetTypeID=1&location=foo';
      var jqWindow = new MockDiv();
      var jqFrame = new MockIFrame();
      var hidAssetID = new MockTextInput({ value: 1 });
      var hidAssetTypeID = new MockTextInput({ value: 1 });
      var hidAddress = new MockTextInput({ value: 'foo' });
      jqWindow.show = function() { showCalled = true; };

      LatLonPicker.Picker.showPicker({ w: jqWindow }, $(jqFrame),
        $(hidAssetID), $(hidAssetTypeID), $(hidAddress));

      Assert.isTrue(showCalled);
      Assert.areEqual(expected, jqFrame.src);
    },

    testHasCoordinatesReturnsFalseWhenNoLatitudeOrLongitude: function() {
      var hidLatitude = new MockTextInput();
      var hidLongitude = new MockTextInput();

      Assert.isFalse(LatLonPicker.Picker.hasCoordinates(hidLatitude,
        hidLongitude));

      hidLatitude.value = 1.1;

      Assert.isFalse(LatLonPicker.Picker.hasCoordinates(hidLatitude,
        hidLongitude));

      hidLatitude.value = '';
      hidLongitude.value = 1.1;

      Assert.isFalse(LatLonPicker.Picker.hasCoordinates(hidLatitude,
        hidLongitude));
    },

    testHasCoordinatesReturnsTrueWhenLatitudeAndLongitudeAreSet: function() {
      var hidLatitude = new MockTextInput({ value: 1.1 });
      var hidLongitude = new MockTextInput({ value: 1.1 });

      Assert.isTrue(LatLonPicker.Picker.hasCoordinates(hidLatitude,
        hidLongitude));
    }
  }
};
