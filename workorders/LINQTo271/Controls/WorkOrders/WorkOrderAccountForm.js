var WorkOrderAccountForm = {
  initialize: function() {
    $('#tblInitialInformation td:even').css('font-weight', 'bold');

    //For the reject dialog:
	  var rejectWindow = getServerElementById('modalWindow');
	  var rejectButton = getServerElementById('btnReject');
    rejectWindow.jqm({ trigger: rejectButton, modal: true, toTop: true });

    this.initializeBusinessUnit();
  },
	initializeBusinessUnit: function() {
    if (getServerElement('lblAccountingType').html() == 'O&amp;M') {
      //show it
      this.toggleBusinessUnit(true);

      //set it from the hidden value
      if (getServerElement('hidBusinessUnit').val() == "") {
        getServerElement('ddlBusinessUnit')[0].selectedIndex = 1;
        getServerElement('hidBusinessUnit').val(getServerElement('ddlBusinessUnit').val());
      }
      else {
        getServerElement('ddlBusinessUnit').val(getServerElement('hidBusinessUnit').val());
      }
    }
    else {
      this.setBusinessUnit('');
    }
  },
  btnSave_Click: function() {
  	var txtAccountCharged = getServerElementById('txtAccountCharged');
  	if (txtAccountCharged && txtAccountCharged.val().length === 0) {
  		getServerElementById('lblAccountChargedRequired').show();
  		txtAccountCharged.focus();
  		return false;
  	}
  	getServerElementById('lblAccountChargedRequired').hide();
    if (txtAccountCharged && txtAccountCharged.val().length > 30) {
      alert('Account Charged must be 30-characters or less.');
      txtAccountCharged.focus();
      return false;
    }
    if (getServerElement('ddlBusinessUnit').is(':visible') && 
          getServerElement('hidBusinessUnit').val() == '') {
      alert('Please select the business unit.');
      getServerElement('ddlBusinessUnit').focus();
      return false;
    }
    var ddlRequiresInvoice = getServerElementById('ddlRequiresInvoice');
    if (typeof ddlRequiresInvoice !== 'undefined' && ddlRequiresInvoice.val() === "") {
	    alert('Please select if an invoice is required or not.');
		  return false;
    }
    // are there labor items enabled? if so we need at least one
	  if (typeof WorkOrderScheduleOfValuesForm !== 'undefined' && !WorkOrderSupervisorApprovalDetailView.validateScheduleOfValues()) {
	  	return false;
	  }
	  return true;
  },
  btnNotesSubmit_Click: function() {
    var notes = getServerElementById('txtRejectionNotes');
    //confirm they entered something
    if (notes.val().length < 10) {
      alert('You must enter a reason for rejection');
      return false;
    }
  },
  ddlBusinessUnit_Change: function() {
    this.setBusinessUnit(getServerElementById('ddlBusinessUnit').val());
  },
  setBusinessUnit: function(value) {
    getServerElementById('hidBusinessUnit').val(value);
  },
  toggleBusinessUnit: function(show) {
    getServerElementById('trBusinessUnit')[(show) ? 'show' : 'hide']();
  }
};