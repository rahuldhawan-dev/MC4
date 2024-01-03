using System.ComponentModel;

namespace MMSINC.Data
{
    /// <summary>
    /// Defines the comparison used for DateRange and NumericRange objects.
    /// </summary>
    public enum RangeOperator
    {
        Between,
        [Description("=")] Equal,
        [Description(">")] GreaterThan,
        [Description(">=")] GreaterThanOrEqualTo,
        [Description("<")] LessThan,
        [Description("<=")] LessThanOrEqualTo
    }
}
