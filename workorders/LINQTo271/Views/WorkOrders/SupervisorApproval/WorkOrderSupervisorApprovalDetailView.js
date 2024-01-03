var WorkOrderSupervisorApprovalDetailView = {
	validateScheduleOfValues: function () {
		if (getServerElementById('hidOperatingCenterHasWorkOrderInvoicing') && !WorkOrderScheduleOfValuesForm.hasScheduleOfValues()) {
			$('#scheduleOfValuesTab').click();
			alert('Please ensure that all schedule of values have been added.');
			return false;
		}
		return true;
	}
};