var VerifyPaymentSummary = {
	selectors: {
		title: '#actionsTileTitle'
	},

	initialize: function () {
		VerifyPaymentSummary.setPageTitle();
		VerifyPaymentSummary.clearErrorMessage();
	},

	setPageTitle: function () {
		$(VerifyPaymentSummary.selectors.title).text('Payment Summary');
	},

	clearErrorMessage: function () {
		Application.clearErrorMessage();
	}
};

$(document).ready(VerifyPaymentSummary.initialize);
