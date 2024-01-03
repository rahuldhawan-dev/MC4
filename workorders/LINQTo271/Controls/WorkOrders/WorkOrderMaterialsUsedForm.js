var WorkOrderMaterialsUsedForm = {
  ELEMENTS: {
    DESCRIPTION: 'txtNonStockDescription',
    PART_NUMBER: 'ddlPartNumber',
    STOCK_LOCATION: 'ddlStockLocation'
  },

  initialize: function () {
    getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.DESCRIPTION).click(this.txtNonStockDescription_Click);
    getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.DESCRIPTION).blur(this.txtNonStockDescription_Blur);
  },

  txtNonStockDescription_Click: function (e) {
    console.log('clicked');
  },

  handleUpdatePanelCallback: function () {
    WorkOrderMaterialsUsedForm.initialize();
  },

  //if a value was entered, then reset and disable the ddls for part/stocklocation
  //if it wasn't re-enable the ddls.
  txtNonStockDescription_Blur: function (e) {
    if (this.value !== '') {
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.PART_NUMBER).val('');
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.PART_NUMBER).attr('disabled', true);
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.STOCK_LOCATION).val('');
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.STOCK_LOCATION).attr('disabled', true);
    }
    else {
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.PART_NUMBER).attr('disabled', false);
      getServerElementById(WorkOrderMaterialsUsedForm.ELEMENTS.STOCK_LOCATION).attr('disabled', false);
    }
  },

  ddlPartNumber_Change: function(ddlPartNumber) {
    var value = $(ddlPartNumber).val();
    if (value) {
      this.loadMaterialByID(value);
    } else {
      getServerElementById('txtNonStockDescription').show();
    }
  },
  loadMaterialByID: function(id) {
    getServerElementById('txtNonStockDescription').hide();
    $('#lblDescription').html(this.getSearchResultStringByValue(id)[1]);
  },
  getSearchResultStringByValue: function(val) {
    var asdf = $('#lbPartSearchResults option[value=' + val + ']');
    return asdf.text().split(' - ');
  }
};