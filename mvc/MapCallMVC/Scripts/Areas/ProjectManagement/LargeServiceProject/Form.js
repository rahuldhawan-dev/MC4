var LargeServiceProject = (function () {
	var ELEMENTS = {};
	var townStateServiceUrl;

	var lsp = {
		init: function() {
			ELEMENTS = {
				coordinate: $('#Coordinate'),
        address: $('#ProjectAddress'),
        operatingCenter: $('#OperatingCenter'),
				town: $('#Town'),
				townName: $('#TownName'),
				state: $('#State')
			};
			townStateServiceUrl = $('#TownStateServiceUrl').val();
      AjaxTable.initialize('#wbsElementTable');
      ELEMENTS.operatingCenter.on('change', lsp.setFindLinkParameters);
		  lsp.setFindLinkParameters();
		},

		setFindLinkParameters: function () {
		  $('#WBSElementFindLink').attr('href', $('#WBSElementFindUrl').val() + '?operatingCenterId=' + ELEMENTS.operatingCenter.val());
    },

		getAddress: function() {
			if (ELEMENTS.coordinate.val()) {
				return null;
			}

			var selectedTown = ELEMENTS.town.find('option:selected');
			var town = ELEMENTS.townName.val() || selectedTown.text();
			var address = ELEMENTS.address.val();
			var state = ELEMENTS.state.val();

			if (!state && selectedTown.val()) {
				$.ajax({
					url: townStateServiceUrl,
					data: { id: ELEMENTS.town.val() },
					async: false,
					type: 'GET',
					success: function (result) { state = result.state; },
					error: function () { alert('Something went wrong finding the state for the selected town.'); }
				});
			}
			return address + ', ' + town + ' ' + state;
		}
	};

	$(document).ready(lsp.init);

	return lsp;
})();