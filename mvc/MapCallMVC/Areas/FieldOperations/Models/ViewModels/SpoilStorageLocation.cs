using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CreateSpoilStorageLocation : SpoilStorageLocationViewModel
    {
        #region Properties

        public override bool Active { get; set; } = true;

        #endregion

        #region Constructors

        public CreateSpoilStorageLocation(IContainer container) : base(container) { }

        #endregion
    }

    public class EditSpoilStorageLocation : SpoilStorageLocationViewModel
    {
        #region Constructors

        public EditSpoilStorageLocation(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(SpoilStorageLocation entity)
        {
            base.Map(entity);
            State = entity.OperatingCenter?.State?.Id;
        }

        #endregion
    }

    public class SpoilStorageLocationViewModel : ViewModel<SpoilStorageLocation>
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above"), EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a Town above"), EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; }

        public virtual bool Active { get; set; }

        #endregion

        #region Constructors

        public SpoilStorageLocationViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchSpoilStorageLocation : SearchSet<SpoilStorageLocation>
    {
        #region Properties

        [DropDown, Required, Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "WorkOrdersEnabledByStateId", DependsOn = "State", PromptText = "Please select a State above."), Required]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}