// Unobtrusive datepicker.
// By Ross Dickinson.
// Blame him if it breaks.
(function ($) {
    var methods = {
        init: function () {
            var $doc = $(document);
            var pickerTextBoxSelector = 'input.date';
            $doc.on('focus', pickerTextBoxSelector, function () {
                methods.onfocus($(this));
            });
            $doc.on('click', '.ui-datepicker-current', function () {
                $('.ui-datepicker-close').click();
            });
            $doc.on('click', 'button.date-picker-trigger', function () {
                var picker = $(this).prev(pickerTextBoxSelector);
                picker.focus();
            });
        },
        onfocus: function ($el) {
            if (methods.initDatePicker($el)) {
                // Need to call focus so it loads the first time.
                $el.focus();
            }
        },
        initDatePicker: function ($el) {
            // Returns false if the date picker is already initialized.
            // We don't wanna reapply the datepicker, obviously.
            if ($el.attr('data-datepicker')) { return false; }

            var args = {
                dateFormat: 'm/d/yy', // Our date displays don't include padding 0s, so the editors shouldn't either.
                timeFormat: 'h:mm TT',
                changeMonth: true,
                changeYear: true,

                /* blur needed to correctly handle placeholder text */
                onSelect: function (dateText, inst) {
                    // jQuery datepicker does not fire the change event for some inexplicable reason.
                    // The event is only fired if you type a date in yourself, but not when you select
                    // a date in the calendar. Firing the change event inside onSelect is the only way
                    // to do this. -Ross 4/24/2018
                    if (dateText !== inst.lastVal) {
                        $(this).change();
                    }

                    // TODO: Come back to this. This breaks the timepicker
                    // when you click the "Now" button and it tries to close
                    // the picker. It just causes it to reopen again.
                    //  $(this).blur().change().focus();
                },
                onClose: function (dateText, inst) {
                    this.focus();
                },
                beforeShow: function (input, inst) {
                    // This was originally here to do some IE focus fix, but for not IE
                    // we always returned true. I'm not even sure this is still needed.
                    return true;
                }
            };

            if (Application && Application.isInTestMode) {
                // By default, the datepicker appears when you focus the associated textbox. This
                // breaks things in regression testing sometimes due to the datepicker getting stuck
                // open from validation/focus. So for regression testing we're changing this so the
                // datepicker itself only shows up when you actually click the button. Annoyingly, this
                // causes a second calendar button to show up which there's no option to get rid of. 
                // However, that's only during testing and it really doesn't matter.
                args.showOn = 'button';
            }

            if ($el.hasClass('date-time')) {
                $el.datetimepicker(args);
            } else {
                $el.datepicker(args);
            }
            $el.attr('data-datepicker', true);

            return true;
        }
    };
    $.unobtrusiveDatePicker = methods;
    $(document).ready(methods.init);
})(jQuery);