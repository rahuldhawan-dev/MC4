(function ($) {

    var ActionBar = {
        _storage: UserStorage.getPluginContainer('actionBar'),
        element: null,

        init: function () {
            ActionBar.element = $('#actionBar');
            ActionBar.initIndexButton();
            ActionBar.initHelpButton();
            ActionBar.initFormsButton();
        },

        initHelpButton: function () {
            ActionBar.helpPanel = new FlyoutPanel({
                content: $('#actionBarHelpPanel'),
                openingTrigger: $('.ab-help')
            });
        },
        initFormsButton: function () {
            ActionBar.formsPanel = new FlyoutPanel({
                content: $('#actionBarFormsPanel'),
                openingTrigger: $('.ab-forms')
            });
        },

        // Index button stuff
        initIndexButton: function () {
            if (Application.routeData.action.toLowerCase() === 'index') {
                ActionBar.setLastSearch();
            } else {
                ActionBar.setIndexLink();
            }
        },

        setIndexLink: function () {
            var lastSearch = ActionBar._storage.get('lastSearch');
            if (lastSearch && lastSearch.query) {
                var curRoute = Application.routeData;
                if (curRoute.controller == lastSearch.route.controller && curRoute.area == lastSearch.route.area) {
                    $('.ab-index').each(function (i, el) {
                        el.href = el.href + '?' + lastSearch.query;
                    });
                }
            }
        },

        setLastSearch: function () {
            var url = location.href;
            // We don't want to differentiate urls based on query parameters
            // or #fragments, otherwise things would get all weird.
            url = url.split('#')[0];
            var queryString = url.split('?')[1];

            var search = {
                query: queryString,
                route: Application.routeData
            }
            ActionBar._storage.set('lastSearch', search);
        }
    };

    var FlyoutPanel = function (options) {
        this.element = $('<div class="action-bar-flyout-panel"></div>');
        this.element.css('display', 'block');

        this.element.append(options.content);
        // This content is hidden by default so there's no brief flash of content on page loads.
        // Also, as of jQuery 3, you can't call show() on an element that has a css class
        // to add display: none to it. It won't override that, which imo seems really silly. They
        // expect you to add/remove css classes from the element instead if you're doing it that way.
        options.content.css('display', 'block');

        this.isDisplayed = false;

        // We need to append the element briefly in order to get the css.
        // For some reason, Firefox can figure this out without the element
        // being attached to the DOM. 

        this.element.appendTo($('body'));
        this.hiddenPosition = parseInt(this.element.css('right'), 10);
        this.element.detach();

        var self = this;
        options.openingTrigger.click(function () {
            self.toggle();
        });

        this.onDocClick = function (e) {
            if (e.target !== self.element[0] && !$(e.target).parents().is(self.element) && !$(e.target).parents().is(ActionBar.element) && !$(e.target).parents().is($('#help'))) {
                self.hide();
            }
        };
    };
    FlyoutPanel.prototype = {
        toggle: function () {
            if (this.isDisplayed) {
                this.hide();
            } else {
                this.show();
            }
        },

        hide: function () {
            var self = this;
            $(document).off('click.actionBar', this.onDocClick);
            var endPosition = this.hiddenPosition - this.element.outerWidth();
            this.element.animate({
                right: endPosition
            }, {
                duration: 200,
                queue: false,
                complete: function () {
                    self.element.detach();
                }
            });

            this.isDisplayed = false;
        },

        show: function () {
            $(document).on('click.actionBar', this.onDocClick);
            this.element.appendTo($('body'));
            var endPosition = this.hiddenPosition + this.element.outerWidth();

            this.element.animate({
                right: endPosition
            }, {
                duration: 200,
                queue: false
            });

            this.isDisplayed = true;
        }
    };

    $(document).ready(ActionBar.init);

})(jQuery)