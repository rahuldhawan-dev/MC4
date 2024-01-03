using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class SearchConfinedSpaceForm : SearchSet<ConfinedSpaceForm>
    {
        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }
        
        [MultiSelect("", "OperatingCenter", "ByStateId", DependsOn = "State")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }
   
        [SearchAlias("ProductionWorkOrder", "Facility.Id")]
        [DropDown("", "Facility", "ByOperatingCenterIds", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }
        
        [View(DisplayName = "Date")]
        public DateRange GeneralDateTime { get; set; }

        [SearchAlias("Entrants", "EntrantType.Id")]
        [ DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(ConfinedSpaceFormEntrantType))]
        public int? EntrantType { get; set; }
    }
}