using System;

namespace MMSINC.Helpers
{
    public enum ButtonType
    {
        /// <summary>
        /// A button that doesn't actually do anything by default.
        /// </summary>
        Button = 0, // Default

        /// <summary>
        /// A button that resets forms.
        /// </summary>
        Reset,

        /// <summary>
        /// A button that submits forms.
        /// </summary>
        Submit
    }

    public class ButtonBuilder : ControlBuilder<ButtonBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets the text inside this button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets/sets the type of button being created. Default is ButtonType.Button.
        /// </summary>
        public ButtonType Type { get; set; }

        /// <summary>
        /// Gets/sets the value of this button.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var button = CreateTagBuilder("button", false);
            button.SetInnerText(Text);

            if (Value != null)
            {
                button.Attributes["value"] = Convert.ToString(Value);
            }

            switch (Type)
            {
                case ButtonType.Button:
                    button.Attributes["type"] = "button";
                    break;

                case ButtonType.Submit:
                    button.Attributes["type"] = "submit";
                    break;

                case ButtonType.Reset:
                    // Our buttons don't use the reset button type because
                    // we use javascript to handle things.
                    button.Attributes["type"] = "button";
                    button.AddCssClass("reset");
                    break;
            }

            return button.ToString();
        }

        #endregion

        #region Public Methods

        public ButtonBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public ButtonBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        public ButtonBuilder AsType(ButtonType type)
        {
            Type = type;
            return this;
        }

        #endregion
    }
}
