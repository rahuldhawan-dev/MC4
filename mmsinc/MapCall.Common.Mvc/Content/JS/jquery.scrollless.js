// Dumb little plugin for making a scrollable div without having a scrollbar.
// Makes use of this scroll wheel plugin http://www.ogonek.net/mousewheel/

(function() {
  var emptyFn = function() { };

  $.fn.extend({
    mousewheel: function(up, down, preventDefault) {
      return this.hover(
          function() {
            $.event.mousewheel.giveFocus(this, up, down, preventDefault);
          },
          function() {
            $.event.mousewheel.removeFocus(this);
          }
      );
    },
    mousewheeldown: function(fn, preventDefault) {
      return this.mousewheel(emptyFn, fn, preventDefault);
    },
    mousewheelup: function(fn, preventDefault) {
      return this.mousewheel(fn, emptyFn, preventDefault);
    },
    unmousewheel: function() {
      return this.each(function() {
        //    $(this).unmouseover().unmouseout();
        $.event.mousewheel.removeFocus(this);
      });
    },
    unmousewheeldown: $.fn.unmousewheel,
    unmousewheelup: $.fn.unmousewheel
  });


  $.event.mousewheel = {
    giveFocus: function(el, up, down, preventDefault) {
      if (el._handleMousewheel) $(el).unmousewheel();

      if (preventDefault == window.undefined && down && down.constructor != Function) {
        preventDefault = down;
        down = null;
      }

      el._handleMousewheel = function(event) {
        if (!event) event = window.event;
        if (preventDefault)
          if (event.preventDefault) event.preventDefault();
          else event.returnValue = false;
        var delta = 0;
        if (event.wheelDelta) {
          delta = event.wheelDelta / 120;
          if (window.opera) delta = -delta;
        } else if (event.detail) {
          delta = -event.detail / 3;
        }
        if (up && (delta > 0 || !down))
          up.apply(el, [event, delta]);
        else if (down && delta < 0)
          down.apply(el, [event, delta]);
      };

      if (window.addEventListener)
        window.addEventListener('DOMMouseScroll', el._handleMousewheel, false);
      window.onmousewheel = document.onmousewheel = el._handleMousewheel;
    },

    removeFocus: function(el) {
      if (!el._handleMousewheel) return;

      if (window.removeEventListener)
        window.removeEventListener('DOMMouseScroll', el._handleMousewheel, false);
      window.onmousewheel = document.onmousewheel = null;
      el._handleMousewheel = null;
    }
  };
})(jQuery);

(function($) {
  // These don't actually matter, just for documentation atm.
  var defaults = {
    'dataKey': 'scrollless',
    'scrollChild': null, // Selector for the inner child that's scrolled.
    'scrollDiff': 40,
    'preventDocumentScroll': true
  };

  var methods = {
    'init': function($el, options) {
      // TODO: Correctly merge options with defaults.

      if ($el.data(defaults.dataKey)) {
        // Already initialized.
        return;
      }

      // We need to disable scrollless on touch screen devices because it doesn't
      // work. Also touchscreen scrollbars look fine. Not sure how this will work 
      // if the computer is both mouse and touchscreen enabled.
      var isTouchDevice = function() {
        return (('ontouchstart' in window)
          || (navigator.MaxTouchPoints > 0)
          || (navigator.msMaxTouchPoints > 0));
      };

      if (isTouchDevice()) {
        $el.css('overflow', 'auto');
        return;
      }
      // Otherwise the scrollChild will spill out
      // of the container.
      $el.css('overflow', 'hidden');
      // Need to set tabindex in order for keyboard controls to work.
      $el.attr('tabindex', '-1');
      var scrollChild = $el.find(options.scrollChild);
      if (scrollChild.css('position') === 'static') {
        // Position needs to be non-static in order to scroll.
        scrollChild.css('position', 'relative');
      }

      var handler = function(isScrollDown) {
        var $this = $el;
        var containerHeight = $this.innerHeight();
        var scrollHeight = scrollChild.outerHeight(true);

        // This makes up for the weird lingering issue where the very bottom won't
        // scroll into view.
        var scrollDiff = (scrollChild.outerHeight() - scrollChild.height());
        scrollHeight += scrollDiff;

        var range = (scrollHeight - containerHeight) * -1;

        // IE likes to set top to 'auto' for some reason.
        var top = parseInt(scrollChild.css('top')) || 0;
        if (range < 0) {
          top = top + (!isScrollDown ? 40 : -40);
        }
        //                else {
        //                   // This means our inner scroll is smaller
        //                    // than the container, so there's nothing to 
        //                    // scroll.
        //                   // return;
        //                }

        if (top >= 0) {
          top = 0;
        } else if (top < range) {
          top = range;
          if (top >= 0) {
            top = 0;
          }
        }
        scrollChild.css('top', top);
      };

      $el.mousewheel(function(event, delta) {
        if (defaults.preventDocumentScroll) {
          if (event.preventDefault) {
            event.preventDefault();
          } else {
            // apparently event.preventDefault doesn't work
            // in IE7 so we have to use this stupid hack.
            event.returnValue = false;
          }
        }
        var isScrollDown = (delta < 0);
        return handler(isScrollDown);
      });

      // TODO: See if there's a way to get how many pixels
      // something scrolls when the page down/up keys are hit
      // and then use that instead of the default value. It'd 
      // make it consistent with what users would normally
      // experience in browsers.
      $el.keydown(function(event) {
        // Don't want it paging up/down if the 
        // if the current focused element is a dropdown
        // or something.
        if (event.target === $el[0] || isValidInIE(event)) {
          if (defaults.preventDocumentScroll) {
            event.preventDefault();
          }
          if (event.which === 33 /* page up */ || event.which === 38 /* up arrow */) {
            handler(false);
          }
          else if (event.which === 34 /* page down */ || event.which === 40 /* down arrow */) {
            handler(true);
          }
        }
      });

      var isValidInIE = function(event) {
        if (event.currentTarget === $el[0]) {
          var $target = $(event.target);
          return !($target.is('input') || $target.is('textarea') || $target.is('select'));
        }
        return false;
      };

      $el.data(defaults.dataKey, true);
    },
    'destroy': function($el) {
      $el.unmousewheel();
      $el.unbind('keydown');
      $el.data(defaults.dataKey, null);
    }
  };

  $.fn.scrollless = function(options) {
    $(this).each(function() {
      methods.init($(this), options);
    });
  };

  $.fn.unscrollless = function() {
    $(this).each(function() {
      methods.destroy($(this));
    });
  };

})(jQuery);