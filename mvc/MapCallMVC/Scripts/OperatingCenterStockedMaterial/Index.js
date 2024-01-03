var OperatingCenterStockedMaterialIndex = {
  selectors: {
    frm: 'form#DeleteOperatingCenterStockedMaterialForm'
  },

  initialize: function() {
    $(OperatingCenterStockedMaterialIndex.selectors.frm).submit(OperatingCenterStockedMaterialIndex.deleteForm_Submit);
  },

  deleteForm_Submit: function() {
    return confirm('Are you sure you wish to delete the chosen Operating Center Stocked Material?');
  }
};

$(document).ready(OperatingCenterStockedMaterialIndex.initialize);