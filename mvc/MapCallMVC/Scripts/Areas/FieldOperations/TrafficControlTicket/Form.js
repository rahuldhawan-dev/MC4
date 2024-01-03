var TrafficControlTicket = (function($) {
	var ELEMENTS = {};
	var townStateServiceUrl, workOrderAccountingCodeUrl;
	var tct = {
		init: function() {
			ELEMENTS = {
				accountingCode: $('#AccountingCode'),
				coordinate: $('#Coordinate'),
				coordinateImage: $('.coordinate-display-icon'),
				coordinatePicker: $('.coordinate-picker-icon'),
				state: $('#State'),
				town: $('#Town'),
				street: $('#Street'),
				streetNumber: $('#StreetNumber'),
				updatedAccountingCodeButton: $('#UpdatedAccountingCode'),
				workOrder: $('#WorkOrder')
			};
			townStateServiceUrl = $('#TownStateServiceUrl').val();
			workOrderAccountingCodeUrl = $('#WorkOrderAccountingCodeUrl').val();
			ELEMENTS.updatedAccountingCodeButton.click(tct.getAccountingCode);
			tct.setCoordinateGlobe();
		},

		// if the ticket is prepopulated, lets make sure we set the correct icon for the picker.
		setCoordinateGlobe: function () {
			if ($(ELEMENTS.coordinate).val() != "" && $(ELEMENTS.coordinateImage).length > 0)
				$(ELEMENTS.coordinateImage).attr('src', $(ELEMENTS.coordinateImage).attr('src').replace('red', 'blue'));
		},

		getAccountingCode: function() {
			$.ajax({
				url: workOrderAccountingCodeUrl,
				data: { id: ELEMENTS.workOrder.val() },
				async: false,
				type: 'GET',
				success: function(result) {
					ELEMENTS.accountingCode.val(result.accountingCode);
				},
				error: function() {
					alert('Something went wrong finding the accounting code for the selected work order.');
				}
			});
		},

		getAddress: function () {
			if (ELEMENTS.coordinate.val()) {
				return null;
			}
			var streetNumber = ELEMENTS.streetNumber.val();
			var street = ELEMENTS.street.find('option:selected').text();
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
			return streetNumber + ' ' + street + ' ' + town + ',' + state;
		}
	};
	$(document).ready(tct.init);
	return tct;
})(jQuery);