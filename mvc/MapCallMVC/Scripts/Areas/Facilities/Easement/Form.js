var Easement = (function ($) {
    var ELEMENTS = {};
    var townStateServiceUrl;

    var eas = {
        init: function () {
            ELEMENTS = {
                coordinate: $('#Coordinate'),
                street: $('#Street'),
                streetNumber: $('#StreetNumber'),
                town: $('#Town')
            };
            townStateServiceUrl = $('#TownStateServiceUrl').val();
        },

        getAddress: function () {
            if (ELEMENTS.coordinate.val() || ELEMENTS.town.val() === '') {
                return null;
            }
            var street = ELEMENTS.streetNumber.val() + ' ' + ELEMENTS.street.find('option:selected').text();
            var townName = ELEMENTS.town.find('option:selected').text();
            var state;

            if (townName.length > 0) {
                $.ajax({
                    url: townStateServiceUrl,
                    data: {
                        id: ELEMENTS.town.val()
                    },
                    async: false, // So this just goes.
                    type: 'GET',
                    success: function (result) {
                        state = result.state;
                    },
                    error: function () {
                        alert("Something went wrong finding the state for the selected town.");
                    }
                });
            }
            return street + ' ' + townName + ', ' + state;
        }
    };
    $(document).ready(eas.init);
    return eas;
})(jQuery);