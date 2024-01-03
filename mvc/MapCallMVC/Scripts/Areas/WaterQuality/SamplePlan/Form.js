// This radio function was originally in jquery.extensions.js which is why
// it's written this way. 
(function ($) {
    $.fn.radio = function (selector, allowUncheck) {
        var $this = $(this);

        if (!$this.filter(':checked').length && !allowUncheck) {
            $this.first().prop('checked', true);
        }

        $(this).click(function (e) {
            $(selector).not(e.target).prop('checked', false);

            if (!allowUncheck) {
                $(e.target).prop('checked', true);
            }
        });
    };
})(jQuery);

(function ($) {
  var Form = {
    radios: {
      CWS_OR_NTNC: 'input#Cws, input#Ntnc',
      STANDARD_OR_REDUCED: 'input#Standard, input#Reduced'
    },
    initialize: function () {
      for (var x in Form.radios) {
        if (Form.radios.hasOwnProperty(x)) {
          $(Form.radios[x]).radio(Form.radios[x]);
        }
      }
    }
  };

  $(document).ready(Form.initialize);
})(jQuery);
