(function ($) {
    const ELEMENTS = {
        hydrantFrame: '#hydrantFrame',
        hydrantEditButton: '#hydrantEditButton',
        hydrantEditUrl: '#hydrantEditUrl'
    };
    const ef = {
        initialize: function () {
            $(ELEMENTS.hydrantEditButton).on('click', ef.onHydrantEditButtonClick);
            $(ELEMENTS.hydrantFrame).on("load", function () { 
                $(ELEMENTS.hydrantFrame).contents().on('click', 'td a,.ui-tabs a.link-button', function () { $(this).attr('target', '_top'); });
            });
        },
        onHydrantEditButtonClick: function () { $(ELEMENTS.hydrantFrame).attr('src', $(ELEMENTS.hydrantEditUrl).val()); },
    };

    $(document).ready(ef.initialize);

    return ef;
})(jQuery);