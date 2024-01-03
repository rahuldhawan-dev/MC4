using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PublicWaterSupplyPressureZoneMap : ClassMap<PublicWaterSupplyPressureZone>
    {
        #region Constants

        public const string TABLE_NAME = "PublicWaterSupplyPressureZones";

        #endregion

        #region Constructors

        public PublicWaterSupplyPressureZoneMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id)
               .GeneratedBy.Identity();

            Map(x => x.HydraulicModelName)
               .Length(PublicWaterSupplyPressureZone.StringLengths.HYDRAULIC_MODEL_NAME)
               .Not.Nullable();

            Map(x => x.HydraulicGradientMin)
               .Not.Nullable();

            Map(x => x.HydraulicGradientMax)
               .Not.Nullable();

            Map(x => x.PressureMin)
               .Nullable();

            Map(x => x.PressureMax)
               .Nullable();

            Map(x => x.CommonName)
               .Length(PublicWaterSupplyPressureZone.StringLengths.COMMON_NAME)
               .Nullable();

            References(x => x.PublicWaterSupply)
               .Not.Nullable()
               .Cascade.None();

            // TODO: CB Optimize this? 
            References(x => x.PublicWaterSupplyFirmCapacity)
               .Formula(
                    "(SELECT pws.CurrentPublicWaterSupplyFirmCapacityId FROM PublicWaterSupplies pws WHERE pws.Id = PublicWaterSupplyId)")
               .ReadOnly();

            HasMany(x => x.Notes)
               .KeyColumn("LinkedId")
               .Inverse()
               .Cascade.None();
        }

        #endregion
    }
}
