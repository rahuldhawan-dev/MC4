var WorkOrderStockApprovalForm = {
  
  initialize: function() {
    $('table.WorkOrderDisplay td:even').css('font-weight', 'bold');
    if (getServerElementById('txtMaterialPostingDate').val() === '')
      getServerElementById('txtMaterialPostingDate').val(new Date().format('MM/dd/yyyy'));
  },
  validateMaterialsApproval: function() {
    var txtDocID = getServerElementById('txtDocID');
    if (this.isDocIDRequired()) {
      if (txtDocID.val() == '') {
        alert('One or more used materials has a valid Part Number.  Please provide a Doc ID value.');
        txtDocID.focus();
        return false;
      }
    }
    return true;
  },
  isDocIDRequired: function() {
    if (getServerElementById('txtMaterialPostingDate').val() === '') {
      alert('Please enter a Material Posting Date.');
      return false;
    }
  //crappy hack that determines if sap enabled or not
    if (getServerElementById('txtDocID').attr('style') === 'display: none;')
      return false;
    var tbl = getServerElementById('gvMaterialsUsed')[0];
    for (var i = tbl.rows.length - 1; i >= 1; --i) {
      if ($(tbl.rows[i].cells[0]).text().trim() != 'n/a')
        return true;
    }
    return false;
  }
};