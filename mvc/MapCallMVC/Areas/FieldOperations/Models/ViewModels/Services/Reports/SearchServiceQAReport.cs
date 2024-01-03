using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    public class SearchServiceQAReport
        : SearchSet<ServiceQualityAssuranceReportItem>, ISearchServiceQAReport
    {
        [SearchAlias("Town", "State.Id"), MultiSelect, EntityMustExist(typeof(State)), EntityMap]
        public int[] State { get; set; }
        
        [MultiSelect(
            "",
            "OperatingCenter",
            "ByStateIdForFieldServicesAssets",
            DependsOn = "State",
            DependentsRequired = DependentRequirement.None)]
        public int[] OperatingCenter { get; set; }
        
        [View("Category of Service"), MultiSelect, EntityMustExist(typeof(ServiceCategory)), EntityMap]
        public int[] ServiceCategory { get; set; }
        
        [SearchAlias("Premise", "PublicWaterSupply.Id")]
        [MultiSelect(
            "",
            "PublicWaterSupply",
            "ByOperatingCenterOrState",
            DependsOn = "State,OperatingCenter",
            DependentsRequired = DependentRequirement.One,
            PromptText = "Select at least one State or Operating Center above")]
        public int[] PublicWaterSupply { get; set; }

        public DateRange DateInstalled { get; set; }
    }
}