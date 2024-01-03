(($) => {
    $(() => {
        ['Approve', 'Reject'].forEach((action) => {
            $('#' + action + 'Button').click(() => {
                if (confirm('Are you sure you want to ' + action + ' this order?')) {
                    $('#' + action + 'Form').submit();
                }
            });
        });
    });
})(jQuery);