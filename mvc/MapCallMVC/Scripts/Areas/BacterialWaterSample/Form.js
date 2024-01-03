var BacterialWaterSample = (function ($) {
	var ELEMENTS = {};
	var townStateServiceUrl;

	var toggleFieldPair = function($element, shouldToggle) {
		$element.closest('.field-pair').toggle(shouldToggle);
	};

	var bacterialWaterSample = {
		init: function () {
			ELEMENTS = {
				coordinate: $('#SampleCoordinate'),
				address: $('#Address'),
				town: $('#SampleTown'),
				sampleType: $('#BacterialSampleType'),
				repeatLocationType: $('#RepeatLocationType'),
				coliformConfirm: $('#ColiformConfirm'),
				colonyCounts: $('#colony-counts'),
				originalBacterialWaterSample: $('#OriginalBacterialWaterSample'),
				estimatingCapitalMainProject: $('#EstimatingProject'),
				sapWorkOrderId: $('#SAPWorkOrderId'),
				isInvalid: $('#IsInvalid'),
				reasonForInvalidation: $('#ReasonForInvalidation')
			};
			townStateServiceUrl = $('#TownStateServiceUrl').val();
			ELEMENTS.sampleType.on('change', bacterialWaterSample.onSampleTypeChange);
			ELEMENTS.coliformConfirm.on('change', bacterialWaterSample.onColiformConfirmChange);
			ELEMENTS.isInvalid.on('change', bacterialWaterSample.onIsInvalidChanged);

			// Need to do an initial fire of these for the toggling.
			bacterialWaterSample.onColiformConfirmChange();
			bacterialWaterSample.onSampleTypeChange();
			bacterialWaterSample.onIsInvalidChanged();
		},

		getAddress: function () {
			if (ELEMENTS.coordinate.val()) {
				return null;
			}
			var address = ELEMENTS.address.val();
			var selectedTown = ELEMENTS.town.find('option:selected');
			if (!selectedTown.val()) {
				return null; // This just errors if the there isn't a town selected.
			}
			var town = selectedTown.text();
			var state = null;

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

			return address + ' ' + town + ' ' + state;
		},

		onIsInvalidChanged: function () {
			toggleFieldPair(ELEMENTS.reasonForInvalidation, ELEMENTS.isInvalid.is(':checked'));
		},

		onSampleTypeChange: function () {
			var selectedSampleType = ELEMENTS.sampleType.find('option:selected').text();

			var isRepeat = selectedSampleType === 'Repeat';
			toggleFieldPair(ELEMENTS.repeatLocationType, isRepeat);
			toggleFieldPair(ELEMENTS.originalBacterialWaterSample, isRepeat);

			var isNewMain = selectedSampleType === 'New Main';
			toggleFieldPair(ELEMENTS.estimatingCapitalMainProject, isNewMain);

			var isSystemRepair = selectedSampleType === 'System Repair';
			toggleFieldPair(ELEMENTS.sapWorkOrderId, isSystemRepair);
		},

		onColiformConfirmChange: function () {
			// This used to be a dropdown
			//ELEMENTS.colonyCounts.toggle(ELEMENTS.coliformConfirm.find('option:selected').val() === 'True');
			ELEMENTS.colonyCounts.toggle(ELEMENTS.coliformConfirm.is(':checked'));
		}
	};

	$(document).ready(bacterialWaterSample.init);

	return bacterialWaterSample;
})(jQuery);
