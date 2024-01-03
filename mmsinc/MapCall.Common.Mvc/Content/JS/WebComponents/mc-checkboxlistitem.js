// ReSharper disable Es6Feature
(function (customElements) {

	const TEMPLATES = {
		checkBoxListItem: WebComponentHelper.defineAndCreateTemplate(`<label><input type="checkbox" /></label>`)
	};

	class MCCheckBoxListItemElement extends HTMLElement {
		constructor() {
			// Always call super first in constructor
			super();

			// NOTE: No reason for shadow DOM for this element.

			// The checkbox MUST be in the light DOM in order to be usable with
			// form posts. Web components don't have input/select style features.
			this._lightDOM = TEMPLATES.checkBoxListItem();
			this._checkBox = this._lightDOM.querySelector('input');

			// Annoyingly need to create a text node in order to add text to the label
			// element without another one. This is needed for css reasons.
			this._labelTextNode = document.createTextNode(''); // Can't pass null. This value is set in the text property.
			this._lightDOM.querySelector('label').appendChild(this._labelTextNode);
    }

		// PROPERTIES AND ATTRIBUTES

		get checked() {
			return this._checkBox.checked;
		}

		set checked(value) {
			// This is a bool prop, but it's also set by the attribute. 
			// Only the existance of the attribute matters, not the value,
			// so we only want to make this checked if the value is not 
			// explicitly false. It would be nice to use Boolean(value) here,
			// but because the attribute is empty 
			this._checkBox.checked = (value !== false);
		}

		// NOTE: The enabled property only exists for cascading stuff 
		// so we can pop a "-- Select --" in here easily.
		get enabled() {
			return !this._checkBox.disabled;
		}

		set enabled(value) {
			// This is a bool prop, but it's also set by the attribute. 
			// Only the existance of the attribute matters, not the value,
			// so we only want to make this checked if the value is not 
			// explicitly false.
			const isEnabled = value !== false;
			this._checkBox.disabled = !isEnabled;
		}

		get name() {
			return this._checkBox.getAttribute('name');
		}

		set name(value) {
			this._checkBox.setAttribute('name', value);
		}

		get text() {
			return this._labelTextNode.nodeValue;
		}

		set text(value) {
			this._labelTextNode.nodeValue = value;
		}

		get value() {
			return this._checkBox.getAttribute('value');
		}

		set value(value) {
			this._checkBox.setAttribute('value', value);
		}

		// In order to do observe attributes, you need to specifically list them in this array.
		// Then any time those attributes are set, attributeChangedCallback will be used.
		static get observedAttributes() { return ['checked', 'enabled', 'name', 'text', 'value']; }

		// This method is called any time an attribute value is changed. It's also called
		// for any observed attributes that exist on the element in the first place. 
		// ex: <mc-checkboxlistitem text="blah"> will fire this method for the title.
		attributeChangedCallback(name, oldValue, newValue) {
			// NOTE: While you *can* change the attribute values on this web component,
			// you should really use the properties instead. The property values do not
			// reflect back to the attribute because I'm not sure of the best practices
			// yet. ex: 'checked' isn't going to be set because there's no event listener
			// wired into the actual checkbox. There's also dealing with mutation observers
			// on child elements and other stuff to potentially deal with to ensure attributes
			// stay in sync. All of this can be implemented later if it actually becomes an issue.
			this[name] = newValue;
		}

		// END PROPERTIES AND ATTRIBUTES

		connectedCallback() {
			this.appendChild(this._lightDOM);
		}
  }

	// Define the new element
	customElements.define('mc-checkboxlistitem', MCCheckBoxListItemElement);

})(customElements);