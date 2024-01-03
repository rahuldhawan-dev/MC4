var WorkOrderInputListView = {
  popUpUrl: '../Input/WorkOrderInputRPCView.aspx?cmd=view&arg=',

  initialize: function() {
    // this is pretty nasty.  it used to just set the type to hidden
    // but IE didn't like that, so now we have to do this:
    var hidAssetID = getServerElementById('hidHistoryAssetID');
    var newHidAssetID = document.createElement('input');
    newHidAssetID.type = 'hidden';
    newHidAssetID.value = hidAssetID.val();
    newHidAssetID.name = hidAssetID[0].name;
    newHidAssetID.id = hidAssetID[0].id;
    newHidAssetID.onchange = hidAssetID[0].onchange;
    hidAssetID.replaceWith($(newHidAssetID));
  },

  gvWorkOrders_Select: function(a) {
    this.onWorkOrderSelect(a);
    return false;
  },

  onWorkOrderSelect: function(a) {
    this.openOrderForEdit(this.getOrderID(a));
  },

  openOrderForEdit: function(id) {
    window.open(this.popUpUrl + id);
  },

  getOrderID: function(a) {
    var cell = this.getParentRow(a).cells[1];
    return cell.innerHTML;
  },

  getParentRow: function(elem) {
    return (elem.tagName && elem.tagName.toLowerCase() == 'tr') ?
      elem : this.getParentRow(elem.parentNode);
  }
};
