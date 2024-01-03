(function($) {
  var getPaginationLinkClickHandler = function($this) {
    return function(e) {
      $this.load($(e.target).data('url'), null, $this.ajaxifyTable);
    };
  };

  var ajaxifyTable = function () {
    $(this).each(function() {
      var $this = $(this);
      $('th.sortable > a', $this).add('a.paginationLink', $this)
        .click(getPaginationLinkClickHandler($this))
        .each(function (i, l) {
          l = $(l);
          l.data('url', l.attr('href'));
          l.attr('href', '#');
        });
    });
  };

  $.fn.ajaxifyTable = ajaxifyTable;
})(jQuery);