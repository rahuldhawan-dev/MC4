var EnvironmentalPermit = (function ($) {
  var ELEMENTS = {};

  const copyValAndTriggerChange = e => {
    const { target } = e;
    $('#CreateEnvironmentalPermitRequirement_' + target.id)
      .val($(target).val())
      .trigger('change.detergent');
  };

  const copyCheckBoxListValAndTriggerChange = e => {
	const id = e.currentTarget.id;
	const val = $.makeArray($(e.currentTarget).find(':checked').map((_, x) => $(x).val())).join(',');

	$('#CreateEnvironmentalPermitRequirement_' + id)
	  .val(val)
	  .trigger('change.detergent');
  };

  var ef = {
    initialize: function () {
      ELEMENTS = {
        childRequiresRequirements: '#CreateEnvironmentalPermitRequirement_RequiresRequirements',
        generalTab: '#GeneralTab',
        operatingCenters: '#OperatingCenters',
        requiresRequirements: '#RequiresRequirements',
        requiresRequirementsHelperText: '#EnvironmentalPermitRequiresRequirementsText',
        requirementsDOM: null,
        requirementsForm: '#requirementsForm',
        requirementsTab: '#RequirementsTab',
        state: '#State'
      };
      //ELEMENTS.requirementsDOM = $(ELEMENTS.requirementsTab).detach();
      $(ELEMENTS.requiresRequirements).on('change', ef.requiresRequirements_change);
      ef.requiresRequirements_change();
      $(ELEMENTS.operatingCenters).on('change', copyCheckBoxListValAndTriggerChange);
      $('div.tabs-container').tabs('option', 'active', 0);
      $(ELEMENTS.state).on('change', copyValAndTriggerChange);
    },
    requiresRequirements_change: function () {
      var showTab = $(ELEMENTS.requiresRequirements).val() === 'True';
      // Toggle the Ajax tab to add or remove the child form(s)?
      // $('a[href=#RequirementsTab]').parent().toggle(showTab);
      if (showTab) {
        // REATTACH THE Requirements DOM
          $(ELEMENTS.generalTab).after(ELEMENTS.requirementsDOM);
          $(ELEMENTS.operatingCenters + ',' + ELEMENTS.state).change();
      } else {
        // DETACH THE Requirements DOM so no fields are required.
        // this stored the detachted dom in here to we can reattach
        ELEMENTS.requirementsDOM = $(ELEMENTS.requirementsTab).detach();
      }
      $(ELEMENTS.requiresRequirementsHelperText).toggle(showTab);
      $(ELEMENTS.childRequiresRequirements).val($(ELEMENTS.requiresRequirements).val());
    }
  };
  $(document).ready(ef.initialize);
  return ef;
})(jQuery);
