var SewerOverflows = (function () {
    var ELEMENTS = {};
	var SELECTORS = {};
	var DISCHARGE_LOCATION_OPTIONS = {};
    var townStateServiceUrl;
	
	var so = {
		init: function() {
            ELEMENTS = {
                coordinate: $('#Coordinate'),
                street: $('#Street'),
                town: $('#Town'),
                operatingCenter: $('#OperatingCenter'),
                bodyOfWater: $('#BodyOfWater'),
                gallonsFlowedIntoBodyOfWater: $('#GallonsFlowedIntoBodyOfWater'),
                maximumOverflowGallons: $('#MaximumOverflowGallons'),
                enforcingAgencyCaseNumber: $('#EnforcingAgencyCaseNumber'),
                criticalNotes: $('#CriticalNotes'),
				dischargeLocation: $('#DischargeLocation'),
            };
			
			SELECTORS = {
				gallonsFlowedIntoBodyOfWater: '#GallonsFlowedIntoBodyOfWater',
			};

			DISCHARGE_LOCATION_OPTIONS = {
				bodyOfWater: 4,
				other: 5
			};
			
            townStateServiceUrl = $('#TownStateServiceUrl').val();
			ELEMENTS.dischargeLocation.change(so.onDischargeLocationChange);
			so.onDischargeLocationChange();
        },

        getAddress: function() {
            // Okay so if there's a coordinate already selected, autopopulating the address
            // and searching will cause the existing coordinate to not be displayed. 
            if (ELEMENTS.coordinate.val()) {
                return null;
            }
            // townName element only exists on edit view.
            var selectedTown = ELEMENTS.town.find('option:selected');
            var town = selectedTown.text();
            var street = ELEMENTS.street.find('option:selected').text();
            var state;

            // We need state from somewhere or else this doesn't work out very well.
            if (selectedTown.val()) {
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

            return street + ', ' + town + ' ' + state;
        },
        
		onDischargeLocationChange: function () {
			if (ELEMENTS.dischargeLocation.val() == DISCHARGE_LOCATION_OPTIONS.bodyOfWater || ELEMENTS.dischargeLocation.val() == DISCHARGE_LOCATION_OPTIONS.other) {
				Application.toggleField(SELECTORS.gallonsFlowedIntoBodyOfWater, true);
			} else {
				Application.toggleField(SELECTORS.gallonsFlowedIntoBodyOfWater, false);
			}
		},

		validateEnforcingAgencyCaseNumber: function () {
			if (ELEMENTS.maximumOverflowGallons.val() == '')
				return true;

			if (ELEMENTS.maximumOverflowGallons.val() < ELEMENTS.gallonsFlowedIntoBodyOfWater.val())
				return (ELEMENTS.enforcingAgencyCaseNumber.val() != '');

			return true;
		}
	}

	$(document).ready(so.init);
	return so;
})();