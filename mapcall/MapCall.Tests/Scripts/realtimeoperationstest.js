/// <reference path="testHelpers.js"/>
/// <reference path="../../../mmsinc/MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../mmsinc/MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../mmsinc/MMSINC.Testing/Scripts/testHelpers.js" />
/// <reference path="../../../mmsinc/MapCall.Common.WebForms/Resources/Scripts/arcgis.js" />
/// <reference path="../../../mmsinc/MapCall.Common.WebForms/Resources/scripts/jquery.js" />
/// <reference path="../../../mmsinc/MapCall.Common.WebForms/Resources/scripts/jquery.esri.mappin.js" />
/// <reference path="../../MapCall/Modules/Maps/realtimeoperations.js" />

module('RealTimeOperations');

test('.intialize() initializes map, layers, marker managers, object managers, ui, and data queue', function() {
  mock(function() {
    var map = new Object();

    mock.expect('RealTimeOperations.initializeLegend')
      .mock(function() { });
    mock.expect('RealTimeOperations.UI.initialize')
      .mock(function() { });
    mock.expect('RealTimeOperations.initializeMap')
      .mock(function() { });
    mock.expect('RealTimeOperations.initializeDataQueue')
      .mock(function() { });

    RealTimeOperations.initialize();

  });
});

test('.initializeLayers() initializes layers', function() {
  mock(function() {

    var expectLayer = function(layer) {
      mock.expect('RealTimeOperations.map.esriMap')
        .withArguments('ensureGraphicsLayer', layer)
        .returnValue();

      mock.expect('RealTimeOperations.map.esriMap')
        .withArguments('onLayerClick', layer, RealTimeOperations.onMarkerClick)
        .returnValue();
    };

    expectLayer('hydrants');
    expectLayer('valves');
    expectLayer('workOrders');
    expectLayer('sandy');
    expectLayer('complaints');
    expectLayer('bactis');
    expectLayer('events');
    expectLayer('vehicles');
    expectLayer('leaks');
    expectLayer('mainBreaks');
    expectLayer('overflows');
    expectLayer('flushingSchedules');
    expectLayer('investigations');
    expectLayer('onecalltickets');

    RealTimeOperations.initializeLayers();
  });
});

test('.initializeLayers() initializes traffic and weather map layers', function() {
  mock(function() {

    mock.expect('RealTimeOperations.map.esriMap')
        .returnValue();

    mock.expect('RealTimeOperations.initializeTrafficLayer')
      .mock(function() { });
    mock.expect('RealTimeOperations.initializeWeatherLayer')
      .mock(function() { });

    RealTimeOperations.initializeLayers();
  });
});

test('.initializeDataQueue() creates an interval used to update the data queue', function() {
  mock(function() {
    var intervalId = 1234, interval = 60000;

    RealTimeOperations.dataIntervalId = null;

    mock.expect('window.setInterval')
      .mock(function(code, ms) {
        equal(interval, ms,
               'Correct interval value used');
        code();
        return intervalId;
      });
    mock.expect('RealTimeOperations.getDataQueueInterval')
      .returnValue(interval);

    RealTimeOperations.initializeDataQueue();

    equal(intervalId, RealTimeOperations.dataIntervalId,
            'dataIntervalId properly set');
  });
});

test('.initializeTrafficLayer() creates new TrafficLayer and sets trafficLayer property', function() {
  mock(function() {
    ok('this doesnt do anything yet');
  });
});

test('.initializeWeatherLayer() creates new WeatherLayer and sets weatherLayer property', function() {
  mock(function() {
    ok('this doesnt do anything yet');
  });
});

test('.generateInfoWindowContent(id, type) generates an iframe tag with the specified id and type', function() {
  var markerID = 1234;
  var markerType = 'sharpie';

  equal('<iframe style="width:100%; height:100%;" src="' + RealTimeOperations.ITEM_DATA_URL + '?ID=' + markerID + '&type=' + markerType + '"></iframe>',
         RealTimeOperations.generateInfoWindowContent(markerID, markerType));
});

