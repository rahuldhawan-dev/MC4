var EstimatingProjectForm = {
  selectors: {
    PROJECT_TYPE_DROP_DOWN: 'select#ProjectType',
    LUMP_SUM_FIELD: 'input#LumpSum'
  },
  
  initialize: function() {
    $(EstimatingProjectForm.selectors.PROJECT_TYPE_DROP_DOWN).change(EstimatingProjectForm.projectType_change);
  },

  projectType_change: function () {
    $(EstimatingProjectForm.selectors.LUMP_SUM_FIELD).parent().parent().parent()
      .toggle($('option:selected', this).text() == 'Non-Framework');
  }
};

$(document).ready(EstimatingProjectForm.initialize);