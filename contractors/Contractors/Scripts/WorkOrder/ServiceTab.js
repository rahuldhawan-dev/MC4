(function ($) {
    const ELEMENTS = {
        serviceFrame: '#serviceFrame',
        serviceEditButton: '#serviceEditButton',
        serviceEditUrl: '#serviceEditUrl'
    };
    const ef = {
        initialize: function () {
            $(ELEMENTS.serviceEditButton).on('click', ef.onServiceEditButtonClick);
            $(ELEMENTS.serviceFrame).on("load", function () { 
                $(ELEMENTS.serviceFrame).contents().on('click', 'td a,.ui-tabs a.link-button,div.field a:not(.link-button)', function () { $(this).attr('target', '_top'); });
                $(ELEMENTS.serviceFrame).contents().find('#DetailsTab > a.link-button').attr("style", "display:none;")
            });
        },
        onServiceEditButtonClick: function () { $(ELEMENTS.serviceFrame).attr('src', $(ELEMENTS.serviceEditUrl).val()); },
    };

    $(document).ready(ef.initialize);

    return ef;
})(jQuery);