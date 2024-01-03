var WorkOrderLookup = (function($) {

	var wol = {
		setMessage: function (message) {
			$('#look-up-error').text(message);
		},
		setData: function(data) {
			$('#Address').val(data.location);
			$('#WorkOrder').val(data.workOrderId);
			$('#OperatingCenter').val(data.operatingCenterId);
			$('#TaskObserved').val(data.description);
			$('#Department').val(data.jobCategoryId);
			wol.setCoordinate(data.latitude, data.longitude);
		},
		setCoordinateId: function(coordinateId) {
			if (coordinateId != null) {
				$('#Coordinate').val(coordinateId);
				$(CoordinatePicker.SELECTORS.pickerIcon)[0].src = $(CoordinatePicker.SELECTORS.pickerIcon)[0].src.replace('red', 'blue');
			} else {
				$(CoordinatePicker.SELECTORS.pickerIcon)[0].src = $(CoordinatePicker.SELECTORS.pickerIcon)[0].src.replace('blue', 'red');
			}
		},
		setCoordinate: function(lat, lon) {
			$.ajax({
				url: $('#CoordinateCreateUrl').val(),
				async: false,
				type: 'POST',
				data: JSON.stringify({
					Latitude: lat, Longitude: lon, IconId: '5', ValueFor: 'Coordinate', CastedIconSet: 'All'
				}),
				contentType: 'application/json; charset=utf-8',
				success: function (json) {
					return wol.setCoordinateId(json.coordinateId);
				},
				error: function() {
					alert('Unable to create a coordinate from the work order.');
				}
			});
		},

		onBegin: function() {
			// Clear out the message.
			wol.setMessage('Searching...');
		},
		onSuccess: function(data) {
			if (!data.success) {
				wol.setMessage(data.message);
			} else {
				wol.setMessage('');
				wol.setData(data);
			}
		},
		onError: function() {
			wol.setMessage('An unexpected error has occurred. Please try again.');
		}
	};

	return wol;

})(jQuery);
