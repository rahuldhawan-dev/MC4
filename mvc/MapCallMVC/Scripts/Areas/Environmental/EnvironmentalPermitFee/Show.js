(($) => {
	const EnvironmentalPermitFee = {
		initialize: () => {
			$('.delete-fee').click(EnvironmentalPermitFee.deleteFee);
		},

		deleteFee: (e) => {
			if (confirm("Are you sure you want to delete this fee?")) {
				const form = $('#delete-fee-form');
				form.find('[name=EnvironmentalPermitFeeId]').val(e.target.value);
				form.submit();
			}
			return false;
		}
	};

	$(EnvironmentalPermitFee.initialize);
})(jQuery);