test('.attachMarker(coordinate, type, icon, layerId) creates a marker with the given parameters and adds it to the layer', function() {
  mock(function() {
    var coordinate = { lat: 1234, lng: 4321 };
    var type = 'thing';
    var icon = {
      url: 'some url',
      width: 300,
      height: 300,
      offset: 'off'
    }
    var layer = 'layer';

    mock.expect('RealTimeOperations.map.esriMap')
      .withArguments('addMarkerImageToLayer', layer, icon.url, icon.width, icon.height, icon.offset, coordinate.lng, coordinate.lat, coordinate)
      .returnValue(null);

    RealTimeOperations.attachMarker(coordinate, type, icon, layer);
  });
});

test('.geoCodeLocation(location) centers the map at the chosen location', function() {
  mock(function() {
    var location = '321 Maple St.';
    var map = new Object();
    var txtSearch = mock.create('txtSearch', ['val']);

    RealTimeOperations.map = map;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_SEARCH)
      .returnValue(txtSearch);
    mock.expect('txtSearch.val')
      .returnValue(location);
    mock.expect('RealTimeOperations.map.esriMap')
      .withArguments('locateAddress', location)
      .returnValue(null);

    RealTimeOperations.geocodeLocation();
  });
});

module('RealTimeOperations.ajax');

test('.getRangeInHours() returns the user-entered date/time range in hours when chosen range is hours', function() {
  mock(function() {
    var ddlRange = mock.create('ddlRange', ['val']);
    var txtRange = mock.create('txtRange', ['val']);
    var expected = 1234;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.DDL_RANGE)
      .mock(function() { })
      .returnValue(ddlRange);
    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_RANGE)
      .mock(function() { })
      .returnValue(txtRange);
    mock.expect('ddlRange.val')
      .returnValue('h');
    mock.expect('txtRange.val')
      .returnValue(1234);

    equal(expected, RealTimeOperations.ajax.getRangeInHours(), 'Returned value as expected');
  });
});

test('.getRangeInHours() returns the user-entered date/time range in hours when chosen range is days', function() {
  mock(function() {
    var ddlRange = mock.create('ddlRange', ['val']);
    var txtRange = mock.create('txtRange', ['val']);
    var expected = 15;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.DDL_RANGE)
      .mock(function() { })
      .returnValue(ddlRange);
    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_RANGE)
      .mock(function() { })
      .returnValue(txtRange);
    mock.expect('ddlRange.val')
      .returnValue('d');
    mock.expect('txtRange.val')
      .returnValue(expected);

    equal(expected * 24, RealTimeOperations.ajax.getRangeInHours(),
           'Returned value multiplied by 24 as expected');
  });
});

test('.getDataQueueInterval() returns the user-entered number of minutes for update/refresh converted to milliseconds', function() {
  mock(function() {
    var minutes = 5;
    var txtInterval = mock.create('txtInterval', ['val']);
    var expected = minutes * 60000;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_INTERVAL)
      .mock(function() { })
      .returnValue(txtInterval);
    mock.expect('txtInterval.val')
      .returnValue(minutes);

    equal(expected, RealTimeOperations.getDataQueueInterval(),
           'Data queue interval properly calculated');
  });
});

test('.filterDotNetJsonData(data) returns data from a .net json response', function() {
  mock(function() {
    var json = '{d: \'actual data\'}';
    var data = { d: 'actual data' };

    mock.expect('$.parseJSON')
      .withArguments(json)
      .exactly('1 time')
      .returnValue(data);

    equal('actual data', RealTimeOperations.ajax.filterDotNetJsonData(json),
           'JSON data fitlered properly');
  });
});

test('.loadMarkerData(serviceMethod, callback) gets data from the given serviceMethod at the webservice url via AJAX and calls the specified callback on success', function() {
  mock(function() {
    var serviceMethod = 'this is the marker data service method';
    var range = 24;
    var callback = function() { };
    var args = {
      type: RealTimeOperations.AJAX_REQUEST_TYPE,
      data: '{\'hours\': ' + range + '}',
      url: RealTimeOperations.SERVICE_URL + serviceMethod,
      contentType: RealTimeOperations.CONTENT_TYPE_JSON,
      dataFilter: RealTimeOperations.ajax.filterDotNetJsonData,
      success: callback
    };

    mock.expect('RealTimeOperations.ajax.getRangeInHours')
      .returnValue(range);
    mock.expect('$.ajax')
      .mock(function(a) {
        equal(args.type, a.type, 'Proper reuqest type used');
        equal(args.data, a.data, 'Proper data sent');
        equal(args.url, a.url, 'Proper url used');
        equal(args.contentType, a.contentType, 'Proper content type used');
        deepEqual(args.dataFilter, a.converters['text json'], 'Proper data filter used');
        deepEqual(args.callback, a.callback, 'Proper callback used');
      });

    RealTimeOperations.ajax.loadMarkerData(serviceMethod, callback);
  });
});


