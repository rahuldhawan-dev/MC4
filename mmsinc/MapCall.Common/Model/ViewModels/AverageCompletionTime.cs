using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class AverageCompletionTime
    {
        public string OperatingCenter { get; set; }

        [View("Average Time to Complete (hrs)", FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes)]
        public double? Completion { get; set; }

        [View("Average time to Approve (hrs)", FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes)]
        public double? Approval { get; set; }

        [View("Average time to issue stock (hrs)", FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes)]
        public double? StockApproval { get; set; }

        [View("Average Man Hours to Complete (hrs)", FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes)]
        public double? ManHours { get; set; }
    }
}
