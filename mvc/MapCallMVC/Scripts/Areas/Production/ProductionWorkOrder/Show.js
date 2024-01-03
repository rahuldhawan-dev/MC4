(function ($) {
	const assignmentEndButton_Click = function (e) {
		const targetButton = e.target;
		const modal = $("#EmployeeAssignmentEndModal");
		// Use detach rather than remove. detach keeps all of the event handlers and
		// jQuery data associated with the elements. This means all of the initiated unobtrusive
		// controls and validation will continue to work.
		modal.detach();

		// Set the necessary data for the record.
		const dateStarted = targetButton.getAttribute('data-datestarted');
		modal.find('#Id').val(targetButton.getAttribute('data-assignmentid'));
		modal.find('#ProductionWorkOrder').val(targetButton.getAttribute('data-productionworkorder'));
		modal.find('#DateStarted').val(dateStarted);
		modal.find('.fp-date-started .field > div').text(dateStarted);
		$(document.body).append(modal);
		// If you pass modal: true to this, dropdowns inside datepickers stop working.
		// This is a known problem with jqModal. This also makes the dialog disappear
		// when clicking the overlay.
		modal.jqm({ modal: false }).jqmShow();
	};

	$(function () {
		$('[name="EmployeeAssignmentEndButton"]').click(assignmentEndButton_Click);
	});
})(jQuery);

(function (global) {

    var removeDisabledAttrFromAsLeftAsFoundElement = function (id, selector) {
        $(AsLeftAsFoundEdit.SELECTORS.SAVE_FORM + id + ' ' + selector).removeAttr('disabled');
    }

    global.AsLeftAsFoundEdit = {
        SELECTORS: {
            ASFOUNDCONDITION_DROPDOWN: '#AsFoundCondition',
            ASFOUNDCONDITIONREASON_DROPDOWN: '#AsFoundConditionReason',
            ASFOUNDCONDITIONCOMMENT_TEXTBOX: '#AsFoundConditionComment',
            ASLEFTCONDITION_DROPDOWN: '#AsLeftCondition',
            ASLEFTCONDITIONREASON_DROPDOWN: '#AsLeftConditionReason',
            ASLEFTCONDITIONCOMMENT_TEXTBOX: '#AsLeftConditionComment',
            REPAIRCOMMENT_TEXTBOX: '#RepairComment',
            EDIT_BUTTON: '#AsLeftAsFoundEditButton',
            SAVE_BUTTON: '#AsLeftAsFoundSaveButton',
            SAVE_FORM: '#productionWorkOrderEquipmentConditionForm'
        },

        initialize: function (id) {
            AsLeftAsFoundEdit.initEvents(id);
        },

        tryEnable: function (id) {
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASFOUNDCONDITION_DROPDOWN);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASFOUNDCONDITIONREASON_DROPDOWN);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASFOUNDCONDITIONCOMMENT_TEXTBOX);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASLEFTCONDITION_DROPDOWN);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASLEFTCONDITIONREASON_DROPDOWN);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.ASLEFTCONDITIONCOMMENT_TEXTBOX);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.REPAIRCOMMENT_TEXTBOX);
            removeDisabledAttrFromAsLeftAsFoundElement(id, AsLeftAsFoundEdit.SELECTORS.SAVE_BUTTON);
        },

        initEvents: function (id) {
            $(AsLeftAsFoundEdit.SELECTORS.SAVE_FORM + id + ' ' + AsLeftAsFoundEdit.SELECTORS.EDIT_BUTTON).click(id, AsLeftAsFoundEdit.onEdit);
        },

        onEdit: function (e) {
            AsLeftAsFoundEdit.tryEnable(e.data);
        }
    };
})(this);