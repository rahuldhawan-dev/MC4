// TODO: Create a tabbed div in Layers to include GIS layer checkboxes.
// Maybe not the best idea because we need this on all the maps. Something built into jquery.esri.mappin.js would be best
// that dynamically adds an overlay on the map with a button to toggle the gis layer legend.
// There's also the built in legend thing that wateroutages uses, might be useful?
// RealTimeOperations.map.esriMap('getGraphicsLayerIds')
// RealTimeOperations.map.esriMap('getLayer', 'asbuilt_aug2014_7806').show()
// TODO: By default, load none of them, probably controlled with the map on arcgis.com
// TODO: Move temp methods out of jquery.esri.mappin.js and do them proper like in the js proj

var makeIcon = function (title, image, width, height) {
  return {
    url: '../../images/' + image,
    width: width,
    height: height,
    offset: 'center',
    title: title
  };
};

var RealTimeOperations = {
	MAP_ID: Application.MAP_ID,
	AJAX_REQUEST_TYPE: 'POST',
	CONTENT_TYPE_JSON: 'application/json;charset=utf-8',
	ITEM_DATA_URL: 'ifm.aspx',
	SERVICE_URL: 'MapServices.asmx/',
	ZOOM_MIN: 8,
	ZOOM_MAX: 19,

	ControlIDs: {
		MAP: '#map_canvas',
		DDL_RANGE: '#ddlRange',
		TXT_RANGE: '#txtRange',
		TXT_INTERVAL: '#txtInterval',
		LBL_UPDATED: '#lblUpdated',
		TXT_SEARCH: '#txtSearch',
		LEGEND: '#mapLegend'
	},

	icons: {
		workOrders: {
			wo0: makeIcon('Not Scheduled', 'icons/construction_gray.png', 32, 27),
			wo1: makeIcon('Scheduled', 'icons/construction_yellow.png', 32, 27),
			wo2: makeIcon('In Progress', 'icons/construction_red.png', 32, 27),
			wo3: makeIcon('Completed', 'icons/construction_green.png', 32, 27),
		},

		complaints: {
			aesthetic: makeIcon('Aesthetic', 'icons/important_black.png', 24, 24),
			medical: makeIcon('Medical', 'icons/important_green.png', 24, 24),
			information: makeIcon('Information', 'icons/important_purple.png', 24, 24),
			other: makeIcon('Other', 'icons/important_red.png', 24, 24),
		},

		misc: {
			bactiSample: makeIcon('Bacti Sample', 'tap_brass.gif', 24, 28),
			leadCopperSample: makeIcon('Lead/Copper', 'tap_brass.gif', 24, 28),
			black: makeIcon('', 'm_ValB.png', 20, 34),
			event: makeIcon('Events', 'MapIcons/x_red.png', 24, 24),
			hydrant: makeIcon(null, 'icons/hydrant_green.png', 32, 32),
			investigation: makeIcon('Investigation', 'system-search.png', 16, 16),
			leak: makeIcon('Leak', 'droplet.gif', 22, 22),
			mo1: makeIcon('', 'icons/flag-red.png', 32, 32),
			mo2: makeIcon('', 'icons/flag-green.png', 32, 32),
			sewerOverflow: makeIcon('Sewer Overflow', 'icons/overflow.png', 32, 32),
			valve: makeIcon('', 'm_BOffBl.gif', 24, 27)
		},

		vehicles: {
			veh0: makeIcon('', 'icons/car.png', 22, 22),
			veh1: makeIcon('Meter Readers', 'icons/truck-yellow.png', 32, 30),
			veh2: makeIcon('FSRs', 'icons/truck-blue.png', 32, 30),
			veh3: makeIcon('Crew Trucks', 'icons/truck-brown.png', 32, 30),
			veh4: makeIcon('Dump Trucks', 'icons/truck-gray.png', 32, 30),
			veh5: makeIcon('Inspectors', 'icons/truck-green.png', 32, 30),
			veh6: makeIcon('Leak Detection', 'icons/truck-orange.png', 32, 30),
			veh7: makeIcon('Valve Truck', 'icons/truck-purple.png', 32, 30),
			veh8: makeIcon('Hydrant Inspectors', 'icons/truck-red.png', 32, 30),
			veh9: makeIcon('Pickup', 'icons/truck-lightblue.png', 32, 30),
			veh10: makeIcon('Meter Truck', 'icons/truck-pink.png', 32, 30),
			veh11: makeIcon('Vac-Truck', 'icons/truck-puke.png', 32, 30),
			veh12: makeIcon('Production', 'icons/truck-teal.png', 32, 30),
		}
	},

	UI: {
		updateData: function() {
			RealTimeOperations.ajax.runQueue();
			RealTimeOperations.UI.updateDataDate();
		},

		updateDataDate: function() {
			$(RealTimeOperations.ControlIDs.LBL_UPDATED).text(new Date().toLocaleTimeString());
		},

		toggleDateRange: function() {
			var txtRange = $(RealTimeOperations.ControlIDs.TXT_RANGE);
			var val = txtRange.val();
			txtRange.val(
				$(RealTimeOperations.ControlIDs.DDL_RANGE).val() == 'd' ? val / 24 : val * 24);
		}
	},

	LayerManagers: {
		hydrants: null,
		valves: null,
		workOrders: null,
		sandy: null,
		complaints: null,
		bactis: null,
		leadCoppers: null,
		events: null,
		vehicles: null,
		leaks: null,
		mainBreaks: null,
		overflows: null,
		flushingSchedules: null,
		investigations: null,
		onecalltickets: null,
		frccWorkOrders: null
  },

  attachMarker: function(coordinate, type, icon, layerId) {
    coordinate.type = type;
    RealTimeOperations.map.esriMap('addMarkerImageToLayer', layerId,
      icon.url, icon.width, icon.height, icon.offset, coordinate.lng, coordinate.lat, coordinate);
  },

  createLegendSection: function(name, icons) {
    var table = $('<table class="legend"></table>');
    table.append('<tr><td class="legend_header" colspan="2">' + name + ':</td></tr>');

     for (var key in icons) {
       var cur = icons[key];
       var tr = '<tr><td class="legend_row"><img src="' + cur.url + '" class="legend_icon"></td><td>' + cur.title + '</td></tr>';
       table.append(tr);
     }

    return table;
  },

  geocodeLocation: function() {
    var address = $(RealTimeOperations.ControlIDs.TXT_SEARCH).val();
    RealTimeOperations.map.esriMap('locateAddress', address);
  },

  getDataQueueInterval: function() {
    return parseInt($(RealTimeOperations.ControlIDs.TXT_INTERVAL).val(), 10) * 60000;
  },

  initialize: function() {
    RealTimeOperations.initializeLegend();
    MapMenu.initialize();
    RealTimeOperations.initializeMap();
    RealTimeOperations.initializeDataQueue();
    RealTimeOperations.UI.updateDataDate();
  },

  initializeDataQueue: function() {
    RealTimeOperations.dataIntervalId = window.setInterval(RealTimeOperations.UI.updateData, RealTimeOperations.getDataQueueInterval());
  },

  initializeLayers: function() {
    for (var layerId in RealTimeOperations.LayerManagers) {
      RealTimeOperations.map.esriMap('ensureGraphicsLayer', layerId);
      RealTimeOperations.map.esriMap('onLayerClick', layerId, RealTimeOperations.onMarkerClick);
    }

    RealTimeOperations.initializeTrafficLayer();
    RealTimeOperations.initializeWeatherLayer();
  },

  initializeTrafficLayer: function() {
    // TODO
    //this.trafficLayer = MapUtilities.createTrafficLayer();
  },

  initializeWeatherLayer: function() {
    // TODO
    // this.weatherLayer = MapUtilities.createWeatherLayer();
  },

  initializeLegend: function() {
    // TODO: HIDE THE LEGEND 

    var legendDiv = $('<div></div>');
    legendDiv.append(RealTimeOperations.createLegendSection('Work Orders', RealTimeOperations.icons.workOrders));
    legendDiv.append(RealTimeOperations.createLegendSection('Complaints', RealTimeOperations.icons.complaints));
    legendDiv.append(RealTimeOperations.createLegendSection('Vehicles', RealTimeOperations.icons.vehicles));
    legendDiv.append(RealTimeOperations.createLegendSection('Other', RealTimeOperations.icons.misc));
    $(RealTimeOperations.ControlIDs.LEGEND).append(legendDiv);
  },

  initializeMap: function() {
    $(RealTimeOperations.ControlIDs.MAP).esriMap({
      initialZoom: 4,
      mapId: RealTimeOperations.MAP_ID,
      callback: RealTimeOperations.onMapCreated,
      legendSelector: 'div#gisLegend',
      layerToggleSelector: 'div#mapGISLayers',
      basemapGallerySelector: 'div#basemapSelector',
      showBaseMapToggle: false,
      proxyOptions: {
      	url: '/proxies/ESRIProxy.ashx',
        proxies: ['www.arcgis.com', 'utility.arcgis.com', 'geoprocess-np.amwater.com', 'awdev.maps.arcgis.com', 'geoprocess.amwater.com', 'aw.maps.argis.com', 'onemap-np.amwaternp.com', 'onemap.amwater.com'],
      }
    });
  },

  onMapCreated: function(map) {
    RealTimeOperations.map = map;
    RealTimeOperations.initializeLayers();
  },

  generateInfoWindowContent: function(id, type) {
    var source = RealTimeOperations.ITEM_DATA_URL + '?ID=' + id + '&type=' + type;
    return '<iframe style="width:100%; height:100%;" src="' + source + '"></iframe>';
  },

  onMarkerClick: function(e) {
    var data = e.graphic.attributes;
    var frame = RealTimeOperations.generateInfoWindowContent(data.id, data.type);
    RealTimeOperations.map.esriMap('displayInfoWindowWithContent', frame, e.mapPoint, e.screenPoint, 500, 300);
    e.stopPropagation();
  },

  updateQueueInterval: function() {
    window.clearInterval(RealTimeOperations.intervalId);
    RealTimeOperations.initializeDataQueue();
  }
};

