var JobSiteCheckList = (function () {
	var setDateOnce = false;

	var jscl = {
		initialize: function() {
			$('#SupervisorSignOffEmployee').change(jscl.onSupervisorSignOffChanged);
			$('#auto-populate-button').click(jscl.onAutoPopulate);

			$('#PressurizedRiskRestrainedType').change(jscl.onPressurizedRiskRestrainedTypeChanged);
			// and initialize once
			jscl.onPressurizedRiskRestrainedTypeChanged();

			$('#IsLongHydrantInstallation').change(jscl.onIsLongHydrantInstallationChanged);
			// init once.
			jscl.onIsLongHydrantInstallationChanged();

			$('#HasExcavation').change(jscl.onHasExcavationChange);
			// init once for page load.
			jscl.onHasExcavationChange();
		},

		onAutoPopulate: function () {
			var sapWorkOrderNumber = $('#SAPWorkOrderId').val();
			if (!sapWorkOrderNumber) {
				return;
			}
			var autoPopMessage = function(msg) {
				$('#auto-populate-message').html(msg);
			};

			var url = $('#by-sap-workorder-number-url').val();
			$.ajax({
				url: url,
				data: {
					sapWorkOrderNumber: sapWorkOrderNumber
				},
				async: false,
				type: 'POST',
				success: function (workOrder) {
					if (!workOrder.success) {
						autoPopMessage('Unable to populate data from the server.');
					} else {
						autoPopMessage('');
						$('#OperatingCenter').val(workOrder.operatingCenterId).change();
						$('#MapCallWorkOrder').val(workOrder.workOrderId);
						$('#Address').val(workOrder.address);
					}
				},
				error: function () {
					autoPopMessage('Unable to populate data from the server.');
				}
			});
		},

		onHasExcavationChange: function() {
			var showTab = $('#HasExcavation option:selected').val() === 'True';
			// enable and disable don't actually hide the tab content if the tab happens to be open.
			// You can see this happen if the tab remembering thing is enabled for the page and the
			// remembered tab is displayed but also disabled.
			$('div.tabs-container').tabs(showTab ? 'enable' : 'disable', '#ExcavationsTab');
			$('a[href="#ExcavationsTab"]').parent().toggle(showTab);
			if (!showTab) {
				// This needs to be fixed on a deeper level at some point. 
				// The ajaxtabs.js tab remembering kinda breaks here if the Excavations tab was loaded previously but 
				// needs to be disabled 
				// 1. Page loads, tab remembering script initializes and switches to the Excavations tab
				// 2. This page's init script runs this method and disables the Excavations tab
				// 3. Now we have the Excavation tab body visible but no tab. 

				// So for right now, we're just forcing the tab to the tab that holds the HasExcavation field
				// if the Excavations tab is active but needs to be disabled.
				// ANNOYING NOTE: As of writing, there is no way of setting the active tab by name, only index.
				// Yet the enable/disable works with that, it's really stupid. Also the active property only
				// returns the index, not the name or element.
				var excavationsTabIndex = $('div.tabs-container .ui-tabs-nav a[href="#ExcavationsTab"]').parent().index();
				if ($('div.tabs-container').tabs('option', 'active') === excavationsTabIndex) {
					$('div.tabs-container').tabs('option', 'active', 0);
				}
			}
		},

		onIsLongHydrantInstallationChanged: function() {
			var isLongHydrantInstallation = $('#IsLongHydrantInstallation option:selected').val() === 'True';
			$('.fp-long-hydrant-installation-calculation').toggle(isLongHydrantInstallation);
		},

		onPressurizedRiskRestrainedTypeChanged: function () {
			var selected = $('#PressurizedRiskRestrainedType option:selected').text();
			$('.fp-no-restraint-reason').toggle(selected === 'No');
			$('.fp-restraint-method').toggle(selected === 'Yes');
		},

		onSupervisorSignOffChanged: function () {
			var sod = $('#SupervisorSignOffDate');
			if (setDateOnce || sod.val() || !$('#SupervisorSignOffEmployee').val()) {
				return;
			}
			setDateOnce = true;

			$.unobtrusiveDatePicker.initDatePicker(sod);
			// Wanna set the date to today if one isn't already set.
			sod.datepicker('setDate', new Date());
		},

		validateHasTrafficControl: function (value, element) {
			if (value !== 'True' && value !== 'true') {
				// It's valid because "Yes" is not selected.
				return true;
			}

			var isChecked = function (fieldId) {
				var field = $('#' + fieldId);
				if (field.length === 0) {
					throw "This field does not exist. It should exist. " + fieldId;
				}

				return field.is(':checked');
			};

			var barricade = isChecked('HasBarricadesForTrafficControl');
			var cones = isChecked('HasConesForTrafficControl');
			var flagPerson = isChecked('HasFlagPersonForTrafficControl');
			var police = isChecked('HasPoliceForTrafficControl');
			var signs = isChecked('HasSignsForTrafficControl');

			return barricade || cones || flagPerson || police || signs;
		},

		validateFourFeetDeep: function (value, element) {
			if ($('#HasExcavationFiveFeetOrDeeper').is(':checked') && !$(element).is(':checked')) {
				return false;
			}
			return true;
		},

		validateProtectionTypes: function (value, element) {
			if ($('#HasExcavationFiveFeetOrDeeper').is(':checked')) {
				var checked = ($('#ProtectionTypes :checked').length > 0);
				if (!checked) {
					return false;
				}
			}
			return true;
		}
	}

	$(document).ready(jscl.initialize);

	return jscl;
})();

