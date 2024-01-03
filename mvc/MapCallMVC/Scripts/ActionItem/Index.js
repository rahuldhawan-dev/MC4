var ActionItemIndex = {
  messages: {
    confirmDelete: 'This will delete the action item permanently.  Continue?'
  },

  selectors: {
    actionItemsTableRows: '#actionItemsTable tbody tr',
    pnlNewActionItem: '#pnlNewActionItem',
    editActionItemButton: 'button.editActionItem',
    updateActionItemButton: 'button.updateActionItem',
    cancelActionItemButton: 'button.cancelActionItem',
    deleteActionItemButton: 'button.deleteActionItem',
    actionItemDisplay: 'span.actionItemDisplay',
    actionItemEdit: 'span.actionItemEdit',
    editDeleteActionItem: 'span.editDeleteActionItem',
    updateCancelActionItem: 'span.updateCancelActionItem',
    noteTextBox: 'textarea#Text'
  },

  initialize: function() {
    ActionItemIndex.initEvents();
  },

  initEvents: function() {
    $(ActionItemIndex.selectors.editActionItemButton).click(ActionItemIndex.editActionItemButton_click);
    $(ActionItemIndex.selectors.updateActionItemButton).click(ActionItemIndex.updateActionItemButton_click);
    $(ActionItemIndex.selectors.cancelActionItemButton).click(ActionItemIndex.cancelActionItemButton_click);
    $(ActionItemIndex.selectors.deleteActionItemButton).click(ActionItemIndex.deleteActionItemButton_click);
    $(ActionItemIndex.selectors.pnlNewActionItem).on('open', ActionItemIndex.pnlNewActionItem_open);
  },

  pnlNewActionItem_open: function () {
    $(ActionItemIndex.selectors.noteTextBox).focus();
  },

  deleteActionItemButton_click: function(e) {
    return confirm(ActionItemIndex.messages.confirmDelete);
  },

  updateActionItemButton_click: function (e) {
    $('form#updateActionItem' + e.target.value).submit();
  },

  cancelActionItemButton_click: function(e) {
    ActionItemIndex.toggleDisplayRow(ActionItemIndex.getParentRow(e.target));
  },

  toggleEditRow: function(row) {
    var display = $(ActionItemIndex.selectors.actionItemDisplay, row);
    var cellHeight = display.closest('td').height();

    display.hide();
    $(ActionItemIndex.selectors.editDeleteActionItem, row).hide();
    $(ActionItemIndex.selectors.updateCancelActionItem, row).show();

    $('textarea', row)
      .removeAttr('disabled')
      .height(cellHeight);
  },

  toggleDisplayRow: function(row) {
    $(ActionItemIndex.selectors.updateCancelActionItem, row).hide();
    $(ActionItemIndex.selectors.actionItemDisplay, row).show();
    $(ActionItemIndex.selectors.editDeleteActionItem, row).show();
    $('textarea', row).attr('disabled', 'disabled');
  },

  getParentRow: function(elem) {
    return $(elem).closest('tr');
  }
};

$(document).ready(ActionItemIndex.initialize);
