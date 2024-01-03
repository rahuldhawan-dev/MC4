(($) => {
  var selectors = {
    operatingCenter: '#OperatingCenter',
    planningPlant: '#PlanningPlant',
    facility: '#Facility',
    functionalLocation: '#FunctionalLocation'
  };

  var clearFunctionalLocation = () => {
    $(selectors.functionalLocation).val('');
  };
  
  $(() => {
    $(selectors.operatingCenter).change(clearFunctionalLocation);
    $(selectors.planningPlant).change(clearFunctionalLocation);
    $(selectors.facility).change(clearFunctionalLocation);
  });
})(jQuery);