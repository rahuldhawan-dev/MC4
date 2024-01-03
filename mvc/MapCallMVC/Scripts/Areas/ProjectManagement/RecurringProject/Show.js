var RecurringProjectShow = {
	map: null,
	initialCoordinates: null,
	mainPicker: null,

	SELECTORS: {
		MAINS: 'img#mains',
		PICKER_DIV: 'div#mainPickerMap',
		DELETE_ENDORSEMENT_FORM: 'form#deleteEndorsementForm'
	},
	initialize: function() {
		RecurringProjectShow._initButtonAndForms(RecurringProjectShow.SELECTORS.DELETE_ENDORSEMENT_FORM, 'endorsement');
		$(RecurringProjectShow.SELECTORS.MAINS).click(RecurringProjectShow._main_click);
	},
	createMainPickerDiv: function() {
		var div = $('<div id="pickerDiv" class="jqmWindow">' +
      '<div class="jqmTitle">' +
      '<button class="jqmClose">Close X</button>' +
      '<span class="jqmTitleText">Mains</span>' +
      '</div>' +
      '<iframe id="pickerFrame" class="jqmContent" src="../../../RecurringProjectMain/Show/7329"></iframe>' +
      '</div>');
		$(document.body).append(div);
		return div;
	},
	closeMainPicker: function() {
		$('button.jqmClose')[0].click();
	},
	getMainPickerDiv: function() {
		var div = $(RecurringProjectShow.SELECTORS.PICKER_DIV);
		return div.length ? div : RecurringProjectShow.createMainPickerDiv();
	},
	_main_click: function() {
		RecurringProjectShow.mainPicker = RecurringProjectShow.getMainPickerDiv().jqm({ modal: true }).jqmShow();
	},
	_cancelFormButton_click: function(button, form) {
		$(button).click(function() {
			$(form).hide();
		});
	},
	_initButtonAndForms: function(removeForm, desc) {
		$(removeForm).submit(function() {
			return confirm('Are you sure you wish to delete the chosen ' + desc + ' record?');
		});
	}
};
$(document).ready(RecurringProjectShow.initialize);