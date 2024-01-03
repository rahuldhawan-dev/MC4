using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyMap : ClassMap<PublicWaterSupply>
    {
        #region Constants

        public const string TABLE_NAME = "PublicWaterSupplies";

        #endregion

        #region Constructors

        public PublicWaterSupplyMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.OperatingArea).Length(PublicWaterSupply.StringLengths.OPERATING_AREA);
            Map(x => x.System).Unique().Length(PublicWaterSupply.StringLengths.SYSTEM);
            Map(x => x.Identifier).Column("PWSID").Unique().Length(PublicWaterSupply.StringLengths.PWSID);
            Map(x => x.LIMSProfileNumber).Nullable();
            Map(x => x.LocalCertifiedStateId).Length(PublicWaterSupply.StringLengths.LOCAL_CERTIFIED_STATE_ID)
                                             .Nullable();
            Map(x => x.AWOwned).Nullable();
            Map(x => x.JanuaryRequiredBacterialWaterSamples).Column("Jan_Required_Bacti_Samples");
            Map(x => x.FebruaryRequiredBacterialWaterSamples).Column("Feb_Required_Bacti_Samples");
            Map(x => x.MarchRequiredBacterialWaterSamples).Column("Mar_Required_Bacti_Samples");
            Map(x => x.AprilRequiredBacterialWaterSamples).Column("Apr_Required_Bacti_Samples");
            Map(x => x.MayRequiredBacterialWaterSamples).Column("May_Required_Bacti_Samples");
            Map(x => x.JuneRequiredBacterialWaterSamples).Column("Jun_Required_Bacti_Samples");
            Map(x => x.JulyRequiredBacterialWaterSamples).Column("Jul_Required_Bacti_Samples");
            Map(x => x.AugustRequiredBacterialWaterSamples).Column("Aug_Required_Bacti_Samples");
            Map(x => x.SeptemberRequiredBacterialWaterSamples).Column("Sep_Required_Bacti_Samples");
            Map(x => x.OctoberRequiredBacterialWaterSamples).Column("Oct_Required_Bacti_Samples");
            Map(x => x.NovemberRequiredBacterialWaterSamples).Column("Nov_Required_Bacti_Samples");
            Map(x => x.DecemberRequiredBacterialWaterSamples).Column("Dec_Required_Bacti_Samples");
            Map(x => x.FreeChlorineReported).Not.Nullable();
            Map(x => x.TotalChlorineReported).Not.Nullable();
            Map(x => x.UsageLastYear).Nullable();
            Map(x => x.AnticipatedActiveDate).Nullable();
            Map(x => x.HasConsentOrder).Nullable();
            Map(x => x.AnticipatedMergerDate).Nullable();
            Map(x => x.ValidTo).Nullable();
            Map(x => x.ValidFrom).Nullable();
            Map(x => x.DateOfOwnership).Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.ConsentOrderStartDate).Nullable();
            Map(x => x.ConsentOrderEndDate).Nullable();
            Map(x => x.NewSystemInitialSafetyAssessmentCompleted).Nullable();
            Map(x => x.DateSafetyAssessmentActionItemsCompleted).Nullable();
            Map(x => x.NewSystemInitialWQEnvAssessmentCompleted).Nullable();
            Map(x => x.DateWQEnvAssessmentActionItemsCompleted).Nullable();

            HasMany(x => x.CustomerDataRecords).KeyColumn("PWSID").Inverse();

            References(x => x.CurrentPublicWaterSupplyFirmCapacity).Nullable();
            References(x => x.Status);
            References(x => x.State);
            References(x => x.AnticipatedMergePublicWaterSupply).Nullable();
            References(x => x.Coordinate);
            References(x => x.Ownership);
            References(x => x.Type);

            HasMany(x => x.EnvironmentalPermits)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade
               .None()
               .Inverse();
            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.SampleSites)
               .KeyColumn("PWSID").Inverse().Cascade.None();
            HasMany(x => x.OperatingCenterPublicWaterSupplies)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade.All()
               .Inverse();
            HasMany(x => x.LicensedOperators)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade.All()
               .Inverse();
            HasMany(x => x.WaterSampleComplianceForms)
               .KeyColumn("PublicWaterSupplyId");
            HasMany(x => x.PendingMergerPublicWaterSupplies)
               .KeyColumn("AnticipatedMergePublicWaterSupplyId");

            HasMany(x => x.FirmCapacities)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade
               .All()
               .Inverse();

            HasMany(x => x.PressureZones)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade
               .All()
               .Inverse();

            HasMany(x => x.PlanningPlantPublicWaterSupplies)
               .KeyColumn("PublicWaterSupplyId")
               .Cascade.All()
               .Inverse();
        }

        #endregion
    }
}
