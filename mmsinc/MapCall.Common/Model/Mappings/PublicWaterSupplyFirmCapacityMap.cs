using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyFirmCapacityMap : ClassMap<PublicWaterSupplyFirmCapacity>
    {
        #region Constants

        public const string TABLE_NAME = "PublicWaterSupplyFirmCapacities";

        #endregion

        #region Constructors

        public PublicWaterSupplyFirmCapacityMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.CurrentSystemPeakDailyDemandMGD).Nullable();
            Map(x => x.CurrentSystemPeakDailyDemandYearMonth).Nullable();
            Map(x => x.TotalSystemSourceCapacityMGD).Nullable();
            Map(x => x.FirmCapacityMultiplier).Precision(5).Scale(4).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.TotalCapacityFacilitySumMGD).Nullable().Formula(
                "(SELECT SUM(tf.FacilityTotalCapacityMGD) FROM tblFacilities tf WHERE tf.PublicWaterSupplyId = PublicWaterSupplyId AND tf.UsedInProductionCapacityCalculation = 1)");

            References(x => x.PublicWaterSupply).Nullable().Cascade.SaveUpdate();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
