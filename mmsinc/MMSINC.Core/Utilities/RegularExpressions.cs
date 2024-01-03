namespace MMSINC.Utilities
{
    /// <summary>
    /// Consts for common regular expressions used throughout, so we don't have to rewrite
    /// and we can test them.
    /// </summary>
    public struct RegularExpressions
    {
        public const string ALPHA_AND_SPACE = "^[a-zA-Z ]+$";
        /// <summary>
        /// #FFFFFF
        /// </summary>
        public const string HEX_COLOR_VALUE = "^#[0-9a-fA-F]{6}$";

        public const string NUMERICAL = "^[0-9]*$";

        /// <summary>
        /// rgba(255, 189, 189, 1)
        /// rgb(255, 189, 189, 1)
        /// </summary>
        public const string RGB_COLOR_VALUE = "^rgba?\\((\\d+), ?(\\d+), ?(\\d+)(?:, ?(\\d+))?\\)$";

        //public const string PHONE = "^(?:1(?: |-))?(?:\\(\\d{3}\\)|\\d{3})(?: |-)?\\d{3}(?: |-)?\\d{4}$";
        public const string PHONE =
            @"^(?:1(?: |-))?(?:\(\d{3}\)|\d{3})(?: |-)?\d{3}(?: |-)?\d{4}(?:(?: |\S){1,3}\d{1,6})?$";

        public const string PHONE_ERROR_MESSAGE =
            "Please adjust your number to match one of the following formats: (123)123-1234 or (123)123-1234x1234";
    }
}