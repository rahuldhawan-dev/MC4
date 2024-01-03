var RoadwayImprovementNotification = (function($) {
	var ELEMENTS = {};
	var townStateServiceUrl;
	var rin = {
		init: function() {
			ELEMENTS = {
				coordinate: $('#Coordinate'),
				state: $('#State'),
				town: $('#Town')
			};
			townStateServiceUrl = $('#TownStateServiceUrl').val();
		},

		getAddress: function() {
			if (ELEMENTS.coordinate.val()) {
				return null;
			}
			var selectedTown = ELEMENTS.town.find('option:selected');
			var town = selectedTown.text();
			var state = ELEMENTS.state.val();

			if (!state && selectedTown.val()) {
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

			return town + ', ' + state;
		}
	};

	$(document).ready(rin.init);
	return rin;
})(jQuery);