// this will exist on the page where GooglePicker.js is used
var ASSET_TYPE_IDS = {
  VALVE:1,
  HYDRANT:2,
  MAIN:3,
  SERVICE:4,
  SEWER_MANHOLE:5,
  SEWER_LATERAL:6,
  SEWER_MAIN:7,
  STORM_CATCH:8
};

var noop = function(){};

var getServerElement = noop;

// need some jquery
var jQuery = function() {};
jQuery.inArray = noop;

var google = {
  maps: {
    event: {
      addListener: noop
    },
    Geocoder: function() {},
    LatLng: function(lat, lng) {
      this.lat = lat;
      this.lng = lng;
    },
    Map: function(mapDiv, opts) {
      this.mapDiv = mapDiv;
      for (var x in opts) {
        this[x] = opts[x];
      }
    },
    MapTypeId: {
      ROADMAP: new Object()
    },
    Marker: function(opts) {
      for (var x in opts) {
        this[x] = opts[x];
      }
    }
  }
};

module('GooglePicker');

//////////////////////////////////GETTERS/////////////////////////////////

test('.getDocumentURL returns URL property of the document', function() {
  var url = document.URL.toString();

  equals(url, GooglePicker.getDocumentURL());
});

test('.getAssetTypeID gathers assetTypeID from querystring as an int', function() {
  jack(function() {
    var assetTypeID = 123321;

    jack.expect('window.parseInt')
      .withArguments(assetTypeID.toString(), 10);
    jack.expect('GooglePicker.getDocumentURL')
      .returnValue('somePage.html?assetTypeID=' + assetTypeID.toString())

    equals(assetTypeID, GooglePicker.getAssetTypeID());
  });
});

test('.getPhysicalAssetTypeIDList returns list of assetTypeIDs that map to a physical asset with coordinates', function() {
  deepEqual([ASSET_TYPE_IDS.VALVE,
             ASSET_TYPE_IDS.HYDRANT,
             ASSET_TYPE_IDS.SEWER_MANHOLE,
             ASSET_TYPE_IDS.STORM_CATCH],
            GooglePicker.getPhysicalAssetTypeIDList());
});

test('.getCenterMarker sets centerMarker to a new google.maps.Marker object with the proper arguments and returns it', function() {
  jack(function() {
    GooglePicker.centerMarker = null;
    var map = jack.create('map', ['getCenter']);
    var marker = jack.create('marker', []);
    var center = jack.create('center', []);

    jack.expect('GooglePicker.getMap')
      .returnValue(map);
    jack.expect('map.getCenter')
      .returnValue(center);
    jack.expect('GooglePicker.createMarker')
      .mock(function(opts) {
        ok(opts.draggable);
        same(map, opts.map);
        same(center, opts.position);
        return marker;
      });
    jack.expect('google.maps.event.addListener')
      .withArguments(marker, 'dragend', GooglePicker.centerMarker_dragend);

    var actual = GooglePicker.getCenterMarker();

    same(marker, actual);
    same(GooglePicker.centerMarker, actual);

    GooglePicker.centerMarker = null;
  });
});

test('.getCenterMarker returns existing centerMarker if already defined', function() {
  var marker = new Object();
  GooglePicker.centerMarker = marker;
  
  same(marker, GooglePicker.getCenterMarker());

  GooglePicker.centerMarker = null;
});

test('.getMap sets map to a new google.maps.Map object with the proper arguments and returns it', function() {
  jack(function() {
    GooglePicker.map = null;
    var divMap = jack.create('divMap', []);
    var defaultCoordinates = jack.create('defaultCoordinates', []);
    
    jack.expect('document.getElementById')
      .withArguments('map')
      .returnValue(divMap);
    jack.expect('GooglePicker.getDefaultCoordinates')
      .returnValue(defaultCoordinates);

    var actual = GooglePicker.getMap();

    same(divMap, actual.mapDiv);
    same(defaultCoordinates, actual.center);
    equal(GooglePicker.DEFAULT_ZOOM_LEVEL, actual.zoom);
    equal(GooglePicker.DEFAULT_MAP_TYPE, actual.mapTypeId);
    same(actual, GooglePicker.map);
    ok(actual.mapTypeControl);
    ok(actual.scrollwheel);
    ok(actual.disableDoubleClickZoom);

    GooglePicker.map = null;
  });
});

