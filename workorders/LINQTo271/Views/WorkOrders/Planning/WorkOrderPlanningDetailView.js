var WorkOrderPlanningDetailView = {
  initialize: function() {
    $('#tblInitialInformation td:even').css('font-weight', 'bold');

    // this tells the update panel to re-init the initial form
    // after it's been posted back
// 20090807 by jduncan: no need for the update panel right now anyway
//    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(this.handleUpdatePanelCallback);
//  },

//  handleUpdatePanelCallback: function() {
//    return (WorkOrderInputFormView.isReadOnly()) ?
//      WorkOrderInputFormView.initializeEdit() :
//      WorkOrderInputFormView.initializeReadOnly();
  }
};