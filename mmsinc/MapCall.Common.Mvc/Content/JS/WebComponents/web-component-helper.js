// ReSharper disable Es6Feature
class WebComponentHelper {

	/**
	 * Returns a function that creates an element from the given template. This
	 * function must be cached by the web component that uses it.
	 *
	 * NOTE: You do not want to supply a style to this if the template isn't being used to generate
	 * a web component with a shadow DOM. You otherwise risk generating duplicate style tags that
	 * will apply to the light DOM instead.
	 *
	 * NOTE 2: If the template contains more than one top-level element, this will return a DocumentFragment.
	 * Be aware that the DocumentFragment will be *empty* immediately after it's appended to another element.
	 *
	 * @param {any} templateHtml - html string used for the template.
	 * @return {function} - a function that can be called to create new deep clones of the template content.
	 */
	static defineAndCreateTemplate(templateHtml) {
		// Using a template element and cloning its content is the most performant
		// way to create the html structure of a web component. This only works
		// if the template is a singleton, though. So we're encapsulating the template
		// and returning a function that clones it each time. 
		const checkboxItemTemplate = document.createElement('template');
		checkboxItemTemplate.innerHTML = templateHtml;

		// Pass true to cloneNode, otherwise a deep clone isn't done and child elements will not be cloned.
		return () => checkboxItemTemplate.content.cloneNode(true);
	}
}