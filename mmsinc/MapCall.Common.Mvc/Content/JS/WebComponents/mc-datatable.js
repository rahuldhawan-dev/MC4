// ReSharper disable Es6Feature
(function (customElements) {

	const TEMPLATES = {
		mainLayout: WebComponentHelper.defineAndCreateTemplate(`<style> 
#selector-controls { 
    display: flex;
	flex-direction: row;
}
.property-list {
	overflow-y: auto;
	display:flex;
	flex-wrap:wrap;
}
/* ::slotted will style LIGHT DOM elements that are in a slot. */
::slotted(.dt-prop-wrapper) {
	width:25%;
	/* Using min-width here so the number of columns is reduced when there isn't enough horizontal space */
	min-width:200px;
	display:flex;
	flex-direction:row; 
	align-items:center; 
	overflow:hidden;
}
::slotted(input) { flex: 0 1 auto; }
::slotted(label) { flex:1; white-space: nowrap; }
::slotted(select) { width:200px; }
::slotted(input[type=text]) { width:200px; }
</style>
<div id="selector-controls">
	<slot name="layout-controls"></slot>
</div>
<div class="editor-controls" style="display:none;">
	<div>
		<slot name="layout-save-controls"></slot>
	</div>
	<div class="property-list">
		<slot name="property-list"></slot>
	</div>
</div>

<slot></slot>`),

		controls: WebComponentHelper.defineAndCreateTemplate(`
<select slot="layout-controls" class="mc-dt-layout-select"></select>
<button slot="layout-controls" class="mc-dt-edit">Edit</button>
<button slot="layout-controls" class="mc-dt-delete">Delete</button>
<input slot="layout-save-controls" type="text" name="LayoutName" placeholder="Layout name" />
<button slot="layout-save-controls" class="mc-dt-save">Save Layout</button>
<button slot="layout-save-controls" class="mc-dt-close">Close</button>
`),

		// NOTE: Styling for this is included in the main DataTable styling. This is done so we aren't creating
		// a new stylesheet everytime this template is created. This only matters because this template is
		// part of the light DOM so the styles could conflict with other page styles.
		propertyItem: WebComponentHelper.defineAndCreateTemplate(`
<div class="dt-prop-wrapper" slot="property-list">
	<input type="checkbox" name="ExportableProperties" />
	<label></label>
</div>`)
	};

	class MCDataTableElement extends HTMLElement {
		constructor() {
			// Always call super first in constructor
			super();

			// STEP 1: GENERATE THE ELEMENT'S SHADOW DOM FROM THE TEMPLATE
			// When using attachShadow, the shadow DOM element is automatically set to the shadowRoot property.
			this.attachShadow({ mode: 'open' });
			this.shadowRoot.appendChild(TEMPLATES.mainLayout());

			// STEP 2: INITIALIZE THE COLUMN/PROPERTY INFORMATION FROM THE TABLE AND GENERATE 
			// THE PROPERTY LIST ELEMENT.

			this.table = this.querySelector('table');
			this.allProperties = {};

			this.table.querySelectorAll('th[data-property]').forEach((th) => {

				const propName = th.getAttribute('data-property');

				const propDescriptor = {
					isVisible: true,
					headerCell: th,
					checkBox: null,
					selectorElement: TEMPLATES.propertyItem()
				};
				this.allProperties[propName] = propDescriptor;

				// NOTE: Using an explicitly defined id here is to make life easier with 
				// selenium testing. This might cause problems if a table has multiple columns
				// referencing the same data property, or if there are multiple data tables on a page.
				const id = `dt${propName}`;

				const label = propDescriptor.selectorElement.querySelector('label');
				label.setAttribute('for', id);
				// Easiest way to match the display text of the column header is to copy it directly,
				// but we also need to remove the sorting icons if they're there. Not all headers
				// have sorting links.
				label.textContent = th.textContent.replace('▾', '').replace('▴', '');

				const checkbox = propDescriptor.selectorElement.querySelector('input');
				propDescriptor.checkBox = checkbox;
				checkbox.value = propName;
				checkbox.setAttribute('id', id);

				this.appendChild(propDescriptor.selectorElement);

				const toggleVisibility = () => {
					this.toggleProperty(propName, checkbox.checked);
				};
				checkbox.addEventListener('change', toggleVisibility);
			});

			// STEP 3: GENERATE AND INITIALIZE THE LAYOUT SELECTOR 

			this.appendChild(TEMPLATES.controls());

			const layoutSelect = this.querySelector('.mc-dt-layout-select');
			const saveButton = this.querySelector('.mc-dt-save');
			const editButton = this.querySelector('.mc-dt-edit');
			const deleteButton = this.querySelector('.mc-dt-delete');
			const closeButton = this.querySelector('.mc-dt-close');
			// If the mc-datatable element has an id, we want to append a version of it to the select
			// element. This is really only necessary for selenium tests at the moment because there 
			// aren't steps for dealing with web components yet.
			const id = this.getAttribute('id');
			if (id) {
				layoutSelect.setAttribute('id', `${id}-layout-name`);
				deleteButton.setAttribute('id', `${id}-delete-layout`);
			}

			const layouts = this.querySelectorAll('mc-data-table-layout');

			// Add a default "All" value to the top of the list so there's always a quick way to
			// display all of the columns.
			const defaultSelectAllItem = MCDataTableElement._createLayoutDataObject('', 'All', [], false);
			this._createAndAppendLayoutSelectOption(defaultSelectAllItem, false);

			const selectedLayoutId = new URLSearchParams(window.location.search).get('SelectedLayout');
			layouts.forEach((x) => {
				const canBeModified = true;
				const properties = MCDataTableElement._parsePropertiesStringToArray(x.getAttribute('properties'));
				const layoutId = x.getAttribute('id');
				const layoutDataObject = MCDataTableElement._createLayoutDataObject(layoutId, x.getAttribute('name'), properties, canBeModified);
				const isSelected = layoutId === selectedLayoutId;
				this._createAndAppendLayoutSelectOption(layoutDataObject, isSelected);
			});

			this.selectedExportableProperties = MCDataTableElement._parsePropertiesStringToArray(this.getAttribute('exportable-properties'));

			layoutSelect.addEventListener('change', this._onLayoutSelectionChanged.bind(this));
			saveButton.addEventListener('click', this._saveLayout.bind(this));
			deleteButton.addEventListener('click', this._deleteLayout.bind(this));
			editButton.addEventListener('click', () => {
				this._togglePropertyEditorVisibility(true);
			});
			closeButton.addEventListener('click', () => {
				this._togglePropertyEditorVisibility(false);
			});
		}

		static _parsePropertiesStringToArray(properties) {
			if (properties) {
				return properties.split(',');
			} else {
				return [];
			}
		}

		/**
		 * Creates a data object for layout information taht can be reused elsewhere.
		 * @param {any} id - The record id for this layout. Can be null.
		 * @param {any} layoutName - The display name of the layout.
		 * @param {any} properties - An array of property name strings.
		 * @param {any} canBeModified - If true, this layout can be edited or deleted.
		 * @return {object} a properly formatted data object.
		 */
		static _createLayoutDataObject(id, layoutName, properties, canBeModified) {
			return {
				id: id,
				layoutName: layoutName,
				properties: properties,
				canBeModified: canBeModified
			};
		}

		_createAndAppendLayoutSelectOption(layoutDataObject, isSelected) {
			const opt = document.createElement('option');
			opt.setAttribute('value', layoutDataObject.id);
			opt.innerText = layoutDataObject.layoutName;
			opt.layoutData = layoutDataObject;
			if (isSelected) {
				opt.selected = 'selected';
			}
			this.querySelector('.mc-dt-layout-select').appendChild(opt);
		}

		/*
		 * Returns an array of the currently selected property names.
		 */
		get selectedExportableProperties() {
			const selected = [];
			for (let prop in this.allProperties) {
				if (this.allProperties[prop].isVisible) {
					selected.push(prop);
				}
			}
			return selected;
		}

		/*
		 * Sets the visible properties to the given array of property names. If this is an empty
		 * array, all properties will become visible. 
		 */
		set selectedExportableProperties(propertyNames) {
			// When this is set, we want to explicitly go through each and toggle the visibility
			// for each property.
			// NOTE: This starts to slow down the more columns there are. 200ms for 50 columns.
			// I've tried generating a stlyesheet and style rules dynamically to see if just
			// adjusting a single style rule would help. It made zero difference in performance.
			const shouldExportAll = propertyNames.length === 0;
			for (let prop in this.allProperties) {
				this.toggleProperty(prop, (shouldExportAll || propertyNames.includes(prop)));
			}
		}

		/*
		 * Gets the selected layout option.
		 */
		get selectedLayoutOption() {
			const layoutSelect = this.querySelector('.mc-dt-layout-select');
			return layoutSelect[layoutSelect.selectedIndex];
		}

		/**
		 * Toggles the visibility of the column associated with the property name.
		 * @param {String} propertyName - the name of the property(not the display value)
		 * @param {Boolean} makeVisible - true if the column should be visible
		 */
		toggleProperty(propertyName, makeVisible) {
			const prop = this.allProperties[propertyName];
			prop.isVisible = makeVisible;

			// Depending on when this is called, the checkbox may already have the correct checked state.
			prop.checkBox.checked = makeVisible;

			const table = this.table;
			const thTag = prop.headerCell;
			// The nth-of-type selector is not zero based so we need to add 1 to select the correct elements.
			// based on its position in the parent tr tag.
			const cssIndex = Array.from(thTag.parentNode.children).indexOf(thTag) + 1;

			const toggleFunc = (cell) => {
				DOMUtils.toggle(cell, makeVisible);
			};
			table.querySelectorAll(`thead tr th:nth-of-type(${cssIndex})`).forEach(toggleFunc);
			table.querySelectorAll(`tbody tr td:nth-of-type(${cssIndex})`).forEach(toggleFunc);
			this._updateLinkUrls();
		}

		/**
		 * @param {any} showEditorControls - true if the editor controls should be displayed, false otherwise.
		 */
		_togglePropertyEditorVisibility(showEditorControls) {
			const editorControls = this.shadowRoot.querySelector('.editor-controls');
			const layoutSelectorControls = this.shadowRoot.querySelector('#selector-controls');
			DOMUtils.toggle(editorControls, showEditorControls);
			DOMUtils.toggle(layoutSelectorControls, !showEditorControls);
		}

		static _generateUpdatedExportablePropertiesUrl(existingUrl, exportableProperties, selectedLayoutId) {
			const properUrl = new URL(existingUrl);
			const queryString = new URLSearchParams(properUrl.search);
			// set will replace the existing ExportableProperties(including when there are duplicate keys)
			// with the given value as a single querystring key.
			queryString.set('ExportableProperties', exportableProperties);
			queryString.set('SelectedLayout', selectedLayoutId);
			return properUrl.pathname + '?' + queryString.toString();
		}

		_updateLinkUrls() {
			const serializedExportableProperties = this.selectedExportableProperties.join(',');
			const selectedLayoutId = this.selectedLayoutOption.layoutData.id;

			const doUrlUpdate = (link) => {
				link.href = MCDataTableElement._generateUpdatedExportablePropertiesUrl(link.href, serializedExportableProperties, selectedLayoutId);
			};

			// TODO: I think a single css class that can be applied to all of these
			// would be helpful.
			// This is gonna smell because I don't want to move the export link for this
			// one table so that it makes more sense UX-wise.
			document.querySelectorAll('.ab-export').forEach(doUrlUpdate);
			// Update the sorting links in the header if any exist.
			this.table.querySelectorAll('th a').forEach(doUrlUpdate);
			// This also kinda smells cause we need to go outside of the table to get the footer
			this.querySelectorAll('.table-footer a').forEach(doUrlUpdate);
		}

		_onLayoutSelectionChanged() {
			const selectedLayoutData = this.selectedLayoutOption.layoutData;
			this.selectedExportableProperties = selectedLayoutData.properties;
			this.querySelector('input[name="LayoutName"]').value = selectedLayoutData.layoutName;
		}

		_deleteLayout() {
			const selectedLayoutData = this.selectedLayoutOption.layoutData;
			if (!selectedLayoutData.canBeModified) {
				alert('The selected layout can not be deleted.');
				return;
			}

			if (!confirm('Are you sure you want to delete the selected layout?')) {
				return;
			}

			const destroyLayoutServiceUrl = this.getAttribute('delete-layout-url') + '/' + selectedLayoutData.id;

			fetch(destroyLayoutServiceUrl, { method: 'DELETE' })
				.then(resp => {
					// NOTE: Fetch doesn't have the same concept of success/error that jQuery had.
					// You have to check resp.ok to ensure the response is a 200. Or you can check all
					// the different status codes manually.
					if (resp.ok) {
						resp.json().then(result => {
							if (result.success) {
								alert('Layout deleted successfully!');
								const layoutSelect = this.querySelector('.mc-dt-layout-select');
								layoutSelect.remove(layoutSelect.selectedIndex);
								this._onLayoutSelectionChanged();
							} else {
								let errMessage = '';
								for (let err in result.errors) {
									errMessage = errMessage + result.errors[err] + ' ';
								}
								alert('There was a problem saving this layout: ' + errMessage);
							}
						});

					} else {
						alert("An unknown error has occured while attempting to delete this layout.");
					}
				});
		}

		_saveLayout() {
			const createLayoutServiceUrl = this.getAttribute('create-layout-url');
			const editLayoutServiceUrl = this.getAttribute('update-layout-url');

			const selectedLayout = this.selectedLayoutOption;
			const selectedLayoutData = selectedLayout.layoutData;
			const selectedExportableProperties = this.selectedExportableProperties;
			const editedLayoutName = this.querySelector('input[name="LayoutName"]').value;
			const shouldUpdateExisting = editedLayoutName === selectedLayoutData.layoutName;

			if (shouldUpdateExisting) {
				if (!selectedLayoutData.canBeModified) {
					alert('The selected layout can not be modified. Change the name and then try saving again.');
					return;
				}
				if (!confirm('Are you sure you want to update and save the selected layout? This will affect all users.')) {
					return;
				}
			} else {
				if (!confirm('Are you sure you want to save this new layout? It will be available to all users.')) {
					return;
				}
			}

			let postUrl = createLayoutServiceUrl;
			if (shouldUpdateExisting) {
				postUrl = editLayoutServiceUrl + "/" + selectedLayoutData.id;
			}

			const postData = {
				properties: selectedExportableProperties,
				typeGuid: this.getAttribute('layout-type'),
				// NOTE: These three properties only matter for creation. They
				// are ignored during updates.
				layoutName: editedLayoutName,
				area: this.getAttribute('layout-area'),
				controller: this.getAttribute('layout-controller')
			};

			if (shouldUpdateExisting) {
				// Only add the id property to the postData if we're editing
				// an existing layout. If we pass a null/empty id value to 
				// the server, the model binder will try to bind that and fail.
				// This is because ViewModel.Id is not nullable, but we don't
				// need an id value at all for create methods.
				postData.id = selectedLayoutData.id;
			}

			// TODO: Fetch is more verbose than jQuery's ajax stuff, because of all
			// the promise stuff. A jQuery-less wrapper around some of this stuff 
			// would probably be useful.
			fetch(postUrl,
				{
					method: 'POST',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(postData)
				})
				.then(resp => {
					// NOTE: Fetch doesn't have the same concept of success/error that jQuery had.
					// You have to check resp.ok to ensure the response is a 200. Or you can check all
					// the different status codes manually.
					if (resp.ok) {
						resp.json().then(result => {
							if (result.success) {
								if (shouldUpdateExisting) {
									// Update the layoutDataObject so if the user selects a different layout
									// and then selects this one again it will still have the changes.
									selectedLayoutData.properties = selectedExportableProperties;
								} else {
									const newLayoutDataObject = MCDataTableElement._createLayoutDataObject(result.id, editedLayoutName, selectedExportableProperties, true);
									this._createAndAppendLayoutSelectOption(newLayoutDataObject, true);
								}
								this._togglePropertyEditorVisibility(false); // Switch back to non-edit mode.
								alert('Layout saved successfully!');
							} else {
								let errMessage = '';
								for (let err in result.errors) {
									errMessage = errMessage + result.errors[err] + ' ';
								}
								alert('There was a problem saving this layout: ' + errMessage);
							}
						});

					} else {
						alert("An unknown error has occured while attempting to save the layout.");
					}
				});
		}
	}

	// Define the new element
	customElements.define('mc-datatable', MCDataTableElement);

})(customElements);