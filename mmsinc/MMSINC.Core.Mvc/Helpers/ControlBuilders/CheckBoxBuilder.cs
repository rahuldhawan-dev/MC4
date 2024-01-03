using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    public class CheckBoxBuilder : ControlBuilder<CheckBoxBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets whether this checkbox is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets/sets the value for this checkbox.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var cb = CreateTagBuilder("input");
            cb.Attributes["type"] = "checkbox";

            if (Checked)
            {
                cb.Attributes["checked"] = "checked";
            }

            if (Value != null)
            {
                cb.Attributes["value"] = Convert.ToString(Value);
            }

            return cb.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the Checked property.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public CheckBoxBuilder IsChecked(bool isChecked)
        {
            Checked = isChecked;
            return this;
        }

        /// <summary>
        /// Sets the value property to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CheckBoxBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        #endregion
    }
}
