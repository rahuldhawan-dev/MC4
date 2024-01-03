const RequisitionIndex = (($) => {
    const methods = {
        initialize: function () {
            AjaxTable.initialize('#requisitions-table');
        }
    };

    $(document).ready(function () {
        methods.initialize();
    });

})(jQuery); 