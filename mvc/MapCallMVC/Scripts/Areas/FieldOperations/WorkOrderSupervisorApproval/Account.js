(($) => {
    const ELEMENTS = {
        cancelRejectButton: $('#cancel-reject-button'),
        rejectButtons: $('.reject-button'),
        rejectDialog: $('#reject-dialog')
    };

    const onCancelRejectButtonClicked = () => {
        ELEMENTS.rejectDialog[0].close();
    }

    const onRejectButtonClicked = () => {
        ELEMENTS.rejectDialog[0].showModal();
    }

    const init = () => {
        ELEMENTS.cancelRejectButton.on('click', onCancelRejectButtonClicked);
        ELEMENTS.rejectButtons.on('click', onRejectButtonClicked);
    }

    $(document).ready(init);
})(jQuery);