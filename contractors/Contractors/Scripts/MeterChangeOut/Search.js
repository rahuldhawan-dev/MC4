var Search = {
  initialize: function() {
    $('body').keypress(function (e) {
      if (e.which === 82 && e.shiftKey === true && e.target.localName === 'body')
        $('button[class=reset]').click();
      if (e.which === 83 && e.shiftKey === true && e.target.localName === 'body')
        $('button[id=Search]').click();
    });
  }
};

$(document).ready(Search.initialize);