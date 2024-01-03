// Numeric TextBox jQuery plugin
// By Ross Dickinson
// So if it screws up let him know.

// TODO: This doesn't support negative numbers.
// TODO: Paste support. 
(function($) {
    var keys = {
        MINUS: 109,
        MINUSIE7: 189
    };

    var methods = {
        initialize: function() {
            $("input[rel^='number']").numericTextbox();
        },
        intHandler: function(e, el) {
            // el is passed in by the decHandler, because doing
            // $(this) in here when decHandler calls it doesn't work.

            // This enables copy/pasting, however it also allows
            // pasting of non-numeric info. That'll need to be fixed.
            if (e.ctrlKey) { return true; }
            if (e.shiftKey) { return false; }

            var key = e.charCode || e.keyCode || 0;

            // Allow for negatives
            if (key == keys.MINUS || key == keys.MINUSIE7) {
                if (!el) { el = $(this); }
                var val = el.val();
                if (val == "") { return true; }
                else if (val.indexOf("-") >= 0) { return false; }
                return false;
            }

            return ((key == 8 || key == 17 || key == 9 || key == 46 ||
                (key >= 37 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105)));
        },
        decHandler: function(e) {
            if (methods.intHandler(e, $(this))) { return true; }
            var key = e.charCode || e.keyCode || 0;
            var hasDecimalAlready = ($(this).val().indexOf(".") >= 0);

            // 110 and 190 being both decimal keys
            return ((key == 110 || key == 190) && !hasDecimalAlready);
        },

        getHandler: function(t) {
            var rel = t.attr("rel");
            if (rel.indexOf("integer") >= 0) {
                return methods.intHandler;
            }
            else if (rel.indexOf("decimal") >= 0) {
                return methods.decHandler;
            }
        },

        isListening: function(el) {
            var isit = el.attr("islistening");
            return (isit === "true");
        },
        setListening: function(el, value) {
            el.attr("islistening", value);
        },
        create: function() {
            var t = $(this);
            if (!methods.isListening(t)) {
                methods.setListening(t, true);
                t.get()[0].dispose = methods.dispose; // UpdatePanels need it.
                t.keydown(methods.getHandler(t));
            }
        },
        // This function's for supporting UpdatePanels.
        dispose: function() {
            var t = $(this);
            t.unbind("keydown", methods.getHandler(t));
            methods.setListening(t, false);
        },
        destroy: function() {
            return this.each(dispose);
        }
    };

    $.fn.numericTextbox = function(opts) {
        if (opts === "destroy") {
            return this.each(methods.dispose);
        }
        else {
            return this.each(methods.create);
        }

    };

    $(document).ready(function() {
        methods.initialize();

        try {
            // This is for UpdatePanel support. 
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(methods.initialize);

        }

        catch (e) { } // Do nothing, no UpdatePanels on page 
    });

})(jQuery);

