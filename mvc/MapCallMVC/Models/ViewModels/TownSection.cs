using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateTownSection : TownSectionViewModel
    {
        #region Constructors

        public CreateTownSection(IContainer container) : base(container) { }

        #endregion
    }

    public class EditTownSection : TownSectionViewModel
    {
        #region Constructors

        public EditTownSection(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(TownSection entity)
        {
            base.Map(entity);
            State = entity.Town.State.Id;
            County = entity.Town.County.Id;
        }

        #endregion
    }

    public class TownSectionViewModel : ViewModel<TownSection>
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "County", "ByStateId", DependsOn = "State"), Required, EntityMap(MapDirections.None), EntityMustExist(typeof(County))]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County"), Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [Required]
        public string Name { get; set; }

        public string Abbreviation { get; set; }
        public string ZipCode { get; set; }
        public bool Active { get; set; }

        [View(DisplayName = "Main SAP Equipment")]
        public int? MainSAPEquipmentId { get; set; }
        [DropDown("FieldOperations", "FunctionalLocation", "ByTownId", DependsOn = "Town")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public int? MainSAPFunctionalLocation { get; set; }

        [View(DisplayName = "Sewer Main SAP Equipment")]
        public int? SewerMainSAPEquipmentId { get; set; }
        [DropDown("FieldOperations", "FunctionalLocation", "ByTownId", DependsOn = "Town")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public int? SewerMainSAPFunctionalLocation { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? DistributionPlanningPlant { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? SewerPlanningPlant { get; set; }

        #endregion

        #region Constructors

        public TownSectionViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchTownSection : SearchSet<TownSection>
    {
        #region Properties

        [DropDown, Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "County", "ByStateId", DependsOn = "State"), Search(CanMap = false)]
        public int? County { get; set; }

        [DropDown("", "Town", "ByCountyId", DependsOn = "County")]
        public int? Town { get; set; }

        public SearchString Name { get; set; }
        public SearchString Abbreviation { get; set; }

        #endregion
    }
}