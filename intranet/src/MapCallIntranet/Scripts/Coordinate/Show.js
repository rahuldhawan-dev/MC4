var CoordinateShow = {
    map: null,
    centerMarker: null,

    SELECTORS: {
        MAP_DIV: 'div#pickerMap',
        LATITUDE: 'input#Latitude',
        LOCATE: 'LocateButton',
        LONGITUDE: 'input#Longitude',
        ICON_URL: 'input#iconUrl',
        ICON_WIDTH: 'input#iconWidth',
        ICON_HEIGHT: 'input#iconHeight',
        GIS_LEGEND: 'div#gis-legend',
        GIS_LAYERS: 'div#gis-layers',
        BASEMAP_GALLERY: 'div#basemap-gallery'
    },

    DEFAULTS: {
        LATITUDE: 40.32246702,
        LONGITUDE: -74.14810180,
        ZOOM_LEVEL: 13
    },

    initialize: function () {
        CoordinateShow.map = $(CoordinateShow.SELECTORS.MAP_DIV).esriMap({
            mapId: Application.MAP_ID,
            callback: CoordinateShow.setCenterIcon,
            legendSelector: CoordinateShow.SELECTORS.GIS_LEGEND,
            layerToggleSelector: CoordinateShow.SELECTORS.GIS_LAYERS,
            basemapGallerySelector: CoordinateShow.SELECTORS.BASEMAP_GALLERY,
            startWithLayersOff: true,
            showBaseMapToggle: false,
            proxyOptions: Application.MAP_PROXY_OPTIONS,
            locateButton: CoordinateShow.SELECTORS.LOCATE
        });
    },

    setCenterIcon: function () {
        var latLng = CoordinateShow.getLatLng();
        CoordinateShow.map.esriMap('centerAndZoom',
            latLng.lng, latLng.lat, CoordinateShow.DEFAULTS.ZOOM_LEVEL);
        CoordinateShow.map.esriMap('addMarkerImage',
            $(CoordinateShow.SELECTORS.ICON_URL).val(),
            parseInt($(CoordinateShow.SELECTORS.ICON_WIDTH).val(), 10),
            parseInt($(CoordinateShow.SELECTORS.ICON_HEIGHT).val(), 10),
            'bottom-center', latLng.lng, latLng.lat);
    },

    getLatLng: function () {
        return {
            lat: parseFloat($(CoordinateShow.SELECTORS.LATITUDE).val()) || CoordinateShow.DEFAULTS.LATITUDE,
            lng: parseFloat($(CoordinateShow.SELECTORS.LONGITUDE).val()) || CoordinateShow.DEFAULTS.LONGITUDE
        };
    }
};