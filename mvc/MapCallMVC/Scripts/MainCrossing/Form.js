var MainCrossing = (function($) {

  var mc = {
    init: function() {
      $('#CrossingCategory').change(mc.onCrossingCategoryChanged);

      // set initial visibility
      mc.onCrossingCategoryChanged();
    },

    onCrossingCategoryChanged: function() {
      var isRailroad = $('#CrossingCategory option:selected').text() === "Railroad";
      var visibleForRailroads = $('.visible-for-railroad');
      if (isRailroad) {
        visibleForRailroads.show();
      } else {
        visibleForRailroads.hide();
      }
    }
  };

  $(document).ready(mc.init);

  return mc;
})(jQuery);