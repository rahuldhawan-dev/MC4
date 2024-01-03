using System;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.BooleanExtensions
    // ReSharper restore CheckNamespace
{
    public static class BooleanExtensions
    {
        #region Private Static Methods

        private static Func<IFormatProvider> _formatProviderFactory;

        #endregion

        #region Static Properties

        // TODO: this is just for inspection in a test
        public static Func<IFormatProvider> FormatProviderFactory
        {
            get { return _formatProviderFactory; }
        }

        #endregion

        #region Constructors

        static BooleanExtensions()
        {
            ResetFormatProviderFactory();
        }

        #endregion

        #region Exposed Static Methods

        public static void SetFormatProviderFactory(Func<IFormatProvider> fn)
        {
            _formatProviderFactory = fn;
        }

        public static void ResetFormatProviderFactory()
        {
            _formatProviderFactory = () => new BooleanFormatProvider();
        }

        #endregion

        #region Extension Methods

        public static string ToString(this bool value, string format)
        {
            return String.Format(_formatProviderFactory(),
                "{0:" + format + "}", value);
        }

        public static bool IsNullOrFalse(this bool? value)
        {
            return !value.HasValue || !value.Value;
        }

        #endregion
    }

    public class BooleanFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region Constants

        public struct FormatStrings
        {
            public const string YES_OR_NO = "yn",
                                YES = "Yes",
                                NO = "No";
        }

        #endregion

        #region Exposed Methods

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var value = (bool)arg;
            format = (format == null ? null : format.Trim().ToLower());

            switch (format)
            {
                case FormatStrings.YES_OR_NO:
                    return value ? FormatStrings.YES : FormatStrings.NO;
                default:
                    var formattable = arg as IFormattable;
                    return (formattable == null)
                        ? arg.ToString()
                        : formattable.ToString(format, formatProvider);
            }
        }

        public object GetFormat(Type formatType)
        {
            return (formatType == typeof(ICustomFormatter)) ? this : null;
        }

        #endregion
    }
}
