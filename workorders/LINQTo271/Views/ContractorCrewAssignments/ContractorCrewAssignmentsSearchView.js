var ContractorCrewAssignmentsSearchView = {
  initialize: function () {
    Sys.Application.add_load(ContractorCrewAssignmentsSearchView.cascadingLoaded);
  },
  cascadingLoaded: function () {
    var cdd = $find('cddCrew');
    if (cdd != null)
      $('#' + cdd._element.id).change(ContractorCrewAssignmentsSearchView.toggleCrew);
  },
  toggleCrew: function () {
    if (!this.disabled)
      this.form.submit();
  }
};

