using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    public class HiddenInputBuilder : ControlBuilder<HiddenInputBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets the value for this hidden input.
        /// </summary>
        public object Value { get; set; }
        private bool _includeUnobtrusiveValidation = true;

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var hidden = CreateTagBuilder("input", _includeUnobtrusiveValidation);
            hidden.Attributes["type"] = "hidden";
            hidden.Attributes["value"] = Convert.ToString(Value);
            return hidden.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the Value property.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HiddenInputBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        public HiddenInputBuilder ExcludeClientSideValidation()
        {
            // tagbuilder knows this as includeUnobtrusiveValidation
            // thus the different in name here
            _includeUnobtrusiveValidation = false;
            return this;
        }

        #endregion
    }
}
