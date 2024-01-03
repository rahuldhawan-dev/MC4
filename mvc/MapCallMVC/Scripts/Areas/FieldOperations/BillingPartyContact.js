var BillingPartyContact = (function($) {
	var bpc = {
		ELEMENTS: null,
		initialize: function() {
			bpc.ELEMENTS = {
				addContactButton: $('#addContactButton'),
				addContactWrap: $('#addContactFormWrap'),
				cancelButton: $('#cancelAddContactButton'),
				contactsTable: $('#contactsTable')
			}
			bpc.initializeTable();
			bpc.initializeAddTownContactSection();
		},

		initializeTable: function() {
			bpc.ELEMENTS.contactsTable.find('form').on('submit', function (ev) {
				if (!confirm("Are you sure you want to remove this contact?")) {
					ev.preventDefault();
					return false;
				}
				return true;
			});
		},

		initializeAddTownContactSection: function() {
			bpc.ELEMENTS.addContactWrap.hideAndSavePosition();
			bpc.ELEMENTS.addContactButton.click(bpc.onAddContactButtonClick);
			bpc.ELEMENTS.cancelButton.click(bpc.onCancelButtonClick);
		},

		onAddContactButtonClick: function() {
			bpc.ELEMENTS.addContactWrap.reattachAtSavedPosition();
			bpc.ELEMENTS.addContactButton.hideAndSavePosition();
		},

		onCancelButtonClick: function() {
			bpc.ELEMENTS.addContactWrap.hideAndSavePosition();
			bpc.ELEMENTS.addContactButton.reattachAtSavedPosition();
		}
	};

	$(document).ready(bpc.initialize);
	return bpc;
})(jQuery);