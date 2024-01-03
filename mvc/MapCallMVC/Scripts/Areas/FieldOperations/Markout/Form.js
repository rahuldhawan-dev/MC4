var MarkoutForm = (function($) {
    const MARKOUT_TYPE_NONE = 38;
    var m = {
        initialize: function () {
            ELEMENTS = {
                markoutType: $('#MarkoutType'),
                note: $('#Note')
            },
            m.initNote();
        },
        initNote: function () {
            ELEMENTS.markoutType.on('change', m.onMarkoutTypeChanged);
            m.onMarkoutTypeChanged();
        },
        onMarkoutTypeChanged: function () {
            const selectedmarkoutTypeId = parseInt(ELEMENTS.markoutType.val(), 10);
            if (selectedmarkoutTypeId == MARKOUT_TYPE_NONE) {
                ELEMENTS.note.closest('.field-pair').show();
            } else {
                ELEMENTS.note.closest('.field-pair').hide();
            }
        },
        validateMarkoutNumber: function (val) {
            var isMarkoutEditable = $('#WorkOrderOperatingCenterMarkoutEditable').val() === 'True';
            if (isMarkoutEditable) {
                // This already has a required field validator so we don't need to do anything here.
                return true;
            }

            // If markout is not editable, then the markout # must have a length of 9. No more, no less.
            return val.length === 9;
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);