test('.loadData(name) calls ajax.loadStandardData(name) for items which do not specify a callback', function() {
  mock(function() {
    var name = 'hydrants';

    mock.expect('RealTimeOperations.ajax.loadStandardData')
      .withArguments(name)
      .mock(function() { });

    RealTimeOperations.ajax.loadData(name);
  });
});

test('.loadData(name) calls ajax.loadAjaxData(name) for items which specify a callback', function() {
  mock(function() {
    var name = 'workOrders';
    mock.expect('RealTimeOperations.ajax.loadSpecialData')
      .withArguments(name)
      .mock(function() { });

    RealTimeOperations.ajax.loadData(name);
  });
});

test('.loadStandardData(name) loads data for the specified item using a standard generated callback', function() {
  mock(function() {
    var oldHydrantAjaxItem = RealTimeOperations.ajax.Dictionary.hydrants;
    var serviceMethod = 'I am the serviceMethod', markerImage = new Object(), markerType = 'type';
    var callback = function() { };

    var ajaxItem = {
      serviceMethod: serviceMethod,
      callback: null,
      markerImage: markerImage,
      markerType: markerType
    };

    RealTimeOperations.ajax.Dictionary['hydrants'] = ajaxItem;

    mock.expect('RealTimeOperations.ajax.getAjaxItemCallback')
      .withArguments(markerImage, markerType, 'hydrants')
      .mock(function() { })
      .returnValue(callback);
    mock.expect('RealTimeOperations.ajax.loadMarkerData')
      .withArguments(serviceMethod, callback)
      .mock(function() { });

    RealTimeOperations.ajax.loadStandardData('hydrants');

    RealTimeOperations.ajax.Dictionary['hydrants'] = oldHydrantAjaxItem;
  });
});

test('.loadSpecialData(name) loads data for the specified item using a specially generated callback', function() {
  mock(function() {
    var callback = function() { };
    var ajaxItem = {
      serviceMethod: 'I am the service method',
      callback: 'FooStuff',
      markerType: 'foo'
    };

    RealTimeOperations.ajax.Dictionary['foo'] = ajaxItem;

    RealTimeOperations.ajax.getFooStuffCallback = function() { };

    mock.expect('RealTimeOperations.ajax.getFooStuffCallback')
      .withArguments(ajaxItem.markerType, 'foo')
      .mock(function() { })
      .returnValue(callback);
    mock.expect('RealTimeOperations.ajax.loadMarkerData')
      .withArguments(ajaxItem.serviceMethod, callback)
      .mock(function() { });

    RealTimeOperations.ajax.loadSpecialData('foo');
  });
});

test('.getAjaxItemCallback(image, type, layer) generates a callback fn that will load all provided data items', function() {
  mock(function() {
    var icon = {};
    var coord = { lat: 1234, lng: 4321 };
    var type = '5309';

    var callback = RealTimeOperations.ajax.getAjaxItemCallback(icon, type, 'someLayer');

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(coord, type, icon, 'someLayer')
      .returnValue(null);

    callback([coord]);
  });
});

test('.getWorkOrdersCallback(type, layer) generates a callback fn that will load all provided data items with the correct images', function() {
  mock(function() {

    var lat = 1234, lng = 4321, id = 867;
    var type = '5309';
    var item = { lat: lat, lng: lng, id: id, type: type };
    var items = [$.extend({}, item, { opt: 1 }),
                 $.extend({}.item, { opt: 2 }),
                 $.extend({}, item, { opt: 3 })];

    var callback = RealTimeOperations.ajax.getWorkOrdersCallback(type, 'workOrders');

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[0], type, RealTimeOperations.icons.workOrders.wo1, 'workOrders')
      .returnValue(null);
    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[1], type, RealTimeOperations.icons.workOrders.wo2, 'workOrders')
      .returnValue(null);
    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[2], type, RealTimeOperations.icons.workOrders.wo3, 'workOrders')
      .returnValue(null);

    callback(items);
  });
});

