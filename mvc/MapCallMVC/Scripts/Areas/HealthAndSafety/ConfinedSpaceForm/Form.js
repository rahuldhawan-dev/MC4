const ConfinedSpaceForm = (($) => {
    const METHOD_OF_COMMUNICATION_OTHER = 3;
    const ELEMENTS = {
        addAtmosphericTestButton: document.getElementById('add-atmospheric-test'),
        addEntrantButton: document.getElementById('add-entrant'),
        bumpTestConfirmedField: document.getElementById('IsBumpTestConfirmed'),
        existingTestsList: document.getElementById('existing-atmospheric-tests-list'),
        newAtmosphericTestsList: document.getElementById('new-atmospheric-tests-list'),
        newEntrantsList: document.getElementById('new-entrants-list'),
        isReclassificationSectionSigned: document.getElementById('IsReclassificationSectionSigned')
    };

    const IS_SECTION_2_COMPLETED = document.getElementById('is-section-2-completed').value === 'True';
    const templateItemCounts = {};

    // NOTE: Should this be needed again, all the basic template cloning functionality
    // should be wrapped up into a web component.
    const createListItemFromTemplate = (templateId, modelPropertyName, initializer) => {
        const originalTemplate = document.getElementById(templateId);

        // We'll be using the template element incorrectly here as we have
        // to modify tons of attributes in the template html each time
        // we render a new clone.
        //
        // "But Ross, why use a template element in the first place?"
        // Because templates don't render. The html inside of them are never
        // added to thet DOM, so we don't need to worry about performance
        // or other scripts wiring things to the template before we can 
        // deal with them. Also cloning templates is optimized to be a lot
        // faster than cloning existing DOM elements.
        const templateHtml = originalTemplate.innerHTML;

        return () => {
            const itemCount = (templateItemCounts[templateId] = templateItemCounts[templateId] || 0);
            // Update all html references to match with what the model binder would
            // expect for an array/list. This needs to match references to both id and name attributes.
            // TODO: This is a hack that isn't very good. The id values can't have brackets.
            //  const correctedTemplateHtml = templateHtml.replace(/NewEntrants/g, `NewEntrants[${itemCount}]`);
            const regex = new RegExp(modelPropertyName, 'g');
            const correctedTemplateHtml = templateHtml.replace(regex, `${modelPropertyName}[${itemCount}]`);

            // Then, to actually convert the html back to parsed elements, 
            // we make a new template and clone that. This is ugly but not
            // as ugly as having to append via rewriting the entire parent
            // element which would break any existing javascript wiring.
            const cloneTemplate = document.createElement('template');
            cloneTemplate.innerHTML = correctedTemplateHtml;
            // cloneNode of a template only gives a DocumentFragment. 
            // So you need to get the actual reference to the element
            // before appending it if you want to do anything with it.
            // Once the DocumentFragment is appended, the DocumentFragment
            // is then empty.
            // We need to wire in the unobtrusive validation, which requires
            // the elements be added to the form to do anything. So we need
            // a reference to the wrapper div before we lose it to the DOM.
            const clone = cloneTemplate.content.cloneNode(true).querySelector('div');

            // This lets us remove items without having to rename everything.
            // Renaming the name/id attributes is a huge pain that most likely
            // will not work due to the way all the unobtrusive stuff gets initialized.
            // Setting this Index field tells the model binder how to deal with 
            // collections being posted with missing numbers in the index sequence.
            // The Index field name has to be named "Property.Index" and not include
            // the indexer part of the name.
            const hiddenIndexField = document.createElement('input');
            hiddenIndexField.type = 'hidden';
            hiddenIndexField.name = `${modelPropertyName}.Index`;
            hiddenIndexField.value = itemCount;

            clone.appendChild(hiddenIndexField);

            if (initializer) {
                initializer(clone, itemCount);
            }

            Application.runDynamicUiParsers(clone);
            templateItemCounts[templateId]++;
            return clone;
        }
    };

    const ENTRANT_TYPES = {
        entrant: 1,
        attendant: 2,
        supervisor: 3
    };
    let entrantCache = [];

    const methods = {
        init: () => {
            $('.hazard-item').each(function () {
                const checkBox = $(this).find('input[type=checkbox]');
                $(checkBox)
                    .on('change', methods.onHazardItemCheckBoxChanged)
                    .change(); // fire this off to initialize the page properly on page load. 
            });
            $('#HasOtherSafetyEquipment')
                .on('change', methods.onHasOtherSafetyEquipmentChanged)
                .change();
            $('#MethodOfCommunication')
                .on('change', methods.onMethodOfCommunicationHasChanged)
                .change();
            $('#IsHotWorkPermitRequired')
                .on('change', methods.onMethodOfHotWorkOrFireWatchHasChanged)
                .change();
            $('#IsFireWatchRequired')
                .on('change', methods.onMethodOfHotWorkOrFireWatchHasChanged)
                .change();
            $('#CanBeControlledByVentilationAlone')
                .on('change', methods.onCanBeControlledByVentilationAloneChanged)
                .change();
            $(ELEMENTS.isReclassificationSectionSigned)
                .on('change', methods.onIsReclassificationSectionSignedChanged);

            methods.atmosphericTests.init();
            methods.entrants.init();
        },

        onCanBeControlledByVentilationAloneChanged: function () {
            const canNotBeControlled = $('#CanBeControlledByVentilationAlone').val() === 'False';
            Application.toggleEnabledTab('Section5Tab', canNotBeControlled);
            Application.toggleEnabledTab('RoleAssignmentTab', canNotBeControlled);
        },

        onHazardItemCheckBoxChanged: function () {
            const checkBox = $(this);
            const isChecked = checkBox.is(':checked');
            const hazardNotes = checkBox.closest('.hazard-item').find('.hazard-notes');
            hazardNotes.toggle(isChecked);
        },

        onHasOtherSafetyEquipmentChanged: function () {
            const isYes = $('#HasOtherSafetyEquipment').val() === 'True';
            Application.toggleField('#HasOtherSafetyEquipmentNotes', isYes);
        },

        onIsReclassificationSectionSignedChanged: (e) => {
            // There isn't a good way to prevent a checkbox from being changed.
            // The checked value changes before the click and change handlers fire, which is annoying.
            // So the only way to do this is to revert the change if the user hits cancel.
            // This shouldn't be an issue for now as there's nothing relying on this checkbox.
            // If you're using Firefox, you will see the checkbox visibly checked while the confirm
            // dialog is open. This doesn't happen in Chrome.
            if (ELEMENTS.isReclassificationSectionSigned.checked) {
                ELEMENTS.isReclassificationSectionSigned.checked = confirm('Are you sure you would like to reclassify the space?');
            }
            // don't do anything if the checkbox is being unchecked.
        },

        onMethodOfCommunicationHasChanged: function () {
            const isOther = parseInt($('#MethodOfCommunication').val()) === METHOD_OF_COMMUNICATION_OTHER;
            Application.toggleField('#MethodOfCommunicationOtherNotes', isOther);
        },

        onMethodOfHotWorkOrFireWatchHasChanged: function () {
            if ($('#IsHotWorkPermitRequired').val() === 'True') {
                $('#IsFireWatchRequired').val('True');
            }
        },

        atmosphericTests: {
            initAddButtonToggle: () => {
                // This toggle only needs to get wired up if the section hasn't been signed.
                // Otherwise, it's always visible. This only loads as a checkbox when the
                // section isn't signed. Otherwise it doesn't exist on the page.
                if (ELEMENTS.bumpTestConfirmedField && ELEMENTS.bumpTestConfirmedField.type === 'checkbox') {
                    const onBumpTestChanged = () => {
                        const makeVisible = ELEMENTS.bumpTestConfirmedField.checked;
                        $(ELEMENTS.addAtmosphericTestButton).toggle(makeVisible);
                    };
                    ELEMENTS.bumpTestConfirmedField.addEventListener('change', onBumpTestChanged);
                    onBumpTestChanged(); // init once
                }
            },
            init: () => {
                methods.atmosphericTests.initAddButtonToggle();

                const atmosphericTemplateCloner = createListItemFromTemplate('atmospheric-test-template', 'NewAtmosphericTests',
                    (newTestForm, itemCount) => {
                        const outOfRangeSignatureSection = newTestForm.querySelector('.signature-atmosphere-test');
                        const outOfRangeCheckbox = newTestForm.querySelector('.atmosphere-test-out-of-range-field');
                        outOfRangeCheckbox.addEventListener('fieldrequired', (e) => {
                            $(outOfRangeSignatureSection).toggle(e.detail.isRequired);
                        });

                        ELEMENTS.newAtmosphericTestsList.appendChild(newTestForm);
                    });

                ELEMENTS.addAtmosphericTestButton.addEventListener('click', atmosphericTemplateCloner);
            }
        },

        entrants: {
            getSelector: (itemCount, name) => `[name="NewEntrants[${itemCount}].${name}"]`,
            getEntrantTypeSelector: itemCount => methods.entrants.getSelector(itemCount, 'EntrantType'),
            getIsEmployeeSelector: itemCount => methods.entrants.getSelector(itemCount, 'IsEmployee'),
            getEmployeeSelector: itemCount => methods.entrants.getSelector(itemCount, 'Employee'),
            getSelectedEntrantType: itemCount => parseInt($(methods.entrants.getEntrantTypeSelector(itemCount)).val(), 10),
            getContractingCompanySelector: itemCount => methods.entrants.getSelector(itemCount, 'ContractingCompany'),
            getContractorNameSelector: itemCount => methods.entrants.getSelector(itemCount, 'ContractorName'),
            getOnEntrantTypeChange: (newEntrantForm, itemCount) => {
                return (event) => {
                    $(newEntrantForm.querySelector('.autocomplete'))
                        .val(null)
                        // disable if selected value is '-- Select --'
                        .prop('disabled', $(event.target).val() === '');
                    $(methods.entrants.getEmployeeSelector(itemCount)).val(null);
                    entrantCache[itemCount] = {};
                };
            },
            getOnAutoCompleteSelect: (itemCount) => {
                return (_, ui) => {
                    entrantCache[itemCount] = {
                        type: methods.entrants.getSelectedEntrantType(itemCount),
                        employee: ui.item.data
                    };
                };
            },
            getOnAutoCompleteResponse: (itemCount) => {
                return (_, ui) => {
                    let toRemove = [];
                    const selectedType = methods.entrants.getSelectedEntrantType(itemCount);
                    // remove employees that already have the current type
                    const typesToRemove = [selectedType];

                    // if current type is entrant, remove employees who are already
                    // attendant or supervisor
                    if (selectedType === ENTRANT_TYPES.entrant) {
                        typesToRemove.push(ENTRANT_TYPES.attendant, ENTRANT_TYPES.supervisor);
                    }

                    for (let i = entrantCache.length - 1; i >= 0; --i) {
                        const current = entrantCache[i];
                        if (typesToRemove.includes(current.type)) {
                            toRemove.push(current.employee);
                        }
                    }

                    for (let i = toRemove.length - 1; i >= 0; --i) {
                        const found = ui.content.filter(x => x.data === toRemove[i]);
                        for (let j = found.length - 1; j >= 0; --j) {
                            ui.content.splice(ui.content.indexOf(found[j]), 1);
                        }
                    }
                };
            },
            initRemoveButton: (newEntrantForm, itemCount) => {
                const removeNewEntrantButton = newEntrantForm.querySelector('.remove-new-entrant');
                removeNewEntrantButton.setAttribute('id', `RemoveEntrant${itemCount}`); // Literally only needed for functional tests.
                removeNewEntrantButton.addEventListener('click', _ => {
                    ELEMENTS.newEntrantsList.removeChild(newEntrantForm);
                    entrantCache[itemCount] = {};
                });
            },
            initEntrantTypeDropDown: (newEntrantForm, itemCount) => {
                $(methods.entrants.getEntrantTypeSelector(itemCount))
                    .on('change', methods.entrants.getOnEntrantTypeChange(newEntrantForm, itemCount));
            },
            initEmployeeAutoComplete: (newEntrantForm, itemCount) => {
                $(newEntrantForm.querySelector('.autocomplete'))
                    .on('autocompleteresponse', methods.entrants.getOnAutoCompleteResponse(itemCount))
                    .on('autocompleteselect', methods.entrants.getOnAutoCompleteSelect(itemCount))
                    .prop('disabled', true);
            },
            initEntrantTemplate: (newEntrantForm, itemCount) => {
                ELEMENTS.newEntrantsList.appendChild(newEntrantForm);

                methods.entrants.initRemoveButton(newEntrantForm, itemCount);
                methods.entrants.initEntrantTypeDropDown(newEntrantForm, itemCount);
                methods.entrants.initEmployeeAutoComplete(newEntrantForm, itemCount);
            },
            initData: data => {
                entrantCache = data;
                templateItemCounts['new-entrant-template'] = data.length;
            },
            init: () => {
                if (!IS_SECTION_2_COMPLETED) {
                    return; // The tab will not even be rendered, so this will throw errors galore if we keep going.
                }
                // TODO: Fix this in a better way. ELEMENTS.addEntrantButton is null when Section 5 *doesn't render*.
                // This happens in the post-completion state of the Edit view.
                ELEMENTS.addEntrantButton?.addEventListener('click',
                    createListItemFromTemplate(
                        'new-entrant-template',
                        'NewEntrants',
                        methods.entrants.initEntrantTemplate));
            }
        },
    };

    $(document).ready(methods.init);

    return methods;
})(jQuery);
