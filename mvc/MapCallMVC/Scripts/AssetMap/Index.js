// NOTE: Try not to reference the esri plugin calls here. This script is already an abstraction layer
//       away from it. Add any calls to Map/Index.js instead.

var AssetMaps = (function($, Maps) {
  var am = {
    SELECTORS: {
      EXTENTS_BUTTON: '#extents-button',
      RESET_DATA_BUTTON: '#reset-data-button',
      RELATED_ASSETS_BUTTON: '#load-related-data-button',
      TOOL_BUTTONS: '.tool-button'
    },
    _extentsUrl: null,
    _relatedAssetsUrl: null,
    _relatedAssetsLoaded: false,

    init: function() {
      $(am.SELECTORS.EXTENTS_BUTTON).on('click', am.onExtentsButtonClicked);
      $(am.SELECTORS.RESET_DATA_BUTTON).on('click', am.resetToInitialData);
      $(am.SELECTORS.RELATED_ASSETS_BUTTON).on('click', am.onLoadRelatedClicked);

      Maps.onInit = function(config) {
        am._extentsUrl = config.additionalData.extentsUrl;

        var createLegendGroup = function(args) {
          // file names are always lowercase
          var filePrefix = args.prefix.toLowerCase();

          Maps.attachLegendItem(args.prefix + 'RequiresInspection', filePrefix + '-red', 'Inspection Required', args.groupName);
          Maps.attachLegendItem(args.prefix + 'RequiresInspectionWithWorkOrder', filePrefix + '-redblack', 'Inspection Required with Work Order', args.groupName);
          Maps.attachLegendItem(args.prefix + 'Default', filePrefix + '-green', 'Not Required', args.groupName);
          Maps.attachLegendItem(args.prefix + 'WorkOrder', filePrefix + '-greenblack', 'Work Order', args.groupName);
          Maps.attachLegendItem(args.prefix + 'Inactive', filePrefix + '-gray', 'Inactive / Retired', args.groupName);
          Maps.attachLegendItem(args.prefix + 'NonPublic', filePrefix + '-blue', 'Non-Public', args.groupName);

          if (args.includeNormalPosition) {
            Maps.attachLegendItem(args.prefix + 'NormallyOpenButClosed', filePrefix + '-purplewhite', 'Normally Open but Closed', args.groupName);
            Maps.attachLegendItem(args.prefix + 'NormallyOpenButClosedWithWorkOrder', filePrefix + '-purpleblack', 'Normally Open but Closed with Work Order', args.groupName);
            Maps.attachLegendItem(args.prefix + 'NormallyClosedButOpen', filePrefix + '-orangewhite', 'Normally Closed but Open', args.groupName);
            Maps.attachLegendItem(args.prefix + 'NormallyClosedButOpenWithWorkOrder', filePrefix + '-orangeblack', 'Normally Closed but Open with Work Order', args.groupName);
          }

          if (args.includeOutOfService) {
            Maps.attachLegendItem(args.prefix + 'OutOfService', filePrefix + '-yellow', 'Out of Service', args.groupName);
            Maps.attachLegendItem(args.prefix + 'OutOfServiceWithWorkOrder', filePrefix + '-yellowblack', 'Out of Service with Work Order', args.groupName);
          }
        };

        createLegendGroup({ groupName: 'Valves', prefix: 'Valve', includeNormalPosition: true });
        createLegendGroup({ groupName: 'Hydrants', prefix: 'Hydrant', includeOutOfService: true });
        createLegendGroup({ groupName: 'Blowoffs', prefix: 'BlowOff' });
        
        var createMainCrossingLegendGroup = function (groupName, prefix, includeOutOfService) {
          // file names are always lowercase
          var filePrefix = prefix.toLowerCase();

          Maps.attachLegendItem(prefix + 'RequiresInspection', filePrefix + '-red', 'Inspection Required', groupName);
          Maps.attachLegendItem(prefix + 'Default', filePrefix + '-green', 'Not Required', groupName);
        };

        createMainCrossingLegendGroup('Main Crossings', 'MainCrossing');

        var createBelowGroundHazardLegendGroup = function (groupName, prefix) {
            // file names are always lowercase
            var filePrefix = prefix.toLowerCase();

            Maps.attachLegendItem(prefix + 'Default', filePrefix + '-green', 'Active', groupName);
            Maps.attachLegendItem(prefix + 'Inactive', filePrefix + '-gray', 'Inactive / Retired', groupName);
        };

        createBelowGroundHazardLegendGroup('Below Ground Hazards', 'BelowGroundHazard');

        var createSewerOpeningLegendGroup = function (groupName, prefix) {
            var filePrefix = prefix.toLowerCase();

            Maps.attachLegendItem(prefix + 'Default', filePrefix + '-green', 'Active', groupName);
            Maps.attachLegendItem(prefix + 'Inactive', filePrefix + '-gray', 'Inactive / Retired', groupName);
        };

        createSewerOpeningLegendGroup('Sewer Openings', 'SewerOpening');
      };

      Maps.onLoadingMapResult = function() {
        $(am.SELECTORS.TOOL_BUTTONS).prop('disabled', true);
      };

      Maps.onLoadedMapResult = function(mapResult) {
        am._relatedAssetsUrl = mapResult.relatedAssetsUrl;
        $(am.SELECTORS.TOOL_BUTTONS).prop('disabled', false);

        var butt = $(am.SELECTORS.RELATED_ASSETS_BUTTON);
        if (!am._relatedAssetsUrl) {
          butt.prop('disabled', true);
        } else {
          butt.prop('disabled', false);
        }
      };

      Maps.onInfoWindowDisplayed = function(e) {
        var graphic = e.graphic;
        var coordinate = graphic.attributes.coordinate;
        var isVisible = true;
        var toggleButton = $('.esri-info .esri-toggle');
        toggleButton.on('click', function() {
          if (isVisible) {
            Maps.removeMarker(graphic);
            isVisible = false;
            toggleButton.text('Show');
          } else {
            // Need to replace graphic ref since attachMarker creates a new graphic.
            graphic = Maps.attachMarker(coordinate);
            isVisible = true;
            toggleButton.text('Hide');
          }
        });

        var inspectButton = $('.esri-info .inspect-button');
        inspectButton.on('click', function() {
          Maps.displayInfoWindow({ event: e, url: $(this).attr('href') });
          Maps.maximizeInfoWindow();
          return false; // Since it's a link button, need to stop the event.
        });
      };
    },

    onExtentsButtonClicked: function() {
      // This button can't function properly until the map is actually created.
      if (!Maps._isMapCreated) {
        return;
      }

      if (!confirm('This process can take anywhere from a few seconds to several minutes to complete. Are you sure you want to continue?')) {
        return;
      }

      var extent = Maps.getExtent();
      var extentData = {
        longitudeMax: extent.xmax,
        longitudeMin: extent.xmin,
        latitudeMax: extent.ymax,
        latitudeMin: extent.ymin
      };
      Maps.loadMapResultFromUrl(am._extentsUrl, extentData, {
        // Need to prevent duplicate icons from appearing when the button's
        // clicked more than once.
        clearAllLayersBeforeLoading: true,

        // Leaving this enabled causes the map to shift over for some reason,
        // even if the extents button was clicked and the map was not moved
        // in any way. Considering all the returned coordinates are supposed
        // to fit in the current view it should not have to move anything.
        fitLayerBoundsAfterLoading: false
      });
    },

    onLoadRelatedClicked: function() {
      if (am._relatedAssetsLoaded) {
        return;
      }

      if (!confirm('This will load other assets related to the search that brought you to this map. This may take several minutes depending on the amount of data. Are you sure you want to continue?')) {
        return;
      }

      // Set this before loading so double clicking the button doesn't try to load the
      // data a million times.
      am._relatedAssetsLoaded = true;

      Maps.loadMapResultFromUrl(am._relatedAssetsUrl, null, {
        clearAllLayersBeforeLoading: false,
        fitLayerBoundsAfterLoading: false,
        onSuccess: function() {
        }
      });
    },

    resetToInitialData: function(successArgs) {
      am._relatedAssetsLoaded = false;
      Maps.resetToInitialData(successArgs);
    },

    reloadAndKillPopup: function() {
      am.resetToInitialData({fitLayerBoundsAfterLoading: false});
      Maps._map.find('.titleButton.close').click();
    },

    // This takes a url to an action that should return a MapResult.
    // This lets the server continue to deal with icon logic instead 
    // of copying parts of it to javascript. Also it will set the
    // coordinate in the correct layer.
    updateLastSelectedMarkerIcon: function(dataUrl) {
      Maps.loadMapResultFromUrl(dataUrl, null, {
        clearAllLayersBeforeLoading: false,
        fitLayerBoundsAfterLoading: false,
        onSuccess: function() {
          Maps.removeMarker(Maps.lastSelectedMarker);
        }
      });
    }
  };

  am.init();
  return am;
})(jQuery, Maps);