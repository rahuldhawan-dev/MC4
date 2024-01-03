var MaterialUsedForm = {
    CONTROLS: {
        partSearch: '#PartSearch',
        partSearchResults: '#PartSearchResults',
        material: '#Material',
        nonStockDescription: '#NonStockDescription',
        stockLocation: '#StockLocation',
        doSearch: '#DoSearch',
        materialUsedId: '#MaterialUsed',
        workOrderId: '#WorkOrder'
    },

    URLS: {
        partSearch: '/Material/MaterialSearch',
        partSearchMaterialUsedId: '/Material/MaterialSearchByMaterialUsedId',
        partSearchWorkOrderId: '/Material/MaterialSearchByWorkOrderId'
    },

    initialize: function () {
        MaterialUsedForm.initializeEvents();
        $(MaterialUsedForm.CONTROLS.doSearch).button({
            icons: { primary: 'ui-icon-search' },
            text: false
        });
    },

    initializeEvents: function () {
        $(MaterialUsedForm.CONTROLS.doSearch).click(MaterialUsedForm.doSearch_Click);
        $(MaterialUsedForm.CONTROLS.partSearchResults).change(MaterialUsedForm.partSearchResults_Change);
        $(MaterialUsedForm.CONTROLS.material).change(MaterialUsedForm.toggleNonStockDescription);
        $(MaterialUsedForm.CONTROLS.stockLocation).change(MaterialUsedForm.toggleNonStockDescription);
    },

    performLookup: function (str) {
        if ($(MaterialUsedForm.CONTROLS.materialUsedId).val() !== undefined) {
            $.getJSON(
                MaterialUsedForm.URLS.partSearchMaterialUsedId,
                { search: str, materialUsedId: $(MaterialUsedForm.CONTROLS.materialUsedId).val() },
                MaterialUsedForm.partSearch_Callback);
        }
        else {
            $.getJSON(
                MaterialUsedForm.URLS.partSearchWorkOrderId,
                { search: str, workOrderId: $(MaterialUsedForm.CONTROLS.workOrderId).val() },
                MaterialUsedForm.partSearch_Callback);
        }
    },

    displayResults: function (results) {
        var sb = [],
            partSearchResults = $(MaterialUsedForm.CONTROLS.partSearchResults)
                .removeAttr('disabled')
                .html('');
        sb.push('<option value="">--Select Here--</option>');
        $(results).each(function (i, o) {
            sb.push('<option value="' + o.Value + '">' + o.Text + '</option>');
        });

        partSearchResults.html(sb.join(''));
    },

    displayNoResults: function () {
        $(MaterialUsedForm.CONTROLS.partSearchResults)
            .attr('disabled', 'disabled')
            .html('<option>No results found.</option>');
    },

    doSearch_Click: function () {
        MaterialUsedForm.performLookup($(MaterialUsedForm.CONTROLS.partSearch).val());
        return false;
    },

    partSearch_Callback: function (d) {
        (d.Options.length) ?
            MaterialUsedForm.displayResults(d.Options) :
            MaterialUsedForm.displayNoResults();
    },

    partSearchResults_Change: function (e) {
        var val = $(e.target).val();
        if (val) {
            $(MaterialUsedForm.CONTROLS.material).val(val).change();
        }
    },

    toggleNonStockDescription: () => {
        var materialHasValue = !!$(MaterialUsedForm.CONTROLS.material).val();
        var stockLocationHasValue = !!$(MaterialUsedForm.CONTROLS.stockLocation).val();
        var shouldMakeNonStockVisible = !(materialHasValue || stockLocationHasValue);
        $(MaterialUsedForm.CONTROLS.nonStockDescription).parent().parent().toggle(shouldMakeNonStockVisible);
        if (!shouldMakeNonStockVisible) {
            // need to wipe out the value so the RequiredWhen validation works identically
            // to the old ThisOr validator that would require one or the other values to be null.
            $(MaterialUsedForm.CONTROLS.nonStockDescription).val('');
        }
    }
};

$(document).ready(MaterialUsedForm.initialize);
