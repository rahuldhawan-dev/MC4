/*
* Detergent CascadingDropDown : a jQuery plugin, version: 1.3 (2019-08-22)
* @requires jQuery v1.7.1 or later

* By Ross Dickinson: http://www.rossisdead.com
* Based off the CascadingDropDown plugin by Raj Kaimal: https://github.com/rajkaimal/CascadingDropDown
* Licensed under the MIT: http://www.opensource.org/licenses/mit-license.php
*/

// TODO: Figure out how this is gonna work with being initialized from ajax content.
//       Don't wanna have the same annoyance that the unobtrusive validation library has.

// ReSharper disable Es6Feature
(function ($) {

	const DEPENDENTS_REQUIRED = {
		// these are all specifically lowercase to make it easier
		// to ensure the value exists without doing string manipulation.
		all: 'all',
		one: 'one',
		none: 'none'
	};

	const CASCADING_ELEMENT_TYPE = {
		select: 'select',
		checkBoxList: 'checkBoxList'
	};

	const getElementType = ($element) => {
		return $element.hasClass('checkbox-list') ? CASCADING_ELEMENT_TYPE.checkBoxList : CASCADING_ELEMENT_TYPE.select;
	};

	const getElementValue = ($element) => {
		if (getElementType($element) === CASCADING_ELEMENT_TYPE.checkBoxList) {
			// jQuery's .val() only returns an empty string if there are multiple checkboxes checked
			// So we need to make our own array.
			const selectedValues = [];
			$element.find('input[type="checkbox"]:checked').each(function () {
				selectedValues.push($(this).val());
			});
			return selectedValues;
		} else {
			return $element.val();
		}
	};

	const serializeElementValue = ($element) => {
		let $elementToSerialize = $element;

		if (getElementType($element) === CASCADING_ELEMENT_TYPE.checkBoxList) {
			// jQuery's .serialize only works on direct input/select elements,
			// so we need to select the checked checkboxes first.
			$elementToSerialize = $element.find('input[type="checkbox"]:checked');
		}
		return $elementToSerialize.serialize();
	};

	// NOTE: The CascadeStorage is only meant to re-set values if you hit the back button.
	//       While this also works if you do a soft-refresh in Firefox/Chrome, it does not
	//       work if you do a soft-refresh in Internet Explorer. Doing a page refresh is
	//       supposed to reset all form values by default(that's how browsers work) so this
	//       works as expected.

	const CascadeStorage = (function () {
		const getHiddenField = ($element) => {
			const hiddenId = $element.attr('id') + "_SelectedValue";
			return $('#' + hiddenId);
		};

		const storageMethods = {
			set: ($element) => {
				getHiddenField($element).val(getElementValue($element));
			},
			get: ($element) => {
				const options = methods.getOptions($element);
				const hiddenVal = getHiddenField($element).val();
				const retArr = [];
				if (!options.supportsMultipleSelectedItems) {
					retArr.push(hiddenVal);
				} else {
					// Returning an array with only an empty string causes
					// issues with cascades that have data preloaded on 
					// page load. The initial selected values will be wiped out if
					// the parent value is changed and then changed back again.
					if (hiddenVal !== '') {
						const split = hiddenVal.split(',');
						for (let i = 0; i < split.length; i++) {
							retArr.push(split[i]);
						}
					}
				}
				return retArr;
			}
		};

		return storageMethods;
	})();

	// Utility method. jQuery returns arrays for the value for multiselect
	// items but not normal selects. Need to know when arrays have an empty
	// value vs a useful value.
	const isArrayEmptyOrHasAllNonEmptyValues = function (arr) {
		if ($.isArray(arr)) {
			if (arr.length === 0) {
				// the array is empty
				return true;
			}

			for (let i = 0; i < arr.length; i++) {
				const cur = arr[i];
				if (cur === null || cur === '') {
					return true;
				}
			}
		}

		return false;
	};

	const detergent = function (options) {
		if (options === 'destroy') {
			methods.destroy($(this));
		} else {
			if (!options.parentSelector) {
				throw 'A parent selector is required.';
			}
			if (!options.actionPath) {
				throw 'An action path is required.';
			}
			return this.each(function () {
				const copiedOptions = $.extend({}, methods.defaults, options);
				methods.initialize($(this), copiedOptions);
			});
		}
	};

	const methods = {
		defaults: {
			// Values that must be set
			actionPath: null,
			parentSelector: null,
			elementType: null,

			// Optional
			appendValueToUrl: false,
			async: true,
			dependentsRequired: DEPENDENTS_REQUIRED.all,
			promptText: '-- Select --',
			loadingText: 'Loading ..',
			errorText: 'Error loading data.',
			noResultsText: 'No Results', // NOTE: There's nothing in MVC for setting this at the moment.
			unselectedText: '-- Select --',
			onLoading: null,
			onLoaded: null,
			httpMethod: 'GET',
			preRendered: false,
			supportsMultipleSelectedItems: false // Will be automatically set to true for listboxes.
		},

		addOption: ($element, value, text, isSelected) => {
			const option = methods.createOption($element, value, text, isSelected);
			$element.append(option);
			return option;
		},

		clear: ($element) => {
			$element.empty();
		},

		createOption: ($element, value, text, isSelected) => {
			value = value || '';
			text = text || value;
			if (methods.getOptions($element).elementType === CASCADING_ELEMENT_TYPE.checkBoxList) {

				const cbListItem = document.createElement('mc-checkboxlistitem');
				cbListItem.value = value;
				cbListItem.name = $element.attr('id');
				cbListItem.text = text;
				cbListItem.checked = !!isSelected; // Sometimes this is undefined, we want to ensure the value being set is true/false.

				// Due to logic flow, this is the only way to determine if we're displaying
				// the promptText at the moment. Can't check if the element is disabled because
				// it will always be disabled at this point.
				cbListItem.enabled = (value !== '');

				return cbListItem;

			} else {
				const option = $('<option></option>')
					.attr('value', value)
					.text(text);

				if (isSelected) {
					option.attr('selected', 'selected');
				}
				return option;
			}
		},

		replaceItems: ($element, items) => {
			// NOTE: When items are being retrieved via JSON, the server doesn't know
			//       what kind of control is being rendered. So for list boxes, we need to 
			//       remove the empty -- Select -- item.
			if (items.length === 0 || (items.length === 1 && !items[0].value && !items[0].Value)) {
				methods.disable($element);
			} else {
				methods.reset($element);

				const options = methods.getOptions($element);

				// NOTE: This will always be an array or null.
				let previouslySelected = options.previousPageLoadSelectedOption;
				if (previouslySelected && previouslySelected.length > 0) {
					// The previously selected value should only be read one time.
					// Otherwise if the parent value gets changed to something else
					// and back, then the child will be reset to what it was on
					// page load rather than when forcing the empty option.

					// It should also get knocked out of storage probably.
					options.previousPageLoadSelectedOption = null;
				} else {
					previouslySelected = methods.getLastGoodSelectedValue($element);
				}

				if (previouslySelected && typeof (previouslySelected) === "string") {
					throw 'replaceItems call for cascades requires an array and not a string.';
				}

				// This needs to specifically be checked for so we don't end up with both the "-- Select --" 
				// value being marked as selected as well as the non-empty item when the server sets
				// an item as selected.
				let serverNonEmptySelectedItem = null;
				items.each(function () {
					if (this.selected) {
						serverNonEmptySelectedItem = this;
					}
				});

				items.each(function () {
					// I wish I remembered why I was checking both lowercase and uppercase versions.
					const val = (this.value || this.Value || '').toString();
					const text = this.text || this.Text;

					let isSelected = false;
					if (serverNonEmptySelectedItem) {
						isSelected = serverNonEmptySelectedItem == this;
					} else {
						// Use $.inArray because IE8 has no indexOf support for arrays.
						isSelected = previouslySelected && $.inArray(val, previouslySelected) > -1;
					}

					if (val || !options.supportsMultipleSelectedItems) {
						methods.addOption($element, val, text, isSelected);
					}
				});
				methods.onLoaded($element);
			}
		},

		destroy: ($element) => {
			const options = methods.getOptions($element);
			$(options.parentSelector).unbind('.detergent');
			$element.removeData('options', null);
			$element.removeData('data-cascading-initialized');
		},

		disable: ($element) => {
			const options = methods.getOptions($element);
			methods.clear($element);
			$element.attr('disabled', 'disabled');
			methods.addOption($element, '', options.promptText);
			$element.trigger('change');
		},

		enable: ($element) => {
			$element.removeAttr('disabled');
			$element.trigger('change');
		},

		getOptions: ($element) => {
			return $element.data('options');
		},

		initialize: function ($element, options) {
			const initialized = !(!$element.data('data-cascading-initialized'));
			if (!initialized) {
				$element.data('data-cascading-initialized', true);

				// Set options early because CascadeStorage.get relies on them existing.
				methods.setOptions($element, options);

				// Initialize cache object for storing previously selected good values.
				options.previousGoodSelectedValues = {};

				// Initialize cache for post data so we don't have to repeatedly
				// call the server for the same data over and over.
				options.cachedItemsBySerializedParentValue = {};

				// Previous value needs to be read before the change event gets fired,
				// otherwise the value in CascadeStorage will be incorrect.
				const previousPageLoadSelectedOption = CascadeStorage.get($element);
				options.previousPageLoadSelectedOption = previousPageLoadSelectedOption;

				// initialize the action params and the parent elements that go with it.
				// the elements render with the parentSelectors and actionParams in the
				// same order. If you're seeing the parameters post back with values for
				// another element, make sure that the DependsOn property on the view model
				// attribute is set to the same order as the action's parameters.
				const dependsOn = options.parentSelector.split(',');
				let $parent = $();
				for (let i = 0; i < dependsOn.length; i++) {
					const actionParam = options.actionParams[i];
          			const $parentEl = $(dependsOn[i]);
					options.parentElementsByActionParam[actionParam] = $parentEl;
					// We need to use $().add() because just using $([array]) breaks
					// in random places for seemingly no good reason.
					$parent = $parent.add($parentEl);
				}

				options.parent = $parent;

				let isFirstTimeInitializing = true;
				const onParentChanged = function () {
					// DependentsRequired.all = All parent dropdowns must have a selected value before child action will trigger
					// DependentsRequired.one = At least one parent dropdown must have a selected value
					// DependentsRequired.none = Parent dropdown value is not required, so we can post if the parent changes back to "-- Select --" or null value.
					let atleastOneParentHasValue = false;
					let allParentsHaveValue = true;
					$parent.each(function () {
						const val = getElementValue($(this));
						if (!val || isArrayEmptyOrHasAllNonEmptyValues(val)) {
							allParentsHaveValue = false;
						} else {
							atleastOneParentHasValue = true;
						}
					});

					const canPost = (allParentsHaveValue ||
						((options.dependentsRequired === DEPENDENTS_REQUIRED.one) && atleastOneParentHasValue) ||
						options.dependentsRequired === DEPENDENTS_REQUIRED.none);

					if (canPost) {
						// This is for when the page initially loads. If the server has already 
						// rendered a list of items, we don't need to send an ajax request for 
						// the data since we already have it.
						const needsFirstTimeData = ($element.children().length === 0);
						if (!isFirstTimeInitializing) {
							methods.post($element);
						}
						else if (needsFirstTimeData) {
							if (options.preRendered) {
                                methods.showNoResults($element);
                            } else {
                                methods.post($element);
                            }
                        }
						// else: DO NOTHING. Otherwise elements with non-empty pre-rendered list items
						// will have their items wiped out.
					} else {
						methods.disable($element);
					}
					isFirstTimeInitializing = false;
				};

				$element.bind('change.detergent', function () {
					CascadeStorage.set($element);
					methods.saveLastGoodSelectedValue($element);
				});

				$parent.bind('change.detergent', onParentChanged);

				// Call this once so any pre-filled selections get saved.
				methods.saveLastGoodSelectedValue($element);

				// When someone clicks the reset button for a form and fires the fauxreset
				// event, we need to handle the reset ourselves due to the way we cache previously
				// selected values. When using DependentsRequired.none, whatever value that was
				// selected for the default null parent value will continue to get set when the
				// reset event is fired. In order to get around this, we need to clear out the
				// cached values and fire onParentChanged. 
				$element.parents('form').on('fauxreset', function () {
					options.previousGoodSelectedValues = {};
					onParentChanged();
				});

				// This needs to be run once on initialization so we can populate
				// any dropdowns that may have been pre-populated by form-state or
				// by the server.
				onParentChanged();
			} else {
				if (options === 'destroy') {
					methods.destroy($element);
				}
			}
		},

		onLoaded: ($element) => {
			methods.enable($element);
			const onload = methods.getOptions($element).onLoaded;
			if ($.isFunction(onload)) {
				onload.call($element);
			}

			// This is a custom event for this plugin.
			$element.trigger('refresh');
		},

		onLoading: ($element) => {
			methods.clear($element);
			const opts = methods.getOptions($element);
			methods.addOption($element, '', opts.loadingText);
			if ($.isFunction(opts.onLoading)) {
				opts.onLoading.call($element);
			}
		},

		onAjaxSuccess: ($element, serializedParentValueKey, data) => {
			const options = methods.getOptions($element);
			// Need to cache the non-jQuery array version.
			options.cachedItemsBySerializedParentValue[serializedParentValueKey] = data;
			const $data = $(data);
			methods.replaceItems($element, $data);
		},

		post: ($element) => {
			methods.onLoading($element);
			const opts = methods.getOptions($element);

			// Need to use serialize to get a proper key that can be cached due to allowing multiple dependent properties.
			const parentValueKey = serializeElementValue(opts.parent);

			const cachedItemsForParentValue = opts.cachedItemsBySerializedParentValue[parentValueKey];
			if (cachedItemsForParentValue) {
				methods.onAjaxSuccess($element, parentValueKey, cachedItemsForParentValue);
			} else {

				const ajaxArgs = {
					'async': opts.async,
					'data': {},
					'dataType': 'json',
					'error': function () {
						methods.showError($element);
					},
					'success': function (data) {
						return (data.length > 0) ? methods.onAjaxSuccess($element, parentValueKey, data) : methods.showNoResults($element);
					},
					// make sure we send arrays the way .net mvc expects them
					'traditional': true,
					'type': opts.httpMethod,
					'url': opts.actionPath
				};

				for (const [actionParam, $parent] of Object.entries(opts.parentElementsByActionParam)) {
					ajaxArgs.data[actionParam] = getElementValue($parent);
				}
				$.ajax(ajaxArgs);
			}
		},

		// TODO: Investigate killing this method entirely. It's useless.
		reset: ($element) => {
			// Don't trigger the change event in here. It causes previously
			// selected values to get wiped out.
			methods.clear($element);
		},

		saveLastGoodSelectedValue: ($element) => {
			const options = methods.getOptions($element);
			const parentValue = getElementValue(options.parent);

			let curVal = getElementValue($element);
			if (curVal) {
				if (!$.isArray(curVal)) {
					curVal = [curVal];
				}
			} else {
				curVal = [];
			}
			options.previousGoodSelectedValues[parentValue] = curVal;
		},

		getLastGoodSelectedValue: ($element) => {
			const options = methods.getOptions($element);
			const parentValue = getElementValue(options.parent);
			return options.previousGoodSelectedValues[parentValue];
		},

		setOptions: ($element, value) => {
			return $element.data('options', value);
		},

		showError: ($element) => {
			// Don't disable the select, it will mess up jquery validation if the field is required.
			methods.clear($element);
			methods.addOption($element, '', methods.getOptions($element).errorText);
		},

		showNoResults: ($element) => {
			// Don't disable the select, it will mess up jquery validation if the field is required.
			methods.clear($element);
			methods.addOption($element, '', methods.getOptions($element).noResultsText);
		}
	};

	// This makes our methods easily replacable.
	$.extend(detergent, methods);
	// And finally, make it accessible to jQuery
	$.fn.detergent = detergent;

	/////////////////////////
	/// UNOBTRUSIVE DETERGENT
	/////////////////////////

	const unobtrusiveMethods = {
		'initialize': ($element, options) => {
			// This makes it easier for us to pass in any group of
			// select elements without having to include the data-cascading
			// attribute check outside of this method.
			if ($element.attr('data-cascading') !== 'true') {
				// No attribute = no init.
				return;
			}
			// meaning someone just called $('something').unobtrusiveDetergent();
			if (!options) {
				// Pass in the data object since it'll count as the various text options
				$element.detergent(unobtrusiveMethods.getSettings($element));
			}
			else {
				// The only option to pass at the moment is 'destroy'.
				unobtrusiveMethods.destroy($element);
			}
		},
		'destroy': function ($element) {
			$element.detergent('destroy');
		},
		'getSettings': function ($element) {
			let data = $element.data('cascadingData');
			if (data === undefined) {
				data = {
					actionPath: $element.attr('data-cascading-action'),
					actionParams: $element.attr('data-cascading-actionparam').split(','),
					parentElementsByActionParam: {},
					parentSelector: $element.attr('data-cascading-dependson'),
					errorText: $element.attr('data-cascading-errortext'),
					httpMethod: $element.attr('data-cascading-httpmethod'),
					loadingText: $element.attr('data-cascading-loadingtext'),
					promptText: $element.attr('data-cascading-prompttext'),
					async: unobtrusiveMethods.getAsync($element),
					appendValueToUrl: ($element.attr('data-cascading-appendvalue') === 'true'),
					preRendered: ($element.attr('data-cascading-prerendered') === 'true'),
					elementType: getElementType($element)
				};

				data.supportsMultipleSelectedItems = $element.prop('multiple') || getElementType($element) === CASCADING_ELEMENT_TYPE.checkBoxList;

				const dependentsRequiredAttr = $element.attr('data-cascading-dependentsrequired');
				const dependentsRequired = DEPENDENTS_REQUIRED[dependentsRequiredAttr];

				if (dependentsRequired) {
					data.dependentsRequired = dependentsRequired;
				} else {
					throw 'Unable to find dependent requirement type: ' + dependentsRequiredAttr;
				}

				$element.data('cascadingData', data);
			}
			return data;
		},
		'getAsync': function ($element) {
			const val = $element.attr('data-cascading-async');
			if (val === undefined) {
				// It's a string, and if it's not set, we're defaulting to true.
				return true;
			}
			else {
				return (val === 'true');
			}
		}
	};

	const unobtrusiveDetergent = function (options) {
		this.each((i, element) => {
			unobtrusiveMethods.initialize($(element), options);
		});
	};

	$.fn.unobtrusiveDetergent = unobtrusiveDetergent;

	$(document).ready(function () {
		$('select, .checkbox-list').unobtrusiveDetergent();
	});
})(jQuery);
