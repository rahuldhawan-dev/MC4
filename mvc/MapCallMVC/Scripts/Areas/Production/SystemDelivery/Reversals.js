var SystemDeliveryEntryReversal = (($) => {
    var ELEMENTS = {};
    const sder = {
        intialize: function () {
            ELEMENTS = {
                addReversal: $('#AddReversal')
            };

            ELEMENTS.addReversal.on('click', sder.onAddReversalButtonClicked);
        },

        onAddReversalButtonClicked: function (e) {
            // We want to prevent the default submit behavior until we have looped through and checked the rows
            e.preventDefault();
            var submitForm = true;
            var dialog = confirm('Are you sure you would like to enter these adjustments?');

            if (dialog) {

                $('#reversalTable tbody tr').each(function () {
                    var reverseField = $(this).find(':checkbox');
                    var updatedField = $(this).find('input[type=text]');
                    var reverseValue = reverseField.is(':checked');
                    var updatedValue = updatedField.val();
                    var commentValue = $(this).find('td:eq(7) input[type=text]').val();

                    if (reverseValue && (commentValue == "" || commentValue == undefined)) {
                        alert('A Comment is required for the Adjustment.');
                        return false;
                    }

                    // Checking for undefined here as well since we have a TR seperating each entry item in the table
                    if (!reverseValue && (updatedValue != "" && updatedValue != undefined)) {
                        alert("Please check the Reversal box if you would like to record the Adjustment value.");
                        submitForm = false;
                        return false;
                    }
                })

                if (submitForm) {
                    $('#ReversalForm').submit();
                }
            }
        },
    }

    sder.ELEMENTS = ELEMENTS;
    $(document).ready(sder.intialize);
    return sder;
})(jQuery);