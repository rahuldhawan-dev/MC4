var EmployeeLinkIndex = {
  messages: {
    confirmDelete: 'Are you sure?'
  },

  selectors: {
    deleteEmployeeLinkButton: 'button.deleteEmployee'
  },
  
  initialize: function() {
    $(EmployeeLinkIndex.selectors.deleteEmployeeLinkButton).click(EmployeeLinkIndex.deleteEmployeeLinkButton_click);
  },

  deleteEmployeeLinkButton_click: function(e) {
      return confirm(EmployeeLinkIndex.messages.confirmDelete);
  }
};

$(document).ready(EmployeeLinkIndex.initialize);