//TODO: Refactor if jason has not already.
var EstimatingProjectShow = {
  selectors: {
    DELETE_COST_FORM: 'form#deleteOtherCostForm',
    DELETE_MATERIAL_FORM: 'form#deleteMaterialForm',
    DELETE_COMPANY_LABOR_COST_FORM: 'form#deleteCompanyLaborCostForm',
    DELETE_CONTRACTOR_LABOR_COST_FORM: 'form#deleteContractorLaborCostForm',
    DELETE_PERMIT_FORM: 'form#deletePermitForm',
  },

  initialize: function () {
    EstimatingProjectShow._initButtonAndForms(
      EstimatingProjectShow.selectors.DELETE_COST_FORM,
      'cost');

    EstimatingProjectShow._initButtonAndForms(
      EstimatingProjectShow.selectors.DELETE_MATERIAL_FORM,
      'material');

    EstimatingProjectShow._initButtonAndForms(
      EstimatingProjectShow.selectors.DELETE_COMPANY_LABOR_COST_FORM,
      'company labor cost');

    EstimatingProjectShow._initButtonAndForms(
      EstimatingProjectShow.selectors.DELETE_CONTRACTOR_LABOR_COST_FORM,
      'contractor labor cost');
      
    EstimatingProjectShow._initButtonAndForms(
      EstimatingProjectShow.selectors.DELETE_PERMIT_FORM,
      'permit');
  },
  
  _cancelFormButton_click: function(button, form) {
    $(button).click(function() {
      $(form).hide();
    });
  },

  _initButtonAndForms: function(removeForm, desc) {
    $(removeForm).submit(function() {
      return confirm('Are you sure you wish to delete the chosen ' + desc + ' record?');
    });
  },
};

$(document).ready(EstimatingProjectShow.initialize);