(function ($) {

    var panel = null;
    var isVisible = false;

    var AdminPanel = {
        'init': function () {
            var button = $('#adminPanelButton');
            AdminPanel.initPanel();
            button.on('click', AdminPanel.togglePanel);
        },
        'initPanel': function () {
            panel = $('#adminPanel');
            AdminPanel.hidePanel();
            // The panel's set to display: none by default to prevent
            // it from briefly flickering on screen.
            panel.css('display', 'block');
        },
        'hidePanel': function () {
            panel.remove();
            $(document).off('click.adminPanel', AdminPanel.onDocClick);
            isVisible = false;
        },
        'showPanel': function (e) {
            $('body').append(panel);
            $(document).on('click.adminPanel', AdminPanel.onDocClick);
            isVisible = true;
            // Stop the event, the document click handler is called
            // immediately after this completes, and the panel then
            // gets hidden again.
            e.stopPropagation();
        },
        'togglePanel': function (e) {
            if (isVisible) {
                AdminPanel.hidePanel(e);
            } else {
                AdminPanel.showPanel(e);
            }
        },
        'onDocClick': function (e) {
            if (e.target !== panel[0] && !$(e.target).parents().is(panel)) {
                AdminPanel.hidePanel();
            }
        }
    };

    AdminPanel.init();
})(jQuery)