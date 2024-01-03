namespace MMSINC.Validation
{
    /// <summary>
    /// Used by RequiredWhenAttribute and CompareToAttribute for indicating the type of value comparison.
    /// </summary>
    public enum ComparisonType
    {
        // NOTE: These names are used for creating default error messages. Make sure they grammatically
        //       fit in to "X must be blah blah Y".
        EqualTo = 0, // Default,

        /// <summary>
        /// Indicates that a target value must be equal to any one item in an array of possible values.
        /// NOTE: Not used by CompareToAttribute.
        /// </summary>
        EqualToAny,
        Contains,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        NotEqualTo,

        /// <summary>
        /// Indicates that a target value must not be equal to any of the items in an array of values.
        /// NOTE: Not used by CompareToAttribute.
        /// </summary>
        NotEqualToAny,

        /// <summary>
        /// Indicates that a target value must fall within a range. The minimum and maximum values are
        /// included in this range.
        /// </summary>
        Between,

        /// <summary>
        /// Indicates that a target value must fall outside of a range. The minimum and maximum values
        /// are included in this range.
        /// </summary>
        NotBetween
    }
}
