var SuccesfulPayment = {
	initialize: function () {
		Application.onJQueryAjaxStart();
		SuccesfulPayment.setPageTitle();
		$('#notifications').hide();
		Application.onJQueryAjaxStop();
	},

	setPageTitle: function () {
		Application.setPageTitle('Payment Successful!');
	}
};

$(document).ready(SuccesfulPayment.initialize);