// Excavation table stuff
(function () {
	$(document).ready(function () {
		var excavationsTable = $('#excavation-details-table');
		var tfootEmpty = excavationsTable.find('tfoot');
		var addExcavationRowButton = $('#add-excavation');
		var createExcavationButton = $('#create-excavation');
		var newExcavationRow = excavationsTable.find('tr.new-excavation-row');
		var cancelExcavationButton = $('#cancel-excavation');

		var Excavations = {
			_addRowHidden: false,
			init: function () {
				newExcavationRow.detach();
				Excavations.hideAddRow();
				addExcavationRowButton.click(function () {
					// Need to pass null to showAddRow, not the event.
					Excavations.showAddRow();
				});
				cancelExcavationButton.click(Excavations.hideAddRow);
				createExcavationButton.click(Excavations.createExcavation);

				$(document).on('click', '.edit-excavation', function (e) {
					Excavations.editExcavation($(e.target).closest('tr'));
				});
			},
			hideAddRow: function () {
				tfootEmpty.detach();
				cancelExcavationButton.hide();
				Excavations._addRowHidden = false;
			},
			showAddRow: function (editRow) {
				if (!Excavations._addRowHidden) {
					tfootEmpty.data('editRow', editRow);
					excavationsTable.append(tfootEmpty);
					cancelExcavationButton.show();
					Excavations._addRowHidden = true;
				}
			},
			createExcavation: function () {
				var isValid = true;

				// Start the cloning early so we don't have to loop through this twice.
				var newRow = newExcavationRow.clone();

				tfootEmpty.find('input, select').each(function () {
					var $el = $(this);
					if ($el.valid()) {
						// We need the selected option's text if it's a select tag, otherwise textbox value.
						var displayValue = $el.find('option:selected').html() || $el.val();
						var name = $el.attr('name');
						var dataCell = newRow.find('[data-for="' + name + '"]');
						dataCell.html(displayValue);
						dataCell.removeAttr('data-for');
						var hidden = $('<input type="hidden" />');
						var unprefixedName = name.replace('editorModel.', '');
						hidden.attr('name', 'Excavations[0].' + unprefixedName);
						hidden.attr('value', $el.val());
						dataCell.append(hidden);
					}
					else {
						isValid = false;
					}
				});
				if (isValid) {
					newRow.find('.edit-excavation').on('click', function () {
						Excavations.editExcavation(newRow);
					});

					var editingRow = tfootEmpty.data('editRow');
					if (editingRow) {
						editingRow.replaceWith(newRow);
						tfootEmpty.data('editRow', null);
					} else {
						excavationsTable.find('tbody').append(newRow);
					}

					Excavations.hideAddRow();
					Excavations.reindexFields();
					addExcavationRowButton.html("Add Excavation");
				}
			},
			editExcavation: function (row) {
				row.find('input[type="hidden"]').each(function () {
					var fieldMatch = this.name.split('.');
					fieldMatch = fieldMatch[fieldMatch.length - 1];
					var field = tfootEmpty.find('input[name$=' + fieldMatch + ']');
					field.val($(this).val());
					field = tfootEmpty.find('select[name$=' + fieldMatch + ']');
					field.val($(this).val());
				});

				Excavations.showAddRow(row);
			},
			reindexFields: function () {
				// In order for the model binder to work with lists correctly, we need
				// to reset the index numbers in each row so that they always go 0, 1, 2, etc.
				excavationsTable.find('tbody tr').each(function (i, el) {
					$(el).find('input:hidden').each(function () {
						var hidden = $(this);
						var hiddenName = hidden.attr('name');
						hidden.attr('name', function () {
							return hiddenName.replace(/\[(\d+)\]/, '[' + i + ']');
						});
					});
				});
			}
		};

		Excavations.init();
	});
})();

