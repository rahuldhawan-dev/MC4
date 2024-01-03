// ReSharper disable Es6Feature
(function ($) {
    const FIELD_REQUIRED_EVENT = 'fieldrequired';
    const CACHED_REQUIREMENTS_MET_KEY = 'requirementsmet';

    $.expr[':'].hasAttrWithPrefix = function (obj, idx, meta, stack) {
        // From http://stackoverflow.com/a/12200042/152168
        // Usage: $(':hasAttrWithPrefix(some-partial-attribute-name)');

        // IE8 returns HTMLCommentElement objects that do not have an attributes property. 
        if (!obj.attributes) { return false; }
        for (let i = 0; i < obj.attributes.length; i++) {
            if (obj.attributes[i].nodeName.indexOf(meta[3]) === 0) return true;
        };
        return false;
    };

    const parseValue = (val, type) => {
        if ($.isArray(val)) {
            const retArray = [];
            $.each(val, function (i, x) {
                retArray.push(parseValue(x, type));
            });
            return retArray;
        }
        switch (type) {
            case 'integer':
                return parseInt(val, 10);
            case 'float':
                return parseFloat(val);
            case 'date':
                return new Date(val);
            case 'string':
                // This is required so comparers don't fail when undefined == '' is called.
                // NOTE: This can't be truthy evaluation because we might require a string == "0" which
                // would end up getting converted to null.
                if (val === undefined || val === null || $.trim(val).length === 0) {
                    return null;
                }
                return val.toString();
            case 'boolean':
                // BoolDropDown has string bool values, so they need to be parsed.
                // Also the !! doesn't work well with strings of 0/1, so they need
                // to be checked for explicitly.
                if (val === 'True' || val === 'true' || val === '1') {
                    return true;
                }
                else if (val === 'False' || val === 'false' || val === '0') {
                    return false;
                }
                else if (val === '') { // yeah this might break things
                    return null;
                }

                return !!val;
            default:
                throw new Error(`compareto validator doesn\'t handle type: ${type}.`);
        }
    };

    const comparers = {
        between: (actualValue, range) => {
            return (range.MinValue <= actualValue && actualValue <= range.MaxValue);
        },
        contains: (arrayOfPossibilities, val1) => {
            for (let i = 0; i < arrayOfPossibilities.length; i++) {
                if (comparers.equalto(val1, arrayOfPossibilities[i])) {
                    return true;
                }
            }
            return false;
        },
        lessthan: (val1, val2) => {
            return (val1 < val2);
        },
        lessthanorequalto: (val1, val2) => {
            return (val1 <= val2);
        },
        equalto: (val1, val2) => {
            return (val1 === val2);
        },
        equaltoany: (val1, arrayOfPossibilities) => {
            for (let i = 0; i < arrayOfPossibilities.length; i++) {
                if (comparers.equalto(val1, arrayOfPossibilities[i])) {
                    return true;
                }
            }
            return false;
        },
        greaterthan: (val1, val2) => {
            return (val1 > val2);
        },
        greaterthanorequalto: (val1, val2) => {
            return (val1 >= val2);
        },
        notbetween: (actualValue, range) => {
            return (range.MinValue >= actualValue || actualValue >= range.MaxValue);
        },
        notequalto: (val1, val2) => {
            return (val1 !== val2);
        },
        notequaltoany: (val1, arrayOfPossibilities) => {
            return !comparers.equaltoany(val1, arrayOfPossibilities);
        }
    };

    const getRequiredWhenDependentProperties = (obj) => {
        // This returns the root data-val-requiredwhenblahblah attribute. It's not supposed
        // to return its child properties(dependentproperty and targetvalue).
        const matches = [];
        for (let i = 0; i < obj.attributes.length; i++) {
            const attr = obj.attributes[i].nodeName;
            if (attr.indexOf('data-val-requiredwhen') === 0) {
                const thisProp = attr.split('data-val-requiredwhen')[1];
                if (thisProp.split('-').length === 1) {
                    matches.push(thisProp);
                }
            }
        };
        return matches;
    };

    const rulesAlreadyParsed = [];
    const parseForRequiredWhen = (selector) => {
        const requiredWhenDependents = $(selector).find(':hasAttrWithPrefix(data-val-requiredwhen)');
        requiredWhenDependents.each(function (i, e) {
            const dependents = getRequiredWhenDependentProperties(e);
            $(dependents).each(function (it, dep) {
                const validator = 'requiredwhen' + dep;
                if ($.inArray(validator, rulesAlreadyParsed) === -1) {
                    rulesAlreadyParsed.push(validator);
                    addRequiredWhenRule(validator);
                }
            });
        });
    };

    $.validator.unobtrusive.dynamicRuleParsers.push(parseForRequiredWhen);

    const addRequiredWhenRule = function (validator) {
        const meetsRequiredRequirement = function (form, params) {
            const targetvalue = params['targetvalue'];

            // get the actual value of the target control
            // note - this probably needs to cater for more 
            // control types, e.g. radios
            const control = form.find('[name="' + params['dependentproperty'] + '"]');
            if (control.length === 0) {
                alert("A RequiredWhen validator depends on the field '" + params['dependentproperty'] + "' but the field can not be found.");
            }
            const controltype = control.attr('type');
            let actualDependentValue = (controltype === 'checkbox' ? control.is(':checked') : control.val());

            // Need to special case for range because range types are the only
            // ones where the target value and the dependent value types are different.
            let targetType = params.targettype;
            if (params.israngetype) {
                switch (params.targettype) {
                    case 'range-float':
                        targetType = 'float';
                        break;
                    case 'range-integer':
                        targetType = 'integer';
                        break;
                }
            }
            actualDependentValue = parseValue(actualDependentValue, targetType);

            // returns true/false
            const meetsRequirement = comparers[params.comparison](actualDependentValue, targetvalue);

            // This needs to cache the most recent required result so that we can properly raise
            // the fieldrequired event when a field has more than one required-when validator.


            return meetsRequirement;
        };

        const doesElementMeetRequiredRequirementsForAtLeastOneDependent = (requirementsKeyDict) => {
            for (let key in requirementsKeyDict) {
                if (requirementsKeyDict[key]) {
                    return true;
                }
            }
            return false;
        };
        const initializeFieldRequiredEventHandler = (options, rules) => {
            const dependentProperty = rules['dependentproperty'];
            const $element = $(options.element);
            let allDependentsForElement = $element.data(CACHED_REQUIREMENTS_MET_KEY);
            if (!allDependentsForElement) {
                allDependentsForElement = {};
                $element.data(CACHED_REQUIREMENTS_MET_KEY, allDependentsForElement);
            }

            const maybeRaiseFieldRequiredEvent = () => {
                allDependentsForElement[dependentProperty] = meetsRequiredRequirement($(options.form), rules);

                // The event needs to be fired based on *all* of the dependent properties a child may have,
                // not just for the child whose value just changed and triggered this function call.
                // isRequired === at least one property is required.
                const isRequired = doesElementMeetRequiredRequirementsForAtLeastOneDependent(allDependentsForElement);
                const fieldRequiredEvent = new CustomEvent(FIELD_REQUIRED_EVENT,
                    {
                        // NOTE: It *has* to be "detail". It's part of the CustomEvent spec.
                        detail: { isRequired: isRequired }
                    });

                // NOTE: If you're listening for this event, you're most likely not going
                // to get it on page load. The validation scripts all run and parse before
                // any page-specific scripts. So anything that relies on this should also
                // manually fire their own event handler during initialization.
                options.element.dispatchEvent(fieldRequiredEvent);
            };

            const dependentControl = $(options.form).find('[name="' + dependentProperty + '"]');
            dependentControl.on('change', maybeRaiseFieldRequiredEvent);
            maybeRaiseFieldRequiredEvent();
        }

        /**
         * This forces the * to toggle on/off when the dependentvalue changes. 
         * 
         * This method uses the same DOM element markup to show this required ui state as does the Application.js for
         * the for when we can't use asp.net mvc declared RequiredWhen attribute validators. Ideally, this element's 
         * html would be shared as a const between these two files, but, we're not really sure where that should live, 
         * so for now - it lives in both.
         */
        const initializeAsteriskToggling = (options, rules) => {
            const $el = $(options.element);
            // Need to find the direct descent with the label class, otherwise we'll start adding
            // asterisks to other things that may have the label class but are unrelated to our field-pair things.
            const labelContainer = $el.closest('.field-pair').find('> .label');

            if (labelContainer.length === 0) {
                // Don't bother continuing if the control isn't rendering in our
                // usual EditorTemplate format.
                return;
            }

            // We need to check that the * span doesn't already exist. A field can have multiple
            // RequiredWhen validators associated with it that would result in multiple *'s appearing.
            let requiredLabel = labelContainer.find('.required-label');
            if (requiredLabel.length === 0) {
                // The space before the * is to be consistent with what ViewTemplateHelper does with required fields.
                requiredLabel = $('<span class="required-label"> *</span>');

                // This needs to come directly after the label tag. Otherwise, when helper description text exists,
                // it appears after the description text and looks weird.
                labelContainer.find('label').after(requiredLabel);
            }

            const toggleAsterisk = (e) => {
                const shouldDisplay = e.detail.isRequired;
                requiredLabel.toggle(shouldDisplay);
            };
            const dependentControl = options.element;
            dependentControl.addEventListener(FIELD_REQUIRED_EVENT, toggleAsterisk);
        }

        const initializeFieldVisibilityToggling = (options, rules) => {
            const dependentControl = options.element;
            const toggleAsterisk = (e) => {
                const shouldDisplay = e.detail.isRequired;
                Application.toggleField(dependentControl, shouldDisplay);
            };
            dependentControl.addEventListener(FIELD_REQUIRED_EVENT, toggleAsterisk);
        }

        $.validator.addMethod(validator,
            function (value, element, params) {
                const shouldCallRequiredValidator = meetsRequiredRequirement($(this.currentForm), params);

                if (shouldCallRequiredValidator) {
                    return $.validator.methods.required.call(this, value, element, params);
                }

                // Return true to indicate the given value is otherwise valid.
                return true;
            }
        );

        $.validator.unobtrusive.adapters.add(
            validator,
            // NOTE: This array determines which data-validation attributes the adapter parses from the
            // html element. So if you start trying to use a new attribute, make sure it's added to this array.
            ['dependentproperty', 'targetvalue', 'targetvaluemin', 'targetvaluemax', 'targettype', 'comparison', 'togglevisibility'],
            (options) => {
                const rules = {
                    dependentproperty: options.params['dependentproperty'],
                    comparison: options.params['comparison'],
                    targettype: options.params['targettype']
                };

                const isRangeFloat = rules.targettype === 'range-float';
                const isRangeInteger = rules.targettye === 'range-integer';
                rules.israngetype = (isRangeFloat || isRangeInteger);

                if (rules.israngetype) {
                    const unparsedMinValue = options.params['targetvaluemin'];
                    const unparsedMaxValue = options.params['targetvaluemax'];
                    if (isRangeFloat) {
                        // Do NOT use the eval'ed targetValue here as it will eval a string like "0000000" to 0.
                        rules.targetvalue = new Range(parseValue(unparsedMinValue, 'float'), parseValue(unparsedMaxValue, 'float'));
                    }
                    else if (isRangeInteger) {
                        // Do NOT use the eval'ed targetValue here as it will eval a string like "0000000" to 0.
                        rules.targetvalue = new Range(parseValue(unparsedMinValue, 'integer'), parseValue(unparsedMaxValue, 'integer'));
                    }
                    else {
                        throw new Error(`Unsupported range type: ${rules.targettype}`);
                    }
                }
                else {
                    const unparsedTargetValue = options.params['targetvalue'];
                    // The reason we're doing an eval is to figure out if this is a json array or not.
                    // and JSON.parse throws for non-json things.
                    const targetValue = eval(unparsedTargetValue);
                    if ($.isArray(targetValue)) {
                        const targetArray = [];
                        for (let i = 0; i < targetValue.length; i++) {
                            targetArray.push(parseValue(targetValue[i], rules.targettype));
                        }
                        rules.targetvalue = targetArray;
                    } else {
                        // Do NOT use the eval'ed targetValue here as it will eval a string like "0000000" to 0.
                        rules.targetvalue = parseValue(unparsedTargetValue, rules.targettype);
                    }
                }

                options.rules[validator] = rules;
                options.messages[validator] = options.message;

                // Initialize anything that relies on the fieldrequired event *before* 
                // initializing the fieldrequired event itself. The toggle
                // listens for that event, so it needs to register before it start
                // firing them off.
                if (options.params['togglevisibility'] === 'true') {
                    initializeFieldVisibilityToggling(options, rules);
                }
                initializeAsteriskToggling(options, rules);
                initializeFieldRequiredEventHandler(options, rules);
            }
        );
    };

    class Range {
        constructor(min, max) {
            this.MinValue = min;
            this.MaxValue = max;
        }
    }

})(jQuery);