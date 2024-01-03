var Maps = (function($) {

  var organizeConfigIcons = function(icons) {
    var organized = [];
    for (var i = 0; i < icons.length; i++) {
      var cur = icons[i];
      organized[cur.id] = cur;
    }
    return organized;
  };

  return {
    LAYER_ID: 'markers',

    SELECTORS: {
      MAP: '#Map',
      DEFAULT_LEGEND: '#default-legend',
      DEFAULT_LEGEND_HEADER: '#default-legend-header',
      GIS_LEGEND: 'div#gis-legend',
      GIS_LAYERS: 'div#gis-layers',
      BASEMAP_GALLERY: 'div#basemap-gallery',
      LOCATE: 'LocateButton' 
    },
    _icons: [],
    _layerIds: [],
    _map: null,
    _dataUrl: null,
    _legend: null,
    _legendGroups: {},
    _isLoadingAjaxData: false,
    _isMapCreated: false,

    // The graphic object of the last selected marker.
    lastSelectedMarker: null,

    onInit: function(config) {
      /* noop by default
       * config has an additionalData property that inheritors
       * might need to use during initialization. 
       */
    },

    init: function(config) {
      $(Maps.SELECTORS.DEFAULT_LEGEND_HEADER).hide();

      if (config.validationErrors) {
        var errMsg = '';
        for (var i = 0; i < config.validationErrors.length; i++) {
          errMsg = errMsg + config.validationErrors[i] + ' ';
        }
        Maps.displayMessage(errMsg);
      }
      else {
        Maps._dataUrl = config.dataUrl;
        Maps._icons = organizeConfigIcons(config.icons);
        Maps.onInit(config);

        $(Maps.SELECTORS.MAP).esriMap({
          mapId: Application.MAP_ID,
          callback: Maps.onMapCreated,
          legendSelector: Maps.SELECTORS.GIS_LEGEND,
          layerToggleSelector: Maps.SELECTORS.GIS_LAYERS,
          basemapGallerySelector: Maps.SELECTORS.BASEMAP_GALLERY,
          startWithLayersOff: true,
          showBaseMapToggle: true,
          proxyOptions: Application.MAP_PROXY_OPTIONS,
          locateButton: Maps.SELECTORS.LOCATE,
          defaultBaseLayers: config.defaultLayers
        });
      }
    },

    attachCoordinateSets: function(coordinateSetsArray) {
      for (var i = 0; i < coordinateSetsArray.length; i++) {
        Maps.attachCoordinateSet(coordinateSetsArray[i]);
      }
    },

    attachCoordinateSet: function(coordinateSet) {
      // NOTE: Don't do fitLayerBounds here as it slows things down when attachCoordinateSet
      //       is being called in a loop.

      // A coordinateSet is allowed to have a null layerId, which means defaulting
      // to the default layer id.
      var layerId = coordinateSet.layerId || Maps.LAYER_ID;
      var lineLayerId = layerId + 'lineLayer';

      // This needs to happen before the icon layer is displayed so the lines appear
      // underneath the icons.
      if (coordinateSet.drawLinesBetweenPoints) {
        Maps.initializeLayer(lineLayerId);
      }

      Maps.initializeLayer(layerId);

      var coordinates = coordinateSet.coordinates;
      for (var i = coordinates.length - 1; i >= 0; --i) {
        var c = coordinates[i];
        c.layerId = layerId;

        if (coordinateSet.drawLinesBetweenPoints) {
          var someBlueishColor = [82, 121, 181, 0.5];
          var someReddishColor = [99, 0, 24, 0.5];
          var nextI = i + 1;
          if (nextI < coordinates.length) {
            Maps._map.esriMap('addLineToLayer', lineLayerId, [c, coordinates[nextI]], {
              color: (i == 0 ? someReddishColor : someBlueishColor), width: 5
            });
          }
        } else {
          c.icon = Maps.getIconById(c.iconId);
          Maps.attachMarker(c);
        }
      }
    },

    attachLegendItem: function(layerId, iconFileName, labelText, group) {
      Maps.initializeLegend();
      var icon = Maps.getIconByFileName(iconFileName);

      var legendRow = $('<tr>' +
                          '<td><input type="checkbox" checked="checked" /></td>' +
                          '<td><img /></td>' +
                          '<td class="label-cell"><label></label></td>' +
                        '</tr>');

      var id = 'chk' + layerId;
      var chk = legendRow.find(':checkbox');
      chk.attr({ id: id });
      chk.change(function() {
        Maps.initializeLayer(layerId);
        Maps._map.esriMap('setLayerVisibility', layerId, chk.is(':checked'));
      });

      legendRow.find('img').attr({ src: icon.url })
                           .css({ width: icon.width, height: icon.height });
      legendRow.find('label').text(labelText).attr({ 'for': id });

      if (group) {
        Maps.getLegendGroup(group).append(legendRow);
      }
      else {
        Maps._legend.append(legendRow);
      }
    },

    attachMarker: function(c) {
      return Maps._map.esriMap('addMarkerImageToLayer', c.layerId,
          c.icon.url, c.icon.width, c.icon.height, c.icon.offset, c.lng, c.lat, { url: c.url, coordinate: c });
    },

    clearLayer: function(layerId) {
      Maps._map.esriMap('clearLayer', layerId);
    },

    clearAllLayers: function() {
      // NOTE: This should only clear layers MapCall adds. Any GIS 
      // layers should be intact.
      for (var i = 0; i < Maps._layerIds.length; i++) {
        Maps.clearLayer(Maps._layerIds[i]);
      }
    },

    displayInfoWindow: function(args) {
      var mapPoint = args.event.mapPoint;
      var screenPoint = args.event.screenPoint;
      var content = args.content;

      if (!args.content && args.url) {
        content = '<iframe style="width:100%; height:100%;" src="' + args.url + '"></iframe>';;
      }
      Maps._map.esriMap('displayInfoWindowWithContent', content, mapPoint, screenPoint);
      Maps.onInfoWindowDisplayed(args.event);
    },

    displayMessage: function(msg) {
      alert(msg);
    },

    getExtent: function() {
      return Maps._map.esriMap('getExtent');
    },

    getIconById: function(id) {
      return Maps._icons[id];
    },

    getIconByFileName: function(partialIconFileName) {
      for (var i = 0; i < Maps._icons.length; i++) {
        var cur = Maps._icons[i];
        // There will be null icons due to the way javascript arrays work
        // and that the icons are indexed by icon.id.
        if (cur && cur.url.indexOf(partialIconFileName) > -1) {
          return cur;
        }
      }

      throw "Can't find icon for " + partialIconFileName;
    },

    hideInfoWindow: function() {
      // This is a hack to get the info window to close because esri does not
      // offer a public method for doing so for some reason.
      // Also note that this only *hides* the window, it's still attached
      // to the DOM.
      Maps._map.find('.titleButton.close').click();
    },

    initializeLayer: function(layerId) {
      // We need to prevent the onLayerClick handler from being added
      // multiple times.
      if (Maps._map.esriMap('hasGraphicsLayer', layerId)) {
        return;
      }

      Maps._map.esriMap('ensureGraphicsLayer', layerId);
      Maps._map.esriMap('onLayerClick', layerId, Maps.onMarkerClick);
      Maps._layerIds.push(layerId);
    },

    // Initializes the base legend elements.
    initializeLegend: function() {
      if (Maps._legend == null) {
        Maps._legend = $('<table class="for-structure"></table>');
        $(Maps.SELECTORS.DEFAULT_LEGEND).append(Maps._legend);
        $(Maps.SELECTORS.DEFAULT_LEGEND_HEADER).show();
      }
    },

    getLegendGroup: function(group) {
      if (!Maps._legendGroups[group]) {
        var fieldset = $('<fieldset><legend>' + group + '</legend><table class="for-structure"></table></fieldset>');
        // This allows each group to be mass toggled.
        fieldset.find('legend').on('click', function() {
          fieldset.find(':checkbox').click();
        });
        $(Maps.SELECTORS.DEFAULT_LEGEND).append(fieldset);
        Maps._legendGroups[group] = fieldset;
      }

      return Maps._legendGroups[group];
    },

    // Sends an ajax request to any url that returns a MapResult and then
    // deals with the result. This should be the only way of loading MapResult
    // data on this page and inheriting pages.
    loadMapResultFromUrl: function(url, data, successArgs) {
      Maps.onLoadingMapResult();

      if (Maps._isLoadingAjaxData) {
        Maps.displayMessage('Please wait for the map to load the previous data request and try again.');
        return;
      }

      successArgs = successArgs || {};

      $.ajax({
        url: url,
        data: data,
        type: 'GET',
        beforeSend: function() {
          Maps._isLoadingAjaxData = true;
        },
        complete: function() {
          // Needs to be set to false regardless of success/error.
          Maps._isLoadingAjaxData = false;
        },
        success: function(mapResult) {
          Maps.loadMapResult(mapResult, successArgs);
          if (successArgs.onSuccess) {
            successArgs.onSuccess(mapResult);
          }
        },
        error: function() {
          Maps.displayMessage('An unexpected error occurred while retrieving map data from the server.');
        }
      });
    },

    maximizeInfoWindow: function() {
      // This is a hack to get the info window to maximize because esri does not
      // offer a public method for doing so for some reason.
      Maps._map.find('.titleButton.maximize').click();
    },

    onInfoWindowDisplayed: function(graphic) {
       /* noop by default
        * This is for inheritors that need to do any processing of the 
        * content that's being displayed(like adding event handlers).
        */
    },

    onMapCreated: function(map, response) {
      // just to make sure it's there during ajax calls and such
      Maps._map = map;
      Maps._isMapCreated = true;
      Maps.loadMapResultFromUrl(Maps._dataUrl);
    },

    onMarkerClick: function(e) {
      // e.graphic.attributes will be null or empty if a polyline is clicked on. We don't
      // want lines opening up info windows.
      if (e.graphic.attributes) {
          Maps.lastSelectedMarker = e.graphic;

        // This is so a window immediately opens when clicking the marker. Sometimes
        // the partial is slow to load and nothing is there to indicate that it's working
        // until a few seconds later.
        Maps.displayInfoWindow({ event: e, content: 'Loading...' });
        $.ajax({
          'url': e.graphic.attributes.url,
          'type': 'GET',
          'success': function(response) {
              Maps.displayInfoWindow({ event: e, content: response });
              $(document).ready(function () {
                  document.getElementById('btnNewInspection').focus();
              });
          },
          'error': function(response) {
            var errMsg = 'An error has occurred while loading this data.';

            // This is for when authentication fails while loading a map popup.
            var respJson = response.responseJSON;
            if (respJson && respJson.success === false) {
              errMsg = respJson.reason;
            }
            Maps.displayInfoWindow({ event: e, content: errMsg });
          }
        });
      }
      e.stopPropagation();
    },

    loadMapResult: function(mapResult, args) {
      if (!Maps.validateMapResult(mapResult)) {
        return;
      }

      var defaults = {
        clearAllLayersBeforeLoading: true,
        fitLayerBoundsAfterLoading: true
      };

      // Extent the defaults with the given args. args is optional as well as any properties on args.
      args = $.extend(defaults, args);

      if (args.clearAllLayersBeforeLoading) {
        Maps.clearAllLayers();
      }

      Maps.attachCoordinateSets(mapResult.coordinateSets);

      if (args.fitLayerBoundsAfterLoading) {
        Maps._map.esriMap('fitAllLayerBounds');
      }

      Maps.onLoadedMapResult(mapResult);
    },

    onLoadedMapResult: function (mapResult) {
      // noop, needed for asset map
    },

    onLoadingMapResult: function() {
      // noop, needed for asset map
    },

    removeMarker: function(graphic) {
      Maps._map.esriMap('removeGraphicFromLayer', graphic, graphic.attributes.coordinate.layerId);
    },

    resetToInitialData: function(successArgs) {
      Maps.loadMapResultFromUrl(Maps._dataUrl, null, successArgs);
    },

    validateMapResult: function(mapResult) {
      // mapData are the values set on MapResult. Crazy right?
      if (!mapResult.modelStateIsValid) {
        var errorMessages = ['An invalid search occurred so points can not be displayed on this map.'];

        for (var key in mapResult.modelStateErrors) {
          errorMessages.push(key + ': ' + mapResult.modelStateErrors[key]);
        }

        var errString = '';
        for (var i = 0; i < errorMessages.length; i++) {
          errString = errString + errorMessages[i] + '\n';
        }
        Maps.displayMessage(errString);
        return false;
      }
      else if (mapResult.coordinateSets.length === 0) {
        Maps.displayMessage('There are no valid points to display on this map.');
        return false;
      }

      var totalCoords = 0;

      for (var i = 0; i < mapResult.coordinateSets.length; i++) {
        var cur = mapResult.coordinateSets[i];
        totalCoords = totalCoords + cur.coordinates.length;
      }

      if (totalCoords == 0) {
        Maps.displayMessage('There are no valid points to display on this map.');
        return false;
      }

      return true;
    }
  };

})(jQuery);