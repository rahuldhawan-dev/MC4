(function ($) {

    var addressFields;
    var CHECK_BOX_SELECTOR = '#chkIncludeAddress';
    var FORM_SELECTOR = '#contactsForm';
    var Contact = {
        initialize: function () {
            $(FORM_SELECTOR).validate({ ignore: [':hidden, :disabled'] });
            addressFields = $('#addressFields').find('input:enabled, select:enabled').not(CHECK_BOX_SELECTOR);
            $(document).on('change', CHECK_BOX_SELECTOR, Contact.toggleAddress);
            Contact.toggleAddress();
        },
        toggleAddress: function () {
            var isChecked = $(CHECK_BOX_SELECTOR).prop('checked');
            addressFields.attr('disabled', (isChecked ? false : 'disabled'));
        }
    };

    $(document).ready(Contact.initialize);

    return Contact;
})(jQuery);