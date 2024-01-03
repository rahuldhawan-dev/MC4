var Streets = (function($) {
	var ELEMENTS = {};
	var townStateServiceUrl;
	var s = {
		init: function() {
			ELEMENTS = {
				coordinate: $('#Coordinate'),
				town: $('#TownId'),
				townText: $('#TownText'),
				state: $('#State'),
				street: $('#Street')
			};
			townStateServiceUrl = $('#TownStateServiceUrl').val();
		},

		getAddress: function() {
			if (ELEMENTS.coordinate.val())
				return null;

			var selectedTown = ELEMENTS.town.val();
			var state = ELEMENTS.state.val();

			if (!state && selectedTown) {
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

			var selectedStreet = ELEMENTS.street.find('option:selected').text() + ' ';
			if (selectedStreet == '-- Select -- ')
				selectedStreet = '';

			return selectedStreet + ' ' + ELEMENTS.townText.val() + ', ' + state;
		}

	};

	$(document).ready(s.init);
	return s;
})(jQuery);