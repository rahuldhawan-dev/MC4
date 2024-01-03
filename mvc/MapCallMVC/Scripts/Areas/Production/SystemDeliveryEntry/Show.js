var SystemDeliveryEntryShow = (function ($) {

    var indexActionBarButton = {
        _storage: UserStorage.getPluginContainer('actionBar'),
        
        init: function () {
            //move index button
            $('.ab-index').insertAfter('.ab-search');

            //add the search query params
            var lastSearch = indexActionBarButton._storage.get('lastSearch');
            if (lastSearch && lastSearch.query && lastSearch.route.controller === 'SystemDeliveryFacilityEntry' && lastSearch.route.area === 'Production') {
                $('.ab-index').each(function (i, el) {
                    el.href = el.href + '?' + lastSearch.query;
                });
            }
        },
    };

    $(document).ready(indexActionBarButton.init);

})(jQuery)