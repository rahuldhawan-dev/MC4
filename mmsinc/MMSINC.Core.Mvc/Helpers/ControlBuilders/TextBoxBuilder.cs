using System;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    // TODO: This needs to work with text, password, and textarea by default.

    public enum TextBoxType
    {
        Text = 0, // default
        Password,
        TextArea,
        Number
    }

    public static class TextBoxTypeExtensions
    {
        public static string ToAttributeValue(this TextBoxType type)
        {
            if (type == TextBoxType.TextArea)
            {
                throw new NotSupportedException(type.ToString());
            }

            return type.ToString().ToLowerInvariant();
        }
    }

    /// <summary>
    /// A control builder that can be used to create textboxes, password boxes, and textarea elements.
    /// </summary>
    public class TextBoxBuilder : ControlBuilder<TextBoxBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets the type of textbox to be rendered.
        /// </summary>
        public TextBoxType Type { get; set; }

        /// <summary>
        /// Gets/sets the value for this textbox.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Private Methods

        private string GetStringValue()
        {
            return Convert.ToString(Value);
        }

        protected override string CreateHtmlString()
        {
            switch (Type)
            {
                case TextBoxType.Text:
                case TextBoxType.Password:
                case TextBoxType.Number:
                    return CreateInputTag();

                case TextBoxType.TextArea:
                    return CreateTextAreaTag();

                default:
                    throw new NotSupportedException();
            }
        }

        private string CreateInputTag()
        {
            var tag = CreateTagBuilder("input");
            tag.Attributes["value"] = GetStringValue();

            tag.Attributes["type"] = Type.ToAttributeValue();

            return tag.ToString(TagRenderMode.SelfClosing);
        }

        private string CreateTextAreaTag()
        {
            var tag = CreateTagBuilder("textarea");
            var val = GetStringValue();
            if (val != null)
            {
                tag.SetInnerText(val);
            }

            return tag.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the Type for this textbox.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TextBoxBuilder AsType(TextBoxType type)
        {
            Type = type;
            return this;
        }

        /// <summary>
        /// Sets the value of the rendered textbox.
        /// </summary>
        /// <param name="value"></param>
        public TextBoxBuilder WithValue(object value)
        {
            Value = value;
            return this;
        }

        #endregion
    }
}
