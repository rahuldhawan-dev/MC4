(function ($) {
  var selectors = {
    isInvalid: '#IsInvalid',
    row: 'div.reason-row'
  };

  var toggle = function(show) {
    $(selectors.row).toggle(show);
  };

  var isInvalid_change = function() {
    toggle(this.checked);
  };

  var init = function () {
    var chk = $(selectors.isInvalid);
    chk.change(isInvalid_change);
    toggle(chk.is(':checked'));
  };

  $(init);
})(jQuery);