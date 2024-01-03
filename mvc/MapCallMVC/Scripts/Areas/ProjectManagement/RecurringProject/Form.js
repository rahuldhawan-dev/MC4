(($) => {
    var selectors = {
        reason: '#OverrideInfoMasterReason',
        toggle: '#GISDataCorrectionInfo',
    };
  
    var reasonChange = () => {
        var selected = $('option:selected', selectors.reason).text();
        var visible = selected === 'GIS Data Incorrect';
        $(selectors.toggle).toggle(visible);
    };

    var inaccuracyTypeChange = () => {
        var selected = $('option:selected', selectors.inaccuracyType).text();

        $(selectors.infoLabel).text(labels[selected]);
    };

    $(() => {
        $(selectors.reason).change(reasonChange);
        reasonChange();
    });
})(jQuery);