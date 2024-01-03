var NoteIndex = {
  messages: {
    confirmDelete: 'This will delete the note permanently.  Continue?'
  },

  selectors: {
    notesTableRows: '#notesTable tbody tr',
    pnlNewNote: '#pnlNewNote',
    editNoteButton: 'button.editNote',
    updateNoteButton: 'button.updateNote',
    cancelEditNoteButton: 'button.cancelEditNote',
    deleteNoteButton: 'button.deleteNote',
    noteDisplay: 'span.noteDisplay',
    noteEdit: 'span.noteEdit',
    editDelete: 'span.editDelete',
    updateCancel: 'span.updateCancel',
    noteTextBox: 'textarea#Text'
  },

  initialize: function() {
    NoteIndex.initEvents();
  },

  initEvents: function() {
    $(NoteIndex.selectors.editNoteButton).click(NoteIndex.editNoteButton_click);
    $(NoteIndex.selectors.updateNoteButton).click(NoteIndex.updateNoteButton_click);
    $(NoteIndex.selectors.cancelEditNoteButton).click(NoteIndex.cancelEditNoteButton_click);
    $(NoteIndex.selectors.deleteNoteButton).click(NoteIndex.deleteNoteButton_click);
    $(NoteIndex.selectors.pnlNewNote).on('open', NoteIndex.pnlNewNote_open);
  },

  pnlNewNote_open: function () {
    $(NoteIndex.selectors.noteTextBox).focus();
  },

  editNoteButton_click: function(e) {
    $(NoteIndex.selectors.notesTableRows).each(function(i, e) {
      NoteIndex.toggleDisplayRow(e);
    });
    NoteIndex.toggleEditRow(NoteIndex.getParentRow(e.target));
  },

  deleteNoteButton_click: function(e) {
    return confirm(NoteIndex.messages.confirmDelete);
  },

  updateNoteButton_click: function (e) {
    $('form#updateNote' + e.target.value).submit();
  },

  cancelEditNoteButton_click: function(e) {
    NoteIndex.toggleDisplayRow(NoteIndex.getParentRow(e.target));
  },

  toggleEditRow: function(row) {
    var display = $(NoteIndex.selectors.noteDisplay, row);
    var cellHeight = display.closest('td').height();

    display.hide();
    $(NoteIndex.selectors.editDelete, row).hide();
    $(NoteIndex.selectors.noteEdit, row).show();
    $(NoteIndex.selectors.updateCancel, row).show();

    $('textarea', row)
      .removeAttr('disabled')
      .height(cellHeight);
  },

  toggleDisplayRow: function(row) {
    $(NoteIndex.selectors.noteEdit, row).hide();
    $(NoteIndex.selectors.updateCancel, row).hide();
    $(NoteIndex.selectors.noteDisplay, row).show();
    $(NoteIndex.selectors.editDelete, row).show();
    $('textarea', row).attr('disabled', 'disabled');
  },

  getParentRow: function(elem) {
    return $(elem).closest('tr');
  }
};

$(document).ready(NoteIndex.initialize);
