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
    public class CreatePlanningPlantWasteWaterSystem : ViewModel<PlanningPlantWasteWaterSystem>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(PlanningPlant)), Required]
        public int? PlanningPlant { get; set; }
        
        [EntityMap, EntityMustExist(typeof(WasteWaterSystem)), Required]
        public int? WasteWaterSystem { get; set; }

        public CreatePlanningPlantWasteWaterSystem(IContainer container) : base(container) { }
    }
}
