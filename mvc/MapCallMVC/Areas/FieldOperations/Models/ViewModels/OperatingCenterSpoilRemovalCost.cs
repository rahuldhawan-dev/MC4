using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class OperatingCenterSpoilRemovalCostViewModel : ViewModel<OperatingCenterSpoilRemovalCost>
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State",
             PromptText = "Please select a State above."), Required, EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required]
        public int Cost { get; set; }

        #endregion

        #region Constructors

        public OperatingCenterSpoilRemovalCostViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateOperatingCenterSpoilRemovalCost : OperatingCenterSpoilRemovalCostViewModel
    {
        #region Constructors

        public CreateOperatingCenterSpoilRemovalCost(IContainer container) : base(container) { }

        #endregion
    }

    public class EditOperatingCenterSpoilRemovalCost : OperatingCenterSpoilRemovalCostViewModel
    {
        #region Constructors

        public EditOperatingCenterSpoilRemovalCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(OperatingCenterSpoilRemovalCost entity)
        {
            base.Map(entity);
            State = entity.OperatingCenter?.State?.Id;
        }

        #endregion
    }

    public class SearchOperatingCenterSpoilRemovalCost : SearchSet<OperatingCenterSpoilRemovalCost>
    {
        [DropDown, Required, Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), Required]
        public int? OperatingCenter { get; set; }
    }
}