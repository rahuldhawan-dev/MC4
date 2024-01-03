using System.Xml.Linq;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MaintenancePlanMap : ClassMap<MaintenancePlan>
    {
        #region Constants

        private const string TABLE_NAME = "MaintenancePlans";
        private const string PARENT_KEY_COLUMN = "MaintenancePlanId";

        public const string PLAN_NUMBER_SQL =
            "(SELECT '9' + Right('00000000' + cast(Id as varchar(8)), 8))";

        public const string PLAN_NUMBER_SQLITE =
            "(SELECT '9' || SUBSTR('00000000' || cast(Id as varchar(8)), -8))";

        public const string HAS_COMPLIANCE_PLAN_SQL =
            "(SELECT CASE" +
            "   WHEN HasCompanyRequirement = 1 THEN 1" +
            "   WHEN HasOshaRequirement = 1 THEN 1" +
            "   WHEN HasPsmRequirement = 1 THEN 1" +
            "   WHEN HasRegulatoryRequirement = 1 THEN 1" +
            "   WHEN HasOtherCompliance = 1 THEN 1" +
            "   ELSE 0" +
            "   END)";

        #endregion

        #region Constructors

        public MaintenancePlanMap()
        {
            Table(TABLE_NAME);
            
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.PlanningPlant).Not.Nullable();
            References(x => x.TaskGroup).Not.Nullable();
            References(x => x.TaskGroupCategory).Not.Nullable();
            References(x => x.WorkDescription).Not.Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.ProductionWorkOrderFrequency).Nullable();
            References(x => x.SkillSet).Nullable();
            References(x => x.DeactivationEmployee).Nullable();
            HasOne(x => x.CountEquipmentMaintenancePlansByMaintenancePlan);

            Map(x => x.Resources).Nullable();
            Map(x => x.EstimatedHours).Nullable();
            Map(x => x.ContractorCost).Nullable();
            Map(x => x.Start).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.HasACompletionRequirement).Not.Nullable();
            Map(x => x.ForecastPeriodMultiplier).Not.Nullable();
            Map(x => x.IsPlanPaused).Not.Nullable();
            Map(x => x.PausedPlanNotes).Nullable();
            Map(x => x.PausedPlanResumeDate).Nullable();
            Map(x => x.AdditionalTaskDetails).Nullable();
            Map(x => x.HasCompanyRequirement).Not.Nullable();
            Map(x => x.HasOshaRequirement).Not.Nullable();
            Map(x => x.HasPsmRequirement).Not.Nullable();
            Map(x => x.HasRegulatoryRequirement).Not.Nullable();
            Map(x => x.HasOtherCompliance).Not.Nullable();
            Map(x => x.OtherComplianceReason).Nullable();
            Map(x => x.LocalTaskDescription).Nullable();
            Map(x => x.DeactivationReason).Nullable();
            Map(x => x.DeactivationDate).Nullable();
            Map(x => x.PlanNumber)
               .DbSpecificFormula(PLAN_NUMBER_SQL, PLAN_NUMBER_SQLITE);
            Map(x => x.HasComplianceRequirement)
               .Formula(HAS_COMPLIANCE_PLAN_SQL);

            HasMany(x => x.ProductionWorkOrders).KeyColumn(PARENT_KEY_COLUMN).Cascade.AllDeleteOrphan().Inverse();

            HasManyToMany(x => x.FacilityAreas)
               .Table("MaintenancePlansFacilityFacilityAreas")
               .ParentKeyColumn(PARENT_KEY_COLUMN)
               .ChildKeyColumn("FacilitiesFacilityAreaId")
               .Cascade.None();

            HasManyToMany(x => x.EquipmentTypes)
               .Table("EquipmentTypesMaintenancePlan")
               .ParentKeyColumn(PARENT_KEY_COLUMN)
               .ChildKeyColumn("EquipmentTypeId")
               .Cascade.None();

            HasManyToMany(x => x.EquipmentPurposes)
               .Table("EquipmentPurposesMaintenancePlan")
               .ParentKeyColumn(PARENT_KEY_COLUMN)
               .ChildKeyColumn("EquipmentPurposeId")
               .Cascade.None();

            HasManyToMany(x => x.Equipment)
               .Table("EquipmentMaintenancePlans")
               .ParentKeyColumn(PARENT_KEY_COLUMN)
               .ChildKeyColumn("EquipmentId")
               .Cascade.None();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.ScheduledAssignments).KeyColumn("MaintenancePlanId").Cascade.AllDeleteOrphan().Inverse();
        }

        #endregion
    }
}