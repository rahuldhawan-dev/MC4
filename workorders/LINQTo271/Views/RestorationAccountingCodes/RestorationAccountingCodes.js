var RestorationAccountingCodes = {
    init: function () {
        RestorationAccountingCodes.initScrollEditTextBoxIntoView();
    },
    initScrollEditTextBoxIntoView: function () {
        $(document).ready(function () {
            var editable = $('.scrollIntoView')[0]; 
            if (editable) { editable.scrollIntoView(); }
        });
    }
};

RestorationAccountingCodes.init();