/// <reference path="jquery-1.7.1-vsdoc.js" />
var Main = {
  LOOKUPS: {
    oddTableRow: 'table tbody tr:nth-child(odd)',
    evenTableRow: 'table tbody tr:nth-child(even)'
  },

  CSS_CLASSES: {
    oddTableRow: 'odd',
    evenTableRow: 'even'
  },

  displayUserError: function (msg) {
    var error = $('.error');
    if (error.length == 0) {
      $('.content').prepend($('<div class="error">' + msg + '</div>'));
    } else {
      error.html(msg);
    }
  },

  initialize: function () {
    Main.initTableStyles();
  },

  initTableStyles: function() {
    $(Main.LOOKUPS.evenTableRow + ',' + Main.LOOKUPS.oddTableRow)
      .removeClass(Main.CSS_CLASSES.oddTableRow)
      .removeClass(Main.CSS_CLASSES.evenTableRow);
    $(Main.LOOKUPS.oddTableRow).addClass(Main.CSS_CLASSES.oddTableRow);
    $(Main.LOOKUPS.evenTableRow).addClass(Main.CSS_CLASSES.evenTableRow);
  }
};

$(document).ready(Main.initialize);
$(document).ajaxComplete(Main.initialize);