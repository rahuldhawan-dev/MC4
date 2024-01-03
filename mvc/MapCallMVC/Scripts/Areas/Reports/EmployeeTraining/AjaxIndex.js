var EmployeeTrainingAjaxIndex = {
  selectors: {
    container: 'div#TrainingRequirements',
    paginationLinks: 'div#TrainingRequirements th.sortable > a, div#TrainingRequirements a.paginationLink',
    table: 'table#trainingRequirementsTable'
  },

  initialize: function () {
    EmployeeTrainingAjaxIndex.initializePaginationLinks();
  },

  initializePaginationLinks: function () {
    var $links = $(EmployeeTrainingAjaxIndex.selectors.paginationLinks);
    $links.click(EmployeeTrainingAjaxIndex.paginationLink_click);
    $links.each(function (i, l) {
      l = $(l);
      l.data('url', l.attr('href'));
      l.attr('href', '#');
    });
  },

  paginationLink_click: function (e) {
    $(EmployeeTrainingAjaxIndex.selectors.container).load($(e.target).data('url'), null, EmployeeTrainingAjaxIndex.paginationLink_success);
  }
};

EmployeeTrainingAjaxIndex.initialize();