using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using StructureMap;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Utilities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditMaintenancePlan : MaintenancePlanViewModel
    {
        private readonly DateTime _today;

        #region Constructors

        public EditMaintenancePlan(IContainer container, IDateTimeProvider dateTime) : base(container)
        {
            _today = dateTime.GetCurrentDate().Date;
        }

        #endregion

        #region Exposed Methods

        public override void Map(MaintenancePlan entity)
        {
            base.Map(entity);
            EquipmentTypes = entity.EquipmentTypes.Select(x => x.Id).ToArray();
            EquipmentPurposes = entity.EquipmentPurposes.Select(x => x.Id).ToArray();
            Facility = entity.Facility.Id;
        }

        public override MaintenancePlan MapToEntity(MaintenancePlan entity)
        {
            var currentMaintenancePlan = _container.GetInstance<IRepository<MaintenancePlan>>().Find(entity.Id);
            var currentMaintenancePlanIsActive = currentMaintenancePlan.IsActive;

            // We need to check if any changes to this plan will cause the scheduling forecast to change.
            // If it will change, we need to trim out the scheduled assignments to prevent any orphaned assignment records
            var oldForecast = GetForecastSample(entity);
            var mapped = base.MapToEntity(entity);
            var newForecast = GetForecastSample(mapped);

            if (oldForecast.Count() != newForecast.Count())
            {
                mapped.ScheduledAssignments.Clear();
            }

            // we only want them setting the fields if it was changed to InActive
            if (currentMaintenancePlanIsActive != false && mapped.IsActive == false)
            {
                mapped.DeactivationDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                var employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
                mapped.DeactivationEmployee = employee;
            }

            // Blank out OtherComplianceReason if OtherComplianceFlag is false
            if (mapped.HasOtherCompliance != true)
            {
                mapped.OtherComplianceReason = null;
            }

            return mapped;
        }

        #endregion

        #region Private Methods

        private IEnumerable<DateTime> GetForecastSample(MaintenancePlan entity)
        {
            return entity.ProductionWorkOrderFrequency.GetFrequencyDates(_today, _today.AddYears(1));
        }

        #endregion
    }
}
