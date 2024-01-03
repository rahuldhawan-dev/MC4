(function (global) {
	/**
	 * 
	 * This is a picker like we have for coordinates, but it lets us pickout mains(lines) from a map layer.
	 * The map layer's name/url used in this are dynamic and come from the parent/opening page.
	 * Mains are given a unique Guid that AMWater uses throughout their GIS that does not change. This
	 * was used as the key for the Mains instead of OBJECTID which changes whenever they change the map.
	 * The JSAPI must be used to draw the checkboxes in the layer menu to determine the LayerName
	 * Uncomment the RecurringProjectsMains.initSelectionLayer(); line in onMapCreated to get the map to load 
	 * then view source on the page to see what layername it stuck in the checkbox
	 * 
	 * There's lots of initialization going on.
	 * 1. The script initializes
	 * 2. The map initializes
	 * 3. Map has a callback for after it's loaded itself
	 * 4. onSelectionClear has to fire for some reason esri hasn't made clear
	 * 
	 */
  global.RecurringProjectsMains = {
    foo: null,
		map: null,
		mapId: null,
		mapLayerName: null,
		centerMarker: null,
		initialCoordinates: null,
		defaultSymbol: null,
		highlightSymbol: null,
		selectionLayer: null,				/* a layer we add to the map that mirrors the main layer that we can highlight our selection */
		selection: [],							/* the currently selected mains */
		currentSelection: [],				/* temporary holder for selections after the map tools finish  */
		recurringProjectId: null,
		initialized: false,

		SELECTORS: {
			ADDRESS: 'input#address',
			BASEMAP_GALLERY: 'div#basemap-gallery',
			DATA_DIV: '#theLayerInfo',
			FIND_BUTTON: 'button#find',
			GIS_LEGEND: 'div#gis-legend',
			GIS_LAYERS: 'div#gis-layers',
			HELP_DIV: '#theHelpInfo',
			HIDE_MAINS: 'div#hideMains',
			ICON_LIST: 'div#iconList',
			LATITUDE: 'input#Latitude',
			LONGITUDE: 'input#Longitude',
			MAP_DIV: '#mainPickerMap',
			OPERATING_CENTER_MAP_ID: '#OperatingCenter_InfoMasterMapId',
      OPERATING_CENTER_MAP_LAYER: '#OperatingCenter_InfoMasterMapLayerName',
			SELECT_EXTENT: '#btnExtent',
			SELECT_LINE: '#btnLine',
			SELECT_POLYGON: '#btnPolygon',
			SELECT_FREEHAND_POLYGON: '#btnFreehandPolygon',
			SELECT_CANCEL: '#btnCancelSelection',
			SELECT_CLEAR: '#btnClearSelection',
			SELECT_SAVE: '#btnSaveSelection',
			SELECT_HELP: '#btnHelp',
			SHOW_MAINS: 'div#showMains',
			TOTAL_LENGTH: '#spanTotalLength'
		},

		DEFAULTS: {
			LATITUDE: 39.399904,
			LONGITUDE: -74.511083,
			ZOOM_LEVEL: 15,
			OUT_FIELDS: ['OBJECTID', 'MainLength', 'FinalGrade', 'TrueTotal', 'FacilityID', 'Diameter_1', 'Material_1', 'InstallDate_1'],
			SELECT_MESSAGE: '',
			MAP_ID: 'af174de08bba4ea6aa936c277411d868',
			MAP_LAYER: 'wPressurizedMain_NJ3_9417'
		},
    
		/**
		 * This adds the items in .selection to the bottom left map display
		 * @returns {} 
		 */
		addItemsToSelectionDisplay: function () {
			var htmlToDisplay = '<table style="width:100%;">' +
				'<thead><tr>' +
					'<td>Risk</td>' +
					'<td>Length</td>' +
					'<td>Guid</td>' +
					'<td>Date Installed</td>' +
					'<td>Diameter</td>' +
					'<td>Material</td>' +
				'</tr></thead><tbody>';
			var totalLength = 0;
			$.map(RecurringProjectsMains.selection, function (obj) {
				totalLength += obj.MainLength;
				htmlToDisplay += '<tr>' +
					'<td>' + obj.FinalGrade + '</td>' +
					'<td>' + obj.MainLength.toFixed(2) + '</td>' +
					'<td style="white-space:nowrap;">' + (obj.FacilityID) + '</td>' +
				  '<td>' + RecurringProjectsMains.getDateInstalled(obj) + '</td>' +
				  '<td>' + RecurringProjectsMains.getDiameter(obj) + '</td>' +
				  '<td>' + RecurringProjectsMains.getMaterial(obj) + '</td>' +
				'</tr>';
			});
			htmlToDisplay = htmlToDisplay + '</tbody></table>';
			$(RecurringProjectsMains.SELECTORS.TOTAL_LENGTH).html(totalLength.toFixed(2));
			$(RecurringProjectsMains.SELECTORS.DATA_DIV).html(htmlToDisplay);
			$(RecurringProjectsMains.toggleHelp(false));
		},

		/**
		 * Completely removes the selected mains.
		 * @returns {} 
		 */
		clearSelection: function () {
			if (RecurringProjectsMains.selection.length === 0)
				return;
			var removal = new esri.tasks.Query();
			removal.where = RecurringProjectsMains.getSelectionWhereQuery(RecurringProjectsMains.selection);
			RecurringProjectsMains.selectionLayer.selectFeatures(removal, esri.layers.FeatureLayer.SELECTION_SUBTRACT);
			RecurringProjectsMains.selection = [];
      $(RecurringProjectsMains.SELECTORS.DATA_DIV).html(RecurringProjectsMains.DEFAULTS.SELECT_MESSAGE);
		  $(RecurringProjectsMains.SELECTORS.TOTAL_LENGTH).html(0);
		},

		geocodeLocation: function () {
			var location = $(RecurringProjectsMains.SELECTORS.ADDRESS).val();
			if (location) {
				RecurringProjectsMains.map.esriMap('locateAddress', location,
          RecurringProjectsMains.getGeocoderCallback(location));
			  RecurringProjectsMains.map.esriMap('setZoom', 15);
			}
		},

		getGeocoderCallback: function (location) {
			return function (latLng) {
				if (latLng) {
					RecurringProjectsMains.setMapCenter(latLng);
				} else {
					alert('Address \'' + location +
            '\' was not found. Please alter your search.');
				}
			};
		},

		getInfomasterGrade: function(score) {
			if (score >= 5.0)
				return 'F';
			if (score >= 4.5)
				return 'D-';
			if (score >= 4)
				return 'D';
			if (score >= 3.5)
				return 'C-';
			if (score >= 3)
				return 'C';
			if (score >= 2.5)
				return 'B-';
			if (score >= 2)
				return 'B';
			if (score >= 1.5)
				return 'A-';
			return 'A';
		},

		/**
		 * Helper method to create a hidden TD to plug into the parent page
		 * @param {} obj 
		 * @param {} i 
		 * @param {} name 
		 * @param {} mcName 
		 * @returns {} 
		 */
		getTdHidden: function (obj, i, val, mcName) {
			return '<input data-val="true" ' +
				 'id="RecurringProjectMains_' + i + '__' + mcName + '" ' +
				 'name="RecurringProjectMains[' + i + '].' + mcName + '" ' +
				 'type="hidden" value="' + val + '">	';
		},

		/**
		 * Helper method to get json from the inputs to make working with the data simpler
		 * @param {} inputs 
		 * @param {} rowIndex 
		 * @returns {} 
		 */
		getArrayOfJsonFromInputs: function (inputs, rowIndex) {
			var mains = [];
			for (var i = 0; i <= rowIndex; i++) {
				var main = {};
				$.each(inputs, function (index,obj) {
					if (obj.name.indexOf('[' + i + ']') > -1) {
						main[obj.name.substring(obj.name.indexOf('[' + i + ']') + 3 + i.toString().length)] = obj.value;
					}
				});
				mains.push(main);
			}
			return mains;
		},
    /**
     * 
     */
    getDiameter: function(obj) {
      return (obj.Diameter_1 === undefined) ? obj.Diameter : obj.Diameter_1;
    },
    getMaterial: function(obj) {
      return (obj.Material_1 === undefined) ? obj.Material : obj.Material_1;
    },
    getDateInstalled: function (obj) {
      if (obj.DateInstalled !== undefined)
        return obj.DateInstalled;
      return ((obj.InstallDate_1 === undefined) ? '' : new Date(obj.InstallDate_1).toLocaleDateString('en-US'));
    },
    
		/**
		 * Helper method to get the row count the array returned by the hidden inputs
		 * RecurringProjectMains[0].Guid, RecurringProjectMains[0].ConsequenceOfFailure,
		 * RecurringProjectMains[1].Guid, RecurringProjectMains[1].ConsequenceOfFailure,
		 * @param {} arr 
		 * @returns {} 
		 */
		getRowCount: function (arr) {
			var rowCount = 0;
			$.map(arr, function (obj) {
				if (obj.name.indexOf("[" + rowCount + "]") < 0) {
					rowCount++;
				}
			});
			return rowCount;
		},

		/**
		 * Queries our Selection Layer for objects that fall within the extent, then
		 * adds these to a currentSelection object which is then added to the full selection
		 * and added to the bottom left map display.
		 * @param {} extent - the extent generated by the selected map tool 
		 * @returns {} 
		 */
		getSelectedItems: function (extent) {
			RecurringProjectsMains.currentSelection = [];
			RecurringProjectsMains.map.esriMap('deactivateSelectionTool', '');
			// create a query for the extent
			var query = new esri.tasks.Query();
			query.geometry = extent;

			// query the feature layer for the extents
			RecurringProjectsMains.selectionLayer.queryFeatures(query, function (response) {
				var selectedFeatures = response.features;
				for (var i = 0; i < selectedFeatures.length; i++) {
					RecurringProjectsMains.currentSelection.push(selectedFeatures[i].attributes);
				}
				RecurringProjectsMains.selectItemsOnMap();
				RecurringProjectsMains.addItemsToSelectionDisplay();
			});
		},

		/**
		 * Instead of retrieving a map query by objectIds, get by FacilityID/Guid which we have saved.
		 * This generates a nice long where clause. If they plan on 100s of mains, this is a bad idea.
		 * @param {} selection 
		 * @returns {} 
		 */
		getSelectionWhereQuery: function (selection) {
			var where = '';
			$.map(selection, function (obj) { where += 'OR FacilityID = \'' + (obj.FacilityID) + '\' '; });
			return where.substring(3);
		},

		initialize: function () {
			RecurringProjectsMains.setMapId();
			RecurringProjectsMains.initMap();
			RecurringProjectsMains.recurringProjectId = global.parent.recurringProjectID;
			$(RecurringProjectsMains.SELECTORS.DATA_DIV).html(RecurringProjectsMains.DEFAULTS.SELECT_MESSAGE);
			$(RecurringProjectsMains.SELECTORS.FIND_BUTTON).click(RecurringProjectsMains.geocodeLocation);
			$(RecurringProjectsMains.SELECTORS.HIDE_MAINS).click(RecurringProjectsMains.toggleIconList);
			$(RecurringProjectsMains.SELECTORS.SHOW_MAINS).click(RecurringProjectsMains.toggleIconList);
		},

		toggleIconList: function (thing) {
			$(RecurringProjectsMains.SELECTORS.SHOW_MAINS).toggle();
			$(RecurringProjectsMains.SELECTORS.ICON_LIST).toggle();
		},

		initMap: function () {
			RecurringProjectsMains.map = $(RecurringProjectsMains.SELECTORS.MAP_DIV).esriMap({
				mapId: RecurringProjectsMains.mapId,
				//initialZoom: RecurringProjectsMains.DEFAULTS.ZOOM_LEVEL,
				callback: RecurringProjectsMains.onMapCreated,
				legendSelector: RecurringProjectsMains.SELECTORS.GIS_LEGEND,
				layerToggleSelector: RecurringProjectsMains.SELECTORS.GIS_LAYERS,
				basemapGallerySelector: RecurringProjectsMains.SELECTORS.BASEMAP_GALLERY,
				startWithLayersOff: false,
				showBaseMapToggle: false,
				showInfoWindowOnClick: false,
				proxyOptions: Application.MAP_PROXY_OPTIONS,
				onItemsSelected: RecurringProjectsMains.getSelectedItems
      });
			RecurringProjectsMains.loadExistingMainsIntoSelection('input:hidden[name^=RecurringProjectMains]');
		},

		/**
		 * Create a layer on the map that mirrors the main layer for toggling the 
		 * selected mains.
		 * @returns {} 
		 */
		initSelectionLayer: function () {
			var layerUrl = RecurringProjectsMains.map.esriMap('ensureGraphicsLayer', RecurringProjectsMains.mapLayerName).url;
			RecurringProjectsMains.selectionLayer = new esri.layers.FeatureLayer(layerUrl, {
				mode: esri.layers.FeatureLayer.MODE_ONDEMAND,
				outFields: RecurringProjectsMains.DEFAULTS.OUT_FIELDS,
				minScale: 0
			});

			// set the style of the mains when they are selected
			RecurringProjectsMains.selectionLayer.setSelectionSymbol(new esri.symbol.SimpleLineSymbol(
				esri.symbol.SimpleLineSymbol.STYLE_SOLID, new esri.Color([255, 0, 0]), 16.0
		 	));
			RecurringProjectsMains.selectionLayer.setOpacity(0.25);

			// set all the symbols to having no symbol
			var nullSymbol = new esri.symbol.SimpleMarkerSymbol().setSize(0);
			RecurringProjectsMains.selectionLayer.setRenderer(new esri.renderer.SimpleRenderer(nullSymbol));

			// add the layer to the map
			RecurringProjectsMains.map.esriMap('addLayer', RecurringProjectsMains.selectionLayer);
			RecurringProjectsMains.selectionLayer.on('selection-clear', function (evt) {
				RecurringProjectsMains.onSelectionClear(evt);
			});
		},

		/**
		 * Wire up the toolbar
		 * @returns {} 
		 */
		initToolbar: function () {
			$(RecurringProjectsMains.SELECTORS.SELECT_EXTENT).click(function () {
				RecurringProjectsMains.map.esriMap('activateSelectionTool', 'EXTENT');
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_FREEHAND_POLYGON).click(function () {
				RecurringProjectsMains.map.esriMap('activateSelectionTool', 'FREEHAND_POLYGON');
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_POLYGON).click(function () {
				RecurringProjectsMains.map.esriMap('activateSelectionTool', 'POLYGON');
			});

			$(RecurringProjectsMains.SELECTORS.SELECT_LINE).click(function () {
				RecurringProjectsMains.map.esriMap('activateSelectionTool', 'LINE');
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_CANCEL).click(function () {
				RecurringProjectsMains.map.esriMap('deactivateSelectionTool', '');
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_CLEAR).click(function () {
				RecurringProjectsMains.clearSelection();
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_SAVE).click(function () {
				RecurringProjectsMains.saveSelection();
			});
			$(RecurringProjectsMains.SELECTORS.SELECT_HELP).click(function () {
				RecurringProjectsMains.toggleHelp($(RecurringProjectsMains.SELECTORS.HELP_DIV).css('display') === 'none');
				});
		},

		/**
		 * Load the mains that are selected in the parent stored as input:hidden
		 * into an array of json objects. These can be used to set the selection and highlight
		 * the selected mains. ObjectId/GUID need to be fixed
		 * @param {} selector - selector to grab the hidden inputs from the parent page
		 * @returns {} 
		 */
		loadExistingMainsIntoSelection: function (selector) {
			var inputs = global.parent.$(selector).serializeArray();
			if (inputs.length < 1)
				return;
			var rowCount = RecurringProjectsMains.getRowCount(inputs);
			var mains = RecurringProjectsMains.getArrayOfJsonFromInputs(inputs, rowCount);
			$.each(mains, function (index, main) {
				RecurringProjectsMains.selection.push({
					'OBJECTID': main.Guid,
					'Guid': main.Guid,
					'FacilityID': main.Guid,
					'FinalGrade': main.Layer,
					'MainLength': parseFloat(main.Length),
					'Diameter': parseFloat(main.Diameter),
					'DateInstalled': main.DateInstalled,
					'Material': main.Material
				});
			});
			RecurringProjectsMains.toggleHelp(false);
		},

		map_doubleClick: function (latLng) {
			RecurringProjectsMains.setCenterMarker(latLng);
		},

		/**
		 * We can't just use OBJECTID to manage all the map objects because that changes over time.
		 * Instead we use Guid which will never change (the Guid for the main everywhere in their system)
		 * They also do not save it in that field name, instead it is in FacilityID
		 * @param {} obj 
		 * @param {} arr 
		 * @param {} prop 
		 * @returns {} 
		 */
		objectIsInArrayBasedOnPropertyValue: function (obj, arr, prop) {
			for (var i = 0; i < arr.length; i++) {
				if (obj[prop].toString() === arr[i][prop].toString())
					return true;
			}
			return false;
		},

		onMapCreated: function (map, response) {
			map.esriMap('disableDoubleClickZoom', RecurringProjectsMains.map_doubleClick);
			RecurringProjectsMains.initToolbar();
			RecurringProjectsMains.initSelectionLayer();
		},

		/**
		 * THIS IS IMPORTANT/ESSENTIAL
		 * There is some weirdness about the API that cannot be tracked down. I suspect it has to do with 
		 * the use of a second layer with the same url/name for the selections. For whatever reason this
		 * method gets fired and clears the selection. So we need to add the selection back.
		 * @param {} evt 
		 * @returns {} 
		 */
		onSelectionClear: function (evt) {
			if (RecurringProjectsMains.selection.length === 0)
				return;
			var selection = new esri.tasks.Query();

			selection.where = RecurringProjectsMains.getSelectionWhereQuery(RecurringProjectsMains.selection);
			RecurringProjectsMains.selectionLayer.selectFeatures(selection, esri.layers.FeatureLayer.SELECTION_ADD);
			RecurringProjectsMains.addItemsToSelectionDisplay();
			if (!RecurringProjectsMains.initialized) {
				RecurringProjectsMains.initialized = true;
				RecurringProjectsMains.selectionLayer.queryExtent(selection, function (extents) {
					RecurringProjectsMains.map.esriMap('setExtent', extents.extent.expand(3), true);
				});
			}
		},

		/**
		 * 
		 * @returns {} 
		 */
		saveSelection: function () {
		  if (!confirm('Are you sure you want to update the Mains and the Proposed Length for the project?\n\nTHE CURRENTLY SELECTED MAINS WILL NOT BE SAVED TO THE RECURRING PROJECT UNTIL YOU SAVE ON THE NEXT SCREEN.'))
				return;

			RecurringProjectsMains.updateParentSelectedMains('#mains-details-table', 'RecurringProjectMains');

			// lets set the parents coordinate 
			if (RecurringProjectsMains.selection.length > 0) {
			  RecurringProjectsMains.setSelectionCenterPoint();
			}

		  // toggle picker icon
		  global.parent.RecurringProjectEdit.togglePickerIcon();

			// hide the modal, we're done.
			global.parent.RecurringProjectEdit.closeMainPicker();
		},

		/**
		 * This checks the .currentSelection (objects the extents tool just captured) and adds or removes
		 * them from the .selection and also adds/removes them from the selection layer(i.e. highlights them).
		 * TODO: This method is too long.
		 */
		selectItemsOnMap: function () {
			var results = RecurringProjectsMains.currentSelection;

			// these object ids are already selected so we will remove them
			var toBeRemoved = $.grep(RecurringProjectsMains.selection, function (element) {
				return RecurringProjectsMains.objectIsInArrayBasedOnPropertyValue(element, results, 'FacilityID');
			});
			// these object ids are not selected, so we will select them
			var toAdd = results.filter(function (obj) {
				return !RecurringProjectsMains.objectIsInArrayBasedOnPropertyValue(obj, RecurringProjectsMains.selection, 'FacilityID');
			});

			// ok, we have some to remove, lets do it
			if (toBeRemoved.length > 0) {
				// remove them from the stored selection.
				RecurringProjectsMains.selection = RecurringProjectsMains.selection.filter(function (obj) { return toBeRemoved.indexOf(obj) == -1; });
				// remove them from the map selection
				var query = new esri.tasks.Query();
				query.where = RecurringProjectsMains.getSelectionWhereQuery(toBeRemoved);
				RecurringProjectsMains.selectionLayer.selectFeatures(query, esri.layers.FeatureLayer.SELECTION_SUBTRACT);
			}

			// ok, we have some to add, lets do it
			if (toAdd.length > 0) {
				// add them to our selection
				$.merge(RecurringProjectsMains.selection, toAdd);
				// add them to the map
				var query = new esri.tasks.Query();
				query.where = RecurringProjectsMains.getSelectionWhereQuery(toAdd);
				RecurringProjectsMains.selectionLayer.selectFeatures(query, esri.layers.FeatureLayer.SELECTION_ADD);
			}
		},

		setCenterMarker: function (latLng) {
			RecurringProjectsMains.setLatLng(latLng);
		},

		setMapCenter: function (latLng) {
			RecurringProjectsMains.map.esriMap('centerAt', latLng.lng, latLng.lat);
			RecurringProjectsMains.setCenterMarker(latLng);
		},

		setLatLng: function (latLng) {
			$(RecurringProjectsMains.SELECTORS.LATITUDE).val(latLng.lat);
			$(RecurringProjectsMains.SELECTORS.LONGITUDE).val(latLng.lng);
		},

		/**
		 * This gets the InfoMasterMapId and LayerName from the opening page. 
		 * They need to exist in the parent page within elements specified by the SELECTORS
		 * @returns {} 
		 */
		setMapId: function () {
			var mapId = global.parent.$(RecurringProjectsMains.SELECTORS.OPERATING_CENTER_MAP_ID).val();
			if (mapId === '')
				mapId = RecurringProjectsMains.DEFAULTS.MAP_ID;
			RecurringProjectsMains.mapId = mapId;

			var mapLayerName = global.parent.$(RecurringProjectsMains.SELECTORS.OPERATING_CENTER_MAP_LAYER).val();
			if (mapLayerName === '')
				mapLayerName = RecurringProjectsMains.DEFAULTS.MAP_LAYER;
			RecurringProjectsMains.mapLayerName = mapLayerName;
		},


    /**
     * We ened to set the parent records CoordinateID. 
     * We use this method to get the center point for the selected mains
     * then pass along it's extents to the method for setting the parent coordinate
     * -> setParentCoordinate
     * @returns {} 
     */
		setSelectionCenterPoint: function () {
      var query = new esri.tasks.Query();
        query.where = RecurringProjectsMains.getSelectionWhereQuery(RecurringProjectsMains.selection);

        RecurringProjectsMains.selectionLayer.queryExtent(query, function (extents) {
		    return RecurringProjectsMains.setParentCoordinate(extents);
	    });
		},
    /**
     * This method takes the selections extent's lat/lng and wires it up to the 
     * AJAX form on the picker and submits it to get the coordinateID for the lat/lng
     * @param {} extents 
     * @returns {} 
     */
    setParentCoordinate: function (extents) {
		  var centerPoint = extents.extent.getCenter();
		  CoordinateAjaxForm.Latitude.value = centerPoint.getLatitude();
		  CoordinateAjaxForm.Longitude.value = centerPoint.getLongitude();
		  CoordinateAjaxForm.saveCoordinates.click();
    },
    /**
     * This method sets the parent window's Coordinate ID field to the one
     * received from the AJAX call.
     * @param {} resultData 
     * @returns {} 
     */
    coordinateAjaxFormOnSubmit: function(resultData) {
      global.parent.$('#Coordinate').val(resultData.coordinateId);
    },

		/**
		 * Toggle the help text.
		 * @returns {} 
		 */
		toggleHelp: function(visible) {
			$(RecurringProjectsMains.SELECTORS.HELP_DIV).toggle(visible);
		},

		/**
		 * This will take the table with rows in the parent for the table selector and replace them with 
		 * all new rows that match the data selection.
		 * @param {} tableSelector 
		 * @param {} dataSelector 
		 * @returns {} 
		 */
		updateParentSelectedMains: function (tableSelector, dataSelector) {
			var tbody = global.parent.$(tableSelector + ' > tbody');
			var htmlToAdd = '';
			var length = 0.0;
			var totalScore = 0.0;
			$.map(RecurringProjectsMains.selection, function (obj, i) {
				length += obj.MainLength;
				totalScore += obj.FinalGrade * obj.MainLength; // A = 1, F = 6
				htmlToAdd += '<tr>' +
					'<td>' + '<input data-val="true" type="hidden" id="RecurringProjectMains_' + i + '__RecurringProject" ' +
											'name="RecurringProjectMains[' + i + '].RecurringProject" ' +
											'value="' + RecurringProjectsMains.recurringProjectId + '" />' +
										obj.FacilityID + RecurringProjectsMains.getTdHidden(obj, i, obj.FacilityID, 'Guid') + '</td>' +
					'<td>' + obj.FinalGrade + RecurringProjectsMains.getTdHidden(obj, i, obj.FinalGrade, 'Layer') + '</td>' +
					//'<td>' + obj.TrueTotal + RecurringProjectsMains.getTdHidden(obj, i, 'TrueTotal', 'TotalInfoMasterScore') + '</td>' +
					'<td>' + obj.MainLength.toFixed(2) + RecurringProjectsMains.getTdHidden(obj, i, obj.MainLength, 'Length') + '</td>' +
				  '<td>' + RecurringProjectsMains.getDateInstalled(obj) + RecurringProjectsMains.getTdHidden(obj, i, RecurringProjectsMains.getDateInstalled(obj), 'DateInstalled') + '</td>' +
				  '<td>' + RecurringProjectsMains.getDiameter(obj) + RecurringProjectsMains.getTdHidden(obj, i, RecurringProjectsMains.getDiameter(obj), 'Diameter') + '</td>' +
				  '<td>' + RecurringProjectsMains.getMaterial(obj) + RecurringProjectsMains.getTdHidden(obj, i, RecurringProjectsMains.getMaterial(obj), 'Material') + '</td>' +
					'</tr>';
			});
      tbody.html(htmlToAdd);
		  var totalInfoMasterScore = (totalScore / length).toFixed(1);
      global.parent.$('#TotalInfoMasterScore').val(totalInfoMasterScore);
      if ((totalScore / length).toFixed(1) <= 2) {
        global.parent.$('#OverrideInfoMasterDecision').val('True').change();
      }
			global.parent.$('#ProposedLength').val(length.toFixed(0));
		}
  }
})(this);