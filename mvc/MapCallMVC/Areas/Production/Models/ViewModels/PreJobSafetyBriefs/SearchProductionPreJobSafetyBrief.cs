using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public class SearchProductionPreJobSafetyBrief : SearchSet<ProductionPreJobSafetyBrief>
    {
        #region Properties

        // Not a dropdown, they're just going to enter the id manually.
        [SearchAlias("ProductionWorkOrder", "Id")]
        public int? ProductionWorkOrder { get; set; }

        public DateRange SafetyBriefDateTime { get; set; }

        [SearchAlias("OperatingCenter", "State.Id", Required = true)]
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        public int? Facility { get; set; }

        [SearchAlias("Workers", "Employee.Id")]
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        public int? Employee { get; set; }

        [SearchAlias("Workers", "Contractor")]
        public SearchString Contractor { get; set; }

        #endregion
    }
}