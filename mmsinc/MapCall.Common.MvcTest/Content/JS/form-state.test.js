/// <reference path="../../../MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/testHelpers.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/simpleStorage.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/user-storage.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/jquery-1.10.2.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/form-state.js" />

module('form-state.js');

preserve([],
    function () {
        var formHolder = $('<div id="formHolder"></div>');

        var initialize = function () {
            simpleStorage.flush();
        };

        var run = function (testFunc) {

            // Resharper test runner obliterates everything in the
            // body tag, so we need to append it when the tests actually start.
            if (formHolder.parents().length === 0) {
                $('body').append(formHolder);
            }
            initialize();
            mock(function () {
                testFunc();
            });

            // Clear any forms that have been appended.
            formHolder.empty();
        };

        var createForm = function (args) {
            var defaults = {
                formId: 'test-form',
                otherInputs: null
            };
            var options = $.extend({}, defaults, args);

            var form = $('<form></form>');
            form.attr('id', options.formId);

            if (options.otherInputs) {
                form.append(options.otherInputs);
            }

            initForm(form);

            return form;
        };

        var initForm = function ($form) {
           
            mock.expect('FormState._getForms').mock(function () {
                return $form;
            });
            formHolder.append($form);
        };

        var trySaveAndRestore = function (args) {
            // Create form and set values that should be saved(or in this case, not be saved).
            var elToAppend = args.input;
            if (args.wrapper) {
                args.wrapper.append(args.input);
                elToAppend = args.wrapper;
            }

            var form = createForm({ otherInputs: elToAppend });
            args.input.val(args.expectedValue);
            // Save form values then reset the testing inputs.
            FormState.saveFormValues();
            //args.input.val('');
            form[0].reset();

            // multi-selects return null instead of '' for some reason.
            deepEqual((args.input.val() || ''), '');

            // Reinit and hope for the best.
            FormState.init();

            return (args.input.val() || '');
        }

        var savesAndRestores = function (args) {
            var result = trySaveAndRestore(args);
            deepEqual(result, args.expectedValue);
        };

        var doesNotSaveAndRestore = function (args) {
            var result = trySaveAndRestore(args);
            deepEqual(result, '');
        }

        // 
        // Tests(obviously)
        //

        test('FormState saves and restores values from textboxes.', function () {
            run(function () {
                savesAndRestores({
                    input: $('<input type="textbox" id="textbox" />'),
                    expectedValue: 'i best display'
                });
            });
        });

        test('FormState saves and restores checkboxes correctly.', function () {
            run(function () {
                // Can't use savesAndRestores here because of the prop check.

                // Create form and set values that should be saved(or in this case, not be saved).
                var input = $('<input type="checkbox" id="checkbox" />');
                var form = createForm({ otherInputs: input });
                input.prop('checked', true);

                // Save form values then reset the testing inputs.
                FormState.saveFormValues();
                form[0].reset();
                equal(input.is(':checked'), false);

                // Reinit and hope for the best.
                FormState.init();
                equal(input.is(':checked'), true);
            });
        });

        test('FormState does not set the value attribute on a checkbox.', function () {
            run(function () {
                // Create form and set values that should be saved(or in this case, not be saved).
                var input = $('<input type="checkbox" id="checkbox" />');
                var form = createForm({ otherInputs: input });
                input.prop('checked', true);
                input.val('i should go away');

                // Save form values then reset the testing inputs.
                FormState.saveFormValues();
                input.val('');

                // Reinit and hope for the best.
                FormState.init();
                equal(input.val(), '');
            });
        });

        test('FormState saves and restores for selects.', function () {
            run(function () {
                savesAndRestores({
                    input: $('<select id="select"><option value="">Nope</option><option value="2">Yup</option></select>'),
                    expectedValue: '2'
                });
            });
        });

        test('FormState can save and restore multi select list values.', function () {
            run(function () {
                savesAndRestores({
                    input: $('<select id="select" multiple><option value="1">One</option><option value="2">Two</option><option value="3">Three</option></select>'),
                    expectedValue: ['1', '3']
                });
            });
        });

        test('FormState does not save and restore values on no-form-state fields.', function () {
            run(function () {
                doesNotSaveAndRestore({
                    input: $('<input type="textbox" id="nope" class="no-form-state" />'),
                    expectedValue: 'i best not display'
                });
            });
        });

        test('FormState does not save and restore values on fields with a parent that has no-form-state class.', function () {
            run(function () {
                doesNotSaveAndRestore({
                    wrapper: $('<div class="no-form-state"></div>'),
                    input: $('<input type="textbox" id="nope" />'),
                    expectedValue: 'i best not display'
                });
            });
        });

        test('FormState does not set values on disabled fields.', function () {
            run(function () {
                doesNotSaveAndRestore({
                    input: $('<input type="textbox" id="nope" disabled />'),
                    expectedValue: 'i best not display'
                });
            });
        });
    });
// leave this at the bottom: 
// preserve()
