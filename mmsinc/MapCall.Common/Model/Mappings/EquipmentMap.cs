using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentMap : ClassMap<Equipment>
    {
        #region Constants

        public const string TABLE_NAME = "Equipment";

        public const string SERVICE_LIFE_SQL = "(SELECT (datediff(year, DateInstalled, GetDate())))",
                            SERVICE_LIFE_SQLITE = "(SELECT ROUND ((JulianDay(date('now')) - JulianDay(date(DateInstalled))) / 365.25))",
                            HAS_COMPLIANCE_PLAN_SQL =
                                "(SELECT CASE WHEN HasProcessSafetyManagement = 1 OR HasCompanyRequirement = 1 OR HasRegulatoryRequirement = 1 OR HasOshaRequirement = 1 OR OtherCompliance = 1 THEN 1 ELSE 0 END)";
        
        #endregion

        #region Constructors

        public EquipmentMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "EquipmentID");

            References(x => x.Coordinate);
            References(x => x.EquipmentPurpose).Column("PurposeId").Nullable().Fetch.Join();
            References(x => x.EquipmentType).Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.FacilityFacilityArea);
            References(x => x.EquipmentStatus).Column("StatusID").Fetch.Join();
            References(x => x.EquipmentModel).Column("ModelID").Cascade.All().Fetch.Join();
            //References(x => x.FunctionalLocation);
            References(x => x.ABCIndicator);
            References(x => x.EquipmentManufacturer).Fetch.Join();
            References(x => x.RequestedBy).Nullable();
            References(x => x.AssetControlSignOffBy).Nullable();
            References(x => x.CreatedBy).Nullable();
            References(x => x.ReplacedEquipment).Nullable();
            References(x => x.ScadaTagName).Nullable();
            References(x => x.ParentEquipment).Nullable();
            References(x => x.OperatingCenter)
               .Formula(
                    "(SELECT OC.OperatingCenterID from OperatingCenters OC INNER JOIN tblFacilities F ON OC.OperatingCenterId = F.OperatingCenterId where F.RecordId = FacilityId)");
            //RiskCharacteristics
            References(x => x.Condition).Nullable();
            References(x => x.Performance, "PerformanceRatingId").Nullable();
            References(x => x.StaticDynamicType).Nullable();
            References(x => x.ConsequenceOfFailure).Nullable();
            References(x => x.LikelyhoodOfFailure).Nullable();
            References(x => x.Reliability).Nullable();
            References(x => x.RiskOfFailure).Nullable();
            References(x => x.ReplacementProductionWorkOrder).Nullable();
            References(x => x.RiskCharacteristicsLastUpdatedBy).Nullable();

            Map(x => x.RiskCharacteristicsLastUpdatedOn).Nullable();
            Map(x => x.FunctionalLocation).Column("FunctionalLocationID").Nullable();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.Number).Nullable();
            Map(x => x.CriticalRating);
            Map(x => x.CriticalNotes).Nullable();
            Map(x => x.SerialNumber);
            Map(x => x.DateInstalled);
            Map(x => x.DateRetired);
            Map(x => x.PSMTCPA);
            Map(x => x.SafetyNotes);
            Map(x => x.MaintenanceNotes);
            Map(x => x.OperationNotes);
            Map(x => x.SAPEquipmentId).Nullable().Unique();
            Map(x => x.WBSNumber).Nullable();
            Map(x => x.SAPEquipmentIdBeingReplaced).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.AssetControlSignOffDate).Nullable();
            Map(x => x.IsReplacement).Not.Nullable();
            Map(x => x.Portable).Not.Nullable();
            Map(x => x.ArcFlashHierarchy);
            Map(x => x.ArcFlashRating);
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.ManufacturerOther).Nullable();
            Map(x => x.LocalizedRiskOfFailure).Nullable();
            Map(x => x.Legacy).Length(Equipment.StringLengths.LEGACY).Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.HasProcessSafetyManagement);
            Map(x => x.HasCompanyRequirement);
            Map(x => x.HasRegulatoryRequirement);
            Map(x => x.HasOshaRequirement);
            Map(x => x.OtherCompliance);
            Map(x => x.OtherComplianceReason).Length(Equipment.StringLengths.OTHER_COMPLIANCE_REASON).Nullable();
            Map(x => x.PlannedReplacementYear).Nullable();
            Map(x => x.EstimatedReplaceCost).Precision(8).Scale(2).Nullable();
            Map(x => x.ExtendedUsefulLifeWorkOrderId).Nullable();
            Map(x => x.LifeExtendedOnDate).Nullable();
            Map(x => x.ExtendedUsefulLifeComment).Length(Equipment.StringLengths.EXTENDED_USEFUL_LIFE_COMMENT).Nullable();
            Map(x => x.HasComplianceRequirement)
               .Formula(HAS_COMPLIANCE_PLAN_SQL);
            Map(x => x.ServiceLife)
               .DbSpecificFormula(SERVICE_LIFE_SQL, SERVICE_LIFE_SQLITE);

            HasMany(x => x.EquipmentDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.EquipmentNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.FilterMediae).KeyColumn("EquipmentId").LazyLoad().Inverse().Fetch.Join().AsSet();
            HasMany(x => x.Characteristics)
               .KeyColumn("EquipmentId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.WorkOrders).KeyColumn("EquipmentId");
            HasMany(x => x.TankInspections).KeyColumn("EquipmentId").LazyLoad().Cascade.DeleteOrphan().Inverse();
            HasMany(x => x.ProductionWorkOrderEquipment).KeyColumn("EquipmentId");
            HasMany(x => x.Sensors).KeyColumn("EquipmentId").LazyLoad()
                                   .Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Videos)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            HasMany(x => x.Links).KeyColumn("EquipmentId").LazyLoad().Cascade.AllDeleteOrphan().Inverse()
                                 .AsSet();
            HasMany(x => x.MaintenancePlans).KeyColumn("EquipmentId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.RedTagPermits).KeyColumn("EquipmentId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.AssetReliabilities).KeyColumn("EquipmentId").LazyLoad().Cascade.None().Inverse();
            HasMany(x => x.WellTests).KeyColumn("EquipmentId").Cascade.AllDeleteOrphan().Inverse();

            HasManyToMany(x => x.ProductionPrerequisites)
               .Table("EquipmentProductionPrerequisites")
               .ParentKeyColumn("EquipmentId")
               .ChildKeyColumn("ProductionPrerequisiteId")
               .Cascade.All();
            HasManyToMany(x => x.EnvironmentalPermits)
               .Table("EnvironmentalPermitsEquipment")
               .ParentKeyColumn("EquipmentId")
               .ChildKeyColumn("EnvironmentalPermitId");

            HasOne(eq => eq.Generator).PropertyRef(gd => gd.Equipment);

            Map(x => x.HasSensorAttached)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM TFProd_Equipment_Sensor tes WHERE tes.EquipmentID = EquipmentID) THEN 1 ELSE 0 END)")
               .ReadOnly();

            Map(x => x.HasOpenLockoutForms)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM LockOutForms lock WHERE lock.EquipmentID = EquipmentID AND lock.ReturnedToServiceDateTime is null) THEN 1 ELSE 0 END)")
               .ReadOnly();

            Map(x => x.HasOpenRedTagPermits)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM RedTagPermits rtp WHERE rtp.EquipmentID = EquipmentID AND rtp.EquipmentRestoredOn IS NULL) THEN 1 ELSE 0 END)")
               .ReadOnly();

            Map(x => x.HasNoSAPEquipmentId)
               .Formula("(CASE WHEN SAPEquipmentId IS NULL THEN 1 WHEN SAPEquipmentID = '' THEN 1 ELSE 0 END)")
               .ReadOnly();

            Map(x => x.IsSignedOffByAssetControl)
               .Formula("(CASE WHEN AssetControlSignOffById IS NOT NULL THEN 1 ELSE 0 END)").ReadOnly();
        }

        #endregion
    }
}
