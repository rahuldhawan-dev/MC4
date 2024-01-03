using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderMap : ClassMap<ProductionWorkOrder>
    {
        #region

        public readonly string OPEN_LOCKOUT_FORMS =
            $"(CASE WHEN EXISTS (SELECT 1 FROM {nameof(LockoutForm)}s L WHERE L.ProductionWorkOrderId = Id AND L.ReturnedToServiceDateTime IS NULL) THEN 1 ELSE 0 END)";

        #endregion

        #region Constants

        private const string DAYS_OVERDUE_SQL =
                                 "(CASE WHEN DateCompleted IS NULL THEN (datediff(day, DueDate, GetDate())) ELSE (datediff(day, DueDate, DateCompleted)) END)",
                             DAYS_OVERDUE_SQLITE =
                                 "(CASE WHEN DateCompleted IS NULL THEN (JulianDay(date('now')) - JulianDay(date(DueDate))) ELSE (JulianDay(date(DateCompleted)) - JulianDay(date(DueDate))) END)";

        #endregion

        public ProductionWorkOrderMap()
        {
            Id(x => x.Id);

            LazyLoad();

            Map(x => x.ApprovedOn).Nullable();
            Map(x => x.DateCompleted).Nullable();
            Map(x => x.FunctionalLocation).Nullable();
            Map(x => x.OrderNotes, "Notes").Nullable();
            Map(x => x.DateReceived).Nullable();
            Map(x => x.BreakdownIndicator).Nullable();
            Map(x => x.SAPWorkOrder).Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
            Map(x => x.WBSElement).Nullable();
            Map(x => x.MaterialsApprovedOn).Nullable();
            Map(x => x.MaterialsPlannedOn).Nullable();
            Map(x => x.DateCancelled).Nullable();
            Map(x => x.CapitalizationReason).Nullable();
            Map(x => x.BasicStart).Nullable();
            Map(x => x.BasicFinish).Nullable();
            Map(x => x.OtherProblemNotes).Nullable();
            Map(x => x.HasAssignmentsOnNonCancelledWorkOrder).Nullable().Formula(
                $"(CASE WHEN DateCancelled IS NOT NULL THEN NULL ELSE CASE WHEN EXISTS (SELECT 1 FROM {nameof(EmployeeAssignment)}s ea WHERE ea.ProductionWorkOrderId = Id) THEN 1 ELSE 0 END END)");
            Map(x => x.IsOpen).Not.Nullable()
                              .Formula(
                                   "(SELECT CASE WHEN DateCancelled IS NULL AND DateCompleted IS NULL THEN 1 ELSE 0 END)");
            Map(x => x.SAPMaintenancePlanId).Nullable();
            Map(x => x.IsLockoutFormStillOpen).Not.Nullable().Formula(OPEN_LOCKOUT_FORMS);

            Map(x => x.NeedsRedTagPermitAuthorization).Nullable();
            Map(x => x.NeedsRedTagPermitAuthorizedOn).Nullable();
            Map(x => x.IsRedTagPermitStillOpen).Not.Nullable()
                                               .Formula("(case when exists (select 1 from RedTagPermits rtp where rtp.ProductionWorkOrderId = Id and rtp.EquipmentRestoredOn is null) then 1 else 0 end)");
            Map(x => x.EstimatedCompletionHours).Not.Nullable();
            Map(x => x.LocalTaskDescription).Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.DueDate).Nullable();
            Map(x => x.AutoCreatedCorrectiveWorkOrder).Not.Nullable();
            Map(x => x.AssignedOnDate).Nullable().LazyLoad()
                                      .Formula("(SELECT MAX(ea.AssignedFor) FROM EmployeeAssignments ea WHERE ea.ProductionWorkOrderId = Id)");
            Map(x => x.DaysOverdue)
               .DbSpecificFormula(DAYS_OVERDUE_SQL, DAYS_OVERDUE_SQLITE);

            References(x => x.NeedsRedTagPermitAuthorizedBy).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PlanningPlant).Nullable();
            References(x => x.Facility).Nullable().LazyLoad();
            References(x => x.FacilityFacilityArea).Nullable();
            References(x => x.EquipmentType).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.Priority).Nullable();
            References(x => x.ProductionWorkDescription).Not.Nullable();
            References(x => x.CapitalizedFrom).Nullable();
            References(x => x.RequestedBy).Nullable();
            References(x => x.CompletedBy).Nullable();
            References(x => x.CancelledBy).Nullable();
            References(x => x.ApprovedBy).Nullable();
            References(x => x.MaterialsApprovedBy).Nullable();
            References(x => x.CancellationReason).Nullable();
            References(x => x.PlantMaintenanceActivityTypeOverride).Nullable();
            References(x => x.CorrectiveOrderProblemCode).Nullable();
            References(x => x.ActionCode).Nullable();
            References(x => x.FailureCode).Nullable();
            References(x => x.CauseCode).Nullable();
            References(x => x.MaintenancePlan).Nullable();
            HasOne(x => x.RedTagPermit).PropertyRef(x => x.ProductionWorkOrder);

            HasOne(x => x.ProductionWorkOrderRequiresSupervisorApproval).PropertyRef(x => x.ProductionWorkOrder);
            HasMany(x => x.ConfinedSpaceForms).KeyColumn("ProductionWorkOrderId").Inverse().Cascade.None();
            HasMany(x => x.WellTests).KeyColumn("ProductionWorkOrderId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ProductionPreJobSafetyBriefs).KeyColumn("ProductionWorkOrderId").Inverse().Cascade.None();
            HasMany(x => x.CurrentAssignments)
               .Subselect(
                    $"(SELECT a.Id FROM {nameof(EmployeeAssignment)}s a WHERE a.DateEnded IS NULL AND a.ProductionWorkOrderId = Id ORDER BY a.Id desc)");
            HasMany(x => x.ProductionWorkOrderProductionPrerequisites).KeyColumn("ProductionWorkOrderId").Cascade
                                                                      .AllDeleteOrphan().Inverse();
            HasMany(x => x.ProductionWorkOrderDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ProductionWorkOrderNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.EmployeeAssignments).KeyColumn("ProductionWorkOrderId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ProductionWorkOrderMaterialUsed).KeyColumn("ProductionWorkOrderId").Cascade.AllDeleteOrphan()
                                                           .Inverse();
            HasMany(x => x.JobObservations).KeyColumn("ProductionWorkOrderId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Equipments).KeyColumn("ProductionWorkOrderId").LazyLoad().Inverse().Cascade
                                      .AllDeleteOrphan();
            HasMany(x => x.ProductionWorkOrderMeasurementPointValues).KeyColumn("ProductionWorkOrderId").LazyLoad()
                                                                     .Inverse().Cascade.None();
            HasMany(x => x.LockoutForms).KeyColumn("ProductionWorkOrderId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.AssetReliabilities).KeyColumn("ProductionWorkOrderId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.TankInspections).KeyColumn("ProductionWorkOrderId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
