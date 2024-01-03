var ChoosePaymentMethod = {
	selectors: {
		form: '#choosePaymentMethodForm',
		title: '#actionsTileTitle'
	},

	initialize: function () {
		ChoosePaymentMethod.setPageTitle();
		ChoosePaymentMethod.clearErrorMessage();
		ChoosePaymentMethod.setupValidation();
	},

	setPageTitle: function () {
		$(ChoosePaymentMethod.selectors.title).text('Choose Payment Method');
	},

	clearErrorMessage: function () {
		Application.clearErrorMessage();
	},

	setupValidation: function () {
		$.validator.unobtrusive.parseDynamicContent(ChoosePaymentMethod.selectors.form);
	}
};

$(document).ready(ChoosePaymentMethod.initialize);
