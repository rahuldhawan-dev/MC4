(function($) {

  // These functions are ripped straight from jquery.validate.unobtrusive.js because there's no
  // way to access them otherwise. They are being attached to the jQuery.validator.unobtrusive
  // object so they can be accessed by any custom unobtrusive validation plugins.

  $.validator.unobtrusive.getModelPrefix = function(fieldName) {
    return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
  };

  $.validator.unobtrusive.appendModelPrefix = function(value, prefix) {
    if (value.indexOf("*.") === 0) {
      value = value.replace("*.", prefix);
    }
    return value;
  };

  $.validator.unobtrusive.escapeAttributeValue = function(value) {
    // As mentioned on http://api.jquery.com/category/selectors/
    return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
  };

  $.validator.unobtrusive.setValidationValues = function(options, ruleName, value) {
    options.rules[ruleName] = value;
    if (options.message) {
      options.messages[ruleName] = options.message;
    }
  };

  // End jquery.validate.unobtrusive.js copying.

  // Fixes the min/max validators being stupid and not parsing
  // strings as numbers. There's currently no way for the jQuery validation
  // stuff to know that a range is supposed to be a number or a date or
  // something else. -Ross 3/27/2015

  var parseValue = function(value) {
    if (typeof value === "string") {
      var asNumber = Number(value);
      if (!isNaN(asNumber)) {
        return asNumber;
      }
      // Might need to do some date parsing here at some point.
    }
    return value;
  };

  $.validator.methods.min = function(value, element, param) {
    param = parseValue(param);
    return this.optional(element) || value >= param;
  };

  $.validator.methods.max = function(value, element, param) {
    param = parseValue(param);
    return this.optional(element) || value <= param;
  };

  $.validator.unobtrusive.dynamicRuleParsers = [];

  (function() {
    var parseValue = function(val, type) {
      switch (type) {
        case 'integer':
          return parseInt(val, 10);
        case 'float':
          return parseFloat(val);
        case 'date':
          return new Date(val);
        case 'string':
          return (val ? val.toString() : val);
        default:
          throw new Error('compareto validator doesn\'t handle type: ' + type);
      }
    };

    var comparers = {
      'lessthan': function(val1, val2) {
        return (val1 < val2);
      },
      'lessthanorequalto': function(val1, val2) {
        return (val1 <= val2);
      },
      'equalto': function(val1, val2) {
        return (val1 == val2);
      },
      'greaterthan': function(val1, val2) {
        return (val1 > val2);
      },
      'greaterthanorequalto': function(val1, val2) {
        return (val1 >= val2);
      },
      'notequalto': function(val1, val2) {
        return (val1 != val2);
      }
    };

    var getRawElementValue = function(element) {
      var tagName = element[0].tagName.toLowerCase();
      switch (tagName) {
        case 'select':
          // Returns an empty string if there's no selected value. 
          return element.find('option:selected').text();

        default:
          return element.val();
      }
    };

    $.validator.addMethod("compareto", function(value, element, params) {
      var otherElement = $(params.form).find(params.otherElementSelector);
      // The reselecting of the element's needed for knockout/ajax/dynamic content situations.
      //            otherElement.unbind("blur.validate-compareto").bind("blur.validate-compareto", function() {
      //                $(element).valid();
      //            });

      if (params.ignorenullvalues === 'True' && !value) {
        return true;
      }
      var thisValue = parseValue(value, params.type);
      var otherValue = parseValue(getRawElementValue(otherElement), params.type);
      return comparers[params.comparison](thisValue, otherValue);
    });

    $.validator.unobtrusive.adapters.add("compareto", ["other", "comparison", "type", "ignorenullvalues"], function(options) {
      var prefix = $.validator.unobtrusive.getModelPrefix(options.element.name);
      var fullOtherName = $.validator.unobtrusive.appendModelPrefix(options.params.other, prefix);
      options.params.form = options.form;
      //options.params.otherElementSelector = ":[name=" + $.validator.unobtrusive.escapeAttributeValue(fullOtherName) + "]";
      options.params.otherElementSelector = "[name=" + $.validator.unobtrusive.escapeAttributeValue(fullOtherName) + "]";

      $(options.params.form).find(options.params.otherElementSelector).unbind("change.validate-compareto").bind("change.validate-compareto", function() {
        $(options.element).valid();
      });
      $(options.params.form).find(options.params.otherElementSelector).unbind("blur.validate-compareto").bind("blur.validate-compareto", function() {
        $(options.element).valid();
      });

      $.validator.unobtrusive.setValidationValues(options, "compareto", options.params);
    });
  })();

  // From http://xhalent.wordpress.com/2011/01/24/applying-unobtrusive-validation-to-dynamic-content/
  $.validator.unobtrusive.parseDynamicContent = function(selector) {
    // Analyze this before parsing everything else.
    //var dynamicParsers = $.validator.unobtrusive.dynamicRuleParsers;
    //for (var i = 0; i < dynamicParsers.length; i++) {
    //    dynamicParsers[i](selector);
    //}

    // $.validator.unobtrusive.parseDynamicRules(selector);
    //use the normal unobstrusive.parse method
    // Except this method is overridden to call parseDynamicRules.
    $.validator.unobtrusive.parse(selector);


    //get the relevant form
    var form = $(selector).first().closest('form');
    //get the collections of unobstrusive validators, and jquery validators
    //and compare the two
    var unobtrusiveValidation = form.data('unobtrusiveValidation');
    // This will be null/undefined if there aren't any fields requiring validation.
    if (unobtrusiveValidation) {

      var validator = form.validate();
      $.each(unobtrusiveValidation.options.rules, function(elname, elrules) {

        if (validator.settings.rules[elname] == undefined) {
          var args = {};
          $.extend(args, elrules);
          args.messages = unobtrusiveValidation.options.messages[elname];
          //edit:use quoted strings for the name selector
          $("[name='" + elname + "']").rules("add", args);
        } else {
          $.each(elrules, function(rulename, data) {
            if (validator.settings.rules[elname][rulename] == undefined) {
              var args = {};
              args[rulename] = data;
              args.messages = unobtrusiveValidation.options.messages[elname][rulename];
              //edit:use quoted strings for the name selector
              $("[name='" + elname + "']").rules("add", args);
            }
          });
        }
      });
    }
  };

  var oldParse = $.validator.unobtrusive.parse;
  $.validator.unobtrusive.parse = function(selector) {
    // Analyze this before parsing everything else.
    var dynamicParsers = $.validator.unobtrusive.dynamicRuleParsers;
    for (var i = 0; i < dynamicParsers.length; i++) {
      dynamicParsers[i](selector);
    }

    oldParse(selector);
    $.validator.unobtrusive.attachValidationFailEvent(selector);
  };

  $.validator.unobtrusive.attachValidationFailEvent = function(selector) {
    // This hack is to give us an event that can be attached live
    // on this invalid-form event since there's no way to get the
    // event to propagate. 
    var form = $(selector).find('form');
    form.on('invalid-form.validate', function() {
      form.trigger('validationerror');
    });
  };

  $(document).on('validationerror', 'form', function() {
    var $form = $(this);
    // This is for getting the tab with a validation error
    // in it to be visible. If the form doesn't wrap around
    // any tabs then there's no need to do things.

    if ($form.find('.tabs-container').length == 0) {
      return;
    }

    // Tabs automatically focus the first form element when 
    // a tab is activated. jQuery seems to also select
    // the first invalid form element when a tab gets changed.

    // DO NOT call elem.focus() here. Something about firing
    // the focus event while the validation plugin is still
    // doing stuff causes the form to submit anyway. This
    // only happens when we focus an element in a tab that 
    // wasn't visible when the submit event fired.

    var elem = $($form.validate().errorList[0].element);

    var tabPanel = elem.parents('div.tab-content');
    if (!tabPanel.is(':visible')) {
      $('a[href="#' + tabPanel.attr('id') + '"]').click();
    }

    return false;
  });

    $.validator.prototype.elementValue = function (element) {
        var type = $(element).attr("type"),
            val = $(element).val();

        // This allows us to actually get the selected values for a CheckBoxList 
        // built wth the CheckBoxListBuilder. This should work with any other validator. 
        // -Ross 3/8/2023
        if ($(element).hasClass('dummy-check-box-list-input')) {
            val = [];
            $(element).closest('.checkbox-list').find('input[type=checkbox]:checked').map(function () {
                val.push(this.value);
            });
        }

        if (type === "radio" || type === "checkbox") {
            return $("input[name='" + $(element).attr("name") + "']:checked").val();
        }

        if (typeof val === "string") {
            return val.replace(/\r/g, "");
        }
        return val;
    };

  // This fixes a bug that occurs in IE10 when running in IE7 compat mode.
  // jQuery will throw an exception when attempting to set the novalidate
  // attribute. jQuery, jQuery validate's author, and Microsoft have all
  // WONTFIX'ed this. Jerks.
  (function() {
    //check for IE7 or lower
    if (document.all && !document.querySelector && $.fn.validate) {
      var
          origValidateFn = $.fn.validate,
          slice = Array.prototype.slice;
      $.fn.validate = function() {
        var
            args = slice.call(arguments, 0),
            origAttrFn = this.attr;
        this.attr = function() {
          var args = slice.call(arguments, 0);

          //do not set the novalidate attribute in IE7 or lower, since it throws an error
          if (args.length > 1 && args[0] === "novalidate") {
            return this;
          }
          return origAttrFn.apply(this, args);
        };
        return origValidateFn.apply(this, args);
      };
    }
  }());

})(jQuery);