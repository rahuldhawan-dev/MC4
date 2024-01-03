// ReSharper disable Es6Feature
var Application = (function ($) {
    var app;

    var createNotificationDisplay = function () {
        var $notifications =
            $(
                '<div id="notifications" class="notification"><img src="' + Application.iconUrls.warning + '" /><div class="message"></div></div>');
        $(app.selectors.siteContent).prepend($notifications);
        return $notifications;
    };

    app = {
        MAP_PROXY_OPTIONS: {
            url: '/proxies/ESRIProxy.ashx',
            // also needs to be set in ESRIProxy.config
            proxies: ['www.arcgis.com', 'utility.arcgis.com', 'geoprocess-np.amwater.com', 'awdev.maps.arcgis.com', 'geoprocess.amwater.com', 'aw.maps.argis.com', 'onemap-np.amwaternp.com', 'onemap.amwater.com']
        },

        selectors: {
            pageTitle: '#pageTitle',
            errorMessage: '.error',
            dataConfirmForm: 'form[data-confirm]',
            notifications: '#notifications',
            noDoubleSubmitForm: 'form.no-double-submit',
            siteContent: '#siteContent',
            submitButtons: ':submit',
            resetButtons: '.reset'
        },

        dynamicUiParsers: [],
        isJQueryAjaxRunning: 0,
        isDojoAjaxRunning: 0,
        isMapAjaxRunning: 0,
        isInsideIframe: false,
        isInTestMode: false,

        // This is set by an inline script in the main layout view somewhere.
        routeData: null,

        displayNotification: function (notification) {
            var $notifications = $(app.selectors.notifications);
            $notifications = $notifications.length ? $notifications : createNotificationDisplay();
            $(".message", $notifications).text(notification);
        },

        clearNotification: function () {
            $(app.selectors.notifications).remove();
        },

        preInitialize: function () {
            // This is to remove the nav/actionbar/topbar when a page loads in an iframe.
            // This needs to run before the document.ready event to prevent a flash-of-content
            // from appearing in Firefox 37.  
            if (window.parent !== window.self) {
                app.isInsideIframe = true;
                $('html').addClass('slim');
            }
        },

        initialize: function () {
            if (Application.isInTestMode) {
                // If regressions are running, all local ajax calls *must* be done synchronously due
                // to the nhibernate session being a singleton. This is required for thread safety.
                $.ajaxSetup({ async: false });
            }
            app.initEvents();
            app.initDynamicUiParsers();
        },

        initEvents: function () {
            $(document).ajaxStop(app.onJQueryAjaxStop);
            $(document).ajaxStart(app.onJQueryAjaxStart);

            if (typeof (require) === 'function') {
                require(['dojo/request', 'dojo/request/notify'],
                    function (request, notify) {
                        notify('send', app.onDojoAjaxStart);
                        notify('done', app.onDojoAjaxStop);
                    });
            }

            if (typeof ($.esriMapSetup) === 'function') {
                $.esriMapSetup({
                    ajaxStart: app.onMapAjaxStart,
                    ajaxStop: app.onMapAjaxStop
                });
            }

            $(document).on('submit', app.selectors.dataConfirmForm, app.onDataConfirmFormSubmit);
            $(document).on('submit', app.selectors.noDoubleSubmitForm, app.onNoDoubleSubmitFormSubmit);
            $(document).on('click', app.selectors.submitButtons, app.onSubmitButtonClick);
            $(document).on('click', app.selectors.resetButtons, app.onResetButtonClicked);
        },

        initDynamicUiParsers: function () {
            // These are all unobtrusive calls that need to be ran
            // on dynamically added content. Generally after ajax
            // calls that return partial views.
            var parsers = app.dynamicUiParsers;
            parsers.push(function ($content) {
                if ($.fn.unobtrusiveChart) {
                    $content.find('.chart').unobtrusiveChart();
                }
            });

            parsers.push(function ($content) {
                if ($.fn.collapsePanel) {
                    $content.find('.collapse-panel').collapsePanel();
                }
            });

            parsers.push(function ($content) {
                if ($.fn.unobtrusiveDetergent) {
                    $content.find('select, .checkbox-list').unobtrusiveDetergent();
                }
            });

            parsers.push(function ($content) {
                if ($.fn.unobtrusiveRangePicker) {
                    $content.find('.range').unobtrusiveRangePicker();
                }
            });

            parsers.push(function ($content) {
                $.validator.unobtrusive.parseDynamicContent($content);
            });

            parsers.push(function ($content) {
                if ($.fn.unobtrusiveUploader) {
                    $content.find('.file-upload').unobtrusiveUploader();
                }
            });

            parsers.push(function ($content) {
                if ($.fn.fullCalendar) {
                    $content.find('.fc').fullCalendar('render');
                }
            });
        },

        runDynamicUiParsers: function (selector) {
            var $content = $(selector);
            for (var i = 0; i < app.dynamicUiParsers.length; i++) {
                app.dynamicUiParsers[i]($content);
            }
        },

        setPageTitle: function (str) {
            $(app.selectors.pageTitle).text(str);
            document.title = str;
        },

        clearErrorMessage: function () {
            $(app.selectors.errorMessage)
                .text('')
                .toggle(false);
        },

        onJQueryAjaxStart: function () {
            app.isJQueryAjaxRunning++;
        },

        onJQueryAjaxStop: function () {
            //Application.isJQueryAjaxRunning--;
            app.isJQueryAjaxRunning =
                app.isJQueryAjaxRunning > 0
                    ? app.isJQueryAjaxRunning - 1
                    : 0;
        },

        onDojoAjaxStart: function () {
            app.isDojoAjaxRunning++;
        },

        onDojoAjaxStop: function () {
            //Application.isDojoAjaxRunning--;
            app.isDojoAjaxRunning =
                app.isDojoAjaxRunning > 0
                    ? app.isDojoAjaxRunning - 1
                    : 0;
        },

        onMapAjaxStart: function () {
            app.isMapAjaxRunning++;
        },

        onMapAjaxStop: function () {
            //Application.isMapAjaxRunning--;
            app.isMapAjaxRunning =
                app.isMapAjaxRunning > 0
                    ? app.isMapAjaxRunning - 1
                    : 0;
        },

        isAjaxRunning: function () {
            if (app.isJQueryAjaxRunning || app.isDojoAjaxRunning || app.isMapAjaxRunning) {
                return true;
            }
            return false;
        },

        onDataConfirmFormSubmit: function (e) {
            var confirmMsg = $(this).attr('data-confirm');
            if (!confirm(confirmMsg)) {
                // This needs to be called because returning false
                // will not prevent the onNoDoubleSubmitFormSubmite
                // handler from being called.
                e.stopImmediatePropagation();
                return false;
            }
            return true;
        },

        onSubmitButtonClick: function () {
            // clear all the submit buttons in case one was already clicked
            $(app.selectors.submitButtons).removeAttr('clicked');
            // add clicked to this one.
            $(this).attr('clicked', 'true');
        },

        onNoDoubleSubmitFormSubmit: function (e) {
            if (e.isImmediatePropagationStopped()) {
                return false;
            }
            //disable all the submit buttons
            $(':submit', this).attr('disabled', 'disabled');
            //get the clicked submit button
            var button = $(':submit[clicked=true]');
            $(this).submit(function () { return false; });

            if (button.val()) {
                button.after('<input type="hidden" value="' + button.val() + '" name="' + button.attr('name') + '" />');
            }

            return true;
        },

        onResetButtonClicked: function (ev) {
            var element = $(this)[0];
            var form = element.form;
            $(':input', form).filter(':visible').val('').removeAttr('checked').removeAttr('selected').trigger('change');
            $('.range', form).unobtrusiveRangePicker('reset');
            $('.multilist', form).multilist('clear');
            // Some dropdowns do not have an empty value so form
            // resetting causes the dropdown to not display any
            // selected value. To get past this, we select the first
            // value in the list. This is only done for selects that
            // request it, because otherwise we want the form reset
            // to just select the empty string value item by default.
            $('select[data-select-on-reset]')
                .each(function () {
                    var $s = $(this);
                    var resetValue = $s.attr('data-select-on-reset');

                    // Specifically check "" instead of !resetValue because we want 0's included.
                    if (resetValue === "") {
                        resetValue = $(this).find('option:first').val();
                    }
                    $(this).val(resetValue);
                });

            // TODO: Make unobtrusiveRangePicker listen for this event.
            // We can't trigger the onreset event because it messes things up in IE.
            // It won't mark the selected dropdown item as selected.
            $(form).trigger('fauxreset');
        },

        // show or hide an editor row (label and all) based on the value of show
        toggleField: (sel, show = true) => $(sel).closest('.field-pair').toggle(show),

        /**
         * Toggles the visual state of a MapCall field and it's label to be required or not required. When using custom 
         * client callbacks to perform validation, the target field may or may not be required given some business rule - 
         * because of this, the typical 'red asterisk' that denotes if a field is required or not is not set; therefore, 
         * use this method when you need to set this required ui state of the MapCall field manually.
         * 
         * This method uses the same DOM element markup to show this required ui state as does the required-when-validation.js for
         * the asp.net mvc defined RequiredWhen attribute validators. Ideally, this element's html would be shared as a const
         * between these two files, but, we're not really sure where that should live, so for now - it lives in both.
         * 
         * @param {jQuery} $target - The MapCall field whose visual state will be toggled
         * @param {boolean} [isRequired=true] - True if the MapCall field should be toggled as required, else false. Default: true
         * @returns {jQuery} - The original target to support chaining
         */
        toggleFieldRequiredUiState: ($target, isRequired = true) => {

            const $targetsLabel = $target.closest('.field-pair').find('label[for="' + $target.attr('id') + '"]');
            const requiredLabelSelector = 'span.required-label';

            if (!isRequired) {
                $targetsLabel.nextAll(requiredLabelSelector).remove();
            }
            else if ($targetsLabel.siblings(requiredLabelSelector).length === 0) {
                $('<span class="required-label"> *</span>').insertAfter($targetsLabel)
            }

            return $target;
        },

        //normalize ture/false values
        getBooleanValue: (sel) => $(sel).val().toLowerCase() === 'true',
        // Enables/disables a jQuery UI tab.
        toggleEnabledTab: (tabId, enable) => {
            const enableOrDisable = enable ? 'enable' : 'disable';
            const tabsContainer = $('.tabs-container');
            // jQuery UI forces you to deal with tabs based on their index.
            // The index is based on the tabs. The tab panels are not a reliable index because they
            // do not exist in their own container. They share it with other non-panel elements.
            const index = tabsContainer.find(`a[href="#${tabId}"]`).parent().index();
            tabsContainer.tabs(enableOrDisable, index);

            // NOTE: You can't exactly disable the active tab. It will not be disabled until
            // you switch to another tab. This shouldn't be an issue for the time being(why would
            // you need to disable the tab you're looking at?), so I'm not gonna work on figuring
            // out how we should deal with that(ie switching to another tab automatically, but which tab
            // would we switch to?);
        }
    };
    return app;
})(jQuery);

Application.preInitialize();
$(document).ready(Application.initialize);