test('.getMap returns existing map if already defined', function() {
  var map = new Object();
  GooglePicker.map = map;

  same(map, GooglePicker.getMap());

  GooglePicker.map = null;
});

test('.getDefaultCoordinates returns a google.maps.LatLng object with the default center coordinates', function() {
  GooglePicker.defaultCoordinates = null;

  var actual = GooglePicker.getDefaultCoordinates();

  equal(actual.lat, GooglePicker.DEFAULTS.lat);
  equal(actual.lng, GooglePicker.DEFAULTS.lng);
  same(actual, GooglePicker.defaultCoordinates);

  GooglePicker.defaultCoordinates = null;
});

test('.getDefaultCoordinates returns cached value if already set', function() {
  var defaultCoordinates = new Object();

  GooglePicker.defaultCoordinates = defaultCoordinates;

  same(defaultCoordinates, GooglePicker.getDefaultCoordinates());

  GooglePicker.defaultCoordinates = null;
});

//////////////////////////////////SETTERS/////////////////////////////////

test('.setAssetCoordinates sets the return value, closes the window, and returns true if new coordinates have been chosen and user confirms the change', function() {
  jack(function() {
    jack.expect('GooglePicker.areNewCoordinatesChosen')
      .mock(noop)
      .returnValue(true);
    jack.expect('window.confirm')
      .withArguments(GooglePicker.NEW_COORDINATES_MESSAGE)
      .returnValue(true);
    jack.expect('GooglePicker.setReturnAndCloseWindow')
      .mock(noop);

    ok(GooglePicker.setAssetCoordinates());
  });
});

test('.setAssetCoordinates does not set center marker or returned lat/lng values and returns false if new coordinates have not been chosen', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['getPosition']);
    var latLng = new Object();

    jack.expect('GooglePicker.areNewCoordinatesChosen')
      .mock(noop)
      .returnValue(false);
    jack.expect('window.confirm')
      .never();
    jack.expect('GooglePicker.setReturnAndCloseWindow')
      .never();

    ok(!GooglePicker.setAssetCoordinates());
  });
});

test('.setAssetCoordinates does not set center marker or returned lat/lng values and returns false if new coordinates have been chosen but user does not confirm the change', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['getPosition']);
    var latLng = new Object();

    jack.expect('GooglePicker.areNewCoordinatesChosen')
      .returnValue(true);
    jack.expect('window.confirm')
      .withArguments(GooglePicker.NEW_COORDINATES_MESSAGE)
      .returnValue(false);
    jack.expect('GooglePicker.setReturnAndCloseWindow')
      .never();

    ok(!GooglePicker.setAssetCoordinates());
  });
});

test('.setWorkOrderCoordinates sets the return value, closes the window, and returns false if new coordinates are set', function() {
  jack(function() {
    jack.expect('GooglePicker.areNewCoordinatesChosen')
      .returnValue(true);
    jack.expect('GooglePicker.setReturnAndCloseWindow')
      .mock(noop);

    ok(!GooglePicker.setWorkOrderCoordinates());
  });
});

test('.setWorkOrderCoordinates does not set center marker and return lat/lng or close window, and returns false if new coordinates are not set', function() {
  jack(function() {
    jack.expect('GooglePicker.areNewCoordinatesChosen')
      .returnValue(false);
    jack.expect('GooglePicker.setReturnAndCloseWindow')
      .never();

    ok(!GooglePicker.setWorkOrderCoordinates());
  });
});

test('.setCenterMarker sets position of center marker and sets hidden lat/lng values', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['setPosition']);
    var hidLat = jack.create('hidLat', ['val']);
    var hidLng = jack.create('hidLng', ['val']);
    var latLng = {
      lat: function() { return GooglePicker.DEFAULTS.lat; },
      lng: function() { return GooglePicker.DEFAULTS.lng; },
    };

    jack.expect('GooglePicker.getCenterMarker')
      .mock(noop)
      .returnValue(centerMarker);
    jack.expect('centerMarker.setPosition')
      .withArguments(latLng);
    jack.expect('window.getServerElement')
      .withArguments('hidLatitude')
      .returnValue(hidLat);
    jack.expect('hidLat.val')
      .withArguments(GooglePicker.DEFAULTS.lat);
    jack.expect('window.getServerElement')
      .withArguments('hidLongitude')
      .returnValue(hidLng);
    jack.expect('hidLng.val')
      .withArguments(GooglePicker.DEFAULTS.lng);

    GooglePicker.setCenterMarker(latLng);
  });
});

