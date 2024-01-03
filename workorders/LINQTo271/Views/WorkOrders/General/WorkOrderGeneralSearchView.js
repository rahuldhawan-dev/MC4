var WorkOrderGeneralSearchView = {
  initialize: function () {
    var ddlAssetType = getServerElementById('ddlAssetType')[0];
    this.ddlAssetType_Change(ddlAssetType);
    getServerElementById('txtWorkOrderNumber').focus();
    getServerElementById('ddlIsAssignedToContractor')
      .change(WorkOrderGeneralSearchView.toggleContractor);

    Sys.Application.add_load(WorkOrderGeneralSearchView.cascadingLoaded);
  },
  cascadingLoaded: function () {
    var cdd = $find('cddContractor');
    if (cdd != null)
      cdd.add_populated(WorkOrderGeneralSearchView.toggleContractor);
  },
  //////////////////////////////EVENT HANDLERS//////////////////////////////
  ddlAssetType_Change: function (elem) {
  	this.onAssetTypeChanged(parseInt($(elem).val()));
  },
  ////////////////////////////EVENT PASSTHROUGHS////////////////////////////
  onAssetTypeChanged: function (assetTypeID) {
    switch (assetTypeID) {
      case ASSET_TYPE_IDS.VALVE:
        this.toggleAssetIDRow(true);
        this.changeAssetIDLabel('Valve ID:');
        break;
      case ASSET_TYPE_IDS.HYDRANT:
        this.toggleAssetIDRow(true);
        this.changeAssetIDLabel('Hydrant ID:');
        break;
      case ASSET_TYPE_IDS.SERVICE:
      case ASSET_TYPE_IDS.SEWER_LATERAL:
        this.toggleAssetIDRow(true);
        this.changeAssetIDLabel('Premise Number:');
        break;
      case ASSET_TYPE_IDS.SEWER_OPENING:
        this.toggleAssetIDRow(true);
        this.changeAssetIDLabel('Opening Number:');
        break;
    	case ASSET_TYPE_IDS.EQUIPMENT:
        this.toggleAssetIDRow(true);
        this.changeAssetIDLabel('EquipmentID:');
        break;

      default:
        this.toggleAssetIDRow(false);
        break;
    }
  },
  ////////////////////////////////UI HELPERS////////////////////////////////
  toggleAssetIDRow: function (show) {
    $('#trAssetID')[(show) ? 'show' : 'hide']();
  },
  changeAssetIDLabel: function (str) {
    $('#tdAssetIDLabel').text(str);
  },
  toggleContractor: function () {
    if (getServerElementById('ddlIsAssignedToContractor').val() != "true"
      || getServerElementById('ddlOperatingCenter').val() == "") {
      getServerElementById('ddlContractor').val(-1);
      getServerElementById('ddlContractor').attr('disabled', true);
    } else {
      getServerElementById('ddlContractor').attr('disabled', false);
    }
  }
};