using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    public class CheckBoxListItemBuilder : ControlBuilder<CheckBoxListItemBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets whether this checkbox is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets/sets the label text displayed next to the checkbox.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets/sets the value for this checkbox.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var cb = CreateTagBuilder("mc-checkboxlistitem");

            if (Checked)
            {
                // The value of the "checked" attribute doesn't matter, it just needs to exist.
                cb.Attributes["checked"] = string.Empty;
            }

            if (Text != null)
            {
                cb.Attributes["text"] = Text;
            }

            if (Value != null)
            {
                cb.Attributes["value"] = Convert.ToString(Value);
            }

            return cb.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the Checked property.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public CheckBoxListItemBuilder IsChecked(bool isChecked)
        {
            Checked = isChecked;
            return this;
        }

        /// <summary>
        /// Sets the value property to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CheckBoxListItemBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        /// <summary>
        /// Sets the value property to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CheckBoxListItemBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        #endregion
    }
}
