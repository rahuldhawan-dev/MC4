/*!
 * jquery.esri.mappin v0.2.15
 * by Jason Duncan
 * built 2016-05-27
 */
(function($, esri, dojo, require) {
  /*** HELPER CLASSES ***/

  var MarkerImage = function(url, width, height, offset) {
    this.url = url;
    this.width = width;
    this.height = height;
    this.offset = offset || 'top-left';
  };

  MarkerImage.prototype = {
    getXOffset: function() {
      if (/^.*center$/.test(this.offset)) {
        return 0;
      }

      var off = Math.floor(this.width / 2);

      if (/^.*right/.test(this.offset)) {
        // for some reason this is the opposite whenever movement happens
        off = off * -1;
      }

      return off;
    },

    getYOffset: function() {
      if (!/^(?:top|bottom).*$/.test(this.offset)) {
        return 0;
      }

      var off = Math.floor(this.height / 2);

      if (/^top.*$/.test(this.offset)) {
        off = off * -1;
      }

      return off;
    },

    toSymbol: function() {
      var symbol = new esri.symbol.PictureMarkerSymbol(this.url, this.width, this.height);
      symbol.xoffset = this.getXOffset();
      symbol.yoffset = this.getYOffset();
      return symbol;
    },

    toGraphic: function(lng, lat, attrs) {
      var graphic = new esri.Graphic(esri.geometry.geographicToWebMercator(new esri.geometry.Point(lng, lat)),
                                     this.toSymbol());
      graphic.setAttributes(attrs || {});
      return graphic;
    }
  };

  var Layer = function(options) {
    this.options = options;
    // Needed for the defaultMapbase.

    if (options.existingLayer) {
      this._instance = options.existingLayer;
    }
    else if (options.url) {
      this._instance = new esri.layers.ArcGISDynamicMapServiceLayer(options.url, options);
    }
    else {
      this._instance = new esri.layers.GraphicsLayer(options);
    }
  };

  Layer.prototype = {
    _instance: null,

    getEsriLayer: function() {
      return this._instance;
    },

    isAddedToMap: function() {
      return (this._instance._map != null);
    }
  };



  /**
   * dojo.js
   * @external dojo
   * @see {@link https://dojotoolkit.org/reference-guide/1.10/ Dojo Toolkit Reference Guide}
   */

  /**
   * dojo/on return object
   * @class external:dojo.Event
   * @see {@link https://dojotoolkit.org/reference-guide/1.10/dojo/on.html dojo/on documentation}
   */

  /**
   * ESRI JSAPI
   * @external esri
   * @see {@link https://developers.arcgis.com/javascript/jsapi/ ArcGIS API for JavaScript}
   */

  /**
   * ESRI Graphic object.
   * @class external:esri.Graphic
   * @see {@link https://developers.arcgis.com/javascript/jsapi/graphic-amd.html esri.Graphic}
   */

  /**
   * ESRI Symbols Namespace
   * @namespace external:esri.symbol
   * @see {@link https://developers.arcgis.com/javascript/jsapi/ ArcGIS API for JavaScript}
   */

  /**
   * ESRI SimpleMarkerSymbol
   * @class external:esri.symbol.SimpleMarkerSymbol
   * @see {@link https://developers.arcgis.com/javascript/jsapi/simplemarkersymbol-amd.html esri.symbol.PictureMarkerSymbol}
   */

  /**
   * ESRI Draw
   * @class external:esri.draw
   * @see {@link https://developers.arcgis.com/javascript/3/jsapi/draw-amd.html}
   */

  /*** CONFIGURATION ***/

  var pluginName = 'esriMap';
  var dataItem = pluginName + 'Item';
  var defaultMarkerStyle = 'STYLE_CROSS';
  var defaultAddressMarkerStyle = defaultMarkerStyle;
  var geocodeServerUrl = 'https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer';
  var minSafeZoom = 11;
  var requirements = [
    'esri/map', 'esri/dijit/BasemapGallery', 'esri/arcgis/utils', 'esri/tasks/locator', 'esri/dijit/Legend',
    'esri/toolbars/draw', 'esri/toolbars/edit', 'esri/geometry/Point', 'esri/geometry/Circle', 'dojox/gfx/move',
    'dojo/parser', 'esri/dijit/LocateButton',
    'dojo/domReady!'
  ];

  var defaults = {
    addressZoom: 15,
    markerStyle: null,
    addressMarkerStyle: null,
    initialZoom: 9,
    ajaxStart: null,
    ajaxStop: null,
    showBaseMapToggle: true,
    renderAllLayersButtons: true,
    renderLayerCheckboxesReversed: false,
    onItemsSelected: null,
    selectionToolbar: null,
    onLayerCheckboxCreated: null,
    onLayerLegendCreated: null,
    locateButtonId: 'LocateButton',
    baseLayerConfig: {
      street: {
        id: 'defaultBasemap',
        labelText: 'Street'
        //url: 'https://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer'
      },
      satellite: {
        id: 'Satellite',
        labelText: 'Satellite',
        url: 'https://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer'
      }
    }
  };

  /*** PRIVATE METHODS ***/

  var ajaxStart = function(attr) {
    if (typeof (attr.ajaxStart) === 'function') {
      attr.ajaxStart();
    }
  };

  var ajaxStop = function(attr) {
    if (typeof (attr.ajaxStop) === 'function') {
      attr.ajaxStop();
    }
  };

  var tryGetLayer = function(attr, layerId) {
    var layer = attr.map.getLayer(layerId);

    if (!layer) {
      $.error('Cound not find layer with id "' + layerId + '".');
    }

    return layer;
  };

  var processDefaults = function() {
    defaults.markerStyle = defaults.markerStyle ||
      esri.symbol.SimpleMarkerSymbol[defaultMarkerStyle];
    defaults.addressMarkerStyle = defaults.addressMarkerStyle ||
      esri.symbol.SimpleMarkerSymbol[defaultAddressMarkerStyle];
  };

  var processRequiredArguments = function(obj) {
    if (!obj.mapId) {
      throw 'No mapId provided.';
    }
  };

  var processObsoleteArguments = function(obj) {
    if (obj.toggleLayers || obj.toggleLayers === false) {
      throw 'The \'toggleLayers\' argument is obsolete. Use \'layerToggleSelector\' instead.';
    }
  }

  var getMapCallback = function($this, attr, callback) {
    return function(response) {
      attr = $.extend({}, $this.data(dataItem), { response: response, map: response.map });
      $this.data(dataItem, attr);

      if (typeof (callback) === 'function') {
        callback($this);
      }

      if (attr.locateButtonId) {
          new esri.dijit.LocateButton({ map: response.map }, attr.locateButtonId).startup();
      }

      if (attr.legendSelector || attr.layerToggleSelector) {
        initLegend($this, attr, response.map, esri.arcgis.utils.getLegendLayers(response));
      }

      if (attr.basemapGallerySelector) {
        initBasemapGallery($this, attr, response.map);
      }

      attr.baseLayers = createBaseLayers(attr);

      attr.loadingElement = createLoadingElement();
      $this.append(attr.loadingElement);

      dojo.connect(attr.map, 'onUpdateStart', function() {
        attr.loadingElement.show();
      });
      dojo.connect(attr.map, 'onUpdateEnd', function() {
        attr.loadingElement.hide();
      });

      if (typeof(attr.onItemsSelected) === 'function') {
        attr.selectionToolbar = new esri.toolbars.Draw(attr.map);
        dojo.connect(attr.selectionToolbar, 'onDrawEnd', attr.onItemsSelected);
      }

      ajaxStop(attr);
    };
  };

  var enableDisableLayerToggles = function(attr, layers) {
    layers = layers || esri.arcgis.utils.getLegendLayers(attr.response);
    $(layers).each(function(i, layer) {
      var chk = $('input[name=checkBox' + layer.layer.id + ']');
      var label = $('label[for=' + chk.attr('name') + ']');

      if (layer.layer.visibleAtMapScale) {
        chk.removeAttr('disabled');
        label.css('color', 'black');
      } else {
        chk.attr('disabled', true);
        label.css('color', 'gray');
      }
    });
  };

  var initLegend = function($this, attr, map, layers) {
    if (attr.legendSelector) {
      new esri.dijit.Legend({
        map: map,
        layerInfos: layers,
      }, $(attr.legendSelector).attr('id')).startup();
    }
    if (attr.layerToggleSelector) {
      var legend = $(attr.layerToggleSelector).empty();

      if (!layers.length) {
        legend.append('<span style="font-style: italic">No layers found.</span>');
        return;
      }

      var toggles = $('<p></p>');

      $(attr.renderLayerCheckboxesReversed ? layers.reverse() : layers).each(function(i, layer) {
        var layerName = layer.title;
        var checkbox = $('<input type="checkbox">')
            .attr('name', 'checkBox' + layer.layer.id)
            .attr('value', layer.layer.id)
            .attr('checked', layer.layer.visible);

        if (layer.layer.visible) {
          checkbox.addClass('defaultVisible');
        }

        checkbox.on('change', function() {
          var targetLayer = map.getLayer($(this).val());
          targetLayer.setVisibility(!targetLayer.visible);
          this.checked = targetLayer.visible;
        });

        if (typeof(attr.onLayerCheckboxCreated) === 'function') {
          attr.onLayerCheckboxCreated($this, layer, checkbox);
        }

        var label = $('<label>' + layerName + '</label>').attr('for', checkbox.attr('name'));

        toggles.append(checkbox, label, '<br />');
      });

      legend.append(toggles);

      if (layers.length > 1 && attr.renderAllLayersButtons) {
        $.each([{text: 'All On', val: true}, {text: 'All Off', val: false}], function(idx, item) {
          var btn = $('<button type="button">' + item.text + '</button>');
          btn.on('click', function() {
            methods.toggleLegendLayers($this, attr, item.val);
          });
          legend.append(btn);
        });
      }

      if (typeof(attr.onLayerLegendCreated) === 'function') {
        attr.onLayerLegendCreated($this, legend);
      }

      if (attr.startWithLayersOff) {
        methods.toggleLegendLayers($this, attr, false);
      }

      enableDisableLayerToggles(attr, layers);

      map.on('zoom-end', function () {
        enableDisableLayerToggles(attr);
      });
    }
  };

  var initBasemapGallery = function($this, attr, map) {
    new esri.dijit.BasemapGallery(
      {showArcGISBasemaps: true, map: map}, $(attr.basemapGallerySelector).attr('id')).startup();
  };

  var plotLocationResults = function($this, attr, results) {
    var location = results.addresses[0];
    var symbol = new esri.symbol.SimpleMarkerSymbol();
    symbol.setStyle(attr.addressMarkerStyle);

    if (attr.addressMarker) {
      attr.map.graphics.remove(attr.addressMarker);
    }

    attr.addressMarker = new esri.Graphic(location.location, symbol);

    attr.map.graphics.add(attr.addressMarker);
    attr.map.centerAndZoom(location.location, attr.addressZoom);

    $this.data(dataItem, attr);
  };

  var getGeometryExtent = function(geometry) {
    // All esri geometry objects have a getExtent method.
    // However, not all getExtent methods actually return a value
    // and not all getExtent methods are part of the public esri API documentation.
    // Some geometry is esri.geometry.Polyline(or some other polygon) which will return an extent object.
    // Some geometry is esri.geometry.Point and returns null so you have to use the x and y properties instead.
    var extent = geometry.getExtent ? geometry.getExtent() : null;
    if (!extent) {
      extent = {
        xmin: geometry.x,
        xmax: geometry.x,
        ymin: geometry.y,
        ymax: geometry.y
      }
    }
    return extent;
  };

  var getLayerBounds = function($this, attr, layerId) {
    var layer = tryGetLayer(attr, layerId);
    var minX, minY, maxX, maxY;

    for (var i = layer.graphics.length - 1; i >= 0; --i) {
      var geometry = getGeometryExtent(layer.graphics[i].geometry);
      minX = minX == null || minX > geometry.xmin ? geometry.xmin : minX;
      maxX = maxX == null || maxX < geometry.xmax ? geometry.xmax : maxX;
      minY = minY == null || minY > geometry.ymin ? geometry.ymin : minY;
      maxY = maxY == null || maxY < geometry.ymax ? geometry.ymax : maxY;
    }

    return {
      minX: minX,
      minY: minY,
      maxX: maxX,
      maxY: maxY
    };
  };

  var layerHasVisibleBounds = function($this, attr, layerId) {
    var layer = tryGetLayer(attr, layerId);
    if (!layer.visible || layer.graphics.length === 0) {
      return false;
    }

    for (var i = 0; i < layer.graphics.length; i++) {
      if (layer.graphics[i].visible) {
        return true;
      }
    }

    return false;
  };

  var createBaseLayers = function(attr) {
    var layers = {};
    for (var key in attr.baseLayerConfig) {
      var cur = attr.baseLayerConfig[key];
      var opts = $.extend({}, cur);
      opts.existingLayer = attr.map.getLayer(cur.id);
      layers[key] = new Layer(opts);
    }
    return layers;
  };

  var createLoadingElement = function() {
    var loader = $('<div class="esri-map-loading">LOADING</div>');
    loader.css({
      backgroundColor: 'white',
      position: 'absolute',
      top: '50%',
      left: '50%',
      padding: 6,
      fontWeight: 'bold'
    });

    return loader;
  };

  var configureProxy = function (options) {
    if (options == null || options.url == null) return;

    esri.config.defaults.io.proxyUrl = options.url;

    for (var x = 0; x < options.proxies.length; x++) {
      esri.addProxyRule({
        urlPrefix: options.proxies[x],
        proxyUrl: options.url
      });
    }
  };

  /*** API ***/

  /**
   * Initializes the map.
   * @alias esriMap
   * @param {object} options - options object
   @ @param {bool} options.showBaseMapToggle - render a drop-down allowing the user to switch the base map
   * @param {string} options.legendSelector - css selector representing an element that the legend can be rendered into.  if unset the legend is not rendered.
   * @param {string} options.basemapGallerySelector - css selector representing an element that the basemap gallery can be rendered into.  if unset the gallery is not rendered.
   * @param {string} options.layerToggleSelector - css selector representing an element that the layer toggles can be rendered into.  if unset the toggles are not rendered
   * @param {bool} options.startWithLayersOff - if true, legendSelector is set, and toggleLayers is true, all layers will be toggled off as soon as possible in the loading process
   * @param {object} options.proxyOptions - options for configuring a proxy to use
   * @param {string} options.proxyOptions.url - url to send the proxy requests too
   * @param {array} options.proxyOptions.proxies - array of the urls that are to be proxied
   * @param {function} options.onLayerCheckboxCreated - callback to allow manipulation of layer checkboxes on creation
   * @param {function} options.onLayerLegendCreated - callback to allow manipulation of the legend layer (checkboxes) on creation
   * @param {bool} options.renderAllLayersButtons - boolean to indicate whether or not to draw the 'All On' and 'All Off' buttons underneath the layer toggles (default is true)
   * @param {bool} options.renderLayerCheckboxesReversed - boolean to indicate whether or not to render the layer toggle checkboxes in reverse (default is false)
   * @param {function} options.onItemSelected - callback to allow manipulation of map data retrieved for extents
   * @param {object} options.selectionToolbar - object for holding the selection drawToolbar
   * @param {object} options.locateButtonId - object for locate button
   * @param {array} options.defaultBaseLayers - array of layers that should be on by default, that layerToggleSelector shouldn't turn off. provide the label's text'
   * @throws If options.mapId is not set.
   */
  var init = function(options) {
    var $this = this;
    require(requirements, function() {
      processRequiredArguments(options);
      processObsoleteArguments(options);
      processDefaults();
      configureProxy(options.proxyOptions);
      $this = $this.each(function() {
        $(this).addClass(pluginName);
        var callback = options.callback;
        delete options.callback;
        var center = options.center;
        delete options.center;
        var attr = $.extend({}, defaults, options);
        var mapOptions = { zoom: attr.initialZoom, showLabels: true };

        if (center) {
          mapOptions.center = esri.geometry.geographicToWebMercator(
            new esri.geometry.Point(center.lng, center.lat));
        }

        ajaxStart(attr);

        $this.data(dataItem, attr);

        esri.arcgis.utils.createMap(options.mapId, $this.attr('id'), { mapOptions: mapOptions })
          .then(getMapCallback($this, attr, callback));
      });
    });
    return $this;
  };

  /**
   * Possible offset values are:
   * "top-left", "top-center", "top-right",
   * "left", "center", "right",
   * "bottom-left", "bottom-center", "bottom-right"
   * @alias offset
   */
  var offset; // not actually used anywhere

  var methods = {

    /**
     * Exposed API methods.
     * @namespace API
     */

    /**
     * Adds a circle to a layer.
     * @alias addCircleToLayer
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId
     * @param {float} lng - longitude where the marker should be added
     * @param {float} lat - latitude where the marker should be added
     * @param {object} options - optional object that includes the radius, fillColor(RGBA array or hex string) and lineColor(RGBA array or hex string).
     */
    addCircleToLayer: function($this, attr, layerId, lng, lat, options) {
      var point = esri.geometry.geographicToWebMercator(new esri.geometry.Point(lng, lat, attr.map.spatialReference));
      var circle = new esri.geometry.Circle(point, { radius: options.radius });

      var sym = new esri.symbol.SimpleFillSymbol();
      sym.setColor(options.fillColor);
      sym.outline.setColor(options.lineColor);

      tryGetLayer(attr, layerId).add(new esri.Graphic(circle, sym));
    },

    /**
     * Adds a line(or multiple lines) to a layer.
     * @alias addLineToLayer
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId
     * @param {array} points - an array of objects that have "lat" and "lng" properties. the objects must be in the order that the line should be drawn.
     * @param {object} lineStyle - optional object that includes a color(RGBA array or hex string) and a width property(int).
     */
    addLineToLayer: function($this, attr, layerId, points, lineStyle) {
      lineStyle = lineStyle || { color: [0, 0, 0], width: 3 };
      var spatial = attr.map.spatialReference;
      var line = new esri.geometry.Polyline(spatial);
      var pathPoints = [];
      for (var i = 0; i < points.length; i++) {
        var cur = points[i];
        pathPoints.push(esri.geometry.geographicToWebMercator(new esri.geometry.Point(cur.lng, cur.lat, spatial)));
      }
      line.addPath(pathPoints);
      var lineSymbol = new esri.symbol.SimpleLineSymbol('solid', new dojo.Color(lineStyle.color), lineStyle.width);
      tryGetLayer(attr, layerId).add(new esri.Graphic(line, lineSymbol));
    },

    /**
     * Creates a marker image with the provided arguments, adds it directly to the map
     * (not to a layer), and returns it.
     * @alias addMarkerImage
     * @memberof API
     * @instance
     * @public
     * @param {string} url - url of the image
     * @param {int} width - width of the image
     * @param {int} height - height of the image
     * @param {offset} offset - offset of the marker
     * @param {float} lng - longitude where the marker should be added
     * @param {float} lat - latitude where the marker should be added
     * @param {object} opts - any extra map arguments
     * @returns {external:esri.Graphic} Newly created marker
     */
    addMarkerImage: function($this, attr, url, width, height, offset, lng, lat, opts) {
      var marker = new MarkerImage(url, width, height, offset).toGraphic(lng, lat, opts);
      attr.map.graphics.add(marker);
      return marker;
    },

    /**
     * Create a marker image with the provided arguments, adds it to the layer with the given id,
     * and returns it.
     * @alias addMarkerImageToLayer
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId - id of the layer to add the marker to
     * @param {string} url - url of the image
     * @param {int} width - width of the image
     * @param {int} height - height of the image
     * @param {offset} offset - offset of the marker
     * @param {float} lng - longitude where the marker should be added
     * @param {float} lat - latitude where the marker should be added
     * @param {object} opts - any extra map arguments
     * @returns {external:esri.Graphic} Newly created marker
     * @throws If a layer cannot be found with the given id.
     */
    addMarkerImageToLayer: function($this, attr, layerId, url, width, height, offset, lng, lat, opts) {
      var marker = new MarkerImage(url, width, height, offset).toGraphic(lng, lat, opts);
      tryGetLayer(attr, layerId).add(marker);
      return marker;
    },

    /**
     * Creates a simple marker with no provided image, adds it directly to the map
     * (not to a layer), and returns it.
     * @alias addSimpleMarker
     * @memberof API
     * @instance
     * @public
     * @param {float} lng - longitude where the marker should be added
     * @param {float} lat - latitude where the marker should be added
     * @param {external:esri.symbol.SimpleMarkerSymbol} style - style *fix this*
     * @returns {external:esri.Graphic} Newly created marker
     */
    addSimpleMarker: function($this, attr, lng, lat, style) {
      var marker = methods.getSimpleMarker($this, attr, lng, lat, style);
      attr.map.graphics.add(marker);
      return marker;
    },

    /**
     * Centers the map to the given coordinates, and zooms it to the given level.
     * @alias centerAndZoom
     * @memberof API
     * @instance
     * @public
     * @param {float} - longitude of the new center
     * @param {float} - latitude of the new center
     * @param {int} - level to zoom to
     */
    centerAndZoom: function($this, attr, lng, lat, zoomLevel) {
      methods.setZoom($this, attr, zoomLevel);
      methods.centerAt($this, attr, lng, lat);
    },

    /**
     * Center the map at the given coordinates.
     * @alias centerAt
     * @memberof API
     * @instance
     * @public
     * @param {float} - longitude of the new center
     * @param {float} - latitude of the new center
     */
    centerAt: function($this, attr, lng, lat) {
      ajaxStart(attr);
      try {
        attr.map.centerAt(esri.geometry.geographicToWebMercator(new esri.geometry.Point(lng, lat)));
      } finally {
        ajaxStop(attr);
      }
    },

    /**
     * Changes the icon of the given marker.
     * @alias changeMarkerIcon
     * @memberof API
     * @instance
     * @public
     * @param {external:esri.Graphic} marker - marker of concern
     * @param {string} url - url of the image
     * @param {int} width - width of the image
     * @param {int} height - height of the image
     * @param {offset} offset - offset of the marker
     */
    changeMarkerIcon: function($this, attr, marker, url, width, height, offset) {
      marker.setSymbol(new MarkerImage(url, width, height, offset).toSymbol());
    },

    /**
     * Clears all graphics from the specified layer.
     * @alias clearLayer
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId - id of the layer to clear
     * @throws If a layer cannot be found with the given id.
     */
    clearLayer: function($this, attr, layerId) {
      tryGetLayer(attr, layerId).clear();
    },

    /**
     * Disables double-click zooming on the map (enabled by default).
     * @alias disableDoubleClickZoom
     * @memberof API
     * @instance
     * @public
     * @param {function} [callback] - if provided, callback function for when the map is double clicked
     * @returns {dojo.Event?} Dojo event handler *double-check namespacing*
     */
    disableDoubleClickZoom: function($this, attr, callback) {
      attr.map.disableDoubleClickZoom();

      if (typeof (callback) === 'function') {
        attr.map.on('dbl-click', function(e) {
          callback({
            lat: e.mapPoint.getLatitude(),
            lng: e.mapPoint.getLongitude()
          });
        });
      }
    },

    /**
     * Displays an info window with the given content at the given location.
     * @alias displayInfoWindowWithContent
     * @memberof API
     * @instance
     * @public
     * @param {string} content - content to display
     * @param {int} width - width of the info window - optional
     * @param {int} height - height of the info window - optional
     */
    displayInfoWindowWithContent: function($this, attr, content, mapPoint, screenPoint, width, height) {
      // TODO: I hate passing mapPoint and screenPoint around like this, need to abstract
      //       that away.  Those values most likely come from the layer click event.
      ajaxStart(attr);
      try {
        // The first window that opens does not include the title bar(which has the
        // maximize/close buttons in it) unless setTitle is called.
        attr.map.infoWindow.setTitle('');
        attr.map.infoWindow.setContent(content);
        if (width && height) {
          attr.map.infoWindow.resize(width, height);
        }
        attr.map.infoWindow.show(mapPoint, attr.map.getInfoWindowAnchor(screenPoint));
      } finally {
        ajaxStop(attr);
      }
    },

    /**
     * Ensures that a layer with the given id exists, and returns it.
     * @alias ensureGraphicsLayer
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} id - id of the layer to add or get
     * @returns {external:esri.layers.GraphicsLayer} found or created layer
     */
    ensureGraphicsLayer: function($this, attr, id) {
      var layer = attr.map.getLayer(id);
      return layer ? layer : attr.map.addLayer(new Layer({ id: id }).getEsriLayer());
    },

    /**
     * Zooms and centers the map to display the extents of all the makers on the specified layer.
     * @alias fitLayerBounds
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} layerId - id of the layer of concern
     * @throws If a layer cannot be found with the given id.
     */
    fitLayerBounds: function($this, attr, layerId) {
      var bounds = getLayerBounds($this, attr, layerId);

      if (bounds.minX && bounds.minY && bounds.maxX && bounds.maxY) {
        attr.map.setExtent(new esri.geometry.Extent(bounds.minX, bounds.minY, bounds.maxX, bounds.maxY, attr.map.spatialReference), true);
      }
    },

    /**
     * Zooms and centers the map to display the extents of all the markers currently visible on all layers.
     * @alias fitAllLayerBounds
     * @memberof API
     * @instance
     * @function
     * @public
     */
    fitAllLayerBounds: function($this, attr) {
      var layers = [];
      var layerIds = attr.map.graphicsLayerIds;

      for (var i = 0; i < layerIds.length; i++) {
        var layer = getLayerBounds($this, attr, layerIds[i]);
        if (layerHasVisibleBounds($this, attr, layerIds[i])) {
          layers.push(layer);
        }
      }

      var minX, minY, maxX, maxY;

      for (var i = 0; i < layers.length; i++) {
        var cur = layers[i];
        minX = minX == null || minX > cur.minX ? cur.minX : minX;
        maxX = maxX == null || maxX < cur.maxX ? cur.maxX : maxX;
        minY = minY == null || minY > cur.minY ? cur.minY : minY;
        maxY = maxY == null || maxY < cur.maxY ? cur.maxY : maxY;
      }

      if (minX && minY && maxX && maxY) {

        // This will occur if there's only one visible graphic, so
        // we need to zoom in on the point or else it'll stay at
        // the current zoom level. Also this has to be done before
        // setExtent is called or else the map zooms in on the wrong
        // area.
        if (minX == maxX && minY == maxY) {
          methods.setZoom($this, attr, attr.addressZoom);
        }

        attr.map.setExtent(new esri.geometry.Extent(minX, minY, maxX, maxY, attr.map.spatialReference), true);
      }
    },

    /**
     * Returns the center of the current map extent.
     * @alias getCenter
     * @memberof API
     * @instance
     * @function
     * @public
     * @returns {LatLng} center of the current map extent
     */
    getCenter: function($this, attr) {
      var center = attr.map.extent.getCenter();
      return {
        lat: center.getLatitude(),
        lng: center.getLongitude()
      };
    },

    /**
     * Gets the current extent for the map.
     * @alias getExtent
     * @memberof API
     * @instance
     * @function
     * @public
     * @returns {object} Hash with xmin, xmax, ymin, and ymax properties.
     */
    getExtent: function($this, attr) {
      var extent = esri.geometry.webMercatorToGeographic(attr.map.extent);
      return {
        xmax: extent.xmax,
        xmin: extent.xmin,
        ymax: extent.ymax,
        ymin: extent.ymin
      };
    },

    /**
     * Gets all graphics attached to the specified layer.
     * @alias getGraphicsFromLayer
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} layerId - id of the layer of concern
     * @returns {array} array of graphics attached to layer
     * @throws If a layer cannot be found with the given id.
     */
    getGraphicsFromLayer: function($this, attr, layerId) {
      return tryGetLayer(attr, layerId).graphics;
    },

    /**
     * Creates and returns a simple marker to be shown at the given coordinates (without adding it to the map).
     * @alias getSimpleMarker
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {float} lng - longitude
     * @param {float} lat - latitude
     * @param {external:esri.symbol.SimpleMarkerSymbol} [style] - optional style
     * @returns {external:esri.Graphic} newly created marker
     */
    getSimpleMarker: function($this, attr, lng, lat, style) {
      var symbol = new esri.symbol.SimpleMarkerSymbol();
      symbol.setStyle(style || attr.markerStyle);
      var point = new esri.geometry.Point(lng, lat, attr.map.spatialReference);
      return new esri.Graphic(esri.geometry.geographicToWebMercator(point), symbol);
    },

    /**
     * Returns the current zoom level of the map.
     * @alias getZoom
     * @memberof API
     * @instance
     * @function
     * @public
     * @returns {int} current zoom level
     */
    getZoom: function($this, attr) {
      return attr.map.getZoom();
    },

    /**
     * Returns true if a graphics layer exists for the given id.
     * @alias hasGraphicsLayer
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} id - id of the layer to find.
     * @returns {bool} true if the graphics layer exists.
     */
    hasGraphicsLayer: function($this, attr, id) {
      return (attr.map.getLayer(id) != null);
    },

    /**
     * Attempts to locate the given address on the map.  If no callback is provided, the results will
     * be plotted on the map using a simple marker in the style specified in the map option
     * `addressMarkerStyle'.
     *
     * @alias locateAddress
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} address - address to locate
     * @param {function} [callback] - if specified, will be called with a LatLng represented the located
     *                                map point, or null if the search was not successful
     */
    locateAddress: function($this, attr, address, callback) {
      var geocoder = new esri.tasks.Locator(geocodeServerUrl);
      geocoder.outSpatialReference = attr.map.spatialReference;
      var geocodeEvent = geocoder.on('address-to-locations-complete', function(results) {
        geocodeEvent.remove();
        if (typeof (callback) === 'function') {
          var args = null;
          if (results.addresses.length) {
            var location = results.addresses[0].location;
            args = {
              lat: location.getLatitude(),
              lng: location.getLongitude()
            };
          }
          callback(args);
        } else {
          plotLocationResults($this, attr, results);
        }
      });
      geocoder.addressToLocations({
        address: {
          SingleLine: address
        }
      });
    },

    /**
     * Makes the provided marker draggable, calling the provided callback function when the marker is dropped.
     * @alias makeDraggable
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {external:esri.Graphic} marker - marker of concern
     * @param {function} callback - callback function, called with a lat/lng when the marker is dropped
     * @returns {external:dojo.Event} event handle for later manipulation/removal
     */
    makeDraggable: function($this, attr, marker, callback) {
      var editToolbar = new esri.toolbars.Edit(attr.map);
      editToolbar.activate(esri.toolbars.Edit.MOVE, marker);

      return dojo.connect(editToolbar, 'onGraphicMoveStop', function(mover) {
        callback({
          lat: mover.geometry.getLatitude(),
          lng: mover.geometry.getLongitude()
        });
      });
    },

    /**
     * Moves the provided marker to the provided coordinates.
     * @alias moveMarker
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {external:esri.Graphic} marker - marker of concern
     * @param {float} lng - new longitude
     * @param {float} lat - new latitude
     */
    moveMarker: function($this, attr, marker, lng, lat) {
      ajaxStart(attr);
      try {
        marker.setGeometry(esri.geometry.geographicToWebMercator(new esri.geometry.Point(lng, lat)));
      } finally {
        ajaxStop(attr);
      }
    },

    /**
     * Set an event handler for when the map extent changes.
     * @alias onExtentChange
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {function} callback - callback fn *check signature*
     * @returns {external:dojo.Event} handle to event for later manipulation/removal
     */
    onExtentChange: function($this, attr, callback) {
      return attr.map.on('extent-change', callback);
    },

    /**
     * Specifies a callback to fire when the speicified layer is clicked (usually meaning a specific marker was clicked).
     * You'll need to provide some sort of attribute values when creating the markers so you can tell from the callback
     * which one was clicked.
     * @alias onLayerClick
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} layerId - id of layer of concern
     * @param {function} callback - callback fn *check signature*
     * @returns {external:dojo.Event} handle to event for later manipulation/removal
     * @throws If a layer cannot be found with the given id.
     */
    onLayerClick: function($this, attr, layerId, callback) {
      return dojo.connect(tryGetLayer(attr, layerId), 'onClick', callback);
    },

    /**
     * Specifieds a callback to fire when a graphic in the specified layer is hovered over. An optional second callback
     * may be used for when the mouse pointer leaves the graphic.
     * @alias onLayerHover
     * @memberof API
     * @instance
     * @function
     * @public
     * @param {string} layerId - id of the layer of concern
     * @param {function} mouseOver - mouse-over callback
     * @param {function} [mouseOut] - mouse-out callback
     * @returns {array} array of created event handles for later manipulation/removal
     * @throws If a layer cannot be found with the given id.
     */
    onLayerHover: function($this, attr, layerId, mouseOver, mouseOut) {
      var layer = tryGetLayer(attr, layerId);
      var handlers = [layer.on('mouse-over', mouseOver)];

      if (typeof (mouseOut) === 'function') {
        handlers.push(layer.on('mouse-out', mouseOut));
      }

      return handlers;
    },

    /**
     * Removes the graphic from the map (might not work if the graphic belongs to a layer).
     * @alias removeGraphic
     * @memberof API
     * @instance
     * @public
     */
    removeGraphic: function($this, attr, graphic) {
      attr.map.graphics.remove(graphic);
    },

    /**
     * Removes the graphic from the layer.
     * @alias removeGraphicFromLayer
     * @memberof API
     * @instance
     * @public
     */
    removeGraphicFromLayer: function($this, attr, graphic, layerId) {
      tryGetLayer(attr, layerId).remove(graphic);
    },

    /**
     * Sets the active base layer. These are predefined and only one layer can be visible at a time.
     * @alias setBaseLayer
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId - id of predefined layer. Either "defaultBasemap" or "Satellite"
     */
    setBaseLayer: function($this, attr, layerId) {
      var foundMatch = false;
      for (var key in attr.baseLayers) {
        var cur = attr.baseLayers[key];
        var isMatchingLayer = (cur.options.id == layerId);
        if (isMatchingLayer) {
          foundMatch = true;
          if (!cur.isAddedToMap()) {
            attr.map.addLayer(cur.getEsriLayer());
          }
        }
        cur.getEsriLayer().setVisibility(isMatchingLayer);
      }

      if (!foundMatch) {
        $.error('There is no base layer with the id "' + layerId + '".');
      }
    },

    /**
     * Toggles the visibility of a layer while leaving its data intact.
     * @alias setLayerVisibility
     * @memberof API
     * @instance
     * @public
     * @param {string} layerId - id for the layer
     * @param {bool} isVisible - true for visible, false for not visible.
     */
    setLayerVisibility: function($this, attr, layerId, isVisible) {
      tryGetLayer(attr, layerId).setVisibility(isVisible);
    },

    /**
     * Sets the zoom level of the map.
     * @alias setZoom
     * @memberof API
     * @instance
     * @public
     * @param {int} zoomLevel - zoom level to set to
     */
    setZoom: function($this, attr, zoomLevel) {
      ajaxStart(attr);
      try {
        attr.map.setZoom(zoomLevel);
      } finally {
        ajaxStop(attr);
      }
    },

    /**
     * Toggles all base layers off using the checkboxes rendered when toggleLayers passed to the
     * initial options is true.  If toggleLayers was not true at the time the map was created,
     * this method has no way to toggle the layers.
     * @alias toggleLegendLayers
     * @memberof API
     * @instance
     * @public
     * @param {bool} show - indicates whether to hide or show the layers, default is false (hide)
     * @throws If the map was not created with a value for layerToggleSelector
     */
    toggleLegendLayers: function($this, attr, show) {
      if (!attr.layerToggleSelector) {
        throw 'This map was created with layerToggleSelector unset, so layers cannot be toggled automatically.';
      }

      show = (show == null) ? false : show;

      var selector = (attr.layerToggleSelector + ' input:checkbox:') + (show ? 'not(:checked)' : 'checked') + '.defaultVisible';
      $(selector).each(function (i, chk) {
        if (attr.defaultBaseLayers !== undefined && !attr.defaultBaseLayers.includes(chk.nextElementSibling.innerHTML)) {
          $(chk).trigger('click')
        };
      });
    },

    /**
     * Toggles on the specified selection/drawing tool
     * @alias activateSelectionTool
     * @memberof API
     * @instance
     * @public
     * @param {string} draw -  DRAW constant from JSAPI for the selection tool to enable
     * https://developers.arcgis.com/javascript/3/jsapi/draw-amd.html
     */
    activateSelectionTool: function ($this, attr, draw){
      if (attr.selectionToolbar == null)
	    	return;
	    attr.selectionToolbar.activate(esri.toolbars.Draw[draw]);
    },

    /**
     * Disables the selection tool
     * @alias deactivateSelectionTool
     * @memberof API
     * @instance
     * @public
     */
    deactivateSelectionTool: function ($this, attr) {
	    if (attr.selectionToolbar != null)
				attr.selectionToolbar.deactivate();
    }, 

    /**
     * Adds a layer to the map
     * @alias addLayer
     * @memberof API
     * @instance
     * @public
     * @param layer - layer to add to the map
     */
    addLayer: function($this, attr, layer) {
			attr.map.addLayer(layer);
		},

		/**
		 * Sets extents of the map to the extent.
     * @alias activateSelectionTool
     * @memberof API
     * @instance
     * @public
		 * @param {} extent - Extent object
		 * @param {} fit - passthrough of api prop
		 */
		setExtent: function($this, attr, extent, fit) {
			attr.map.setExtent(extent, fit);
		}
  };

  /*** MODULE DEFINITION ***/

  $.fn[pluginName] = function(method) {
    if (methods[method]) {
      var $this = $(this);
      var attr = $this.data(dataItem);
      var args = [$this, attr].concat(Array.prototype.slice.call(arguments, 1));
      return methods[method].apply(this, ($this, attr, args));
    } else if (typeof method === 'object' || !method) {
      return init.apply(this, arguments);
    } else {
      $.error('Method ' + method + ' does not exist');
    }
  };

  $[pluginName + 'Setup'] = function(args) {
    if (typeof (args) === 'string') {
      return defaults[args];
    }

    for (var x in args) {
      if (defaults.hasOwnProperty(x)) {
        defaults[x] = args[x];
      }
    }
  };
})(jQuery, esri, dojo, require);