test('.getComplaintsCallback(type, layer) generates a callback fn that will load all provided data items with the correct images', function() {
  mock(function() {
    var lat = 1234, lng = 4321, id = 867, type = '5309';
    var item = { lat: lat, lng: lng, id: id, type: type };
    var items = [$.extend({}, item, { opt: 1 }),
                 $.extend({}, item, { opt: 2 }),
                 $.extend({}, item, { opt: 3 }),
                 $.extend({}, item, { opt: 4 })];

    var callback = RealTimeOperations.ajax.getComplaintsCallback(type, 'complaints');

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[0], type, RealTimeOperations.icons.complaints.aesthetic, 'complaints')
      .returnValue(null);

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[1], type, RealTimeOperations.icons.complaints.medical, 'complaints')
      .returnValue(null);

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[2], type, RealTimeOperations.icons.complaints.information, 'complaints')
      .returnValue(null);

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[3], type, RealTimeOperations.icons.complaints.other, 'complaints')
      .returnValue(null);

    callback(items);
  });
});

test('.getFlushingSchedulesCallback(type, layer) generates a callback fn that will load all provided data items with the correct images', function() {
  mock(function() {
    var lat = 1234, lng = 4321, id = 867, type = '5309', radius = 1111;
    var item = { lat: lat, lng: lng, id: id, type: type, opt: radius };
    var items = [item];
    var layer = 'flushingSchedules';
    var callback = RealTimeOperations.ajax.getFlushingSchedulesCallback(type, layer);

    mock.expect('RealTimeOperations.map.esriMap')
      .mock(function(method, layerId, coordLng, coordLat, lineStyle) {
        equal('addCircleToLayer', method);
        equal(lat, coordLat);
        equal(lng, coordLng);
        equal(layerId, layer);
        equal(radius, lineStyle.radius);
      });

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(item, type, RealTimeOperations.icons.misc.black, 'flushingSchedules')
      .returnValue(null);

    callback(items);
  });
});

test('.getMainBreaksCallback(type, layer) generates a callback fn that will load all provided data items with the correct images', function() {
  mock(function() {
    var lat = 1234, lng = 4321, id = 867, type = '5309';
    var item = { lat: lat, lng: lng, id: id, type: type };
    var items = [$.extend({}, item, { opt: 1 }),
                 $.extend({}, item, { opt: 2 }),
                 $.extend({}, item, { opt: 3 }),
                 $.extend({}, item, { opt: 4 })];
    var layer = 'mainBreaks';
    var callback = RealTimeOperations.ajax.getMainBreaksCallback(type, layer);

    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[0], type, RealTimeOperations.icons.workOrders.wo1, layer)
      .returnValue(null);
    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[1], type, RealTimeOperations.icons.workOrders.wo2, layer)
      .returnValue(null);
    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[2], type, RealTimeOperations.icons.workOrders.wo3, layer)
      .returnValue(null);
    mock.expect('RealTimeOperations.attachMarker')
      .withArguments(items[3], type, RealTimeOperations.icons.workOrders.wo4, layer)
      .returnValue(null);

    callback(items);
  });
});

test('.unloadData(name) clears data from named layer', function() {
  mock(function() {
    var name = 'hydrants';

    mock.expect('RealTimeOperations.map.esriMap')
      .withArguments('clearLayer', name)
      .returnValue(null);

    RealTimeOperations.ajax.unloadData('hydrants');
  });
});

test('.toggleData(name, show) loads the data for and enqueues the given name if show is true', function() {
  mock(function() {
    var name = 'foo';

    mock.expect('RealTimeOperations.ajax.enqueue')
      .withArguments(name)
      .mock(function() { });
    mock.expect('RealTimeOperations.ajax.loadData')
      .withArguments(name)
      .mock(function() { });

    RealTimeOperations.ajax.toggleData(name, true);
  });
});

