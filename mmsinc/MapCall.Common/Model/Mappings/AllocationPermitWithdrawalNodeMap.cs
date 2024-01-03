using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AllocationPermitWithdrawalNodeMap : ClassMap<AllocationPermitWithdrawalNode>
    {
        public AllocationPermitWithdrawalNodeMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("AllocationPermitWithdrawalNodeID");
            References(x => x.AllocationCategory);
            References(x => x.Facility);
            References(x => x.Coordinate);

            Map(x => x.WellPermitNumber).Length(25);
            Map(x => x.Description).Length(1073741823);
            Map(x => x.AllowableGpm).Column("AllowableGPM").Precision(18).Scale(2);
            Map(x => x.AllowableGpd).Column("AllowableGPD").Precision(18).Scale(2);
            Map(x => x.AllowableMgm).Column("AllowableMGM").Precision(18).Scale(2);
            Map(x => x.CapableGpm).Column("CapableGPM").Precision(18).Scale(2);
            Map(x => x.WithdrawalConstraint).Length(1073741823);
            Map(x => x.HasStandByPower);
            Map(x => x.CapacityUnderStandbyPower).Precision(18).Scale(2);

            HasManyToMany(x => x.AllocationGroupings)
               .Table("AllocationPermitsAllocationPermitWithdrawalNodes")
               .ParentKeyColumn("AllocationPermitWithdrawalNodeID")
               .ChildKeyColumn("AllocationPermitID");

            HasManyToMany(x => x.Equipment)
               .Table(nameof(AllocationPermitWithdrawalNode) + "s" + EquipmentMap.TABLE_NAME)
               .ParentKeyColumn("AllocationPermitWithdrawalNodeID")
               .ChildKeyColumn("EquipmentId");
        }
    }
}
