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
    public class CreatePlanningPlantPublicWaterSupply : ViewModel<PlanningPlantPublicWaterSupply>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(PlanningPlant)), Required]
        public int? PlanningPlant { get; set; }
        
        [EntityMap, EntityMustExist(typeof(PublicWaterSupply)), Required]
        public int? PublicWaterSupply { get; set; }

        public CreatePlanningPlantPublicWaterSupply(IContainer container) : base(container) { }
    }
}