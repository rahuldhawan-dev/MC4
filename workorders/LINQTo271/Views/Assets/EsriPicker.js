var EsriPicker = {
  DEFAULTS: {
    lat: 40.32246702,
    lng: -74.14810180
  },
  SELECTORS: {
    MAP: '#map'
  },
  MAP_ID: Application.MAP_ID,
  DEFAULT_ZOOM_LEVEL: 13,

  //////////////////////////////////GETTERS/////////////////////////////////

  getDocumentURL: function() {
    return document.URL.toString();
  },

  getAssetTypeID: function() {
    return parseInt(this.getDocumentURL().match(/assetTypeID=(\d+)/)[1], 10);
  },

  getPhysicalAssetTypeIDList: function() {
    return [ASSET_TYPE_IDS.VALVE,
            ASSET_TYPE_IDS.HYDRANT,
            ASSET_TYPE_IDS.SEWER_OPENING,
            ASSET_TYPE_IDS.STORM_CATCH];
  },

  getCenterMarker: function() {
    if (!EsriPicker.centerMarker) {
      var center = EsriPicker.map.esriMap('getCenter');
      EsriPicker.centerMarker = EsriPicker.map.esriMap('addSimpleMarker', center.lng, center.lat);
      // TODO: This thing doesn't work at all.
      // map.esriMap('makeDraggable', EsriPicker.centerMarker, EsriPicker.centerMarker_dragend);
    }
    return EsriPicker.centerMarker;
  },

  //////////////////////////////////SETTERS/////////////////////////////////

  setCenterMarker: function(latLng) {
    EsriPicker.map.esriMap('moveMarker', EsriPicker.getCenterMarker(), latLng.lng, latLng.lat);

    if (latLng.lat != '0' && latLng.lng != '0') {
      getServerElement('hidLatitude').val(latLng.lat);
      getServerElement('hidLongitude').val(latLng.lng);
    }
  },

  setReturnLatLng: function(latLng) {
    if (window.top.setLat && window.top.setLon) {
      window.top.setLat(latLng.lat);
      window.top.setLon(latLng.lng);
    }
  },

  setAssetCoordinates: function() {
    if (this.areNewCoordinatesChosen()) {
      this.setReturn();
      return true;
    }
    return false;
  },

  setWorkOrderCoordinates: function() {
    if (this.areNewCoordinatesChosen()) {
    	this.setReturn();
    	this.closeWindow();
    }
    return false;
  },

  setMapCenter: function(latLng) {
    EsriPicker.map.esriMap('centerAt', latLng.lng, latLng.lat);
  },

  /////////////////////////////HELPER FUNCTIONS/////////////////////////////

  isNaNOrEmptyString: function(str) {
    return isNaN(str) || (str == '');
  },

  closeWindow: function() {
    window.top.document.getElementById('btnClose').click();
  },

  areNewCoordinatesChosen: function() {
    var lat = getServerElement('hidLatitude').val();
    var lng = getServerElement('hidLongitude').val();

    return !(lat == this.DEFAULTS.lat && lng == this.DEFAULTS.lng);
  },

  loadMap: function() {
    var lat = getServerElement('hidLatitude').val() || EsriPicker.DEFAULTS.lat;
    var lng = getServerElement('hidLongitude').val() || EsriPicker.DEFAULTS.lng;
    MapMenu.initialize();
    
    $(this.SELECTORS.MAP).esriMap({
      mapId: this.MAP_ID,
      initialZoom: this.DEFAULT_ZOOM_LEVEL,
      center: { lat: lat, lng: lng },
      legendSelector: 'div#gisLegend',
      layerToggleSelector: 'div#mapGISLayers',
      basemapGallerySelector: 'div#basemapSelector',
      showBaseMapToggle: false,
      startWithLayersOff: true,
      defaultBaseLayers: ['Threat Alerts'],
      proxyOptions: {
        url: '/proxies/ESRIProxy.ashx',
        proxies: ['www.arcgis.com', 'utility.arcgis.com', 'geoprocess-np.amwater.com', 'awdev.maps.arcgis.com', 'geoprocess.amwater.com', 'aw.maps.argis.com', 'onemap-np.amwaternp.com', 'onemap.amwater.com']
      },
      callback: function(map) {
        EsriPicker.map = map;
        // Ensure the center marker is created *before* geocoding or else we can't make it draggable.
        var centerMaker = EsriPicker.getCenterMarker();
        EsriPicker.map.esriMap('disableDoubleClickZoom', EsriPicker.map_doubleClick);
        if ((lat == EsriPicker.DEFAULTS.lat.toString() && lng == EsriPicker.DEFAULTS.lng.toString()) || (lat == 0 && lng == 0)) {
          EsriPicker.geocodeLocation();
        }
      }
    });
  },

  geocodeLocation: function() {
    var location = jQuery('#txtLocation').val();
    if (location) {
      EsriPicker.map.esriMap('locateAddress', location, EsriPicker.getGeocoder_callback(location));
    }
  },

  setReturn: function() {
    var lat = getServerElement('hidLatitude').val();
    var lng = getServerElement('hidLongitude').val();
    //var latLng = this.getCenterMarker().getPosition();
    //this.setCenterMarker(latLng);
    this.setReturnLatLng({lat: lat, lng: lng});
  },

  delayAndSetMapCenter: function(latLng) {
    setTimeout(function() {
      EsriPicker.setMapCenter(latLng);
      EsriPicker.setCenterMarker(latLng);
    }, 500);
  },

  //////////////////////////////EVENT HANDLERS//////////////////////////////

  btnSave_Click: function() {
    var that = EsriPicker;
    var fn = (jQuery.inArray(that.getAssetTypeID.call(that), that.getPhysicalAssetTypeIDList.call(that)) > -1) ?
      that.setAssetCoordinates : that.setWorkOrderCoordinates;
    return fn.call(that);
  },

  centerMarker_dragend: function(latLng) {
    EsriPicker.delayAndSetMapCenter(latLng);
  },

  getGeocoder_callback: function(location) {
    return function(latLng) {
      if (latLng) {
        EsriPicker.setMapCenter(latLng);
        EsriPicker.setCenterMarker(latLng);
      } else {
        alert('Address \'' + location +
          '\' was not found. Please alter your search.');
      }
    };
  },

  map_doubleClick: function(latLng) {
    EsriPicker.setMapCenter(latLng);
    EsriPicker.setCenterMarker(latLng);
  }
};
