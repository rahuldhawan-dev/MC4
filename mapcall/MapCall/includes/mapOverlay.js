var Maps = (function($) {
  return {
    MAP_ID: Application.MAP_ID,

    SELECTORS: {
      MAP: '#map',
      LOADING: '#loading',
      LOCATION: '#txtLocation',
      LOCATE: 'LocateButton'
    },
    _data: {},
    _icons: [],
    _map: null,
    _canAttachMarkers: false,

    init: function () {
      MapMenu.initialize();
      $(Maps.SELECTORS.MAP).esriMap({
        mapId: Maps.MAP_ID,
        callback: Maps.onMapCreated,
        legendSelector: 'div#gisLegend',
        layerToggleSelector: 'div#mapGISLayers',
        locateButton: Maps.SELECTORS.LOCATE,
        basemapGallerySelector: 'div#basemapSelector',
        showBaseMapToggle: false,
        startWithLayersOff: true,
				proxyOptions: {
      		url: '/proxies/ESRIProxy.ashx',
          proxies: ['www.arcgis.com', 'services1.arcgis.com', 'tiles1.arcgis.com', 'tiles2.arcgis.com', 'services2.arcgis.com', 'utility.arcgis.com', 'onemap-np.amwaternp.com', 'onemap.amwater.com']
				}
      });
    },

    getIconById: function(id) {
      for (var i = Maps._icons.length - 1; i >= 0; --i) {
        if (Maps._icons[i].id == id) {
          return Maps._icons[i];
        }
      }
      return null;
    },

    loadMapData: function(mapData) {
      if (mapData.coordinates.length == 0) {
        Maps.displayMessage('There are no valid points to display on this map.');
      } else {
        mapData.loaded = false;
        Maps._data[mapData.layerId] = mapData;

        if (Maps._canAttachMarkers) {
          Maps.loadPendingMapData();
        }
      }
    },

    loadPendingMapData: function() {
      for (var key in Maps._data) {
        var cur = Maps._data[key];
        if (!cur.loaded) {
          cur.loaded = true;
          Maps._map.esriMap('centerAt', cur.center.lng, cur.center.lat);
          Maps.attachMarkers(cur.coordinates, cur.layerId);
        }
      }
    },

    attachMarkers: function(coordinates, layerId) {
      Maps._map.esriMap('ensureGraphicsLayer', layerId);
      Maps._map.esriMap('onLayerClick', layerId, Maps.onMarkerClick);
      for (var i = coordinates.length - 1; i >= 0; --i) {
        var coordinate = coordinates[i];
        var icon = Maps.getIconById(coordinate.iconId);
        Maps._map.esriMap('addMarkerImageToLayer', layerId,
          icon.url, icon.width, icon.height, icon.offset, coordinate.lng, coordinate.lat,
          { url: coordinate.url });
      }
      Maps._map.esriMap('fitLayerBounds', layerId);
    },

    geocodeLocation: function() {
      var location = $(Maps.SELECTORS.LOCATION).val();
      if (location) {
        Maps._map.esriMap('locateAddress', location, null, true);
      }
    },

    onMapCreated: function(map) {
      Maps._canAttachMarkers = true;
      Maps._map = map;
      Maps.loadPendingMapData();
      $(Maps.SELECTORS.LOADING).hide();
    },

    onMarkerClick: function(e) {
      var frame = '<iframe style="width:100%; height:100%;" src="' + e.graphic.attributes.url + '"></iframe>';
      Maps._map.esriMap('displayInfoWindowWithContent', frame, e.mapPoint, e.screenPoint, 500, 300);
      e.stopPropagation();
    },

    displayMessage: function(msg) {
      alert(msg);
    },

    setExtent: function() {
      var extent = Maps._map.esriMap('getExtent');
      // This is based off of mapOverlay.js's setExtents method
      // which doesn't work properly, so this doesn't work properly
      // either.
      $('#txtSouthWestLat').val(extent.ymin);
      $('#txtSouthWestLng').val(extent.xmin);
      $('#txtNorthEastLat').val(extent.ymax);
      $('#txtNorthEastLng').val(extent.xmax);
    },

    toggleLayer: function(checkBoxId, buttonId, layerId) {
      if ($('#' + checkBoxId).is(':checked')) {
        Maps.setExtent();
        // If this is the default loaded data, we can just redisplay the existing data.
        // Otherwise, we need to click the button to do the UpdatePanel junk.
        if (buttonId) {
          $('#' + buttonId).click();
        } else {
          var mapData = Maps._data[layerId];
          mapData.loaded = false;
          Maps.loadMapData(mapData);
        }
      } else {
        if (Maps._map.esriMap('hasGraphicsLayer', layerId)) {
          Maps._map.esriMap('clearLayer', layerId);
        }
      }
    },
  };

})(jQuery);


function toggleDisplay(elementId) {
  if (document.getElementById(elementId).className == 'displayNone')
    document.getElementById(elementId).className = 'div' + elementId;
  else
    document.getElementById(elementId).className = 'displayNone';
}