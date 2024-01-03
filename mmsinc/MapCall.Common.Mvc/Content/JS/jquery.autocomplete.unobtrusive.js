// Ross made this!
(function ($) {

    var DATA_KEY = 'unobtrusive autocomplete key';

    var MvcAutocomplete = function ($textBox, options) {
        if (!options.httpMethod) {
            options.httpMethod = 'GET';
        }
        this.options = options;
        this.textBox = $textBox;
        // This needs to be done by name rather than id. This is the simplest
        // fix for dealing with using these in templates that need to be used
        // with model binding lists. If the id has a bracket in it, the id breaks.
        // This would be better handled by converting this to a web component where
        // we have control over the generated html client-side rather than having to
        // look up pre-rendered elements.
        this.hidden = $(`input[name="${options.dependent}"]`);
        this.initialize();
    };
    
    MvcAutocomplete.prototype = {
        options: null,
        textBox: null,
        hidden: null,

        initialize: function () {
            var self = this;
            self.textBox.autocomplete({
                delay: 300,
                source: function (req, resp) {
                    return self.getSource(req, resp);
                },
                select: function (event, data) {
                    return self.select(data);
                },
                change: function (event, data) {
                    // NOTE: The change event is called on blur of the visible input, not
                    // the autocomplete menu. It's also called when the autocomplete's
                    // searchTimeout runs. This can cause weird timing issues in the functional tests.
                    return self.select(data);
                }
            });

            // add an event listener to the parent to reset this and its hidden values when it's changed
            $(self.options.dependsOn).on('change', function () {
                self.textBox.val('');
                self.hidden.val('');
                self.hidden.change();
            });
        },

        getSource: function (searchReq, sourceCallback) {
            var options = this.options;
            var dependsOn = $(options.dependsOn).val();
            var actionParams = options.actionParam.split(",");
            var ajaxArgs = {
                async: false,
                error: function() {
                    sourceCallback([
                        {
                            label: 'An error has occured while retrieving the autocomplete list.'
                        }
                    ]);
                },
                success: function(data) {
                    sourceCallback(data);
                },
                // needs to be true so the data gets serialized in a way the mvc modelbinder
                // is expecting it. Specifically needed for autocomplete cascades that have multi-value parents.
                // The querystring gets formatted wrong without traditional = true. It'll come up as ?prop[]=1 instead of ?prop=1.
                traditional: true,
                type: options.httpMethod,
                url: options.actionPath
            };
            ajaxArgs.data = {};
            ajaxArgs.data[actionParams[0]] = searchReq.term;
            ajaxArgs.data[actionParams[1]] = dependsOn;

            if (actionParams[1] != "" && dependsOn == "") { }

            else {
              $.ajax(ajaxArgs);
            }
        },

        select: function (data) {
            var selected = (data && data.item ? data.item.data : null);
            this.hidden.val(selected);
            // Client side validation for this.
            this.hidden.change();
            this.hidden.valid();
        }
    };

    var unobtrusiveAutocomplete = {
        initialize: function () {
            $('.autocomplete').each(unobtrusiveAutocomplete.initializeAutocomplete);
            $(document).on('focus', '.autocomplete', unobtrusiveAutocomplete.initializeAutocomplete);
        },

        initializeAutocomplete: function () {
            var $textBox = $(this);
            if (!$textBox.data(DATA_KEY)) {
                var getAttr = function (attrSuffix) {
                    return $textBox.attr('data-autocomplete-' + attrSuffix);
                };

                var options = {
                    'actionPath': getAttr('action'),
                    'actionParam': getAttr('actionparam'),
                    'dependent': getAttr('dependent'),
                    'dependsOn': getAttr('dependson'),
                    'httpMethod': getAttr('httpmethod'),
                    'placeHolder':getAttr('placeholder')
                };

                var mvcAuto = new MvcAutocomplete($textBox, options);
                $textBox.data(DATA_KEY, mvcAuto);
            }
        }
    };

    $(document).ready(unobtrusiveAutocomplete.initialize);
})(jQuery);