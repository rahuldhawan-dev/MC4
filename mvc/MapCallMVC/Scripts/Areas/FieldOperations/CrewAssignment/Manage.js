var Manage = {
  'initialize': function () {
    $('#assignmentsTable').tableDnD({
      'onDragClass': 'dragged',
      'onDrop': Manage.setPriorities
    });
  },
  'onRemoveSuccess': function () {
    // Server returns a 204 No Content on success, so we need to reload the page.
    window.location.reload(false);
  },
  'onRemoveError': function () {
    alert('There was a problem removing this crew assignment. Please reload the page and try again.');
  },
  'setPriorities': function (table, row) {
    $(table).find('tbody tr').each(Manage.setPriority);
  },
  'setPriority': function (i, row) {
    $(row).find('td').eq(2).html(i + 1);
  }
};

$(document).ready(Manage.initialize);