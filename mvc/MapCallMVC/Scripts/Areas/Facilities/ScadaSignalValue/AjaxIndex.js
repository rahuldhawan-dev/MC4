var ScadaSignalValueAjaxIndex = {
	selectors: {
		container: 'div#SCADASignalValues',
		paginationLinks: 'div#SCADASignalValues th.sortable > a, div#SCADASignalValues a.paginationLink',
		table: 'table#scadaSignalValuesTable'
	},
	initialize: function() {
		ScadaSignalValueAjaxIndex.initializePaginationLinks();
	},
	initializePaginationLinks: function() {
		var $links = $(ScadaSignalValueAjaxIndex.selectors.paginationLinks);
		$links.click(ScadaSignalValueAjaxIndex.paginationLink_click);
		$links.each(function(i, l) {
			l = $(l);
			l.data('url', l.attr('href'));
			l.attr('href', '#');
		});
	},
	paginationLink_click: function(e) {
		$(ScadaSignalValueAjaxIndex.selectors.container).load($(e.target).data('url'), null, ScadaSignalValueAjaxIndex.paginationLink_success);
	}
};

ScadaSignalValueAjaxIndex.initialize();