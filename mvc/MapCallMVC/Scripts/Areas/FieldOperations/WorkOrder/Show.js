var WorkOrder = {
    selectors: {
        cancelOrderPanel: 'div#cancelOrderPanel',
        btnCancelOrder: '#btnCancelOrder'
    },

    initialize: function () {
        $(WorkOrder.selectors.btnCancelOrder).click(WorkOrder.btnCancelOrder_Click);
        WorkOrder.tryLoadingLastSAPNotificationSearch();
    },

    btnCancelOrder_Click: function () {
        if ($(WorkOrder.selectors.cancelOrderPanel).is(":visible")) {
            $(WorkOrder.selectors.cancelOrderPanel).hide();
        }
        else {
            $(WorkOrder.selectors.cancelOrderPanel).show();
        }       
    },

    tryLoadingLastSAPNotificationSearch() {
        const lastSearch = SAPNotificationSearchStorage.getLastSearch();
        if (lastSearch) {
            const sapLink = $('#sap-notification-search-link');
            // NOTE: No need to append the ? for the querystring as it's already
            // included in the lastSearch variable.
            let url = sapLink.attr('href') + lastSearch;
            sapLink.attr('href', url);
        }
    }
};

$(document).ready(WorkOrder.initialize);