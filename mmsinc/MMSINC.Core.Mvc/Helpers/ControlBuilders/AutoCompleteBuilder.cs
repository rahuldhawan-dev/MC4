using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    public class AutoCompleteBuilder : ControlBuilder<AutoCompleteBuilder>
    {
        #region Consts

        private const string DEFAULT_HTTP_METHOD = "GET";

        /// <summary>
        /// The additional string added onto the id/name of the autocomplete textbox.
        /// </summary>
        public const string AUTOCOMPLETE_FIELD_ADDITION = "AutoComplete";

        #endregion

        #region Fields

        private string _httpMethod;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the parameter for the controller action that is called.
        /// </summary>
        public string ActionParameterName { get; set; }

        /// <summary>
        /// Gets/sets the controller action that's called when the parent's value is changed.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets/sets the http method required to access the controller action. Defaults to "GET" if not set.
        /// </summary>
        public string HttpMethod
        {
            get { return _httpMethod ?? DEFAULT_HTTP_METHOD; }
            set { _httpMethod = value; }
        }

        /// <summary>
        /// Gets / Sets the field that the autocomplete results should be filtered on
        /// </summary>
        public string DependsOn { get; set; }

        // <summary>
        /// Gets / Sets the field that the autocomplete text box uses as a place holder
        /// </summary>
        public string PlaceHolder { get; set; }

        /// <summary>
        /// Gets/sets the optional display text that will be displayed in the autocomplete textbox.
        /// If this is not set then the Value will be used instead.
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Gets/sets the value for this autocomplete textbox.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw new InvalidOperationException("An Id must be set for an autocomplete textbox to work.");
            }

            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new InvalidOperationException("The Url property must be set.");
            }

            if (string.IsNullOrWhiteSpace(ActionParameterName))
            {
                throw new InvalidOperationException("The ActionParameterName property must be set.");
            }

            // The hidden field is a textbox so that it works correctly with jquery validation. It's hackish, yes.
            // The css class is what makes it hidden.
            var stringValue = Convert.ToString(Value);
            var hidden = CreateTagBuilder("input");
            hidden.Attributes["type"] = "text";
            hidden.Attributes["value"] = stringValue;
            hidden.AddCssClass("autocomplete-faux-hidden");
            // so this isn't tabbable
            hidden.Attributes["tabindex"] = "-1";

            // NOTE: Name isn't needed for this textbox as its value isn't needed during model binding.
            var tb = new TextBoxBuilder();
            tb.Value = DisplayText ?? Value;
            tb.Id = Id + "_" + AUTOCOMPLETE_FIELD_ADDITION;
            tb.AddCssClass("autocomplete");

            // TODO: Add autocomplete attributes to tb.
            tb.HtmlAttributes["data-autocomplete-action"] = Url;
            tb.HtmlAttributes["data-autocomplete-actionparam"] = ActionParameterName;
            tb.HtmlAttributes["data-autocomplete-dependent"] = Name;
            // turn the browsers built in autocomplete/autofill off
            tb.HtmlAttributes["autocomplete"] = "off";

            if (HttpMethod != DEFAULT_HTTP_METHOD)
            {
                tb.HtmlAttributes["data-autocomplete-httpmethod"] = HttpMethod;
            }

            if (!String.IsNullOrWhiteSpace(DependsOn))
            {
                tb.HtmlAttributes["data-autocomplete-dependson"] = "#" + DependsOn;
            }

            if (!String.IsNullOrWhiteSpace(PlaceHolder))
            {
                tb.HtmlAttributes["placeholder"] = PlaceHolder;
            }

            return hidden.ToString(TagRenderMode.SelfClosing) + tb.ToHtmlString();
        }

        #endregion

        #region Public Methods

        public AutoCompleteBuilder WithDisplayText(string displayText)
        {
            DisplayText = displayText;
            return this;
        }

        /// <summary>
        /// Sets the value of the rendered textbox.
        /// </summary>
        /// <param name="value"></param>
        public AutoCompleteBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        #endregion
    }
}
