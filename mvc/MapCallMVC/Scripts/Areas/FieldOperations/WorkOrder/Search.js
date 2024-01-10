// This script is meant to be used wherever work orders are searchable, if you need to add more specific
// functionality for a particular work order search page then a script should be added to a folder with that
// name ("WorkOrderScheduling\Search.js", for instance).
var WorkOrders = (function ($) {
    var ELEMENTS = {};
    const REQUESTED_BY_ACOUSTIC_MONITORING_ID = 6;
    const ASSET_TYPE_SERVICE_ID = 4;
    const ASSET_TYPE = [4,6]

    var m = {
        initialize: function () {
            ELEMENTS = {
                acousticMonitoringType: $('#AcousticMonitoringType'),
                assetType: $('#AssetType'),
                premiseNumber: $('#PremiseNumber_Value'),
                previousServiceLineMaterial: $('#PreviousServiceLineMaterial'),
                requestedBy: $('#RequestedBy')
            };
            m.initAssetType();
            m.initRequestedBy();
        },
        initAssetType: function () {
            ELEMENTS.assetType.on('change', m.onAssetTypeChanged);
            m.onAssetTypeChanged();
        },
        onAssetTypeChanged: function () {
            Application.toggleField('#PremiseNumber_Value', false);
            Application.toggleField('#PreviousServiceLineMaterial', false);
            Application.toggleField('#CompanyServiceLineMaterial', false);
            Application.toggleField('#CustomerServiceLineMaterial', false);
            ELEMENTS.premiseNumber.val(null);
            ELEMENTS.previousServiceLineMaterial.val(null);
            var selectedValues = ELEMENTS.assetType.val();
            if (selectedValues)
            {
                var integerArray = selectedValues.map(Number);
                if (integerArray.some(r => ASSET_TYPE.includes(r))) {
                    Application.toggleField('#PremiseNumber_Value', true);
                    Application.toggleField('#PreviousServiceLineMaterial', true);
                    Application.toggleField('#CompanyServiceLineMaterial', true);
                    Application.toggleField('#CustomerServiceLineMaterial', true);
                }
            }
        },

        initRequestedBy: function () {
            ELEMENTS.requestedBy.on('change', m.onRequestedByChanged);
            m.onRequestedByChanged();
        },
        onRequestedByChanged: function () {
            Application.toggleField('#AcousticMonitoringType', false);
            ELEMENTS.acousticMonitoringType.val(null);
            var selectedValues = ELEMENTS.requestedBy.val();
            if (selectedValues && selectedValues.map(Number).includes(REQUESTED_BY_ACOUSTIC_MONITORING_ID)) {
                Application.toggleField('#AcousticMonitoringType', true);
            }
        }
    };
    $(document).ready(m.initialize);
    return m;
})(jQuery);