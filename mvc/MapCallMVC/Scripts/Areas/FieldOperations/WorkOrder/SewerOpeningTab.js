(function ($) {
    const ELEMENTS = {
        sewerOpeningFrame: '#sewerOpeningFrame',
        sewerOpeningEditButton: '#sewerOpeningEditButton',
        sewerOpeningEditUrl: '#sewerOpeningEditUrl'
    };
    const ef = {
        initialize: function () {
            $(ELEMENTS.sewerOpeningEditButton).on('click', ef.onSewerOpeningEditButtonClick);
            $(ELEMENTS.sewerOpeningFrame).on("load", function () { 
                $(ELEMENTS.sewerOpeningFrame).contents().on('click', 'td a,.ui-tabs a.link-button', function () { $(this).attr('target', '_top'); });
            });
        },
        onSewerOpeningEditButtonClick: function () { $(ELEMENTS.sewerOpeningFrame).attr('src', $(ELEMENTS.sewerOpeningEditUrl).val()); },
    };

    $(document).ready(ef.initialize);

    return ef;
})(jQuery);