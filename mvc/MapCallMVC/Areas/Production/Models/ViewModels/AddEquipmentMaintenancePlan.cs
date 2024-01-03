using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddEquipmentMaintenancePlan : ViewModel<MaintenancePlan>
    {
        #region Properties

        [EntityMap(MapDirections.ToPrimary)] // Removed EntityMustExist and DoesNotAutoMap as we still want to map the values to the view model
        public int? Facility { get; set; }

        [EntityMap(MapDirections.ToPrimary)] // Removed EntityMustExist and DoesNotAutoMap as we still want to map the values to the view model
        public int[] EquipmentTypes { get; set; }

        [DoesNotAutoMap]
        [MultiSelect(Area = "", Controller = "Equipment", Action = "GetActiveInServiceSAPEquipmentWhereNotPresentInPlan", DependsOn = "Facility,EquipmentTypes,Id", PromptText = "This Plan is missing Facility or Equipment Type data.")]
        public int[] Equipment { get; set; }

        #endregion

        #region LogicalProps

        [DoesNotAutoMap]
        public string EquipmentTypeValues => String.Join(",", EquipmentTypes);

        #endregion

        #region Constructors

        public AddEquipmentMaintenancePlan(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            if (Equipment == null || Equipment.Length == 0)
            {
                return base.MapToEntity(entity);
            }

            var equipmentDoesNotAlreadyExistInEntity = new Func<Equipment, bool>(
                equipment => entity.Equipment.All(x => x.Id != equipment.Id));

            var selectedEquipment = _container.GetInstance<IRepository<Equipment>>().FindManyByIds(Equipment);
            var equipmentToAdd = selectedEquipment.Values.Where(equipmentDoesNotAlreadyExistInEntity);

            entity.Equipment.AddRange(equipmentToAdd);
            return entity;
        }

        #endregion
    }
}
