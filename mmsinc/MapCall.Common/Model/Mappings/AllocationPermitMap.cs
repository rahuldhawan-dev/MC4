using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AllocationPermitMap : ClassMap<AllocationPermit>
    {
        #region Constructors

        public AllocationPermitMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("AllocationPermitID");

            References(x => x.PublicWaterSupply); /// ?????
            References(x => x.EnvironmentalPermit);
            References(x => x.OperatingCenter);

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.System).Length(50);
            Map(x => x.SurfaceSupply);
            Map(x => x.GroundSupply);
            Map(x => x.GeologicalFormation).Length(64);
            Map(x => x.ActivePermit);
            Map(x => x.EffectiveDateOfPermit);
            Map(x => x.RenewalApplicationDate);
            Map(x => x.ExpirationDate);
            Map(x => x.SubAllocationNumber).Length(50);
            Map(x => x.Gpd).Column("GPD").Precision(18).Scale(2);
            Map(x => x.Mgm).Column("MGM").Precision(18).Scale(2);
            Map(x => x.Mgy).Column("MGY").Precision(18).Scale(2);
            Map(x => x.PermitType).Length(50);
            Map(x => x.PermitFee).Precision(10);
            Map(x => x.SourceDescription).Length(100);
            Map(x => x.SourceRestrictions).Length(255);
            Map(x => x.PermitNotes).Column("Notes").Length(4000);
            Map(x => x.Gpm).Column("GPM").Precision(18).Scale(2);

            HasMany(x => x.AllocationPermitDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.AllocationPermitNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();

            HasManyToMany(x => x.AllocationPermitWithdrawalNodes)
               .Table("AllocationPermitsAllocationPermitWithdrawalNodes")
               .ParentKeyColumn("AllocationPermitID")
               .ChildKeyColumn("AllocationPermitWithdrawalNodeID");
        }

        #endregion
    }
}
