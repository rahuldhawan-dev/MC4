// Shared for Edit/New
var RecurringProjectEdit = {
	map: null,
	initialCoordinates: null,
	mainPicker: null,
	operatingCenterInfoMasterMapUrl: null, 
  isContractedOperationsUrl: null,

	SELECTORS: {
	  ASSET_CATEGORY: '#AssetCategory',
	  MAINS: 'img#mains',
    OPERATING_CENTER: '#OperatingCenter',
	  OVERRIDE_INFOMASTER_DESCISION: '#OverrideInfoMasterDecision',
		MAP_ID: '#OperatingCenter_InfoMasterMapId',
		MAP_LAYER: '#OperatingCenter_InfoMasterMapLayerName',
		PICKER_DIV: 'div#mainPickerMap',
		DELETE_ENDORSEMENT_FORM: 'form#deleteEndorsementForm',
    RECURRING_PROJECT_TYPE: '#RecurringProjectType',
	  TOTAL_INFOMASTER_SCORE: '#TotalInfoMasterScore'
	},
	initialize: function () {
		$(RecurringProjectEdit.SELECTORS.MAINS).click(RecurringProjectEdit.onMainClick);
		$(RecurringProjectEdit.SELECTORS.OPERATING_CENTER).change(RecurringProjectEdit.onOperatingCenterChange);
		RecurringProjectEdit.operatingCenterInfoMasterMapUrl = $('#OperatingCenterInfoMasterMapUrl').val();
		RecurringProjectEdit.isContractedOperationsUrl = $('#IsContractedOperationsUrl').val();
		$('form').submit(RecurringProjectEdit.onSubmit);
		AjaxTable.initialize('#wbsElementTable');
	  $(RecurringProjectEdit.SELECTORS.OPERATING_CENTER).on('change', RecurringProjectEdit.setFindLinkParameters);
    RecurringProjectEdit.setFindLinkParameters();
	  $(RecurringProjectEdit.SELECTORS.OVERRIDE_INFOMASTER_DESCISION).on('change',
	    RecurringProjectEdit.onOverrideInfomasterDecisionChange);
	},
	setFindLinkParameters: function () {
	  $('#WBSElementFindLink').attr('href', $('#WBSElementFindUrl').val() + '?operatingCenterId=' + $(RecurringProjectEdit.SELECTORS.OPERATING_CENTER).val());
	},
		createMainPickerDiv: function () {
		var div = $('<div id="mainPickerDiv" class="jqmWindow">' +
      '<div class="jqmTitle">' +
      '<button class="jqmClose">Close X</button>' +
      '<span class="jqmTitleText">Mains</span>' +
      '</div>' +
      '<iframe id="pickerFrame" class="jqmContent" src="../../../RecurringProjectMain/Edit/7329"></iframe>' +
      '</div>');
		$(document.body).append(div);
		return div;
	},
	closeMainPicker: function() {
		$('button.jqmClose')[0].click();
	},
	getMainPickerDiv: function () {
		var div = $(RecurringProjectEdit.SELECTORS.PICKER_DIV);
		return div.length ? div : RecurringProjectEdit.createMainPickerDiv();
	},
	hasMainsSelected: function () {
	  return $('#mains-details-table tr').length > 1;
	},
	onSubmit: function () {
		// constant also located in EditRecurringProject
	  if ($('#thisIsMyIdThereAreManyLikeItButThisOneIsMine').val() > 7500 && !RecurringProjectEdit.hasMainsSelected()) {
	    // bug 3551: If project type == "New" then infomaster is not required.
	    if ($(RecurringProjectEdit.SELECTORS.RECURRING_PROJECT_TYPE).find('option:selected').text() === 'New') {
	      return true;
	    }

	    // bug 3551: If asset category == "Wastewater" then infomaster is not required.
	    if ($(RecurringProjectEdit.SELECTORS.ASSET_CATEGORY).find('option:selected').text() === 'Wastewater') {
	      return true;
	    }

			alert('Please select one or more mains through the Infomaster Main Selector.');
			return false;
		}
		return true;
	},
	onMainClick: function () {
		RecurringProjectEdit.mainPicker = RecurringProjectEdit.getMainPickerDiv().jqm({ modal: true }).jqmShow();
	},
	onOperatingCenterChange: function() {
		var mapId = '';
		var mapLayer = '';
		$.ajax({
			url: RecurringProjectEdit.operatingCenterInfoMasterMapUrl,
			data: {
				id: $(RecurringProjectEdit.SELECTORS.OPERATING_CENTER).val()
			},
			async: false,
			type: 'GET',
			success: function(result) {
				mapId = result.InfoMasterMapId;
				mapLayer = result.InfoMasterMapLayerName;
			},
			error: function() {
				alert('There was a problem loading the map id for the selected operating center.');
			}
		});
		$(RecurringProjectEdit.SELECTORS.MAP_ID).val(mapId);
		$(RecurringProjectEdit.SELECTORS.MAP_LAYER).val(mapLayer);
  },
	onOverrideInfomasterDecisionChange: function () {
	  var totalScore = $(RecurringProjectEdit.SELECTORS.TOTAL_INFOMASTER_SCORE).val();
	  if (totalScore !== '' && totalScore <= 2.5) {
	    $(RecurringProjectEdit.SELECTORS.OVERRIDE_INFOMASTER_DESCISION).val('True');
	    alert('You must override the decision if the total infomaster score is 2.5 or less.');
	  }
	  return true;
	},
	togglePickerIcon: function () {
	  $(RecurringProjectEdit.SELECTORS.PICKER_GLOBE)
      .attr('src', '../../Content/images/map-icon-' + (RecurringProjectEdit.hasMainsSelected() ? 'blue' : 'red') + '.png');
	},
	validateDecadeInstalledOverride: function (val, elem) {
	  if (val === "")
	    return true;
    return (val.search(/^[0-9]*0$/) > -1);
  },
	validateWBSNumber: function(wbsValue, wbsElement) {
    // WBSNumber is required when the status is "Complete" and the OperatingCenter is NOT contracted operations
	  if (wbsValue) {
	    return true;
	  }

	  if ($('#Status option:selected').text() !== 'Complete') {
      return true; 
    }

	  var isContractedOperations = false;

	  $.ajax({
	    url: RecurringProjectEdit.isContractedOperationsUrl,
	    data: {
	      id: $(RecurringProjectEdit.SELECTORS.OPERATING_CENTER).val()
	    },
	    async: false, // The return value needs to be returned from this validation method.
	    type: 'GET',
	    success: function (result) {
	      isContractedOperations = result.IsContractedOperations;
	    },
	    error: function () {
	      alert('An error occured while validation WBS Number.');
	    }
	  });

	  return isContractedOperations;
	}

};
$(document).ready(RecurringProjectEdit.initialize);