/*
* Reusable static methods that can be used with any elements.
*/
// ReSharper disable Es6Feature
class DOMUtils {
	static hide(el) {
		el.style.display = 'none';
	}

	static show(el) {
		// If hide was called before this, the empty string should
		// cause the element to revert back to its previous styling, 
		// however the style has been defined.
		el.style.display = '';
	}

	static toggle(el, makeVisible) {
		if (makeVisible) {
			DOMUtils.show(el);
		} else {
			DOMUtils.hide(el);
		}
	}

	/**
	 * Adds or removes a value-less attribute to an html element.
	 *
	 * @param {HTMLElement} el - The element being attributed
	 * @param {String} attributeName - The attribute to add to/remove from the element
	 * @param {Boolean} mustHaveAttribute - True if the attribute must be added to the element if it does not already exist.
	 *	                                    False if the attribute should be removed if it does exist on the element.
	 */
	static toggleAttribute(el, attributeName, mustHaveAttribute) {
		if (mustHaveAttribute) {
			el.setAttribute(attributeName, ''); // Must pass an empty string for a value-less attribute
		} else {
			el.removeAttribute(attributeName);
		}
	}
}