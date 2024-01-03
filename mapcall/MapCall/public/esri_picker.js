var EsriPicker = (function ($) {
  var picker = {
  	MAP_ID: Application.MAP_ID,

    DEFAULTS: {
      LATITUDE: 40.32246702,
      LONGITUDE: -74.14810180,
      ZOOM_LEVEL: 13
    },

    SELECTORS: {
      MAP_DIV: 'div#map',
      ADDRESS: 'input#txtLocation',
      FIND_BUTTON: 'button#btnGeoCode',
      SAVE_BUTTON: 'button#btnSave',
      LEGEND_HEADER: 'div.gis-legend-heading',
      LEGEND_SECTION: 'div.gis-legend-section'
    },

    currentLatLng: null,

    initialize: function () {
      MapMenu.initialize();
      picker.initMap();
      picker.initEvents();
    },

    initMap: function () {
      picker.initialCoordinates = picker.getLatLng();
      picker.map = $(picker.SELECTORS.MAP_DIV).esriMap({
        mapId: picker.MAP_ID,
        initialZoom: picker.DEFAULTS.ZOOM_LEVEL,
        center: picker.initialCoordinates,
        callback: picker.onMapCreated,
        legendSelector: 'div#gisLegend',
        layerToggleSelector: 'div#mapGISLayers',
        basemapGallerySelector: 'div#basemapSelector',
        showBaseMapToggle: false,
        startWithLayersOff: true,
        proxyOptions: {
        	url: '/proxies/ESRIProxy.ashx',
          proxies: ['www.arcgis.com', 'services1.arcgis.com', 'tiles1.arcgis.com', 'tiles2.arcgis.com', 'services2.arcgis.com', 'utility.arcgis.com', 'onemap-np.amwaternp.com', 'onemap.amwater.com']
        }
      });
    },

    initEvents: function () {
      $(picker.SELECTORS.FIND_BUTTON).click(picker.geocodeLocation);
      $(picker.SELECTORS.LEGEND_HEADER).click(picker.legendHeader_click);
    },
  	
    legendHeader_click: function (e) {
      $(e.target).toggleClass('open')
        .next(picker.SELECTORS.LEGEND_SECTION).toggle();
    },

    centerMarker_dragend: function (latLng) {
      picker.delayAndSetMapCenter(latLng);
    },

    createCenterMarker: function ($icon) {
      var center = picker.getLatLng();
      var marker = picker.map.esriMap('addMarkerImage', $icon.attr('src'), $icon.width(),
                                          $icon.height(), $icon.attr('data-icon-offset'),
                                          center.lng, center.lat);
      picker.map.esriMap('makeDraggable', marker, picker.centerMarker_dragend);
      return marker;
    },

    delayAndSetMapCenter: function (latLng) {
      setTimeout(function () {
        picker.setMapCenter(latLng);
      }, 500);
    },

    geocodeLocation: function () {
      var location = $(picker.SELECTORS.ADDRESS).val();
      if (location) {
        picker.map.esriMap('locateAddress', location,
          picker.getGeocoderCallback(location));
      }
    },

    getGeocoderCallback: function (location) {
      return function (latLng) {
        if (latLng) {
          picker.setMapCenter(latLng);
          picker.setCenterMarker(latLng);
        } else {
          alert('Address \'' + location +
            '\' was not found. Please alter your search.');
        }
      };
    },

    getCenterMarker: function ($icon) {
      if (!picker.centerMarker) {
        picker.centerMarker = picker.createCenterMarker($icon);
      }
      return picker.centerMarker;
    },

    getDefaultIconInSet: function () {
      return $('img[data-default-icon="true"]');
    },

    getLatLng: function () {
      if ($('#hidDefaultLatitude').val() !== '')
        EsriPicker.DEFAULTS.LATITUDE = $('#hidDefaultLatitude').val();
      if ($('#hidDefaultLongitude').val() !== '')
        EsriPicker.DEFAULTS.LONGITUDE = $('#hidDefaultLongitude').val();

      return {
        lat: parseFloat(picker.getLatitude().value  || picker.DEFAULTS.LATITUDE),
        lng: parseFloat(picker.getLongitude().value || picker.DEFAULTS.LONGITUDE)
      };
    },

    getLatitude: function () {
      if (picker.latitude == null) {
        var opener = picker.getOpener();
        picker.latitude = opener.getElementById('txtLatitude') || opener.txtLat ||
                        opener.getElementById('ctl00_cphMain_DataElement1_DetailsView1_txtLatCoordinateID_-1') ||
                        opener.getElementById('ctl00_cphMain_DataElement1_DetailsView1_txtLatCoordinateID_0');
      }

      return picker.latitude;
    },
    getLongitude: function () {
      if (picker.longitude == null) {
        var opener = picker.getOpener();
        picker.longitude = opener.getElementById('txtLongitude') || opener.txtLon ||
                         opener.getElementById('ctl00_cphMain_DataElement1_DetailsView1_txtLonCoordinateID_-1') ||
                         opener.getElementById('ctl00_cphMain_DataElement1_DetailsView1_txtLonCoordinateID_0');
      }

      return picker.longitude;
    },

    getOpener: function () {
      if (picker.opener == null) {
        picker.opener = (window.top.frames[1]) ? window.top.frames[1].document : window.top.document;
      }
      return picker.opener;
    },

    hidePicker: function () {
      picker.getOpener().lightview.hide();
    },

    map_doubleClick: function (latLng) {
      picker.setMapCenter(latLng);
    },

    onMapCreated: function (map) {
      map.esriMap('disableDoubleClickZoom', picker.map_doubleClick);
      picker.setCenterIcon();
      // There might be an address pre-populated, so we want the map to select that.
      picker.geocodeLocation();
    },

    saveCoordinates: function () {
      picker.getLatitude().value = picker.currentLatLng.lat;
      picker.getLongitude().value = picker.currentLatLng.lng;
      picker.getOpener().lightview.hide();
    },

    setCenterIcon: function () {
      var icon = picker.getDefaultIconInSet();
      picker.getCenterMarker(icon);
      picker.map.esriMap('changeMarkerIcon', picker.centerMarker, icon.attr('src'),
        icon.width(), icon.height(), icon.attr('data-icon-offset'));
    },

    setCenterMarker: function (latLng) {
      picker.map.esriMap('moveMarker', picker.centerMarker, latLng.lng, latLng.lat);
      picker.setLatLng(latLng);
    },

    setLatLng: function (latLng) {
      picker.currentLatLng = latLng;
    },

    setMapCenter: function (latLng) {
      picker.map.esriMap('centerAt', latLng.lng, latLng.lat);
      picker.setCenterMarker(latLng);
    }
  };

  return picker;
})(jQuery);