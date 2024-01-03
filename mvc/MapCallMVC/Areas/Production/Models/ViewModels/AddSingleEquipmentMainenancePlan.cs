using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddSingleEquipmentMaintenancePlan : ViewModel<Equipment>
    {
        [DoesNotAutoMap]
        public int Facility { get; set; }

        [DoesNotAutoMap]
        public int EquipmentType { get; set; }

        [DoesNotAutoMap]

        [DropDown(Area = "Production", Controller = "MaintenancePlan", Action = "ByFacilityIdAndEquipmentTypeId", DependsOn = "Facility,EquipmentType")]
        [Required]
        public int? MaintenancePlan { get; set; }

        [DoesNotAutoMap]
        public string ControlId { get; set; }

        public AddSingleEquipmentMaintenancePlan(IContainer container) : base(container) { }

        public override Equipment MapToEntity(Equipment entity)
        {
            //Don't add if it's already a plan it's linked to.
            if (MaintenancePlan != null && entity.MaintenancePlans.All(p => p.MaintenancePlan.Id != MaintenancePlan))
            {
                entity.MaintenancePlans.Add(new EquipmentMaintenancePlan {
                    MaintenancePlan =
                        _container.GetInstance<IRepository<MaintenancePlan>>().Find(MaintenancePlan.Value),
                    Equipment = entity
                });
            }

            return entity;
        }
    }
}