test('.setReturnLatLng sets lat/lng values using code from parent window if present', function() {
  jack(function() {
    window.top.setLat = noop;
    window.top.setLon = noop;
    var latLng = {
      lat: function() { return GooglePicker.DEFAULTS.lat; },
      lng: function() { return GooglePicker.DEFAULTS.lng; },
    };

    jack.expect('window.top.setLat')
      .withArguments(GooglePicker.DEFAULTS.lat);
    jack.expect('window.top.setLon')
      .withArguments(GooglePicker.DEFAULTS.lng);

    GooglePicker.setReturnLatLng(latLng);
  });
});

test('.setMapCenter sets center point of map from latLng', function() {
  jack(function() {
    var latLng = {
      lat: function() { return GooglePicker.DEFAULTS.lat; },
      lng: function() { return GooglePicker.DEFAULTS.lng; },
    };
    var map = jack.create('map', ['setCenter']);

    jack.expect('GooglePicker.getMap')
      .mock(noop)
      .returnValue(map);
    jack.expect('map.setCenter')
      .withArguments(latLng);

    GooglePicker.setMapCenter(latLng);
  });  
});

/////////////////////////////HELPER FUNCTIONS/////////////////////////////

test('.isNaNOrEmptyString returns true if value is NaN or an empty string, or else false', function() {
  ok(GooglePicker.isNaNOrEmptyString(1 / 's'));
  ok(GooglePicker.isNaNOrEmptyString(''));
  ok(GooglePicker.isNaNOrEmptyString('this string is not a number'));
  ok(!GooglePicker.isNaNOrEmptyString('1234'));
});

test('.closeWindow closes the window that the picker is running in', function() {
  jack(function() {
    var btnClose = jack.create('btnClose', ['click']);

    jack.expect('window.top.document.getElementById')
      .withArguments('btnClose')
      .mock(noop)
      .returnValue(btnClose);
    jack.expect('btnClose.click');

    GooglePicker.closeWindow();
  });
});

test('.createMarker creates and returns a new google.maps.Marker object', function() {
  var actual = GooglePicker.createMarker({ draggable: true, 'abc': 'xyz' });

  same(google.maps.Marker, actual.constructor);
  ok(actual.draggable);
  equal(actual.abc, 'xyz');
});

test('.loadMap sets up the map with the center point defined as sent by lat and lng controls, and geocodes the current address if provided if current lat/lng are the default values', function() {
  jack(function() {
    var hidLat = jack.create('hidLat', ['val']);
    var hidLng = jack.create('hidLng', ['val']);
    var lat = GooglePicker.DEFAULTS.lat;
    var lng = GooglePicker.DEFAULTS.lng;

    jack.expect('window.getServerElement')
      .withArguments('hidLatitude')
      .returnValue(hidLat);
    jack.expect('window.getServerElement')
      .withArguments('hidLongitude')
      .returnValue(hidLng);
    jack.expect('hidLat.val')
      .returnValue(lat);
    jack.expect('hidLng.val')
      .returnValue(lng);
    jack.expect('GooglePicker.setUpMap')
      .withArguments(lat, lng)
      .mock(noop);
    jack.expect('GooglePicker.geocodeLocation')
      .mock(noop);

    GooglePicker.loadMap();
  });
});

test('.loadMap sets up the map with the center point defined as sent by lat and lng controls, but does not geocode the current address if current lat/lng are not the default values', function() {
  jack(function() {
    var hidLat = jack.create('hidLat', ['val']);
    var hidLng = jack.create('hidLng', ['val']);
    var lat = 'not the default';
    var lng = 'not the default';

    jack.expect('window.getServerElement')
      .withArguments('hidLatitude')
      .returnValue(hidLat);
    jack.expect('window.getServerElement')
      .withArguments('hidLongitude')
      .returnValue(hidLng);
    jack.expect('hidLat.val')
      .returnValue(lat);
    jack.expect('hidLng.val')
      .returnValue(lng);
    jack.expect('GooglePicker.setUpMap')
      .withArguments(lat, lng)
      .mock(noop);
    jack.expect('GooglePicker.geocodeLocation')
      .never();

    GooglePicker.loadMap();
  });
});

