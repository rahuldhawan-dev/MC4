// In order for a value to be saved to FormState:
//      - The form must have an ID.
//      - The form must have the class "has-form-state".
//      - The input/select must have either an id or a name attribute.
//      - The input/select must not have the 'no-state' class.
//      - The input must not be a password box.
//      - The input must have a non-null/empty value.


// This must be loaded after jQuery and before any jQuery plugins.
var FormState = (function ($) {

    var currentUrl = (function () {
        var url = location.href;
        // We don't want to differentiate urls based on query parameters
        // or #fragments, otherwise things would get all weird.
        url = url.split('#')[0];
        url = url.split('?')[0];
        // We also want to make sure we can handle the same url regardless
        // of whether or not it has the ending slash.
        if (url[url.length - 1] != '/') {
            url = url + '/';
        }
        return url;
    })();

    var noFormState = (function() {
      var url = location.href;

      url = url.split('?')[1] || '';
      var queryParts = url.split('&');

      for (var i = queryParts.length - 1; i >= 0; --i) {
        var curPart = queryParts[i].split('=');

        if (curPart[0].toLowerCase() === 'noformstate') {
          return curPart[1].toLowerCase() === 'true';
        }          
      }

      return false;
    })();

    var methods = {

        // Storage is always based per url, so we can tack that onto the plugin key.
        _storage: UserStorage.getPluginContainer('formState.' + currentUrl),

        // this exists for testing
        _getForms: function () {
            return $('form.has-form-state');
        },

        // Returns all the forms with .has-state-form
        getForms: function () {
            var forms = methods._getForms();
            forms.each(function () {
                methods.ensureFormIsValidForFormState($(this));
            });
            return forms;
        },

        init: function () {
            if (noFormState) {
              return;
            }
          
            var forms = methods.getForms();
            methods.loadPreviousFormValues();

            forms.each(function () {
                var $form = $(this);
                methods.initSaving($form);
            });
        },

        ensureFormIsValidForFormState: function ($form) {
            if (!$form.attr('id')) {
                throw 'A form with the has-form-state class must have an id in order to differentiate it from multiple forms on a page.';
            }
        },

        initSaving: function ($form) {
            // We really only care about saving values if a form is submitted. 
          $form.submit(methods.saveFormValues);
          $form.on('fauxreset', methods.saveFormValues);
        },

        loadPreviousFormValues: function () {
            var values = methods._storage.get('values');
            if (values) {
                for (var selector in values) {
                    methods.loadPreviousValue($(selector), values[selector]);
                }
            }
        },

        _canSaveState: function ($input) {
            if ($input.is(':password, :button, :disabled')) {
                return false;
            }
            if ($input.hasClass('no-form-state')) {
                return false;
            }
            if ($input.closest('.no-form-state').length > 0) {
                return false;
            }

            return true;
        },

        loadPreviousValue: function ($input, value) {
            if (methods._canSaveState($input)) {
                // Apparently $input.val() for checkboxes will always return "on"
                // if it doesn't have a value attribute set. Cause that makes sense.
                // NOTE: Server will have no way of overriding this functionality.
                if ($input.is(':checkbox')) {
                    $input.prop('checked', value);
                }
                else {
                    $input.val(value);
                }
            }
        },

        _getCurrentValue: function ($input) {
            // can't use val on checkboxes, it doesn't return the checked state.
            if ($input.is(':checkbox')) {
                return $input.is(':checked');
            }

            return $input.val();
        },

        _getSelector: function ($form, $input) {
            var identifier = '';
            var name = $input.attr('name');
            if (name) {
                identifier = '[name="' + name + '"]';
            } else {
                var id = $input.attr('id');
                if (id) {
                    identifier = '#' + id;
                }
            }

            if (!identifier) {
                return null;
            }
            return '#' + $form.attr('id') + ' ' + identifier;
        },

        _save: function (values) {
            if (values) {
                // This lets us overwrite all the existing values, which is nice, cause it
                // won't leave lingering values hanging out in storage taking up space.
                methods._storage.set('values', values);
            }
            else {
                // If there aren't any values, we don't wanna waste space in local storage.
                methods._storage.clearAll();
            }
        },

        saveFormValues: function () {
            var values = {};
            var hasValues = false;

            methods.getForms().each(function () {
                var $form = $(this);

                // We do not deal with passwords ever, and buttons probably never.
                $form.find(':input').each(function () {
                    var $input = $(this);
                    if (methods._canSaveState($input)) {
                        var val = methods._getCurrentValue($input);
                        if (val) {
                            // Most fields should have a name attribute otherwise they're
                            // useless for postbacks. Some fields may only have an id though,
                            // like cascading dropdowns have hidden inputs that do not postback.
                            var selector = methods._getSelector($form, $input);

                            // We don't store fields that don't have a selector. They'd
                            // be impossible to use when restoring values.
                            if (selector) {
                                values[selector] = val;
                                hasValues = true;
                            }
                        }
                    }
                });
            });

            if (hasValues) {
                methods._save(values);
            } else {
                methods._save(null);
            }
        }
    };

    methods.init();
    return methods;
})(jQuery);