test('.toggleData(name, show) unloads the data for and dequeues the given name if show is false', function() {
  mock(function() {
    var name = 'bar';

    mock.expect('RealTimeOperations.ajax.dequeue')
      .withArguments(name)
      .mock(function() { });
    mock.expect('RealTimeOperations.ajax.unloadData')
      .withArguments(name)
      .mock(function() { });

    RealTimeOperations.ajax.toggleData(name, false);
  });
});

test('.reloadData(name) unloads and loads the data for the given name', function() {
  mock(function() {
    var name = 'valves';

    mock.expect('RealTimeOperations.ajax.loadData')
      .withArguments(name)
      .mock(function() { });
    mock.expect('RealTimeOperations.ajax.unloadData')
      .withArguments(name)
      .mock(function() { });

    RealTimeOperations.ajax.reloadData(name);
  });
});

test('.enqueue(name) sets the `inQueue\' flag on the item in the AjaxDictionary to true', function() {
  RealTimeOperations.ajax.Dictionary.valves.inQueue = false;

  RealTimeOperations.ajax.enqueue('valves');

  ok(RealTimeOperations.ajax.Dictionary.valves.inQueue, 'Valves properly queued');

  RealTimeOperations.ajax.Dictionary.valves.inQueue = false;
});

test('.dequeue(name) sets the `inQueue\' flag on the item in the AjaxDictionary to false', function() {
  RealTimeOperations.ajax.Dictionary.bactis.inQueue = true;

  RealTimeOperations.ajax.dequeue('bactis');

  ok(!RealTimeOperations.ajax.Dictionary.bactis.inQueue, 'Bactis properly dequeued');

  RealTimeOperations.ajax.Dictionary.bactis.inQueue = false;
});

test('.runQueue() reloads all data for items which have been queued', function() {
  mock(function() {
    var calledForHydrants, calledForValves, calledForFlushingSchedules;
    calledForHydrants = calledForValves = calledForFlushingSchedules = false;

    mock.expect('RealTimeOperations.ajax.reloadData')
      .mock(function(name) {
        if (!calledForHydrants) {
          calledForHydrants = (name == 'hydrants');
        }
        if (!calledForValves) {
          calledForValves = (name == 'valves');
        }
        if (!calledForFlushingSchedules) {
          calledForFlushingSchedules = (name == 'flushingSchedules');
        }
      });

    RealTimeOperations.ajax.Dictionary.hydrants.inQueue = true;
    RealTimeOperations.ajax.Dictionary.valves.inQueue = true;
    RealTimeOperations.ajax.Dictionary.flushingSchedules.inQueue = true;

    RealTimeOperations.ajax.runQueue();

    ok(calledForHydrants, 'Hydrants loaded');
    ok(calledForValves, 'Valves loaded');
    ok(calledForFlushingSchedules, 'FlushingSchedules loaded');
  });
});


test('.updateQueueInterval() clears current data queue interval, and updates to the latest user-entered value', function() {
  mock(function() {
    var intervalId = 4321;

    RealTimeOperations.intervalId = intervalId;

    mock.expect('window.clearInterval')
      .withArguments(intervalId)
      .mock(function() { });
    mock.expect('RealTimeOperations.initializeDataQueue')
      .mock(function() { });

    RealTimeOperations.updateQueueInterval();
  });
});

module('RealTimeOperations.UI');

test('.updateData() runs queue and updates data date', function() {
  mock(function() {
    mock.expect('RealTimeOperations.ajax.runQueue')
      .mock(function() { });
    mock.expect('RealTimeOperations.UI.updateDataDate')
      .mock(function() { });

    RealTimeOperations.UI.updateData();
  });
});

test('.toggleDateRange() switches from days to hours', function() {
  mock(function() {
    var ddlRange = mock.create('ddlRange', ['val']);
    var txtRange = mock.create('txtRange', ['val']);
    var rangeInDays = 2;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.DDL_RANGE)
      .mock(function() { })
      .returnValue(ddlRange);
    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_RANGE)
      .mock(function() { })
      .returnValue(txtRange);
    mock.expect('ddlRange.val')
      .returnValue('h');
    mock.expect('txtRange.val')
      .exactly('2 times')
      .mock(function(val) {
        if (val) {
          equal(rangeInDays * 24, val);
          return null;
        } else {
          return rangeInDays;
        }
      });

    RealTimeOperations.UI.toggleDateRange();
  });
});

