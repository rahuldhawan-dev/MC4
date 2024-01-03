var MainBreakForm = (function ($) {
    const WATER_MAIN_BREAK_REPLACE = 80;
    var m = {
        initialize: function () {
            ELEMENTS = {
                finalWorkDescription: $('#FinalWorkDescription'),
                footageReplaced: $('#FootageReplaced'),
                replacedWith: $('#ReplacedWith'),
                saveMainBreakButton: $('#SaveMainBreak'),
                cancelMainBreakButton: $('#CancelMainBreak'),
                mainBreakErrorMessage: $('#main-break-error-message'),
                btnFinalize: $('#btnFinalize')
            },
            m.initFinalWorkDescription();
            m.initSaveBtnClicked();
        },
        initFinalWorkDescription: function () {
            ELEMENTS.finalWorkDescription.on('change', m.onFinalWorkDescriptionChanged);
            m.onFinalWorkDescriptionChanged();
        },
        onFinalWorkDescriptionChanged: function () {
            const selectedWorkDescriptionId = parseInt(ELEMENTS.finalWorkDescription.val(), 10);
            if (selectedWorkDescriptionId == WATER_MAIN_BREAK_REPLACE) {
                ELEMENTS.replacedWith.prop('disabled', false);
                ELEMENTS.footageReplaced.prop('disabled', false);
                ELEMENTS.replacedWith.prop('required', true);
                ELEMENTS.footageReplaced.prop('required', true);
            } else {
                ELEMENTS.footageReplaced.prop('disabled', true);
                ELEMENTS.replacedWith.prop('disabled', true);
                ELEMENTS.replacedWith.prop('required', false);
                ELEMENTS.footageReplaced.prop('required', false);
            }
        },
        initSaveBtnClicked: function () {
            ELEMENTS.saveMainBreakButton.on('click', m.onSaveButtonClicked);
            ELEMENTS.cancelMainBreakButton.on('click', m.onCancelButtonClicked);
        },
        onSaveButtonClicked: function () {
            ELEMENTS.mainBreakErrorMessage.hide();
            ELEMENTS.btnFinalize.prop('disabled', false);
        },
        onCancelButtonClicked: function () {
            if ($('#mainBreaksTable tr').length < 2) {
                ELEMENTS.mainBreakErrorMessage.show();
                ELEMENTS.btnFinalize.prop('disabled', true);
            }
            else {
                ELEMENTS.mainBreakErrorMessage.hide();
                ELEMENTS.btnFinalize.prop('disabled', false);
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);