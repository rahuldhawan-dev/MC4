using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RemoveEquipmentMaintenancePlan : ViewModel<MaintenancePlan>
    {
        [DoesNotAutoMap]
        public int[] Equipment { get; set; }

        public RemoveEquipmentMaintenancePlan(IContainer container) : base(container) { }

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            var plans = _container.GetInstance<IRepository<Equipment>>()
                                  .Where(x => Equipment.Contains(x.Id));

            foreach (var plan in plans)
            {
                entity.Equipment.Remove(plan);
            }

            return entity;
        }
    }
}
