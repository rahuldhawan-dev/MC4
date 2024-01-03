var ServiceLineProtectionInvestigationForm = (function($) {
	var ELEMENTS = {};

	var slpif = {
		init: function() {
			ELEMENTS = {
				coordinate: $('#Coordinate'),
				streetNumber: $('#StreetNumber'),
				street: $('#Street'),
				state: $('#State'),
				town: $('#CustomerCity'),
				operatingCenter: $('#OperatingCenter')
			};
			ELEMENTS.town.on('change', slpif.onTownChange);
		},
		onTownChange: function() {
			if (ELEMENTS.operatingCenter[0] !== null && ELEMENTS.operatingCenter[0].length === 2) {
				ELEMENTS.operatingCenter.val(ELEMENTS.operatingCenter[0][1].value);
			}
		},
		getAddress: function () {
			var address = ELEMENTS.streetNumber.val() + ' ';
			if (ELEMENTS.street.val() !== '') 
				address += ELEMENTS.street.find('option:selected').text();

			var town = (ELEMENTS.town.val() !== '') ? ELEMENTS.town.find('option:selected').text() : '';
			//remove everything after the comma
			town = town.substring(0, town.indexOf(','));
			var state = (ELEMENTS.state.val() !== '') ? ELEMENTS.state.find('option:selected').text() : '';

			return address + ' ' + town + ' ' + state;
		}
	}

	$(document).ready(slpif.init);

	return slpif;
})(jQuery);