using System;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Determines how a bool/nullable bool should be displayed in editor/display templates.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BoolFormatAttribute : ModelFormatterAttribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets the display value for True values.
        /// </summary>
        public string True { get; set; }

        /// <summary>
        /// Gets/sets the display value for False values
        /// </summary>
        public string False { get; set; }

        /// <summary>
        /// Gets/sets the display value for Null values. 
        /// If Null is null then a default value may be used in its place
        /// for dropdowns.
        /// </summary>
        public string Null { get; set; }

        #endregion

        #region Constructor

        public BoolFormatAttribute() : this("True", "False", null) { }

        public BoolFormatAttribute(string trueText, string falseText) : this(trueText, falseText, null) { }

        public BoolFormatAttribute(string trueText, string falseText, string nullText)
        {
            True = trueText;
            False = falseText;
            Null = nullText;
        }

        #endregion

        public override string FormatValue(object value)
        {
            var boolVal = (bool?)value;
            if (!boolVal.HasValue)
            {
                return Null;
            }

            return boolVal.Value ? True : False;
        }
    }
}
