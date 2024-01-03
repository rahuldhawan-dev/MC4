var TrainingRequirement = (function($) {
	var tr = {
		selectors: {
			IS_INITIAL_REQUIREMENT: 'select#IsInitialRequirement',
			RECURRING_TRAINING_FREQUENCY_DAYS: 'input#TrainingFrequency_Frequency',
			DDL_INITIAL_TRAINING_MODULE: 'select#ActiveInitialTrainingModule',
			DDL_RECURRING_TRAINING_MODULE: 'select#ActiveRecurringTrainingModule',
			DDL_INITIAL_AND_RECURRING_TRAINING_MODULE: 'select#ActiveInitialAndRecurringTrainingModule',
			HAS_COMBINED_TRAINING_MODULE: 'input#HasCombinedTrainingModule'
		},

		initialize: function () {
			$(tr.selectors.IS_INITIAL_REQUIREMENT).change(tr.toggleTrainingModules);
			$(tr.selectors.RECURRING_TRAINING_FREQUENCY_DAYS).change(tr.toggleTrainingModules);
			$(tr.selectors.HAS_COMBINED_TRAINING_MODULE).change(tr.toggleCombined);
			tr.toggleCombined();
			tr.toggleTrainingModules();
		},

		toggleTrainingModules: function() {
			// if recurring days > 0 then display both "recurring" and the "initial and recurring" sections
			if ($(tr.selectors.RECURRING_TRAINING_FREQUENCY_DAYS).val() > 0) {
				//tr.toggleFieldRow($(tr.selectors.DDL_RECURRING_TRAINING_MODULE), true);
				//tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_AND_RECURRING_TRAINING_MODULE), true);
				tr.toggleFieldRow($(tr.selectors.HAS_COMBINED_TRAINING_MODULE), true);
				tr.toggleCombined();
			} else {
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_TRAINING_MODULE), true);
				tr.toggleFieldRow($(tr.selectors.HAS_COMBINED_TRAINING_MODULE), false);
				tr.toggleFieldRow($(tr.selectors.DDL_RECURRING_TRAINING_MODULE), false);
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_AND_RECURRING_TRAINING_MODULE), false);
				tr.toggleFieldRow($(tr.selectors.HAS_COMBINED_TRAINING_MODULE), false);
			}
		},

		toggleCombined: function() {
			if ($(tr.selectors.HAS_COMBINED_TRAINING_MODULE).is(':checked')) {
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_AND_RECURRING_TRAINING_MODULE), true);
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_TRAINING_MODULE), false);
				$(tr.selectors.DDL_INITIAL_TRAINING_MODULE).val(null);
				tr.toggleFieldRow($(tr.selectors.DDL_RECURRING_TRAINING_MODULE), false);
				$(tr.selectors.DDL_RECURRING_TRAINING_MODULE).val(null);
			} else {
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_AND_RECURRING_TRAINING_MODULE), false);
				$(tr.selectors.DDL_INITIAL_AND_RECURRING_TRAINING_MODULE).val(null);
				tr.toggleFieldRow($(tr.selectors.DDL_INITIAL_TRAINING_MODULE), true);
				tr.toggleFieldRow($(tr.selectors.DDL_RECURRING_TRAINING_MODULE), true);
			}
		},

		toggleFieldRow: function(field, visible) {
			field.parent().parent().parent().toggle(visible);
		}

	}

	$(document).ready(tr.initialize());
})(jQuery);