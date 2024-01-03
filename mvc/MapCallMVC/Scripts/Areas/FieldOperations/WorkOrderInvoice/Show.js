var WorkOrderInvoiceShow = {
	selectors: {
		DELETE_SCHEDULE_OF_VALUE_FORM: 'form#deleteScheduleOfValueForm',
		SCHEDULE_OF_VALUE_FORM: 'form#scheduleOfValuesForm',
		SCHEDULE_VALUE_CATEGORY: '#ScheduleOfValueCategory',
		IS_OVERTIME: '#IsOvertime',
		LABOR_UNIT_COST: '#LaborUnitCost',
		OTHER_DESCRIPTION: '#OtherDescription'
	},
	categories: {
		LABOR: '21',
		OTHER: ['27', '28']
	},

	initialize: function () {
		WorkOrderInvoiceShow._initButtonAndForms(
			WorkOrderInvoiceShow.selectors.DELETE_SCHEDULE_OF_VALUE_FORM,
			'schedule of value');
		$(WorkOrderInvoiceShow.selectors.SCHEDULE_VALUE_CATEGORY).change(WorkOrderInvoiceShow.scheduleOfValueCategoryOnChange);
		WorkOrderInvoiceShow.scheduleOfValueCategoryOnChange();
	},
	
	_initButtonAndForms: function(removeForm, desc) {
		$(removeForm)
			.submit(function() {
				return confirm('Are you sure you wish to delete the chosen ' + desc + ' record?');
			});
	},

	scheduleOfValueCategoryOnChange: function() {
		var isOvertimeVisible = $(WorkOrderInvoiceShow.selectors.SCHEDULE_VALUE_CATEGORY).val() === WorkOrderInvoiceShow.categories.LABOR;
		$(WorkOrderInvoiceShow.selectors.IS_OVERTIME).closest('.field-pair').toggle(isOvertimeVisible);
		if (!isOvertimeVisible) {
				$(WorkOrderInvoiceShow.selectors.IS_OVERTIME).val('');
		}

		var isOtherVisible = WorkOrderInvoiceShow.categories.OTHER.indexOf($(WorkOrderInvoiceShow.selectors.SCHEDULE_VALUE_CATEGORY).val()) > -1;
		$(WorkOrderInvoiceShow.selectors.LABOR_UNIT_COST).closest('.field-pair').toggle(isOtherVisible);
		$(WorkOrderInvoiceShow.selectors.OTHER_DESCRIPTION).closest('.field-pair').toggle(isOtherVisible);
		if (!isOtherVisible) {
			$(WorkOrderInvoiceShow.selectors.OTHER_DESCRIPTION).val('');
			$(WorkOrderInvoiceShow.selectors.LABOR_UNIT_COST).val('');
		}
	}
};

$(document).ready(WorkOrderInvoiceShow.initialize);
