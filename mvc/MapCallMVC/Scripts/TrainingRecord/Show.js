var TrainingRecordShow = {
	selectors: {
		DELETE_TRAINING_SESSION_FORM: "form#deleteTrainingSession"
	},
	initialize: function() {
		$(TrainingRecordShow.selectors.DELETE_TRAINING_SESSION_FORM).submit(function() {
			return confirm('Are you sure you wish to remove the Training Session?');
		});
    // hacky way to hide employees schedule send notification button
	  $('div[id*="EmployeesAttended"] form[action="/Notification/Create"]').toggle(false);
	}
};

$(document).ready(TrainingRecordShow.initialize);