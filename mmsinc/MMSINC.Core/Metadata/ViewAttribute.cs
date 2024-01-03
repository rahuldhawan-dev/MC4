using System;
using MMSINC.Utilities;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Use this attribute in place of Display/DisplayName/DisplayFormatAttributes. If you use this
    /// and see a spot where it's not being recognized, let Ross know. This is meant primarily for
    /// working with MVC views.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ViewAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets a static description used to describe this property. Help text basically.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets how the property name is displayed with DisplayFor/EditorFor templates. If null, the property
        /// name will be used instead.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets/sets how the property value is formatted. If null, no formatting will be done.
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Gets/sets whether the DisplayFormat should be applied when editing a value. Default is TRUE unlike DisplayFormatAttribute.
        /// </summary>
        public bool ApplyFormatInEditMode { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a ViewAttribute with the given display name. No format string is set.
        /// </summary>
        /// <param name="displayName"></param>
        public ViewAttribute(string displayName) : this()
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Creates a ViewAttribute with the given FormatStyle style. The FormatStyle is converted to the appropriate
        /// format string and set on the DisplayFormat property.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="style"></param>
        public ViewAttribute(string displayName, FormatStyle style) : this(displayName)
        {
            DisplayFormat = CommonStringFormats.ToFormatString(style);
            ApplyFormatInEditMode = GetDefaultValueForApplyingFormatStyleToEdit(style);
        }

        /// <summary>
        /// Creates a ViewAttribute and sets the format.
        /// </summary>
        /// <param name="style"></param>
        public ViewAttribute(FormatStyle style) : this(null, style) { }

        /// <summary>
        /// Creates an empty ViewAttribute instance.
        /// </summary>
        public ViewAttribute()
        {
            ApplyFormatInEditMode = true; // Default
        }

        #endregion

        #region Private Methods

        private static bool GetDefaultValueForApplyingFormatStyleToEdit(FormatStyle style)
        {
            switch (style)
            {
                case FormatStyle.Currency:
                case FormatStyle.CurrencyNoDecimal:
                    return false; // Otherwise we end up with $ signs in the textboxes.
            }

            return true;
        }

        #endregion
    }
}