test('.toggleDateRange() switches from hours to days', function() {
  mock(function() {
    var ddlRange = mock.create('ddlRange', ['val']);
    var txtRange = mock.create('txtRange', ['val']);
    var rangeInHours = 72;

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.DDL_RANGE)
      .mock(function() { })
      .returnValue(ddlRange);
    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.TXT_RANGE)
      .mock(function() { })
      .returnValue(txtRange);
    mock.expect('ddlRange.val')
      .returnValue('h');
    mock.expect('txtRange.val')
      .exactly('2 times')
      .mock(function(val) {
        if (val) {
          equal(rangeInHours * 24, val);
          return null;
        } else {
          return rangeInHours;
        }
      });

    RealTimeOperations.UI.toggleDateRange();
  });
});

test('.updateDataDate() sets the date label to the current date', function() {
  mock(function() {
    var lblUpdated = mock.create('lblUpdated', ['text']);
    var date = mock.create('date', ['toLocaleTimeString']);
    var dateAsString = 'I am the date as a string';

    mock.expect('$')
      .withArguments(RealTimeOperations.ControlIDs.LBL_UPDATED)
      .mock(function() { })
      .returnValue(lblUpdated);
    mock.expect('Date')
      .mock(function() { })
      .returnValue(date);
    mock.expect('date.toLocaleTimeString')
      .returnValue(dateAsString);
    mock.expect('lblUpdated.text')
      .withArguments(dateAsString);

    RealTimeOperations.UI.updateDataDate();
  });
});

test('.initialize() wires toggle effects to UI toolbar controls', function() {
  mock(function() {
    var calledWithLayers, calledWithLegend, calledWithOptions;
    calledWithLayers = calledWithLegend, calledWithOptions = false;

    mock.expect('RealTimeOperations.UI.wireToggleEffect')
      .exactly('3 times')
      .mock(function(arg) {
        if (!calledWithLayers) {
          calledWithLayers = (arg === RealTimeOperations.ControlIDs.TOOLBAR_ITEMS.LAYERS);
        }
        if (!calledWithLegend) {
          calledWithLegend = (arg === RealTimeOperations.ControlIDs.TOOLBAR_ITEMS.LEGEND);
        }
        if (!calledWithOptions) {
          calledWithOptions = (arg === RealTimeOperations.ControlIDs.TOOLBAR_ITEMS.OPTIONS);
        }
      });

    RealTimeOperations.UI.initialize();

    ok(calledWithLayers, 'Toggle effect wired to "layers" toolbar item');
    ok(calledWithLegend, 'Toggle effect wired to "legend" toolbar item');
    ok(calledWithOptions, 'Toggle effect wired to "options" toolbar item');
  });
});

test('.fireToggleEffect toggles element', function() {
  mock(function() {
    var id = '#someId';
    var foo = mock.create('foo', ['toggle']);

    mock.expect('$')
      .withArguments(id)
      .mock(function() { })
      .returnValue(foo);
    mock.expect('foo.toggle')
      .withArguments(RealTimeOperations.UI.TOGGLE_EFFECT,
        RealTimeOperations.UI.TOGGLE_OPTIONS,
        RealTimeOperations.UI.TOGGLE_SPEED);

    RealTimeOperations.UI.fireToggleEffect(id);
  });
});

test('.wireToggleEffect wires toggle effect to element', function() {
  mock(function() {
    var toolbarItem = { ctrl: 'header', body: 'body' };
    var element = mock.create('element', ['click']);
    var clickFn;

    mock.expect('$')
      .withArguments(toolbarItem.ctrl)
      .mock(function() { })
      .returnValue(element);
    mock.expect('element.click')
       .exactly('1 time')
       .mock(function(cb) {
         clickFn = cb;
       });
    mock.expect('RealTimeOperations.UI.fireToggleEffect')
      .withArguments(toolbarItem.body)
      .mock(function() { });

    RealTimeOperations.UI.wireToggleEffect(toolbarItem);
    clickFn();
  });
});
