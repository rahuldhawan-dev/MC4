var WorkOrderMarkoutPlanningSearchView = {
  initialize: function() {
    getServerElementById('txtWorkOrderNumber').focus();
  },
  btnSearch_Click: function() {
    var ddlOperatingCenter = getServerElementById('ddlOperatingCenter')[0];
    if (ddlOperatingCenter.selectedIndex == 0) {
      alert('Please select an operating center');
      return false;
    }
    return true;
  }
};
