using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchProductionWorkDescription : SearchSet<ProductionWorkDescription>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OrderType))]
        public int? OrderType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        public int? PlantMaintenanceActivityType { get; set; }

        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MaintenancePlanTaskType))]
        public int? MaintenancePlanTaskType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TaskGroup))]
        public int? TaskGroup { get; set; }

        #endregion
    }
}
