var PaymentInformation = {
	selectors: {
		button: '#openPaymentInfo',
		token: AuthorizeNetPopup.selectors.token
	},

	initialize: function () {
	  $(PaymentInformation.selectors.button).click(PaymentInformation.getToken);
	},

	getToken: function () {
		$.ajax({
			type: 'POST',
			url: $(PaymentInformation.selectors.button).val(),
			success: PaymentInformation.paymentInfoPopup
		});
	},

	paymentInfoPopup: function (token) {
		$(PaymentInformation.selectors.token).val(token);
		AuthorizeNetPopup.openManagePopup();
	  $(PaymentInformation.selectors.button).prop('disabled', true).append(' (reload page to use again)');
	},

  onPopupClosed: function() {
  }
};

$(document).ready(PaymentInformation.initialize);