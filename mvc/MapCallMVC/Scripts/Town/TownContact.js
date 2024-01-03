var TownContact = (function ($) {
    var tc = {
        ELEMENTS: null,
        initialize: function () {
            tc.ELEMENTS = {
                addContactButton: $('#addContactButton'),
                addContactWrap: $('#addContactFormWrap'),
                cancelButton: $('#cancelAddContactButton'),
                contactsTable: $('#contactsTable')
            };
            tc.initializeTable();
            tc.initializeAddTownContactSection();
        },

        initializeTable: function () {
            tc.ELEMENTS.contactsTable.find('form').on('submit', function (ev) {
                if (!confirm("Are you sure you want to remove this contact?")) {
                    ev.preventDefault();
                    return false;
                }
                return true;
            });
        },

        initializeAddTownContactSection: function () {
            tc.ELEMENTS.addContactWrap.hideAndSavePosition();
            tc.ELEMENTS.addContactButton.click(tc.onAddContactButtonClick);
            tc.ELEMENTS.cancelButton.click(tc.onCancelButtonClick);
        },

        onAddContactButtonClick: function () {
            tc.ELEMENTS.addContactWrap.reattachAtSavedPosition();
            tc.ELEMENTS.addContactButton.hideAndSavePosition();
        },

        onCancelButtonClick: function () {
            tc.ELEMENTS.addContactWrap.hideAndSavePosition();
            tc.ELEMENTS.addContactButton.reattachAtSavedPosition();
        }
    };

    $(document).ready(tc.initialize);
    return tc;
})(jQuery);