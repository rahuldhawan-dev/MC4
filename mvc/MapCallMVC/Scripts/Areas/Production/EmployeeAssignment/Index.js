(function ($) {
	var getDefaultDate = function () {
		var today = new Date();
		var vars = [], hash;
		var q = document.URL.split('?')[1];
		if (q != undefined) {
			q = q.split('&');
			for (var i = 0; i < q.length; i++) {
				hash = q[i].split('=');
				vars.push(hash[1]);
				vars[hash[0]] = hash[1];
			}
		}

		if (vars['AssignedFor'] !== '') {
			return unescape(vars['AssignedFor']);
		}

		return dateToString(today);
	};

	var getCalendarUrl = function () {
		return window.top.location.href.replace('?', '/Index.cal?');
	};

	var eventRender = function (event, element) {
		if (event.description) {
			element.attr('title', event.description);
		}
	};

	const assignmentEndButton_Click = function (e) {
		const targetButton = e.target;
		const modal = $('#EmployeeAssignmentEndModal');
		// Use detach rather than remove. detach keeps all of the event handlers and
		// jQuery data associated with the elements. This means all of the initiated unobtrusive
		// controls and validation will continue to work.
		modal.detach();

		// Set the necessary data for the record.
		const dateStarted = targetButton.getAttribute('data-datestarted');
		modal.find('#Id').val(targetButton.getAttribute('data-assignmentid'));
		modal.find('#ProductionWorkOrder').val(targetButton.getAttribute('data-productionworkorder'));
		modal.find('#DateStarted').val(dateStarted);
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