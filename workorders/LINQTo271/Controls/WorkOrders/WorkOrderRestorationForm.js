var WorkOrderRestorationForm = {
  gvRestorations_Select: function(a) {
    return this.createViewRestorationPopUp(this.getWorkOrderIdFromAnchorInRow(a));
  },

  gvRestorations_Print: function(a) {
    return this.createPrintRestorationPopUp(this.getWorkOrderIdFromAnchorInRow(a));
  },

  getWorkOrderIdFromAnchorInRow: function(a) {
    var row = getParentRow(a);
    return row.cells[(row.cells.length == 8) ? 1 : 0].getElementsByTagName('input')[0].value;
  },

  lbInsertRestoration_Click: function() {
    return this.createNewRestorationPopUp();
  },

  createViewRestorationPopUp: function(id) {
    window.open('../../Restorations/RestorationRPCPage.aspx?cmd=view&arg=' + id.toString());
    return false;
  },

  createNewRestorationPopUp: function() {
    window.open('/Modules/Mvc/FieldOperations/Restoration/New/' + getServerElementById('lblWorkOrderID').text());
    return false;
  },

  createPrintRestorationPopUp: function(id) {
    window.open('../../Restorations/RestorationReadOnlyRPCPage.aspx?cmd=view&arg=' + id.toString());
    return false;
  }
};
