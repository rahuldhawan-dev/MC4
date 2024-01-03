/// <reference path="../../../../mmsinc/MapCall.Common.Mvc/Content/JS/jquery-1.10.2.js" />
/// <reference path="../../../../mmsinc/MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../../mmsinc/MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../../mmsinc/MMSINC.Testing/Scripts/testHelpers.js" />

/// <reference path="../../../MapCallMVC/scripts/Coordinate/New.js" />

module('Coordinate/New.js');

preserve(['Application'], function () {
  var target = CoordinateEdit;
  window.Application = {
    MAP_ID: 'foo'
  };

  test('initialize performs necessary initialization', function () {
    mock(function () {
      mock.expect('CoordinateEdit.initMap').mock(noop);
      mock.expect('CoordinateEdit.initEvents').mock(noop);
      mock.expect('CoordinateEdit.tryEnableSave').mock(noop);
     // mock.expect('CoordinateEdit.setParentCoordinateId').mock(noop);

      target.initialize();
    });
  });

  test('initMap sets initialCoordinates', function () {
    preserve(['CoordinateEdit.initialCoordinates'], function () {
      var obj = {};
      mock(function () {
        var mapMock = mock.create('map', {map: null});
        mock.expect('CoordinateEdit.getLatLng').returnValue(obj);
        mock.expect('$').withArguments(CoordinateEdit.SELECTORS.MAP_DIV).returnValue(mapMock);
        mock.expect('map.esriMap').mock(noop);

        target.initMap();

        deepEqual(CoordinateEdit.initialCoordinates, obj);
      });
    });
  });

  test('initMap passes options to map fn', function () {
    var obj = {};
    mock(function () {
      var mapMock = mock.create('map', {map: null});
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.MAP_DIV).returnValue(mapMock);
      mock.expect('map.esriMap').mock(function(args) {
        equal(args.mapId, CoordinateEdit.MAP_ID);
        equal(args.initialZoom, CoordinateEdit.DEFAULTS.ZOOM_LEVEL);
        equal(args.callback, CoordinateEdit.onMapCreated);
      });

      target.initMap();
    });
  });

  test('initMap sets map reference', function () {
    mock(function () {
      var mapMock = mock.create('map', {map: null});
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.MAP_DIV).returnValue(mapMock);
      mock.expect('map.esriMap').mock(noop).returnValue(mapMock);

      target.initMap();

      deepEqual(CoordinateEdit.map, mapMock);
    });
  });

  test('initEvents sets up click events for find button and icons', function () {
    mock(function () {
      var findButton = mock.create('findButton');
      var icon = mock.create('icon');

      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.FIND_BUTTON).returnValue(findButton);
      mock.expect('findButton.click').withArguments(CoordinateEdit.geocodeLocation);
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.ICON).returnValue(icon);
      mock.expect('icon.click').withArguments(CoordinateEdit.icon_click);

      target.initEvents();
    });
  });

  test('onMapCreated disables double-click zoom and geocodes the current location', function() {
    preserve(['CoordinateEdit.initialCoordinates'], function() {
      mock(function() {
        var latLng = {lat: 1, lng: 2};
        CoordinateEdit.initialCoordinates  = latLng;
        var map = mock.create('map');

        mock.expect('map.esriMap').withArguments('disableDoubleClickZoom', CoordinateEdit.map_doubleClick);
        mock.expect('CoordinateEdit.setCenterIcon').mock(noop);
        mock.expect('CoordinateEdit.geocodeLocation').mock(noop);

        target.onMapCreated(map);
      });
    });
  });

  test('setCenterIcon creates the center icon using the provided icon id', function() {
    mock(function() {
      var iconIdObj = mock.create('iconId');
      var iconId = 'this is the iconId';
      var icon = mock.create('icon');
      icon.length = 1;

      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.ICON_ID).returnValue(iconIdObj);
      mock.expect('iconId.val').returnValue(iconId);
      mock.expect('CoordinateEdit.getIconByIconId').withArguments(iconId).returnValue(icon);
      mock.expect('CoordinateEdit.getCenterMarker').mock(noop).withArguments(icon);
      mock.expect('icon.click').mock(noop);

      target.setCenterIcon();
    });
  });

  test('setCenterIcon creates the center icon to the default if no icon id provided', function() {
    mock(function() {
      var iconIdObj = mock.create('iconId');
      var iconId = 'this is the iconId';
      var icon = [];
      var defaultIcon = mock.create('defaultIcon', 'click');

      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.ICON_ID).returnValue(iconIdObj);
      mock.expect('iconId.val').returnValue(iconId);
      mock.expect('CoordinateEdit.getIconByIconId').withArguments(iconId).returnValue(icon);
      mock.expect('CoordinateEdit.getDefaultIconInSet').mock(noop).returnValue(defaultIcon);
      mock.expect('CoordinateEdit.getCenterMarker').mock(noop).withArguments(defaultIcon);
      mock.expect('defaultIcon.click').mock(noop);

      target.setCenterIcon();
    });
  });

  test('getIconByIconId gets the icon image by the iconId value', function () {
    var icon = {};

    mock(function () {
      mock.expect('$')
        .withArguments(CoordinateEdit.SELECTORS.ICON_LIST + ' button[value="666"] img')
        .returnValue(icon);

      deepEqual(target.getIconByIconId(666), icon);
    });
  });

  preserve(['CoordinateEdit.initialCoordinates'], function () {
    var tryEnableSaveTest = function (desc, fn) {
      test('tryEnableSave ' + desc, function () {
        mock(function () {
          CoordinateEdit.initialCoordinates = mock.create('initialCoordinates');
          var current = mock.create('currentCoordinates');
          mock.expect('CoordinateEdit.getLatLng').mock(noop).returnValue(current);
          mock.expect('$').withArguments(CoordinateEdit.SELECTORS.ICON_ID)
            .returnValue(mock.create('iconId'));

          fn();

          target.tryEnableSave();
        });
      });
    };

    var setupIconId = function (iconId) {
      mock.expect('iconId.val').returnValue(iconId);
    };

    var setCurrentCoordinates = function (lat, lng) {
      mock.expect('currentCoordinates.lat').maybe().returnValue(lat);
      mock.expect('currentCoordinates.lng').maybe().returnValue(lng);
    };

    var setInitialCoordinates = function (lat, lng) {
      mock.expect('initialCoordinates.lat').maybe().returnValue(lat);
      mock.expect('initialCoordinates.lng').maybe().returnValue(lng);
    };

    var shouldEnableButton = function () {
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.SAVE_BUTTON)
        .returnValue(mock.create('saveButton'));
      mock.expect('saveButton.removeAttr').withArguments('disabled');
    };

    var shouldNotEnableButton = function () {
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.COORDINATE_ID).never();
    };

    tryEnableSaveTest('enables the save button if latitude has been updated', function () {
      setupIconId(1);

      setCurrentCoordinates(2, 1);
      setInitialCoordinates(1, 1);

      shouldEnableButton();
    });

    tryEnableSaveTest('enables the save button if longitude has been updated', function () {
      setupIconId(1);

      setCurrentCoordinates(1, 2);
      setInitialCoordinates(1, 1);

      shouldEnableButton();
    });

    tryEnableSaveTest('does not enable the save button if nothing has been updated', function () {
      setupIconId(1);

      setCurrentCoordinates(1, 1);
      setInitialCoordinates(1, 1);

      shouldNotEnableButton();
    });

    tryEnableSaveTest('does not enable the save button if the icon has not been chosen', function () {
      setupIconId(null);

      setCurrentCoordinates(2, 2);
      setInitialCoordinates(1, 1);

      shouldNotEnableButton();
    });
  });

  test('setParentCoordinateId sets coordinateid on the parent', function () {
    mock(function() {
      var siblingCoordinateValuesSpan = mock.create('cp-coordinate-values', {text: function() { /* noop? */ }});
      var parentElem = mock.create('parentElem', { find: function() { return siblingCoordinateValuesSpan; }});
      var parentCoordinateId = mock.create('parentCoordinateId', { val: function() {}, parent: function() { return parentElem; }});
      var valueFor = 'foo bar';
      mock.expect('CoordinateEdit.getQuerystringValue').mock(noop).withArguments('valueFor')
        .returnValue(valueFor);
      mock.expect('parent.$').withArguments('#' + valueFor).returnValue(parentCoordinateId);
      mock.expect('CoordinateEdit.fixParentCoordinateIcon').mock(noop)
        .withArguments(42, parentCoordinateId);

      target.setParentCoordinateId({
        coordinateId: 42,
        lat: 52,
        lng: 63
      });
    });
  });

  test('setParentCoordinateId does not set coordinateId on the parent if zero', function () {
    mock(function() {
      var siblingCoordinateValuesSpan = mock.create('cp-coordinate-values', { text: function() { /* noop? */ } });
      var parentElem = mock.create('parentElem', { find: function() { return siblingCoordinateValuesSpan; } });
      var parentCoordinateId = mock.create('parentCoordinateId', { parent: function() { return parentElem; } });
      var valueFor = 'foo bar';
      mock.expect('CoordinateEdit.getQuerystringValue').mock(noop).withArguments('valueFor')
        .returnValue(valueFor);
      mock.expect('parent.$').withArguments('#' + valueFor).returnValue(parentCoordinateId);
      mock.expect('parentCoordinateId.val').never();
      mock.expect('CoordinateEdit.fixParentCoordinateIcon').mock(noop)
        .withArguments(0, parentCoordinateId);

      target.setParentCoordinateId({
        coordinateId: 0,
        lat: 52,
        lng: 63
      });
    });
  });

  preserve([], function () {
    var testGetLatLng = function (desc, lat, lng, exLat, exLng) {
      test('getLatLng ' + desc, function () {
        mock(function () {
          mock.expect('$').withArguments(CoordinateEdit.SELECTORS.LATITUDE)
            .returnValue(mock.create('lat', {val: function() {return lat;}}));
          mock.expect('$').withArguments(CoordinateEdit.SELECTORS.LONGITUDE)
            .returnValue(mock.create('lng', {val: function() {return lng;}}));

          var result = target.getLatLng();
          equal(result.lat, exLat);
          equal(result.lng, exLng);
        });
      });
    };

    testGetLatLng('gets current coordinate values if set', '1.1', '2.2', '1.1', '2.2');
    testGetLatLng('gets default coordinate values if unset',
                  '', '', CoordinateEdit.DEFAULTS.LATITUDE, CoordinateEdit.DEFAULTS.LONGITUDE);
  });

  test('setLatLng sets the latitude and longitude hidden input values', function () {
    mock(function () {
      var latLng = {lat: 1.1, lng: 2.2};
      var lat = mock.create('lat');
      var lng = mock.create('lng');
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.LATITUDE).returnValue(lat);
      mock.expect('lat.val').withArguments(1.1);
      mock.expect('$').withArguments(CoordinateEdit.SELECTORS.LONGITUDE).returnValue(lng);
      mock.expect('lng.val').withArguments(2.2);

      target.setLatLng(latLng);
    });
  });

  test('getCenterMarker returns center marker if already set', function () {
    preserve(['CoordinateEdit.centerMarker'], function () {
      CoordinateEdit.centerMarker = {};

      deepEqual(target.getCenterMarker(), CoordinateEdit.centerMarker);
    });
  });

  test('getCenterMarker creates, sets, and returns the center marker if unset', function () {
    preserve(['CoordinateEdit.centerMarker', 'CoordinateEdit.map'], function () {
      mock(function () {
        CoordinateEdit.centerMarker = null;
        var centerMarker = {};

        mock.expect('CoordinateEdit.createCenterMarker')
          .mock(noop).returnValue(centerMarker);

        deepEqual(target.getCenterMarker(), centerMarker);
      });
    });
  });

  test('createCenterMarker creates marker on center of map instance and makes it draggable', function () {
    preserve(['CoordinateEdit.map'], function () {
      mock(function() {
        CoordinateEdit.map = mock.create('map', {map: null});
        var center = {lat: 1, lng: 2};
        var width = 3;
        var height = 4;
        var src = 'this is the src';
        var offset = 'this is the offset';
        var icon = mock.create('icon', {
          attr: function(str) {
            switch (str) {
              case 'src':
              return src;
              case 'data-icon-offset':
              return offset;
              default:
              throw 'vOv';
            }
          },
          width: function() {return width;},
          height: function() {return height;}
        });
        var marker = {};

        mock.expect('CoordinateEdit.getLatLng').returnValue(center);
        mock.expect('map.esriMap')
          .withArguments('addMarkerImage', src, width, height, offset, center.lng, center.lat)
          .returnValue(marker);
        mock.expect('map.esriMap')
          .withArguments('makeDraggable', marker, CoordinateEdit.centerMarker_dragend);

        var ret = target.createCenterMarker(icon);

        deepEqual(ret, marker);
      });
    });
  });

  test('setCenterMarker sets the position of the center marker and tries to enable the save button', function () {
    preserve(['CoordinateEdit.centerMarker', 'CoordinateEdit.map'], function() {
      mock(function () {
        CoordinateEdit.map = mock.create('map');
        CoordinateEdit.centerMarker = {};
        var latLng = {lat: 1, lng: 2};

        mock.expect('map.esriMap').withArguments('moveMarker', CoordinateEdit.centerMarker, latLng.lng, latLng.lat);
        mock.expect('CoordinateEdit.setLatLng').mock(noop).withArguments(latLng);
        mock.expect('CoordinateEdit.tryEnableSave');

        target.setCenterMarker(latLng);
      });
    });
  });

  test('setMapCenter sets the center of the map and center marker', function () {
    preserve(['CoordinateEdit.map'], function () {
      mock(function () {
        CoordinateEdit.map = mock.create('map');
        var latLng = {lat: 1, lng: 2};

        mock.expect('map.esriMap').withArguments('centerAt', latLng.lng, latLng.lat);
        mock.expect('CoordinateEdit.setCenterMarker').mock(noop).withArguments(latLng);

        target.setMapCenter(latLng);
      });
    });
  });

  test('geocodeLocation does nothing if location has not been entered', function () {
    mock(function () {
      var location = { val: function () { return ''; } };

      mock.expect('$').returnValue(location);
      mock.expect('CoordinateEdit.getGeocoderCallback').never();

      target.geocodeLocation();
    });
  });

  test('geocodeLocation does nothing if location has Select something in it', function () {
    mock(function () {
      var location = { val: function () { return 'Select'; } };

      mock.expect('$').returnValue(location);
      mock.expect('CoordinateEdit.getGeocoderCallback').never();

      target.geocodeLocation();
    });
  });

  test('geocodeLocation geocodes the given location', function () {
    preserve(['CoordinateEdit.map'], function() {
      mock(function () {
        CoordinateEdit.map = mock.create('map');
        var theLocation = 'the location';
        var location = { val: function () { return theLocation; } };
        var callback = {};

        mock.expect('$').returnValue(location);
        mock.expect('CoordinateEdit.getGeocoderCallback')
          .mock(noop).withArguments(theLocation).returnValue(callback);
        mock.expect('map.esriMap').withArguments('locateAddress', theLocation, callback);

        target.geocodeLocation();
      });
    });
  });

  test('getGeocoderCallback returns a function that will set the map center and center marker', function () {
    mock(function () {
      var location = {};

      mock.expect('CoordinateEdit.setMapCenter').withArguments(location).mock(noop);

      target.getGeocoderCallback(null)(location);
    });
  });
});