test('.setUpMap creates map, sets the center from the provided arguments, creates the center marker, and sets the dblclick event listener', function() {
  jack(function() {
    var map = jack.create('map', ['setCenter']);
    var lat = GooglePicker.DEFAULTS.lat;
    var lng = GooglePicker.DEFAULTS.lng;

    jack.expect('GooglePicker.getMap')
      .mock(noop)
      .returnValue(map);
    jack.expect('map.setCenter')
      .mock(function(latLng) {
        equal(lat, latLng.lat);
        equal(lng, latLng.lng);
      });
    jack.expect('GooglePicker.getCenterMarker')
      .mock(noop);
    jack.expect('google.maps.event.addListener')
      .withArguments(map, 'dblclick', GooglePicker.map_dblclick);

    GooglePicker.setUpMap(lat, lng);
  });
});

test('.createGeocoder creates and returns a new google.maps.Geocoder object', function() {
  var actual = GooglePicker.createGeocoder();

  same(google.maps.Geocoder, actual.constructor);
});

test('.geocodeLocation makes a geocode request from google maps if the current location is not empty', function() {
  jack(function() {
    var geocoder = jack.create('geocoder', ['geocode']);
    var txtLocation = jack.create('txtLocation', ['val'])
    var location = 'this is the location';
    var callback = new Object();

    jack.expect('jQuery')
      .withArguments('#txtLocation')
      .returnValue(txtLocation);
    jack.expect('txtLocation.val')
      .returnValue(location);
    jack.expect('GooglePicker.createGeocoder')
      .returnValue(geocoder);
    jack.expect('GooglePicker.getGeocoder_callback')
      .withArguments(location)
      .returnValue(callback);
    jack.expect('geocoder.geocode')
      .mock(function(opts, cb) {
        equal(location, opts.address);
        same(callback, cb);
      });

    GooglePicker.geocodeLocation();
  });
});

test('.areNewCoordinatesChosen returns true if center marker is not still at the default coordinates', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['getPosition']);
    var latLng = {
      lat: function() { return 1; },
      lng: function() { return 2; }
    };

    jack.expect('GooglePicker.getCenterMarker')
      .mock(noop)
      .returnValue(centerMarker);
    jack.expect('centerMarker.getPosition')
      .returnValue(latLng);
    
    ok(GooglePicker.areNewCoordinatesChosen());
  });
});

test('.areNewCoordinatesChosen returns false if center marker is still at the default coordinates', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['getPosition']);
    var latLng = {
      lat: function() { return GooglePicker.DEFAULTS.lat; },
      lng: function() { return GooglePicker.DEFAULTS.lng; }
    };

    jack.expect('GooglePicker.getCenterMarker')
      .mock(noop)
      .returnValue(centerMarker);
    jack.expect('centerMarker.getPosition')
      .returnValue(latLng);
    
    ok(!GooglePicker.areNewCoordinatesChosen());
  });
});

test('.setReturnAndCloseWindow sets the return lat/lng values and closes the window', function() {
  jack(function() {
    var centerMarker = jack.create('centerMarker', ['getPosition']);
    var latLng = {
      lat: function() { return GooglePicker.DEFAULTS.lat; },
      lng: function() { return GooglePicker.DEFAULTS.lng; }
    };

    jack.expect('GooglePicker.getCenterMarker')
      .mock(noop)
      .returnValue(centerMarker);
    jack.expect('centerMarker.getPosition')
      .returnValue(latLng);
    jack.expect('GooglePicker.setCenterMarker')
      .withArguments(latLng)
      .mock(noop);
    jack.expect('GooglePicker.setReturnLatLng')
      .withArguments(latLng)
      .mock(noop);
    jack.expect('GooglePicker.closeWindow')
      .mock(noop);

    GooglePicker.setReturnAndCloseWindow(latLng);
  });
});

