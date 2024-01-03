(($) => {
    const PublicWaterStatuses = {
        PENDING: '4',
        PENDING_MERGER: '5'
    };

    const OptionalVisibilityItems = {
        initialize: () => {
             $('#Status').change(OptionalVisibilityItems.onStatusChange);

             // Fire once to init the field visibility.  
             OptionalVisibilityItems.onStatusChange();
        },

            onStatusChange: () => {
            const selectedStatus = $('#Status').val();
            if (selectedStatus === PublicWaterStatuses.PENDING) {
                 Application.toggleField('#AnticipatedActiveDate', true);
            } else {
                // Why are we leaving this visible if the user enters a value but it's otherwise not Pending? 
                if ($('#AnticipatedActiveDate').val() !== '') {
                    Application.toggleField('#AnticipatedActiveDate', true);
                } else {
                    Application.toggleField('#AnticipatedActiveDate', false);
                }
            }

            const isVisibleForPendingMerger = selectedStatus === PublicWaterStatuses.PENDING_MERGER;
            Application.toggleField('#AnticipatedMergerDate', isVisibleForPendingMerger);
            Application.toggleField('#ValidTo', isVisibleForPendingMerger);
            Application.toggleField('#ValidFrom', isVisibleForPendingMerger);
            Application.toggleField('#AnticipatedMergePublicWaterSupply', isVisibleForPendingMerger);
        }
    };

    $(OptionalVisibilityItems.initialize);

})(jQuery);