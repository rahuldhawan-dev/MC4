(function ($) {
  var openClass = 'open';
  var headerSelector = 'div.map-legend-header';
  var sectionSelector = 'div.map-legend-section';
  var openHeaderSelector = headerSelector + '.' + openClass;
  var topMargin = 6;
  // leave room for ESRI logo
  var bottomMargin = 42;
  var sectionHeight = 37;

  var headerClick = function(e) {
    var $current = $(openHeaderSelector);
    if (e.target != $current[0]) {
      $current.toggleClass(openClass)
        .next(sectionSelector).toggle();
    }

    $(e.target).toggleClass(openClass)
      .next(sectionSelector).toggle();
  };

  var fixSectionMaxHeight = function() {
    var margins = topMargin + bottomMargin;
    var sections = sectionHeight * $(headerSelector).length;
    var height = $(window).height() - margins - sections;
    $(sectionSelector).css('max-height', height + 'px');
  };

  $(document).ready(function() {
    $(headerSelector).click(headerClick);
    $(window).resize(fixSectionMaxHeight);
    fixSectionMaxHeight();
  });
})(jQuery);