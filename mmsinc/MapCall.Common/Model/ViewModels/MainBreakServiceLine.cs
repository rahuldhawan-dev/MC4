using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class MainBreakReportItem
    {
        public string State { get; set; }
        public string OperatingCenterCode { get; set; }
        public string OperatingCenterName { get; set; }
        public string WorkDescription { get; set; }
        public int MonthCompleted { get; set; }
        public int MonthTotal { get; set; }
        public int Year { get; set; }
        public string OperatingCenter => $"{OperatingCenterCode} - {OperatingCenterName}";
    }

    public class MainBreakReport : MonthlyReportViewModel
    {
        public string State { get; set; }
        public string OperatingCenter { get; set; }
        public string WorkDescription { get; set; }
    }

    public interface ISearchMainBreakReport : ISearchSet<MainBreakReportItem>
    {
        [SearchAlias("wo.State", "s", "Id")]
        int? State { get; set; }

        [SearchAlias("wo.OperatingCenter", "opc", "Id")]
        int[] OperatingCenter { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }

        [SearchAlias("OperatingCenter", "oc", "IsContractedOperations")]
        bool? IsContractedOperations { get; set; }
    }
}
