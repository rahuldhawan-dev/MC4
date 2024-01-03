var SewerOpeningShow = {
	selectors: {
		DELETE_CONNECTION_FORM: 'form#deleteSewerOpeningConnectionForm'
	},

	initialize: function() {
		$(SewerOpeningShow.selectors.DELETE_CONNECTION_FORM).submit(function() {
			return confirm('Are you sure you wish to delete the chosen connection?');
		});
	}
};

$(document).ready(SewerOpeningShow.initialize)