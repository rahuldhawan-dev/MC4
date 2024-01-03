using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.ViewModels
{
    public class NpdesRegulatorsDueInspectionReportItem
    {
        public string OperatingCenter { get; set; }

        [DoesNotExport]
        public int OperatingCenterId { get; set; }

        public string Town { get; set; }

        [DoesNotExport]
        public int TownId { get; set; }

        public string Status { get; set; }

        [DoesNotExport]
        public int StatusId { get; set; }

        [DoesNotExport]
        public DateRange DepartureDateTime { get; set; }

        public int Count { get; set; }
    }
}
