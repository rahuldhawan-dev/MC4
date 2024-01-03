var AjaxIndex = {
  selectors: {
    container: 'div#trainingRecordsContainer',
    paginationLinks: '#trainingRecordsContainer th.sortable > a, #trainingRecordsContainer a.paginationLink'
  },

  initialize: function() {
    var $container = $(AjaxIndex.selectors.container);
    $container.load($container.data('url'), null, AjaxIndex.initializePaginationLinks);
  },

  initializePaginationLinks: function() {
    var $links = $(AjaxIndex.selectors.paginationLinks);
    $links.click(AjaxIndex.paginationLink_click);
	$links.each(function(i, l) {
	  l = $(l);
	  l.data('url', l.attr('href'));
	  l.attr('href', '#');
	});
  },
   
  paginationLink_click: function(e) {
	$(AjaxIndex.selectors.container).load($(e.target).data('url'), null, AjaxIndex.paginationLink_success);
  },

  paginationLink_success: function() {
	AjaxIndex.initializePaginationLinks();
  }
};

$(AjaxIndex.initialize)