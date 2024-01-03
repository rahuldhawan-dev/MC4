// ReSharper disable Es6Feature
// TODO: The resetting of unobtrusive validation might not be needed if the form's InsertionMode != Replace
var Tabs = {
	'initialize': function () {
		var container = $('.tabs-container');

		const onActivateOrCreateTab = function (panel, isCreate) {
			// So we don't reload every time a tab gets displayed.
			if (panel.attr('data-ajax-tab') && !panel.attr('loaded')) {

				// OLD NOTE(from when ajaxtabs were first made in Contractors)
				// We only wanna submit ajax forms that are set to GET.
				// If they're set to POST then they were rendered with the page.

				// NEW NOTE
				// The content in the tab can be pre-loaded(makes the views simpler to use 
				// in some cases) so check for the preload attribute before submitting.
				var tabForms = panel.find('form[data-ajax="true"][data-ajax-method="GET"]')
					.not('[data-ajax-tab-preloaded="true"]');
				tabForms.submit();
			}

			if (!panel.attr('loaded')) {
				panel.attr('loaded', true);
				Application.runDynamicUiParsers(panel);
			}

			// Don't use "input[type=\'text\']:first, textarea:first, select:first" for this.
			// It will always find the first textbox before it finds the first textarea or select tag.
			var firstInput = panel.find('input[type=\'text\'], textarea, select').filter(':visible:enabled:first');
			if (!firstInput.hasClass('date')) {
				// We don't wanna automatically focus the datepicker, it's annoying. 
				try {
					firstInput.focus();
				} catch (e) { }
			}

			// The tab index will always be 0 when the tabs are initially created.
			// Setting it to 0 on create breaks tab-remembering.
			if (!isCreate) {
				var activeTabIndex = container.tabs('option', 'active');
				TabStorage.set(activeTabIndex);
            }
		};

		container.tabs({
			show: false, // Disables the animation when showing/hiding
			hide: false,
			create: function (event, ui) {
				// The create event is the only event that fires for the first
				// tab that loads on a page. The activate event fires for all 
				// others. Because that makes sense.
				return onActivateOrCreateTab(ui.panel, true);
			},
			activate: function(event, ui) {
				return onActivateOrCreateTab(ui.newPanel, false);
			}
		});

		var tabs = container.find('.tab-content');
		tabs.each(function (i, el) {
			Tabs.initializeTab($(el));
		});

		// Some pages might have tabs where it does not make sense to have them
		// remember the selected tab(like creating a new record that has multiple tabs)
		if (!container.hasClass('no-tab-storage')) {
			var lastActiveTab = TabStorage.get();
			if (lastActiveTab) {
				container.tabs('option', 'active', lastActiveTab);
			}
		}
	},
	'initializeTab': function ($tab) {
		if ($tab.attr('data-ajax-tab')) {
			Tabs.initializeAjaxTab($tab);
		}
	},
	'initializeAjaxTab': function ($tab) {
		Tabs.initializeForms($tab);
		Tabs.initializeSortingAndPagingLinks($tab);
	},
	'initializeForms': function ($tab) {
		// Ajaxtabs were originally created to support one single ajax form. Basically like
		// an UpdatePanel. Things go wrong when there are multiple ajax forms in a tab. So only
		// one can be the primary tab. Any additional form that isn't meant to trigger the ajax
		// tab update needs to have the not-ajax-tab-form class added to it.
		var form = $tab.find('form[data-ajax="true"]').not('.not-ajax-tab-form');
		// If there's no form then the tab won't be ajaxing.
		if (form.length === 0) { return; }
		var tabId = $tab.attr('id');
		// Only add the the data-ajax-update attribute if one isn't already specified
		// by the form. Some tabs have persistent content and only partially replace the tab content(ex MVC documents)
		const updateElementId = ($tab.attr('data-ajax-update-target-id') || tabId);
		form.attr('data-ajax-update', '#' + updateElementId);
		form.attr('data-ajax-begin', 'Tabs.onBegin("' + tabId + '")');
		form.attr('data-ajax-complete', 'Tabs.onComplete("' + tabId + '")');
		form.attr('data-ajax-error', 'Tabs.onError("' + tabId + '")');

		var customSuccess = form.attr('data-ajax-success');
		customSuccess = (customSuccess ? customSuccess : '');
		form.attr('data-ajax-success', 'Tabs.onSuccess("' + tabId + '", "' + customSuccess + '", data, xhr)');

		var loading = Tabs.createLoadingElement();
		$tab.append(loading);
		form.attr('data-ajax-loading', '#' + loading.attr('id'));
	},
	'initializeSortingAndPagingLinks': function ($tab) {
		// Why does this remove the href and store it in data? -Ross 7/11/2018
		$('th.sortable > a, a.paginationLink', $tab)
			.click(function (e) {
				const tabUpdateElementId = $tab.attr('data-ajax-update-target-id');
				// If the update-target is not set, we assume the entire tab has ajax-loaded content.
				const ajaxContentThatHasLinks = !tabUpdateElementId ? $tab : $tab.find('#' + tabUpdateElementId);
				ajaxContentThatHasLinks.load($(e.target).data('url'), function () {
					Tabs.onSuccess($tab.attr('id'));
				});
			})
			.each(function (i, l) {
				var $l = $(l);
				$l
					.data('url', $l.attr('href'))
					.attr('href', '#');
			});
	},
	'createLoadingElement': function () {
		var el = $('<div class="ajax-tab-loading"></div>');
		el.attr('id', 'loading-' + Math.floor(Math.random() * 10000));
		el.hide();
		return el;
	},
	'onBegin': function (tabId) { },
	'onComplete': function (tabId) { },
	'onError': function (tabId) { },
	// Called when the ajax form succeeds at posting.
	'onSuccess': function (tabId, customSuccess, data, xhr) {
		// TODO: Only call parseDynamicContent if the success results in replacement
		var selector = '#' + tabId;
		// $.validator.unobtrusive.parseDynamicContent(selector);
		Tabs.initializeTab($(selector));
		Application.runDynamicUiParsers(selector);

		// Call whatever custom handler was set from the mvc view.
		if (customSuccess) {
			deGlobalize(customSuccess)(data, xhr);
		}
	}
};

var TabStorage = (function () {
	if (!window.UserStorage) {
		// Return something fake that has the same api.
		return {
			set: function () { },
			get: function () { }
		};
	}

	var currentUrl = (function () {
		var url = location.href;
		// We don't want to differentiate urls based on query parameters
		// or #fragments, otherwise things would get all weird.
		url = url.split('#')[0];
		url = url.split('?')[0];
		// We also want to make sure we can handle the same url regardless
		// of whether or not it has the ending slash.
		if (url[url.length - 1] != '/') {
			url = url + '/';
		}
		return url;
	})();

	var storage = UserStorage.getPluginContainer('tabs.' + currentUrl);
	return {
		set: function (val) {
			storage.set('lastSelectedTab', val);
		},
		get: function () {
			return storage.get('lastSelectedTab');
		}
	};

})();

var deGlobalize = function (str, curObj) {
	var parts = str.split('.');
	curObj = (curObj || window)[parts.shift()];
	return parts.length ? deGlobalize(parts.join('.'), curObj) : curObj;
};

$(document).ready(function () {
	Tabs.initialize();
});
