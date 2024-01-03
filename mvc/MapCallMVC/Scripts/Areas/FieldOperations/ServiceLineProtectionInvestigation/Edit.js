var ServiceLineProtectionInvestigationEdit = (function () {
	var ELEMENTS = {};

	var slpi = {
		init: function() {
			ELEMENTS = {
				service: $('#Service'),
				dateInstalled: $('#DateInstalled'),
				companyServiceMaterial: $('#CompanyServiceMaterial'),
				companyServiceSize: $('#CompanyServiceSize')
			}

			ELEMENTS.service.change(slpi.onServiceChange);
		},

		onServiceChange: function() {
			var serviceId = ELEMENTS.service.val();
			if (serviceId == null || serviceId === '')
				return;
			$.ajax({
				url: '../../../FieldOperations/Service/Show/' + serviceId + '.json',
				async: false,
				type: 'GET',
				success: function(result) {
					if (result != null) {
						ELEMENTS.dateInstalled.val(result.DateInstalled);
						ELEMENTS.companyServiceMaterial.val(result.ServiceMaterial);
						ELEMENTS.companyServiceSize.val(result.ServiceSize);
					}
				}
			});
		}
	}

	$(document).ready(slpi.init);
	return slpi;
})();