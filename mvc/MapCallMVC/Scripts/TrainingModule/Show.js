var TrainingModuleShow = {
  selectors: {
    frmRemovePositionGroupCommonName: 'form#frmRemovePositionGroupCommonName'
  },
  initialize: function () {
    $(TrainingModuleShow.selectors.frmRemovePositionGroupCommonName).submit(TrainingModuleShow.frmRemovePositionGroupCommonName_Submit);
  },

  frmRemovePositionGroupCommonName_Submit: function() {
    return confirm('Are you sure you wish to remove the chosen Position Group Common Name from this Training Module?');
  }
};

$(document).ready(TrainingModuleShow.initialize);