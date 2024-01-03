var CoordinateVerifier = (function ($) {
    var MAX_ZOOM = 19;
    var MAP_ID = '6f796eb2f2284f89be86048b80f5f6cf';
    var LAYER_ID = 'markers';
    var ICON_DICTIONARY = [
        5,  // black  - unselected
        8,  // blue   - american water
        18, // purple - geocode farm
        6,  // green  - data science toolkit
        4   // red    - user-selected
    ];
    var DEFAULT_ICON = ICON_DICTIONARY[0];
    var SELECTORS = {
        GENERAL: {
            MAP: '#Map',
            LEFT_FRAME: 'div#framecontent > div.innertube',
            LIST_ITEM: 'ul > li',
            ZOOM_EXTENTS: 'a#zoomExtents',
            TO_INIT: 'input#actionName'
        },
        INDEX: {
            LIST: 'div.innertube > ul > li.outer',
            DATA_ID: 'input[name=\'id\']',
            LATITUDE: 'input[name=\'latitude\']',
            LONGITUDE: 'input[name=\'longitude\']',
            SOURCE: 'input[name=\'source\']',
            PAGE_LINKS: 'div.pagination > a',
            BACK_TO_SEARCH: 'a#backToSearch'
        },
        EDIT: {
            LIST: 'div.innertube > div.edit',
            LIST_ITEMS: 'div.innertube > div.edit ul > li',
            BACK_TO_LIST: 'div.edit > a#backToList',
            LATITUDE: 'input#Latitude',
            LONGITUDE: 'input#Longitude',
            SAVE_BUTTON: 'button#save',
            FORM: 'form#editForm',
            ID: 'input#Id',
            VERIFIED_COORDINATE_ID: 'input#VerfiedCustomerCoordinateId',
            RETURN_URL: 'input#ReturnUrl'
        },
        SEARCH: {
            FORM: 'form#searchForm'
        }
    };

    // used mainly to call up one of the actions below
    var semiSafeEval = function (action) {
        if (!action.match(/^[A-Z][a-z]+$/)) {
            throw '`' + action + '\' is not a valid action name.';
        }

        return eval(action);
    };

    var Map = {
        _map: null,
        _icons: null,

        init: function (config, action) {
            Map._icons = config.icons;
            Map._map = $(SELECTORS.GENERAL.MAP).esriMap({
                mapId: MAP_ID,
                callback: Map.getOnMapCreatedFn(action),
                locateButton: Maps.SELECTORS.LOCATE
            });
        },

        getOnMapCreatedFn: function (action) {
            action = semiSafeEval(action);
            return function (map) {

                map.esriMap('ensureGraphicsLayer', LAYER_ID);
                map.esriMap('disableDoubleClickZoom');
                action.init();
            };
        },

        resetMarkerIcons: function () {
            $(Map.getAllMarkers()).each(function (i, marker) {
                Map.changeMarkerIcon(marker, Map.getIconById(DEFAULT_ICON));
            });
        },

        clearMarkerEvents: function () {
            // TODO: this is probably no longer valid
            //$(Map.getAllMarkers()).each(function (i, m) {
            //  gmaps.event.clearInstanceListeners(m);
            //});
        },

        // wrappers

        removeMarker: function (marker) {
            Map._map.esriMap('removeGraphic', marker);
        },

        attachMarkers: function (points, onLoad) {
            Map._map.esriMap('ensureGraphicsLayer', LAYER_ID);
            for (var i = points.length - 1; i >= 0; --i) {
                var point = points[i];
                var icon = Map.getIconById(point.iconId);
                Map._map.esriMap('addMarkerImageToLayer', LAYER_ID, icon.url, icon.width, icon.height, icon.offset, point.lng, point.lat, point.attr);
            }

            if (typeof (onLoad) == 'function') {
                onLoad();
            }
        },

        fitBounds: function () {
            Map._map.esriMap('fitLayerBounds', LAYER_ID);
        },

        getAllMarkers: function () {
            return Map._map.esriMap('getGraphicsFromLayer', LAYER_ID);
        },

        getIconById: function (iconId) {
            for (var i = Map._icons.length - 1; i >= 0; --i) {
                if (Map._icons[i].id == iconId) {
                    return Map._icons[i];
                }
            }

            return null;
        },

        changeMarkerIcon: function (marker, icon) {
            Map._map.esriMap('changeMarkerIcon', marker, icon.url, icon.width, icon.height, icon.offset);
        },

        getMarker: function (dataId) {
            var graphics = Map._map.esriMap('getGraphicsFromLayer', LAYER_ID);
            for (var i = graphics.length - 1; i >= 0; --i) {
                var graphic = graphics[i];
                if (graphic.attributes.id == dataId) {
                    return graphic;
                }
            }
            return null;
        },

        getCenter: function () {
            return Map._map.esriMap('getCenter');
        },

        setCenter: function (point) {
            Map._map.esriMap('centerAt', point.lng, point.lat);
        },

        clearAllMarkers: function () {
            Map._map.esriMap('clearLayer', LAYER_ID);
        }
    };

    var Common = {
        lastAction: null,

        gatherAndDisplayPoints: function (listSelector, mapFn, useDefaultIcon) {
            var points = [];
            var attempt = function () {
                $(listSelector).each(function (i, listElem) {
                    $(SELECTORS.GENERAL.LIST_ITEM, listElem).each(function (j, coordinateElem) {
                        var item = mapFn(coordinateElem);
                        if (useDefaultIcon) {
                            // use black icon unless hovered
                            item.iconId = DEFAULT_ICON;
                        }
                        points.push(item);
                    });
                });

                return Map.attachMarkers(points, function () {
                    Common.zoomExtents();
                });
            };
            var attempts = 0;

            try {
                attempts++;
                attempt();
            } catch (e) {
                Map.clearMarkerEvents();
                Map.clearAllMarkers();
                if (attempts < 2) {
                    attempts++;
                    attempt();
                } else {
                    throw (e);
                }
            }
        },

        zoomExtents: function () {
            Map.fitBounds();
        },

        zoomExtents_click: function () {
            Common.zoomExtents();
        },

        cleanUp: function () {
            Map.clearMarkerEvents();
            if (Common.lastAction) {
                Common.lastAction.cleanUp();
            }
            Map.clearAllMarkers();
        },

        changeScreen: function (url) {
            Common.cleanUp();
            $(SELECTORS.GENERAL.LEFT_FRAME).load(url, function (txt, status, request) {
                var toInit = semiSafeEval($(SELECTORS.GENERAL.TO_INIT).val());
                toInit.init();
                Common.lastAction = toInit;
            });
        },

        changeScreenPost: function (url, data) {
            Common.cleanUp();
            $.post(url, data, function (txt) {
                $(SELECTORS.GENERAL.LEFT_FRAME).html(txt);
                var toInit = semiSafeEval($(SELECTORS.GENERAL.TO_INIT).val());
                toInit.init();
                Common.lastAction = toInit;
            });
        }
    };

    var Index = {
        _lastPageUrl: null,
        _hoverEvents: null,
        _clickEvent: null,

        init: function () {
            // needs to happen before the points are displayed because there's no knowing
            // how long that will take
            Index.initEvents();
            Common.gatherAndDisplayPoints(SELECTORS.INDEX.LIST, Index.getCoordinateFromListItem, true);
        },

        initEvents: function () {
            $(SELECTORS.INDEX.LIST)
                .bind('mouseenter', Index.listItem_mouseover)
                .bind('mouseleave', Index.listItem_mouseout)
                .bind('click', Index.listItem_click);
            $(SELECTORS.INDEX.PAGE_LINKS).bind('click', Index.pageLink_click);
            $(SELECTORS.GENERAL.ZOOM_EXTENTS).bind('click', Common.zoomExtents_click);
            $(SELECTORS.INDEX.BACK_TO_SEARCH).bind('click', Index.backToSearch_click);
            Index._hoverEvents = Map._map.esriMap('onLayerHover', LAYER_ID, Index.marker_mouseover, Index.marker_mouseout);
            Index._clickEvent = Map._map.esriMap('onLayerClick', LAYER_ID, Index.marker_click);
        },

        cleanUp: function () {
            $(SELECTORS.INDEX.LIST).unbind('mouseenter').unbind('mouseleave').unbind('click');
            $(SELECTORS.INDEX.PAGE_LINKS).unbind('click');
            $(SELECTORS.GENERAL.ZOOM_EXTENTS).unbind('click');
            $(SELECTORS.INDEX.BACK_TO_SEARCH).unbind('click');
            $(Index._hoverEvents).each(function (i, e) {
                e.remove();
            });
            Index._clickEvent.remove();
        },

        getCoordinateFromListItem: function (elem, parentItem) {
            parentItem = parentItem || elem.parentElement.parentElement;
            elem = $(elem);
            return {
                lat: parseFloat(elem.data('latitude')),
                lng: parseFloat(elem.data('longitude')),
                iconId: ICON_DICTIONARY[parseInt(elem.data('source'), 10)],
                attr: {
                    parentItem: parentItem,
                    id: parseInt(elem.data('id'), 10)
                }
            };
        },

        getLocationFromListItem: function (elem) {
            return {
                dataId: parseInt($(elem).data('id'), 10),
                click: function () {
                    Index.listItem_click.call(elem);
                }
            };
        },

        getPageUrl: function () {
            // TODO: using document.URL here is dangerous,
            //       what if we had loaded from any page other than <site>/CustomerLocation
            //return Index._lastPageUrl || (document.URL + '/Index.frag');
            return Index._lastPageUrl || window.location.protocol + '//' + window.location.host + window.location.pathname + '/Index.frag' + window.location.search;
        },

        setPageUrl: function (url) {
            Index._lastPageUrl = url;
        },

        getQuerystringValue: function () {
            if (Index._lastPageUrl) {
                return Index._lastPageUrl.substring(Index._lastPageUrl.indexOf('?') + 1);
            }
            return window.location.search.replace('?', '');
        },

        // events

        marker_mouseover: function (e) {
            var parentItem = e.graphic.attributes.parentItem;
            Index.listItem_mouseover.call(parentItem);
            $(parentItem).addClass('hover');
        },

        marker_mouseout: function (e) {
            var parentItem = e.graphic.attributes.parentItem;
            Index.listItem_mouseout.call(parentItem);
            $(parentItem).removeClass('hover');
        },

        marker_click: function (e) {
            Index.listItem_click.call(e.graphic.attributes.parentItem);
        },

        listItem_mouseover: function () {
            $(SELECTORS.GENERAL.LIST_ITEM, this).each(function (j, coordinateElem) {
                var coord = Index.getCoordinateFromListItem(coordinateElem);
                var marker = Map.getMarker(coord.attr.id);
                var shape = marker.getDojoShape();
                Map.changeMarkerIcon(marker, Map.getIconById(coord.iconId));
                if (shape) {
                    shape.moveToFront();
                }
            });
        },

        listItem_mouseout: function () {
            Map.resetMarkerIcons();
        },

        listItem_click: function () {
            Index.setPageUrl(Index.getPageUrl());
            Common.changeScreen($(this).data('url'));
        },

        pageLink_click: function (e) {
            var url = $(this).attr('href');
            Index.setPageUrl(url);
            Common.changeScreen(url, Index);
            e.preventDefault();
            return false;
        },

        backToSearch_click: function (e) {
            try {
                Common.changeScreen($(e.target).data('url'));
            } finally {
                e.preventDefault();
                return false;
            }
        }
    };

    var Edit = {
        centerMarker: null,
        dragEvent: null,

        init: function () {
            Edit.initEvents();
            var token = Map._map.esriMap('onExtentChange', function (e) {
                // need to set the center marker after the extent changes by gathering and
                // displaying points
                Edit.centerMarker = Edit.createCenterMarker();
                Edit.initMapEvents();
                token.remove();
            });
            Common.gatherAndDisplayPoints(SELECTORS.EDIT.LIST, Edit.getCoordinateFromListItem);
        },

        initEvents: function () {
            $(SELECTORS.EDIT.BACK_TO_LIST).bind('click', Edit.backToList_click);
            $(SELECTORS.GENERAL.ZOOM_EXTENTS).bind('click', Common.zoomExtents_click);
            $(SELECTORS.EDIT.FORM).bind('submit', Edit.form_submit);
        },

        initMapEvents: function () {
            Edit.dragEvent = Map._map.esriMap('makeDraggable', Edit.centerMarker, Edit.centerMarker_dragend);
        },

        cleanUp: function () {
            $(SELECTORS.EDIT.BACK_TO_LIST).unbind('click');
            $(SELECTORS.GENERAL.ZOOM_EXTENTS).unbind('click');
            Edit.dragEvent.remove();
            Map.removeMarker(Edit.centerMarker);
            Edit.centerMarker = null;
            $(SELECTORS.EDIT.FORM).unbind('submit');
        },

        getCoordinateFromListItem: function (elem) {
            elem = $(elem);
            return {
                dataId: parseInt(elem.data('id'), 10),
                lat: parseFloat(elem.data('latitude')),
                lng: parseFloat(elem.data('longitude')),
                iconId: ICON_DICTIONARY[parseInt(elem.data('source'), 10)],
                click: function () {
                    Edit.existingMarker_click.call(elem);
                }
            };
        },

        createCenterMarker: function () {
            var icon = Map.getIconById(DEFAULT_ICON);
            var center = Map.getCenter();
            return Map._map.esriMap('addMarkerImage', icon.url, icon.width,
                icon.height, icon.offset, center.lng, center.lat);
        },

        // events

        backToList_click: function (e) {
            //if (!confirm('Are you sure?'))
            //    return false;
            Common.changeScreen(Index.getPageUrl());
            e.preventDefault();
            return false;
        },

        map_doubleClick: function (e) {
            Edit.centerMarker.setPosition(e.latLng);
            Edit.centerMarker_dragend(e);
        },

        centerMarker_dragend: function (e) {
            Map.setCenter(e);
            $(SELECTORS.EDIT.VERIFIED_COORDINATE_ID).val(null);
            $(SELECTORS.EDIT.LATITUDE).val(e.lat);
            $(SELECTORS.EDIT.LONGITUDE).val(e.lng);
            $(SELECTORS.EDIT.SAVE_BUTTON).removeAttr('disabled');
            Common.zoomExtents();
        },

        form_submit: function (e) {
            $(SELECTORS.EDIT.RETURN_URL).val(Index.getQuerystringValue());
            var form = $(SELECTORS.EDIT.FORM);
            try {
                Common.changeScreenPost(form.attr('action'), form.serialize());
            } finally {
                e.preventDefault();
                return false;
            }
        }
    };

    var Search = {
        init: function () {
            Search.initEvents();
            $('select').unobtrusiveDetergent();
        },

        initEvents: function () {
            $(SELECTORS.SEARCH.FORM).bind('submit', Search.form_submit);
        },

        cleanUp: function () {
            $(SELECTORS.SEARCH.FORM).unbind('submit');
        },

        // events

        form_submit: function (e) {
            var form = $(this);
            var url = form.attr('action') + '?' + form.serialize();
            Index.setPageUrl(url);
            Common.changeScreen(url);
            e.preventDefault();
            return false;
        }
    };

    // API
    // All we have is an init method. This needs to be the last statement in this
    // function.

    return {
        init: function (mapConfig, action) {
            Map.init(mapConfig, action);
        }
    };
})(jQuery);
