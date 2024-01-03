var WorkOrderSupervisorApprovalResourceView = {
    initialize: function() {
        var operatingCenterID = getServerElementById('hidOperatingCenterID').val();
        var workorderID = getServerElementById('lblWorkOrderID').val();
        var assetTypeID = getServerElementById('hidAssetTypeID').val();
        var assetID = getServerElementById('hidHydrantID').val() ||
            getServerElementById('hidValveID').val() ||
            getServerElementById('hidPremiseNumber').val();

        getServerElementById('hidHistoryOperatingCenterID').val(operatingCenterID);
        getServerElementById('hidHistoryAssetTypeID').val(assetTypeID);
        getServerElementById('hidHistoryAssetID').val(assetID);
        // this is the most fragile part of this. the asset types that shouldn't be related.
        if (assetTypeID !== '3' && assetTypeID !== '7' && assetID !== '' && assetID !== undefined) {
            getServerElementById('hidHistoryAssetID').change();
        }
    }
};