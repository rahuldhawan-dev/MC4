(($) => {
    $(() =>
        $('form#deleteEnvironmentalNonComplianceEventActionItemForm')
            .submit(() => confirm('Are you sure you wish to delete the chosen Action Item?')));
})(jQuery);