/////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////INPUT/EDIT//////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////

var WorkOrderInputDetailView = {
  initialize: function() {
    // make tabstrip

    getServerElementById('ddlOperatingCenter').focus();
    if (!this.isInEditMode()) {
      // patch into the validation
      var oldValidate = WorkOrderInputFormView.validateForm, that = this;
      WorkOrderInputFormView.validateForm = function() {
        return oldValidate.call(WorkOrderInputFormView) && that.allowNewWorkOrder();
      }
    }
  },
  //////////////////////////////EVENT HANDLERS//////////////////////////////

  btnEdit_Click: function() {
    return this.performEditAction();
  },

  ////////////////////////////EVENT PASSTHROUGHS////////////////////////////
  performEditAction: function() {
    if (isRPCPage()) {
      window.top.location = window.top.location.toString().replace('cmd=view', 'cmd=update');
      return false;
    }
    return true;
  },

  ////////////////////////////////UI HELPERS////////////////////////////////
  isInEditMode: function() {
    return getServerElementById('btnEdit').length > 0;
  },

  ////////////////////////////////VALIDATION////////////////////////////////
  allowNewWorkOrder: function() {
    //TODO: make sure it isnt in edit mode.
    if (getServerElementById('hidAssetHasOpenWorkOrders').val() == 'True') {
      return confirm('There is an existing order for this asset which has not been completed.  Are you sure you want to create a new order?');
    }
    return true;
  }
};