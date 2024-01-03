var SystemDeliveryEntryForm = (($) => {
  var ELEMENTS = {};
  const sdef = {
    intialize: function() {
      ELEMENTS = {
        distribute: $('#distribute'),
        entryTable: $('#entryTable'),
        weeklyTotal: $('#WeeklyTotal'),
        newEntries: document.getElementById('entries-list')
      };

      ELEMENTS.distribute.on('click', sdef.onDistributeButtonClicked);
    },

    onDistributeButtonClicked: function() {
      $('#entryTable tr#entries').each(function() {        
        let weeklyTotal = $(this).find('td.weeklyTotal input').val();

        if (weeklyTotal.trim()) { 
            let weeklyEntries = $(this).find('td:not(td.weeklyTotal) input');
            let distribute = weeklyTotal / 7;

            $(weeklyEntries).each(function () {
                $(this).val(distribute.toFixed(5));
            });
        }
      });
    }
  }

  sdef.ELEMENTS = ELEMENTS;
  $(document).ready(sdef.intialize);
  return sdef;
})(jQuery);