test('.delayAndSetMapCenter sets map center to given latLng after a half-second delay', function() {
  jack(function() {
    var latLng = new Object();

    jack.expect('GooglePicker.setMapCenter')
      .mock(noop)
      .withArguments(latLng);
    jack.expect('window.setTimeout')
      .mock(function (fn, timeout) {
        fn();
        equal(500, timeout);
      });

    GooglePicker.delayAndSetMapCenter.call(new Object(), latLng);
  });
});

//////////////////////////////EVENT HANDLERS//////////////////////////////

test('.btnSave_Click sets asset coordinates, and returns the result if current assetTypeID is in the list', function() {
  jack(function() {
    var assetTypeID = new Object();
    var assetTypeIDs = new Object();
    var result = new Object();

    jack.expect('GooglePicker.getAssetTypeID')
      .returnValue(assetTypeID);
    jack.expect('GooglePicker.getPhysicalAssetTypeIDList')
      .returnValue(assetTypeIDs);
    jack.expect('jQuery.inArray')
      .withArguments(assetTypeID, assetTypeIDs)
      .returnValue(0);
    jack.expect('GooglePicker.setAssetCoordinates')
      .returnValue(result);

    same(result, GooglePicker.btnSave_Click.call(new Object()));
  });
});

test('.btnSave_Click sets WorkOrder coordinates and returns result if assetTypeID is not in the list', function() {
  jack(function() {
    var assetTypeID = new Object();
    var assetTypeIDs = new Object();
    var result = new Object();

    jack.expect('GooglePicker.getAssetTypeID')
      .returnValue(assetTypeID);
    jack.expect('GooglePicker.getPhysicalAssetTypeIDList')
      .returnValue(assetTypeIDs);
    jack.expect('jQuery.inArray')
      .withArguments(assetTypeID, assetTypeIDs)
      .returnValue(-1);
    jack.expect('GooglePicker.setWorkOrderCoordinates')
      .returnValue(result);

    same(result, GooglePicker.btnSave_Click.call(new Object()));
  });
});

test('.getGeocoder_callback returns a callback fn that will set the center of the map to the point provided', function() {
  jack(function() {
    var map = jack.create('map', ['setCenter']);
    var point = {
      geometry: {
        location: new Object()
      }
    };
    var points = [point];
    var fn = GooglePicker.getGeocoder_callback();

    jack.expect('window.alert')
      .never();
    jack.expect('GooglePicker.setMapCenter')
      .mock(noop)
      .withArguments(point.geometry.location);
    jack.expect('GooglePicker.setCenterMarker')
      .mock(noop)
      .withArguments(point.geometry.location);

    fn.call(new Object(), points);
  });
});

test('.getGeocoder_callback returns a callback fn which will let the user know if a point could not be found for the given address', function() {
  jack(function() {
    var points = [];
    var location = 'this is the location';
    var expectedMessage = 'Address \'' + location + '\' was not found.  Please alter your search.';
    var fn = GooglePicker.getGeocoder_callback(location);

    jack.expect('GooglePicker.setMapCenter')
      .never();
    jack.expect('GooglePicker.setCenterMarker')
      .never();
    jack.expect('window.alert')
      .mock(noop)
      .withArguments(expectedMessage);

    fn.call(new Object(), points);
  });
});

test('.centerMarker_dragend sets the map center to the new coordinates after a delay', function() {
  jack(function() {
    var obj = {
      latLng: {
        lat: function() { return 1; },
        lng: function() { return 2; }
      }
    };
    
    jack.expect('GooglePicker.delayAndSetMapCenter')
      .mock(noop)
      .withArguments(obj.latLng);

    GooglePicker.centerMarker_dragend.call(new Object(), obj);
  });
});

test('.map_dblclick sets the center marker, sets the map center after a delay, and returns false', function() {
  jack(function() {
    var obj = {
      latLng: {
        lat: function() { return 1; },
        lng: function() { return 2; }
      }
    };
    
    jack.expect('GooglePicker.setCenterMarker')
      .mock(noop)
      .withArguments(obj.latLng);
    jack.expect('GooglePicker.delayAndSetMapCenter')
      .mock(noop)
      .withArguments(obj.latLng);

    ok(!GooglePicker.map_dblclick.call(new Object(), obj));
  });
});
