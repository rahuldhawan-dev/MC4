// This script is meant to be used wherever work orders are searchable, if you need to add more specific
// functionality for a particular work order search page then a script should be added to a folder with that
// name ("WorkOrderScheduling\Search.js", for instance).

(($, app) => {
  const SELECTORS = {
    ACOUSTIC_MONITORING_TYPE: '#AcousticMonitoringType',
    REQUESTED_BY: '#RequestedBy'
  };

  const WORK_ORDER_REQUESTERS = {
    ACOUSTIC_MONITORING: '6'
  };

  const onRequestedByChange = () => {
    const showAcousticMonitoringType =
      $(SELECTORS.REQUESTED_BY).val() === WORK_ORDER_REQUESTERS.ACOUSTIC_MONITORING;

    app.toggleField(SELECTORS.ACOUSTIC_MONITORING_TYPE, showAcousticMonitoringType);

    if (!showAcousticMonitoringType) {
      $(SELECTORS.ACOUSTIC_MONITORING_TYPE).val(null);
    }
  };

  $(document).ready(() =>{
    $(SELECTORS.REQUESTED_BY).on('change', onRequestedByChange);
    onRequestedByChange();
  });
})(jQuery, Application);