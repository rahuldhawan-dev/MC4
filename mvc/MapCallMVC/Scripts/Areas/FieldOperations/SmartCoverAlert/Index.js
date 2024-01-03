var SmartCoverAlert = (function ($) {
    const acknowledgeButton_Click = function (e) {
        const targetButton = e.target;
        const modal = $('#EditSmartCoverAlertModal');
        modal.detach();

        //// Set the necessary data for the record.
        modal.find('#Id').val(targetButton.getAttribute('Id'));
        $(document.body).append(modal);
        modal.jqm({ modal: false }).jqmShow();
    };
    
    $(function () {
        $('[name="acknowledgeButton"]').click(acknowledgeButton_Click);
    });
})(jQuery);