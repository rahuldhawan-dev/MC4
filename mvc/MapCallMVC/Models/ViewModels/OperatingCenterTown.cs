using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    [Serializable]
    public class CreateOperatingCenterTown : ViewModel<OperatingCenterTown>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int OperatingCenter { get; set; }
        [EntityMap,EntityMustExist(typeof(Town))]
        public int Town { get; set; }
        [Required, StringLength(4)]
        public string Abbreviation { get; set; }

        [View(DisplayName = "Main SAP Equipment")]
        public int? MainSAPEquipmentId { get; set; }
        [DropDown("FieldOperations", "FunctionalLocation", "ByTownForMainAsset", DependsOn = "Town")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public int? MainSAPFunctionalLocation { get; set; }

        [View(DisplayName= "Sewer Main SAP Equipment")]
        public int? SewerMainSAPEquipmentId { get; set; }
        [DropDown("FieldOperations", "FunctionalLocation", "ByTownForSewerMainAsset", DependsOn = "Town")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public int? SewerMainSAPFunctionalLocation { get; set; }

        [DropDown("PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? DistributionPlanningPlant { get; set; }
        [DropDown("PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? SewerPlanningPlant { get; set; }
        
        #endregion

        #region Constructors

        public CreateOperatingCenterTown(IContainer container) : base(container) { }

        #endregion
    }
}