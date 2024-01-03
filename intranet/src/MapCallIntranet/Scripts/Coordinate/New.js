(function (global) {
    var getUrlVars = function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    };

    var useManualEntry = function () {
        return getUrlVars()['manual'] === 'true';
    };

    global.CoordinateEdit = {
        map: null,
        centerMarker: null,
        // initial coordinates, either from the defaults or the hidden inputs
        initialCoordinates: null,

        SELECTORS: {
            MAP_DIV: 'div#pickerMap',
            ADDRESS: 'input#address',
            FIND_BUTTON: 'button#find',
            SAVE_BUTTON: 'button#saveCoordinates',
            LATITUDE: 'input#Latitude',
            LONGITUDE: 'input#Longitude',
            ICON_ID: 'input#IconId',
            ICON_LIST: 'div#iconList',
            ICON: 'div#iconList button',
            COORDINATE_ID: 'input#CoordinateId',
            GIS_LEGEND: 'div#gis-legend',
            GIS_LAYERS: 'div#gis-layers',
            BASEMAP_GALLERY: 'div#basemap-gallery',
            DEVICE_LOCATION_BUTTON: 'button#useDeviceLocation'
        },

        DEFAULTS: {
            ZOOM_LEVEL: 4
        },

        initialize: function () {
            CoordinateEdit.initialCoordinates = CoordinateEdit.getLatLng();
            if (useManualEntry()) {
                CoordinateEdit.initManual();
            } else {
                CoordinateEdit.initMap();
            }
            CoordinateEdit.initEvents();
            CoordinateEdit.tryEnableSave();
            // CoordinateEdit.setParentCoordinateId();
        },

        initMap: function () {
            CoordinateEdit.map = $(CoordinateEdit.SELECTORS.MAP_DIV).esriMap({
                mapId: '4b54e6124c88409db3637ad4a6443fac', initialZoom: CoordinateEdit.DEFAULTS.ZOOM_LEVEL,
                center: CoordinateEdit.initialCoordinates, callback: CoordinateEdit.onMapCreated,
                legendSelector: CoordinateEdit.SELECTORS.GIS_LEGEND,
                layerToggleSelector: CoordinateEdit.SELECTORS.GIS_LAYERS,
                basemapGallerySelector: CoordinateEdit.SELECTORS.BASEMAP_GALLERY,
                startWithLayersOff: true,
                showBaseMapToggle: false,
                proxyOptions: Application.MAP_PROXY_OPTIONS,
                locateButton: CoordinateEdit.SELECTORS.LOCATE
            });
        },

        initManual: function () {
            $(CoordinateEdit.SELECTORS.LATITUDE + ',' + CoordinateEdit.SELECTORS.LONGITUDE).attr('type', 'text');
            $('<br /><label for="' + CoordinateEdit.SELECTORS.LATITUDE + '">Latitude:</label>')
                .insertBefore($(CoordinateEdit.SELECTORS.LATITUDE));
            $('<br /><label for="' + CoordinateEdit.SELECTORS.LONGITUDE + '">Longitude:</label>')
                .insertBefore($(CoordinateEdit.SELECTORS.LONGITUDE));
        },

        initEvents: function () {
            $(CoordinateEdit.SELECTORS.FIND_BUTTON).click(CoordinateEdit.geocodeLocation);
            $(CoordinateEdit.SELECTORS.ICON).click(CoordinateEdit.icon_click);
            if (navigator.geolocation) {
                $(CoordinateEdit.SELECTORS.DEVICE_LOCATION_BUTTON)
                    .show()
                    .click(CoordinateEdit.deviceLocationButton_click);
            }
            if (useManualEntry()) {
                $(CoordinateEdit.SELECTORS.LATITUDE + ',' + CoordinateEdit.SELECTORS.LONGITUDE)
                    .change(CoordinateEdit.tryEnableSave);
            }
        },

        onMapCreated: function (map, response) {
            map.esriMap('disableDoubleClickZoom', CoordinateEdit.map_doubleClick);
            $(CoordinateEdit.SELECTORS.FIND_BUTTON).prop('disabled', false);
            CoordinateEdit.setCenterIcon();
            // There might be an address pre-populated, so we want the map to select that.
            CoordinateEdit.geocodeLocation();
        },

        getParentCoordinatePicker: function () {
            return global.parent.CoordinatePicker;
        },

        setCenterIcon: function () {
            var iconId = $(CoordinateEdit.SELECTORS.ICON_ID).val();
            var icon = CoordinateEdit.getIconByIconId(iconId);
            if (!icon.length) {
                icon = CoordinateEdit.getDefaultIconInSet();
            }
            CoordinateEdit.getCenterMarker(icon);
            icon.click();
        },

        getIconByIconId: function (iconId) {
            return $(CoordinateEdit.SELECTORS.ICON_LIST + ' button[value="' + iconId + '"] img');
        },

        getDefaultIconInSet: function () {
            return $(CoordinateEdit.SELECTORS.ICON_LIST + ' button img[data-default-icon="true"]');
        },

        tryEnableSave: function () {
            var current = CoordinateEdit.getLatLng();
            var iconId = $(CoordinateEdit.SELECTORS.ICON_ID).val();
            if (iconId != '' && iconId != '0' &&
                (current.lat != CoordinateEdit.initialCoordinates.lat ||
                    current.lng != CoordinateEdit.initialCoordinates.lng)) {
                $(CoordinateEdit.SELECTORS.SAVE_BUTTON).removeAttr('disabled');
            }
        },

        setParentCoordinateId: function (data) {
            var coordinateId = data.coordinateId;
            var parentElem = parent.$('#' + CoordinateEdit.getQuerystringValue('valueFor'));
            if (coordinateId != '0') {
                parentElem.val(coordinateId);
            }
            CoordinateEdit.fixParentCoordinateIcon(coordinateId, parentElem);
            var latLngSpan = parentElem.parent().find('.cp-coordinate-values');
            latLngSpan.text(data.lat + ', ' + data.lng);
        },

        fixParentCoordinateIcon: function (coordinateId, parentElem) {
            var parentIcon = parent.$('img', parentElem.parent());
            var iconUrl = parentIcon.attr('src') || '';
            if (coordinateId != '' && coordinateId != '0') {
                parentIcon.attr('src', iconUrl.replace('red', 'blue'));
            } else {
                parentIcon.attr('src', iconUrl.replace('blue', 'red'));
            }
        },

        getLatLng: function () {
            return {
              lat: parseFloat($(CoordinateEdit.SELECTORS.LATITUDE).val()) || Application.DEFAULT_COORDINATES.lat,
              lng: parseFloat($(CoordinateEdit.SELECTORS.LONGITUDE).val()) || Application.DEFAULT_COORDINATES.lng
            };
        },

        setLatLng: function (latLng) {
            $(CoordinateEdit.SELECTORS.LATITUDE).val(latLng.lat);
            $(CoordinateEdit.SELECTORS.LONGITUDE).val(latLng.lng);
        },

        getCenterMarker: function ($icon) {
            if (!CoordinateEdit.centerMarker) {
                CoordinateEdit.centerMarker = CoordinateEdit.createCenterMarker($icon);
            }
            return CoordinateEdit.centerMarker;
        },

        createCenterMarker: function ($icon) {
            var center = CoordinateEdit.getLatLng();
            var marker = CoordinateEdit.map.esriMap('addMarkerImage', $icon.attr('src'), $icon.width(),
                $icon.height(), $icon.attr('data-icon-offset'),
                center.lng, center.lat);
            CoordinateEdit.map.esriMap('makeDraggable', marker, CoordinateEdit.centerMarker_dragend);
            return marker;
        },

        setCenterMarker: function (latLng) {
            CoordinateEdit.map.esriMap('moveMarker', CoordinateEdit.centerMarker, latLng.lng, latLng.lat);
            CoordinateEdit.setLatLng(latLng);
            CoordinateEdit.tryEnableSave();
        },

        setMapCenter: function (latLng) {
            CoordinateEdit.map.esriMap('centerAt', latLng.lng, latLng.lat);
            CoordinateEdit.setCenterMarker(latLng);
        },

        geocodeLocation: function () {
            var location = $(CoordinateEdit.SELECTORS.ADDRESS).val();
            if (location && location.indexOf('Select') !== 0) {
                CoordinateEdit.map.esriMap('locateAddress', location,
                    CoordinateEdit.getGeocoderCallback(location));
            }
        },

        getGeocoderCallback: function (location) {
            return function (latLng) {
                if (latLng) {
                    CoordinateEdit.setMapCenter(latLng);
                } else {
                    alert('Address \'' + location +
                        '\' was not found. Please alter your search.');
                }
            };
        },

        icon_click: function (e) {
            var $target = e.target.src ? $(e.target) : $('img', e.target);
            if (!useManualEntry()) {
                CoordinateEdit.map.esriMap('changeMarkerIcon',
                    CoordinateEdit.centerMarker,
                    $target.attr('src'),
                    $target.width(),
                    $target.height(),
                    $target.attr('data-icon-offset'));
            }
            $(CoordinateEdit.SELECTORS.ICON_ID).val($target.parent().val());
            CoordinateEdit.tryEnableSave();
        },

        deviceLocationButton_click: function () {
            navigator.geolocation.getCurrentPosition(function (latLng) {
                CoordinateEdit.setMapCenter({ lat: latLng.coords.latitude, lng: latLng.coords.longitude });
            });
        },

        delayAndSetMapCenter: function (latLng) {
            global.setTimeout(function () {
                CoordinateEdit.setMapCenter(latLng);
            }, 500);
        },

        centerMarker_dragend: function (latLng) {
            CoordinateEdit.delayAndSetMapCenter(latLng);
        },

        map_doubleClick: function (latLng) {
            CoordinateEdit.setMapCenter(latLng);
        },

        getQuerystringValue: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.search);
            return results == null ?
                '' : decodeURIComponent(results[1].replace(/\+/g, " "));
        },

        onSave: function (resultData) {
            CoordinateEdit.setParentCoordinateId(resultData);
            CoordinateEdit.getParentCoordinatePicker().close();
        }
    };
})(this);
