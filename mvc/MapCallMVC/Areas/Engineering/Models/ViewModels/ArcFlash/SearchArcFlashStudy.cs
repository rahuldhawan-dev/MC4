using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash
{
    public class SearchArcFlashStudy : SearchSet<ArcFlashStudy>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("Facility.OperatingCenter", "State.Id")]
        public int? State { get; set; }
        
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        [SearchAlias("Facility", "OperatingCenter.Id", Required = true)]
        [EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        
        [EntityMustExist(typeof(Facility)), EntityMap]
        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center.")]
        public int? Facility { get; set; }
        
        [DropDown,EntityMap, EntityMustExist(typeof(ArcFlashStatus))]
        public int? ArcFlashStatus { get; set; }
        
        [View(DisplayName = "Last Date Validated")]
        public DateRange DateLabelsApplied { get; set; }
    }
}
