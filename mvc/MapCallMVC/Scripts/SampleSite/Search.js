var Search = {  
  initialize: function() {
    Search.OutOfServiceArea_change({ target: $('#OutOfServiceArea').change(Search.OutOfServiceArea_change) });
  },

  toggleThing: function(jq, show) {
    $($($(jq.parent()).parent()).parent()).toggle(show);
  },

  OutOfServiceArea_change: function (e) {
    var val = $(e.target)[0].checked;

    Search.toggleThing($('#TownText'), !!val);
    Search.toggleThing($('#Town'), !val);
  }
};

$(document).ready(Search.initialize);