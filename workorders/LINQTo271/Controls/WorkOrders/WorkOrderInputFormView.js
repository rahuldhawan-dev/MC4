/////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////INPUT/EDIT//////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////


//////////////////////////////EVENT HANDLERS//////////////////////////////

var WorkOrderInputFormView = {
  PREMISE_RGX: /^[a-zA-Z0-9]\d{8,9}$/,

  btnSave_Click: function() {
    var valid = this.validateForm();

    if (valid) {
      this.toggleSaveButton();
      this.enableFields(['txtStreetNumber', 'txtZipCode']);
    }

    return valid;
  },

	ddlOperatingCenter_Change: function() {
		this.onOperatingCenterChanged();
	},

  ddlTown_Change: function() {
    this.onTownChanged();
  },

  ddlTownSection_Change: function() {
    this.onTownSectionChanged();
  },

  ddlStreet_Change: function() {
    this.onStreetChanged();
  },

  ddlNearestCrossStreet_Change: function() {
    this.onNearestCrossStreetChanged();
  },

  txtStreetNumber_Change: function() {
    this.onStreetNumberChanged();
  },

  ddlRequestedBy_Change: function(elem) {
    this.onRequesterChanged(elem.selectedIndex);
  },

  ddlAssetType_Change: function(elem) {
    this.onAssetTypeChanged($(elem).val());
  },

  ddlAssetID_Change: function(elem) {
    this.onAssetIDChanged($(elem).val());
  },

  ddlDescriptionOfWork_Change: function(elem) {
  	this.onDescriptionOfWorkChanged();
	  this.setTownCriticalMainBreakNotes();
  },

  ddlPlantMaintenanceActivityTypeOverride_Change: function() {
    this.onPlantMaintenanceActivityTypeOverrideChanged();
  },

  txtPremiseNumber_Change: function(elem) {
    var assetID = elem.value;
    if (!this.PREMISE_RGX.test(assetID))
      return;
    else
      this.onAssetIDChanged(assetID);
  },

  llpAsset_Click: function(event) {
    // avoid 'this' issues by calling a lambda and setting the receiver
    // explicitly to WorkOrderInputFormView
    return (function(event) {
      if (!this.isAssetChosen()) {
        alert('Please select an Asset Type and Asset before attempting to set its coordinates.');
        return false;
      }
      this.setAssetAddressField();
      return true;
    }).call(WorkOrderInputFormView, event);
  },

  rdoRevisit_Click: function(arg) {
    (arg == 'revisit') ? this.toggleForRevisit() : this.toggleForInitial();
  },

  txtOriginalOrderNumber_Change: function(elem) {
    elem = $(elem);
    if (elem.val()) {
      var orderNumber = parseInt(elem.val(), 10);
      $.ajax({
        type: 'POST',
        url: '../WorkOrdersServiceView.asmx/GetWorkOrderById',
        data: '{id: ' + orderNumber + '}',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function(msg) {
          WorkOrderInputFormView.loadExistingOrderData.call(WorkOrderInputFormView,
                                                            msg.d);
        },
        error: function(msg) {
          alert('Error loading existing order number ' + orderNumber);
        }
      });
    }
  },

  ////////////////////////////EVENT PASSTHROUGHS////////////////////////////

	onOperatingCenterChanged: function() {
		this.setTownCriticalMainBreakNotes();
	},

  onTownChanged: function() {
  	this.setAssetAddressField();
	  this.setTownCriticalMainBreakNotes();
  },

  onTownSectionChanged: function() {
    this.setAssetAddressField();
  },

  onStreetChanged: function() {
    this.setAssetAddressField();
  },

  onNearestCrossStreetChanged: function() {
    this.setAssetAddressField();
  },

  onStreetNumberChanged: function() {
    this.setAssetAddressField();
  },

  onRequesterChanged: function(requesterID) {
    var label = '&nbsp;';
    this.toggleCustomerInfo(false);
    this.toggleCallCenterInfo(false);
    this.toggleEmployeeInfo(false);
    this.toggleAcousticMonitoringInfo(false);
    switch (requesterID) {
      case REQUESTER_IDS.CUSTOMER:
        this.toggleCustomerInfo(true);
        label = 'Customer Name: ';
        break;
      case REQUESTER_IDS.EMPLOYEE:
        this.toggleEmployeeInfo(true);
        label = 'Employee Name: ';
        break;
      case REQUESTER_IDS.ACOUSTIC_MONITORING:
        this.toggleAcousticMonitoringInfo(true);
        label = 'Acoustic Monitoring: ';
        break;
      default:
        break;
    }
    $('#lblRequester').html(label);
  },

  onAssetTypeChanged: function (assetTypeID) {
    var label = 'Asset ID: ';
    var elemID = 'ddlDummyAssetID';
    this.hideAssetInputFields();
    this.setTownCriticalMainBreakNotes();
    switch (parseInt(assetTypeID)) {
      case ASSET_TYPE_IDS.VALVE:
        label = 'Valve ID: ';
        elemID = 'ddlValve';
        break;
      case ASSET_TYPE_IDS.HYDRANT:
        label = 'Hydrant ID: ';
        elemID = 'ddlHydrant';
        break;
      case ASSET_TYPE_IDS.SEWER_OPENING:
        label = 'Sewer Opening ID:';
        elemID = 'ddlSewerOpening';
        break;
      case ASSET_TYPE_IDS.STORM_CATCH:
        label = 'Storm Catch ID:';
        elemID = 'ddlStormCatch';
        break;
      case ASSET_TYPE_IDS.SERVICE:
      case ASSET_TYPE_IDS.SEWER_LATERAL:
        label = 'Premise #:<br/>Service #:';
        elemID = 'pnlService';
        break;
      case ASSET_TYPE_IDS.EQUIPMENT:
        label = 'Equipment:';
        elemID = 'ddlEquipment';
        break;
      case ASSET_TYPE_IDS.MAIN:
      case ASSET_TYPE_IDS.SEWER_MAIN:
        label = '';
        elemID = '';
        break;
      case ASSET_TYPE_IDS.MAIN_CROSSING:
        label = 'Main Crossing:';
        elemID = 'ddlMainCrossing';
        break;
      default:
        break;
    }
    /// AssetTypeID should be set on the page here. -ARR 20090309
    if (assetTypeID != null) {
      var hidAssetTypeID = getServerElementById('hidAssetTypeID');
      hidAssetTypeID.val(assetTypeID.toString());
    }

    getServerElementById('ddlDummyAssetID').hide();
    if (elemID != '') {
      getServerElementById(elemID).show();
    }
    $('#lblAssetID').html(label);
    this.getAssetCoordinates();
  },

  onAssetIDChanged: function(assetID) {
    var hidHistoryAssetID = getServerElementById('hidHistoryAssetID');
    var hidHistoryAssetTypeID = getServerElementById('hidHistoryAssetTypeID');
    var hidHistoryOperatingCenterID = getServerElementById('hidHistoryOperatingCenterID');
    var hidAssetID = getServerElementById('hidAssetID');
    var hidAssetTypeID = getServerElementById('hidAssetTypeID');
    if (assetID != "")
    	hidAssetID.val(assetID);
    hidHistoryAssetID.val(assetID);
    hidAssetTypeID.val(getServerElementById('ddlAssetType').val());
    hidHistoryAssetTypeID.val(hidAssetTypeID.val());
    hidHistoryOperatingCenterID.val(getServerElementById('ddlOperatingCenter').val());
    if (hidHistoryAssetID.length) {
      hidHistoryAssetID[0].onchange();
    }
    this.getAssetCoordinates();
  },

  onDescriptionOfWorkChanged: function(description) {
    const isMainBreak = this.isMainBreak();
    const workDescriptionId = getServerElementById('ddlDescriptionOfWork').val();
    const workDescription = WORK_DESCRIPTIONS.find(x => x.Id.toString() === workDescriptionId);
    const $trMainBreakInfo = $('.trMainBreakInfo');
    const $mainBreakLi = $('#mainBreakLi');

    $trMainBreakInfo[isMainBreak ? 'show' : 'hide']();

    if (!isMainBreak) {
      $('option:eq(0)',
        getServerElementById('ddlCustomerImpactRange'))
        .attr('selected', 'selected');
      $('option:eq(0)',
        getServerElementById('ddlRepairTimeRange'))
        .attr('selected', 'selected');
    }
    
    if ($mainBreakLi.length > 0) {
      $mainBreakLi.toggle(isMainBreak);
    }

    if (workDescription && workDescription.DigitalAsBuiltRequired) {
      getServerElementById('chkDigitalAsBuiltRequired')
          .prop('checked', true)
          .prop('disabled', true);
    } else {
      getServerElementById('chkDigitalAsBuiltRequired').removeProp('disabled');
    }
  },

  onPlantMaintenanceActivityTypeOverrideChanged: function () {
    var overrideVal = $(getServerElementById('ddlPlantMaintenanceActivityTypeOverride')[0]).val();
    // we need more clarification here because the account tab also includes this txtAccountCharged and we're dealing with the input view here.
    var accountChargedInputs = $("[id$='txtAccountCharged']");
    if (accountChargedInputs.length > 0) {
      var accountCharged = $('#' + accountChargedInputs[0].id);

      accountCharged.closest('.field-pair').toggle(overrideVal !== '');

      if (overrideVal === '18') {
        accountCharged.closest('.field-pair').toggle(false);
        accountCharged.val('');
      }
    }
  },

  /////////////////////////////UI FUNCTIONALITY/////////////////////////////

  isReadOnly: function() {
    return (getServerElementById('btnSave').length > 0);
  },

  initializeInputEdit: function() {
    this.initializeDate();
    this.tryInitializeEdit();
  },

  // the actual mode doesn't matter.  the change events for these
  // won't cause an error if there is no selected value in the ddls,
  // so this can simply be called during initializeInputEdit.
  tryInitializeEdit: function() {
    var ddlAssetType = getServerElementById('ddlAssetType');
    var ddlDescriptionOfWork = getServerElementById('ddlDescriptionOfWork');
    var ddlRequestedBy = getServerElementById('ddlRequestedBy');
    var ddlPlantMaintenanceActivityTypeOverride = getServerElementById('ddlPlantMaintenanceActivityTypeOverride');
    if (ddlPlantMaintenanceActivityTypeOverride.length > 0) {
      ddlPlantMaintenanceActivityTypeOverride[0].onchange();
    }
    ddlRequestedBy[0].onchange();
    window.setTimeout(function() {
      WorkOrderInputFormView.tryInitializeAssetType(ddlAssetType[0],
                                                    ddlDescriptionOfWork[0]);
    }, 50);
    //alert('ok, is work description populated?');
    $('#wbsDialogToggle').on('click', function () { $('#wbsDialog').dialog(); });
  },


  // ddlAssetType and ddlDescriptionOfWork are both cascading drop
  // downs, so they may not have values populated immediately once the
  // form is loaded.  this will wait long enough to have values before
  // acting upon them
  tryInitializeAssetType: function(ddlAssetType, ddlDescriptionOfWork) {
    if (!ddlAssetType.disabled && !ddlDescriptionOfWork.disabled) {
      ddlAssetType.onchange();
      ddlDescriptionOfWork.onchange();
    } else {
      window.setTimeout(function() {
        WorkOrderInputFormView.tryInitializeAssetType(ddlAssetType,
                                                      ddlDescriptionOfWork);
      }, 50);
    }
  },

  initializeDate: function() {
    var txtDateReceived = getServerElementById('ccDateReceived')[0];
    
    if (txtDateReceived != null && txtDateReceived.value == '') {
      txtDateReceived.value = toCalendarControlDateString(new Date());
    }
  },

  displayValidationMessage: function(msg) {
    this.toggleValidationArea(true);
    $('#tdNotificationArea').html(msg);
  },

  toggleValidationArea: function(visible) {
    toggleElementArray(visible, [
          $('#trNotificationArea')
      ]);
  },

  toggleCustomerInfo: function(visible) {
    toggleElementArray(visible, [
          $('.trCustomerInfo'),
          getServerElementById('txtCustomerName')
      ]);
  },

  toggleEmployeeInfo: function(visible) {
    toggleElementArray(visible, [
          getServerElementById('ddlRequestingEmployee')
      ]);
  },

  toggleAcousticMonitoringInfo: function (visible) {
    toggleElementArray(visible, [
      getServerElementById('ddlAcousticMonitoringType')
    ]);
  },

  toggleCallCenterInfo: function(visible) {
  },

  toggleSaveButton: function() {
    getServerElementById('btnSave').hide();
    $('#btnDummySave').show();
  },

  toggleForRevisit: function() {
    this.resetDescriptionsOfWork('revisit');
    $('#trOrderNumbers').show();
    var txtOriginalOrderNumber = getServerElementById('txtOriginalOrderNumber');
    if (txtOriginalOrderNumber.val()) {
      this.txtOriginalOrderNumber_Change(txtOriginalOrderNumber);
    } else {
      alert('Please enter the number of the original order being revisited.');
      txtOriginalOrderNumber.focus();
    }
  },

  toggleForInitial: function() {
    $('#trOrderNumbers').hide();
    this.resetDescriptionsOfWork('initial');
    this.enableFields(['ddlOperatingCenter',
                       'ddlTown',
                       'ddlTownSection',
                       'txtStreetNumber',
                       'ddlStreet',
                       'ddlNearestCrossStreet',
                       'txtZipCode']);
  },

  hideAssetInputFields: function() {
    toggleElementArray(false, [
          getServerElementById('ddlValve'),
          getServerElementById('ddlHydrant'),
          getServerElementById('ddlMain'),
          getServerElementById('ddlSewerOpening'),
          getServerElementById('ddlStormCatch'),
          getServerElementById('pnlService'),
					getServerElementById('ddlEquipment'),
          getServerElementById('ddlMainCrossing')
      ]);
    toggleElementArray(true, [
          getServerElementById('ddlDummyAssetID')
      ]);
  },

  resetForm: function() {
    this.toggleCustomerInfo(false);
  },

  setField: function(fieldID, value, childID) {
    getServerElementById(fieldID)
      .val(value);

    childID = childID || [];
    if (typeof (childID) == 'string') {
      childID = [childID];
    }

    for (var i = childID.length - 1; i >= 0; --i) {
      $find(childID[i])._onParentChange(false, null);
    }
  },

  disableField: function(fieldID) {
    getServerElementById(fieldID).attr('disabled', 'disabled');
  },

  setAndDisableField: function(fieldID, value, childID) {
    WorkOrderInputFormView.setField(fieldID, value, childID);
    WorkOrderInputFormView.disableField(fieldID);
  },

  waitThenSetAndDisableField: function (fieldID, value, childID, callback) {
    if (getServerElementById(fieldID).attr('disabled') || getServerElementById(fieldID).attr('disabled') == 'disabled') {
      window.setTimeout(function() {
        WorkOrderInputFormView.waitThenSetAndDisableField(fieldID, value,
                                                          childID, callback);
      }, 500);
    } else {
      WorkOrderInputFormView.setAndDisableField(fieldID, value, childID);
      if (callback) {
        callback();
      }
    }
  },

  waitThenSetField: function(fieldID, value, childID, callback) {
    if (getServerElementById(fieldID).attr('disabled') == true) {
      window.setTimeout(function() {
        WorkOrderInputFormView.waitThenSetField(fieldID, value, childID,
                                                callback);
      }, 500);
    } else {
      WorkOrderInputFormView.setField(fieldID, value, childID);
      if (callback) {
        callback();
      }
    }
  },

  enableFields: function(fieldIDs) {
    for (var i = fieldIDs.length - 1; i >= 0; --i) {
      getServerElementById(fieldIDs[i]).removeAttr('disabled');
    }
  },

  ////////////////////////////////VALIDATION////////////////////////////////

  validateForm: function() {
    this.toggleValidationArea(false);
    switch (false) {
      case this.validateLocationInfo():
      case this.validateJobInfo():
      case this.validateRequesterInfo():
      case this.validateAssetInfo():
      case this.validateMainBreakInfo():
      case this.validateForRevisit():
      case this.validateSAPNotificationNumber():
      case this.validateSAPWorkOrderNumber():
      case this.validateAccountCharged():
      case this.validateDigitalAsBuiltInfo():
        return false;
      default:
        return true;
    }
  },

  validateSAPNotificationNumber: function () {
    var txtSAPNotificationNumber = getServerElementById('txtSAPNotificationNumber');
    if (txtSAPNotificationNumber.val() == '')
      return true;
    if (/(^0)/.test(txtSAPNotificationNumber.val())) {
    	this.displayValidationMessage('Please enter a valid SAP Notification #');
    	txtSAPNotificationNumber.focus();
    	return false;
    }

    if (/^\d+$/.test(txtSAPNotificationNumber.val()))
      return true;
    
    this.displayValidationMessage('Please enter an integer for the SAP Notification #');
    txtSAPNotificationNumber.focus();
    return false;
  },

  validateSAPWorkOrderNumber: function () {
    var txtSAPWorkOrderNumber = getServerElementById('txtSAPWorkOrderNumber');
    if (txtSAPWorkOrderNumber.val() === '')
    	return true;
    if (/(^0)/.test(txtSAPWorkOrderNumber.val())) {
    	this.displayValidationMessage('Please enter a valid SAP WorkOrder #');
    	txtSAPWorkOrderNumber.focus();
    	return false;
    }
    if (/^\d+$/.test(txtSAPWorkOrderNumber.val()))
      return true;

    this.displayValidationMessage('Please enter an integer for the SAP WorkOrder #');
    txtSAPWorkOrderNumber.focus();
    return false;
  },

  validateRequesterInfo: function() {
    var ddlRequestedBy = getServerElementById('ddlRequestedBy');
    var requesterID = parseInt(ddlRequestedBy[0].value);
    switch (requesterID) {
      case REQUESTER_IDS.CUSTOMER:
        return this.validateCustomerInfo();
      case REQUESTER_IDS.EMPLOYEE:
        return this.validateEmployeeInfo();
      case REQUESTER_IDS.LOCAL_GOVERNMENT:
        return this.validateGovernmentInfo();
    	case REQUESTER_IDS.FRCC:
    		return this.validateFRCC();
      case REQUESTER_IDS.CALL_CENTER:
      case REQUESTER_IDS.ACOUSTIC_MONITORING:
      case REQUESTER_IDS.NSI:
		    return true;
    }
    this.displayValidationMessage('Please specify who requested this work order.');
    ddlRequestedBy.focus();
    return false;
  },

  validateLocationInfo: function() {
    var valid = true;
    var msg = '';
    var elem = null;
    var ddlOperatingCenter = getServerElementById('ddlOperatingCenter');
    var ddlTown = getServerElementById('ddlTown');
    var ddlTownSection = getServerElementById('ddlTownSection');
    var txtStreetNumber = getServerElementById('txtStreetNumber');
    var ddlStreet = getServerElementById('ddlStreet');
    var ddlNearestCrossStreet = getServerElementById('ddlNearestCrossStreet');
    switch (true) {
      case (ddlOperatingCenter[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the operating center.';
        elem = ddlOperatingCenter;
        break;
      case (ddlTown[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the town.';
        elem = ddlTown;
        break;
      case (ddlStreet[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the street.';
        elem = ddlStreet;
        break;
      case (txtStreetNumber.val() == ''):
        valid = false;
        msg = 'Please enter the nearest (or customer) house number.';
        elem = txtStreetNumber;
        break;
      case (ddlNearestCrossStreet[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the nearest cross street.';
        elem = ddlNearestCrossStreet;
        break;
    }
    if (!valid) {
      this.displayValidationMessage(msg);
      elem.focus();
    }
    return valid;
  },

  validateJobInfo: function() {
    var valid = true;
    var msg = '';
    var elem = null;
    var ddlAssetType = getServerElementById('ddlAssetType');
    var ddlDrivenBy = getServerElementById('ddlDrivenBy');
    var ddlPriority = getServerElementById('ddlPriority');
    var ddlDescriptionOfWork = getServerElementById('ddlDescriptionOfWork');
    var ddlMarkoutRequirement = getServerElementById('ddlMarkoutRequirement');
    var ccDateReceived = getServerElementById('ccDateReceived');
    switch (true) {
      case (ddlAssetType[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the asset type.';
        elem = ddlAssetType;
        break;
      case (ddlDrivenBy[0].selectedIndex < 1):
        valid = false;
        msg = 'Please choose the purpose of this order.';
        elem = ddlDrivenBy;
        break;
      case (ddlPriority[0].selectedIndex < 1):
        valid = false;
        msg = 'Please select job priority.';
        elem = ddlPriority;
        break;
      case (ddlDescriptionOfWork[0].selectedIndex < 1):
        valid = false;
        msg = 'Please select the description of work.';
        elem = ddlDescriptionOfWork;
        break;
      case (ddlMarkoutRequirement[0].selectedIndex < 1):
        valid = false;
        msg = 'Please select the markout requirement.';
        elem = ddlMarkoutRequirement;
        break;
      case (ccDateReceived.val() == ''):
        valid = false;
        msg = 'Please enter the date received.';
        elem = ccDateReceived;
        break;
    }
    if (!valid) {
      this.displayValidationMessage(msg);
      elem.focus();
      return false;
    }
    return valid;
  },

  validateCustomerInfo: function() {
    var valid = true;
    var msg = '';
    var elem = null;
    var txtCusomerName = getServerElementById('txtCustomerName');
    var txtPhoneNumber = getServerElementById('txtPhoneNumber');
    var txtStreetNumber = getServerElementById('txtStreetNumber');
    switch (true) {
      case (txtCusomerName.val() == ''):
        valid = false;
        msg = 'Please enter the customer\'s name.';
        elem = txtCusomerName;
        break;
      case (/_/.test(txtPhoneNumber.val())):
        valid = false;
        msg = 'Please enter the customer\'s phone number.';
        elem = txtPhoneNumber;
        break;
      case (txtStreetNumber.val() == ''):
        valid = false;
        msg = 'Please enter the customer\'s street number.';
        elem = txtStreetNumber;
        break;
    }
    if (!valid) {
      this.displayValidationMessage(msg);
      elem.focus();
    }
    return valid;
  },

  validateEmployeeInfo: function() {
    var ddlRequestingEmployee = getServerElementById('ddlRequestingEmployee');
    if (ddlRequestingEmployee.val() == '') {
      this.displayValidationMessage('Please select an employee.');
      ddlRequestingEmployee.focus();
      return false;
    }

    return true;
  },

  validateGovernmentInfo: function() {
    return true;
  },

  validateFRCC: function () {
  	var txtSAPNotificationNumber = getServerElementById('txtSAPNotificationNumber');
  	if (txtSAPNotificationNumber.val() == '' && getServerElement('ddlRequestedBy').val() === '5') {
			this.displayValidationMessage('Please enter an SAP Notification #');
			txtSAPNotificationNumber.focus();
			return false;
		}
		return true;
	},

  validateAssetInfo: function() {
    var valid = true;
    var msg = '';
    var elem = null;
    var ddlAssetType = getServerElementById('ddlAssetType');
    var ddlValve = getServerElementById('ddlValve');
    var ddlHydrant = getServerElementById('ddlHydrant');
    var ddlSewerOpening = getServerElementById('ddlSewerOpening');
    var ddlStormCatch = getServerElementById('ddlStormCatch');
    var ddlEquipment = getServerElementById('ddlEquipment');
    var ddlMainCrossing = getServerElementById('ddlMainCrossing');
    var txtServiceNumber = getServerElementById('txtServiceNumber');
    var txtPremiseNumber = getServerElementById('txtPremiseNumber');
    var assetTypeID = parseInt(ddlAssetType.val(), 10);
    switch (assetTypeID) {
      case ASSET_TYPE_IDS.VALVE:
        msg = 'Please choose a valve.';
        elem = ddlValve;
        break;
      case ASSET_TYPE_IDS.HYDRANT:
        msg = 'Please choose a hydrant.';
        elem = ddlHydrant;
        break;
      case ASSET_TYPE_IDS.SEWER_OPENING:
        msg = 'Please choose a sewer opening.';
        elem = ddlSewerOpening;
        break;
      case ASSET_TYPE_IDS.STORM_CATCH:
        msg = 'Please choose a storm/catch asset.';
        elem = ddlStormCatch;
        break;
    	case ASSET_TYPE_IDS.EQUIPMENT:
    		msg = 'Please choose a piece of equipment';
    		elem = ddlEquipment;
	      break;
      case ASSET_TYPE_IDS.MAIN_CROSSING:
        msg = 'Please choose a main crossing';
        elem = ddlMainCrossing;
        break;
      case ASSET_TYPE_IDS.MAIN:
      case ASSET_TYPE_IDS.SEWER_MAIN:
        break;
      case ASSET_TYPE_IDS.SERVICE:
      case ASSET_TYPE_IDS.SEWER_LATERAL:
        msg = 'Please enter a premise #.';
        elem = txtPremiseNumber;
        break;
    }

    valid = (elem == null || elem.val() != '');

    if (valid) {
      if (elem == txtPremiseNumber) {
        var premiseNumber = txtPremiseNumber.val();
        if (!this.PREMISE_RGX.test(premiseNumber)) {
          msg = 'The entered Premise Number is not valid.';
          valid = false;
        } else if (/^(\d)\1{8,9}$/.test(premiseNumber)) {
          msg = 'The premise number ' + premiseNumber + ' is not valid.  Please enter notes explaining why a placeholder premise number was used.';
          // if lblNotes exists, we're in edit and don't need to check, otherwise make sure they entered notes.
          valid = getServerElement('lblNotes').length > 0 || /^.{5}/.test(getServerElementById('txtNotes').val());
        }
      } 
      if (!this.areCoordinatesSet()) {
        valid = false;
        msg = 'Please enter the location for this order using the globe icon.';
        elem = null;
      }
    }

    if (!valid) {
      this.displayValidationMessage(msg);
      if (elem != null) {
        elem.focus();
      }
    }

    return valid;
  },

  validateMainBreakInfo: function() {
    var elem, msg, valid = true;
    if (this.isMainBreak()) {
      elem = getServerElementById('ddlCustomerImpactRange');
      if (elem.val() == '') {
        valid = false;
        msg = 'The order being entered is for a main break.  Please enter the estimated number of customers impacted by the break';
        elem.focus();
      }

      elem = getServerElementById('ddlRepairTimeRange');
      if (elem.val() == '') {
        valid = false;
	      msg = 'The order being entered is for a main break.  Please enter the estimated number of hours it will take to repair or replace the main.';
        elem.focus();
      }

      elem = getServerElementById('ddlCustomerAlert');
      if (elem.val() == '') {
        valid = false;
        msg = 'The order being entered is for a main break.  Please indicate whether a customer alert should be entered into the system.';
        elem.focus();
      }

      elem = getServerElementById('ddlSignificantTrafficImpact');
      if (elem.val() == '') {
        valid = false;
        msg = 'The order being entered is for a main break.  Please indicate whether this will cause a significant traffic impact.';
        elem.focus();
      }

      if (getServerElement('gvMainBreak').length > 0 && getServerElement('gvMainBreak')[0].rows[1].cells[0].innerHTML === '' && this.isCompleted()) {
        msg = 'The work description indicates a completed main break. Main break information needs to be entered.  Please click the "Main Break" tab, and enter a record for the main break information.';
        valid = false;
      }
    }

    if (!valid) {
      alert(msg);
    }

    return valid;
  },

  validateForRevisit: function() {
    if (getServerElementById('rdoRevisitRevisit').attr('checked') &&
        !getServerElementById('txtOriginalOrderNumber').val()) {
      getServerElementById('txtOriginalOrderNumber').focus();
      this.displayValidationMessage('Please enter the number of the original order being revisited.');
      return false;
    }
    return true;
  },

  validateAccountCharged: function() {
    var overrideVal = $(getServerElementById('ddlPlantMaintenanceActivityTypeOverride')[0]).val();
    var accountChargedInputs = $("[id$='txtAccountCharged']");
    if (accountChargedInputs.length > 0) {
      var accountCharged = $('#' + accountChargedInputs[0].id);
      
      if (overrideVal !== '' && overrideVal !== '18' && accountCharged.val() === '') {
        alert('WorkOrder must include a WBS Number if PMAT is being overridden.');
        accountCharged.focus();
        return false;
      }
    }
    return true;
  },

  validateDigitalAsBuiltInfo: function() {
    if (this.isCompleted() &&
        getServerElementById('chkDigitalAsBuiltRequired').prop('checked') &&
        getServerElementById('ddlDigitalAsBuiltCompleted').val() === '') {
      this.displayValidationMessage(
        'Please indicate whether or not a digital as-built has been completed.');
      return false;
    }
    return true;
  },

  isAssetChosen: function() {
    var chosen = false;
    var assetTypeID = parseInt(getServerElementById('ddlAssetType').val(), 10);

    switch (assetTypeID) {
      case ASSET_TYPE_IDS.VALVE:
      case ASSET_TYPE_IDS.HYDRANT:
      case ASSET_TYPE_IDS.SEWER_OPENING:
      case ASSET_TYPE_IDS.STORM_CATCH:
      case ASSET_TYPE_IDS.MAIN_CROSSING:
        chosen = (getServerElementById('hidAssetID').val() != '');
        break;
      case ASSET_TYPE_IDS.EQUIPMENT:
      case ASSET_TYPE_IDS.MAIN:
      case ASSET_TYPE_IDS.SERVICE:
      case ASSET_TYPE_IDS.SEWER_LATERAL:
      case ASSET_TYPE_IDS.SEWER_MAIN:
        chosen = true;
        break;
    }

    return chosen;
  },

  areCoordinatesSet: function() {
    return getServerElementById('imgShowPicker')[0].src.toString().indexOf('blue') > -1;
  },

  //////////////////////////////HELPER METHODS//////////////////////////////

  getAssetAddress: function() {
    var sb = '', tmp;
    var txtStreetNumber = getServerElementById('txtStreetNumber');
    var ddlStreet = getServerElementById('ddlStreet');
    var ddlTownSection = getServerElementById('ddlTownSection');
    var ddlTown = getServerElementById('ddlTown');
    var hidState = getServerElementById('hidState');

    //the single = is correct here. we are checking for undefined.
    if (tmp = txtStreetNumber.val()) {
      sb += tmp + ' ';
    }
    if (tmp = getSelectedText(ddlStreet)) {
      sb += tmp + ' ';
    }
    if (tmp = getSelectedText(ddlTownSection)) {
      sb += tmp + ' ';
    }
    if (tmp = getSelectedText(ddlTown)) {
      sb += tmp + ' ';
    }
    if (tmp = hidState.val()) {
      sb += tmp + ' ';
    }

    return sb.substring(0, sb.length ? sb.length - 1 : 0);
  },

  setAssetAddressField: function() {
    getServerElementById('hidAddress').val(this.getAssetAddress());
  },

  setTownCriticalMainBreakNotes: function () {
  	if (getServerElementById('ddlTown').val() != '' && this.isMainBreak()) {
		  $.ajax({
		  	type:'POST',
		  	url: '../../Towns/TownsServiceView.asmx/GetTownCriticalMainBreakNotes',
		  	data: '{townId: ' + getServerElementById('ddlTown').val() + '}',
		  	contentType: 'application/json; charset=utf-8',
		  	dataType: 'json',
		  	success: function (msg) {
		  		WorkOrderInputFormView.displayValidationMessage(msg.d);
		  	},
				error: function(msg) {
					this.displayValidationMessage('Error loading town critical main break notes.');
				}
		  });
	  } else {
	  	this.toggleValidationArea(false);
	  }
  },

  resetDescriptionsOfWork: function(contextKey) {
    var ddlAssetType = getServerElementById('ddlAssetType');
    var assetTypeID = ddlAssetType.val();
    var cddDescriptionOfWork = $find('cddDescriptionOfWork');

    ddlAssetType.val('');
    cddDescriptionOfWork._onParentChange(false, null);
    ddlAssetType.val(assetTypeID);
    if (this.isInput)
      contextKey += "input";
    cddDescriptionOfWork.set_contextKey(contextKey);
    cddDescriptionOfWork._onParentChange(false, null);
  },

  // ALTERNATIVE TO THIS IS PROPER INSERT/EDIT TEMPLATES
  isInput: function() {
    return !getServerElementById('rdoRevisitInitial').attr('disabled');
  },

  isMainBreak: function() {
    var rgxMainBreak = /^WATER MAIN BREAK REP(AIR|LACE)$/;
    var lblDescriptionOfWork = getServerElementById('lblDescriptionOfWork');
    return rgxMainBreak
      .test(lblDescriptionOfWork.length == 0 ?
            $(':selected', getServerElementById('ddlDescriptionOfWork')).text() :
            lblDescriptionOfWork.text());
  },

  isCompleted: () => getServerElementById('lblDateCompleted').html() !== '',

  ///////////////////////////////////AJAX///////////////////////////////////

  getAssetCoordinateValues: function() {
    var latitude = parseFloat(getServerElementById('hidLatitude').val(), 10);
    var longitude = parseFloat(getServerElementById('hidLongitude').val(), 10);

    return (latitude && longitude) ? { latitude: latitude, longitude: longitude} : null;
  },

  getAssetCoordinates: function () {
  	var picker = getServerElementById('imgShowPicker')[0];
	  picker.disabled = false;
    if (!this.isAssetChosen()) {
      this.setAssetCoordinates(null, '', ''); //ARR 20090309
      return;
    }

    var assetTypeID = parseInt(getServerElementById('hidAssetTypeID').val(), 10);
    var assetID = getServerElementById('hidAssetID').val();

    switch (assetTypeID) {
      case ASSET_TYPE_IDS.VALVE:
      case ASSET_TYPE_IDS.HYDRANT:
      case ASSET_TYPE_IDS.SEWER_OPENING:
    	case ASSET_TYPE_IDS.STORM_CATCH:
      case ASSET_TYPE_IDS.EQUIPMENT:
      case ASSET_TYPE_IDS.MAIN_CROSSING:
        if (assetID) {
          $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: '../../Coordinates/CoordinateServiceView.asmx/GetCoordinatesForAsset',
            data: '{assetTypeID:' + assetTypeID + ',assetID:' + assetID + '}',
            dataType: 'json',
            success: function(resp) {
              WorkOrderInputFormView.setAssetCoordinates(resp.d);
            }
          });
        }
        break;
      case ASSET_TYPE_IDS.MAIN:
      case ASSET_TYPE_IDS.SERVICE:
      case ASSET_TYPE_IDS.SEWER_LATERAL:
      case ASSET_TYPE_IDS.SEWER_MAIN:
        var coordinates = this.getAssetCoordinateValues();
        if (coordinates) this.setAssetCoordinates(coordinates);
        break;
    }

	  if (assetTypeID == ASSET_TYPE_IDS.EQUIPMENT)
		  picker.disabled = true;
  },

  setAssetCoordinates: function(coord) {
    coord = coord || { latitude: '', longitude: '' };

    LatLonPicker.ImageButton.setIcon(getServerElementById('imgShowPicker'),
      getServerElementById('hidLatitude').val(coord.latitude),
      getServerElementById('hidLongitude').val(coord.longitude));
  },

  loadExistingOrderData: function(data) {
    this.setAndDisableField('ddlOperatingCenter',
                            data.operatingCenterID,
                            ['cddTowns', 'cddAssetType', 'cddRequestingEmployee']);
    this.waitThenSetAndDisableField('ddlTown',
                                    data.townID,
                                    ['cddTownSection', 'cddStreet',
                                     'cddNearestCrossStreet'], function() {
                                       $find('cddTowns').set_SelectedValue(data.townID.toString());
                                     });
    this.waitThenSetAndDisableField('ddlTownSection', data.townSectionID, null,
                                    function() {
                                      if (data.townSectionID != null)
                                        $find('cddTownSection').set_SelectedValue((data.townSectionID).toString());
                                    });
    this.setAndDisableField('txtStreetNumber', data.streetNumber);

    // asset depends heavily on street
    this.waitThenSetAndDisableField('ddlStreet', data.streetID,
                                    ['cddValve', 'cddHydrant', 'cddSewerOpening', 'cddStormCatch'],
                                    function() {
                                      $find('cddStreet').set_SelectedValue(data.streetID.toString());
                                      switch (data.assetTypeID) {
                                        case ASSET_TYPE_IDS.VALVE:
                                          WorkOrderInputFormView.waitThenSetField('ddlValve', data.valveID, null,
                                function() {
                                  $find('cddValve').set_SelectedValue(data.valveID.toString());
                                  WorkOrderInputFormView.onAssetIDChanged(data.valveID);
                                });
                                          break;
                                        case ASSET_TYPE_IDS.HYDRANT:
                                          WorkOrderInputFormView.waitThenSetField('ddlHydrant', data.hydrantID, null,
                                function() {
                                  $find('cddHydrant').set_SelectedValue(data.hydrantID.toString());
                                  WorkOrderInputFormView.onAssetIDChanged(data.hydrantID);
                                });
                                          break;
                                        case ASSET_TYPE_IDS.MAIN_CROSSING:
                                          WorkOrderInputFormView.waitThenSetField('ddlMainCrossing', data.mainCrossingID, null,
                                            function () {
                                              $find('cddMainCrossing').set_SelectedValue(data.mainCrossingID.toString());
                                              WorkOrderInputFormView.onAssetIDChanged(data.mainCrossingID);
                                            });
                                          break;
                                        case ASSET_TYPE_IDS.SEWER_OPENING:
                                          WorkOrderInputFormView.waitThenSetField('ddlSewerOpening', data.sewerOpeningID, null,
                                function() {
                                  $find('cddSewerOpening').set_SelectedValue(data.sewerOpeningID.toString());
                                  WorkOrderInputFormView.onAssetIDChanged(data.sewerOpeningID);
                                });
                                          break;
                                        case ASSET_TYPE_IDS.STORM_CATCH:
                                          WorkOrderInputFormView.waitThenSetField('ddlStormCatch', data.stormCatchID, null,
                                function() {
                                  $find('cddStormCatch').set_SelectedValue(data.stormCatchID.toString());
                                  WorkOrderInputFormView.onAssetIDChanged(data.stormCatchID);
                                });
                                          break;
                                        case ASSET_TYPE_IDS.SERVICE:
                                          WorkOrderInputFormView.setField('txtPremiseNumber', data.premiseNumber);
                                          WorkOrderInputFormView.setField('txtServiceNumber', data.serviceNumber);
                                          WorkOrderInputFormView.onAssetIDChanged(data.premiseNumber);
                                          break;
                                        case ASSET_TYPE_IDS.MAIN:
                                        case ASSET_TYPE_IDS.SEWER_MAIN:
                                          WorkOrderInputFormView.onAssetIDChanged(getServerElementById('ddlMain').val());
                                          break;
                                      }
                                    });

    this.waitThenSetAndDisableField('ddlNearestCrossStreet', data.crossStreetID,
                                    null, function() {
                                      $find('cddNearestCrossStreet').set_SelectedValue(data.crossStreetID.toString());
                                    });
    this.setAndDisableField('txtZipCode', data.zipCode);
    this.waitThenSetField('ddlAssetType', data.assetTypeID,
                          ['cddDescriptionOfWork'], function() {
                            $find('cddAssetType').set_SelectedValue(data.assetTypeID.toString());
                            WorkOrderInputFormView.onAssetTypeChanged(data.assetTypeID);
                            WorkOrderInputFormView.setAssetCoordinates(data);
                          });
  },

  /////////////////////////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////READ ONLY//////////////////////////////////////////////
  /////////////////////////////////////////////////////////////////////////////////////////////////////

  /////////////////////////////UI FUNCTIONALITY/////////////////////////////

  initializeReadOnly: function() {
    var assetTypeName = getServerElementById('lblAssetType').text();
    this.setAssetIDLabelFromAssetTypeName(assetTypeName);
    var requester = getServerElementById('lblRequestedBy').text();
    this.displayRequesterInfoByRequester(requester);
    this.tryInitializeMainBreakInfo();
  },

  tryInitializeMainBreakInfo: function() {
    if (this.isMainBreak()) {
    	$('.trMainBreakInfo').show();
			$('.trMainBreakTownInfo').show();
    }
  },

  setAssetIDLabelFromAssetTypeName: function(assetTypeName) {
    $('#divAssetLabel').text(assetTypeName + ' ID:');
  },

  displayRequesterInfoByRequester: function(requester) {
    switch (requester) {
      case 'Customer':
        this.displayCustomerInfo(true);
        break;
      case 'Call Center':
        this.displayCallCenterInfo(true);
        break;
      case 'Employee':
        this.displayEmployeeInfo(true);
        break;
      case 'Acoustic Monitoring':
        this.displayMonitoringInfo(true);
        break;
    }
  },

  displayCustomerInfo: function(show) {
    toggleElementArray(show, [
      getServerElementById('lblCustomerName'),
      $('.trCustomerInfo')]);
    $('#divRequesterLabel').text('Customer Name: ');
  },

  displayCallCenterInfo: function(show) {
  },

  displayEmployeeInfo: function(show) {
    toggleElementArray(show, [getServerElementById('lblRequestingEmployeeID')]);
    $('#divRequesterLabel').text('Requesting Employee: ');
  },

  displayMonitoringInfo: function(show) {
    toggleElementArray(show, [getServerElementById('lblAcousticMonitoringType')]);
    $('#divRequesterLabel').text('Acoustic Monitoring Type: ');
  }
};
