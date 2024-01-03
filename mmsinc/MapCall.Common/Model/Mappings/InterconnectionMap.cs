using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionMap : ClassMap<Interconnection>
    {
        #region Constructors

        public InterconnectionMap()
        {
            Id(x => x.Id, "InterconnectionId").GeneratedBy.Identity();

            References(x => x.PurchaseSellTransfer);
            References(x => x.OperatingStatus);
            References(x => x.Category);
            References(x => x.DeliveryMethod);
            References(x => x.FlowControlMethod);
            References(x => x.Direction);
            References(x => x.Type);
            References(x => x.Facility);
            References(x => x.InletPWSID);
            References(x => x.OutletPWSID);
            References(x => x.Coordinate);

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.DEPDesignation).Length(Interconnection.StringLengths.DEP_DESIGNATION);
            Map(x => x.ProgramInterestNumber).Length(Interconnection.StringLengths.PROGRAM_INTEREST_NUMBER);
            Map(x => x.PurchasedWaterAccountNumber).Length(Interconnection.StringLengths.ACCOUNT_NUMBER);
            Map(x => x.SoldWaterAccountNumber).Length(Interconnection.StringLengths.ACCOUNT_NUMBER);
            Map(x => x.DirectConnection);
            Map(x => x.InletConnectionSize).Precision(10);
            Map(x => x.OutletConnectionSize).Precision(10);
            Map(x => x.InletStaticPressure).Precision(10);
            Map(x => x.OutletStaticPressure).Precision(10);
            Map(x => x.MaximumFlowCapacity).Precision(53);
            Map(x => x.MaximumFlowCapacityStressedCondition).Precision(53);
            Map(x => x.DistributionPipingRestrictions);
            Map(x => x.WaterQuality);
            Map(x => x.FluoridatedSupplyReceivingPurveyor).Not.Nullable();
            Map(x => x.FluoridatedSupplyDeliveryPurveyor).Not.Nullable();
            Map(x => x.ChloramineResidualReceivingPurveyor).Not.Nullable();
            Map(x => x.ChloramineResidualDeliveryPurveyor).Not.Nullable();
            Map(x => x.CorrosionInhibitorReceivingPurveyor).Not.Nullable();
            Map(x => x.CorrosionInhibitorDeliveryPurveyor).Not.Nullable();
            Map(x => x.ReversibleCapacity).Precision(53);
            Map(x => x.AnnualTestRequired).Not.Nullable();
            Map(x => x.Contract);
            Map(x => x.ContractMaxSummer).Precision(53);
            Map(x => x.ContractMinSummer).Precision(53);
            Map(x => x.ContractMaxWinter).Precision(53);
            Map(x => x.ContractMinWinter).Precision(53);
            Map(x => x.ContractStartDate).Nullable();
            Map(x => x.ContractEndDate).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasManyToMany(x => x.Meters)
               .Table("InterconnectionsMeters")
               .ParentKeyColumn("InterconnectionId")
               .ChildKeyColumn("MeterId");
        }

        #endregion
    }
}
