var EntityLookups = {
  initialize: function() {
    $('table#lookups tbody tr').each(function(i, tr) {
      $('td:last', tr).text($('a[href="' + $('td a', tr).attr('href') + '"]').length > 1 ? 'yes' : 'no');
      var cells = $('td', tr);
      //if (cells[1].innerText == cells[2].innerText)
      //{
      //  $(tr).css('outline', '1px solid red');
      //}
    });
  }
};

$(document).ready(EntityLookups.initialize);