RealTimeOperations.ajax = {
  Dictionary: {
    valves: {
      serviceMethod: 'RecentValveInspections',
      markerType: 'val',
      markerImage: RealTimeOperations.icons.misc.valve
    },
    hydrants: {
      serviceMethod: 'RecentHydrantInspections',
      markerType: 'hyd',
      markerImage: RealTimeOperations.icons.misc.hydrant
    },
    workOrders: {
      serviceMethod: 'RecentWorkOrders',
      markerType: 'wo',
      callback: 'WorkOrders'
    },
    onecalltickets: {
      serviceMethod: 'RecentOneCallTickets',
      markerType: 'oct',
      callback: 'OneCallTickets'
    },
    sandy: {
      serviceMethod: 'RecentSandyWorkOrders',
      markerType: 'wo',
      callback: 'WorkOrders'
    },
    complaints: {
      serviceMethod: 'RecentWaterQualityComplaints',
      markerType: 'com',
      callback: 'Complaints'
    },
    bactis: {
      serviceMethod: 'RecentBactiResults',
      markerType: 'bac',
      markerImage: RealTimeOperations.icons.misc.bactiSample
    },
		leadCoppers: {
			serviceMethod: 'RecentLeadCoppers',
			markerType: 'led',
			markerImage: RealTimeOperations.icons.misc.leadCopperSample
		},
    events: {
      serviceMethod: 'ActiveEvents',
      markerType: 'evt',
      markerImage: RealTimeOperations.icons.misc.event
    },
    vehicles: {
      serviceMethod: 'Vehicles',
      markerType: 'veh',
      callback: 'Vehicles'
    },
    leaks: {
      serviceMethod: 'Leaks',
      markerType: 'lek',
      callback: 'Leaks'
    },
    investigations: {
      serviceMethod: 'Investigations',
      markerType: 'inv',
      markerImage: RealTimeOperations.icons.misc.investigation
    },
    overflows: {
      serviceMethod: 'RecentSewerOverflows',
      markerType: 'ovr',
      markerImage: RealTimeOperations.icons.misc.sewerOverflow
    },
    flushingSchedules: {
      serviceMethod: 'FlushingSchedules',
      markerType: 'flu',
      callback: 'FlushingSchedules'
    },
    mainBreaks: {
      serviceMethod: 'RecentMainBreaks',
      markerType: 'wo',
      callback: 'MainBreaks'
    },
		frccWorkOrders: {
			serviceMethod: 'RecentFRCCWorkOrders',
			markerType: 'wo',
			callback: 'FRCCWorkOrders'
		}
  },

  dequeue: function(name) {
    RealTimeOperations.ajax.Dictionary[name].inQueue = false;
  },

  enqueue: function(name) {
    RealTimeOperations.ajax.Dictionary[name].inQueue = true;
  },

  filterDotNetJsonData: function(json) {
    return $.parseJSON(json).d;
  },

  getAjaxItemCallback: function(icon, type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        RealTimeOperations.attachMarker(items[i], type, icon, layer);
      }
    };
  },

  getComplaintsCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        var icon = null;
        switch (coord.opt) {
        case 1:
          icon = RealTimeOperations.icons.complaints.aesthetic;
          break;
        case 2:
          icon = RealTimeOperations.icons.complaints.medical;
          break;
        case 3:
          icon = RealTimeOperations.icons.complaints.information;
          break;
        case 4:
          icon = RealTimeOperations.icons.complaints.other;
          break;
        }
        RealTimeOperations.attachMarker(coord, type, icon, layer);
      }
    };
  },

  getFlushingSchedulesCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        RealTimeOperations.map.esriMap('addCircleToLayer', layer, coord.lng, coord.lat, {
          radius: coord.opt,
          fillColor: [0, 0, 0, 0.25],
          lineColor: [0, 0, 0, 1]
        });
        RealTimeOperations.attachMarker(coord, type, RealTimeOperations.icons.misc.black, layer);
      }
    };
  },

  getLeaksCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        var icon = (coord.opt == 0) ? RealTimeOperations.icons.misc.leak : RealTimeOperations.icons.misc.investigation;
        RealTimeOperations.attachMarker(coord, type, icon, layer);
      }
    };
  },

  getMainBreaksCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        var icon = RealTimeOperations.icons.workOrders['wo' + coord.opt.toString()];
        RealTimeOperations.attachMarker(coord, type, icon, layer);
      }
    };
  },

	getFRCCWorkOrdersCallback: function(type, layer) {
		return function (items) {
			for (var i = items.length - 1; i >= 0; --i) {
				var coord = items[i];
				var icon = RealTimeOperations.icons.workOrders['wo' + coord.opt.toString()];
				RealTimeOperations.attachMarker(coord, type, icon, layer);
			}
		};
	},

  getOneCallTicketsCallback: function(type, layer) {
    return function(items) {
      // Otherwise we're gonna hammer the hell out of esri's servers and it's gonna
      // be really slow.
      var MAX_ITEMS_TO_GEOCODE = Math.min(50, items.length);
      for (var i = 0; i < MAX_ITEMS_TO_GEOCODE; i++) {
        var item = items[i];

        var location = item.title;
        RealTimeOperations.map.esriMap('locateAddress', location, (function (x) {
        	return function (coord) {
        		if (coord) {
        			x.lat = coord.lat;
        			x.lng = coord.lng;
        			var icon = RealTimeOperations.icons.misc['mo' + x.opt.toString()];
        			RealTimeOperations.attachMarker(x, type, icon, layer);
        		}
        	}
        })(item));
      }
    };
  },

  getVehiclesCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        var icon = RealTimeOperations.icons.vehicles['veh' + coord.opt.toString()];
        RealTimeOperations.attachMarker(coord, type, icon, layer);
      }
    };
  },

  getWorkOrdersCallback: function(type, layer) {
    return function(items) {
      for (var i = items.length - 1; i >= 0; --i) {
        var coord = items[i];
        var icon = RealTimeOperations.icons.workOrders['wo' + coord.opt.toString()];
        RealTimeOperations.attachMarker(coord, type, icon, layer);
      }
    };
  },

  getRangeInHours: function() {
    var val = $(RealTimeOperations.ControlIDs.TXT_RANGE).val();
    return $(RealTimeOperations.ControlIDs.DDL_RANGE).val() == 'd' ? val * 24 : val;
  },

  loadMarkerData: function(serviceMethod, callback) {
    $.ajax({
      type: RealTimeOperations.AJAX_REQUEST_TYPE,
      data: '{\'hours\': ' + RealTimeOperations.ajax.getRangeInHours() + '}',
      url: RealTimeOperations.SERVICE_URL + serviceMethod,
      contentType: RealTimeOperations.CONTENT_TYPE_JSON,
      converters: { "text json": RealTimeOperations.ajax.filterDotNetJsonData },
      success: callback
    });
  },

  loadSpecialData: function(name) {
    var ajaxItem = RealTimeOperations.ajax.Dictionary[name];
    RealTimeOperations.ajax.loadMarkerData(ajaxItem.serviceMethod,
      RealTimeOperations.ajax['get' + ajaxItem.callback + 'Callback'](ajaxItem.markerType, name));
  },

  loadStandardData: function(name) {
    var ajaxItem = RealTimeOperations.ajax.Dictionary[name];
    RealTimeOperations.ajax.loadMarkerData(ajaxItem.serviceMethod,
      RealTimeOperations.ajax.getAjaxItemCallback(ajaxItem.markerImage, ajaxItem.markerType, name));
  },

  loadData: function(name) {
    if (RealTimeOperations.ajax.Dictionary[name].callback == null) {
      RealTimeOperations.ajax.loadStandardData(name);
    } else {
      RealTimeOperations.ajax.loadSpecialData(name);
    }
  },

  reloadData: function(name) {
    RealTimeOperations.ajax.unloadData(name);
    RealTimeOperations.ajax.loadData(name);
  },

  runQueue: function() {
    for (var x in RealTimeOperations.ajax.Dictionary) {
      if (RealTimeOperations.ajax.Dictionary[x].inQueue) {
        RealTimeOperations.ajax.reloadData(x);
      }
    }
  },

  toggleData: function(name, show) {
    if (show) {
      RealTimeOperations.ajax.enqueue(name);
      RealTimeOperations.ajax.loadData(name);
    } else {
      RealTimeOperations.ajax.dequeue(name);
      RealTimeOperations.ajax.unloadData(name);
    }
  },

  unloadData: function(name) {
    RealTimeOperations.map.esriMap('clearLayer', name);
  },
};
