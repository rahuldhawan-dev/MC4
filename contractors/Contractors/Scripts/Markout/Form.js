var MarkoutForm = {
	MARKOUT_TYPE_NONE: 38,
	initialize: function () {
		this.toggleNote();
		$('#MarkoutType').change(this.toggleNote);
	},
	toggleNote: function () {
		return MarkoutForm.noteRow().toggle($('#MarkoutType').val() == MarkoutForm.MARKOUT_TYPE_NONE);
	},
	noteRow: function () {
		return $('#Note').parent().parent().parent();
	},
	validateMarkoutNumber: function (val) {
		var isMarkoutEditable = $('#WorkOrderOperatingCenterMarkoutEditable').val() === 'True';
		if (isMarkoutEditable) {
			// This already has a required field validator so we don't need to do anything here.
			return true;
		}

		// If markout is not editable, then the markout # must have a length of 9. No more, no less.
		return val.length === 9;
	}
};

$(document).ready(function () {
	MarkoutForm.initialize();
});
