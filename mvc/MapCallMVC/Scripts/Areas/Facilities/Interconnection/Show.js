var InterconnectionShow = {
  selectors: {
    addMeterPanel: 'div#addMeterPanel',
    btnAddMeter: '#btnAddMeter',
    frmRemoveMeter: 'form#frmRemoveMeter'
  },

  initialize: function() {
    $(InterconnectionShow.selectors.btnAddMeter).click(InterconnectionShow.btnAddMeter_Click);
    $(InterconnectionShow.selectors.btnCancelAddMeter).click(InterconnectionShow.btnCancelAddMeter_Click);
    $(InterconnectionShow.selectors.frmRemoveMeter).submit(InterconnectionShow.frmRemoveMeter_Submit);
  },

  btnAddMeter_Click: function() {
    $(InterconnectionShow.selectors.addMeterPanel).show();
  },

  btnCancelAddMeter_Click: function() {
    $(InterconnectionShow.selectors.addMeterPanel).hide();
  },

  frmRemoveMeter_Submit: function() {
    return confirm('Are you sure you wish to remove the chosen meter from this interconnection?');
  }
};

$(document).ready(InterconnectionShow.initialize);