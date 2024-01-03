using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class TrainingModulePositionGroupCommonNameReportItem
    {
        public string ModuleCategory { get; set; }
        public string ModuleTitle { get; set; }
        public string PositionGroupCommonName { get; set; }
        public bool IsOSHARequirement { get; set; }
    }

    public interface
        ISearchTrainingModulePositionGroupCommonName : ISearchSet<TrainingModulePositionGroupCommonNameReportItem>
    {
        [Search(CanMap = false)]
        int? TrainingModule { get; set; }

        [Search(CanMap = false)]
        bool? IsOSHARequirement { get; set; }

        int[] Ids { get; set; }
    }
}
