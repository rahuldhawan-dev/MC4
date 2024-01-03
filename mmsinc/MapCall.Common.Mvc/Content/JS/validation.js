
/// <reference path="jquery-1.7.1-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
(function ($) {
	// make sure we don't ignore hidden fields by default  
	$.validator.setDefaults({ ignore: [] });

	$.validator.addMethod('at-least', function (value, element, min) {
		min = parseInt(min, 10);
		return $('[name="' + element.name + '"]:checked').length >= min;
	}, '');

	$.validator.unobtrusive.adapters.add('at-least', ['min'], function (options) {
		options.rules['at-least'] = options.params.min;
		options.messages['at-least'] = options.message;
	});

	$.validator.addMethod('integer', function (value, element) {
		// The other validators correctly determine if an integer is actually a number or not.
		// We just need to ensure there's not a decimal point.
		// Null values occur if a select tag does not have any options.
		if (value === null) {
			return true;
		}
		return value.indexOf('.') === -1;
	}, '');

	$.validator.unobtrusive.adapters.add('integer', [], function (options) {
		// There aren't any "rules" for this, but without a rules prop set
		// the validation method is never called.
		options.rules['integer'] = '';
		options.messages['integer'] = options.message;
	});

	// shouldbetrue
	$.validator.addMethod('requiresconfirmationmethod', function (value, element) {
		return $(element).is(':checked');
	}, '');

	$.validator.unobtrusive.adapters.addBool('requiresconfirmation', 'requiresconfirmationmethod');

	$.validator.addMethod('clientcallback', function (value, element, callbackMethod) {
		var form = $(this.currentForm);
		var method = eval(callbackMethod);
		if (!method) {
            throw `Unable to find method "${callbackMethod}" for clientcallback validator.`;
        }
		return method.call(this, value, element, form);
	});

	$.validator.unobtrusive.adapters.add('clientcallback', ['method'], function (options) {
		options.rules['clientcallback'] = options.params.method;
		options.messages['clientcallback'] = options.message;
	});

	// /shouldbetrue

	// This date range script is from http://mvccontrolstoolkit.codeplex.com/
	$.validator.addMethod(
		"daterange",
		function (value, element, param) {
			var minValue = param[0]; if (minValue == '') minValue = null;
			var maxValue = param[1]; if (maxValue == '') maxValue = null;

			if (minValue != null) minValue = new Date(minValue);
			if (maxValue != null) maxValue = new Date(maxValue);

			if ((!value || !value.length) && this.optional(element)) return true; /*success*/
			var convertedValue = null;
			if (typeof jQuery.global !== 'undefined' && typeof jQuery.global.parseFloat === 'function') {
				// convertedValue = new Date(value);
				convertedValue = jQuery.global.parseDate(value);
			}
			else {
				convertedValue = new Date(value);
			}
			if (!isNaN(convertedValue) &&
				(minValue == null || minValue <= convertedValue) &&
				(maxValue == null || convertedValue <= maxValue)) {
				return true; /* success */
			}
			return false;
		},
		"date is not in the required range");
	jQuery.validator.unobtrusive.adapters.add("daterange", ["min", "max"], function (options) {
		var min = options.params.min,
			max = options.params.max;
		options.rules["daterange"] = [min, max];
		if (options.message) {
			options.messages["daterange"] = options.message;
		}
	});

	$.validator.addMethod(
		"requireddaterange",
		function (value, element, param) {
			var minValue = param[0]; if (minValue == '') minValue = null;
			var maxValue = param[1]; if (maxValue == '') maxValue = null;

			if (minValue != null) minValue = new Date(minValue);
			if (maxValue != null) maxValue = new Date(maxValue);

			if ((!value || !value.length) && this.optional(element)) return true; /*success*/
			var convertedValue = null;
			if (typeof jQuery.global !== 'undefined' && typeof jQuery.global.parseFloat === 'function') {
				// convertedValue = new Date(value);
				convertedValue = jQuery.global.parseDate(value);
			}
			else {
				convertedValue = new Date(value);
			}
			if (!isNaN(convertedValue) &&
				(minValue == null || minValue <= convertedValue) &&
				(maxValue == null || convertedValue <= maxValue)) {
				return true; /* success */
			}
			return false;
		},
		"date is not in the required range");
	jQuery.validator.unobtrusive.adapters.add("requireddaterange", ["min", "max"], function (options) {
		var min = options.params.min,
			max = options.params.max;
		options.rules["requireddaterange"] = [min, max];
		if (options.message) {
			options.messages["requireddaterange"] = options.message;
		}
	});
})(jQuery);
