var WorkOrderScheduleOfValuesForm = {
	initialize: function () {
		WorkOrderScheduleOfValuesForm.initChangeEvents();

		// setup the form initially
		getServerElementById('ddlScheduleOfValue').change();
		getServerElementById('ddlScheduleOfValueCategory').change();
	},

	initChangeEvents: function() {
		getServerElementById('ddlScheduleOfValue').change(WorkOrderScheduleOfValuesForm.onOvertimeChange);
		getServerElementById('ddlScheduleOfValueCategory').change(WorkOrderScheduleOfValuesForm.onScheduleOfValueCategoryChange);

		$('[id*=ddlScheduleOfValueEdit').change(WorkOrderScheduleOfValuesForm.onOvertimeEditChange);
		$('[id*=ddlScheduleOfValueCategoryEdit').change(WorkOrderScheduleOfValuesForm.onScheduleOfValueCategoryEditChange);

		$('[id*=ddlScheduleOfValueEdit').change();
		$('[id*=ddlScheduleOfValueCategoryEdit').change();
	},

	handleUpdatePanelCallback: function() {
		WorkOrderScheduleOfValuesForm.initChangeEvents();
	},

	validateScheduleOfValue: function () {
		if (getServerElement('ddlScheduleOfValueCategory').val() === '') {
			alert('Please select a Schedule of Value Category');
			getServerElement('ddlScheduleOfValueCategory').focus();
			return false;
		}
		if (getServerElement('ddlScheduleOfValues').val() === 'Please select a category') {
			alert('Please select a Schedule of Value');
			getServerElement('ddlScheduleOfValues').focus();
			return false;
		}
		return true;
	},

	validateScheduleOfValueEdit: function () {
		if ($('select[name*=ValueCategoryEdit]').val() === '') {
			alert('Please select a Schedule of Value Category');
			$('select[name*=ValueCategoryEdit]').focus();
			return false;
		}
		if ($('select[name*=ScheduleOfValueEdit]').val() === 'Please select a category') {
			alert('Please select a Schedule of Value');
			$('select[name*=ScheduleOfValueEdit]').focus();
			return false;
		}
		return true;
	},

	hasScheduleOfValues: function () {
		return ($('span[id*=lblScheduleOfValueCategory]').length || $('select[name*=ValueCategoryEdit]').length);
	},

	onOvertimeChange: function () {
		if (getServerElementById('ddlScheduleOfValueCategory').val() === "21") {
			getServerElementById('chkIsOvertime').toggle(true);
			getServerElementById('chkIsOvertime').checked = true;
		}
		else {
			getServerElementById('chkIsOvertime').toggle(false);
		}
	},

	onOvertimeEditChange: function () {
		if ($('[id*=ddlScheduleOfValueCategoryEdit').val() === "21") {
			$('input[id*=chkIsOvertimeEdit]').toggle(true);
			$('input[id*=chkIsOvertimeEdit]').checked = true;
		}
		else {
			$('input[id*=chkIsOvertimeEdit]').toggle(false);
		}
	},

	onScheduleOfValueCategoryChange: function() {
		if (['27', '28'].indexOf($('[id*=ddlScheduleOfValueCategory').val()) > -1)
			$('[id*=txtOtherDescription').toggle(true);
		else
			$('[id*=txtOtherDescription').toggle(false);
	},

	onScheduleOfValueCategoryEditChange: function () {
		if (['27', '28'].indexOf($('[id*=ddlScheduleOfValueCategoryEdit').val()) > -1)
			$('[id*=txtOtherDescriptionEdit').toggle(true);
		else
			$('[id*=txtOtherDescriptionEdit').toggle(false);
	}
};