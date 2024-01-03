using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    /// <summary>
    /// This glorified TextBoxBuilder adds some date css classes.
    /// </summary>
    public class DatePickerBuilder : ControlBuilder<DatePickerBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets whether this DatePicker should also include a TimePicker. False by default.
        /// </summary>
        public bool IncludeTimePicker { get; set; }

        /// <summary>
        /// Gets/sets the value for this DatePicker. This can be a DateTime object or a formatted string.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var tag = CreateTagBuilder("input");
            tag.Attributes["type"] = "text";
            tag.Attributes["value"] = Convert.ToString(Value);
            tag.Attributes["autocomplete"] = "off";

            tag.AddCssClass("date");

            if (IncludeTimePicker)
            {
                tag.AddCssClass("date-time");
            }

            // Ensure this isn't a submit button
            var button = new ButtonBuilder()
                        .AsType(ButtonType.Button)
                        .WithCssClass("date-picker-trigger");

            // Need a space between them so the margin between the two is automatic.
            return tag.ToString(TagRenderMode.SelfClosing) + " " + button.ToHtmlString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the value of the rendered textbox.
        /// </summary>
        /// <param name="value"></param>
        public DatePickerBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        public DatePickerBuilder WithTimePicker(bool includeTimePicker)
        {
            IncludeTimePicker = includeTimePicker;
            return this;
        }

        #endregion
    }
}
