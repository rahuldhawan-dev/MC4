using MapCall.Common.Model.Entities;
using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RemoveAllEquipmentMaintenancePlan : ViewModel<MaintenancePlan>
    {
        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            var linkedEquipment = entity.Equipment.ToList();
            foreach (var equipment in linkedEquipment)
            {
                entity.Equipment.Remove(equipment);
            }
            return entity;
        }

        public RemoveAllEquipmentMaintenancePlan(IContainer container) : base(container) { }
    }
}