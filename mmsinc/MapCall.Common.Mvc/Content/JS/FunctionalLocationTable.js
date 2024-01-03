(function ($) {
    var selector = '[data-ajax-table="#functionalLocationTable"]';
    function onClick() {
        var url = $(selector).attr('href');
        // so we don't keep appending 
        var append = '?operatingCenterId=' + $('#OperatingCenter').val() + '&facilityId=' + $('#Facility').val();
        if (url.indexOf('Find?') === -1) {
            url += append;
        } else {
            url = url.substring(0, url.indexOf('Find') + 4) + append;
        }
        $(selector).attr('href', url);
    }

    $(() => {
        AjaxTable.initialize('#functionalLocationTable');
        $(selector).on('click', onClick);
    });
})(jQuery);