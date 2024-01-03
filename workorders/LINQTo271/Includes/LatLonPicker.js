var LatLonPicker = {
  ImageButton: {
    setIcon: function(jqImg, jqLat, jqLon) {
      if (LatLonPicker.Picker.hasCoordinates(jqLat, jqLon)) {
        jqImg[0].src = jqImg[0].src.replace('red', 'blue');
      } else {
        jqImg[0].src = jqImg[0].src.replace('blue', 'red');
      }
    }
  },

  Picker: {
    url: '',

    initialize: function(jqWindow, jqTrigger, jqFrame, jqAssetID, jqAssetTypeID, jqLat, jqLon, jqAddress, handler) {
      jqWindow.jqm({
        modal: true,
        toTop: true, // needed to fix IE7 z-index ordering bug.
        trigger: jqTrigger,
        onShow: (function() {
          return function(obj) {
            if (handler == null || handler()) {
              LatLonPicker.Picker.showPicker(obj, jqFrame, jqAssetID, jqAssetTypeID, jqLat, jqLon, jqAddress);
            } else {
              $(jqWindow).jqmHide();
            }
          };
        })()
      });
    },

    generateUrl: function(jqAssetID, jqAssetTypeID, jqLat, jqLon, jqAddress) {
      return LatLonPicker.Picker.url + '?assetID=' + jqAssetID.val() +
        '&assetTypeID=' + jqAssetTypeID.val().toString() +
        '&location=' + jqAddress.val() +
        '&latitude=' + jqLat.val() + '&longitude=' + jqLon.val();
    },

    showPicker: function(obj, jqFrame, jqAssetID, jqAssetTypeID, jqLat, jqLon, jqAddress) {
      var url = this.generateUrl(jqAssetID, jqAssetTypeID, jqLat, jqLon, jqAddress);
      jqFrame[0].src = url;
      obj.w.show();
    },

    hasCoordinates: function(jqLat, jqLon) {
      return (jqLat.val() !== '' &&
              jqLon.val() !== '' &&
              jqLat.val() !== '0' &&
              jqLon.val() !== '0');
    }
  }
};
