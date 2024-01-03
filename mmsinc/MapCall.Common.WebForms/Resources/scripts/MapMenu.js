var MapMenu = (function () {
  var openClass = 'open';
  var headerSelector = 'div.map_menu_header';
  var sectionSelector = 'div.map_menu_section';
  var openHeaderSelector = headerSelector + '.' + openClass;
  var topMargin = 6;
  // leave room for ESRI logo
  var bottomMargin = 62;
  var sectionHeight = 26;

  var headerClick = function(e) {
    var $current = jQuery(openHeaderSelector);
    if (e.target != $current[0]) {
      $current.toggleClass(openClass)
        .next(sectionSelector).toggle();
    }

    jQuery(e.target).toggleClass(openClass)
      .next(sectionSelector).toggle();
  };

  var fixSectionMaxHeight = function() {
    var margins = topMargin + bottomMargin;
    var sections = sectionHeight * jQuery(headerSelector).length;
    var height = jQuery(window).height() - margins - sections;
    jQuery(sectionSelector).css('max-height', height + 'px');
  };

  return {
    initialize: function() {
      jQuery(document).ready(function() {
        jQuery(headerSelector).click(headerClick);
        jQuery(window).resize(fixSectionMaxHeight);
        fixSectionMaxHeight();
      });
    }
  };
})();
