var ValveNew = (function ($) {
	var val = {
		selectors: {
			IS_FOUND_VALVE: '#IsFoundValve',
			VALVE_SUFFIX: '#ValveSuffix',
			TOWN_SECTION: 'select#TownSection',
			TOWN_ABBREVIATION_TYPE: 'hid#AbbreviationType'
		},
		initialize: function() {
			val.initFoundValveChanged();
		},
		initFoundValveChanged: function() {
			$(val.selectors.IS_FOUND_VALVE).change(val.onFoundValveChanged);
			$(val.selectors.IS_FOUND_VALVE).change();
		},
		onFoundValveChanged: function () {
			var suffixParent = $(val.selectors.VALVE_SUFFIX).closest('.field-pair');
			if ($(val.selectors.IS_FOUND_VALVE).is(':checked')) {
				$(val.selectors.VALVE_SUFFIX).prop('disable', false);
				suffixParent.show();
			} else {
				$(val.selectors.VALVE_SUFFIX).prop('disable', true);
				suffixParent.hide();
			}
		}
	};

	$(document).ready(val.initialize);

	return val;
})(jQuery);