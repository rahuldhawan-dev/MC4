// NOTE: This script needs to work for both DateRange.cshtlm and NumericRange.cshtml
(function($) {
  var methods = {
    init: function($el) {
      if ($el.attr('range-init')) {
        return;
      }
      $el.attr('range-init', 'true');

      var data = {
        start: $el.find('.range-start'),
        end: $el.find('.range-end'),
        operator: $el.find('.range-operator')
      };
      $el.data('range', data);

      data.operator.on('change', function() {
        methods.change($el);
      });
      methods.change($el);
    },
    change: function($el) {
      var data = $el.data('range');
      
      // DatePicker has the calendar button that needs
      // to be toggled on and off as well as the textbox.
      // If this button isn't directly next to the the textbox
      // then it should be ignored.
      var triggerSib = data.start.next();
      var toggleSib = triggerSib.is(':button');
      if (data.operator.val() != 0) {
        data.start.val('');
        data.start.hide();
        if (toggleSib) {
          triggerSib.hide();
        } 
      } else {
        data.start.show();
        if (toggleSib) {
          triggerSib.show();
        }
      }
    },
    reset: function($el) {
      var data = $el.data('range');
      data.start.val('');
      data.start.show();
      data.end.val('');
      data.operator.val('0');
    }
  };
  $.fn.unobtrusiveRangePicker = function(options) {
    if (!options) {
      $(this).each(function() {
        methods.init($(this));
      });
    }
    else if (options === 'reset') {
      $(this).each(function() {
        methods.reset($(this));
      });
    }
  };
  $(document).ready(function() {
    $('.range').unobtrusiveRangePicker();
  });
})(jQuery);