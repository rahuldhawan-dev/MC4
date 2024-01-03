using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateMaintenancePlan : CreateMaintenancePlanBase
    {
        #region Properties

        [DropDown(Area = "", Controller = "OperatingCenter", Action = "ActiveByStateIdOrAll", DependsOn = nameof(State),
            PromptText = "Please select a state above")]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter;
            set => base.OperatingCenter = value;
        }

        [Required, EntityMap, EntityMustExist(typeof(Equipment))]
        [MultiSelect(Area = "", Controller = "Equipment", Action = "GetActiveInServiceSAPEquipmentByFacilityIdsOrEquipmentTypeIds", DependsOn = "Facility,EquipmentTypes", PromptText = "Please select a facility and equipment type above")]
        public int[] Equipment { get; set; }

        #endregion

        #region Constructors

        public CreateMaintenancePlan(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(MaintenancePlan entity)
        {
            base.Map(entity);
            EquipmentTypes = entity.EquipmentTypes.Select(x => x.Id).ToArray();
            EquipmentPurposes = entity.EquipmentPurposes.Select(x => x.Id).ToArray();
            Facility = entity.Facility.Id;
            Equipment = entity.Equipment.Select(x => x.Id).ToArray();
        }

        #endregion
    }

    public abstract class CreateMaintenancePlanBase : MaintenancePlanViewModel
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(Equipment))]
        [MultiSelect(Area = "", Controller = "Equipment", Action = "ActiveSAPByFacilityIdsOrEquipmentPurposeIds", DependsOn = "Facility,EquipmentTypes", PromptText = "Please select a facility and equipment type above")]
        public int[] Equipment { get; set; }

        #endregion

        #region Constructors

        protected CreateMaintenancePlanBase(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            entity = base.MapToEntity(entity);
            entity.ForecastPeriodMultiplier = 1.0m;
            entity.IsPlanPaused = false;
            entity.WorkDescription = _container.GetInstance<ProductionWorkDescriptionRepository>().GetMaintenancePlanWorkDescription();

            if (entity.IsActive == false)
            {
                var employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
                entity.DeactivationDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                entity.DeactivationEmployee = employee;
            }

            return entity;
        }

        #endregion
    }
}