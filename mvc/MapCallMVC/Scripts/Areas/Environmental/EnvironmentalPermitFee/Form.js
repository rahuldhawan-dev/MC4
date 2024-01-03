(($) => {
	const EnvironmentalPermitFee = {
		initialize: () => {
			$('#PaymentMethod').change(EnvironmentalPermitFee.onPaymentMethodChange);
			// Fire once to init the field visibility.
			EnvironmentalPermitFee.onPaymentMethodChange();
		},

		onPaymentMethodChange: () => {
			const selectedPaymentMethod = $('#PaymentMethod').val();
			Application.toggleField($('#PaymentMethodMailAddress'), selectedPaymentMethod === '1');
			Application.toggleField($('#PaymentMethodPhone'), selectedPaymentMethod === '2');
			Application.toggleField($('#PaymentMethodUrl'), selectedPaymentMethod === '3');
		}
	};

	$(EnvironmentalPermitFee.initialize);

})(jQuery);