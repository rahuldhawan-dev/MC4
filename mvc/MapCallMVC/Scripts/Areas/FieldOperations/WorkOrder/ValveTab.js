(function ($) {
    const ELEMENTS = {
        valveFrame: '#valveFrame',
        valveEditButton: '#valveEditButton',
        valveEditUrl: '#valveEditUrl'
    };
    const ef = {
        initialize: function () {
            $(ELEMENTS.valveEditButton).on('click', ef.onValveEditButtonClick);
            $(ELEMENTS.valveFrame).on("load", function () {
                $(ELEMENTS.valveFrame).contents().on('click', 'td a,.ui-tabs a.link-button', function () { $(this).attr('target', '_top'); });
            });
        },
        onValveEditButtonClick: function () { $(ELEMENTS.valveFrame).attr('src', $(ELEMENTS.valveEditUrl).val()); },
    };

    $(document).ready(ef.initialize);

    return ef;
})(jQuery);